using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using FormTimer = System.Windows.Forms.Timer;

// ==============================
// サムネイルを表示するためのユーザーコントロール
// ==============================

namespace MyPDF
{
    public partial class PdfThumbnailViewer : UserControl
    {
        // pdfiumを格納(初期化)
        private dynamic? _pdfDocument = null;

        // 複数選択のコア：選択されたページ番号を保持する（高速な検索・追加・削除が可能）
        private HashSet<int> _selectedIndices = new HashSet<int>();
        // キーボード移動や範囲選択の基準となる「アクティブ（最後に関わった）ページ」
        // 最後にクリックしたページ番号を格納
        private int _activeIndex = 0;
        // マウスホバーしているページ番号を格納
        private int _hoveredIndex = -1;

        // 最強のボトルネック対策：各ページの縦横実寸サイズを記憶しておくキャッシュ配列
        // PDFを開いた時点では要素数0（Array.Empty）で初期化
        private SizeF[] _pageSizeCache = Array.Empty<SizeF>();

        // サムネイル用紙を描画(160x160)
        private const int ThumbWidth = 160;
        private const int ThumbHeight = 160;
        private const int Spacing = 20; // サムネイル同士の上下左右の余白
        private const int BottomTextHeight = 20; // サムネイルの下に描画するページ番号のためのテキスト領域高さ

        // サムネイル1個が占有する「実質的な横幅総サイズ」(160 + 20)
        private const int ItemBlockWidth = ThumbWidth + Spacing;
        // サムネイル1個が占有する「実質的な縦幅総サイズ」(160 + 20 + 20)
        private const int ItemBlockHeight = ThumbHeight + Spacing + BottomTextHeight;
        // 現在画面がどれくらい下にスクロールされているか(ピクセル単位の移動量)
        private int _scrollY = 0;
        // 画面の右端に配置する、Windows標準の「垂直スクロールバー」
        private VScrollBar _vScrollBar;

        // 中ボタンスクロール用変数
        // ドラッグスクロールモード中かどうかを判定するフラグ
        private bool _isMiddleDragging = false;
        // マウスの中ボタンが押された画面上の初期座標（X, Y）を記録
        private Point _middleStartPos;
        // 中ボタンによる自動スクロール（オートスクロール）を、一定周期（ミリ秒単位）で実行するためのタイマーオブジェクト
        private FormTimer _autoScrollTimer;
        // 中ボタンスクロール時の「スクロールのスピード（速度と方向）」(プラスなら下へ、マイナスなら上)
        private int _scrollVelocity = 0;

        // 背景色
        //private readonly Color _backColor = Color.FromArgb(45, 45, 48);
        //private readonly Color _backColor = Color.DarkGray;
        // 選択色
        //private readonly Color _selectedColor = Color.FromArgb(0, 122, 204);
        private readonly Color _selectedColor = Color.DeepPink;
        // マウスホバー色
        private readonly Color _hoverColor = Color.FromArgb(63, 63, 65);
        //private readonly Font _textFont = new Font("Segoe UI", 9f, FontStyle.Regular);
        // ページ番号のフォント、サイズ
        private readonly Font _textFont = new Font("Yu Gothic UI", 12f, FontStyle.Regular);
        // 読み込まれたPDFの「総ページ数」を保持するプロパティ
        public int PageCount { get; private set; } = 0;

        // 複数選択に対応するため、選択されたインデックスのリストを渡すイベントに変更
        // 選択されているページが変化したとき、外側（Form1 など）にそれを一斉通知するためのイベント
        public event EventHandler<List<int>>? SelectionChanged;

        // サムネイルが右クリックされたことを外側にページ番号（1始まり）付きで通知するイベント
        // 成否フラグ(bool) と ページ番号(int) を一緒に渡せるようにする
        public event EventHandler<(bool isSuccess, int pageNumber)>? ThumbnailRightClicked;

        // 外部から現在選択されているページ一式を取得・設定するためのプロパティ
        public List<int> SelectedIndices => new List<int>(_selectedIndices);

        // ページがドラッグ＆ドロップで並び替えられたことをForm1に通知するイベント(移動：ページ入れ替え)
        // 引数: (移動元の0始まりインデックス, 移動先の0始まりインデックス)
        public event EventHandler<(int targetIdx, bool before)>? PageMoved;
        // ドラッグ開始時のインデックス(移動：ページ入れ替え)
        private int _draggedIndex = -1;

        // マウスが押された位置を一時記憶
        private Point _mouseDownPos;
        // 現在マウスが乗っているサムネイルのインデックス
        private int _dropTargetIndex = -1;
        // そのサムネイルの「前(左/上)」か「後(右/下)」か
        private bool _dropIsBefore = true;  

        public PdfThumbnailViewer()
        {
            InitializeComponent();
            // ダブルバッファリングを有効(画面ちらつき防止)
            this.DoubleBuffered = true;
            //this.BackColor = _backColor;

            // このコントロールをクリックした時にキーボードフォーカスを受け取れるようにする
            this.SetStyle(ControlStyles.Selectable, true);
            this.TabStop = true;
            // スクロールバーのインスタンスを作成
            _vScrollBar = new VScrollBar { Dock = DockStyle.Right, Enabled = false };
            // スクロールバーのつまみが動かされたときのイベント処理
            _vScrollBar.Scroll += (s, e) => { _scrollY = _vScrollBar.Value; Invalidate(); };
            // 作成した右端スクロールバーを、このユーザーコントロールの画面部品の一部としてドッキング
            this.Controls.Add(_vScrollBar);
            // オートスクロールタイマーを作成(16ミリ秒間隔:約60フレーム)
            _autoScrollTimer = new FormTimer { Interval = 16 };
            // 16ミリ秒が経過するたびにオートスクロール処理メソッド
            _autoScrollTimer.Tick += AutoScrollTimer_Tick;
            // 画面が描画されるタイミングで、メインのサムネイルレンダリング関数
            this.Paint += PdfThumbnailViewer_Paint;
            // 画面上でマウスボタンが「押された」瞬間のイベント
            this.MouseDown += PdfThumbnailViewer_MouseDown;
            // 画面上でマウスカーソルが「動いた」時のイベント
            this.MouseMove += PdfThumbnailViewer_MouseMove;
            // 画面上でマウスボタンが「離された」瞬間のイベント
            this.MouseUp += PdfThumbnailViewer_MouseUp;
            // マウスカーソルがこのコントロールの敷地外へ完全に「脱出した」時の処理
            this.MouseLeave += (s, e) => { _hoveredIndex = -1; Invalidate(); };
            // ユーザーがウィンドウのサイズをびよーんと変更（リサイズ）したときの処理
            this.Resize += (s, e) => { UpdateScrollRange(); Invalidate(); };
            // マウスのホイールをコリコリと上下に回転させたときのイベント
            this.MouseWheel += PdfThumbnailViewer_MouseWheel;
            // ドラッグ＆ドロップを許可(移動：ページ入れ替え)
            this.AllowDrop = true;
            // ドラッグ＆ドロップのイベントの紐付け(移動：ページ入れ替え)
            this.DragOver += PdfThumbnailViewer_DragOver;
            this.DragDrop += PdfThumbnailViewer_DragDrop;
        }

        // ==============================
        // ドキュメントの読み込みと廃棄
        // Form1側でPDFファイルを開いた時に呼び出され、このコントロールにPDFデータをセットする関数
        // ==============================
        public void LoadDocument(dynamic? document)
        {
            // すでに別のPDFを開いていた場合、一旦安全に古いドキュメントの初期化・廃棄処理
            CloseDocument();
            // 引数で渡された新しいPDFドキュメントを、クラス共通の変数にセット
            _pdfDocument = document;
            // PDFある？
            if (_pdfDocument != null)
            {
                // 総ページ数をセット
                PageCount = _pdfDocument.PageCount;

                // 10万ページ対応：重いループを完全に撤廃！
                // とりあえずサイズ「ゼロ」の配列をページ数分だけ爆速で確保する
                _pageSizeCache = new SizeF[PageCount];
            }
            else
            {
                // PDFない
                // 総ページ数を0に
                PageCount = 0;
                // サイズのキャッシュ配列を空に
                _pageSizeCache = Array.Empty<SizeF>();
            }
            // 基準の対象ページを先頭（0ページ目）にリセット
            _activeIndex = 0;
            // 初期値として1ページ目を選択
            _selectedIndices.Add(0);
            // スクロールの位置を一番上（0ピクセル）に強制リセット
            _scrollY = 0;
            // 右側のスクロールバーがどれくらい動けるべきかを計算
            UpdateScrollRange();
            // 最初の描画が走る
            Invalidate(); 
        }

        // ==============================
        // 表示をクリアする
        // ==============================
        public void CloseDocument()
        {
            // もし中ボタンでオートスクロール中なら、強制的にそのタイマーとドラッグ状態を停止
            StopMiddleScroll();
            // PDFが現在開かれている場合のみ、クリア処理
            if (_pdfDocument != null)
            {
                // PDFオブジェクトの参照を消去し、メモリを解放できる状態
                _pdfDocument = null;
                // 総ページ番号0に
                PageCount = 0;
                // キャッシュ配列をクリア
                _pageSizeCache = Array.Empty<SizeF>();
                // 選択されていたすべてのページ情報をリセット
                _selectedIndices.Clear();
                // 画面を更新し、それまで表示されていたサムネイルを一斉に消去
                Invalidate();
            }
        }

        // ==============================
        // レイアウト・列数の自動計算
        // 現在のコントロールの横幅に基づいて、サムネイルが横一列に「最大何個（何列）並べられるか」を自動計算する関数
        // ==============================
        private int GetColumnCount()
        {
            // 純粋にサムネイルを並べられる「有効画面幅」
            float displayWidth = this.Width - (_vScrollBar.Enabled ? _vScrollBar.Width : 0);
            // 有効画面幅の中に、サムネイル1個分のブロック幅（ItemBlockWidth = 180px）が何個入るか
            int cols = (int)Math.Floor((displayWidth - Spacing) / ItemBlockWidth);
            // 画面が極端に狭くリサイズされた場合でも、列数が0やマイナスにならないよう、最低でも必ず「1列」は保証
            return Math.Max(1, cols);
        }

        // ==============================
        // 垂直スクロールバーの最大可動範囲（スクロール可能量）を算出・設定する関数
        // ==============================
        private void UpdateScrollRange()
        {
            // PDFがないなら戻る
            if (_pdfDocument == null) return;
            // レイアウト・列数の自動計算関数を呼び出し、現在の最新の横列数を取得
            int cols = GetColumnCount();
            // 総ページ数を列数で割り算(例：11ページ÷3列＝3.66 → 切り上げて 4行）
            int rows = (int)Math.Ceiling((double)PageCount / cols);

            // 全アイテムが標準サイズであると仮定して全体の高さを一瞬で計算
            int totalHeight = Spacing + rows * ItemBlockHeight;
            // 全体の高さから、今目に見えているコントロール自身の高さを引き算し、
            // 「隠れていてスクロールして動かせる最大ピクセル量」を出す
            int maxV = totalHeight - this.Height;

            // 隠れている部分がある（画面に収まりきらずスクロールが必要な）場合の処理
            if (maxV > 0)
            {
                // 右端のスクロールバーを活性化して操作可能
                _vScrollBar.Enabled = true;
                // Windows標準の仕様（つまみの大きさ分だけMaximumが手前で止まる挙動）を補正するため、
                // ページの最大値につまみの大きさ（LargeChange）を足してジャストの限界値をセット
                _vScrollBar.Maximum = maxV + _vScrollBar.LargeChange;
                // 現在のスクロール位置（_scrollY）が、リサイズによって最大範囲を超えてしまったり、
                // マイナスになったりしないように、正しい範囲（0〜最大値）の間にギュッと丸める
                _scrollY = Math.Max(0, Math.Min(_scrollY, maxV));
                // 調整が終わった正しいスクロール値を、右端のスクロールバーのつまみの位置に反映
                _vScrollBar.Value = _scrollY;
            }
            else
            {
                // 全ページが画面内にすっぽり収まっていて、スクロールする必要がない場合の処理
                // 右端のスクロールバーをグレーアウトして無効化
                _vScrollBar.Enabled = false;
                // スクロール位置は完全にトップ（0）に固定
                _scrollY = 0;
            }
        }

        // ==============================
        // メイン描画処理（Paintイベント）
        // 画面を実際にレンダリングする最も重要な関数
        // ==============================
        private void PdfThumbnailViewer_Paint(object? sender, PaintEventArgs e)
        {
            // PDFがないなら戻る
            if (_pdfDocument == null) return;
            // 画面に絵や文字を描くためのキャンバス兼筆となる Graphics オブジェクトを取得
            Graphics g = e.Graphics;
            // 画像を縮小描画するときの補間アルゴリズムの画質をあえて「Low（低解像度重視）」に落とす
            g.InterpolationMode = InterpolationMode.Low;
            // ページ番号の文字のアンチエイリアス（ClearType）を有効
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 現在配置すべきグリッドの列数を取得
            int cols = GetColumnCount();
            // スクロールバーを除いた有効な画面の横幅を計算
            float displayWidth = this.Width - (_vScrollBar.Enabled ? _vScrollBar.Width : 0);
            // センタリング計算
            int startX = (int)(displayWidth - (cols * ItemBlockWidth - Spacing)) / 2;

            // スクロール位置から、現在画面に見えているべき「開始行」と「終了行」を逆算（さらに高速化）
            // 開始行
            int startRow = Math.Max(0, _scrollY / ItemBlockHeight);
            // 終了行
            int endRow = Math.Min((int)Math.Ceiling((double)PageCount / cols), (_scrollY + this.Height) / ItemBlockHeight + 1);
            // 開始行と列数を掛け算し、画面に映る最初の「ページインデックス（何ページ目か）」を特定
            int startIndex = startRow * cols;
            // 終了行と列数を掛け算し、画面に映る最後の「ページインデックス」を特定
            int endIndex = Math.Min(PageCount, endRow * cols);

            // 10万ページ対応：全ページをループせず、見えているインデックス（数十個）だけピンポイントで回す！
            for (int i = startIndex; i < endIndex; i++)
            {
                // 現在処理しているページインデックス i が、上から数えて「何行目」に属するかを割り算
                int row = i / cols;
                // 現在処理しているページインデックス i が、左から数えて「何列目」に属するかを余り算
                int col = i % cols;
                // 左端の開始位置に、列番号×ブロック幅を足し算して、このサムネイルの「正確な表示X座標」を求める
                int itemLeftX = startX + col * ItemBlockWidth;
                // 縦の基準位置に、行番号×ブロック高さを掛け、そこから現在のスクロール量（_scrollY）を引き算することで、
                // スクロールに連動して上下にスライドする「正確な表示Y座標」
                int itemTopY = Spacing + row * ItemBlockHeight - _scrollY;

                // 念のための安全ガード（基本は通る）
                if (itemTopY + ItemBlockHeight < 0 || itemTopY > this.Height) continue;

                // 完全オンデマンド：今まさに画面に映ったこの瞬間、キャッシュが空なら実寸を1回だけ見に行く
                if (_pageSizeCache[i] == SizeF.Empty)
                {
                    try
                    {
                        // PDFオブジェクトのサイズ管理プロパティにアクセス
                        var pageSizes = _pdfDocument.PageSizes;
                        // pdfiumから i ページ目の「本物の用紙寸法（ポイント単位の横幅と縦幅）」を1回だけ抜き取り
                        SizeF size = pageSizes[i];

                        // 万が一、Pdfiumから取得したサイズが0だった場合の安全ガード
                        if (size.Width <= 0 || size.Height <= 0)
                        {
                            // 標準A4ポイント
                            // 国際標準的なA4縦サイズ（595×842ポイント）
                            size = new SizeF(595, 842); 
                        }
                        // ここで本物のサイズ（A4横やA3など）をキャッシュ！
                        _pageSizeCache[i] = size; 
                    }
                    catch
                    {
                        // エラー時のフォールバック
                        // 国際標準的なA4縦サイズ（595×842ポイント）
                        _pageSizeCache[i] = new SizeF(595, 842); 
                    }
                }

                // サムネイルの縦横比を、取得した実寸キャッシュに合わせて綺麗にフィッティングさせる
                SizeF realSize = _pageSizeCache[i];

                // 超混在対応ロジック：A4縦の高さ（約842ポイント）を「基準の100%」としてスケールを測る
                // これにより、A3（1191×842）が来た時に、A4よりも物理的に巨大であることが計算で導き出されます
                //float baseScale = 842f;
                // 初期値として160×160を入れる
                float currentThumbWidth = ThumbWidth;
                float currentThumbHeight = ThumbHeight;
                // 縦横のサイズが正常にあることを確認して、実寸比率の計算を開始
                if (realSize.Width > 0 && realSize.Height > 0)
                {
                    // 小さいものは拡大、大きいものは縮小し、160x160の枠にぴったり合わせるロジック
                    // 横幅基準のスケールと、縦幅基準のスケールをそれぞれ計算
                    float scaleX = (float)ThumbWidth / realSize.Width;
                    float scaleY = (float)ThumbHeight / realSize.Height;

                    // 縦横比（アスペクト比）を維持するため、より厳しい方（小さい方の倍率）を採用する
                    // これにより、小さいPDFは160枠まで均等に「拡大」され、大きいPDFは「縮小」されます
                    float finalScale = Math.Min(scaleX, scaleY);

                    currentThumbWidth = realSize.Width * finalScale;
                    currentThumbHeight = realSize.Height * finalScale;

                    /*
                    // A4縦の標準的なサイズ（160px）を1.0としたときの、このページのドット換算サイズ
                    // 160pxの枠に対して、どれくらいの比率で描画すべきかを計算
                    float factor = (float)ThumbWidth / baseScale;

                    // 物理実寸通りのピクセルサイズを一旦計算
                    currentThumbWidth = realSize.Width * factor;
                    currentThumbHeight = realSize.Height * factor;
                    */


                    // 万が一、超巨大な図面などで160pxの最大枠をはみ出してしまう場合の安全ガード
                    if (currentThumbWidth > ThumbWidth || currentThumbHeight > ThumbHeight)
                    {
                        // 限界枠をどれくらいオーバーしているかの超過比率（縦と横で厳しい方）を計算
                        float overRatio = Math.Min((float)ThumbWidth / currentThumbWidth, (float)ThumbHeight / currentThumbHeight);
                        // 縦横に超過比率をかけて、160pxの枠内に収まるように縮小
                        currentThumbWidth *= overRatio;
                        currentThumbHeight *= overRatio;
                    }
                }

                // 160×160の領域（ブロック）の中で、用紙が中央に綺麗に収まるように配置座標（X, Y）を計算
                int imgX = itemLeftX + (ThumbWidth - (int)currentThumbWidth) / 2;
                int imgY = itemTopY + (ThumbHeight - (int)currentThumbHeight) / 2;
                // 最終的にPDFの用紙を型どって描画するための、ジャストな長方形（座標とサイズ）オブジェクトを生成
                Rectangle thumbRect = new Rectangle(imgX, imgY, (int)currentThumbWidth, (int)currentThumbHeight);

                // --- 背景・ホバー・選択のUI描画 ---
                // 枠線や座布団は、用紙のサイズ（thumbRect）ではなく、外側の共通ブロック（ThumbWidth/Height）を基準にするとVS風でナウくなる
                // 複数選択対応：HashSetの中に自分が含まれているかをチェック
                if (_selectedIndices.Contains(i))
                {
                    // 選択されたアイテム全体の背景を塗り潰す
                    // アルファ値「50」を指定して、半透明に
                    using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(50, _selectedColor)))
                    {
                        g.FillRectangle(bgBrush, itemLeftX - 6, itemTopY - 6, ThumbWidth + 12, ThumbHeight + 12);
                    }

                    // 選択枠：太めのモダンなアクセント線
                    using (Pen p = new Pen(_selectedColor, 2))
                    {
                        g.DrawRectangle(p, itemLeftX - 4, itemTopY - 4, ThumbWidth + 8, ThumbHeight + 8);
                    }
                }
                // マウスホバー
                else if (i == _hoveredIndex)
                {
                    // マウスホバーの色を選択範囲の塗り潰し
                    using (SolidBrush b = new SolidBrush(_hoverColor))
                    {
                        g.FillRectangle(b, itemLeftX - 6, itemTopY - 6, ThumbWidth + 12, ThumbHeight + 12);
                    }
                }

                // 用紙の白い土台を描画
                g.FillRectangle(Brushes.White, thumbRect);

                // レンダリング（計算されたジャストな可変サイズでPdfiumに描画させる）
                using (Image pageImage = _pdfDocument.Render(i, (int)currentThumbWidth, (int)currentThumbHeight, 24, 24, false))
                {
                    // ページ空か？
                    if (pageImage != null)
                    {
                        // 縮小した白い土台に貼る
                        g.DrawImage(pageImage, thumbRect);
                    }
                }

                // ページ番号の描画(0始まりなんで、+1する)
                string pageText = (i + 1).ToString();
                // 指定したフォント（Yu Gothic UI, 12pt）でその数字を描いたときに、画面上で「縦横何ピクセルの大きさになるか」を事前にシミュレーション（計測）
                Size textSize = TextRenderer.MeasureText(pageText, _textFont);
                // 計測した文字の横幅からページ番号がド真ん中になるようにする
                int textX = itemLeftX + (ThumbWidth - textSize.Width) / 2;
                // 常に160pxの枠の下に綺麗に並ぶ
                int textY = itemTopY + ThumbHeight + 6; 
                // ページ番号を描画
                //TextRenderer.DrawText(g, pageText, _textFont, new Point(textX, textY), Color.White);
                TextRenderer.DrawText(g, pageText, _textFont, new Point(textX, textY), Color.Black);
            }

            // ドラッグ＆ドロップ用の挿入インジケータ（縦線）を描画
            if (_dropTargetIndex != -1)
            {
                int row = _dropTargetIndex / cols;
                int col = _dropTargetIndex % cols;

                // 既存の座標計算と完全に同じロジックでターゲットの位置を特定
                int itemLeftX = startX + col * ItemBlockWidth;
                int itemTopY = Spacing + row * ItemBlockHeight - _scrollY;

                // 線のX座標を決定（前ならブロックの左端、後ならブロックの右端）
                // 選択座布団のサイズ（itemLeftX - 6 から +12）に綺麗に重なるように調整しています
                int lineX = _dropIsBefore ? (itemLeftX - 6) : (itemLeftX + ThumbWidth + 6);

                // 線の上下Y座標（座布団の高さとページ番号の隙間に合わせる）
                int topY = itemTopY - 6;
                int bottomY = itemTopY + ThumbHeight + 6;

                // 太さ3ピクセルのRoyalBlue（好みに合わせてColor.Orange等でもOK）で線を描画
                //using (Pen insertPen = new Pen(Color.RoyalBlue, 3))
                using (Pen insertPen = new Pen(Color.Red, 3))
                {
                    // メインの縦線
                    g.DrawLine(insertPen, lineX, topY, lineX, bottomY);

                    // 視認性を劇的に上げるため、上下に小さな「丁字（ピン）」の横線をトッピング
                    g.DrawLine(insertPen, lineX - 4, topY, lineX + 4, topY);
                    g.DrawLine(insertPen, lineX - 4, bottomY, lineX + 4, bottomY);
                }
            }


            // 中ボタンドラッグスクロールモード中かどうかを判定
            if (_isMiddleDragging)
            {
                // 最初に中ボタンをクリックした原点位置（_middleStartPos）を中心に、直径12pxの「薄いグレーの円（拠点）」を描画
                g.FillEllipse(Brushes.LightGray, _middleStartPos.X - 6, _middleStartPos.Y - 6, 12, 12);
                // そのグレーの円の周りを黒い極細線で縁取りし、WindowsのWebブラウザでおなじみのスクロール拠点のマークを描画
                g.DrawEllipse(Pens.Black, _middleStartPos.X - 6, _middleStartPos.Y - 6, 12, 12);
            }

        }

        // ==============================
        // マウス位置検知（ヒットテスト）
        // 「何ページ目のサムネイルの上か」を数式のみで一発特定して番号を返す、非常に重要な判定関数
        // ==============================
        private int HitTest(Point mouseLocation)
        {
            // PDFがないなら戻る
            if (_pdfDocument == null) return -1;
            // 現在のレイアウトの横列数を取得
            int cols = GetColumnCount();
            // スクロールバーを除いた有効な画面幅
            float displayWidth = this.Width - (_vScrollBar.Enabled ? _vScrollBar.Width : 0);
            // サムネイルの並びの左端の開始X座標（真ん中寄せのオフセット）を算出
            int startX = (int)(displayWidth - (cols * ItemBlockWidth - Spacing)) / 2;

            // マウスの位置からインデックスを直接逆算（10万回ループを回さない超高速化）
            // マウスが左から数えて「何列目のエリアを指しているか」を数式一撃で導き出す
            int col = (mouseLocation.X - startX) / ItemBlockWidth;
            // 上から数えて「何行目のエリアを指しているか」を一撃で導き出す
            int row = (mouseLocation.Y + _scrollY - Spacing) / ItemBlockHeight;

            // 計算された列や行が、グリッドの範囲外（左すぎる、右すぎる、上すぎるなど）を指していた場合は、該当なし（-1）を返す
            if (col < 0 || col >= cols || row < 0) return -1;
            // 「行番号×横の列数＋列番号」という2次元配列を1次元に直す基本数式
            // 「通算ページインデックス」を割り出す
            int index = row * cols + col;
            // 割り出されたインデックスが、PDFの実際の総ページ数の内側に収まっているかを確認
            if (index >= 0 && index < PageCount)
            {
                // 正確な左端のX座標を逆算
                int itemLeftX = startX + col * ItemBlockWidth;
                // 正確な上端のY座標をスクロール量を加味して逆算
                int itemTopY = Spacing + row * ItemBlockHeight - _scrollY;
                // 周囲の余白（ホバー座布団のサイズ）を含めた、このページの「有効クリック判定エリア」の四角形を作成
                Rectangle clickArea = new Rectangle(itemLeftX - 6, itemTopY - 6, ThumbWidth + 12, ThumbHeight + 12);
                // 座標内ならページ番号を返す
                if (clickArea.Contains(mouseLocation)) return index;
            }
            // 範囲外なら-1を返す
            return -1;
        }

        // ==============================
        // 画面上でマウスカーソルが「動いた」時のイベント
        // ==============================
        private void PdfThumbnailViewer_MouseMove(object? sender, MouseEventArgs e)
        {
            if (_pdfDocument == null) return;

            // // 中ボタンドラッグスクロールモード中かどうかを判定
            if (_isMiddleDragging)
            {
                // 中ボタンクリック位置から、現在のマウスのY座標が上下に「何ピクセル離れたか（距離）」を計算
                int deltaY = e.Y - _middleStartPos.Y;
                // 離れた距離が10pxを超えていれば、距離に応じた移動スピード（deltaY / 5）を速度にセット
                _scrollVelocity = Math.Abs(deltaY) > 10 ? deltaY / 5 : 0;
                return;
            }

            // 左ボタンが押されている、かつ有効なサムネイルの上から始まっている場合
            if (e.Button == MouseButtons.Left && _draggedIndex != -1)
            {
                // Windows標準の「これ以上動いたらドラッグとみなす」距離（通常4ピクセル程度）を超えたかチェック
                if (Math.Abs(e.X - _mouseDownPos.X) >= SystemInformation.DragSize.Width ||
                    Math.Abs(e.Y - _mouseDownPos.Y) >= SystemInformation.DragSize.Height)
                {
                    // ドラッグ操作を正式に開始！（複数選択の状態が Form1.GetSelectedPagesText() で維持されます）
                    this.DoDragDrop(_draggedIndex, DragDropEffects.Move);

                    // ドラッグが終わるまでここにブロッキングされるので、終わったらリセット
                    _draggedIndex = -1;
                    return;
                }
            }

                // 通常移動時は、マウスの現在地を先ほどの HitTest 関数に放り込み、どのページの上にマウスがいるかを調べる
                int newHover = HitTest(e.Location);
            // 前回マウスが乗っていたページから、新しく別のページへと移動した場合（変更があったとき）のみ、中に入る
            if (newHover != _hoveredIndex)
            {
                // 最新のマウスホバー対象のページ番号を上書き記憶
                _hoveredIndex = newHover;
                // ホバーのグレー枠（座布団）を古いページから消して新しいページに描き直すため、画面の更新を要求
                Invalidate();
            }
        }

        // ==============================
        // 画面上でマウスボタンが「押された」瞬間のイベント
        // ==============================
        private void PdfThumbnailViewer_MouseDown(object? sender, MouseEventArgs e)
        {
            // PDFがないなら戻る
            if (_pdfDocument == null) return;

            // クリックされたらキーボードフォーカスをこのコントロールに持ってくる
            this.Focus();
            // 押されたボタンが「マウスホイール（中ボタン）」だった場合の、ドラッグ開始処理
            if (e.Button == MouseButtons.Middle)
            {
                // 中ボタンスクロールモードを「ON（true）」に
                _isMiddleDragging = true;
                // 押し下げられたその瞬間のマウス座標を、スクロールの基準原点として記憶
                _middleStartPos = e.Location;
                // 開始したばかりなので、最初はスクロール速度を0（静止）にしておく
                _scrollVelocity = 0;
                // マウスのカーソル形状を、上下矢印がついたWindows標準のスクロール専用アイコン（NoMoveVert）に変化
                this.Cursor = Cursors.NoMoveVert;
                // 16ミリ秒ごとに画面を勝手にスライドさせるオートスクロールタイマーを起動
                _autoScrollTimer.Start();
                // 画面にスクロール原点のグレーの丸マークを描画させるためにPaintを呼び出す
                Invalidate();
                return;
            }

            // 押されたボタンが通常のマウス「左クリック」だった場合の処理
            if (e.Button == MouseButtons.Left)
            {
                // マウスが押された位置を記憶（MouseMoveでのドラッグ開始判定用）
                _mouseDownPos = e.Location;

                // クリックされた座標を HitTest に渡し、何ページ目がクリックされたのか番号を取得
                int clickIdx = HitTest(e.Location);
                // ページがクリックされていた場合の処理
                if (clickIdx != -1)
                {
                    _draggedIndex = clickIdx;

                    // すでに選択されているページを通常クリックした場合は、
                    // MouseDownの時点では選択を上書きせず、MouseMove（ドラッグするかどうか）に判断を委ねる。
                    if (_selectedIndices.Contains(clickIdx) && ModifierKeys == Keys.None)
                    {
                        // 選択状態をキープ（何もしない）
                    }
                    else
                    {
                        // 未選択のページ、またはCtrl/Shift押しなら即座に選択変更
                        HandleSelection(clickIdx, ModifierKeys);
                    }

                }
            }
        }

        // ==============================
        // マウスやキーボードからの選択要求を統合処理するコアロジック
        // ==============================
        private void HandleSelection(int targetIndex, Keys modifiers)
        {
            // Shift が長押しされているかどうかを true/false で判定
            bool isShift = (modifiers & Keys.Shift) == Keys.Shift;
            // Ctrl キーが長押しされているかどうかを true/false で判定
            bool isCtrl = (modifiers & Keys.Control) == Keys.Control;
            // Shift範囲選択の挙動
            // Shift を押しながら新しいターゲットが指定された場合、中に入る
            if (isShift && _selectedIndices.Count > 0)
            {
                // Shift選択：前回のアクティブ位置からターゲットまでの「範囲」をすべて選択
                _selectedIndices.Clear();
                // 前回最後に関わったページ番号（_activeIndex）と、今回クリックされたページ番号を比較し、数値の「小さい方」を開始ページに指定
                int start = Math.Min(_activeIndex, targetIndex);
                // 同様に、2つのページ番号を比較し、数値の「大きい方」を終了ページに指定
                int end = Math.Max(_activeIndex, targetIndex);
                // 開始ページから終了ページまで、間にあるページ番号を1つずつすべて順番に網羅するループを回す
                for (int i = start; i <= end; i++)
                {
                    // 範囲内にあるすべてのページ番号を、選択リスト（_selectedIndices）に一斉に放り込む
                    // 範囲選択が完成
                    _selectedIndices.Add(i);
                }
            }
            // Ctrl飛び地選択の挙動
            // Ctrl を押しながら新しいターゲットが指定された場合、中に入る
            else if (isCtrl)
            {
                // Ctrl選択：クリックしたページの選択状態を反転（トグル）
                if (_selectedIndices.Contains(targetIndex))
                    // 選択を解除するために、リストからそのページ番号を消去
                    _selectedIndices.Remove(targetIndex);
                else
                    // 追加選択するために、リストにそのページ番号を新しく登録
                    _selectedIndices.Add(targetIndex);
            }
            // 通常選択の挙動
            // Shift も Ctrl も押されていない
            else
            {
                // 通常選択：これまでの選択を全クリアして、対象だけを選択
                _selectedIndices.Clear();
                // 今回新しく選んだその1ページだけを、リストに登録
                _selectedIndices.Add(targetIndex);
            }

            // 最後に関わったページをアクティブターゲットに記憶
            _activeIndex = targetIndex;
            // 選択状態が変わり、枠線や半透明の座布団エフェクトを描き直す必要があるため、Paintイベントを即座に発行
            Invalidate();

            // イベント発火（選択が変わったことを外に伝える）
            SelectionChanged?.Invoke(this, SelectedIndices);
        }

        // ==============================
        // プログラムから選択状態を完全にクリアするメソッド
        // ==============================
        public void ClearSelectionWithoutEvent()
        {
            // 選択リストを空にする
            _selectedIndices.Clear();

            // アクティブ位置も初期化（必要であれば）
            _activeIndex = -1;

            // 画面の選択枠を消すために再描画
            Invalidate();
        }

        // ==============================
        // プログラムから純粋に複数選択を追加するためのメソッド
        // ==============================
        public void AddSelectionWithoutClearing(int targetIndex)
        {
            // インデックスが範囲外なら処理しない（安全ガード）
            if (targetIndex < 0) return;

            // すでに選択リストに含まれていない場合のみ追加（重複防止）
            if (!_selectedIndices.Contains(targetIndex))
            {
                _selectedIndices.Add(targetIndex);
            }

            // 最後に関わったページをアクティブターゲットに記憶
            _activeIndex = targetIndex;

            // 再描画を要求
            Invalidate();

            // ※ここでは毎回イベント（SelectionChanged）を飛ばすと重くなるため、
            // 呼び出し側のループが終わった後に外側で一発飛ばすか、必要に応じてInvokeしてください。
            SelectionChanged?.Invoke(this, SelectedIndices);
        }

        // ==============================
        // 中ボタンスクロールの終了とタイマー駆動
        // ==============================
        private void PdfThumbnailViewer_MouseUp(object? sender, MouseEventArgs e)
        {
            if (_pdfDocument == null) return;

            // 離されたボタンが「中ボタン」であれば、オートスクロールを終了させる関数
            if (e.Button == MouseButtons.Middle)
            {
                StopMiddleScroll();
            }

            // 右クリックの処理
            if (e.Button == MouseButtons.Right)
            {

                // マウスが離された座標にあるサムネイルのインデックス（0始まり）を取得
                int clickIdx = HitTest(e.Location);

                // 有効なページ（0〜総ページ数）の上で右クリックされていた場合
                if (clickIdx != -1)
                {
                    // サムネイルの上で右クリックされた場合（成功：true）
                    ThumbnailRightClicked?.Invoke(this, (true, clickIdx + 1));
                }
                else
                {
                    // 余白などで右クリックされた場合（失敗：false、ページは0にしておく）
                    ThumbnailRightClicked?.Invoke(this, (false, 0));
                }
            }

            // 左クリックの処理
            if (e.Button == MouseButtons.Left)
            {
                int clickIdx = HitTest(e.Location);

                // ドラッグされずにその場でマウスが離され、かつMouseDown時と同じページの上であれば
                // 留保していた「そのページだけの単一選択」をここで確定させる
                if (clickIdx != -1 && clickIdx == _draggedIndex && ModifierKeys == Keys.None)
                {
                    HandleSelection(clickIdx, ModifierKeys);
                }

                // リセット
                _draggedIndex = -1;
            }
        }

        // ==============================
        // 中ボタンによるオートスクロール状態を安全に停止・リセットする関数
        // ==============================
        private void StopMiddleScroll()
        {
            // 中ボタンドラッグスクロールモード中かどうかを判定
            if (_isMiddleDragging)
            {
                // モードフラグを「OFF（false）」に
                _isMiddleDragging = false;
                // 16ミリ秒ごとに動いていたスライド用のタイマーを停止
                _autoScrollTimer.Stop();
                // マウスのカーソル形状を、元の標準的な矢印
                this.Cursor = Cursors.Default;
                Invalidate();
            }
        }

        // ==============================
        // 中ボタンスクロール中に、16ミリ秒（タイマーの間隔）が経過するたびに自動で何度も強制実行される、画面スライドの心臓部
        // ==============================
        private void AutoScrollTimer_Tick(object? sender, EventArgs e)
        {
            // 「中ボタンが押されていない」「スクロール速度が0（静止中）」「ページが少なくてスクロールバー自体が無効」のいずれかに該当する場合は戻る
            if (!_isMiddleDragging || _scrollVelocity == 0 || !_vScrollBar.Enabled) return;
            // スクロールバーが動くことができる、本物の最大限界Yピクセル座標を逆算
            int maxV = _vScrollBar.Maximum - _vScrollBar.LargeChange;
            // 現在のスクロール位置（_scrollY）に対して、マウスの引っ張り距離から求めた速度（_scrollVelocity）を足し算し、画面を上、または下へ強制スライド
            _scrollY += _scrollVelocity;
            // スライドした結果、画面の一番上（0）を突き抜けて上に飛び出してしまった場合は、0でガチッと食い止る
            if (_scrollY < 0) _scrollY = 0;
            // スライドした結果、画面の一番下（maxV）を突き抜けて下に飛び出してしまった場合は、最大値の位置でガチッと食い止る
            if (_scrollY > maxV) _scrollY = maxV;
            // 計算し終わった最新のスクロール位置を、右側のスクロールバーのつまみの位置に連動
            _vScrollBar.Value = _scrollY;
            // 再描画
            Invalidate();
        }

        // ==============================
        // マウスホイールと外部制御
        // ==============================
        private void PdfThumbnailViewer_MouseWheel(object? sender, MouseEventArgs e)
        {
            // スクロールする必要がないほどページ数が少なければ、ホイールを回されても無視して終了
            if (!_vScrollBar.Enabled) return;
            // スクロールバーの最大可動ピクセル制限を取得
            int maxV = _vScrollBar.Maximum - _vScrollBar.LargeChange;
            // ホイールの回転方向と量(上に回すとプラス120、下に回すとマイナス120）
            _scrollY -= e.Delta;
            // ホイールを上に回しすぎて画面最上部を突破しないように0でロック
            if (_scrollY < 0) _scrollY = 0;
            // ホイールを下に回しすぎて画面最下部を突破しないように最大値でロック
            if (_scrollY > maxV) _scrollY = maxV;
            // 確定したスクロール値をつまみの位置にフィードバック
            _vScrollBar.Value = _scrollY;
            // 再描画
            Invalidate();
        }

        // ==============================
        // 外部（Form1やメインビューア側など）から「強制的にサムネイルの指定ページを選択状態にしてくれ！」と命令されたときに呼び出される
        // 連動用の公開メソッド
        // ==============================
        public void SetSelection(int index)
        {
            // 命令されたページ番号が、存在する正しい範囲内かチェック
            if (index >= 0 && index < PageCount)
            {
                // これまでの古い複数選択状態をすべて一旦クリア
                _selectedIndices.Clear();
                // 外部から指示されたその1ページだけを選択状態（HashSet）に格納
                _selectedIndices.Add(index);
                // キーボード移動などの基準点となるアクティブ番号も、この指定ページに同期
                _activeIndex = index;
                // 外部から選択された時にスクロールも自動追従
                EnsureVisible(index);
                // 再描画
                Invalidate();
            }
        }

        // ==============================
        // 矢印キーを別のコントロールへのフォーカス移動に使わせず、このコントロール内で横取りする
        // キーボード入力の横取り（オーバーライド）
        // ==============================
        protected override bool IsInputKey(Keys keyData)
        {
            // 押されたキーの種類（修飾キーを除いた純粋なキーコード）で分岐判定
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Up: // 上
                case Keys.Down: // 下
                case Keys.Left: // 左
                case Keys.Right: // 右
                    return true;
            }
            // それ以外
            return base.IsInputKey(keyData);
        }

        // ==============================
        // キーボードの矢印キーが押された時の移動ロジック
        // ==============================
        protected override void OnKeyDown(KeyEventArgs e)
        {
            // PDFが何も開かれていなければ、矢印キーを動かす意味がないので何もせず戻る
            if (_pdfDocument == null || PageCount == 0) return;
            // 現在のレイアウトが「横に最大何列並んでいるか」を取得
            int cols = GetColumnCount();
            // これから移動する予定の、新しい目標ページ番号用の変数
            // 現在の基準地（_activeIndex）をコピーしてスタート
            int newIndex = _activeIndex;
            // 押された矢印キーの方向（e.KeyCode）に合わせた分岐処理
            switch (e.KeyCode)
            {
                // 「左矢印」が押されたなら、現在のページ番号からマイナス1
                case Keys.Left: newIndex = _activeIndex - 1; break;
                // 「右矢印」が押されたなら、現在のページ番号にプラス1
                case Keys.Right: newIndex = _activeIndex + 1; break;
                // 「上矢印」が押されたなら、現在のページ番号から横の列数（cols）を一気に引き算
                case Keys.Up: newIndex = _activeIndex - cols; break;
                // 「下矢印」が押されたなら、現在のページ番号に横の列数（cols）を一気に足し算
                case Keys.Down: newIndex = _activeIndex + cols; break;
                // 万が一それ以外の関係ないキーがこの関数に入り込んできた場合は、標準のキー処理を実行してそのまま終了
                default:
                    base.OnKeyDown(e);
                    return;
            }
            // 計算された移動先のページ番号（newIndex）が、最初のページ（0）より前だったりはみ出したりしていない、
            // 実在する正しいページ番号の範囲内であることをチェック
            if (newIndex >= 0 && newIndex < PageCount)
            {
                // 矢印キー移動時も、Shiftキーの状態（e.Modifiers）をそのまま引き渡す
                HandleSelection(newIndex, e.Modifiers);
                // 選択されたサムネイルが自動でスクロールして追いかけてくるように、視覚追従関数（EnsureVisible）を呼び出す
                EnsureVisible(_activeIndex);
            }
            // 挙動をここでクローズ
            e.Handled = true;
        }

        // ==============================
        // 選択されたページが画面外に隠れていたら、見える位置まで自動スクロールする
        // ==============================
        private void EnsureVisible(int index)
        {
            if (PageCount == 0) return;

            // 現在のレイアウトの横列数を調べる
            int cols = GetColumnCount();
            // 対象のページが、上から数えて「何行目」に位置するか
            int row = index / cols;
            // スクロールを引かない絶対Y座標
            int itemTopY = Spacing + row * ItemBlockHeight;

            // --- 【ここから中央寄せの計算に修正】 ---

            // 1. 対象の「行」の真ん中（中心点）のY座標を計算
            int itemCenterY = itemTopY + (ItemBlockHeight / 2);

            // 2. このコントロール自体の画面の真ん中のY座標（高さの半分）
            int halfClientHeight = this.Height / 2;

            // 3. 対象の行が画面のド真ん中に来るような「理想のスクロール位置」を計算
            int idealScrollY = itemCenterY - halfClientHeight;

            // 4. スクロールバーが移動できる絶対的なMAXリミット値を計算
            // （LargeChangeの引き算は、環境によって最大値を超えないための安全ガードです）
            int maxV = Math.Max(0, _vScrollBar.Maximum - _vScrollBar.LargeChange);

            // 5. 理想のスクロール位置を、0 ～ MAXリミット値の範囲内に安全に収める
            _scrollY = Math.Max(0, Math.Min(idealScrollY, maxV));

            // 調整された最終的なスクロール値を、右端のスクロールバーのつまみの位置に反映
            _vScrollBar.Value = _scrollY;

            Invalidate();

        }

        // ==============================
        // ドラッグ中（移動中）
        // ドラッグ中のマウスカーソルの変化と、手を離した（ドロップした）瞬間の並び替え処理
        // ==============================
        private void PdfThumbnailViewer_DragOver(object? sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(typeof(int)))
            {
                e.Effect = DragDropEffects.Move;

                // マウス位置の座標を取得
                Point clientPoint = this.PointToClient(new Point(e.X, e.Y));

                // ドラッグ中の自動スクロール処理
                // スクロールバーが有効なときだけ処理
                if (_vScrollBar.Enabled) 
                {
                    // 画面の上端・下端から「何ピクセル以内」に入ったらスクロールさせるか
                    int scrollZone = 100;
                    // 1回のリロードで進むスクロール量（ピクセル）
                    int scrollSpeed = 30; 

                    // 【上端付近にいる場合】上にスクロール
                    if (clientPoint.Y < scrollZone)
                    {
                        int newScrollY = Math.Max(0, _scrollY - scrollSpeed);
                        if (newScrollY != _scrollY)
                        {
                            _scrollY = newScrollY;
                            // スクロールバーの位置も同期
                            _vScrollBar.Value = _scrollY;
                            // 画面再描画
                            this.Invalidate(); 
                        }
                    }
                    // 【下端付近にいる場合】下にスクロール
                    else if (clientPoint.Y > this.Height - scrollZone)
                    {
                        int newScrollY = Math.Min(_vScrollBar.Maximum - _vScrollBar.LargeChange + 1, _scrollY + scrollSpeed);
                        if (newScrollY != _scrollY)
                        {
                            _scrollY = newScrollY;
                            _vScrollBar.Value = _scrollY;
                            this.Invalidate();
                        }
                    }
                }

                int hoverIdx = HitTest(clientPoint);

                if (hoverIdx != -1)
                {
                    // カラム数（横並び数）を取得
                    int cols = GetColumnCount();
                    int col = hoverIdx % cols;
                    int row = hoverIdx / cols;

                    // 有効クリックエリアの計算と同じロジックで、そのサムネイルの正確な中心座標を割り出す
                    int displayWidth = this.Width - (_vScrollBar.Enabled ? _vScrollBar.Width : 0);
                    int startX = (int)(displayWidth - (cols * ItemBlockWidth - Spacing)) / 2;
                    int itemLeftX = startX + col * ItemBlockWidth;
                    int itemTopY = Spacing + row * ItemBlockHeight - _scrollY;

                    // サムネイルの中心X座標（複数列グリッド配置を想定）
                    int itemCenterX = itemLeftX + ThumbWidth / 2;

                    // マウスがサムネイルの中心より「左」にあれば「前」、「右」にあれば「後」
                    bool isBefore = clientPoint.X < itemCenterX;

                    // 値が変わったときだけ画面を更新（チラつき防止）
                    if (_dropTargetIndex != hoverIdx || _dropIsBefore != isBefore)
                    {
                        _dropTargetIndex = hoverIdx;
                        _dropIsBefore = isBefore;
                        this.Invalidate(); // OnPaintを強制的に走らせて線を描画
                    }
                }
                else
                {
                    // サムネイルの無い虚無の空間にいる場合
                    if (_dropTargetIndex != -1)
                    {
                        _dropTargetIndex = -1;
                        this.Invalidate();
                    }
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }

        }

        // ==============================
        // ドロップ
        // ドラッグ中のマウスカーソルの変化と、手を離した（ドロップした）瞬間の並び替え処理
        // ==============================
        private void PdfThumbnailViewer_DragDrop(object? sender, DragEventArgs e)
        {
            if (e.Data == null || !e.Data.GetDataPresent(typeof(int))) return;
            
            if (_dropTargetIndex != -1)
            {
                // DragOverで計算済みのターゲットと前後フラグをそのままイベントでForm1へ通知
                PageMoved?.Invoke(this, (_dropTargetIndex, _dropIsBefore));
            }

            // ドロップが終わったのでターゲット情報をリセットして再描画
            _dropTargetIndex = -1;
            this.Invalidate();

        }

        // ==============================
        // ドロップ位置の線を消す
        // ==============================
        private void PdfThumbnailViewer_DragLeave(object? sender, EventArgs e)
        {
            // コントロール外に出たらターゲットをクリアして線を消す
            _dropTargetIndex = -1;
            this.Invalidate();
        }

    }
}

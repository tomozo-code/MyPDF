using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Navigation;
using iText.Kernel.XMP;
using iText.Kernel.XMP.Impl.XPath;
using iText.Kernel.XMP.Options;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Font;
using iText.StyledXmlParser.Jsoup.Nodes;
using Org.BouncyCastle.Asn1.Cms;
using PdfiumViewer;
using System.Buffers;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO.Pipelines;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Xml.Linq;
using static MyPDF.PdfCustomViewer;
//using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DrawingColor = System.Drawing.Color;
using DrawingPoint = System.Drawing.Point;
using IOPath = System.IO.Path;
using ITextDoc = iText.Kernel.Pdf.PdfDocument;
using ITextImage = iText.Layout.Element.Image;
using PdfiTextReader = iText.Kernel.Pdf.PdfReader;
using PdfiumDoc = PdfiumViewer.PdfDocument;
using SysImage = System.Drawing.Image;
using SysRectangle = System.Drawing.Rectangle;

// ==============================
// ライブラリ：iText、PdfiumViewer.Core
// PDFの閲覧・編集・管理をシンプルに行う

// --- 役割 ---
// PDF表示 → PdfiumViewer.Core → pdfCustomViewer(ユーザーコントロール:自前)
// しおり操作 → TreeView
// サムネイル → PdfThumbnailViewer(ユーザーコントロール:自前)
// 保存 → iText

// --- セキュリティ設定の仕様 ---
// パスなし → 編集可能
// 開くパス(User) → パス要求 → 閲覧モード(編集不可)
// 権限パス(Owner) → パス要求 → 編集モード(編集可能)
// 開くパス(User) + 制限パス(Owner) → どのパスで開いたかで　閲覧 or 編集
// 開くパスのみの設定は不可
// 権限パスのみの設定は可

// --- フォーム ---
// Form1:メインフォーム
// Form2:PDFのプロパティ
// Form3:バージョン情報
// Form4:セキュリティ設定
// Form5:保護パスワード入力
// Form6:しおりプロパティ設定
// Form7:指定ページの回転
// Form8:指定ページの削除
// Form9:指定ページの抽出
// Form10:ファイル挿入
// Form11:指定ページの移動
// Form12:指定ページの置換
// Form13:画像PDF変換の設定
// Form14:PDF画像変換の設定

// --- クラス ---
// BookmarkInfo.cs:しおり用(メタデータ)
// ComboItem.cs:PDFプロパティ用(開き方コンボボックス) Form2用
// NaturalStringComparer.cs:画像をPDFに変換のファイル名ソート用クラス
// PageRangeHelper.cs:ページ指定入力のチェック
// PdfBookmarkLoader.cs:PDFのしおり情報取得用
// PdfDateUtil.cs:PDFメタデータの日付変換
// PdfFileUtil.cs:作業用ファイル作成
// PdfLoadResult.cs:PDF開く結果格納用
// PdfOpenResult.cs:開く・挿入・置換用 パスワード入力処理
// PdfSecurityHelper.cs:PDFを開いて権限確認(開く・挿入・置換用)
// PdfSettings.cs:PDFプロパティ用(メタデータ)
// PdfSettingsLoader.cs:PDFの設定情報を読み込む
// SecuritySettings.cs:セキュリティ設定用(メタデータ)
// ==============================

namespace MyPDF
{
    public partial class Form1 : Form
    {

        // ドラッグ＆ドロップ用
        // dropTargetNode → ドロップ先
        private TreeNode? dropTargetNode = null;
        // nsertAfter → 下に入れるか
        private bool insertAfter = false;
        // insertAsChild → 子にするか
        private bool insertAsChild = false;
        // 右クリック時に「ページジャンプしないようにするフラグ」
        private bool isRightClickSelecting = false;
        // しおりホバー中ノード
        private TreeNode? hoverNode = null;

        // ページ監視用（Pdfiumにイベントないので自前監視）
        private int lastPage = -1;
        private System.Windows.Forms.Timer pageTimer = new System.Windows.Forms.Timer();
        // 未保存フラグ(更新フラグ)(true:更新あり、false:更新なし)
        public bool isDirty = false;
        // 今PDF開いてる？のフラグ(ture:開いてる、false:開いてない)
        private bool isOpening = false;
        // 編集可能フラグ(PDFがセキュリティありかチェック用 true:可、false:不可)
        private bool canEdit = true;
        // サムネイルクリック時の同期フラグ
        private bool _isSyncing = false;

        // PDF設定・セキュリティ
        // 現在読み込んでいるPDFの各種設定（メタデータ・表示設定など）
        private PdfSettings? currentSettings = new PdfSettings();
        // 現在のセキュリティ設定（パスワード・権限など）
        private SecuritySettings? currentSecurity;

        // UI補助
        // ツールチップに表示するヒント文字列
        private string? toolHintTxt = null;

        // パスワード関連
        // 入力されたパスワード（一時用)
        //private string? password = null;
        // PDFをオーナーパスで開いたときのパスワードを格納
        private string? currentPassword = null;

        // 保存用
        // 元ファイルパス
        private string originalPath = "";
        // 作業用ファイルパス
        private string workingPath = "";

        // 画像PDF変換用
        // 画像PDFサイズ
        private int PdfImageMode = 2;
        // 配置場所
        private int PdfPlace = 0;
        // 余白
        private float PdfMarginTop = 0;
        private float PdfMarginBottom = 0;
        private float PdfMarginLeft = 0;
        private float PdfMarginRight = 0;

        // 仮想サムネ用
        private int thumbnailPageCount = 0;
        // サムネイルの選択ページ番号
        private int currentThumbnailPage = -1;
        // サムネイルを右クリしたときのページ番号
        private int thumbnailRightClickPage = -1;
        // サムネイル複数選択リスト
        private List<int> _savedSelectedIndices = new List<int>();

        // PDF画像変換用
        public enum SaveConflictMode
        {
            Ask,
            Overwrite,
            Rename
        }
        private SaveConflictMode _conflictMode = SaveConflictMode.Ask;
        private bool _applyToAll = false;

        // PDF開くのパスワード入力時メッセージ
        private static readonly string PasswordOpenMessage = "閲覧パスワードで開いた場合、編集不可(閲覧モード)になります。" + Environment.NewLine +
            "権限パスワードで開いた場合、編集可能(編集モード)になります。" + Environment.NewLine +
            "権限パスワードで開いたPDFファイルは、保存時に制限やパスワードが破棄されますので、" + Environment.NewLine +
            "保護を設定したい場合は" + Environment.NewLine +
            "「ファイル(F) - セキュリティ設定(T)...」から再設定してください。" + Environment.NewLine +
            "権限パスワードのみ設定されているPDFファイルは、パスワードなしで開くことができます。" + Environment.NewLine +
            "その場合は、編集不可(閲覧モード)になります。";

        // 挿入処理のパスワード入力時メッセージ
        private static readonly string InsertPassMessage = "挿入するPDFファイルは保護されています。" + Environment.NewLine +
            "権限パスワードの場合は挿入可能ですが、閲覧パスワードの場合は挿入できません。";

        // 置換処理のパスワード入力時メッセージ
        private static readonly string OkikaePassMessage = "置換するPDFファイルは保護されています。" + Environment.NewLine +
                        "権限パスワードの場合は置換可能ですが、閲覧パスワードの場合は置換できません。";

        // アプリ名（タイトルバー表示用）
        private string myName = "ともさんのPDF編集帖";
        // ツールチップ表示用：直前にマウスが乗っていたノード
        private TreeNode? lastNode = null;

        public Form1()
        {
            InitializeComponent();

            // フォームサイズ
            this.Width = 1000;
            this.Height = 700;
            this.MinimumSize = new Size(400, 400);

            // フォーム最大化
            //this.WindowState = FormWindowState.Maximized;
            // アプリ名（タイトルバー表示）
            this.Text = myName;

            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.SplitterDistance = 350;
            ViewSelect.Enabled = false;

            //pdfViewer1.Dock = DockStyle.Fill;
            tabControl1.Dock = DockStyle.Fill;
            pdfThumbnailViewer1.Dock = DockStyle.Fill;
            pdfCustomViewer1.Dock = DockStyle.Fill;

            // pdfViewerのしおり表示を無効
            //pdfViewer1.ShowBookmarks = false;
            // pdfViewerのツールバー表示を無効
            //pdfViewer1.ShowToolbar = false;


            pdfViewer1.Visible = false;
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            //PageEditMenu.Visible = false;
            //ShioriMenu.Visible = false;

            ShioriMenu.Enabled = false;
            PageEditMenu.Enabled = false;

            //splitter1.Visible = false;

            //panel1.Width = 300;
            tabControl1.Width = 350;

            treeView1.Dock = DockStyle.Fill;
            treeView1.ShowNodeToolTips = false;

            StatusLabel.Text = "ファイル: PDF未選択";
            TotalPagetoolStripLabel.Text = "/ 1 ";
            toolHintTxt = "ファイル: PDF未選択";

            // しおり列選択 代わりに線なし
            treeView1.FullRowSelect = true;
            treeView1.ShowLines = false;

            // エラー表示用
            //Extxt.Visible = true;
            //Extxt.Dock = DockStyle.Bottom;

            // Tempフォルダを開くメニュー(開発中のエラー時に確認する)
            tempOpen.Visible = false;

            // しおりドラッグ中のちらつき防止(普通は触れないので呪文を唱える感じ)
            // TreeView の「ダブルバッファ」を強制ONにして、描画ちらつきを減らす
            // ダブルバッファとは、画面に直接描かずに、裏画面（メモリ）に描く → 下書きしてから一瞬で貼る感じ
            // TreeViewクラス情報取得しアクセス
            typeof(TreeView).InvokeMember(
                "DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | // 隠しプロパティも対象に触る
                System.Reflection.BindingFlags.Instance | // treeView1 個体に対して設定
                System.Reflection.BindingFlags.SetProperty, // プロパティへ値を書き込む
                null, // 通常はnullでOK
                treeView1, // 実際に対象となるTreeView
                new object[] { true }); // DoubleBuffered = trueと意味する
        }

        // ==============================
        // フォームが読み込まれたとき    
        // ==============================
        private async void Form1_Load(object sender, EventArgs e)
        {
            // ショートカットキーの設定
            // Ctrl+O(開く)
            OpenMenu.ShortcutKeys = Keys.Control | Keys.O;
            // 画像をPDFに変換
            ConvPdf.ShortcutKeys = Keys.Control | Keys.J;
            // Ctrl + G(既定のPDFアプリで開く)
            AcrobatOpenMenu.ShortcutKeys = Keys.Control | Keys.G;
            // Ctrl+S(上書き保存)
            SaveMenu.ShortcutKeys = Keys.Control | Keys.S;
            // Ctrl+B(しおり作成)
            AddShioriToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.B;
            AddShioriMenu.ShortcutKeys = Keys.Control | Keys.B;
            // Ctrl+D(PDFのプロパティ)
            PdfPropertyMenu.ShortcutKeys = Keys.Control | Keys.D;
            // Ctrl+T(セキュリティ設定)
            SecurityMenu.ShortcutKeys = Keys.Control | Keys.T;
            // Ctrl+A(しおりのプロパティ)
            ShioriProToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.A;
            ShioriProMenu.ShortcutKeys = Keys.Control | Keys.A;
            // Ctrl+L(ページ割り当て)数値キーを上段(テンキーではない)
            SetShioriToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.L;
            SetShioriMenu.ShortcutKeys = Keys.Control | Keys.L;
            // 全てのしおりを展開
            AllShioriTenkaiToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D1;
            AllShioriTenkaiMenu.ShortcutKeys = Keys.Control | Keys.D1;
            // 全てのしおりを縮小
            AllShioriSyukusyouToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D2;
            AllShioriSyukusyouMenu.ShortcutKeys = Keys.Control | Keys.D2;
            // 選択中のしおりを展開
            ShioriTenkaiToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D3;
            ShioriTenkaiMenu.ShortcutKeys = Keys.Control | Keys.D3;
            // 選択中のしおりを縮小
            ShioriSyukusyouToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D4;
            ShioriSyukusyouMenu.ShortcutKeys = Keys.Control | Keys.D4;
            // しおりインポート
            ImportShioriToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.I;
            ImportShioriMenu.ShortcutKeys = Keys.Control | Keys.I;
            // しおりエクスポート
            ExportShioriToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.E;
            ExportShioriMenu.ShortcutKeys = Keys.Control | Keys.E;
            // 指定して回転
            //RotatePagesSetting.ShortcutKeys = Keys.Shift | Keys.Control | Keys.R;
            //RotatePagesSetting2.ShortcutKeys = Keys.Shift | Keys.Control | Keys.R;
            PageDetailesRotate2.ShortcutKeys = Keys.Shift | Keys.Control | Keys.R;
            // 移動
            //PageMove.ShortcutKeys = Keys.Shift | Keys.Control | Keys.M;
            PageMove2.ShortcutKeys = Keys.Shift | Keys.Control | Keys.M;
            // 挿入
            //PageInsert.ShortcutKeys = Keys.Shift | Keys.Control | Keys.I;
            PageInsert2.ShortcutKeys = Keys.Shift | Keys.Control | Keys.I;
            // 置換
            //ReplacementMenu.ShortcutKeys = Keys.Shift | Keys.Control | Keys.K;
            ReplacementMenu2.ShortcutKeys = Keys.Shift | Keys.Control | Keys.K;
            // 指定して抽出
            //PageExtractSetting.ShortcutKeys = Keys.Shift | Keys.Control | Keys.X;
            //PageExtractSetting2.ShortcutKeys = Keys.Shift | Keys.Control | Keys.X;
            SelectPageExtract2.ShortcutKeys = Keys.Shift | Keys.Control | Keys.X;
            // 指定して削除
            //PageDeleteSetting.ShortcutKeys = Keys.Shift | Keys.Control | Keys.D;
            //PageDeleteSetting2.ShortcutKeys = Keys.Shift | Keys.Control | Keys.D;
            SelectPageDel2.ShortcutKeys = Keys.Shift | Keys.Control | Keys.D;
            // PDFを画像に変換
            //ConvImgSetting.ShortcutKeys = Keys.Shift | Keys.Control | Keys.G;
            ConvImgSetting2.ShortcutKeys = Keys.Shift | Keys.Control | Keys.G;

            // 0.1秒ごとにページを監視
            pageTimer.Interval = 100; // 0.1秒ごと
            pageTimer.Tick += PageTimer_Tick;
            pageTimer.Start();

            // ZoomModeの選択
            ZoomtoolStripComboBox.Items.AddRange(new object[] { "自動調整", "高さに合わせる", "幅に合わせる" });
            // 自動調整
            ZoomtoolStripComboBox.SelectedIndex = 0;

            // プロブレスバー初期化
            ProgressBar.Visible = false;
            ProgressBar.Minimum = 0;
            ProgressBar.Maximum = 100;
            ProgressBar.Value = 0;
            ProgressBar.Style = ProgressBarStyle.Continuous;

            // ツールチップ設定(通常コントロール用:Tagに表示させたい内容を書く)
            SetTooltipAll(this);

            // メニューリセット
            MenuReset();

            // 未保存フラグOFF
            isDirty = false;
        }

        // ==============================
        // ページスクロールで今のページ番号を取得    
        // ==============================
        private void PageTimer_Tick(object? sender, EventArgs e)
        {
            // PDFが開かれていないなら処理しない
            //if (pdfViewer1.Document == null) return;

            // PDFが開かれていないなら処理しない
            if (pdfCustomViewer1.Document == null) return;

            // マウスがメインビューアの上にあるかどうかを判定
            // マウスカーソルの位置（画面全体の座標）を、pdfCustomViewer1から見た相対座標に変換
            DrawingPoint clientMousePos = pdfCustomViewer1.PointToClient(Cursor.Position);

            // マウスがメインビューアの枠内に入っているか
            bool isMouseOverMainViewer = pdfCustomViewer1.ClientRectangle.Contains(clientMousePos);

            // ユーザーがサムネイル側を「本当に操作中」のときだけガードする
            // ※マウスがメインビューア上にある（＝ホイールを回そうとしている）なら、フォーカスがあってもガードしない
            if ((pdfThumbnailViewer1.Focused && !isMouseOverMainViewer) ||
                (Control.MouseButtons == MouseButtons.Left && pdfThumbnailViewer1.Bounds.Contains(pdfThumbnailViewer1.PointToClient(Cursor.Position))) ||
                (Control.ModifierKeys & Keys.Shift) == Keys.Shift ||
                (Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                // テキストボックスの更新だけ行い、サムネイル選択はスルー
                int currentIdx = pdfCustomViewer1.CurrentPage;
                NewPagetoolStripTextBox.Text = (currentIdx + 1).ToString();
                lastPage = currentIdx;
                return;
            }

            // 自作のカスタムビューアから、現在表示中のページ（0始まり）を取得
            int current = pdfCustomViewer1.CurrentPage;

            // ページ変わった時だけ更新
            if (current != lastPage)
            {
                lastPage = current;

                currentThumbnailPage = current;

                if (tabControl1.SelectedTab == tabPage2)
                {
                    // 無限ループ（チャタリング）防止ガード
                    if (!pdfThumbnailViewer1.SelectedIndices.Contains(current))
                    {
                        pdfThumbnailViewer1.SetSelection(current);
                    }
                }
                // NewPagetoolStripTextBoxにページ番号を表示
                NewPagetoolStripTextBox.Text = (current + 1).ToString();
            }

        }

        // ==============================
        // 既定のPDFアプリで開く
        // ==============================
        private void AcrobatOpenMenu_Click(object sender, EventArgs e)
        {
            // PDFのパスが空ならやめる
            if (string.IsNullOrEmpty(originalPath)) return;

            try
            {
                // 既定のアプリで開く(Acrobat Reader とか) new System.Diagnostics.ProcessStartInfoは起動設定オブジェクト作成
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    // PDFのパス
                    FileName = originalPath,
                    // このファイル開いてってお願いする(true:ファイル開いて、false:exe起動して)
                    UseShellExecute = true
                });
            }
            catch (Exception ex) // エラー捕捉
            {
#if DEBUG
                Extxt.Text = ex.Message;
                MessageBox.Show("外部アプリで開けませんでした。\n" + ex.Message, "外部アプリオープン失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
#else
                MessageBox.Show("外部アプリで開けませんでした。", "外部アプリオープン失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif

            }
        }

        // ==============================
        // 開くを押したとき    
        // ==============================
        private async void OpenMenu_Click(object sender, EventArgs e)
        {
            // 変更がある場合(未保存確認ダイアログ)
            if (!await ConfirmDiscard())
                // キャンセルの場合は開かない
                return;

            try
            {
                // ファイル選択ダイアログを作成
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Title = "PDFファイルを開く";
                    //PDFだけに制限
                    ofd.Filter = "PDFファイル (*.pdf)|*.pdf";
                    // ダイアログ表示 「開く」ボタンが押されたときだけ中に入る
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            BeginProcessUi("読込中...");

                            // 開く処理へ awaitを付けて非同期メソッドを呼び出す
                            await OpenPdfAsync(ofd.FileName);

                        }
                        finally
                        {
                            // 例外が出てもフォームを操作可能にする
                            EndProcessUi();
                        }
                    }
                }
            }
            finally
            {
                // 「開いてるフラグ」を必ず解除
                isOpening = false;
            }
        }

        // ==============================
        // UI開始
        // ==============================
        private void BeginProcessUi(string msg)
        {
            StatusLabel.Text = msg;
            ProgressBar.Visible = true;
            ProgressBar.Style = ProgressBarStyle.Marquee;

            this.Enabled = false;
        }

        // ==============================
        // UI終了
        // ==============================
        private void EndProcessUi()
        {
            StatusLabel.Text = toolHintTxt;
            ProgressBar.Visible = false;
            ProgressBar.Style = ProgressBarStyle.Continuous;

            this.Enabled = true;
        }

        // ==============================
        // 開く処理
        // 1. パスワード確認
        // 2. セキュリティ情報設定
        // 3. 作業ファイル作成
        // 4. PDF設定読み込み
        // 5. PDF表示
        // 6. しおり表示
        // 7. しおりスクロール
        // 8. UI更新
        // 9. ステータス更新
        // 10. タイトル更新
        // 11. 閲覧モード通知
        // ==============================
        private async Task OpenPdfAsync(string path)
        {
            // パスワード保持用初期化
            currentPassword = null;

            // PDFを開いて権限確認へ(パス入力、PDFオープン、権限確認、暗号方式取得)
            var result = PdfSecurityHelper.CheckPdfPermission(path, PasswordOpenMessage, () => ShowPasswordDialog(PasswordOpenMessage));

            // 開けなかった場合 戻る(キャンセル、パス違う、壊れたPDFとか)
            if (!result.Success) return;

            currentPassword = result.Password;

            // 編集可能判定(true:編集可、false:編集不可)
            canEdit = result.IsOwner;

            // セキュリティ設定を格納
            SetupSecurityInfo(result);

            // しおり名編集 ON/OFF (true:編集可、false:編集不可)
            treeView1.LabelEdit = canEdit;

            // 作業用ファイルを破棄(前回PDFの tempファイル削除)
            CleanupWorkingFile();

            try
            {

                // PDF + しおりをまとめてバックグラウンドで作る
                var resultData = await Task.Run(() =>
                {
                    // PDF読み込み→PdfLoadResultへ格納
                    var load = LoadPdfData(path, result.Password);

                    // 元ファイルパス
                    originalPath = path;
                    // 作業用ファイルパス
                    workingPath = load.WorkingPath;
                    // 保存との整合性
                    currentSettings = load.Settings;

                    // PDFのしおり情報を取得
                    var bookmarks = PdfBookmarkLoader.Load(path, result.Password);

                    return (load, bookmarks);
                });

                // UIは一回でまとめて更新
                this.Invoke(() =>
                {
                    // PDF表示（最初に）
                    DisplayPdf(resultData.load.Document);

                    // しおり表示（すぐ続けて）
                    treeView1.BeginUpdate();
                    treeView1.Nodes.Clear();
                    // しおり表示UI専用
                    BuildTree(resultData.bookmarks, treeView1.Nodes);
                    treeView1.EndUpdate();

                    // その他UI更新
                    // しおりの先頭へスクロール
                    ResetBookmarkView();
                    // 右クリックメニュー更新
                    UpdateContextMenuState();
                    // ステータスバーにファイル名(元ファイル)と総ページ数
                    UpdateStatus(originalPath, currentSettings?.TotalPage ?? 0);
                    // タイトルバー更新
                    UpdateWindowTitle(path, canEdit);
                    // 閲覧モードメッセージ表示
                    ShowReadOnlyMessage(path);

                });

                // 未保存フラグOFF
                isDirty = false;

                thumbnailPageCount = currentSettings?.TotalPage ?? 0;

#if DEBUG
                // パスワード確認用
                Extxt.Text = currentPassword;
#endif
            }

            catch (Exception ex) // エラー捕捉
            {
#if DEBUG
                Extxt.Text = ex.ToString();
                MessageBox.Show("開くエラー:\n" + ex.ToString());
#else
                MessageBox.Show("PDFファイルを開けませんでした。", "PDFファイルオープン失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
            }
        }

        // ==============================
        // PDF読み込み→PdfLoadResultへ格納
        // ==============================
        private PdfLoadResult LoadPdfData(string path, string? password)
        {
            // 作業用ファイルパス
            string workingPath = PdfFileUtil.CreateTempPdfCopy(path);
            // 保存との整合性 作業用ファイルのデータを入れる
            var settings = PdfSettingsLoader.LoadPdfSettings(workingPath, path, password);
            // Pdfiumで表示
            // パス有無判定(パスワードがnull? nullならパスワードなし)
            var document = string.IsNullOrEmpty(password)
                ? PdfiumDoc.Load(workingPath) // パスワードなし
                : PdfiumDoc.Load(workingPath, password); // パスワードあり

            // PdfLoadResultへ格納
            return new PdfLoadResult
            {
                WorkingPath = workingPath,
                Settings = settings,
                Document = document
            };
        }

        // ==============================
        // セキュリティ設定を格納
        // ==============================
        private void SetupSecurityInfo(PdfOpenResult result)
        {
            // セキュリティ情報(SecuritySettings生成)
            currentSecurity ??= new SecuritySettings();
            // Owner権限で開いたか保存
            currentSecurity.IsOwnerOpened = result.IsOwner;
            // AES256など暗号方式保存
            currentSecurity.Encryption = result.CryptoMode;
            // チェック状態リセット
            currentSecurity.Check_Owner = false;
            currentSecurity.Check_User = false;
            // Owner権限で開いた場合
            if (result.IsOwner)
            {
                // 編集モード
                // 入力されたOwnerパス保存
                currentSecurity.OwnerPassword = result.Password ?? "";
                currentSecurity.UserPassword = null;　// Userパスは未使用
                // 現在開いているPDFのパスワード保存
                currentPassword = result.Password;
            }
            else
            {
                // 閲覧モード
                // Userパス保存
                currentSecurity.UserPassword = result.Password ?? "";
                currentSecurity.OwnerPassword = null;　// Ownerパス無し
                // 編集不可なので編集処理へパスは流さない
                currentPassword = null;
            }
        }

        // ==============================
        // PDF表示
        // ==============================
        private void DisplayPdf(PdfiumViewer.PdfDocument document)
        {
            // PDFを表示
            //pdfViewer1.Document = document;

            // pdfCustomViewerにPDFを表示
            pdfCustomViewer1.LoadDocument(document);

            // PDFの縦横方向に応じた表示
            DisplaySize();

            //pdfViewer1.ZoomMode = PdfViewerZoomMode.FitBest;
            // ページ番号表示
            NewPagetoolStripTextBox.Text = "1";
            // サムネイル生成
            pdfThumbnailViewer1.LoadDocument(document);

            if (tabControl1.SelectedTab == tabPage1)
            {
                ShioriMenu.Enabled = true;
                PageEditMenu.Enabled = false;

                // しおりにフォーカス
                treeView1.Focus();
            }

            if (tabControl1.SelectedTab == tabPage2)
            {
                ShioriMenu.Enabled = false;
                PageEditMenu.Enabled = true;

                // サムネイルにフォーカス
                pdfThumbnailViewer1.Focus();

            }
        }

        // ==============================
        // PDFの縦横方向に応じた表示
        // ==============================
        private void DisplaySize()
        {
            var pageSizes = pdfCustomViewer1?.Document?.PageSizes;
            SizeF firstPageSize = pageSizes?[0];

            // 自動調整
            ZoomtoolStripComboBox.SelectedIndex = 0;

            // 縦長か横長かを判定
            if (firstPageSize.Height >= firstPageSize.Width)
            {
                // 縦長なら高さに合わせる
                pdfCustomViewer1?.FitToHeight();
                ZoomtoolStripComboBox.SelectedIndex = 1;
            }
            else
            {
                // 横長なら幅に合わせる
                pdfCustomViewer1?.FitToWidth();
                ZoomtoolStripComboBox.SelectedIndex = 2;
            }

        }

        // ==============================
        // しおり表示UI専用
        // ==============================
        private void BuildTree(IEnumerable<BookmarkInfo> items, TreeNodeCollection nodes)
        {
            foreach (var item in items)
            {
                var node = new TreeNode(item.BmTitle)
                {
                    ForeColor = item.SelectedColor,
                    NodeFont = new Font(treeView1.Font, item.SelectedStyle),
                    Tag = item,
                    // 通常アイコン(桃豚アイコン)
                    ImageIndex = 0,
                    // 選択時アイコン(白豚アイコン)
                    SelectedImageIndex = 1
                };

                nodes.Add(node);

                BuildTree(item.Children, node.Nodes);

                // しおり展開 or 縮小
                if (item.IsOpen)
                    node.Expand();
            }
        }

        // ==============================
        // しおりの先頭へスクロール
        // ==============================
        private void ResetBookmarkView()
        {
            if (treeView1.Nodes.Count == 0)
                return;

            treeView1.TopNode = treeView1.Nodes[0];
            // 選択解除したい場合
            treeView1.SelectedNode = null;
        }

        // ==============================
        // パスワードがある場合のダイアログ
        // ==============================
        private string? ShowPasswordDialog(string message)
        {
            // 流用しているので、出すメッセージをセットしている
            using (var f = new Form5(message))
            {
                var result = f.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // 入力されたパスワードを返す
                    return f.Password;
                }

                return null;
            }
        }

        // ==============================
        // タイトルバー更新
        // ==============================
        private void UpdateWindowTitle(string path, bool canEdit)
        {
            // ファイル名取得
            string fileName = IOPath.GetFileName(path);
            // フルアクセス
            this.Text = myName + " - [ " + fileName + " ]";
            // 閲覧モード
            if (!canEdit)
            {
                this.Text = myName + " - [ " + fileName + "(閲覧モード) ]";
            }
        }

        // ==============================
        // 閲覧モードメッセージ表示
        // ==============================
        private void ShowReadOnlyMessage(string path)
        {
            // falseなら閲覧モードのメッセージ
            if (!canEdit)
            {
                MessageBox.Show(
                    "[ " + IOPath.GetFileName(path) + " ] は制限が設定されています。" + Environment.NewLine + "編集不可です。",
                    "制限確認(編集不可)",
                    MessageBoxButtons.OK, MessageBoxIcon.Information
                );
            }

        }

        // ==============================
        // ツリービューでキーを押したとき
        // ==============================
        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            // F2で編集 F2押された かつ SelectedNode != null(nullでない)
            if (e.KeyCode == Keys.F2 && treeView1.SelectedNode != null)
            {
                // 選択中のしおりを編集できるようにする
                treeView1.SelectedNode.BeginEdit();
            }

            // Deleteで削除 Delete押された かつ  SelectedNode != null(nullでない)
            if (e.KeyCode == Keys.Delete && treeView1.SelectedNode != null)
            {
                // 現在選択中のノードを node に保存
                var node = treeView1.SelectedNode;
                // 削除確認ダイアログを表示
                if (MessageBox.Show($"「{node.Text}」を削除しますか？" + Environment.NewLine + "配下のしおりも削除されます。",
                    "しおり削除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    // Noなら削除中止
                    return;
                // TreeViewからノード削除 子もまとめて削除
                node.Remove();

                // 右クリックメニュー更新
                UpdateContextMenuState();

                // 未保存フラグON
                isDirty = true;
            }
        }

        // ==============================
        // ツリービューのしおり編集後
        // ==============================
        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            // 念のため nullチェック（安全対策）編集対象のしおりがない場合戻る
            if (e.Node == null) return;

            // キャンセルされた場合 e.Labelは新しく入力された文字列 キャンセルはnullになる
            if (e.Label == null) return;

            // 空文字防止
            if (string.IsNullOrWhiteSpace(e.Label))
            {
                MessageBox.Show("しおり名は空にできません。", "しおり名確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // 編集をキャンセル
                e.CancelEdit = true;
                return;
            }

            // BookmarkInfoに反映
            var info = e.Node.Tag as BookmarkInfo;
            // BookmarkInfoがあるか？
            if (info != null)
            {
                // 空(null)?
                var title = string.IsNullOrWhiteSpace(e.Label)
                    // 空なら元のしおり名
                    ? e.Node.Text
                    // 空じゃないので入力したしおり名
                    : e.Label;

                // 表示とデータのズレがないようにTreeView表示とBookmarkInfoを同じものにする
                info.BmTitle = title;
            }

            // 変更されてない場合は無視
            if (e.Node.Text == e.Label)
                return;

            // 未保存フラグON
            isDirty = true;
        }

        // ==============================
        // ツリービューのしおりを押したとき
        // キー移動でしおりを選択したとき
        // ==============================
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // 右クリック？
            if (isRightClickSelecting)
            {
                // 右クリック時はジャンプしない(ページ移動させない)
                //isRightClickSelecting = false;
                return; // ←ジャンプさせない
            }
            // PDF開いてる？(true:開いてる、false:開いてない)
            // 開いてたら戻る
            if (isOpening) return;

            // クリックしたしおりのページを取得
            if (e.Node?.Tag is BookmarkInfo info && info.Page > 0)
            {
                // 取得したページへジャンプ
                JumpToPage(info.Page);
            }

        }

        // ==============================
        // ツリービューのしおりを押したとき
        // マウスクリックでしおりを選択したとき
        // ==============================
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // 右クリック？
            if (isRightClickSelecting)
            {
                // 右クリック時はジャンプしない(ページ移動させない)
                //isRightClickSelecting = false;
                return; // ←ジャンプさせない
            }
            // PDF開いてる？(true:開いてる、false:開いてない)
            // 開いてたら戻る
            if (isOpening) return;

            // クリックしたしおりのページを取得
            if (e.Node?.Tag is BookmarkInfo info && info.Page > 0)
            {
                // 取得したページへジャンプ
                JumpToPage(info.Page);
            }
        }

        // ==============================
        // しおりのページにジャンプする処理
        // ==============================
        private void JumpToPage(int page)
        {
            // PDFにページがないなら戻る
            //if (pdfViewer1.Document == null) return;

            // PDFにページがないなら戻る
            if (pdfCustomViewer1.Document == null) return;

            // pdfCustomViewer1は0始まりなので -1
            int pageIndex = page - 1;
            // 指定ページを表示            
            pdfCustomViewer1.ScrollToPage(pageIndex);

            // Pdfiumは0始まりなので -1
            //pdfViewer1.Renderer.Page = page - 1;

        }

        // ==============================
        // ステータスバーにファイル名、総ページ数
        // ==============================
        private void UpdateStatus(string path, int pageCount)
        {
            // ステータスバーにフルパスと総ページ数を表示
            //toolStripStatusLabel1.Text = $"パス: {path} | 総ページ数: {pageCount}";
            StatusLabel.Text = $"パス: {path}";
            TotalPagetoolStripLabel.Text = $"/ {pageCount} ";

            //toolHintTxt = $"パス: {path} | 総ページ数: {pageCount}";
            toolHintTxt = $"パス: {path}";
        }

        // ==============================
        // 上書き保存を押したとき
        // ==============================
        private async void SaveMenu_Click(object sender, EventArgs e)
        {
            // PDFのパスが空ならやめる
            if (string.IsNullOrEmpty(originalPath)) return;
            // 保存処理へ PDFのパスを渡す
            // awaitを付けて非同期メソッドを呼び出す
            await SavePdf(originalPath);

        }

        // ==============================
        // 名前を付けて保存を押したとき
        // ==============================
        private async void SaveAsMenu_Click(object sender, EventArgs e)
        {
            // PDFのパスが空ならやめる
            if (string.IsNullOrEmpty(originalPath)) return;
            // 保存ダイアログを表示
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                // 今開いているファイル名を取得
                string baseName = IOPath.GetFileNameWithoutExtension(originalPath);
                sfd.Title = "名前を付けてPDFファイルを保存";
                sfd.Filter = "PDFファイル (*.pdf)|*.pdf";
                sfd.FileName = baseName + "_new.pdf"; // 今開いているファイル名の後に _new.pdf を付ける

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // OK 押したら 保存処理へ PDFのパスを渡す
                    // awaitを付けて非同期メソッドを呼び出す
                    await SavePdf(sfd.FileName);
                }
            }
        }

        // ==============================
        // 保存処理(上書き保存、名前を付けて保存 共通)
        // 処理を分離した
        // UI制御、保存前チェック、PDF書き込み、Viewer解放、再読み込み、メッセージ表示、セキュリティ処理、メタデータ処理
        // ==============================
        private async Task SavePdf(string savePath)
        {
            // パスがあるか
            if (string.IsNullOrEmpty(originalPath))
                return;
            // ファイルロック確認
            if (!CheckFileLock(savePath))
                return;
            // 現在ページ取得
            // 現在の表示ページ番号を退避
            int currentPage = pdfCustomViewer1.CurrentPage;

            try
            {
                // UI開始
                BeginProcessUi("保存中...");
                // しおりを退避
                var bookmarkNodes = CloneBookmarks();
                // Viewer解放
                ReleaseViewer();

                bool securityEnabled = await SavePdfCoreAsync(savePath, bookmarkNodes);

                ReloadPdf(currentPage);

                if (securityEnabled)
                {
                    ShowSecurityMessage(savePath);
                }

                isDirty = false;
            }
            catch (Exception ex)
            {
                ShowSaveError(ex);
            }
            finally
            {
                // UI終了
                EndProcessUi();
            }
        }

        // ==============================
        // 保存処理(上書き保存、名前を付けて保存 共通)
        // ファイルロック確認
        // ==============================
        private bool CheckFileLock(string savePath)
        {
            if (savePath != originalPath)
                return true;

            try
            {
                // 誰とも共有せず読み書きで開こうとする Acrobatなどで開かれていると失敗する
                using var test = new FileStream(savePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException) // ロックされているとここへ
            {
                MessageBox.Show(
                     "PDFファイルが他のアプリで開かれています。" + Environment.NewLine +
                     "閉じてから保存してください。",
                     "保存失敗",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Warning
                 );

                return false;
            }

            return true;
        }

        // ==============================
        // 保存処理(上書き保存、名前を付けて保存 共通)
        // しおり退避
        // ==============================
        private List<TreeNode> CloneBookmarks()
        {
            // リスト化
            List<TreeNode> list = new();
            // リストにしおりを退避
            foreach (TreeNode node in treeView1.Nodes)
            {
                list.Add((TreeNode)node.Clone());
            }

            return list;
        }

        // ==============================
        // 保存処理(上書き保存、名前を付けて保存 共通)
        // Viewer解放
        // ==============================
        private void ReleaseViewer()
        {
            // Viewer解放（ロック防止）
            //if (pdfViewer1.Document == null)
            //    return;

            // Viewer解放（ロック防止）
            if (pdfCustomViewer1.Document == null)
                return;

            // サムネイル破棄
            pdfThumbnailViewer1.CloseDocument();
            // pdfCustomViewer解放
            pdfCustomViewer1.CloseDocument();

            // PDF Viewer解放
            //pdfViewer1.Document.Dispose();
            // Viewerから切り離し
            //pdfViewer1.Document = null;
        }

        // ==============================
        // 保存処理(上書き保存、名前を付けて保存 共通)
        // 保存本体
        // ==============================
        private async Task<bool> SavePdfCoreAsync(string savePath, List<TreeNode> bookmarkNodes)
        {
            bool msgFlag = false;

            // 一時作業用ファイル
            string tempPath = workingPath + ".tmp";

            await Task.Run(() =>
            {
                // PDFを書き出すためのオブジェクトを用意
                PdfWriter writer;

                // 一時作業用ファイルに書き込む
                // 制限チェックと開くのどっちかにチェックが入ってたらセキュリティ設定する
                if (currentSecurity != null && (currentSecurity.Check_Owner || currentSecurity.Check_User))
                {
                    // パスワード設定(開くパス)（UTF-8バイト配列に変換）
                    byte[]? userPass = currentSecurity.Check_User && !string.IsNullOrEmpty(currentSecurity.UserPassword)
                        ? Encoding.UTF8.GetBytes(currentSecurity.UserPassword) // UTF-8 byte配列へ変換
                        : null; // 未設定なら null

                    // パスワード設定(権限パス)（UTF-8バイト配列に変換）
                    byte[]? ownerPass = currentSecurity.Check_Owner && !string.IsNullOrEmpty(currentSecurity.OwnerPassword)
                        ? Encoding.UTF8.GetBytes(currentSecurity.OwnerPassword) // UTF-8 byte配列へ変換
                        : null; // 未設定なら null
                                // tempへ保存
                    writer = new PdfWriter(
                        tempPath, // tempのパス
                        new WriterProperties().SetStandardEncryption(
                            // パスワード設定(開くパス)
                            userPass,
                            // パスワード設定(権限パス)
                            ownerPass,
                            // 操作制御(印刷許可とか)
                            currentSecurity.Permissions,
                            // AES256等の暗号方式
                            currentSecurity.Encryption
                        )
                    );

                    // パスありなのでtrueに
                    msgFlag = true;
                }
                else
                {
                    // セキュリティなし
                    writer = new PdfWriter(tempPath);
                }

                // 今開いているPDFのパスで開く
                ReaderProperties props = string.IsNullOrEmpty(currentPassword)
                    ? new ReaderProperties() // パスなし
                    : new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(currentPassword)); // パスあり

                // 作業用PDFを開く
                using (var fs = new FileStream(workingPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                // iText Reader生成
                using (var reader = new PdfiTextReader(fs, props))
                // 読み込み＋保存用PDF Document
                using (var pdf = new ITextDoc(reader, writer))
                {
                    // 既存しおりを削除
                    pdf.GetCatalog().Remove(PdfName.Outlines);
                    // しおりを新規作成
                    var outlines = pdf.GetOutlines(true);
                    // しおりの初期は展開
                    outlines.SetOpen(true);

                    // ツリービューをPDFへ
                    foreach (TreeNode node in bookmarkNodes)
                    {
                        AddOutlineFromNode(pdf, outlines, node);
                    }

                    // メタデータ設定
                    ApplyMetadataAndViewerSettings(pdf);

                    /*

                    // 既存メタデータを完全クリア(info)
                    pdf.GetTrailer().Remove(PdfName.Info);
                    // 既存メタデータを完全クリア(XMP)
                    pdf.GetCatalog().Remove(PdfName.Metadata);

                    // 新しいinfoを作り直す
                    var info = pdf.GetDocumentInfo();

                    // 空対策
                    string Clean(string? s) => string.IsNullOrWhiteSpace(s) ? "" : s.Trim();

                    // 設定未読込チェック
                    if (currentSettings == null)
                    {
                        throw new Exception("設定が読み込まれていません。");
                    }

                    // タイトル
                    string title = Clean(currentSettings.Title);
                    // 作成者
                    string author = Clean(currentSettings.Author);
                    // サブタイトル
                    string subject = Clean(currentSettings.Subject);
                    // キーワード
                    string keywordsRaw = Clean(currentSettings.Keywords);
                    // PDF変換
                    string producer = myName;
                    // アプリケーション
                    string appName = myName;

                    // キーワード解析開始
                    var keywordList = keywordsRaw
                        // Acrobatの改行入力に対応(改行分割)
                        .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(k => k.Trim()) // 前後空白除去
                        .Where(k => !string.IsNullOrEmpty(k)) // 空除去
                        .Distinct() // 重複除去
                        .ToList(); // List化

                    // PDF Info設定
                    info.SetTitle(title);
                    info.SetAuthor(author);
                    info.SetSubject(subject);
                    info.SetCreator(appName);

                    // Info（セミコロン区切り）
                    //string keywordsJoined = string.Join("; ", keywordList);
                    //info.SetKeywords(keywordsJoined);
                    //info.SetMoreInfo("Keywords", keywordsJoined);

                    //info.SetProducer(producer);

                    // XMPメタデータ生成
                    var xmp = XMPMetaFactory.Create();

                    // 念のため完全クリア
                    xmp.DeleteProperty(XMPConst.NS_DC, "subject");
                    xmp.DeleteProperty(XMPConst.NS_DC, "title");
                    xmp.DeleteProperty(XMPConst.NS_DC, "description");
                    xmp.DeleteProperty(XMPConst.NS_DC, "creator");

                    // タイトル
                    xmp.SetLocalizedText(XMPConst.NS_DC, "title", "", "x-default", title);
                    // 説明
                    xmp.SetLocalizedText(XMPConst.NS_DC, "description", "", "x-default", subject);
                    // 作成者
                    xmp.AppendArrayItem(XMPConst.NS_DC, "creator",
                        new PropertyOptions(PropertyOptions.ARRAY_ORDERED),
                        author, null);

                    // Keywords完全クリア
                    //info.SetKeywords("");
                    pdf.GetTrailer().GetAsDictionary(PdfName.Info)?.Remove(PdfName.Keywords);

                    string keywordsJoined = string.Join("; ", keywordList);

                    // Info
                    //info.SetKeywords(keywordsJoined);

                    // subject完全削除
                    while (xmp.DoesPropertyExist(XMPConst.NS_DC, "subject"))
                    {
                        // 削除
                        xmp.DeleteProperty(XMPConst.NS_DC, "subject");
                    }

                    // キーワード追加
                    if (keywordList.Count > 0)
                    {
                        var opt = new PropertyOptions(PropertyOptions.ARRAY);
                        // キーワード1個ずつ
                        foreach (var k in keywordList.Distinct())
                        {
                            // XMPへ追加
                            xmp.AppendArrayItem(XMPConst.NS_DC, "subject", opt, k, null);
                        }
                    }

                    // PDF情報
                    xmp.SetProperty(XMPConst.NS_PDF, "Producer", producer);
                    // CreatorTool(クリエーターツール)
                    xmp.SetProperty(XMPConst.NS_XMP, "CreatorTool", producer);
                    // PDFにXMP設定
                    pdf.SetXmpMetadata(xmp);

                    // 表示設定
                    var catalog = pdf.GetCatalog();
                    // 表示モード(しおり表示など設定 アクロバットなどでPDFを開いたときの設定値)
                    switch (currentSettings?.PageMode)
                    {
                        // パネルは閉じた状態
                        case "UseNone":
                            catalog.SetPageMode(PdfName.UseNone);
                            break;
                        // しおりパネルが表示された状態
                        case "UseOutlines":
                            catalog.SetPageMode(PdfName.UseOutlines);
                            break;
                        // サムネイルが表示された状態
                        case "UseThumbs":
                            catalog.SetPageMode(PdfName.UseThumbs);
                            break;
                        // 添付ファイルパネルが表示された状態
                        case "UseAttachments":
                            catalog.SetPageMode(PdfName.UseAttachments);
                            break;
                        // レイヤーパネルが表示された状態
                        case "UseOC":
                            catalog.SetPageMode(PdfName.UseOC);
                            break;
                    }

                    // ページレイアウト切替(単ページ・見開き等設定 アクロバットなどでPDFを開いたときの設定値)
                    switch (currentSettings?.PageLayout)
                    {
                        // 単ページ表示
                        case "SinglePage":
                            catalog.SetPageLayout(PdfName.SinglePage);
                            break;
                        // 連続表示
                        case "OneColumn":
                            catalog.SetPageLayout(PdfName.OneColumn);
                            break;
                        // 見開き表示
                        case "TwoColumnLeft":
                            catalog.SetPageLayout(PdfName.TwoColumnLeft);
                            break;
                        // 表紙＋見開き表示
                        case "TwoPageLeft":
                            catalog.SetPageLayout(PdfName.TwoPageLeft);
                            break;
                    }

                    // ページ範囲チェック
                    // 総ページ数
                    int max = pdf.GetNumberOfPages();
                    // 開始ページを範囲内へ補正
                    int page = Math.Max(1, Math.Min(currentSettings.OpenPage, max));
                    // 開始ページ取得
                    var p = pdf.GetPage(page);

                    // 初期倍率切替
                    switch (currentSettings?.Zoom)
                    {
                        // 全体表示
                        case "Fit":
                            catalog.SetOpenAction(PdfExplicitDestination.CreateFit(p));
                            break;
                        // 幅に合わせる
                        case "FitH":
                            catalog.SetOpenAction(PdfExplicitDestination.CreateFitH(p, p.GetPageSize().GetTop()));
                            break;
                        // 高さに合わせる
                        case "FitV":
                            catalog.SetOpenAction(PdfExplicitDestination.CreateFitV(p, 0));
                            break;
                        // 数値
                        case "XYZ":
                            float scale = currentSettings.ZoomValue ?? 1.0f;
                            catalog.SetOpenAction(PdfExplicitDestination.CreateXYZ(p, 0, p.GetPageSize().GetTop(), scale));
                            break;
                        // 該当しない場合(全体表示を割り当てる)
                        default:
                            catalog.SetOpenAction(PdfExplicitDestination.CreateFit(p));
                            break;
                    }

                    */

                }
            });

            //File.Copy(workingPath, savePath, true);
            // 一時作業用ファイルを作業用ファイルにコピーして一時ファイルを消す
            File.Move(tempPath, workingPath, true);
            // 作業用ファイルを元ファイルにコピー true:同じ名前は上書き
            File.Copy(workingPath, savePath, true);

            originalPath = savePath;

            return msgFlag;
        }

        // ==============================
        // 指定されたPDFドキュメントに、現在のメタデータと表示設定を書き込む
        // ==============================
        private void ApplyMetadataAndViewerSettings(ITextDoc pdf)
        {
            // 1. 既存メタデータの完全クリア(info)
            pdf.GetTrailer().Remove(PdfName.Info);
            // 既存メタデータを完全クリア(XMP)
            pdf.GetCatalog().Remove(PdfName.Metadata);

            // 新しいinfoを作り直す
            var info = pdf.GetDocumentInfo();

            // 空対策のローカル関数
            string Clean(string? s) => string.IsNullOrWhiteSpace(s) ? "" : s.Trim();

            // 設定が読み込まれていない、または新規作成時でnullの場合はデフォルトインスタンスを生成
            var settings = currentSettings ?? new PdfSettings(); // ※お使いの設定クラス名に合わせてください

            // 各項目をクリーンアップして取得
            string title = Clean(settings.Title); // タイトル
            string author = Clean(settings.Author); // 作成者
            string subject = Clean(settings.Subject); // サブタイトル
            string keywordsRaw = Clean(settings.Keywords); // キーワード
            string producer = myName; // PDF変換
            string appName = myName; // アプリケーション

            // キーワード解析開始
            var keywordList = keywordsRaw
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(k => k.Trim()) // 前後空白除去
                .Where(k => !string.IsNullOrEmpty(k)) // 空除去
                .Distinct() // 重複除去
                .ToList(); // List化

            // PDF Info（レガシーメタデータ）設定
            info.SetTitle(title);
            info.SetAuthor(author);
            info.SetSubject(subject);
            info.SetCreator(appName);


            // Info（セミコロン区切り）
            //string keywordsJoined = string.Join("; ", keywordList);
            //info.SetKeywords(keywordsJoined);
            //info.SetMoreInfo("Keywords", keywordsJoined);

            //info.SetProducer(producer);

            // XMPメタデータ（モダンメタデータ）生成
            var xmp = XMPMetaFactory.Create();

            // 念のため完全クリア
            xmp.DeleteProperty(XMPConst.NS_DC, "subject");
            xmp.DeleteProperty(XMPConst.NS_DC, "title");
            xmp.DeleteProperty(XMPConst.NS_DC, "description");
            xmp.DeleteProperty(XMPConst.NS_DC, "creator");

            xmp.SetLocalizedText(XMPConst.NS_DC, "title", "", "x-default", title);
            xmp.SetLocalizedText(XMPConst.NS_DC, "description", "", "x-default", subject);
            xmp.AppendArrayItem(XMPConst.NS_DC, "creator", new PropertyOptions(PropertyOptions.ARRAY_ORDERED), author, null);

            // Keywordsクリアと再設定
            //info.SetKeywords("");
            pdf.GetTrailer().GetAsDictionary(PdfName.Info)?.Remove(PdfName.Keywords);

            string keywordsJoined = string.Join("; ", keywordList);

            // Info
            //info.SetKeywords(keywordsJoined);

            // subject完全削除
            while (xmp.DoesPropertyExist(XMPConst.NS_DC, "subject"))
            {
                // 削除
                xmp.DeleteProperty(XMPConst.NS_DC, "subject");
            }

            // キーワード追加
            if (keywordList.Count > 0)
            {
                var opt = new PropertyOptions(PropertyOptions.ARRAY);
                // キーワード1個ずつ
                foreach (var k in keywordList)
                {
                    // XMPへ追加
                    xmp.AppendArrayItem(XMPConst.NS_DC, "subject", opt, k, null);
                }
            }
            // PDF情報
            xmp.SetProperty(XMPConst.NS_PDF, "Producer", producer);
            xmp.SetProperty(XMPConst.NS_XMP, "CreatorTool", appName);
            // PDFにXMP設定
            pdf.SetXmpMetadata(xmp);

            // --- ここから表示設定 (Catalog) ---
            var catalog = pdf.GetCatalog();

            // 表示モード
            switch (settings.PageMode)
            {
                case "UseNone": catalog.SetPageMode(PdfName.UseNone); break;
                case "UseOutlines": catalog.SetPageMode(PdfName.UseOutlines); break;
                case "UseThumbs": catalog.SetPageMode(PdfName.UseThumbs); break;
                case "UseAttachments": catalog.SetPageMode(PdfName.UseAttachments); break;
                case "UseOC": catalog.SetPageMode(PdfName.UseOC); break;
                default: catalog.SetPageMode(PdfName.UseNone); break;
            }

            // ページレイアウト
            switch (settings.PageLayout)
            {
                case "SinglePage": catalog.SetPageLayout(PdfName.SinglePage); break;
                case "OneColumn": catalog.SetPageLayout(PdfName.OneColumn); break;
                case "TwoColumnLeft": catalog.SetPageLayout(PdfName.TwoColumnLeft); break;
                case "TwoPageLeft": catalog.SetPageLayout(PdfName.TwoPageLeft); break;
                default: catalog.SetPageLayout(PdfName.SinglePage); break;
            }

            // 初期倍率と開始ページ
            int maxPages = pdf.GetNumberOfPages();
            if (maxPages > 0)
            {
                int openPage = Math.Max(1, Math.Min(settings.OpenPage, maxPages));
                var p = pdf.GetPage(openPage);

                switch (settings.Zoom)
                {
                    case "Fit":
                        catalog.SetOpenAction(PdfExplicitDestination.CreateFit(p));
                        break;
                    case "FitH":
                        catalog.SetOpenAction(PdfExplicitDestination.CreateFitH(p, p.GetPageSize().GetTop()));
                        break;
                    case "FitV":
                        catalog.SetOpenAction(PdfExplicitDestination.CreateFitV(p, 0));
                        break;
                    case "XYZ":
                        float scale = settings.ZoomValue ?? 1.0f;
                        catalog.SetOpenAction(PdfExplicitDestination.CreateXYZ(p, 0, p.GetPageSize().GetTop(), scale));
                        break;
                    default:
                        catalog.SetOpenAction(PdfExplicitDestination.CreateFit(p));
                        break;
                }
            }
        }

        // ==============================
        // 保存処理(上書き保存、名前を付けて保存 共通)
        // 再読み込み
        // ==============================
        private void ReloadPdf(int currentPage)
        {
            // 再読込用パス(パスワード取得)
            string? openPassword = GetOpenPassword();
            // pdfiumViewer読み込み(パスがある場合はパスで読み込み)
            var doc = string.IsNullOrEmpty(openPassword)
                ? PdfiumDoc.Load(workingPath) // パスなし
                : PdfiumDoc.Load(workingPath, openPassword); // パスあり
            // PDF表示
            //pdfViewer1.Document = doc;

            // pdfCustomViewerにPDFを表示
            pdfCustomViewer1.LoadDocument(doc);

            // パスワードを更新
            currentPassword = openPassword;
            // 保存との整合性 作業用ファイルのデータを入れる
            //currentSettings = LoadPdfSettings(workingPath, openPassword);
            // ステータスバーにファイル名(元ファイル)と総ページ数
            //UpdateStatus(originalPath, doc.PageCount);

            // 保存との整合性 作業用ファイルのデータを入れる
            currentSettings = PdfSettingsLoader.LoadPdfSettings(workingPath, originalPath, openPassword);
            // ステータスバーにファイル名(元ファイル)と総ページ数
            UpdateStatus(originalPath, currentSettings.TotalPage);

            // ページ範囲チェック(読み込んだときに1ページ目にもどるので、退避しておいたページをセット)
            if (currentPage >= 0 && currentPage < doc.PageCount)
            {
                // 保存前のページへ戻す
                //pdfViewer1.Renderer.Page = currentPage;
                pdfCustomViewer1.ScrollToPage(currentPage);
            }

            // サムネイル生成
            pdfThumbnailViewer1.LoadDocument(doc);

            // リロード完了後、計算しておいたターゲットページを強制選択＆スクロール追従させる
            if (doc.PageCount > 0)
            {
                // 削除によって総ページ数が減り、ターゲットがはみ出る場合の安全ガード
                int finalSelectIdx = Math.Min(currentPage, doc.PageCount - 1);

                // サムネイル側の公開メソッドを呼び出して選択インデックスを上書き
                pdfThumbnailViewer1.SetSelection(finalSelectIdx);

            }

            // ファイル名だけ取得
            string fileName = IOPath.GetFileName(originalPath);
            // タイトルバー更新
            this.Text = $"{myName} - [ {fileName} ]";
        }

        // ==============================
        // 保存処理(上書き保存、名前を付けて保存 共通)
        // パスワード取得
        // ==============================
        private string? GetOpenPassword()
        {
            // 権限パスが設定されている場合はパスを取得
            // 権限パスをセット
            if (!string.IsNullOrEmpty(currentSecurity?.OwnerPassword))
                return currentSecurity.OwnerPassword;
            // 閲覧パスをセット
            if (!string.IsNullOrEmpty(currentSecurity?.UserPassword))
                return currentSecurity.UserPassword;

            return null;
        }

        // ==============================
        // 保存処理(上書き保存、名前を付けて保存 共通)
        // エラー表示
        // ==============================
        private void ShowSaveError(Exception ex)
        {
#if DEBUG
            Extxt.Text = ex.ToString();
            MessageBox.Show(ex.ToString());
#else
            MessageBox.Show("保存に失敗しました。", "保存失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
        }

        // ==============================
        // セキュリティ設定メッセージ表示
        // ==============================
        private void ShowSecurityMessage(string savePath)
        {
            // セキュリティ未設定なら何もしない
            if (currentSecurity == null)
                return;

            // Owner/User両方未設定なら何もしない
            if (string.IsNullOrEmpty(currentSecurity.OwnerPassword) &&
                string.IsNullOrEmpty(currentSecurity.UserPassword))
            {
                return;
            }

            string? setOwnerPassword = null;
            string? setUserPassword = null;

            // 閲覧パス
            if (!string.IsNullOrEmpty(currentSecurity.UserPassword))
            {
                setUserPassword = currentSecurity.UserPassword;
            }

            // 権限パス
            if (!string.IsNullOrEmpty(currentSecurity.OwnerPassword))
            {
                setOwnerPassword = currentSecurity.OwnerPassword;
            }

            string messageEdit;

            // 閲覧パスなし
            if (string.IsNullOrEmpty(setUserPassword))
            {
                messageEdit =
                    "権限パスワードは、 " + setOwnerPassword + " です。" + Environment.NewLine +
                    "閲覧パスワードは、設定されていません。";
            }
            else
            {
                messageEdit =
                    "権限パスワードは、 " + setOwnerPassword + " です。" + Environment.NewLine +
                    "閲覧パスワードは、 " + setUserPassword + " です。";
            }

            string fileName = IOPath.GetFileName(savePath);

            MessageBox.Show(
                "[ " + fileName + " ] はセキュリティが設定されました。" +
                Environment.NewLine +
                messageEdit,
                "セキュリティ確認",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        // ==============================
        // 作業用ファイル破棄（共通）
        // 前回PDFの tempファイル削除
        // ==============================
        private void CleanupWorkingFile()
        {
            try
            {
                // Viewer解放（ロック防止）
                //if (pdfViewer1.Document != null)
                if (pdfCustomViewer1.Document != null)
                {
                    // サムネイル破棄
                    pdfThumbnailViewer1.CloseDocument();
                    // pdfCustomViewer解放
                    pdfCustomViewer1.CloseDocument();

                    // PDF Viewer解放
                    //pdfViewer1.Document.Dispose();
                    // Viewerから切り離し
                    //pdfViewer1.Document = null;

                }

                // UI更新してロック解除を待つ
                //Application.DoEvents();

                // workingファイル削除
                if (!string.IsNullOrEmpty(workingPath) && File.Exists(workingPath))
                {
                    // 削除失敗してもリトライ
                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
                            // 作業ファイル削除
                            File.Delete(workingPath);
                            break;
                        }
                        catch
                        {
                            // 100ms待ってみる
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }

                // パスもクリア
                workingPath = "";
            }
            catch (Exception ex) // エラー補足
            {
#if DEBUG
                Extxt.Text = ex.ToString();
                MessageBox.Show("作業用ファイル削除エラー:\n" + ex.ToString());
#else
                MessageBox.Show("作業用ファイルが削除できませんでした。" + Environment.NewLine +
                    "「C:\\Users\\<ユーザー名>\\AppData\\Local\\Temp\\」に" + Environment.NewLine +
                    "「MyPDFwork_***.pdf」および「MyPDFwork_***.pdf.tmp」という作業用ファイル残っているため" + Environment.NewLine +
                    "手動で削除してください。", "作業用ファイル削除失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
            }
        }

        // ==============================
        // 未保存確認ダイアログ
        // ==============================
        private async Task<bool> ConfirmDiscard()
        //private bool ConfirmDiscard()
        {
            // 未保存フラグがfalseなんでそのまま帰返す
            if (!isDirty) return true;

            // 未保存ありなので保存するか聞く
            var result = MessageBox.Show(
                "変更が保存されていません。上書き保存しますか？" + Environment.NewLine +
                "はい(Y)：上書き保存" + Environment.NewLine +
                "いいえ(N)：保存しない(変更を破棄)" + Environment.NewLine +
                "キャンセル：中止",
                "保存確認",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Cancel)
                // キャンセル
                return false;

            if (result == DialogResult.Yes)
            {
                // はい 上書き保存
                // awaitを付けて非同期メソッドを呼び出す
                await SavePdf(originalPath);
            }
            else if (result == DialogResult.No)
            {
                // いいえ
            }

            return true;
        }

        // ==============================
        // ツリービューをPDFへ の処理
        // TreeViewのノードを、PDFの「しおり(PdfOutline)」へ変換するメソッド
        // 再帰で呼ばれて、階層構造もそのまま保存
        // ==============================
        private void AddOutlineFromNode(ITextDoc pdf, PdfOutline parent, TreeNode node)
        {

            // 安全に取り出す
            // node.Tagには、しおり名、ページ番号、色、スタイル、展開状態が入ってる
            if (node.Tag is not BookmarkInfo info) return;
            // しおりの移動先ページ番号取得
            int page = info.Page;
            // PDF総ページ数取得
            int max = pdf.GetNumberOfPages();

            // しおり範囲チェック(ページ番号が異常か確認)
            if (page < 1 || page > max)
            {
                // 不正なしおりなので保存しない
                Debug.WriteLine($"しおりスキップ: {node.Text} page={page} max={max}");
                return;
            }

            // しおり作成
            var outline = parent.AddOutline(node.Text);

            // ページリンク設定(しおりクリック時の移動先設定)
            outline.AddDestination(PdfExplicitDestination.CreateFit(pdf.GetPage(page)));

            // 色の設定
            var c = info.SelectedColor;
            // RGB値をPDF用カラーへ変換
            outline.SetColor(new DeviceRgb(c.R, c.G, c.B));
            // しおり文字スタイル用フラグ初期化 スタイルは、0 → 通常、1 → イタリック、2 → ボールド、3 → ボールドイタリック
            int style = 0;

            // 両方通ると ボールドイタリックになるはず
            // しおり ボールド
            if (info.SelectedStyle.HasFlag(FontStyle.Bold))
                style |= PdfOutline.FLAG_BOLD;
            //しおり イタリック
            if (info.SelectedStyle.HasFlag(FontStyle.Italic))
                style |= PdfOutline.FLAG_ITALIC;

            // iText標準APIでスタイル設定
            outline.SetStyle(style);

            // "/F"を強制的に書き込む
            outline.GetContent().Put(PdfName.F, new PdfNumber(style));

            Debug.WriteLine("保存直後 style: " + outline.GetStyle());

            // しおり内部辞書取得
            var dict = outline.GetContent();
            // /F の実値取得
            var f = dict.GetAsNumber(PdfName.F);

            Debug.WriteLine("F raw: " + f);

            // しおりの展開状態(info.IsOpenがtrue:展開、false:縮小)
            outline.SetOpen(info.IsOpen);

            // 子ノード再帰(階層構造維持)(再帰)
            foreach (TreeNode child in node.Nodes)
            {
                // 再帰呼び出し
                AddOutlineFromNode(pdf, outline, child);
            }
        }

        // ==============================
        // コンテキストメニューの表示をリセット
        // ==============================
        private void MenuReset()
        {
            // しおり作成
            AddShioriToolStripMenuItem.Enabled = false;
            AddShioriMenu.Enabled = false;
            // しおり削除
            DelShioriToolStripMenuItem.Enabled = false;
            DelShioriMenu.Enabled = false;
            // 全てのしおり削除
            AllDelToolStripMenuItem.Enabled = false;
            AllDelMenu.Enabled = false;
            // 現在のページ番号をしおりに設定
            SetShioriToolStripMenuItem.Enabled = false;
            SetShioriMenu.Enabled = false;

            // 上書き保存
            SaveMenu.Enabled = false;
            // 名前を付けて保存
            SaveAsMenu.Enabled = false;
            // 既定のPDFアプリで開く
            AcrobatOpenMenu.Enabled = false;
            // PDFのプロパティ
            PdfPropertyMenu.Enabled = false;
            // セキュリティ設定
            SecurityMenu.Enabled = false;
            // しおりのプロパティ
            ShioriProToolStripMenuItem.Enabled = false;
            ShioriProMenu.Enabled = false;
            // しおりインポート
            ImportShioriToolStripMenuItem.Enabled = false;
            ImportShioriMenu.Enabled = false;
            // しおりエクスポート
            ExportShioriToolStripMenuItem.Enabled = false;
            ExportShioriMenu.Enabled = false;

            // サムネイルドラッグ＆ドロップ
            pdfThumbnailViewer1.AllowDrop = false;

            // 表示方法切り替え
            ViewSelect.Enabled = false;


            // 移動
            PageMove.Enabled = false;
            PageMove2.Enabled = false;
            PageMove3.Enabled = false;
            // 挿入
            PageInsert.Enabled = false;
            PageInsert2.Enabled = false;
            PageInsert3.Enabled = false;
            // 置換
            ReplacementMenu.Enabled = false;
            ReplacementMenu2.Enabled = false;
            ReplacementMenu3.Enabled = false;
            // 抽出
            PageExtractSetting.Enabled = false;
            PageExtractSetting2.Enabled = false;
            PageExtractSetting3.Enabled = false;
            // ページ削除
            PageDeleteSetting.Enabled = false;
            PageDeleteSetting2.Enabled = false;
            PageDeleteSetting3.Enabled = false;
            // 回転
            RotatePagesSetting.Enabled = false;
            RotatePagesSetting2.Enabled = false;
            RotatePagesSetting3.Enabled = false;
            RotatePagesSettingLeft90.Enabled = false;
            RotatePagesSettingRight90.Enabled = false;
            RotatePagesSetting180.Enabled = false;
            // 画像変換
            ConvImgSetting.Enabled = false;
            ConvImgSetting2.Enabled = false;
            ConvImgSetting3.Enabled = false;

            // 全てのしおりを展開
            AllShioriTenkaiToolStripMenuItem.Enabled = false;
            AllShioriTenkaiMenu.Enabled = false;
            // 全てのしおりを縮小
            AllShioriSyukusyouToolStripMenuItem.Enabled = false;
            AllShioriSyukusyouMenu.Enabled = false;
            // 選択中のしおりを展開
            ShioriTenkaiToolStripMenuItem.Enabled = false;
            ShioriTenkaiMenu.Enabled = false;
            // 選択中のしおりを縮小
            ShioriSyukusyouToolStripMenuItem.Enabled = false;
            ShioriSyukusyouMenu.Enabled = false;

            // ページ番号
            NewPagetoolStripTextBox.Enabled = false;
            // 表示方法
            ZoomtoolStripComboBox.Enabled = false;
            // 閉じる
            CloseMenu.Enabled = false;
        }

        // ==============================
        // 右クリックメニュー更新 ON/OFF
        // ==============================
        private void UpdateContextMenuState()
        {
            // ツリービューノードが0以上:true
            bool hasNodes = treeView1.Nodes.Count > 0;

            // 表示しているPDFの総ページ数取得
            int pageCount = currentSettings?.TotalPage ?? 0;

            // 編集可能か？(true:可能、false:不可)
            if (!canEdit)
            {
                // セキュリティありの場合
                // しおり作成
                AddShioriToolStripMenuItem.Enabled = false;
                AddShioriMenu.Enabled = false;
                // しおり削除
                DelShioriToolStripMenuItem.Enabled = false;
                DelShioriMenu.Enabled = false;
                // 全てのしおり削除
                AllDelToolStripMenuItem.Enabled = false;
                AllDelMenu.Enabled = false;
                // 現在のページ番号をしおりに設定
                SetShioriToolStripMenuItem.Enabled = false;
                SetShioriMenu.Enabled = false;

                // 上書き保存
                SaveMenu.Enabled = false;
                // 名前を付けて保存
                SaveAsMenu.Enabled = false;
                // 既定のPDFアプリで開く
                AcrobatOpenMenu.Enabled = true;
                // PDFのプロパティ
                PdfPropertyMenu.Enabled = false;
                // セキュリティ設定
                SecurityMenu.Enabled = false;
                // しおりのプロパティ
                ShioriProToolStripMenuItem.Enabled = false;
                ShioriProMenu.Enabled = false;
                // しおりインポート
                ImportShioriToolStripMenuItem.Enabled = false;
                ImportShioriMenu.Enabled = false;
                // しおりエクスポート
                ExportShioriToolStripMenuItem.Enabled = false;
                ExportShioriMenu.Enabled = false;

                // サムネイルドラッグ＆ドロップ
                pdfThumbnailViewer1.AllowDrop = false;

                // 移動
                PageMove.Enabled = false;
                PageMove2.Enabled = false;
                PageMove3.Enabled = false;
                // 挿入
                PageInsert.Enabled = false;
                PageInsert2.Enabled = false;
                PageInsert3.Enabled = false;
                // 置換
                ReplacementMenu.Enabled = false;
                ReplacementMenu2.Enabled = false;
                ReplacementMenu3.Enabled = false;
                // 抽出
                PageExtractSetting.Enabled = false;
                PageExtractSetting2.Enabled = false;
                PageExtractSetting3.Enabled = false;
                // ページ削除
                PageDeleteSetting.Enabled = false;
                PageDeleteSetting2.Enabled = false;
                PageDeleteSetting3.Enabled = false;
                // 回転
                RotatePagesSetting.Enabled = false;
                RotatePagesSetting2.Enabled = false;
                RotatePagesSetting3.Enabled = false;
                RotatePagesSettingLeft90.Enabled = false;
                RotatePagesSettingRight90.Enabled = false;
                RotatePagesSetting180.Enabled = false;
                // 画像変換
                ConvImgSetting.Enabled = false;
                ConvImgSetting2.Enabled = false;
                ConvImgSetting3.Enabled = false;

            }
            else
            {
                // セキュリティなしの場合
                // しおり作成
                AddShioriToolStripMenuItem.Enabled = true;     // 常にOK
                AddShioriMenu.Enabled = true;     // 常にOK
                // しおり削除
                DelShioriToolStripMenuItem.Enabled = hasNodes; // ノードある時だけ
                DelShioriMenu.Enabled = hasNodes; // ノードある時だけ
                // 全てのしおり削除
                AllDelToolStripMenuItem.Enabled = hasNodes; // ノードある時だけ
                AllDelMenu.Enabled = hasNodes; // ノードある時だけ
                // 現在のページ番号をしおりに設定
                SetShioriToolStripMenuItem.Enabled = hasNodes; // ノードある時だけ
                SetShioriMenu.Enabled = hasNodes; // ノードある時だけ

                // 上書き保存
                SaveMenu.Enabled = true;
                // 名前を付けて保存
                SaveAsMenu.Enabled = true;
                // 既定のPDFアプリで開く
                AcrobatOpenMenu.Enabled = true;
                // PDFのプロパティ
                PdfPropertyMenu.Enabled = true;
                // セキュリティ設定
                SecurityMenu.Enabled = true;
                // しおりのプロパティ
                ShioriProToolStripMenuItem.Enabled = hasNodes; // ノードある時だけ
                ShioriProMenu.Enabled = hasNodes; // ノードある時だけ
                // しおりインポート                               
                ImportShioriToolStripMenuItem.Enabled = true;     // 常にOK
                ImportShioriMenu.Enabled = true;     // 常にOK
                // しおりエクスポート
                ExportShioriToolStripMenuItem.Enabled = hasNodes; // ノードある時だけ
                ExportShioriMenu.Enabled = hasNodes; // ノードある時だけ

                // サムネイルドラッグ＆ドロップ
                pdfThumbnailViewer1.AllowDrop = true;

                // 移動
                PageMove.Enabled = true;
                PageMove2.Enabled = true;
                PageMove3.Enabled = true;
                // 挿入
                PageInsert.Enabled = true;
                PageInsert2.Enabled = true;
                PageInsert3.Enabled = true;
                // 置換
                ReplacementMenu.Enabled = true;
                ReplacementMenu2.Enabled = true;
                ReplacementMenu3.Enabled = true;
                // 抽出
                PageExtractSetting.Enabled = true;
                PageExtractSetting2.Enabled = true;
                PageExtractSetting3.Enabled = true;
                // ページ削除
                if (pageCount <= 1)
                {
                    PageDeleteSetting.Enabled = false;
                    PageDeleteSetting2.Enabled = false;
                    PageDeleteSetting3.Enabled = false;
                }
                else
                {
                    PageDeleteSetting.Enabled = true;
                    PageDeleteSetting2.Enabled = true;
                    PageDeleteSetting3.Enabled = true;
                }
                // 回転
                RotatePagesSetting.Enabled = true;
                RotatePagesSetting2.Enabled = true;
                RotatePagesSetting3.Enabled = true;
                RotatePagesSettingLeft90.Enabled = true;
                RotatePagesSettingRight90.Enabled = true;
                RotatePagesSetting180.Enabled = true;
                // 画像変換
                ConvImgSetting.Enabled = true;
                ConvImgSetting2.Enabled = true;
                ConvImgSetting3.Enabled = true;

            }

            // 全てのしおりを展開
            AllShioriTenkaiToolStripMenuItem.Enabled = hasNodes;
            AllShioriTenkaiMenu.Enabled = hasNodes;
            // 全てのしおりを縮小
            AllShioriSyukusyouToolStripMenuItem.Enabled = hasNodes;
            AllShioriSyukusyouMenu.Enabled = hasNodes;
            // 選択中のしおりを展開
            ShioriTenkaiToolStripMenuItem.Enabled = hasNodes;
            ShioriTenkaiMenu.Enabled = hasNodes;
            // 選択中のしおりを縮小
            ShioriSyukusyouToolStripMenuItem.Enabled = hasNodes;
            ShioriSyukusyouMenu.Enabled = hasNodes;

            // ページ番号
            NewPagetoolStripTextBox.Enabled = true;
            // 表示方法
            ZoomtoolStripComboBox.Enabled = true;
            // 閉じる
            CloseMenu.Enabled = true;
            // 表示方法切り替え
            ViewSelect.Enabled = true;
        }

        // ==============================
        // 右クリックメニューの「しおり追加」を押したとき
        // ==============================
        private void AddShioriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 新しく作るしおりノード変数
            TreeNode? newNode = null;
            // TreeView描画停止(大量更新中のちらつき防止と再描画負荷低減)
            treeView1.BeginUpdate();
            treeView1.SuspendLayout();

            try
            {
                // 新しいTreeNode作成
                newNode = new TreeNode("新しいしおり")
                {
                    // 通常アイコン(桃豚アイコン)
                    ImageIndex = 0,
                    // 選択時アイコン(白豚アイコン)
                    SelectedImageIndex = 1
                };

                // 現在の表示ページを取得
                int currentPage = 1;
                // PDF開いてる？
                //if (pdfViewer1.Document != null)
                //{
                // 現在表示ページ取得(ゼロ始まりなので +1 する)
                //    currentPage = pdfViewer1.Renderer.Page + 1;
                //}
                // BookmarkInfo初期化開始
                newNode.Tag = new BookmarkInfo
                {
                    // しおり名
                    BmTitle = "新しいしおり",
                    // 表示されているページ
                    Page = currentPage,
                    // 色は黒(デフォルト)
                    SelectedColor = DrawingColor.Black,
                    // スタイルは標準(デフォルト)
                    SelectedStyle = FontStyle.Regular,
                    // 展開
                    IsOpen = true
                };

                // 選択ノードがあるか？
                if (treeView1.SelectedNode != null)
                {
                    // 現在選択ノード取得
                    TreeNode selected = treeView1.SelectedNode;
                    // 親ノード取得
                    TreeNode? parent = selected.Parent;
                    // 追加先ノード一覧決定 親なし → ルート 親あり → 親の子一覧
                    TreeNodeCollection nodes = parent == null ? treeView1.Nodes : parent.Nodes;
                    // 下に追加(選択ノードの次位置)
                    int index = selected.Index + 1;
                    // 選択ノードの直下へ挿入
                    nodes.Insert(index, newNode);
                }
                else
                {
                    // 選択ノードが無い場合はルートに追加
                    treeView1.Nodes.Add(newNode);
                }

                // 追加したノードを選択
                treeView1.SelectedNode = newNode;
                // 自動スクロール
                newNode.EnsureVisible();
                // TreeViewへフォーカス戻す
                treeView1.Focus();

                // 未保存フラグON
                isDirty = true;

            }
            finally // エラーでも必ず実行
            {
                // TreeView再描画再開
                treeView1.ResumeLayout();
                treeView1.EndUpdate();

                // 作成すると何故かノードの一番上で編集状態になるので苦肉の策(ちょっと遅らせる)
                BeginInvoke(new Action(() =>
                {
                    // もう一回遅らせる
                    BeginInvoke(new Action(() =>
                    {
                        // TreeView再描画
                        treeView1.Refresh();
                        // そのまま名前編集
                        newNode?.BeginEdit();
                    }));

                    // 右クリックメニュー更新
                    UpdateContextMenuState();
                }));
            }
        }

        // ==============================
        // ツリービューを右クリックしたとき
        // 右クリックしたしおりを選択状態にする
        // ==============================
        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            // しおりを右クリック？
            if (e.Button == MouseButtons.Right)
            {
                // 右クリックで選択中フラグ」をON
                isRightClickSelecting = true;
                // マウス位置にあるノード(しおり)取得
                var node = treeView1.GetNodeAt(e.X, e.Y);
                // ノード(しおり)上をクリックした？
                if (node != null)
                {
                    // 右クリックでも選択状態に
                    treeView1.SelectedNode = node;
                }
            }
        }

        // ==============================
        // ツリービューを右クリックしたとき
        // マウスアップしたときにfalseにする
        // ==============================
        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            // しおりを右クリック？
            if (e.Button == MouseButtons.Right)
            {
                // 右クリックで選択中フラグ」をOFF
                isRightClickSelecting = false;
            }
        }

        // ==============================
        // 右クリックメニューの「しおり削除」を押したとき
        // ==============================
        private void DelShioriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 選択ノードが無ければ戻る
            if (treeView1.SelectedNode == null) return;
            // 現在選択されているしおり取得
            var node = treeView1.SelectedNode;

            if (MessageBox.Show($"「{node.Text}」を削除しますか？" + Environment.NewLine + "配下のしおりも削除されます。",
                "しおり削除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No)
                // Noなら中止
                return;
            // 選択しおり削除
            treeView1.SelectedNode.Remove();

            // 右クリックメニュー更新
            UpdateContextMenuState();

            // 未保存フラグON
            isDirty = true;
        }


        // ==============================
        // ツリービューのしおりをドラッグ開始
        // (ドラッグを開始するとイベント発生)
        // ==============================
        // ドラッグ＆ドロップは分散処理してるので
        // ItemDrag(ドラッグ開始)
        // DragEnter(ドラッグ入った)
        // DragOver(移動中)
        // DragDrop(ドロップした)
        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // 権限パスで開いたらドラッグ禁止
            if (!canEdit) return;

            if (e.Item is TreeNode node)
            {
                // ドラッグ元からしおりを移動
                DoDragDrop(node, DragDropEffects.Move);
            }

        }

        // ==============================
        // ツリービューのしおりがドロップ可能
        // (ドロップ可能なところにマウスカーソルが入ったらイベント発生)
        // ==============================
        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            // 権限パスで開いたらドラッグ中禁止(見た目対策)
            if (!canEdit)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            // ドロップ先にしおりを移動
            e.Effect = DragDropEffects.Move;
        }

        // ==============================
        // ツリービューのしおりをドラッグ中
        // ==============================
        private void treeView1_DragOver(object sender, DragEventArgs e)
        {
            // 権限パスで開いたらドラッグ中禁止(見た目対策)
            if (!canEdit)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            // ドラッグ中のデータがない or TreeView型でない場合のチェック
            if (e.Data == null || !e.Data.GetDataPresent(typeof(TreeNode)))
            {
                // ドロップ先がないので戻る
                dropTargetNode = null;
                treeView1.Invalidate();
                return;
            }

            // ドラッグ位置座標
            var pt = treeView1.PointToClient(new System.Drawing.Point(e.X, e.Y));

            // ドラッグ位置にあるしおり(ノード)を取得
            TreeNode? node = treeView1.GetNodeAt(pt);

            // しおり(ノード)ある？
            if (node == null)
            {
                // なければ戻る
                dropTargetNode = null;
                treeView1.Invalidate();
                return;
            }

            // しおり(ノード)の矩形領域を取得
            var bounds = node.Bounds;

            // しおり(ノード)の上中下の3分割に分ける
            int topZone = bounds.Top + bounds.Height / 3;
            int bottomZone = bounds.Bottom - bounds.Height / 3;

            // 上に入れる(▲)
            if (pt.Y < topZone)
            {
                // 上 → 同じレベル
                insertAfter = false;
                insertAsChild = false;
            }
            //下に入れる(▼)
            else if (pt.Y > bottomZone)
            {
                // 下 → 同じレベル
                insertAfter = true;
                insertAsChild = false;
            }
            // 子に入れる(▶)
            else
            {
                // 中央 → 子にする
                insertAsChild = true;
            }

            dropTargetNode = node;
            treeView1.Invalidate();


            // スクロール追従
            int margin = 20; // 端の感度（調整OK）

            if (pt.Y < margin)
            {
                // 上スクロール
                TreeNode? prev = treeView1.TopNode?.PrevVisibleNode;
                if (prev != null)
                    treeView1.TopNode = prev;
            }
            else if (pt.Y > treeView1.Height - margin)
            {
                // 下スクロール
                TreeNode? next = treeView1.TopNode?.NextVisibleNode;
                if (next != null)
                    treeView1.TopNode = next;
            }

            // ホバーで展開
            if (node != null)
            {
                node.Expand();
            }

        }

        // ==============================
        // ツリービューのしおりをドロップ
        // ==============================
        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {

            // 権限パスで開いたらドロップ禁止(念の為)
            if (!canEdit) return;

            // しおり(ドロップデータ)がTreeNodeでない場合は戻る
            if (e.Data == null || !e.Data.GetDataPresent(typeof(TreeNode)))
                return;

            // ドラッグされてきたしおり(ノード)を取得
            TreeNode? draggedNode = e.Data.GetData(typeof(TreeNode)) as TreeNode;

            // Null or ドロップ対象がない場合は戻る
            if (draggedNode == null || dropTargetNode == null)
                return;

            // 子孫チェック（これ先でOK）
            TreeNode? tmp = dropTargetNode;
            while (tmp != null)
            {
                if (tmp == draggedNode) return;
                tmp = tmp.Parent;
            }

            // ドラッグ先(元の位置)からしおりを削除
            draggedNode.Remove();

            if (insertAsChild)
            {
                // 子と判断の場合は「子として追加」
                dropTargetNode.Nodes.Add(draggedNode);
                dropTargetNode.Expand();
            }
            // 同階層の場合
            else
            {
                // 親ノード取得
                TreeNode? parent = dropTargetNode.Parent;
                TreeNodeCollection nodes =
                    parent == null ? treeView1.Nodes : parent.Nodes;

                int index = dropTargetNode.Index;

                if (insertAfter) index++;

                if (index < 0 || index > nodes.Count)
                    nodes.Add(draggedNode);
                else
                    nodes.Insert(index, draggedNode);
            }

            // ドロップ後に新しいしおり(移動したしおり)を選択状態に
            treeView1.SelectedNode = draggedNode;

            // ドロップ対象をクリアし再描画
            dropTargetNode = null;
            treeView1.Invalidate();

            // 右クリックメニュー更新
            UpdateContextMenuState();

            // 未保存フラグON
            isDirty = true;

        }

        // ==============================
        // ツリービューのしおり ドラッグ中にマウスがツリービューから離れたらイベント発生
        // ==============================
        private void treeView1_DragLeave(object sender, EventArgs e)
        {

            dropTargetNode = null;
            // 再描画要求
            treeView1.Invalidate();

        }

        // ==============================
        // ツリービューのしおり ドラッグ中の描画
        // ==============================
        private void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            //e.DrawDefault = true;

            // --- 自前で描画 ---------------
            // 標準描画は使わない
            e.DrawDefault = false;
            // 描画用の Graphics オブジェクトを取り出す
            Graphics g = e.Graphics;

            // null対策
            if (e.Node == null) return;

            // --- 選択・ホバー状態に応じた背景を描画 -----------------------
            // 選択状態
            bool isSelected = (treeView1.SelectedNode == e.Node);

            // 背景色
            DrawingColor backColor;

            // 選択中？
            if (isSelected)
            {
                // 選択中だったら背景を Windows標準選択色 にする
                backColor = DrawingColor.Pink;
                //backColor = SystemColors.Highlight;

            }
            // マウスホバー中？
            else if (e.Node == hoverNode)
            {
                // ホバー色(薄いグレー)
                //backColor = DrawingColor.FromArgb(230, 240, 250);
                backColor = DrawingColor.FromArgb(200, 200, 200);
                //backColor = DrawingColor.FromArgb(63, 63, 65);

            }
            // 選択もホバーもしてない
            else
            {
                // TreeView通常背景色を使う
                backColor = treeView1.BackColor;
            }

            // 背景塗り
            using (Brush backBrush = new SolidBrush(backColor))
            {
                //矩形塗り開始(しおりの右端まで塗る)
                // ブラシ、x座標、Y座標、幅、高さ を指定
                g.FillRectangle(
                    backBrush, // 使うブラシ(背景色)
                    0, // x座標(左端から塗る)
                    e.Bounds.Top, // y座標(今描画しているしおりの上端)
                    treeView1.Width, // 幅(TreeViewの全幅)
                    treeView1.ItemHeight // 高さ(TreeViewの全高)
                    );
            }

            // --- ツリー線描画 ----------------
            // 線色(選択状態は白、未選択はデープピンク)
            DrawingColor lineColor =
                isSelected
                ? DrawingColor.White
                : DrawingColor.DeepPink;

            // 線を描くための Pen 作成
            using (Pen linePen = new Pen(lineColor))
            {
                // 使える線種
                // DashStyle.Solid       実線
                // DashStyle.Dot         点線 ← Explorer風
                // DashStyle.Dash        長い破線
                // DashStyle.DashDot     一点鎖線
                // DashStyle.DashDotDot  二点鎖線
                //linePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                linePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                // 線幅
                linePen.Width = 1;
                // 線を線幅の真ん中に書く
                linePen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;

                // ノード中央Y座標計算
                int midY = e.Bounds.Top + treeView1.ItemHeight / 2;

                // +ボックス中央X
                int glyphX = e.Bounds.X - 25;

                // 横線（+からアイコン方向）
                // ルート以外だけ横線
                if (e.Node.Level > 0)
                {
                    // 子ノードなので横線を描画
                    g.DrawLine(
                    linePen, // 使うPen
                    glyphX + 5, // 開始x座標
                    midY, // 開始y座標
                    glyphX + 20, // 終了x座標
                    midY // 終了y座標
                    );
                }

                // 縦線（自ノード）
                // 末っ子なら中央まで
                int bottomY;

                // 次兄弟がいない？
                if (e.Node.NextNode == null)
                {
                    // └ (末っ子なので中央で縦線を止める)
                    bottomY = midY;
                }
                else
                {
                    // ├ (兄弟おるんで、縦線を下まで伸ばす)
                    bottomY = e.Bounds.Bottom;
                }
                // 縦線描画
                g.DrawLine(
                    linePen, // 使うPen
                    glyphX + 5, // 開始x座標
                    e.Bounds.Top, // 開始y座標
                    glyphX + 5, // 終了x座標
                    bottomY // 終了y座標
                );

                // 親階層の縦線(親ノード取得)
                TreeNode? parent = e.Node.Parent;

                //int parentX = glyphX - 20;

                // 親が存在する限り上へ辿る
                while (parent != null)
                {
                    // 親階層のXを計算(縦線が揃うように計算)
                    int x = (e.Bounds.X - 25) - ((e.Node.Level - parent.Level) * 16);

                    // 次兄弟があるなら縦線継続
                    if (parent.NextNode != null)
                    {
                        g.DrawLine(
                            linePen, // 使うPen
                            x + 5, // 開始x座標
                            e.Bounds.Top, // 開始y座標
                            x + 5, // 終了x座標
                            e.Bounds.Bottom // 終了y座標
                        );
                    }
                    // さらに上の親へ移動
                    parent = parent.Parent;
                }
            }

            // ---  + - ボタン描画  --------------------------
            if (e.Node.Nodes.Count > 0)
            {
                // 四角領域を作成
                SysRectangle glyphRect = new SysRectangle(
                    e.Bounds.X - 10,
                    e.Bounds.Y + (treeView1.ItemHeight - 10) / 2,
                    10,
                    10
                );

                // 枠(枠はグレー、塗り潰しは白)
                g.FillRectangle(Brushes.White, glyphRect);
                g.DrawRectangle(Pens.Gray, glyphRect);

                // 横線(-)
                g.DrawLine(
                    Pens.Black,
                    glyphRect.Left + 2,
                    glyphRect.Top + 5,
                    glyphRect.Right - 2,
                    glyphRect.Top + 5
                );

                // 閉じてる時だけ縦線（＋）
                if (!e.Node.IsExpanded)
                {
                    g.DrawLine(
                        Pens.Black,
                        glyphRect.Left + 5,
                        glyphRect.Top + 2,
                        glyphRect.Left + 5,
                        glyphRect.Bottom - 2
                    );
                }

            }

            // アイコン描画(通常0 / 選択中1)
            int iconIndex = isSelected ? 1 : 0;

            if (iconIndex < imageList1.Images.Count)
            {
                SysImage img = imageList1.Images[iconIndex];

                int imgX = e.Bounds.X + 2;

                int imgY =
                    e.Bounds.Y +
                    (treeView1.ItemHeight - img.Height) / 2;

                g.DrawImage(img, imgX, imgY);
            }

            // ノードごとの文字設定
            // ノード文字色
            DrawingColor foreColor = (e.State & TreeNodeStates.Selected) != 0 ? DrawingColor.Black : e.Node.ForeColor;
            //foreColor = (e.State & TreeNodeStates.Selected) != 0 ? SystemColors.HighlightText : e.Node.ForeColor;
            //DrawingColor foreColor = (e.State & TreeNodeStates.Selected) != 0 ? SystemColors.HighlightText : e.Node.ForeColor;

            // ノードフォント
            Font nodeFont = e.Node.NodeFont ?? treeView1.Font;

            int textX = e.Bounds.X + 20;

            // 上下中央用Rect
            SysRectangle textRect = new SysRectangle(
                //e.Bounds.X + 20,
                textX,
                e.Bounds.Y,
                //e.Bounds.Width + e.Bounds.X,
                treeView1.Width - textX - 4,
                treeView1.ItemHeight
            );

            // 文字描画
            TextRenderer.DrawText(
                g, // 描画先の Graphics オブジェクト
                e.Node?.Text, // 描画する文字列(しおり名)
                nodeFont, // フォント
                textRect, // しおりを書く範囲
                foreColor, // 色
                TextFormatFlags.VerticalCenter | // 縦方向中央揃え
                TextFormatFlags.Left | // 左寄せ
                TextFormatFlags.NoPrefix | // &を特殊記号として扱わない
                TextFormatFlags.EndEllipsis // 文字が長すぎる場合に ... を付ける(機能してない？)
            );


            // --- ドラッグ中描画処理 ---------------
            if (dropTargetNode == null) return;
            if (e.Node != dropTargetNode) return;

            //var g = e.Graphics;
            var bounds = e.Bounds;

            // 子にする場合
            if (insertAsChild)
            {
                // 枠用Rect
                SysRectangle rect = new SysRectangle(
                    bounds.X + 20,
                    bounds.Y,
                    //bounds.Width,
                    treeView1.Width,
                    bounds.Height
                );

                // 背景ハイライト
                using (Brush b = new SolidBrush(
                    DrawingColor.FromArgb(80, DrawingColor.LightBlue)))
                {
                    g.FillRectangle(b, rect);
                }

                // 枠線
                using (Pen pen = new Pen(DrawingColor.Blue, 2))
                {
                    g.DrawRectangle(pen, rect);
                }

                // ▶マーク（左に表示 青枠 + ▶）
                g.DrawString("▶",
                    this.Font,
                    Brushes.Blue,
                    bounds.X - 20,
                    bounds.Y + 3);
            }
            else
            {

                // 同レベル（上下）
                int y = insertAfter
                    ? bounds.Bottom
                    : bounds.Top;

                // 太線
                using (Pen pen = new Pen(DrawingColor.Red, 3))
                {
                    g.DrawLine(pen, 0, y, treeView1.Width, y);
                }

                // ▲▼マーク（上 or 下に表示 赤線 + ▲▼）
                string arrow = insertAfter ? "▼" : "▲";

                g.DrawString(arrow,
                    this.Font,
                    Brushes.Red,
                    bounds.X - 20,
                    y - 20);
            }
        }

        // ==============================
        // 現在のページ番号表示(NewPagetoolStripTextBox)でキーを押したとき
        // ==============================
        private void NewPagetoolStripTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // エンター以外なら戻る
            if (e.KeyCode != Keys.Enter) return;
            // プロパティ情報ががないなら戻る
            if (currentSettings == null) return;

            // 数値チェック
            if (!int.TryParse(NewPagetoolStripTextBox.Text, out int page))
                return;

            // ページ範囲チェック
            int maxPage = currentSettings?.TotalPage ?? 0;

            // ページ範囲外なら戻る
            if (page < 1 || page > maxPage)
                return;

            // ジャンプ
            JumpToPage(page);

            // Enter音防止
            e.SuppressKeyPress = true;
        }

        // ==============================
        // NewPagetoolStripTextBoxをクリックしたとき
        // ==============================
        private void NewPagetoolStripTextBox_Click(object sender, EventArgs e)
        {
            // 全選択
            NewPagetoolStripTextBox.SelectAll();
        }

        // ==============================
        // 表示されているページをしおりにセット
        // ==============================
        private void SetShioriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // しおりが選択されていないなら何もしない
            if (treeView1.SelectedNode == null) return;
            // しおりが選択されていないなら何もしない
            //if (pdfViewer1.Document == null) return;

            // pdf開いてないなら何もしない
            if (pdfCustomViewer1.Document == null) return;

            // 今表示しているページ番号を取得(0始まりなので+1)
            //int currentPage = pdfViewer1.Renderer.Page + 1;
            int currentPage = pdfCustomViewer1.CurrentPage + 1;


            // 選択中のしおりにページ番号を保存
            if (treeView1.SelectedNode.Tag is BookmarkInfo info)
            {
                info.Page = currentPage;
            }

            // 未保存フラグON
            isDirty = true;

            // 視覚的にわかるように(チェック用)
            //treeView1.SelectedNode.Text = $"{treeView1.SelectedNode.Text.Split('(')[0].Trim()} ({currentPage})";
        }

        // ==============================
        // 終了を押したとき
        // ==============================
        private void EndMenu_Click(object sender, EventArgs e)
        {
            // 現在のフォームを閉じる
            this.Close();
        }

        // ==============================
        // フォームが閉じられる直前の処理
        // ==============================
        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 未保存チェック
            if (!await ConfirmDiscard())
            {
                e.Cancel = true;
                return;
            }
            // 作業用ファイルを破棄(前回PDFの tempファイル削除)
            CleanupWorkingFile();
        }

        // ==============================
        // PDFプロパティを表示
        // ==============================
        private void PdfPropertyMenu_Click(object sender, EventArgs e)
        {
            // PDFが開かれていないなら処理しない
            //if (pdfViewer1.Document == null)
            //{
            //    MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            // PDFが開かれていないなら処理しない
            if (pdfCustomViewer1.Document == null)
            {
                MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // PDF設定情報が存在するか確認
            if (currentSettings == null)
            {
                MessageBox.Show("PDF設定情報の取得に失敗しました。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var f = new Form2(currentSettings))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    currentSettings = f.Settings;
                    // 未保存フラグON
                    isDirty = true;

                }
            }
        }

        // ==============================
        // マウスONで説明(ToolStrip系) 
        // ==============================
        private void menuStrip1_MouseEnter(object? sender, EventArgs e)
        {
            if (sender is ToolStripItem item) StatusLabel.Text = item.ToolTipText;
        }

        // ==============================
        // マウス離脱(ToolStrip系)
        // ==============================
        private void menuStrip1_MouseLeave(object? sender, EventArgs e)
        {
            // 戻す
            StatusLabel.Text = toolHintTxt;

        }

        // ==============================
        // マウスONで説明(通常コントロール) Tagに書く 
        // ==============================
        private void Control_MouseEnter(object? sender, EventArgs e)
        {
            if (sender is Control ctrl)
            {
                StatusLabel.Text = ctrl.Tag?.ToString() ?? "";
            }
        }

        // ==============================
        // マウス離脱(通常コントロール)
        // ==============================
        private void Control_MouseLeave(object? sender, EventArgs e)
        {
            // 戻す
            StatusLabel.Text = toolHintTxt;
        }

        // ==============================
        // マウスONで説明の実行(通常コントロール) Tagに書いたもの 
        // ==============================
        private void SetTooltipAll(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl.Tag != null)
                {
                    ctrl.MouseEnter += Control_MouseEnter;
                    ctrl.MouseLeave += Control_MouseLeave;
                }

                // 子コントロールも再帰
                if (ctrl.HasChildren)
                {
                    SetTooltipAll(ctrl);
                }
            }
        }

        // ==============================
        // 全てのしおりを展開
        // ==============================
        private void AllShioriTenkaiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 全てのしおりを展開
            treeView1.ExpandAll();
        }

        // ==============================
        // 全てのしおりを縮小
        // ==============================
        private void AllShioriSyukusyouToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 全てのしおりを縮小
            treeView1.CollapseAll();
        }

        // ==============================
        // 選択中のしおりを展開
        // ==============================
        private void ShioriTenkaiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            var node = treeView1.SelectedNode;
            // 選択中のしおり以下を展開
            node.ExpandAll();
        }

        // ==============================
        // 選択中のしおりを縮小
        // ==============================
        private void ShioriSyukusyouToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            var node = treeView1.SelectedNode;
            // 選択中のしおり以下を縮小
            node.Collapse();

        }

        // ==============================
        // しおりが展開されたとき
        // ==============================
        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            treeView1.Invalidate();
            if (e.Node?.Tag is BookmarkInfo info)
            {
                info.IsOpen = true;
            }

        }

        // ==============================
        // しおりが縮小されたとき
        // ==============================
        private void treeView1_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            treeView1.Invalidate();
            if (e.Node?.Tag is BookmarkInfo info)
            {
                info.IsOpen = false;
            }

        }

        // ==============================
        // バージョン情報フォームを呼び出す
        // ==============================
        private void VerMenu_Click(object sender, EventArgs e)
        {
            using (var aboutform = new Form3())
            {
                aboutform.ShowDialog(this);
            }
        }

        // ==============================
        // 使い方を開く
        // ==============================
        private void UseMenu_Click(object sender, EventArgs e)
        {
            try
            {
                // 既定のPDFアプリで開く(Acrobat Reader とか)
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "sample.pdf",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
#if DEBUG
                Extxt.Text = ex.Message;
                MessageBox.Show("外部アプリで開けませんでした。\n" + ex.Message, "外部アプリオープン失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
#else
                MessageBox.Show("外部アプリで開けませんでした。", "外部アプリオープン失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif

            }

        }

        // ==============================
        // 作業用ファイル保存先フォルダ(temp)を開く
        // ==============================
        private void tempOpen_Click(object sender, EventArgs e)
        {
            try
            {
                // Windows環境のTempフォルダのパス（C:\Users\<ユーザー名>\AppData\Local\Temp\）を自動取得
                string tempFolder = IOPath.GetTempPath();

                if (Directory.Exists(tempFolder))
                {
                    // エクスプローラーでTempフォルダを開く
                    System.Diagnostics.Process.Start("explorer.exe", tempFolder);
                }
                else
                {
                    MessageBox.Show("Tempフォルダが見つかりませんでした。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex) //エラー補足
            {
                MessageBox.Show($"フォルダを開けませんでした。\n{ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==============================
        // ZoomtoolStripComboBoxを選択したとき
        // ==============================
        private void ZoomtoolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (pdfViewer1.Document == null) return;

            if (pdfCustomViewer1.Document == null) return;

            // 表示方法選択
            int selectedIndex = ZoomtoolStripComboBox.SelectedIndex;

            switch (selectedIndex)
            {
                // 自動調整
                case 0:
                    // カスタム側：ひとまず100%（1.0f）にする例（またはFitToWidthでもOK）
                    //pdfCustomViewer1.Zoom = 1.0f;
                    pdfCustomViewer1.AutoFit();
                    break;

                // 高さに合わせる
                case 1:
                    // カスタム側：高さに合わせるメソッドを呼び出す
                    pdfCustomViewer1.FitToHeight();
                    break;

                // 幅に合わせる
                case 2:
                    // カスタム側：すでに作成済みの幅合わせメソッドを呼び出す
                    pdfCustomViewer1.FitToWidth();
                    break;
            }
            // 既存のページ番号を取得（0始まり）
            int currentPage = pdfCustomViewer1.CurrentPage;
            // カスタムビューア側も、倍率変更後に現在のページ位置へ再スクロールさせて描画更新
            pdfCustomViewer1.ScrollToPage(currentPage);


            /*
            // 先にZoomをリセット（ここが核心）
            pdfViewer1.Renderer.Zoom = 1.0f;

            switch (ZoomtoolStripComboBox.SelectedIndex)
            {
                // 自動調整
                case 0:
                    pdfViewer1.ZoomMode = PdfViewerZoomMode.FitBest;
                    break;

                // 高さに合わせる
                case 1:
                    pdfViewer1.ZoomMode = PdfViewerZoomMode.FitHeight;
                    break;

                // 幅に合わせる
                case 2:
                    pdfViewer1.ZoomMode = PdfViewerZoomMode.FitWidth;
                    break;

            }

            // 再描画
            int page = pdfViewer1.Renderer.Page;

            pdfViewer1.Refresh();

            */
        }


        // ==============================
        // セキュリティ設定を開く
        // ==============================
        private void SecurityMenu_Click(object sender, EventArgs e)
        {
            using (var f = new Form4(currentSecurity))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    // 設定を受け取る
                    currentSecurity = f.Settings;
                    // 未保存フラグON
                    isDirty = true;

                }
            }

        }

        // ==============================
        // しおりのプロパティを開く
        // ==============================
        private void ShioriProToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = treeView1.SelectedNode;

            if (node == null)
            {
                MessageBox.Show("しおりを選択してください。", "しおり選択確認", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // BookmarkInfoからしおりのプロパティ(色、スタイル)を取得
            if (node.Tag is not BookmarkInfo info)
            {
                MessageBox.Show("しおり情報が取得できません。", "しおり情報確認", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 現在値取得
            System.String? currentBmTitle = info.BmTitle ?? "";
            int currentPage = info.Page;
            DrawingColor currentColor = info.SelectedColor;
            FontStyle currentStyle = info.SelectedStyle;

            // PDFが開かれていないなら処理しない
            //if (pdfViewer1.Document == null)
            //{
            //    MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            // PDFが開かれていないなら処理しない
            if (pdfCustomViewer1.Document == null)
            {
                MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // PDF設定情報が存在するか確認
            if (currentSettings == null)
            {
                MessageBox.Show("PDF設定情報の取得に失敗しました。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Form6起動
            using (var f = new Form6(currentBmTitle, currentPage, currentColor, currentStyle, currentSettings.TotalPage))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    // データ更新（本体）
                    info.BmTitle = f.SelectedBmTitle;
                    info.Page = f.SelectedPage;
                    info.SelectedColor = f.SelectedColor;
                    info.SelectedStyle = f.SelectedStyle;

                    // UIにも反映
                    node.Text = f.SelectedBmTitle;
                    node.ForeColor = f.SelectedColor;

                    node.NodeFont = new Font(
                        treeView1.Font.FontFamily,
                        treeView1.Font.Size,
                        f.SelectedStyle
                        );

                    treeView1.Refresh();

                    // 未保存フラグON
                    isDirty = true;

                }
            }
        }

        // ==============================
        // しおりマウスホバー
        // ==============================
        private void treeView1_MouseLeave(object sender, EventArgs e)
        {
            hoverNode = null;
            treeView1.Invalidate();
        }


        // ==============================
        // しおりツールチップ
        // ==============================
        private void treeView1_MouseMove(object sender, MouseEventArgs e)
        {
            var node = treeView1.GetNodeAt(e.Location);

            // マウスホバーで薄く塗る
            if (hoverNode != node)
            {
                hoverNode = node;
                treeView1.Invalidate();
            }

            // --- ツールチップ処理 --------------------
            if (node == lastNode) return;
            lastNode = node;

            if (node == null)
            {
                treeToolTip.SetToolTip(treeView1, "");
                return;
            }

            if (node.Tag is not BookmarkInfo info) return;

            // スタイル変換
            string styleText = GetStyleName(info.SelectedStyle);
            // 色を16進に変換
            string colorName = GetColorName(info.SelectedColor);

            string text =
                $"しおり名： {info.BmTitle}\n" +
                $"ページ： {info.Page}\n" +
                //$"展開： {(info.IsOpen ? "開く" : "閉じる")}\n" +
                $"スタイル： {styleText}\n" +
                $"色： {colorName} (16進)";

            treeToolTip.SetToolTip(treeView1, text);
        }

        // ==============================
        // 色を16進に変換(ツールチップ用)
        // ==============================
        private string GetColorName(DrawingColor color)
        {
            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        // ==============================
        // スタイル変換(ツールチップ用)
        // ==============================
        private string GetStyleName(FontStyle style)
        {
            bool isBold = style.HasFlag(FontStyle.Bold);
            bool isItalic = style.HasFlag(FontStyle.Italic);

            if (isBold && isItalic)
                return "ボールドイタリック";

            if (isBold)
                return "ボールド";

            if (isItalic)
                return "イタリック";

            return "標準";
        }

        // ==============================
        // しおりインポート
        // ==============================
        private async void ImportShioriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "しおりファイル(CSV)をインポート";
                ofd.Filter = "CSVファイル (*.csv)|*.csv";

                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    BeginProcessUi("読込中...");

                    var list = await Task.Run(() =>
                    {
                        return LoadCsvBookmarks(ofd.FileName);
                    });

                    // 既存しおり削除（上書き）
                    treeView1.Nodes.Clear();

                    BuildTreeFromCsv(list);
                    // 右クリックメニュー更新
                    UpdateContextMenuState();

                    // 未保存フラグON
                    isDirty = true;

                    MessageBox.Show("しおりをインポートしました。", "インポート", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
#if DEBUG
                    Extxt.Text = ex.Message;
                    MessageBox.Show("インポート失敗:\n" + ex.Message, "インポート失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
#else
                    MessageBox.Show("しおりファイル(CSVファイル)をインポートできませんでした。" + Environment.NewLine +
                        "しおりファイルの内部データを確認してください。", "しおりファイルインポート失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
                }
                finally
                {
                    EndProcessUi();
                }

            }
        }

        // ==============================
        // しおりCSVの読み込み
        // ==============================
        private List<CsvBookmark> LoadCsvBookmarks(string path)
        {
            var list = new List<CsvBookmark>();
            var lines = File.ReadAllLines(path, Encoding.UTF8);

            for (int i = 1; i < lines.Length; i++) // 1行目スキップ
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                var cols = lines[i].Split(',');

                if (cols.Length < 5) continue;

                var item = new CsvBookmark
                {
                    Title = cols[0],
                    Page = int.TryParse(cols[1], out int p) ? p : 1,
                    Level = int.TryParse(cols[2], out int l) ? l : 0,
                    Style = ParseStyle(cols[3]),
                    Color = ParseColor(cols[4])
                };

                list.Add(item);
            }

            return list;
        }

        // ==============================
        // しおり(ツリービュー)構築
        // CSVから読み込んだ「しおり一覧」を使って、TreeViewに階層構造を作るメソッド
        // ==============================
        private void BuildTreeFromCsv(List<CsvBookmark> list)
        {
            // 親子関係を管理するためにStack（スタック）を作成
            // スタックは、後入れ先出し（LIFO）の入れ物
            Stack<TreeNode> stack = new Stack<TreeNode>();

            // CSVのしおりを1件ずつ処理
            foreach (var item in list)
            {
                // TreeView用ノード作成 表示文字列はCSVのタイトル
                TreeNode node = new TreeNode(item.Title);

                // しおりクラスに入れる
                node.Tag = new BookmarkInfo
                {
                    BmTitle = item.Title, // しおり名
                    Page = item.Page, // ページ番号
                    IsOpen = true, // 展開
                    SelectedColor = item.Color, // 文字色
                    SelectedStyle = item.Style // スタイル
                };

                // 色をUIに適用
                node.ForeColor = item.Color;
                // スタイルをUIに適用
                node.NodeFont = new Font(treeView1.Font, item.Style);
                // CSVの階層レベル取得
                int level = item.Level;

                while (stack.Count > level)
                    // 現在の階層まで戻す
                    stack.Pop();

                // ルート(親)か子か判定
                if (stack.Count == 0)
                    // ルートノードとして追加
                    treeView1.Nodes.Add(node);
                else
                    // 親あり stackの一番上を親として追加
                    stack.Peek().Nodes.Add(node);
                // 今追加したノードをstackへ積む 次の子要素の親候補になる
                stack.Push(node);
            }
            // 全しおりを展開表示
            //treeView1.ExpandAll();
        }

        // ==============================
        // スタイルを変換(CSV読み込み)インポートとエクスポート共用
        // ==============================
        private FontStyle ParseStyle(string s)
        {
            return s switch
            {
                "ボールド" => FontStyle.Bold,
                "イタリック" => FontStyle.Italic,
                "ボールドイタリック" => FontStyle.Bold | FontStyle.Italic,
                _ => FontStyle.Regular
            };
        }

        // ==============================
        // 色を変換(CSV読み込み)
        // ==============================
        private DrawingColor ParseColor(string hex)
        {
            try
            {
                return ColorTranslator.FromHtml(hex);
            }
            catch
            {
                return DrawingColor.Black;
            }
        }

        // ==============================
        // しおりエクスポート
        // ==============================
        private void ExportShioriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.Nodes.Count == 0)
            {
                MessageBox.Show("しおりがありません。", "しおり確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "しおりをCSV形式でエクスポート";
                sfd.Filter = "CSVファイル (*.csv)|*.csv";
                sfd.FileName = "shiori.csv";

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    ExportCsvBookmarks(sfd.FileName);
                    MessageBox.Show("しおりをエクスポートしました。", "エクスポート", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
#if DEBUG
                    Extxt.Text = ex.Message;
                    MessageBox.Show("エクスポート失敗:\n" + ex.Message, "エクスポート失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
#else
                    MessageBox.Show("しおりのエプスポートに失敗しました。", "しおりエクスポート失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
                }
            }
        }

        // ==============================
        // しおりCSVの書き出し
        // ==============================
        private void ExportCsvBookmarks(string path)
        {
            var lines = new List<string>();

            // ヘッダー
            lines.Add("しおり名,ページ番号,階層,スタイル,色");

            foreach (TreeNode node in treeView1.Nodes)
            {
                ExportNodeRecursive(node, 0, lines);
            }

            File.WriteAllLines(path, lines, Encoding.UTF8);
        }

        // ==============================
        // 再帰でツリーをCSV化
        // ==============================
        private void ExportNodeRecursive(TreeNode node, int level, List<string> lines)
        {
            if (node.Tag is BookmarkInfo info)
            {
                string style = GetStyleName(info.SelectedStyle);
                string color = GetColorHex(info.SelectedColor);

                // CSV行
                string line = $"{EscapeCsv(info.BmTitle)},{info.Page},{level},{style},{color}";
                lines.Add(line);
            }

            foreach (TreeNode child in node.Nodes)
            {
                ExportNodeRecursive(child, level + 1, lines);
            }
        }

        // ==============================
        // 色を変換16進へ(エクスポート)
        // ==============================
        private string GetColorHex(DrawingColor color)
        {
            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        // ==============================
        // CSVエスケープ
        // ==============================
        private string EscapeCsv(string? text)
        {
            if (string.IsNullOrEmpty(text))
                return "";

            if (text.Contains(",") || text.Contains("\"") || text.Contains("\n"))
            {
                text = text.Replace("\"", "\"\"");
                return $"\"{text}\"";
            }

            return text;
        }

        // ==============================
        // しおりを全て削除
        // ==============================
        private void AllDelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.Nodes.Count == 0)
            {
                MessageBox.Show("削除するしおりがありません。", "しおり無し確認", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (treeView1.Nodes.Count > 0)
            {
                if (MessageBox.Show($"全てのしおりを削除しますか？",
                    "全しおり削除", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    // Noをデフォルトに
                    MessageBoxDefaultButton.Button2) == DialogResult.No)
                    return;

                // ツリービューを初期化
                treeView1.Nodes.Clear();
                // 選択解除（安全）
                treeView1.SelectedNode = null;
                // ツールチップ消す
                treeToolTip.SetToolTip(treeView1, "");
                // 右クリックメニュー更新
                UpdateContextMenuState();

                // 未保存フラグON
                isDirty = true;

            }

        }

        // ==============================
        // ページ回転処理(複数対応)
        // ==============================
        private void RotatePages(string pageText, int addRotation)
        {
            //if (pdfViewer1.Document == null) return;

            if (pdfCustomViewer1.Document == null) return;

            try
            {

                //int currentPage = pdfViewer1.Renderer.Page;

                // 回転後にどこのサムネイルを選択するか、あらかじめターゲットを決めておく
                // 複数選択されている場合もあるので、一番若い（先頭の）選択インデックスを基準にする
                int nextSelectIndex = 0;
                if (pdfThumbnailViewer1.SelectedIndices.Count > 0)
                {
                    // 選択されている最小のインデックス（0始まり）を取得
                    int firstSelectedIdx = pdfThumbnailViewer1.SelectedIndices.Min();

                    // リロード後はページをセットしておく
                    nextSelectIndex = firstSelectedIdx;
                }

                // サムネイルクリア
                pdfThumbnailViewer1.LoadDocument(null);
                // pdfCustomViewer解放
                pdfCustomViewer1.CloseDocument();

                // Viewer解放
                //pdfViewer1.Document.Dispose();
                //pdfViewer1.Document = null;

                string tempPath = workingPath + ".tmp";

                ReaderProperties props = string.IsNullOrEmpty(currentPassword)
                    ? new ReaderProperties()
                    : new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(currentPassword));

                using (var reader = new PdfReader(workingPath, props))
                using (var writer = new PdfWriter(tempPath))
                using (var pdf = new ITextDoc(reader, writer))
                {
                    int total = pdf.GetNumberOfPages();

                    // 構文チェック(PageRangeHelper.csを呼ぶ)して格納
                    var pages = PageRangeHelper.ParsePageRanges(pageText, total);

                    // ページコピー
                    foreach (int p in pages)
                    {
                        var page = pdf.GetPage(p);
                        int rot = page.GetRotation();
                        page.SetRotation((rot + addRotation) % 360);

                    }

                }

                // 一時ファイルを作業用ファイルにコピー
                File.Copy(tempPath, workingPath, true);
                File.Delete(tempPath);

                // 再表示
                var doc = string.IsNullOrEmpty(currentPassword)
                    ? PdfiumDoc.Load(workingPath)
                    : PdfiumDoc.Load(workingPath, currentPassword);

                //pdfViewer1.Document = doc;

                // pdfCustomViewerにPDFを表示
                pdfCustomViewer1.LoadDocument(doc);

                //if (currentPage < doc.PageCount)
                //    pdfViewer1.Renderer.Page = currentPage;

                // サムネイル生成
                pdfThumbnailViewer1.LoadDocument(doc);

                // 自動で選択される1ページ目のサムネイルをリセット
                pdfThumbnailViewer1.ClearSelectionWithoutEvent();

                int SelectedIdx = 0;
                if (_savedSelectedIndices != null && _savedSelectedIndices.Count > 0)
                {
                    // 選択された最初のページ（インデックス）を取得
                    SelectedIdx = _savedSelectedIndices[0];
                }
                else
                {
                    // サムネイル未選択（起動直後など）なら、現在の表示ページを基準にする
                    SelectedIdx = pdfCustomViewer1.CurrentPage;
                }

                // メインビューアをそのページへスクロールさせる場合
                pdfCustomViewer1.ScrollToPage(SelectedIdx);

                if (doc.PageCount > 0)
                {
                    // ターゲットがはみ出る場合の安全ガード
                    int finalSelectIdx = Math.Min(nextSelectIndex, doc.PageCount - 1);
                    // サムネイル側の公開メソッドを呼び出して選択インデックスを上書き
                    pdfThumbnailViewer1.SetSelection(finalSelectIdx);

                }

                // リロード完了後、退避しておいた複数選択状態を復元する
                if (doc.PageCount > 0 && _savedSelectedIndices?.Count > 0)
                {
                    foreach (int selectIdx in _savedSelectedIndices)
                    {
                        // 安全ガード（回転前後のページ数変化トラブル防止）
                        if (selectIdx >= 0 && selectIdx < doc.PageCount)
                        {
                            // サムネイル側の選択メソッドを呼び出す
                            pdfThumbnailViewer1.AddSelectionWithoutClearing(selectIdx);
                        }
                    }
                }

                // PDFの縦横方向に応じた表示
                DisplaySize();

                // 未保存フラグON
                isDirty = true;
            }
            catch (Exception ex)
            {
#if DEBUG
                Extxt.Text = ex.ToString();
                MessageBox.Show("回転エラー:\n" + ex.ToString());
#else
                MessageBox.Show("回転中にエラーが発生しました。", "回転失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
            }
        }

        // ==============================
        // ページを指定して回転フォームを呼ぶ
        // ==============================
        private void RotatePagesSetting_Click(object sender, EventArgs e)
        {
            // PDFが開かれていないなら処理しない
            //if (pdfViewer1.Document == null)
            //{
            //    MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            // PDFが開かれていないなら処理しない
            if (pdfCustomViewer1.Document == null)
            {
                MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // PDF設定情報が存在するか確認
            if (currentSettings == null)
            {
                MessageBox.Show("PDF設定情報の取得に失敗しました。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // どこから呼ばれたか
            string pageText;

            if (sender == RotatePagesSetting || sender == RotatePagesSetting2)
            {
                // メニュー or pdfViewer
                //pageText = (pdfViewer1.Renderer.Page + 1).ToString();
                pageText = (pdfCustomViewer1.CurrentPage + 1).ToString();
            }
            else
            {
                // サムネイル
                pageText = GetSelectedPagesText();
            }


            //int page = pdfViewer1.Renderer.Page + 1;

            // Form7起動
            using (var f = new Form7(pageText, currentSettings.TotalPage))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {

                    RotatePages(f.ExtractText, f.RotationAngle);

                }
            }

        }

        // ==============================
        // ページ削除処理(複数対応)
        // ==============================
        private void DeletePages(string pageText)
        {
            //if (pdfViewer1.Document == null) return;

            if (pdfCustomViewer1.Document == null) return;

            try
            {
                //int currentPage = pdfViewer1.Renderer.Page;
                int currentPage = pdfCustomViewer1.CurrentPage + 1;

                // 削除後にどこのサムネイルを選択するか、あらかじめターゲットを決めておく
                // 複数選択されている場合もあるので、一番若い（先頭の）選択インデックスを基準にする
                int nextSelectIndex = 0;
                if (pdfThumbnailViewer1.SelectedIndices.Count > 0)
                {
                    // 選択されている最小のインデックス（0始まり）を取得
                    int firstSelectedIdx = pdfThumbnailViewer1.SelectedIndices.Min();

                    // 例: 5ページ目(idx:4)を消したら、リロード後は新しい5ページ目(idx:4)を指すようにする
                    nextSelectIndex = firstSelectedIdx;
                }

                // サムネイルクリア
                pdfThumbnailViewer1.LoadDocument(null);
                // pdfCustomViewer解放
                pdfCustomViewer1.CloseDocument();

                // Viewer解放
                //pdfViewer1.Document.Dispose();
                //pdfViewer1.Document = null;

                string tempPath = workingPath + ".tmp";

                ReaderProperties props = string.IsNullOrEmpty(currentPassword)
                    ? new ReaderProperties()
                    : new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(currentPassword));

                using (var reader = new PdfReader(workingPath, props))
                using (var writer = new PdfWriter(tempPath))
                using (var pdf = new ITextDoc(reader, writer))
                {
                    int total = pdf.GetNumberOfPages();

                    // 構文チェック(PageRangeHelper.csを呼ぶ)して格納
                    var pages = PageRangeHelper.ParsePageRanges(pageText, total);

                    // 後ろから削除
                    foreach (int p in pages.OrderByDescending(x => x))
                    {
                        pdf.RemovePage(p);
                    }

                    // しおり補正
                    AdjustBookmarksAfterDelete(treeView1.Nodes, pages);
                }

                // 一時ファイルを作業用ファイルにコピー
                File.Copy(tempPath, workingPath, true);
                File.Delete(tempPath);

                // 再表示
                var doc = string.IsNullOrEmpty(currentPassword)
                    ? PdfiumDoc.Load(workingPath)
                    : PdfiumDoc.Load(workingPath, currentPassword);

                //pdfViewer1.Document = doc;

                // pdfCustomViewerにPDFを表示
                pdfCustomViewer1.LoadDocument(doc);

                // ページ位置補正
                //if (doc.PageCount > 0)
                //{
                //    pdfViewer1.Renderer.Page = Math.Min(currentPage, doc.PageCount - 1);
                //}

                // 右クリックメニュー更新
                UpdateContextMenuState();

                // 保存との整合性 作業用ファイルのデータを入れる
                currentSettings = PdfSettingsLoader.LoadPdfSettings(workingPath, originalPath, currentPassword);
                // ステータスバーにファイル名(元ファイル)と総ページ数
                UpdateStatus(originalPath, currentSettings.TotalPage);

                // サムネイル生成
                pdfThumbnailViewer1.LoadDocument(doc);
                // 自動で選択される1ページ目のサムネイルをリセット
                pdfThumbnailViewer1.ClearSelectionWithoutEvent();

                // リロード完了後、計算しておいたターゲットページを強制選択＆スクロール追従させる
                if (doc.PageCount > 0)
                {
                    // 削除によって総ページ数が減り、ターゲットがはみ出る場合の安全ガード
                    int finalSelectIdx = Math.Min(nextSelectIndex, doc.PageCount - 1);

                    // サムネイル側の公開メソッドを呼び出して選択インデックスを上書き
                    pdfThumbnailViewer1.SetSelection(finalSelectIdx);
                    pdfCustomViewer1.ScrollToPage(finalSelectIdx);
                }

                // PDFの縦横方向に応じた表示
                DisplaySize();

                // 未保存フラグON
                isDirty = true;
            }
            catch (Exception ex)
            {
#if DEBUG
                Extxt.Text = ex.ToString();
                MessageBox.Show("削除エラー:\n" + ex.ToString());
#else
                MessageBox.Show("削除中にエラーが発生しました。", "削除失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
            }
        }

        // ==============================
        // しおりページ補正1（削除対応）
        // ==============================
        private void AdjustBookmarksAfterDelete(TreeNodeCollection nodes, List<int> deletedPages)
        {
            // 削除したページ番号
            deletedPages = deletedPages
                .Distinct() // 重複ページは1つにまとめる
                .OrderBy(x => x) // 昇順に並び替え
                .ToList();

            // しおりリストの後ろから処理
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                // しおりページ補正2
                AdjustNodeAfterDelete(nodes[i], deletedPages);

            }
        }

        // ==============================
        // しおりページ補正2（削除対応）
        // ==============================
        private void AdjustNodeAfterDelete(TreeNode node, List<int> deletedPages)
        {

            // 先に子を処理
            for (int i = node.Nodes.Count - 1; i >= 0; i--)
            {
                // 再帰呼び出し
                AdjustNodeAfterDelete(node.Nodes[i], deletedPages);
            }

            if (node.Tag is BookmarkInfo info)
            {

                // 削除対象ページ
                if (deletedPages.Contains(info.Page))
                {

                    //PromoteChildrenAndRemove(node);
                    // しおり昇格ロジックへ
                    PromoteChildrenAndRemove(node, treeView1.Nodes);
                    return;

                }

                // 自分より前に消えたページ数
                int shift = deletedPages.Count(p => p < info.Page);

                // ページ補正
                info.Page -= shift;
            }
        }

        // ==============================
        // しおり昇格ロジック(挿入、抽出、置換、削除)
        // ==============================
        private void PromoteChildrenAndRemove(TreeNode node, TreeNodeCollection rootNodes)
        {
            TreeNodeCollection parentNodes;

            // 親あり？
            if (node.Parent != null)
            {
                parentNodes = node.Parent.Nodes;
            }
            else
            {
                // ルートノード
                parentNodes = rootNodes;

            }

            int index = parentNodes.IndexOf(node);

            // 子を繰り上げ
            while (node.Nodes.Count > 0)
            {
                TreeNode child = node.Nodes[0];

                node.Nodes.Remove(child);

                parentNodes.Insert(index, child);

                index++;
            }

            // 自分削除
            parentNodes.Remove(node);
        }

        // ==============================
        // ページを指定して削除
        // ==============================
        private void PageDeleteSetting_Click(object sender, EventArgs e)
        {
            // PDFが開かれていないなら処理しない
            //if (pdfViewer1.Document == null)
            //{
            //    MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            // PDFが開かれていないなら処理しない
            if (pdfCustomViewer1.Document == null)
            {
                MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // PDF設定情報が存在するか確認
            if (currentSettings == null)
            {
                MessageBox.Show("PDF設定情報の取得に失敗しました。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // どこから呼ばれたか
            string pageText;

            if (sender == PageDeleteSetting || sender == PageDeleteSetting2)
            {
                // メニュー or pdfViewer
                //pageText = (pdfViewer1.Renderer.Page + 1).ToString();
                pageText = (pdfCustomViewer1.CurrentPage + 1).ToString();
            }
            else
            {
                // サムネイル
                pageText = GetSelectedPagesText();
            }

            // Form8起動
            using (var f = new Form8(pageText, currentSettings.TotalPage))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {

                    DeletePages(f.ExtractText);

                }
            }

        }

        // ==============================
        // 閉じるを押したとき
        // ==============================
        private async void CloseMenu_Click(object sender, EventArgs e)
        {
            // 変更がある場合(未保存確認ダイアログ)
            if (!await ConfirmDiscard())
                // キャンセルの場合は閉じない
                return;

            // 閉じる処理を呼ぶ
            await CloseCurrentPdf();

        }

        // ==============================
        // 閉じる処理
        // ==============================
        private async Task CloseCurrentPdf()
        {

            // サムネイル破棄
            pdfThumbnailViewer1.CloseDocument();

            // Viewer完全リセット
            ResetPdfViewer();

            // 作業用ファイルを破棄(前回PDFの tempファイル削除)
            CleanupWorkingFile();

            ShioriMenu.Enabled = false;
            PageEditMenu.Enabled = false;
            ViewSelect.Enabled = false;

            // 状態リセット
            originalPath = "";
            workingPath = "";
            currentPassword = null;
            currentSettings = null;
            currentSecurity = null;

            // UI
            treeView1.Nodes.Clear();
            Extxt.Text = "";

            this.Text = myName;

            StatusLabel.Text = "ファイル: PDF未選択";
            NewPagetoolStripTextBox.Text = "1";
            TotalPagetoolStripLabel.Text = "/ 1 ";
            toolHintTxt = "ファイル: PDF未選択";

            // 未保存フラグOFF
            isDirty = false;
        }

        // ==============================
        // Viewer完全リセット処理
        // ==============================
        private void ResetPdfViewer()
        {
            // Panel2から外す
            //panel2.Controls.Remove(pdfViewer1);
            // splitContainer1.Panel2から外す
            //splitContainer1.Panel2.Controls.Remove(pdfViewer1);
            //this.Controls.Remove(pdfViewer1);

            // 念のため破棄
            try
            {
                // pdfCustomViewer解放
                pdfCustomViewer1.CloseDocument();

                //pdfViewer1.Document?.Dispose();
                //pdfViewer1.Dispose();
            }
            catch { }

            // 表示が消えないので新しく作り直す
            //pdfViewer1 = new PdfViewer();

            // 設定を再適用
            //pdfViewer1.Dock = DockStyle.Fill;
            // pdfViewerのしおり表示を無効
            //pdfViewer1.ShowBookmarks = false;
            // pdfViewerのツールバー表示を無効
            //pdfViewer1.ShowToolbar = false;
            //pdfViewer1.BorderStyle = BorderStyle.None;
            //pdfViewer1.ContextMenuStrip = contextMenuStrip2;

            // 再追加(splitContainer1.Panel2へ)
            //this.Controls.Add(pdfViewer1);
            //splitContainer1.Panel2.Controls.Add(pdfViewer1);
            //panel2.Controls.Add(pdfViewer1);
            //pdfViewer1.BringToFront();

            // メニューリセット
            MenuReset();

        }

        // ==============================
        // ページ抽出処理(複数対応)
        // ==============================
        private async Task ExtractPagesAsync(string pageText)
        {
            //if (pdfViewer1.Document == null) return;

            if (pdfCustomViewer1.Document == null) return;

            try
            {
                // 保存パス用
                string savePath = "";

                while (true)
                {
                    // 保存ダイアログ
                    using (SaveFileDialog sfd = new SaveFileDialog())
                    {
                        string baseName = IOPath.GetFileNameWithoutExtension(originalPath);
                        sfd.Title = "抽出PDFを保存";
                        sfd.Filter = "PDFファイル (*.pdf)|*.pdf";
                        sfd.FileName = baseName + "_Extract.pdf";

                        // 前回入力したパスを保持
                        if (!string.IsNullOrEmpty(savePath))
                        {
                            sfd.FileName = IOPath.GetFileName(savePath);
                            sfd.InitialDirectory = IOPath.GetDirectoryName(savePath);
                        }

                        // キャンセル
                        if (sfd.ShowDialog() != DialogResult.OK)
                            return;

                        savePath = sfd.FileName;
                    }

                    // 保存できるかチェック(他のアプリで開いてる？)
                    try
                    {
                        // 既に存在する場合だけチェック
                        if (File.Exists(savePath))
                        {
                            // 誰とも共有せず読み書きで開こうとする Acrobatなどで開かれていると失敗する
                            using (var test = new FileStream(savePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                            {
                                // 開けるか確認だけなんで、何もしない
                            }
                        }
                        // チェックOK
                        break;
                    }
                    // ロックされているとここへ
                    catch (IOException)
                    {
                        MessageBox.Show(
                            "PDFファイルが他のアプリで開かれています。" + Environment.NewLine +
                            "ファイル名を変更してください。",
                            "抽出ファイル保存失敗",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        // SaveFileDialogへ戻る
                        continue;
                    }
                }

                await Task.Run(() =>
                {

                    // 読み込みパスワード（今開いてる状態を使う）
                    ReaderProperties props = string.IsNullOrEmpty(currentPassword)
                        ? new ReaderProperties()
                        : new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(currentPassword));

                    using (var reader = new PdfReader(workingPath, props))
                    using (var srcPdf = new ITextDoc(reader))
                    using (var writer = new PdfWriter(savePath))
                    using (var destPdf = new ITextDoc(writer))
                    {
                        int total = srcPdf.GetNumberOfPages();

                        // 構文チェック(PageRangeHelper.csを呼ぶ)して格納
                        var pages = PageRangeHelper.ParsePageRanges(pageText, total);

                        // ページコピー
                        foreach (int p in pages)
                        {
                            srcPdf.CopyPagesTo(p, p, destPdf);
                        }
                        // しおり複製用Tree作成
                        TreeView tempTree = CloneTreeView(treeView1);

                        // しおり抽出
                        //AdjustBookmarksForExtract(startPage, endPage);
                        AdjustBookmarksForExtract(tempTree.Nodes, pages);

                        // PDFへしおり追加
                        var destOutlines = destPdf.GetOutlines(true);
                        foreach (TreeNode node in tempTree.Nodes)
                        {
                            AddOutlineFromNode(destPdf, destOutlines, node);
                        }

                        // メタデータ設定
                        ApplyMetadataAndViewerSettings(destPdf);
                    }

                });

                if (MessageBox.Show("抽出したPDFを既定のPDFアプリで開きますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = savePath,
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
#if DEBUG
                        Extxt.Text = ex.Message;
                        MessageBox.Show("外部アプリで開けませんでした。\n" + ex.Message, "外部アプリオープンエラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
#else
                        MessageBox.Show("外部アプリで開けませんでした。", "外部アプリオープン失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Extxt.Text = ex.ToString();
                MessageBox.Show("抽出エラー:\n" + ex.ToString());
#else
                MessageBox.Show("抽出中にエラーが発生しました。", "抽出失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
            }

        }

        // ==============================
        // しおり抽出処理1
        // ==============================
        private void AdjustBookmarksForExtract(TreeNodeCollection nodes, List<int> extractedPages)
        {
            // 元ページ → 新ページ
            Dictionary<int, int> pageMap = new Dictionary<int, int>();

            for (int i = 0; i < extractedPages.Count; i++)
            {
                // 元 → 新
                pageMap[extractedPages[i]] = i + 1;
            }

            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                AdjustNodeForExtract(nodes[i], pageMap, nodes);
            }
        }

        // ==============================
        // しおり抽出処理2
        // ==============================
        private void AdjustNodeForExtract(TreeNode node, Dictionary<int, int> pageMap, TreeNodeCollection rootNodes)
        {

            // 先に子を処理
            for (int i = node.Nodes.Count - 1; i >= 0; i--)
            {
                AdjustNodeForExtract(node.Nodes[i], pageMap, rootNodes);
            }

            if (node.Tag is BookmarkInfo info)
            {
                // 抽出対象外
                if (!pageMap.ContainsKey(info.Page))
                {
                    // しおり昇格ロジックへ
                    PromoteChildrenAndRemove(node, rootNodes);
                    return;

                }

                // 新ページへ変換
                info.Page = pageMap[info.Page];
            }

        }

        // ==============================
        // しおり抽出処理3(TreeView(しおり)コピー)
        // ==============================
        private TreeView CloneTreeView(TreeView original)
        {
            TreeView clone = new TreeView();

            foreach (TreeNode node in original.Nodes)
            {
                //clone.Nodes.Add((TreeNode)node.Clone());
                clone.Nodes.Add(CloneNode(node));
            }

            return clone;
        }

        // ==============================
        // しおり抽出処理4(TreeNode深コピー)
        // ==============================
        private TreeNode CloneNode(TreeNode source)
        {
            TreeNode newNode = new TreeNode(source.Text);

            // 表示関係
            newNode.ForeColor = source.ForeColor;
            newNode.BackColor = source.BackColor;
            newNode.ImageIndex = source.ImageIndex;
            newNode.SelectedImageIndex = source.SelectedImageIndex;

            if (source.NodeFont != null)
            {
                newNode.NodeFont = (Font)source.NodeFont.Clone();
            }

            // Tagを深コピー
            if (source.Tag is BookmarkInfo info)
            {
                newNode.Tag = new BookmarkInfo
                {
                    BmTitle = info.BmTitle,
                    Page = info.Page,
                    IsOpen = info.IsOpen,
                    SelectedColor = info.SelectedColor,
                    SelectedStyle = info.SelectedStyle
                };
            }

            // 子ノード再帰
            foreach (TreeNode child in source.Nodes)
            {
                newNode.Nodes.Add(CloneNode(child));
            }

            return newNode;
        }

        // ==============================
        // ページを指定して抽出を押したとき
        // ==============================
        private async void PageExtractSetting_Click(object sender, EventArgs e)
        {
            // PDFが開かれていないなら処理しない
            //if (pdfViewer1.Document == null)
            //{
            //    MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            // PDFが開かれていないなら処理しない
            if (pdfCustomViewer1.Document == null)
            {
                MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // PDF設定情報が存在するか確認
            if (currentSettings == null)
            {
                MessageBox.Show("PDF設定情報の取得に失敗しました。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // どこから呼ばれたか
            string pageText;

            //if (sender == PageExtractSetting || sender == PageExtractSetting2)
            //{
            // メニュー or pdfViewer
            //pageText = (pdfViewer1.Renderer.Page + 1).ToString();
            //    pageText = (pdfCustomViewer1.CurrentPage + 1).ToString();
            //}
            //else
            //{
            // サムネイル
            pageText = GetSelectedPagesText();
            //}

            // Form9起動
            using (var f = new Form9(pageText, currentSettings.TotalPage))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        BeginProcessUi("抽出中...");

                        await ExtractPagesAsync(f.ExtractText);
                    }
                    finally
                    {
                        EndProcessUi();
                    }
                }
            }
        }

        // ==============================
        // 挿入を押したとき(ファイルから)
        // ==============================
        private async void PageInsert_Click(object sender, EventArgs e)
        {
            // PDFが開かれていないなら処理しない
            //if (pdfViewer1.Document == null) return;

            // PDFが開かれていないなら処理しない
            if (pdfCustomViewer1.Document == null) return;

            // PDF設定情報が存在するか確認
            if (currentSettings == null)
            {
                // ない
                MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // ファイル選択ダイアログ作成
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    // 表示しているページを取得
                    //int nowPage = pdfViewer1.Renderer.Page + 1;
                    int nowPage = pdfCustomViewer1.CurrentPage + 1;


                    if (sender == PageInsert || sender == PageInsert2)
                    {
                        // メニュー or pdfViewer
                        //nowPage = pdfViewer1.Renderer.Page + 1;
                        nowPage = pdfCustomViewer1.CurrentPage + 1;
                    }
                    else
                    {
                        // サムネイル
                        nowPage = thumbnailRightClickPage;
                    }

                    ofd.Title = "挿入するPDFを選択";
                    ofd.Filter = "PDFファイル (*.pdf)|*.pdf";
                    // ダイアログ表示(キャンセルなら戻る)
                    if (ofd.ShowDialog() != DialogResult.OK)
                        return;
                    // 選択されたPDFのフルパス取得
                    string insertPath = ofd.FileName;

                    // PDFを開いて権限確認へ(パス入力、PDFオープン、権限確認、暗号方式取得)
                    var result = PdfSecurityHelper.CheckPdfPermission(insertPath, InsertPassMessage, () => ShowPasswordDialog(InsertPassMessage));

                    // 開けた？ あかんかったら戻る
                    if (!result.Success)
                        return;

                    // オーナー権限ある？
                    if (!result.IsOwner)
                    {
                        // 権限不足ならメッセージ出す(開くパスやオーナーパスのみでパスワードなしで開いた場合)
                        MessageBox.Show("ページ挿入するには権限パスワードが必要です。", "挿入不可", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 挿入するPDF用の認証情報取得
                    ReaderProperties insertProps = result.ReaderProps;
                    // 挿入するPDFの入力されたパスワード保存
                    string? insertPassword = result.Password;
                    // 挿入するPDF総ページ数格納用
                    int InsTotalPages = 1;

                    // PdfReader を使って PDF を読み込み
                    using (PdfReader reader = new PdfReader(insertPath, insertProps))
                    using (ITextDoc pdfDoc = new ITextDoc(reader))
                    {
                        // 挿入するファイルの総ページ数
                        InsTotalPages = pdfDoc.GetNumberOfPages();
                    }

                    // Form10起動(挿入するPDFパス、開いているPDFのページ番号、開いているPDFの総ページ数、挿入するPDFの総ページ数)
                    using (var f = new Form10(insertPath, nowPage, currentSettings.TotalPage, InsTotalPages))
                    {
                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            // PDF挿入処理へ(挿入するPDFパス、挿入するPDFのページ指定、挿入する場所のページ番号、前 or 後、挿入するPDF用の認証情報、挿入するPDFの入力されたパスワード)
                            await InsertPdfAsync(insertPath, f.ExtractText, f.TargetPage, f.InsertBefore, insertProps, insertPassword);

                        }
                    }

                }
            }
            catch (Exception ex) // エラー捕捉
            {
#if DEBUG
                Extxt.Text = ex.ToString();
                MessageBox.Show("挿入エラー:\n" + ex.ToString());
#else
                MessageBox.Show("挿入中にエラーが発生しました。", "挿入失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
            }
        }

        // ==============================
        // PDF挿入処理（パスあり対応）
        // ==============================
        private async Task InsertPdfAsync(string insertPath, string pageText, int targetPage, bool insertBefore, ReaderProperties insertProps, string? insertPassword)
        {
            // 一時作業用ファイル
            string tempPath = workingPath + ".tmp";

            // パスワード有無判定
            insertProps = string.IsNullOrEmpty(insertPassword)
                ? new ReaderProperties() // パスなし
                : new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(insertPassword)); // パスあり

            try
            {

                // サムネイルクリア
                pdfThumbnailViewer1.LoadDocument(null);
                // pdfCustomViewer解放
                pdfCustomViewer1.CloseDocument();

                // Viewer解放
                //pdfViewer1.Document?.Dispose();
                //pdfViewer1.Document = null;

                /*
                // Viewer解放
                if (pdfViewer1.Document != null)
                {
                    // ファイルロック解除しViewerから切り離す
                    pdfViewer1.Document.Dispose();
                    pdfViewer1.Document = null;
                }

                // Viewer解放
                if (pdfCustomViewer1.Document != null)
                {
                    // サムネイルクリア
                    pdfThumbnailViewer1.LoadDocument(null);

                    // pdfCustomViewer解放
                    pdfCustomViewer1.Dispose();
                }
                */

                //Application.DoEvents();


                int insertPage = 0;
                int insertCount = 0;

                // 表示しているPDFの
                ReaderProperties props = string.IsNullOrEmpty(currentPassword)
                    ? new ReaderProperties()
                    : new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(currentPassword));

                // ページ変換表(しおり補正で使う)
                // 例えば 挿入PDF:3ページ目 メインPDF:8ページ目へ入った なら 3→8 を記録
                Dictionary<int, int> insertPageMap = new Dictionary<int, int>();

                StatusLabel.Text = "ページ挿入中...";
                ProgressBar.Visible = true;
                ProgressBar.Minimum = 0;
                ProgressBar.Value = 0;
                ProgressBar.Style = ProgressBarStyle.Continuous;

                this.Enabled = false;

                await Task.Run(() =>
                {

                    // using は終了時 Dispose 自動実行
                    using (var mainReader = new PdfReader(workingPath, props)) // 開いてるPDFを読込
                    using (var insertReader = new PdfReader(insertPath, insertProps)) // 挿入するPDFを読込
                    using (var writer = new PdfWriter(tempPath)) // 出力するPDFを生成
                    using (var mainPdf = new ITextDoc(mainReader, writer)) // 開いているPDF Document
                    using (var insertPdf = new ITextDoc(insertReader)) // 挿入するPDF Document
                    {
                        // 挿入位置決定
                        // 例えば、 3の前→3、3の後→4
                        insertPage = insertBefore ? targetPage : targetPage + 1;

                        // ページ範囲解析
                        // 例えば、1-3,5の場合、　[1,2,3,5]に .Distinct()重複除去 .OrderBy(x => x)昇順ソート .ToList()リスト化
                        var insertPages = PageRangeHelper.ParsePageRanges(pageText, insertPdf.GetNumberOfPages()).Distinct().OrderBy(x => x).ToList();

                        // 挿入ページ数
                        insertCount = insertPages.Count;

                        this.Invoke(() =>
                        {
                            ProgressBar.Maximum = insertCount;
                        });

                        // コピー先ページ位置
                        int newPagePos = insertPage;

                        int current = 0;

                        // 1ページずつコピー
                        foreach (int p in insertPages)
                        {
                            current++;

                            this.Invoke(() =>
                            {
                                ProgressBar.Value = current;
                                StatusLabel.Text = $"ページ挿入中... {current}/{ProgressBar.Maximum}";
                            });

                            // 挿入するPDF Document(insertPdf)のpページを開いているPDF Document(mainPdf)のnewPagePosへ挿入
                            insertPdf.CopyPagesTo(p, p, mainPdf, newPagePos);
                            // ページ対応記録
                            insertPageMap[p] = newPagePos;
                            // 次ページ位置へ
                            newPagePos++;

                        }
                    }
                });

                // 元しおり(開いているPDF)補正
                // 例えば、4ページ目へ3ページ挿入なら4以降のしおりを+3する
                FixBookmarksForInsert(insertPage, insertCount);

                // 挿入するPDFのしおり取得
                using (var insertReader2 = new PdfReader(insertPath, insertProps))// 挿入するPDF再読込(しおり取得用)
                using (var insertPdfDoc = new ITextDoc(insertReader2))
                {
                    // 挿入するPDFしおり追加
                    ImportBookmarksFromPdf(insertPdfDoc, insertPageMap, treeView1, false);
                    // 不要なしおりを整理　Page=-1ならしおり削除 子が有効ならしおり昇格する
                    CleanupInsertedBookmarks(treeView1.Nodes, insertPageMap);
                }

                // 元PDF削除
                File.Delete(workingPath);
                // tmp → 本ファイル化
                File.Move(tempPath, workingPath);
                //File.Delete(tempPath);


                // 再表示用PDFロード
                var doc = string.IsNullOrEmpty(currentPassword)
                    ? PdfiumDoc.Load(workingPath)
                    : PdfiumDoc.Load(workingPath, currentPassword);
                // PDF表示
                //pdfViewer1.Document = doc;

                // pdfCustomViewerにPDFを表示
                pdfCustomViewer1.LoadDocument(doc);

                // 保存との整合性 作業用ファイルのデータを入れる
                currentSettings = PdfSettingsLoader.LoadPdfSettings(workingPath, originalPath, currentPassword);
                // ステータスバーにファイル名(元ファイル)と総ページ数
                UpdateStatus(originalPath, currentSettings.TotalPage);

                // 右クリックメニュー更新
                UpdateContextMenuState();

                // 挿入先のページを表示
                //pdfViewer1.Renderer.Page = targetPage - 1;

                // サムネイル生成
                pdfThumbnailViewer1.LoadDocument(doc);

                pdfCustomViewer1.ScrollToPage(insertPage - 1);
                // サムネイル側の公開メソッドを呼び出して選択インデックスを上書き
                pdfThumbnailViewer1.SetSelection(insertPage - 1);

                // リロード完了後、計算しておいたターゲットページを強制選択＆スクロール追従させる
                //if (doc.PageCount > 0)
                //{
                // 挿入よりターゲットがはみ出る場合の安全ガード
                //    int finalSelectIdx = Math.Min(targetPage, doc.PageCount - 1);

                // サムネイル側の公開メソッドを呼び出して選択インデックスを上書き
                //    pdfThumbnailViewer1.SetSelection(finalSelectIdx);

                //}

                // 未保存フラグON
                isDirty = true;
            }
            catch (Exception ex) // エラー捕捉
            {
#if DEBUG
                Extxt.Text = ex.ToString();
                MessageBox.Show("挿入エラー:\n" + ex.ToString());
#else
                MessageBox.Show("挿入中にエラーが発生しました。", "挿入失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
            }
            finally
            {
                StatusLabel.Text = toolHintTxt;
                ProgressBar.Value = 0;
                ProgressBar.Visible = false;
                this.Enabled = true;
            }
        }

        // ==============================
        // しおりページ補正（挿入用）1
        // ==============================
        private void FixBookmarksForInsert(int insertPage, int insertCount)
        {
            foreach (TreeNode node in treeView1.Nodes)
            {
                FixNode(node, insertPage, insertCount);
            }
        }

        // ==============================
        // しおりページ補正（挿入用）2
        // ==============================
        private void FixNode(TreeNode node, int insertPage, int insertCount)
        {
            if (node.Tag is BookmarkInfo info)
            {
                // 挿入位置以降はズラす
                if (info.Page >= insertPage)
                {
                    info.Page += insertCount;
                }
            }

            // 子ノード再帰
            foreach (TreeNode child in node.Nodes)
            {
                FixNode(child, insertPage, insertCount);
            }
        }

        // ==============================
        // しおりをTreeViewへ追加(挿入、置換用)1
        // ==============================
        private void ImportBookmarksFromPdf(ITextDoc insertPdf, Dictionary<int, int> pageMap, TreeView treeView, bool isReplace)
        {

            var outlines = insertPdf.GetOutlines(false);

            if (outlines == null)
                return;

            // 挿入開始ページ
            int firstInsertPage = pageMap.Values.Min();

            // 「挿入直前ページ」のしおりを探す
            TreeNode? baseNode = FindBookmarkByPage(treeView.Nodes, firstInsertPage - 1);

            // 追加先
            TreeNodeCollection targetNodes;

            // 挿入Index
            int insertIndex;


            // 挿入先決定
            if (baseNode != null)
            {
                // 子がある場合
                if (baseNode.Nodes.Count > 0)
                {
                    // 子階層へ入れる
                    targetNodes = baseNode.Nodes;

                    // 先頭へ
                    insertIndex = 0;
                }
                else
                {
                    // 子なし → 同階層の次へ
                    if (baseNode.Parent != null)
                    {
                        targetNodes = baseNode.Parent.Nodes;
                    }
                    else
                    {
                        targetNodes = treeView.Nodes;
                    }

                    insertIndex = targetNodes.IndexOf(baseNode) + 1;
                }
            }
            else
            {
                // fallback
                targetNodes = treeView.Nodes;

                insertIndex = FindInsertIndex(treeView.Nodes, firstInsertPage);
            }

            // 完全Tree生成
            foreach (var root in outlines.GetAllChildren())
            {
                var node = CreateNodeFromOutline(insertPdf, root, pageMap, isReplace);

                if (node != null)
                {
                    targetNodes.Insert(insertIndex, node);

                    insertIndex++;
                }
            }

        }

        // ==============================
        // 指定ページのしおり取得
        // ==============================
        private TreeNode? FindBookmarkByPage(TreeNodeCollection nodes, int page)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Tag is BookmarkInfo info)
                {
                    if (info.Page == page)
                        return node;
                }

                var child = FindBookmarkByPage(node.Nodes, page);

                if (child != null)
                    return child;
            }

            return null;
        }

        // ==============================
        // しおりをTreeViewへ追加(挿入、置換用)2
        // ==============================
        private int FindInsertIndex(TreeNodeCollection nodes, int insertPage)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Tag is BookmarkInfo info)
                {
                    if (info.Page >= insertPage)
                        return i;
                }
            }
            return nodes.Count;
        }

        // ==============================
        // しおりをTreeViewへ追加(挿入、置換用)3
        // ==============================
        private TreeNode? CreateNodeFromOutline(ITextDoc pdf, PdfOutline outline, Dictionary<int, int> pageMap, bool isReplace)
        {

            // isReplace true:置換、false:挿入
            try
            {
                int page = 0;

                string title = outline.GetTitle();

                // Outlineの生データ取得（これが本体）
                var dict = outline.GetContent();

                PdfObject? destObj = null;

                // /Dest を優先取得
                if (dict.ContainsKey(PdfName.Dest))
                {
                    destObj = dict.Get(PdfName.Dest);
                }

                // /A（Action）から取得
                else if (dict.ContainsKey(PdfName.A))
                {
                    var action = dict.GetAsDictionary(PdfName.A);
                    if (action != null && action.ContainsKey(PdfName.D))
                    {
                        destObj = action.Get(PdfName.D);
                    }
                }

                if (destObj is PdfArray arr && arr.Size() > 0)
                {
                    var pageDict = arr.GetAsDictionary(0);

                    if (pageDict != null)
                    {
                        var pageObj = pdf.GetPage(pageDict);
                        page = pdf.GetPageNumber(pageObj);
                    }
                }
                // Named Destination（文字列）
                else if (destObj is PdfString name)
                {
                    var nameTree = pdf.GetCatalog().GetNameTree(PdfName.Dests);
                    var names = nameTree.GetNames();

                    // （string → PdfString）
                    var key = name;

                    if (names.ContainsKey(key))
                    {
                        var obj = names[key] as PdfArray;

                        if (obj != null && obj.Size() > 0)
                        {
                            var pageDict = obj.GetAsDictionary(0);

                            if (pageDict != null)
                            {
                                var pageObj = pdf.GetPage(pageDict);
                                page = pdf.GetPageNumber(pageObj);
                            }
                        }
                    }
                }

                // 挿入位置に合わせて補正
                int newPage;

                if (isReplace)
                {
                    // 置換
                    // 置換位置に合わせて補正
                    newPage = -1;

                    // pageMap にあるページだけ有効
                    if (pageMap.ContainsKey(page))
                    {
                        newPage = pageMap[page];
                    }

                }
                else
                {
                    // 挿入
                    // 挿入位置に合わせて補正
                    newPage = -1;

                    // pageMap にあるページだけ有効
                    if (pageMap.ContainsKey(page))
                    {
                        newPage = pageMap[page];
                    }

                }

                bool isOpen = true;

                var countObj = dict.GetAsNumber(PdfName.Count);

                if (countObj != null)
                {
                    isOpen = countObj.IntValue() >= 0;
                }

                // 文字の色
                // デフォルト
                DrawingColor selectedColor = DrawingColor.Black;

                var color = outline.GetColor();

                if (color != null)
                {
                    // iTextのColor → RGB取得
                    var rgb = color.GetColorValue(); // ←これがfloat[]

                    if (rgb != null && rgb.Length >= 3)
                    {
                        selectedColor = DrawingColor.FromArgb(
                            (int)(rgb[0] * 255),
                            (int)(rgb[1] * 255),
                            (int)(rgb[2] * 255)
                        );
                    }
                }

                // 文字のスタイル
                int style = outline.GetStyle() ?? 0;

                FontStyle fontStyle = FontStyle.Regular;

                // 両方通るとボールドイタリックになるはず
                // ボールド
                if ((style & PdfOutline.FLAG_BOLD) != 0)
                    fontStyle |= FontStyle.Bold;
                // イタリック
                if ((style & PdfOutline.FLAG_ITALIC) != 0)
                    fontStyle |= FontStyle.Italic;

                FontStyle selectedStyle = fontStyle;

                var node = new TreeNode(title)
                {
                    Tag = new BookmarkInfo
                    {
                        // しおり名
                        BmTitle = title,
                        //　ページ番号
                        Page = newPage,
                        // 展開 or 縮小
                        IsOpen = isOpen,
                        // 色
                        SelectedColor = selectedColor,
                        // スタイル
                        SelectedStyle = selectedStyle

                    }
                };

                // UIに反映
                node.ForeColor = selectedColor;
                node.NodeFont = new Font(treeView1.Font, selectedStyle);

                // 子も再帰
                foreach (var child in outline.GetAllChildren())
                {
                    var childNode = CreateNodeFromOutline(pdf, child, pageMap, isReplace);
                    if (childNode != null)
                        node.Nodes.Add(childNode);
                }

                return node;
            }
            catch
            {
                return null;
            }
        }

        // ==============================
        // 挿入しおり整理
        // ==============================
        private void CleanupInsertedBookmarks(TreeNodeCollection nodes, Dictionary<int, int> pageMap)
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                CleanupInsertedNode(nodes[i], nodes, pageMap);
            }
        }

        // ==============================
        // 挿入しおり整理（再帰）
        // ==============================
        private void CleanupInsertedNode(TreeNode node, TreeNodeCollection rootNodes, Dictionary<int, int> pageMap)
        {
            // 先に子
            for (int i = node.Nodes.Count - 1; i >= 0; i--)
            {
                CleanupInsertedNode(node.Nodes[i], rootNodes, pageMap);
            }

            if (node.Tag is BookmarkInfo info)
            {
                // Page=-1 → 対象外
                if (info.Page == -1)
                {
                    PromoteChildrenAndRemove(node, rootNodes);
                }
            }
        }

        // ==============================
        // ページ移動
        // ==============================
        private void PageMove_Click(object sender, EventArgs e)
        {
            // PDFが開かれていないなら処理しない
            //if (pdfViewer1.Document == null)
            //{
            //    MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            // PDFが開かれていないなら処理しない
            if (pdfCustomViewer1.Document == null)
            {
                MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // PDF設定情報が存在するか確認
            if (currentSettings == null)
            {
                MessageBox.Show("PDF設定情報の取得に失敗しました。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // どこから呼ばれたか
            string pageText;

            //if (sender == PageMove || sender == PageMove2)
            //{
            // メニュー or pdfViewer
            //pageText = (pdfViewer1.Renderer.Page + 1).ToString();
            //    pageText = (pdfCustomViewer1.CurrentPage + 1).ToString();
            //}
            //else
            //{
            // サムネイル
            pageText = GetSelectedPagesText();
            //}

            // Form11起動
            using (var f = new Form11(pageText, currentSettings.TotalPage))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {

                    MovePdfPage(f.ExtractText, f.TargetPage, f.MoveBefore);
                }
            }

        }

        // ==============================
        // ページ移動処理
        // ==============================
        private void MovePdfPage(string pageText, int target, bool before)
        {
            string tempPath = workingPath + ".tmp";

            Dictionary<int, int> pageMap = new Dictionary<int, int>();

            List<int> movePages = new List<int>();

            // 移動先決定
            int insertIndex = target;

            try
            {
                // サムネイルクリア
                pdfThumbnailViewer1.LoadDocument(null);
                // pdfCustomViewer解放
                pdfCustomViewer1.CloseDocument();

                // Viewer解放
                //pdfViewer1.Document?.Dispose();
                //pdfViewer1.Document = null;

                ReaderProperties props = string.IsNullOrEmpty(currentPassword)
                    ? new ReaderProperties()
                    : new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(currentPassword));

                using (var reader = new PdfReader(workingPath, props))
                using (var writer = new PdfWriter(tempPath))
                using (var srcPdf = new ITextDoc(reader))
                using (var destPdf = new ITextDoc(writer))
                {
                    int total = srcPdf.GetNumberOfPages();

                    // ページ解析
                    movePages = PageRangeHelper.ParsePageRanges(pageText, total).Distinct().OrderBy(x => x).ToList();

                    if (movePages.Count == 0)
                        return;

                    HashSet<int> moveSet = new HashSet<int>(movePages);

                    // 移動先決定
                    insertIndex = before ? target : target + 1;

                    // 移動対象の直前補正
                    int beforeCount = movePages.Count(p => p < insertIndex);

                    insertIndex -= beforeCount;

                    if (insertIndex < 1)
                        insertIndex = 1;

                    int currentPos = 1;

                    for (int i = 1; i <= total; i++)
                    {
                        // 挿入位置
                        if (currentPos == insertIndex)
                        {
                            foreach (int p in movePages)
                            {
                                srcPdf.CopyPagesTo(p, p, destPdf);
                                pageMap[p] = currentPos++;
                            }
                        }

                        // 移動対象はスキップ
                        if (moveSet.Contains(i))
                            continue;

                        srcPdf.CopyPagesTo(i, i, destPdf);

                        pageMap[i] = currentPos++;
                    }

                    // 最後尾挿入
                    if (currentPos <= total)
                    {
                        foreach (int p in movePages)
                        {
                            if (!pageMap.ContainsKey(p))
                            {
                                srcPdf.CopyPagesTo(p, p, destPdf);
                                pageMap[p] = currentPos++;
                            }
                        }
                    }

                }

                // しおり補正
                FixBookmarksForMove(pageMap);

                // 上書き
                File.Delete(workingPath);
                File.Move(tempPath, workingPath);
                File.Delete(tempPath);

                // 再表示
                var doc = string.IsNullOrEmpty(currentPassword)
                    ? PdfiumDoc.Load(workingPath)
                    : PdfiumDoc.Load(workingPath, currentPassword);

                //pdfViewer1.Document = doc;

                // pdfCustomViewerにPDFを表示
                pdfCustomViewer1.LoadDocument(doc);

                // 保存との整合性 作業用ファイルのデータを入れる
                currentSettings = PdfSettingsLoader.LoadPdfSettings(workingPath, originalPath, currentPassword);
                // ステータスバーにファイル名(元ファイル)と総ページ数
                UpdateStatus(originalPath, currentSettings.TotalPage);

                // 移動先を表示
                //pdfViewer1.Renderer.Page = insertIndex - 1;

                // サムネイル生成
                pdfThumbnailViewer1.LoadDocument(doc);
                // 自動で選択される1ページ目のサムネイルをリセット
                pdfThumbnailViewer1.ClearSelectionWithoutEvent();

                // サムネイル側の公開メソッドを呼び出して選択インデックスを上書き
                pdfThumbnailViewer1.SetSelection(insertIndex - 1);


                if (doc.PageCount > 0 && movePages.Count > 0)
                {
                    // 移動元となった各ページ（1始まり）が、移動後に何番になったかを調べて復元
                    foreach (int oldPageNum in movePages)
                    {
                        // pageMapに存在する場合、そこから移動後の「1始まりページ番号」を取得
                        if (pageMap.TryGetValue(oldPageNum, out int newPageNum))
                        {
                            // サムネイル用に「0始まりインデックス」に変換
                            int newSelectIdx = newPageNum - 1;

                            // 安全ガード
                            if (newSelectIdx >= 0 && newSelectIdx < doc.PageCount)
                            {
                                // 移動後の位置を順番に買い足して選択状態にする
                                pdfThumbnailViewer1.AddSelectionWithoutClearing(newSelectIdx);
                            }
                        }
                    }

                }

                // 未保存フラグON
                isDirty = true;
            }
            catch (Exception ex)
            {
#if DEBUG
                Extxt.Text = ex.ToString();
                MessageBox.Show("移動エラー:\n" + ex.ToString());
#else
                MessageBox.Show("移動中にエラーが発生しました。", "移動失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
            }
        }

        // ==============================
        // ページ移動(しおり補正1)
        // ==============================
        private void FixBookmarksForMove(Dictionary<int, int> pageMap)
        {
            foreach (TreeNode node in treeView1.Nodes)
            {
                FixNodeMove(node, pageMap);
            }
        }

        // ==============================
        // ページ移動(しおり補正2)
        // ==============================
        private void FixNodeMove(TreeNode node, Dictionary<int, int> pageMap)
        {
            if (node.Tag is BookmarkInfo info)
            {
                if (pageMap.ContainsKey(info.Page))
                {
                    info.Page = pageMap[info.Page];
                }
            }

            foreach (TreeNode child in node.Nodes)
            {
                FixNodeMove(child, pageMap);
            }
        }

        // ==============================
        // 置換を押したとき
        // ==============================
        private async void ReplacementMenu_Click(object sender, EventArgs e)
        {
            // PDFが開かれていないなら処理しない
            //if (pdfViewer1.Document == null)
            //{
            //    MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            // PDFが開かれていないなら処理しない
            if (pdfCustomViewer1.Document == null)
            {
                MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // PDF設定情報が存在するか確認
            if (currentSettings == null)
            {
                MessageBox.Show("PDF設定情報の取得に失敗しました。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Title = "置換するPDFを選択";
                    ofd.Filter = "PDFファイル (*.pdf)|*.pdf";

                    if (ofd.ShowDialog() != DialogResult.OK)
                        return;

                    string replacementPath = ofd.FileName;

                    // PDFを開いて権限確認へ(パス入力、PDFオープン、権限確認、暗号方式取得)
                    var result = PdfSecurityHelper.CheckPdfPermission(replacementPath, OkikaePassMessage, () => ShowPasswordDialog(OkikaePassMessage));

                    if (!result.Success)
                        return;

                    if (!result.IsOwner)
                    {
                        MessageBox.Show("ページ置換するには権限パスワードが必要です。", "置換不可", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    ReaderProperties insertProps = result.ReaderProps;
                    string? insertPassword = result.Password;

                    int InsTotalPages = 1;

                    // PdfReader を使って PDF を読み込み
                    using (PdfReader reader = new PdfReader(replacementPath, insertProps))
                    using (ITextDoc pdfDoc = new ITextDoc(reader))
                    {
                        // 置換するファイルの総ページ数
                        InsTotalPages = pdfDoc.GetNumberOfPages();
                    }

                    // どこから呼ばれたか
                    string pageText;

                    //if (sender == ReplacementMenu || sender == ReplacementMenu2)
                    //{
                    // メニュー or pdfViewer
                    //pageText = (pdfViewer1.Renderer.Page + 1).ToString();
                    //    pageText = (pdfCustomViewer1.CurrentPage + 1).ToString();
                    //}
                    //else
                    //{
                    // サムネイル
                    pageText = GetSelectedPagesText();
                    //}

                    // Form12起動
                    using (var f = new Form12(currentSettings.PdfFileName ?? "", replacementPath, pageText, currentSettings.TotalPage, InsTotalPages))
                    {
                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            await ReplacementPdfAsync(replacementPath, f.ExtractText, f.TargetStartPage, f.TargetEndPage, insertProps, insertPassword);

                        }
                    }

                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Extxt.Text = ex.ToString();
                MessageBox.Show("置換エラー:\n" + ex.ToString());
#else
                MessageBox.Show("置換中にエラーが発生しました。", "置換失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
            }
        }

        // ==============================
        // ページ置換処理
        // ==============================
        private async Task ReplacementPdfAsync(string okikaePath, string pageText, int start, int end, ReaderProperties okikaeProps, string? okikaePassword)
        {

            string tempPath = workingPath + ".tmp";

            okikaeProps = string.IsNullOrEmpty(okikaePassword)
                ? new ReaderProperties()
                : new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(okikaePassword));

            int repCount = 0;

            try
            {
                // サムネイルクリア
                pdfThumbnailViewer1.LoadDocument(null);
                // pdfCustomViewer解放
                pdfCustomViewer1.CloseDocument();

                //pdfViewer1.Document?.Dispose();
                //pdfViewer1.Document = null;

                ReaderProperties props = string.IsNullOrEmpty(currentPassword)
                    ? new ReaderProperties()
                    : new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(currentPassword));

                Dictionary<int, int> replacePageMap = new Dictionary<int, int>();

                StatusLabel.Text = "ページ置換中...";
                ProgressBar.Visible = true;
                ProgressBar.Minimum = 0;
                ProgressBar.Value = 0;
                ProgressBar.Style = ProgressBarStyle.Continuous;

                this.Enabled = false;

                await Task.Run(() =>
                {
                    using (var mainReader = new PdfReader(workingPath, props))
                    using (var repReader = new PdfReader(okikaePath, okikaeProps))
                    using (var writer = new PdfWriter(tempPath))
                    using (var mainPdf = new ITextDoc(mainReader))
                    using (var repPdf = new ITextDoc(repReader))
                    using (var destPdf = new ITextDoc(writer))
                    {
                        int total = mainPdf.GetNumberOfPages();

                        var replacePages = PageRangeHelper.ParsePageRanges(pageText, repPdf.GetNumberOfPages()).Distinct().OrderBy(x => x).ToList();

                        repCount = replacePages.Count;

                        this.Invoke(() =>
                        {
                            ProgressBar.Maximum = total;
                        });

                        int current = 0;

                        for (int i = 1; i <= total; i++)
                        {
                            current++;

                            this.Invoke(() =>
                            {
                                ProgressBar.Value = current;
                                StatusLabel.Text = $"ページ置換中... {current}/{ProgressBar.Maximum}";
                            });

                            // 置換開始位置
                            if (i == start)
                            {
                                // 置換PDFをコピー
                                int destPage = start;

                                foreach (int p in replacePages)
                                {
                                    repPdf.CopyPagesTo(p, p, destPdf);

                                    replacePageMap[p] = destPage;

                                    destPage++;
                                }
                            }

                            // 置換対象はスキップ
                            if (i >= start && i <= end)
                                continue;

                            // 通常コピー
                            mainPdf.CopyPagesTo(i, i, destPdf);
                        }
                    }
                });

                // 置換PDFのしおり有無確認
                using (var repReader2 = new PdfReader(okikaePath, okikaeProps))
                using (var repPdfDoc = new ITextDoc(repReader2))
                {
                    bool hasBookmarks = HasBookmarks(repPdfDoc);

                    // 置換PDFにしおりあり
                    if (hasBookmarks)
                    {
                        // 元しおり削除
                        RemoveBookmarksInRange(start, end);

                        // 後続ページ補正
                        ShiftBookmarksAfter(start, end, repCount);

                        // 新しおり追加
                        ImportBookmarksFromPdf(
                            repPdfDoc,
                            replacePageMap,
                            treeView1,
                            true);

                        // Page=-1 を昇格
                        CleanupImportedBookmarks(treeView1.Nodes);
                    }

                    // 置換PDFにしおりなし
                    else
                    {
                        ShiftBookmarksAfter(start, end, repCount);
                    }

                }

                // 上書き
                File.Delete(workingPath);
                File.Move(tempPath, workingPath);
                //File.Delete(tempPath);

                // 再表示
                var doc = string.IsNullOrEmpty(currentPassword)
                    ? PdfiumDoc.Load(workingPath)
                    : PdfiumDoc.Load(workingPath, currentPassword);

                //pdfViewer1.Document = doc;

                // pdfCustomViewerにPDFを表示
                pdfCustomViewer1.LoadDocument(doc);

                // 保存との整合性 作業用ファイルのデータを入れる
                currentSettings = PdfSettingsLoader.LoadPdfSettings(workingPath, originalPath, currentPassword);
                // ステータスバーにファイル名(元ファイル)と総ページ数
                UpdateStatus(originalPath, currentSettings.TotalPage);

                // 右クリックメニュー更新
                UpdateContextMenuState();

                // 置換ページの先頭を表示
                //pdfViewer1.Renderer.Page = start - 1;

                // サムネイル生成
                pdfThumbnailViewer1.LoadDocument(doc);

                // リロード完了後、計算しておいたターゲットページを強制選択＆スクロール追従させる
                if (doc.PageCount > 0)
                {
                    // 置換にてターゲットがはみ出る場合の安全ガード
                    int finalSelectIdx = Math.Min(start - 1, doc.PageCount - 1);

                    // サムネイル側の公開メソッドを呼び出して選択インデックスを上書き
                    pdfThumbnailViewer1.SetSelection(finalSelectIdx);

                }

                // 未保存フラグON
                isDirty = true;
            }
            catch (Exception ex)
            {
#if DEBUG
                Extxt.Text = ex.ToString();
                MessageBox.Show("置換エラー:\n" + ex.ToString());
#else
                MessageBox.Show("置換中にエラーが発生しました。", "置換失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
            }
            finally
            {

                StatusLabel.Text = toolHintTxt;
                ProgressBar.Value = 0;
                ProgressBar.Visible = false;
                this.Enabled = true;
            }
        }

        // ==============================
        // ページ置換処理()1
        // ==============================
        private void CleanupImportedBookmarks(TreeNodeCollection nodes)
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                CleanupNode(nodes[i], nodes);
            }
        }

        // ==============================
        // ページ置換処理()2
        // ==============================
        private void CleanupNode(TreeNode node, TreeNodeCollection rootNodes)
        {
            // 先に子
            for (int i = node.Nodes.Count - 1; i >= 0; i--)
            {
                CleanupNode(node.Nodes[i], rootNodes);
            }

            if (node.Tag is BookmarkInfo info)
            {
                // Page=-1 は対象外
                if (info.Page == -1)
                {
                    PromoteChildrenAndRemove(node, rootNodes);
                }
            }
        }

        // ==============================
        // ページ置換処理(PDFにしおりが存在するか)
        // ==============================
        private bool HasBookmarks(ITextDoc pdf)
        {
            var outlines = pdf.GetOutlines(false);

            if (outlines == null)
                return false;

            return outlines.GetAllChildren().Count > 0;
        }

        // ==============================
        // ページ置換処理(しおり補正1)
        // ==============================
        private void RemoveBookmarksInRange(int start, int end)
        {
            for (int i = treeView1.Nodes.Count - 1; i >= 0; i--)
            {
                RemoveNode(treeView1.Nodes[i], start, end);
            }
        }

        // ==============================
        // ページ置換処理(しおり補正2)
        // ==============================
        private void RemoveNode(TreeNode node, int start, int end)
        {
            // 先に子
            for (int i = node.Nodes.Count - 1; i >= 0; i--)
            {
                RemoveNode(node.Nodes[i], start, end);
            }

            if (node.Tag is BookmarkInfo info)
            {
                if (info.Page >= start && info.Page <= end)
                {
                    PromoteChildrenAndRemove(node, treeView1.Nodes);
                }
            }

        }

        // ==============================
        // ページ置換処理(しおり補正3)
        // ==============================
        private void ShiftBookmarksAfter(int start, int end, int repCount)
        {
            int removed = end - start + 1;
            int diff = repCount - removed;

            foreach (TreeNode node in treeView1.Nodes)
            {
                ShiftNode(node, start, end, diff);
            }
        }

        // ==============================
        // ページ置換処理(しおり補正4)
        // ==============================
        private void ShiftNode(TreeNode node, int start, int end, int diff)
        {

            if (node.Tag is BookmarkInfo info)
            {
                if (info.Page > end)
                {
                    info.Page += diff;
                }
            }

            foreach (TreeNode child in node.Nodes)
            {
                ShiftNode(child, start, end, diff);
            }
        }

        // ==============================
        // 画像をPDFに変換を押したとき
        // ==============================
        private async void ConvPdf_Click(object sender, EventArgs e)
        {
            // awaitを付けて非同期メソッドを呼び出す
            await ImageConvPdf();
        }

        // ==============================
        // 画像をPDFに変換処理
        // ==============================
        private async Task ImageConvPdf()
        {

            // 変更がある場合(未保存確認ダイアログ)
            if (!await ConfirmDiscard())
                // キャンセルの場合は開かない
                return;

            try
            {
                // ファイル選択ダイアログを作成
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Title = "PDFに変換する画像を選択";
                    //画像ファイル
                    ofd.Filter = "画像ファイル (*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff)|*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff";
                    // 複数選択
                    ofd.Multiselect = true;
                    // ダイアログ表示(キャンセルなら戻る)
                    if (ofd.ShowDialog() != DialogResult.OK)
                        return;

                    // 画像PDFのサイズ設定
                    // Form13起動
                    using (var f = new Form13())
                    {
                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            PdfImageMode = f.PdfImageMode;
                            PdfPlace = f.PdfPlace;
                            // 余白 単位はpt 1mm = 約2.83pt
                            PdfMarginTop = f.PdfMarginTop * 2.83f;
                            PdfMarginBottom = f.PdfMarginBottom * 2.83f;
                            PdfMarginLeft = f.PdfMarginLeft * 2.83f;
                            PdfMarginRight = f.PdfMarginRight * 2.83f;
                        }
                    }

                    // 保存先選択
                    using (SaveFileDialog sfd = new SaveFileDialog())
                    {
                        sfd.Title = "保存先を選択";
                        sfd.Filter = "PDFファイル (*.pdf)|*.pdf";
                        //sfd.FileName = "NewPDF.pdf";
                        // 最初の画像をベースにファイル名を設定
                        string firstName = IOPath.GetFileNameWithoutExtension(ofd.FileNames[0]);
                        sfd.FileName = firstName + ".pdf";

                        // 保存ダイアログ(キャンセルなら戻る)
                        if (sfd.ShowDialog() != DialogResult.OK)
                            return;

                        // 未保存フラグOFF
                        isDirty = false;

                        // 閉じる処理を呼ぶ
                        await CloseCurrentPdf();

                        // 念のためしおり消す
                        treeView1.Nodes.Clear();

                        // リスト化する
                        var files = ofd.FileNames.OrderBy(x => x, new NaturalStringComparer()).ToList();

                        //int pageNum = 1;

                        StatusLabel.Text = "PDF変換中...";
                        ProgressBar.Visible = true;
                        ProgressBar.Minimum = 0;
                        ProgressBar.Maximum = files.Count;
                        ProgressBar.Value = 0;
                        ProgressBar.Style = ProgressBarStyle.Continuous;

                        this.Enabled = false;

                        await Task.Run(() =>
                        {

                            // PDF作成
                            using (PdfWriter writer = new PdfWriter(sfd.FileName))
                            using (ITextDoc pdf = new ITextDoc(writer))
                            using (iText.Layout.Document document = new iText.Layout.Document(pdf))
                            {

                                var outlines = pdf.GetOutlines(true);
                                outlines.SetOpen(true);

                                this.Invoke(() =>
                                {
                                    treeView1.BeginUpdate();
                                });

                                try
                                {

                                    int current = 0;

                                    foreach (string imagePath in files)
                                    {
                                        current++;
                                        // 【プログレスバーテスト用】あえて3秒間（3000ミリ秒）スレッドを待機させる
                                        //System.Threading.Tasks.Task.Delay(3000).Wait();

                                        this.Invoke(() =>
                                        {
                                            ProgressBar.Value = current;
                                            StatusLabel.Text = $"PDF変換中... {current}/{ProgressBar.Maximum}";
                                        });

                                        // 画像読み込み
                                        ImageData imageData = ImageDataFactory.Create(imagePath);

                                        // Image作成
                                        ITextImage image = new ITextImage(imageData);

                                        // ページサイズ取得
                                        float imgWidth = image.GetImageWidth();
                                        float imgHeight = image.GetImageHeight();

                                        // ページサイズを設定
                                        PageSize pageSize;
                                        switch (PdfImageMode)
                                        {
                                            // A4縦
                                            case 0:
                                                pageSize = PageSize.A4;
                                                break;
                                            // A4横
                                            case 1:
                                                pageSize = PageSize.A4.Rotate();
                                                break;
                                            //元サイズ
                                            default:
                                                pageSize = new PageSize(imgWidth + PdfMarginLeft + PdfMarginRight, imgHeight + PdfMarginTop + PdfMarginBottom);
                                                break;
                                        }

                                        // 新規ページ追加
                                        pdf.AddNewPage(pageSize);

                                        // 設定したサイズに収める
                                        float pageWidth = pageSize.GetWidth();
                                        float pageHeight = pageSize.GetHeight();

                                        // 余白ゼロ(上、右、下、左)
                                        //document.SetMargins(0, 0, 0, 0);

                                        // 画像が使える範囲
                                        float availableWidth = pageWidth - PdfMarginLeft - PdfMarginRight;
                                        float availableHeight = pageHeight - PdfMarginTop - PdfMarginBottom;

                                        // 元サイズ以外の場合
                                        if (PdfImageMode != 2)
                                        {
                                            // 画像を余白内に収める
                                            image.ScaleToFit(availableWidth, availableHeight);
                                        }

                                        // 画像縮小後のサイズ取得
                                        float scaledWidth = image.GetImageScaledWidth();
                                        float scaledHeight = image.GetImageScaledHeight();

                                        float x = PdfMarginLeft;
                                        float y = PdfMarginBottom;

                                        // 配置設定
                                        switch (PdfPlace)
                                        {
                                            // 中央
                                            case 0:
                                                x = PdfMarginLeft + (availableWidth - scaledWidth) / 2;
                                                y = PdfMarginBottom + (availableHeight - scaledHeight) / 2;
                                                break;

                                            // 上詰め
                                            case 1:
                                                x = PdfMarginLeft + (availableWidth - scaledWidth) / 2;
                                                y = pageHeight - PdfMarginTop - scaledHeight;
                                                break;

                                            // 下詰め
                                            case 2:
                                                x = PdfMarginLeft + (availableWidth - scaledWidth) / 2;
                                                y = PdfMarginBottom;
                                                break;

                                            // 左詰め
                                            case 3:
                                                x = PdfMarginLeft;
                                                y = PdfMarginBottom + (availableHeight - scaledHeight) / 2;
                                                break;

                                            // 右詰め
                                            case 4:
                                                x = pageWidth - PdfMarginRight - scaledWidth;
                                                y = PdfMarginBottom + (availableHeight - scaledHeight) / 2;
                                                break;
                                        }

                                        // pdf.GetNumberOfPages()はページ番号、xyは左下が原点(0,0)で、xは右へ、yは上へ
                                        image.SetFixedPosition(pdf.GetNumberOfPages(), x, y);

                                        // 追加
                                        document.Add(image);

                                        // ファイル名をしおりに
                                        string bookmarkName = IOPath.GetFileNameWithoutExtension(imagePath);

                                        PdfOutline outline = outlines.AddOutline(bookmarkName);

                                        outline.AddDestination(PdfExplicitDestination.CreateFit(pdf.GetPage(current)));

                                        TreeNode newNode = new TreeNode(bookmarkName)
                                        {
                                            // 通常アイコン(桃豚アイコン)
                                            ImageIndex = 0,
                                            // 選択時アイコン(白豚アイコン)
                                            SelectedImageIndex = 1
                                        };

                                        newNode.Tag = new BookmarkInfo
                                        {
                                            // しおり名
                                            BmTitle = bookmarkName,
                                            // 表示されているページ
                                            //Page = pageNum,
                                            Page = current,
                                            // 色は黒(デフォルト)
                                            SelectedColor = DrawingColor.Black,
                                            // スタイルは標準(デフォルト)
                                            SelectedStyle = FontStyle.Regular,
                                            // 展開
                                            IsOpen = true
                                        };
                                        // ルートに追加
                                        this.Invoke(() =>
                                        {
                                            treeView1.Nodes.Add(newNode);
                                        });
                                        //pageNum++;
                                    }

                                    // メタデータ設定
                                    ApplyMetadataAndViewerSettings(pdf);
                                }
                                finally
                                {
                                    // しおり追加終了
                                    this.Invoke(() =>
                                    {
                                        treeView1.EndUpdate();
                                    });
                                }

                            }
                        });

                        // 保存したファイルパス
                        originalPath = sfd.FileName;

                        try
                        {
                            // 作業用ファイルを破棄(前回PDFの tempファイル削除)
                            CleanupWorkingFile();

                            // 作業ファイル作成
                            // 作業用ファイルパス
                            workingPath = PdfFileUtil.CreateTempPdfCopy(originalPath);
                            // C:\Users\<ユーザー名>\AppData\Local\Temp\ に作業用ファイルを置く
                            //workingPath = IOPath.Combine(IOPath.GetTempPath(), $"MyPDFwork_{Guid.NewGuid()}.pdf");


                            // 元ファイルを作業用ファイルにコピー true:同じ名前は上書き
                            File.Copy(originalPath, workingPath, true);

                            // Pdfiumで表示
                            PdfiumViewer.PdfDocument document;

                            // パスワードなし
                            document = PdfiumDoc.Load(workingPath);

                            //pdfViewer1.Document = document;

                            // pdfCustomViewerにPDFを表示
                            pdfCustomViewer1.LoadDocument(document);

                            // 保存との整合性 作業用ファイルのデータを入れる
                            currentSettings = PdfSettingsLoader.LoadPdfSettings(workingPath, originalPath, null);
                            // ステータスバーにファイル名(元ファイル)と総ページ数
                            UpdateStatus(originalPath, currentSettings.TotalPage);

                            ProgressBar.Value = 0;
                            ProgressBar.Visible = false;

                            // 自動調整
                            ZoomtoolStripComboBox.SelectedIndex = 0;

                            //pdfViewer1.ZoomMode = PdfViewerZoomMode.FitBest;

                            // セキュリティなし
                            canEdit = true;

                            treeView1.LabelEdit = true;

                            // 右クリックメニュー更新
                            UpdateContextMenuState();

                            // タイトルバー更新
                            UpdateWindowTitle(originalPath, canEdit);

                            // サムネイル生成
                            pdfThumbnailViewer1.LoadDocument(document);

                            // 未保存フラグOFF
                            isDirty = false;

                        }
                        catch (Exception ex)
                        {
#if DEBUG
                            Extxt.Text = ex.ToString();
                            MessageBox.Show("変換後オープンエラー:\n" + ex.ToString(), "変換後オープン失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
#else
                            MessageBox.Show("画像からPDFへの変換に失敗しました。", "変換失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Debug.WriteLine(ex.ToString());
#endif
                        }

                        MessageBox.Show("PDF変換完了" + Environment.NewLine +
                            "ページの並びは、画像ファイルの名前順となっています。" + Environment.NewLine +
                            "必要に応じて並び替え(移動)を行って下さい。", "PDF変換確認", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Extxt.Text = ex.ToString();
                MessageBox.Show("変換エラー:\n" + ex.ToString(), "変換失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
#else
                MessageBox.Show("画像からPDFへの変換に失敗しました。", "変換失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Debug.WriteLine(ex.ToString());
#endif
            }
            finally
            {
                StatusLabel.Text = toolHintTxt;
                ProgressBar.Value = 0;
                ProgressBar.Visible = false;
                this.Enabled = true;
            }
        }

        /*
        // ==============================
        // PdfViewer1右クリックしたとき(挙動が変なので念のため)
        // ==============================
        private void pdfViewer1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // 自前でコンテキストメニュー表示
                contextMenuStrip2.Show(pdfViewer1, e.Location);
            }
        }
        */

        // ==============================
        // TreeView(しおりパネル)がリサイズされたら
        // ==============================
        private void treeView1_Resize(object sender, EventArgs e)
        {
            treeView1.Refresh();
        }

        // ==============================
        // Pdfを画像変換を押したとき
        // ==============================
        private async void ConvImgSetting_Click(object sender, EventArgs e)
        {
            // PDFが開かれていないなら処理しない
            //if (pdfViewer1.Document == null)
            //{
            //    MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            // PDFが開かれていないなら処理しない
            if (pdfCustomViewer1.Document == null)
            {
                MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // PDF設定情報が存在するか確認
            if (currentSettings == null)
            {
                MessageBox.Show("PDF設定情報の取得に失敗しました。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(currentSettings.PdfPath))
            {
                MessageBox.Show("PDFパスが取得できません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // どこから呼ばれたか
            string pageText;

            if (sender == ConvImgSetting || sender == ConvImgSetting2)
            {
                // メニュー or pdfViewer
                //pageText = (pdfViewer1.Renderer.Page + 1).ToString();
                pageText = (pdfCustomViewer1.CurrentPage + 1).ToString();
            }
            else
            {
                // サムネイル
                pageText = GetSelectedPagesText();
            }

            // 今開いているファイル名を取得
            string baseName = IOPath.GetFileNameWithoutExtension(originalPath);

            // Form14起動
            using (var f = new Form14(pageText, baseName, currentSettings.TotalPage))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    // 保存先フォルダ
                    string saveFolder = "";

                    // フォルダ選択
                    using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                    {
                        // 説明
                        fbd.Description = "保存先フォルダ選択して下さい";
                        // 初期フォルダ
                        fbd.SelectedPath = currentSettings.PdfPath;
                        // 表示(キャンセルされたら終了)
                        if (fbd.ShowDialog() != DialogResult.OK)
                            return;

                        // 保存先
                        saveFolder = fbd.SelectedPath;
                    }

                    // awaitを付けて非同期メソッドを呼び出す
                    await PdfConvImage(f.ExtractText, f.ImgFileName, saveFolder, f.ImgDpi, f.ImgType, f.IsColor);

                }
            }
        }

        // ==============================
        // PDFを画像に変換処理(非同期)
        // pageText：「1-5,8」みたいなページ指定文字列、ImgFileName：保存する画像のベース名、PdfPath：PDFファイルパス、
        // ImgDpi：解像度(DPI)、ImgType：png/jpg/bmpなど、isColor：true=カラー、false=グレースケール
        // ==============================
        private async Task PdfConvImage(string pageText, string ImgFileName, string saveFolderPath, int ImgDpi, string ImgType, bool isColor)
        {
            // PDFが開かれていないなら処理しない
            //if (pdfViewer1.Document == null)
            //{
            //    MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            // PDFが開かれていないなら処理しない
            if (pdfCustomViewer1.Document == null)
            {
                MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // PDF設定情報が存在するか確認
            if (currentSettings == null)
            {
                MessageBox.Show("PDF設定情報の取得に失敗しました。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 保存先
            string saveFolder = saveFolderPath;

            try
            {
                // 「全部に適用」の状態初期化
                _applyToAll = false;
                // 重複時は毎回確認
                _conflictMode = SaveConflictMode.Ask;

                // PDF総ページ数取得
                int total = currentSettings.TotalPage;

                // ページ解析
                var pages = PageRangeHelper.ParsePageRanges(pageText, total).Distinct().OrderBy(x => x).ToList();
                // プログレスバー初期化
                try
                {
                    StatusLabel.Text = "画像変換中...";
                    ProgressBar.Visible = true;
                    ProgressBar.Minimum = 0;
                    ProgressBar.Maximum = pages.Count;
                    ProgressBar.Value = 0;
                    ProgressBar.Style = ProgressBarStyle.Continuous;


                    // ボタンの連打防止などのためにUIを一時的に無効化
                    this.Enabled = false;

                    // 非同期処理開始
                    await Task.Run(() =>
                    {
                        int current = 0;

                        // 各ページ
                        foreach (int p in pages)
                        {
                            current++;

                            // UI更新
                            this.Invoke(() =>
                            {
                                // 進捗更新
                                ProgressBar.Value = current;
                                StatusLabel.Text = $"画像変換中... {current}/{ProgressBar.Maximum}";
                            });

                            // PDFサイズ取得
                            //var size = pdfViewer1.Document.PageSizes[p - 1];
                            var size = pdfCustomViewer1.Document.PageSizes[p - 1];


                            // DPIからピクセル計算
                            int width = (int)(size.Width / 72f * ImgDpi);
                            int height = (int)(size.Height / 72f * ImgDpi);

                            // PDF → Bitmap
                            //using Bitmap bmp = (Bitmap)pdfViewer1.Document.Render(
                            using Bitmap bmp = (Bitmap)pdfCustomViewer1.Document.Render(
                                p - 1,
                                width,
                                height,
                                ImgDpi,
                                ImgDpi,
                                PdfRenderFlags.Annotations
                            );

                            // 保存用Bitmap
                            Bitmap saveBmp = bmp;

                            // 元のままでない場合中に入る
                            if (!isColor)
                            {
                                // グレースケール変換(LockBits方式)
                                saveBmp = ToGrayScaleFast(bmp);
                            }

                            // 拡張子
                            string ext = ImgType.ToLower();

                            // 保存パス
                            string savePath = IOPath.Combine(saveFolder, $"{ImgFileName}_{p}.{ext}");
                            // 同名ファイルがある場合の処理
                            savePath = (string)this.Invoke(new Func<string>(() =>
                            {
                                return GetSafeSavePath(savePath);
                            }));

                            // 画像形式選択(デフォルトはpng)
                            ImageFormat format = ImageFormat.Png;
                            // 拡張子毎に保存形式を切り替える
                            switch (ext)
                            {
                                case "jpg":
                                case "jpeg":
                                    format = ImageFormat.Jpeg;
                                    break;

                                case "png":
                                    format = ImageFormat.Png;
                                    break;

                                case "bmp":
                                    format = ImageFormat.Bmp;
                                    break;

                                case "tif":
                                case "tiff":
                                    format = ImageFormat.Tiff;
                                    break;
                            }

                            // 保存
                            saveBmp.Save(savePath, format);

                            // グレースケール変換Bitmapだけ破棄
                            if (!isColor)
                            {
                                saveBmp.Dispose();
                            }
                        }
                    });

                }
                // ユーザーキャンセル専用
                catch (OperationCanceledException)
                {
                    MessageBox.Show("処理をキャンセルしました。", "処理中断", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //MessageBox.Show("画像変換が完了しました。", "変換完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (MessageBox.Show("画像変換が完了しました。" + Environment.NewLine +
                    "保存先フォルダを開きますか？",
                    "変換完了",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    try
                    {
                        // エクスプローラーを起動し、フォルダをWindows標準動作で開く
                        System.Diagnostics.Process.Start(
                            new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = saveFolder,
                                UseShellExecute = true
                            }
                        );
                    }
                    catch (Exception ex) // エラー補足
                    {
#if DEBUG
                        Extxt.Text = ex.ToString();
                        MessageBox.Show("保存先フォルダを開けませんでした。\n" + ex.ToString(), "フォルダオープン失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
#else
                        MessageBox.Show("保存先フォルダを開けませんでした。", "フォルダオープン失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
                    }
                }


            }
            catch (Exception ex) // エラー補足
            {
#if DEBUG
                Extxt.Text = ex.ToString();
                MessageBox.Show("画像変換エラー:\n" + ex.ToString());
#else
                MessageBox.Show("画像変換中にエラーが発生しました。", "変換失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
            }
            finally
            {
                // 元の表示に戻す
                StatusLabel.Text = toolHintTxt;
                ProgressBar.Value = 0;
                ProgressBar.Visible = false;
                this.Enabled = true;
            }
        }

        // ==============================
        // PDFを画像に変換処理 グレースケール変換ロジック
        // ==============================
        private Bitmap ToGrayScaleFast(Bitmap src)
        {
            // 出力用のBitmapを作成（元の解像度を維持）
            Bitmap bmp = new Bitmap(src.Width, src.Height, PixelFormat.Format24bppRgb);
            // 元画像のDPIをコピー
            bmp.SetResolution(src.HorizontalResolution, src.VerticalResolution);
            // Graphics生成
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // グレースケール変換用のカラーマトリックスを設定
                // NTSC形式のウェイト（0.299, 0.587, 0.114）を反映
                ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                {
                    new float[] {0.299f, 0.299f, 0.299f, 0, 0}, // 出力Rの計算
                    new float[] {0.587f, 0.587f, 0.587f, 0, 0}, // 出力Gの計算
                    new float[] {0.114f, 0.114f, 0.114f, 0, 0}, // 出力Bの計算
                    new float[] {0,      0,      0,      1, 0}, // Alpha値(透明度はそのまま使う)
                    new float[] {0,      0,      0,      0, 1}  // 固定値(触らない)
                });
                // ImageAttributes作成
                using (ImageAttributes attributes = new ImageAttributes())
                {
                    // カラーマトリックス登録
                    attributes.SetColorMatrix(colorMatrix);

                    // 変換マトリックスを適用しながら一気に描画
                    g.DrawImage(src,
                        new SysRectangle(0, 0, src.Width, src.Height),
                        0, 0, src.Width, src.Height,
                        GraphicsUnit.Pixel,
                        attributes);
                }
            }

            return bmp;
        }


        /*
         
        // ==============================
        // PDFを画像に変換処理 グレースケール変換ロジック(LockBits方式)
        // ==============================

        private Bitmap ToGrayScaleFast(Bitmap src)
        {
            // 24bitへ変換
            Bitmap bmp = new Bitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            bmp.SetResolution(src.HorizontalResolution, src.VerticalResolution);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawImage(src, 0, 0);
            }

            SysRectangle rect = new SysRectangle(0, 0, bmp.Width, bmp.Height);

            // Bitmapロック
            BitmapData data = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = data.Stride;
            IntPtr ptr = data.Scan0;

            int bytes = Math.Abs(stride) * bmp.Height;

            byte[] rgbValues = new byte[bytes];

            // Bitmap → byte配列
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            // 1ピクセルずつ処理
            for (int y = 0; y < bmp.Height; y++)
            {
                int row = y * stride;

                for (int x = 0; x < bmp.Width; x++)
                {
                    int index = row + (x * 3);

                    byte b = rgbValues[index];
                    byte g = rgbValues[index + 1];
                    byte r = rgbValues[index + 2];

                    // グレースケール計算
                    byte gray =
                        (byte)(
                            r * 0.299 +
                            g * 0.587 +
                            b * 0.114
                        );

                    // RGB全部へ同じ値
                    rgbValues[index] = gray;
                    rgbValues[index + 1] = gray;
                    rgbValues[index + 2] = gray;
                }
            }

            // byte配列 → Bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // ロック解除
            bmp.UnlockBits(data);

            return bmp;
        }

        */

        // ==============================
        // PDFを画像に変換処理 ヘルパー関数(上書き確認)1
        // そのまま保存する or 別名に変更する を決定する
        // ==============================
        private string GetSafeSavePath(string path)
        {
            // 保存先に同名ファイルが存在しないならそのまま返す
            if (!File.Exists(path))
                return path;
            // 「この選択をすべてに適用」を選んだとき
            // _conflictMode = Rename なら ResolvePath()を呼ぶ
            if (_applyToAll)
                return ResolvePath(path, _conflictMode);
            // 確認ダイアログ表示
            DialogResult result = MessageBox.Show(
                $"同名ファイルがあります:\n{IOPath.GetFileName(path)}\n\n上書きしますか？",
                "確認",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);
            // キャンセルなら OperationCanceledExceptionを投げる
            if (result == DialogResult.Cancel)
                throw new OperationCanceledException();
            // 上書きを選択した場合
            if (result == DialogResult.Yes)
            {
                // 「この選択をすべてに適用しますか？」を聞く
                if (AskApplyAll())
                {
                    // 以後確認しない
                    _applyToAll = true;
                    // 上書き
                    _conflictMode = SaveConflictMode.Overwrite;
                }
                // 元のファイル名で保存
                return path;
            }
            // 別名保存を選択した場合
            if (result == DialogResult.No)
            {
                // 全部適用確認
                if (AskApplyAll())
                {
                    // 今後確認しない
                    _applyToAll = true;
                    // 今後は自動で別名作成
                    _conflictMode = SaveConflictMode.Rename;
                }
                // 別名を生成して返す
                return ResolvePath(path, SaveConflictMode.Rename);
            }
            // 念のため(通常ここには来ない)
            return path;
        }

        // ==============================
        // PDFを画像に変換処理 ヘルパー関数(上書き確認)2
        // 実際に別名を作る関数
        // ==============================
        private string ResolvePath(string path, SaveConflictMode mode)
        {
            // 上書きなら何もしない
            if (mode == SaveConflictMode.Overwrite)
                return path;
            // フォルダ取得
            string? dir = IOPath.GetDirectoryName(path);
            // 拡張子なし名前
            string name = IOPath.GetFileNameWithoutExtension(path);
            // 拡張子
            string ext = IOPath.GetExtension(path);

            int i = 1;
            // 生成用
            string newPath;
            // 必ず1回実行
            do
            {
                // sample_1.png、sample_2.pngのように名前を付ける
                newPath = IOPath.Combine(dir ?? "", $"{name}_{i}{ext}");
                i++;
            }
            // 存在する限り繰り返す
            while (File.Exists(newPath));
            // 決定した保存名を返す
            return newPath;
        }

        // ==============================
        // PDFを画像に変換処理 ヘルパー関数(上書き確認)3
        // 「全部に適用」を聞くだけの関数
        // ==============================
        private bool AskApplyAll()
        {
            return MessageBox.Show("この選択をすべてに適用しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        // ==============================
        // サムネイルの選択ページを取得(複数ページ可)
        // ==============================
        private string GetSelectedPagesText()
        {
            List<int> pages = new();
            // サムネイル選択ページを集める
            foreach (int index in pdfThumbnailViewer1.SelectedIndices)
            {
                pages.Add(index + 1);
            }

            // もしサムネイルが1つも選択されていない場合は、
            // 現在メインビューアで表示中のページをフォールバックとして取得
            //if (pages.Count == 0 && pdfViewer1.Document != null)
            if (pages.Count == 0 && pdfCustomViewer1.Document != null)
            {
                //pages.Add(pdfViewer1.Renderer.Page + 1);
                pages.Add(pdfCustomViewer1.CurrentPage + 1);
            }

            pages.Sort();

            // 既存の連番圧縮ロジックへそのまま渡して "1-3,5" などの文字列にする
            return CompressPageRanges(pages);
        }

        // ==============================
        // サムネイルの選択(連番圧縮)
        // ==============================
        private string CompressPageRanges(List<int> pages)
        {
            if (pages.Count == 0)
                return "";

            StringBuilder sb = new();

            int start = pages[0];
            int end = pages[0];

            for (int i = 1; i < pages.Count; i++)
            {
                if (pages[i] == end + 1)
                {
                    end = pages[i];
                }
                else
                {
                    AppendRange(sb, start, end);

                    start = end = pages[i];
                }
            }

            AppendRange(sb, start, end);

            return sb.ToString();
        }

        // ==============================
        // サムネイルの選択(範囲追加)
        // 1-5,8,12-13のようにする
        // ==============================
        private void AppendRange(StringBuilder sb, int start, int end)
        {
            if (sb.Length > 0)
                sb.Append(",");

            if (start == end)
            {
                sb.Append(start);
            }
            else
            {
                sb.Append($"{start}-{end}");
            }
        }

        // ==============================
        // サムネイルを選択し削除処理
        // ==============================
        private void PageDeleteSetting3_Click(object sender, EventArgs e)
        {
            string pages = GetSelectedPagesText();
            int maxPage = currentSettings?.TotalPage ?? 0;

            // 構文チェック(PageRangeHelper.csを呼ぶ)
            var ChackPages = PageRangeHelper.ParsePageRanges(pages, maxPage);
            // 全ページ削除禁止
            if (ChackPages.Count >= maxPage)
            {
                MessageBox.Show("最低1ページは残してください。", "ページ削除エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DeletePages(pages);
        }

        // ==============================
        // サムネイルを選択し左90°回転処理
        // ==============================
        private void RotatePagesSettingLeft90_Click(object sender, EventArgs e)
        {
            // サムネイル複数選択状態を保持
            SaveCurrentSelections();
            // 選択ページを整える
            string pages = GetSelectedPagesText();
            // 回転処理
            RotatePages(pages, 270);
        }

        // ==============================
        // サムネイルを選択し右90°回転処理
        // ==============================
        private void RotatePagesSettingRight90_Click(object sender, EventArgs e)
        {
            // サムネイル複数選択状態を保持
            SaveCurrentSelections();
            // 選択ページを整える
            string pages = GetSelectedPagesText();
            // 回転処理
            RotatePages(pages, 90);
        }

        // ==============================
        // サムネイルを選択し180°回転処理
        // ==============================
        private void RotatePagesSetting180_Click(object sender, EventArgs e)
        {
            // サムネイル複数選択状態を保持
            SaveCurrentSelections();
            // 選択ページを整える
            string pages = GetSelectedPagesText();
            // 回転処理
            RotatePages(pages, 180);
        }

        // ==============================
        // サムネイルを選択し抽出処理
        // ==============================
        private async void PageExtractSetting3_Click(object sender, EventArgs e)
        {
            string pages = GetSelectedPagesText();
            await ExtractPagesAsync(pages);
        }

        // ==============================
        // サムネイルタブをクリックした場合
        // ==============================
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // PDFが開かれていないなら処理しない
            //if (pdfViewer1.Document == null)
            //    return;
            // PDFが開かれていないなら処理しない
            if (pdfCustomViewer1.Document == null)
                return;

            if (tabControl1.SelectedTab == tabPage1)
            {
                ShioriMenu.Enabled = true;
                PageEditMenu.Enabled = false;

                // しおりにフォーカス
                treeView1.Focus();
            }

            if (tabControl1.SelectedTab == tabPage2)
            {
                ShioriMenu.Enabled = false;
                PageEditMenu.Enabled = true;

                // サムネイルにフォーカス
                pdfThumbnailViewer1.Focus();

                // 表示ページを取得
                //int page = pdfViewer1.Renderer.Page;
                //int page = pdfCustomViewer1.CurrentPage + 1;

            }
        }

        // ==============================
        // サムネイルをクリックした場合
        // ==============================
        private void PdfThumbnailViewer1_SelectionChanged(object? sender, List<int> selectedIndices)
        {
            if (selectedIndices.Count == 0) return;

            // プログラムからの強制操作が始まるのでロックをかける！
            _isSyncing = true;

            try
            {
                // 複数選択されている場合、最後に選択された（追加された）ページをメインビューアで表示する
                int latestPage = selectedIndices[selectedIndices.Count - 1];

                // 最後に選択したページを格納(ページを表示する)
                //pdfViewer1.Renderer.Page = latestPage;
                pdfCustomViewer1.ScrollToPage(latestPage);
                currentThumbnailPage = latestPage;
            }
            finally
            {
                // スクロール命令が完全に終わったら、ロックを解除する
                _isSyncing = false;
            }

        }

        // ==============================
        // 右クリックしたサムネイルのページ番号を取得
        // ==============================
        private void pdfThumbnailViewer1_ThumbnailRightClicked(object? sender, (bool isSuccess, int pageNumber) e)
        {
            // PDFが開かれていないなら処理しない
            //if (pdfViewer1.Document == null)
            //    return;
            // PDFが開かれていないなら処理しない
            if (pdfCustomViewer1.Document == null)
                return;

            if (e.isSuccess)
            {
                // サムネイルから正常に取得できた場合
                thumbnailRightClickPage = e.pageNumber;

            }
            else
            {
                // 取得できなかった場合：NewPagetoolStripTextBox から値を取得する
                // テキストボックスの中身が正しい数字（整数）に変換できるかチェック
                if (int.TryParse(NewPagetoolStripTextBox.Text, out int parsedPage))
                {
                    thumbnailRightClickPage = parsedPage;
                }
                else
                {
                    // もしテキストボックスが空だったり文字が入っていた場合の安全ガード（例: 1ページ目にするなど）
                    thumbnailRightClickPage = 1;
                }
            }
        }

        // ==============================
        // サムネイル　ドラッグ＆ドロップで移動(ページ入れ替え)
        // ==============================
        private void PdfThumbnailViewer1_PageMoved(object? sender, (int targetIdx, bool before) e)
        {
            // 選択されているページテキスト（例: "2,3" や "5-7"）を取得
            string pageText = GetSelectedPagesText();
            if (string.IsNullOrEmpty(pageText)) return;

            // 移動先のページ番号（1始まり）を計算
            int targetPage = e.targetIdx + 1;

            // ページ移動処理へ丸投げ
            MovePdfPage(pageText, targetPage, e.before);
        }

        // ==============================
        // 現在のサムネイル選択状態（またはメインの現在ページ）を退避する
        // ==============================
        private void SaveCurrentSelections()
        {
            _savedSelectedIndices.Clear();

            // サムネイルの選択インデックスをそのままコピー
            foreach (int index in pdfThumbnailViewer1.SelectedIndices)
            {
                _savedSelectedIndices.Add(index);
            }

            // サムネイル未選択時のフォールバック（メイン画面の現在ページを救済）
            if (_savedSelectedIndices.Count == 0 && pdfCustomViewer1.Document != null)
            {
                _savedSelectedIndices.Add(pdfCustomViewer1.CurrentPage);
            }
        }

        // ==============================
        // 連続スクロールと1ページずつ表示の切り替え
        // ==============================
        private void ViewSelect_Click(object sender, EventArgs e)
        {
            if (ViewSelect.Checked)
            {
                // 連続スクロール表示にする
                pdfCustomViewer1.ViewMode = PdfCustomViewer.PdfViewMode.Continuous;
                ViewSelect.Text = "連続スクロール表示をOFFにする";
            }
            else
            {
                // 1ページずつの表示にする
                pdfCustomViewer1.ViewMode = PdfCustomViewer.PdfViewMode.SinglePage;
                ViewSelect.Text = "連続スクロール表示をONにする";
            }

            // 数値チェック
            if (!int.TryParse(NewPagetoolStripTextBox.Text, out int page))
                return;

            // ページ範囲チェック
            int maxPage = currentSettings?.TotalPage ?? 0;

            // ページ範囲外なら戻る
            if (page < 1 || page > maxPage)
                return;

            // ジャンプ
            JumpToPage(page);

        }
    }
}
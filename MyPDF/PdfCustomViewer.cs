using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

// ==============================
// PDFを表示するためのユーザーコントロール
// ==============================

namespace MyPDF
{
    public partial class PdfCustomViewer : UserControl
    {
        // 表示対象のPDFドキュメント
        // 例: PdfiumViewer.PdfDocument など
        private dynamic? _pdfDocument = null;
        //private int _currentPage = 0;

        // カメラ（表示位置・倍率）の管理変数
        private float _zoom = 1.0f;          // 1.0 = 100%
        private PointF _offset = new PointF(0, 20); // 画面左上からの描画オフセット

        // マウスドラッグ（スクロール）用の変数
        private bool _isDragging = false;
        private Point _lastMousePos;

        // ページ間の隙間（余白）ピクセル数
        private const int PageSpacing = 20;

        // 基準となるページのサイズ（A4比率：横800 × 縦1130）
        // 本来はページごとにPdfiumからサイズ（実寸）を取得すべきですが、まずは固定で実装します
        //private const float BasePageWidth = 800;
        //private const float BasePageHeight = 1130;

        // 変更：外部から現在のドキュメントを参照・解放できるようにプロパティ化します
        public dynamic? Document => _pdfDocument;

        // 追加：PDFの総ページ数を保持する変数（今後のページめくり機能で使います）
        public int PageCount { get; private set; } = 0;

        // 現在表示中のページ番号（0始まりのインデックス）を取得する
        public int CurrentPage
        {
            get
            {
                if (_pdfDocument == null || PageCount == 0) return 0;

                // 単ページ表示なら、現在のページインデックスをそのまま返す
                if (_viewMode == PdfViewMode.SinglePage)
                {
                    // 現在のページが全体のページ数を超えていたら0に戻す
                    if (_singlePageIndex >= PageCount) _singlePageIndex = 0;
                    return _singlePageIndex;
                }

                // 画面の中央線を基準にする
                // 画面上端の絶対Y座標（正の数）
                float currentTopY = -_offset.Y;

                // 現在のコントロール（表示エリア）の高さの半分を計算
                float halfControlHeight = this.ClientSize.Height / 2f;

                // 上端位置に半分を足して、「画面の真ん中を通っている絶対Y座標」を割り出す
                float centerY = currentTopY + halfControlHeight;

                float spacing = PageSpacing * _zoom;
                float accumulatedHeight = 0;

                for (int i = 0; i < PageCount; i++)
                {
                    var pageSizes = _pdfDocument?.PageSizes;
                    SizeF originalSize = pageSizes?[i];
                    float pageHeight = originalSize.Height * _zoom;

                    // このページの「上端」と「下端」の範囲を計算
                    float pageTop = accumulatedHeight;
                    float pageBottom = accumulatedHeight + pageHeight + spacing;

                    // 画面の中央線（centerY）が、このページの縦の範囲内にすっぽり入っているか判定
                    if (centerY >= pageTop && centerY <= pageBottom)
                    {
                        return i;
                    }

                    accumulatedHeight += pageHeight + spacing;
                }

                // スクロールが一番下まで行きすぎて中央線が最終ページも突き抜けた場合は、安全のため最終ページを返す
                return PageCount - 1;
            }
        }

        // ズーム倍率の外部公開プロパティ
        // 【追加】プロパティウィンドウに表示させない
        [Browsable(false)]
        // 【追加】核心：デザイナによる自動コード生成（シリアル化）の対象外にする
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float Zoom
        {
            get => _zoom;
            set
            {
                // 0.1倍〜5倍の安全ガード
                float nextZoom = Math.Max(0.1f, Math.Min(value, 5.0f));
                if (_zoom != nextZoom)
                {
                    _zoom = nextZoom;
                    UpdateScrollBarRanges();
                    Invalidate();
                }
            }
        }

        // スクロールバーコントロール
        private VScrollBar _vScrollBar;
        private HScrollBar _hScrollBar;
        private bool _isUpdatingScrollBars = false; // 無限ループ防止フラグ

        // 表示方法
        public enum PdfViewMode
        {
            Continuous, // 連続スクロール表示（今までのモード）
            SinglePage  // ページ表示（単ページ切り替えモード）
        }
        // 初期表示状態(単ページ)
        //private PdfViewMode _viewMode = PdfViewMode.Continuous;
        private PdfViewMode _viewMode = PdfViewMode.SinglePage;
        private int _singlePageIndex = 0; // 単ページ表示時の現在のページ番号

        // デザイナーの自動コード生成（シリアル化）の対象外にする
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PdfViewMode ViewMode
        {
            get => _viewMode;
            set
            {
                if (_viewMode != value)
                {
                    _viewMode = value;
                    // モードが変わったら表示をリセット
                    if (_viewMode == PdfViewMode.SinglePage)
                    {
                        _singlePageIndex = this.CurrentPage; // 今のページを引き継ぐ
                        _offset = new PointF(0, 20); // スクロール位置をリセット
                    }
                    UpdateScrollBarRanges(); // スクロールバーの再計算
                    Invalidate(); // 再描画
                }
            }
        }

        public PdfCustomViewer()
        {
            InitializeComponent();

            // 描画のちらつき（チャタリング）を抑えるためのダブルバッファリング設定
            this.DoubleBuffered = true;
            this.BackColor = Color.DarkGray; // PDFの背景（余白部分）の色
            // ★修正：サムネイルビューアと同じ洗練されたダークグレーに統一
            //this.BackColor = Color.FromArgb(45, 45, 48);

            // スクロールバーの初期化と配置
            _vScrollBar = new VScrollBar { Dock = DockStyle.Right, Enabled = false };
            _hScrollBar = new HScrollBar { Dock = DockStyle.Bottom, Enabled = false };

            _vScrollBar.Scroll += ScrollBar_Scroll;
            _hScrollBar.Scroll += ScrollBar_Scroll;

            this.Controls.Add(_vScrollBar);
            this.Controls.Add(_hScrollBar);

            this.Paint += PdfCustomViewer_Paint;
            this.MouseDown += PdfCustomViewer_MouseDown;
            this.MouseMove += PdfCustomViewer_MouseMove;
            this.MouseUp += PdfCustomViewer_MouseUp;
            this.Resize += PdfCustomViewer_Resize; // 画面サイズ変更時にも追従

        }

        // ==============================
        // 外部からPDFドキュメントをセットするメソッド
        // ==============================
        public void LoadDocument(dynamic document)
        {
            // 新しいドキュメントをロードする前に古いものをクリア
            CloseDocument();

            _pdfDocument = document;
            //_currentPage = 0;

            // 新しいPDFを開くので、強制的にページ位置とスクロール位置を初期化する
            _singlePageIndex = 0;
            _offset = new PointF(0, 20);

            // 追加：読み込んだドキュメントから総ページ数を取得（ライブラリの仕様に合わせてください）
            // PdfiumViewerの場合は document.PageCount です
            if (_pdfDocument != null)
            {
                PageCount = _pdfDocument.PageCount;

                // ドキュメントが正常に読み込めたら、コントロール幅に合わせる
                //FitToWidth();
                AutoFit();
            }
            else
            {
                PageCount = 0;
            }

            ResetView();
        }

        // ==============================
        // 表示 自動調整
        // ==============================
        public void AutoFit()
        {
            if (_pdfDocument == null || PageCount == 0) return;

            // 現在表示中のページ番号を退避
            int currentPageIndex = this.CurrentPage;

            // 表示しているページのサイズ（ポイント単位）を取得
            SizeF firstPageSize = _pdfDocument?.PageSizes[currentPageIndex];

            if (firstPageSize.Height >= firstPageSize.Width)
            {
                // 縦向き
                FitToHeight();
            }
            else
            {
                // 横向き
                FitToWidth();
            }
        }

        // ==============================
        // コントロールの横幅に合わせてズーム倍率を自動計算する
        // ==============================
        public void FitToWidth()
        {
            if (_pdfDocument == null || PageCount == 0) return;

            try
            {
                // 現在表示中のページ番号を退避
                int currentPageIndex = this.CurrentPage;

                // 表示しているページのサイズ（ポイント単位）を取得
                SizeF firstPageSize = _pdfDocument?.PageSizes[currentPageIndex];
                if (firstPageSize.Width <= 0) return;

                // 基礎となるコントロールの有効幅を取得（ClientSizeを優先）
                float baseWidth = this.Width;
                if (this.ClientSize.Width > 0) baseWidth = this.ClientSize.Width;

                // 左右のマージン
                float padding = 20f;
                float availableWidth = baseWidth - padding;

                // ------------------------------------------------------------
                // 【モード別】幅計算の調整
                // ------------------------------------------------------------
                if (_viewMode == PdfViewMode.Continuous)
                {
                    // 連続スクロール時は常に垂直スクロールバーが出る可能性を考慮して幅を引く
                    float vScrollBarWidth = (_vScrollBar != null) ? _vScrollBar.Width : 0;
                    availableWidth -= vScrollBarWidth;
                }
                else
                {
                    // 単一ページ時は、拡大されていなければ垂直スクロールバーは出ないので引かない
                }

                if (availableWidth <= 0) availableWidth = baseWidth;

                // 最適なズーム倍率を逆算
                _zoom = availableWidth / firstPageSize.Width;
                _zoom = Math.Max(0.1f, Math.Min(_zoom, 1.0f));

                // 【モード別】オフセット（スクロール位置）の計算
                if (_viewMode == PdfViewMode.SinglePage)
                {
                    // 単一ページ表示：描画側（Paint）が自動で中央寄せするため位置リセット
                    _offset = new PointF(0, 0);
                }
                else
                {
                    // 連続スクロールモード：元いたページの位置へ強制縦スクロール
                    float spacing = PageSpacing * _zoom;
                    float targetY = spacing;

                    for (int i = 0; i < currentPageIndex; i++)
                    {
                        var pageSizes = _pdfDocument?.PageSizes;
                        SizeF originalSize = pageSizes?[i];
                        float pageHeight = originalSize.Height * _zoom;

                        targetY += pageHeight + spacing;
                    }

                    // ぴったり元のページが先頭（上部マージン20px空け）にくるようにオフセットを設定
                    _offset = new PointF(0, -targetY + 20);
                }

                // スクロールバーの範囲を再計算して画面をリフレッシュ
                UpdateScrollBarRanges();
                Invalidate();
            }
            catch
            {
                ResetView();
            }
        }

        // ==============================
        // コントロールの高さに合わせてズーム倍率を自動計算する（現在ページキープ版）
        // ==============================
        public void FitToHeight()
        {
            if (_pdfDocument == null || PageCount == 0) return;

            try
            {
                // 現在表示中のページ番号を退避
                int currentPageIndex = this.CurrentPage;

                // 表示しているページのサイズ（ポイント単位）を取得
                SizeF firstPageSize = _pdfDocument?.PageSizes[currentPageIndex];
                if (firstPageSize.Height <= 0) return;

                // 基礎となるコントロールの有効高さを取得（ClientSizeを優先）
                float baseHeight = this.Height;
                if (this.ClientSize.Height > 0) baseHeight = this.ClientSize.Height;

                // 【モード別】高さ計算とパディングの調整
                float padding = 40f;
                float hScrollBarHeight = 0f;

                if (_viewMode == PdfViewMode.SinglePage)
                {
                    // 単一ページ時は見切れ防止用にパディングを多めに取り、下バーの厚み(20px)も引く
                    padding = 50f;
                    hScrollBarHeight = 20f;
                }

                float availableHeight = baseHeight - padding - hScrollBarHeight;
                if (availableHeight <= 0) availableHeight = baseHeight;

                // 新しいズーム倍率を「高さ基準」で逆算
                _zoom = availableHeight / firstPageSize.Height;
                _zoom = Math.Max(0.1f, Math.Min(_zoom, 1.0f));

                // 【モード別】オフセット（スクロール位置）の計算
                if (_viewMode == PdfViewMode.SinglePage)
                {
                    // 単一ページ表示：描画側（Paint）が自動で中央寄せするため位置リセット
                    _offset = new PointF(0, 0);
                }
                else
                {
                    // 連続スクロールモード：元いたページの位置へ強制縦スクロール
                    float spacing = PageSpacing * _zoom;
                    float targetY = spacing;

                    for (int i = 0; i < currentPageIndex; i++)
                    {
                        var pageSizes = _pdfDocument?.PageSizes;
                        SizeF originalSize = pageSizes?[i];
                        float pageHeight = originalSize.Height * _zoom;

                        targetY += pageHeight + spacing;
                    }

                    _offset = new PointF(0, -targetY + 20);
                }

                // スクロールバーの範囲を再計算して画面をリフレッシュ
                UpdateScrollBarRanges();
                Invalidate();
            }
            catch
            {
                _zoom = 1.0f;
                _offset = (_viewMode == PdfViewMode.SinglePage) ? new PointF(0, 0) : new PointF(0, 20);
                Invalidate();
            }
        }

        // ==============================
        // PDFを閉じる
        // ==============================
        public void CloseDocument()
        {
            if (_pdfDocument != null)
            {
                try
                {
                    _pdfDocument.Dispose();
                }
                catch
                {

                }
                _pdfDocument = null;
                PageCount = 0;
                Invalidate();
            }
        }

        // ==============================
        // 表示位置と倍率を初期状態に戻す
        // ==============================
        public void ResetView()
        {
            if (_pdfDocument != null)
            {
                FitToWidth();
            }
            else
            {
                _zoom = 1.0f;
                _offset = new PointF(20, 20); // 少し余白を持たせる
                Invalidate(); // 再描画要求
            }
        }

        // ==============================
        // PDF全体の仮想サイズを計算し、スクロールバーの可動範囲を更新する
        // ==============================
        private void UpdateScrollBarRanges()
        {
            if (_pdfDocument == null || _isUpdatingScrollBars) return;

            _isUpdatingScrollBars = true;

            float totalHeight = 0;
            float maxWidth = 0;

            if (_viewMode == PdfViewMode.Continuous)
            {
                // 【連続スクロールモード】全ページの合計高さと最大幅を計算
                float spacing = PageSpacing * _zoom;
                totalHeight = spacing; // 初期の上部余白

                // 1. 全ページの合計高さと、最大横幅を動的に再計算
                for (int i = 0; i < PageCount; i++)
                {
                    // dynamic型に対して安全にPageSizesを取得
                    var pageSizes = _pdfDocument?.PageSizes;
                    SizeF originalSize = pageSizes?[i];
                    float pageWidth = originalSize.Width * _zoom;
                    float pageHeight = originalSize.Height * _zoom;

                    totalHeight += pageHeight + spacing;
                    if (pageWidth > maxWidth) maxWidth = pageWidth;
                }
            }
            else
            {
                // 【単ページ表示モード】「現在の1ページ」のサイズだけを基準にする
                var pageSizes = _pdfDocument?.PageSizes;
                SizeF originalSize = pageSizes?[_singlePageIndex];
                maxWidth = originalSize.Width * _zoom;
                totalHeight = originalSize.Height * _zoom + 40f; // 上下マージン分(片側20px)を加算
            }

            // 表示画面の純粋なサイズ（スクロールバー自身の厚みを除く）
            float displayWidth = this.Width - (_vScrollBar.Visible ? _vScrollBar.Width : 0);
            float displayHeight = this.Height - (_hScrollBar.Visible ? _hScrollBar.Height : 0);

            // --- 縦スクロールの限界値チェックと丸め処理 ---
            int maxV = (int)(totalHeight - displayHeight);
            if (maxV > 0)
            {
                _vScrollBar.Enabled = true;
                _vScrollBar.Maximum = maxV + _vScrollBar.LargeChange - 1;

                // モードごとの縦方向のクランプ（移動制限）
                if (_viewMode == PdfViewMode.Continuous)
                {
                    // _offset.Yが限界を超えていたら強制補正（無限スクロールを阻止）
                    if (_offset.Y > 20) _offset.Y = 20;          // 1ページ目より上には行かせない
                    if (_offset.Y < -maxV) _offset.Y = -maxV;  // 最終ページより下には行かせない
                }
                else
                {
                    // 単ページ拡大時：はみ出た分だけ上下スクロール可能にする（上端は見切れない）
                    // 描画位置yの基準が「(画面高 - ページ高)/2 + offset.Y」なので、
                    // offset.Y の動ける範囲は 「-maxV/2」 ～ 「maxV/2」 になる
                    float limitY = maxV / 2f;
                    if (_offset.Y > limitY + 20) _offset.Y = limitY + 20;
                    if (_offset.Y < -limitY + 20) _offset.Y = -limitY + 20;
                }

                // スクロールバーのツマミ位置を同期
                if (_viewMode == PdfViewMode.Continuous)
                {
                    _vScrollBar.Value = Math.Max(0, Math.Min((int)-_offset.Y, _vScrollBar.Maximum - _vScrollBar.LargeChange + 1));
                }
                else
                {
                    float limitY = maxV / 2f;
                    _vScrollBar.Value = Math.Max(0, Math.Min((int)(limitY - _offset.Y + 20), _vScrollBar.Maximum - _vScrollBar.LargeChange + 1));
                }
            }
            else
            {
                // 画面にすっぽり収まる場合はスクロール不要
                _vScrollBar.Enabled = false;
                // 単ページ時は中央配置されるため、オフセットは20（初期値）
                _offset.Y = (_viewMode == PdfViewMode.Continuous) ? 20 : 20;
            }

            // --- 横スクロールの限界値チェックと丸め処理 ---
            int maxH = (int)(maxWidth - displayWidth);
            if (maxH > 0)
            {
                _hScrollBar.Enabled = true;
                _hScrollBar.Maximum = maxH + _hScrollBar.LargeChange - 1;

                // 横方向のスクロールドラッグ無限化を阻止
                if (_offset.X > maxH / 2f) _offset.X = maxH / 2f;
                if (_offset.X < -maxH / 2f) _offset.X = -maxH / 2f;

                _hScrollBar.Value = Math.Max(0, Math.Min((int)(maxH / 2f - _offset.X), _hScrollBar.Maximum - _hScrollBar.LargeChange + 1));
            }
            else
            {
                _hScrollBar.Enabled = false;
                _offset.X = 0; // 画面に収まる場合は中央に固定
            }

            _isUpdatingScrollBars = false;
        }

        // ==============================
        // スクロールバーが手動で動かされたときのイベント
        // ==============================
        private void ScrollBar_Scroll(object? sender, ScrollEventArgs e)
        {
            if (_isUpdatingScrollBars) return;

            // 縦スクロール
            _offset.Y = -_vScrollBar.Value;

            // 横スクロール（センタリング基準からの差分で移動）
            if (_hScrollBar.Enabled)
            {
                int maxH = _hScrollBar.Maximum - _hScrollBar.LargeChange + 1;
                _offset.X = (maxH / 2f) - _hScrollBar.Value;
            }
            else
            {
                _offset.X = 0;
            }

            Invalidate();
        }

        // ==============================
        // リサイズされたとき
        // ==============================
        private void PdfCustomViewer_Resize(object? sender, EventArgs e)
        {
            // PDFが開かれていないなら何もしない
            if (_pdfDocument == null || PageCount == 0) return;

            // 現在の表示モード（またはコンボボックスの状態）を維持してリサイズ追従させる
            if (_viewMode == PdfViewMode.SinglePage)
            {
                // 単一ページ表示のときは、常に画面内にすっぽり収まるように「高さ合わせ」を掛け直す
                FitToHeight();
            }
            else
            {
                // 連続スクロールモードのときは、これまで通り「幅合わせ」を掛け直す
                FitToWidth();
            }

            //UpdateScrollBarRanges();
            //Invalidate();
        }

        // ==============================
        // 描画ロジック（A4/A3などの実寸サイズ混在対応版）
        // ==============================
        private void PdfCustomViewer_Paint(object? sender, PaintEventArgs e)
        {
            if (_pdfDocument == null) return;

            Graphics g = e.Graphics;

            // 描画品質を最高レベル（クッキリ表示）に設定する
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; // 高品質な拡大縮小（ボヤけ防止）
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality; // アンチエイリアスを綺麗にかける
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality; // 1ピクセルのズレによる滲みを防ぐ
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit; // テキストフォントの最適化

            if (_viewMode == PdfViewMode.Continuous)
            {
                // 連続スクロール表示
                //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //g.SmoothingMode = SmoothingMode.HighQuality;

                float spacing = PageSpacing * _zoom;
                int pageCount = _pdfDocument.PageCount;

                // 次のページを描画するY座標の基準位置
                float currentTopY = _offset.Y;

                // 表示画面（UserControl自体）の横幅
                float controlWidth = this.Width;

                for (int i = 0; i < pageCount; i++)
                {
                    // 1. Pdfiumからページの「本来のサイズ（ポイント単位）」を取得
                    SizeF originalSize = _pdfDocument.PageSizes[i];

                    // 2. 現在のズーム倍率を掛け算して画面上のピクセルサイズにする
                    float pageWidth = originalSize.Width * _zoom;
                    float pageHeight = originalSize.Height * _zoom;

                    // 各ページの描画開始X座標（左右中央揃え）を計算
                    // 基本は「(画面幅 - ページ幅) / 2」ですが、手のひらツール（ドラッグ）での
                    // 左右スクロール量（_offset.X）も連動できるように足し算します。
                    float pageLeftX = (controlWidth - pageWidth) / 2f + _offset.X;

                    // 3. 画面外にあるページは描画をスキップして軽快にする（クリッピング最適化）
                    if (currentTopY + pageHeight < 0 || currentTopY > this.Height)
                    {
                        currentTopY += pageHeight + spacing;
                        continue;
                    }

                    // 4. ズームに応じたDPI計算（パターンA）
                    int currentDpiX = (int)(96 * _zoom);
                    int currentDpiY = (int)(96 * _zoom);
                    currentDpiX = Math.Max(10, Math.Min(currentDpiX, 300));
                    currentDpiY = Math.Max(10, Math.Min(currentDpiY, 300));

                    // ズームに関係なく、常にベースを 200 DPI として高精細にレンダリングする
                    //int currentDpiX = 200;
                    //int currentDpiY = 200;

                    // 4. ズームに応じたDPI計算（解像度倍）
                    //int currentDpiX = (int)(192 * _zoom);
                    //int currentDpiY = (int)(192 * _zoom);
                    //currentDpiX = Math.Max(150, Math.Min(currentDpiX, 300));
                    //currentDpiY = Math.Max(150, Math.Min(currentDpiY, 300));

                    // --- 背景（用紙の白い土台）と影の描画 ---
                    // X座標の基準を「_offset.X」から、今計算した「pageLeftX」に差し替え
                    g.FillRectangle(Brushes.DimGray, pageLeftX + 4, currentTopY + 4, pageWidth, pageHeight);
                    g.FillRectangle(Brushes.White, pageLeftX, currentTopY, pageWidth, pageHeight);

                    // --- PDFページの画像（Bitmap）を描画 ---
                    using (Image pageImage = _pdfDocument.Render(i, (int)pageWidth, (int)pageHeight, currentDpiX, currentDpiY, false))
                    {
                        if (pageImage != null)
                        {
                            g.DrawImage(pageImage, pageLeftX, currentTopY, pageWidth, pageHeight);
                        }
                    }

                    // 次のページの描画位置（Y座標）を進める
                    currentTopY += pageHeight + spacing;
                }
            }
            else
            {
                // 【単ページ表示】現在の1ページだけを描画する
                var pageSizes = _pdfDocument.PageSizes;
                SizeF originalSize = pageSizes[_singlePageIndex];

                float w = originalSize.Width * _zoom;
                float h = originalSize.Height * _zoom;

                // 1. スクロールバーが表示されている場合、その厚み・幅を差し引いた「純粋な表示エリア」を出す
                float hScrollBarHeight = (_hScrollBar != null && _hScrollBar.Visible) ? _hScrollBar.Height : 0;
                float vScrollBarWidth = (_vScrollBar != null && _vScrollBar.Visible) ? _vScrollBar.Width : 0;

                float clientWidth = this.Width - vScrollBarWidth;
                float clientHeight = this.Height - hScrollBarHeight;

                // 2. 画面中央に配置するための計算
                float x = (clientWidth - w) / 2f + _offset.X;
                float y = (clientHeight - h) / 2f + _offset.Y;

                // 3. 画面に収まっている時は、ドラッグによる微小なズレを防ぐため完全中央にロックする
                if (h <= clientHeight && _vScrollBar?.Enabled == false)
                {
                    y = (clientHeight - h) / 2f;
                }
                if (w <= clientWidth && _hScrollBar?.Enabled == false)
                {
                    x = (clientWidth - w) / 2f;
                }

                // ちょっと上へあげる
                if (h <= clientHeight)
                {
                    y -= 10f;
                }

                // ズームに応じたDPI計算
                int currentDpiX = (int)(96 * _zoom);
                int currentDpiY = (int)(96 * _zoom);
                currentDpiX = Math.Max(10, Math.Min(currentDpiX, 300));
                currentDpiY = Math.Max(10, Math.Min(currentDpiY, 300));

                // ズームに関係なく、常にベースを 200 DPI として高精細にレンダリングする
                //int currentDpiX = 200;
                //int currentDpiY = 200;

                // ズームに応じたDPI計算
                //int currentDpiX = (int)(192 * _zoom);
                //int currentDpiY = (int)(192 * _zoom);
                //currentDpiX = Math.Max(150, Math.Min(currentDpiX, 300));
                //currentDpiY = Math.Max(150, Math.Min(currentDpiY, 300));

                // 背景（用紙の白い土台）と影の描画
                g.FillRectangle(Brushes.DimGray, x + 4, y + 4, w, h);
                g.FillRectangle(Brushes.White, x, y, w, h);

                // _pdfDocument.Render を使って、指定ページ（_singlePageIndex）を描画
                using (Image pageImage = _pdfDocument.Render(_singlePageIndex, (int)w, (int)h, currentDpiX, currentDpiY, false))
                {
                    if (pageImage != null)
                    {
                        g.DrawImage(pageImage, x, y, w, h);
                    }
                }
            }

        }

        // ==============================
        // マウスホイール（Ctrlで拡大縮小、通常で上下スクロール）
        // ==============================
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (_pdfDocument == null) return;

            if ((ModifierKeys & Keys.Control) == Keys.Control)
            {
                Point mousePos = e.Location;
                float displayWidth = this.Width - (_vScrollBar.Enabled ? _vScrollBar.Width : 0);

                float pdfX = (mousePos.X - (displayWidth / 2f) - _offset.X) / _zoom;
                float pdfY = (mousePos.Y - _offset.Y) / _zoom;

                float zoomFactor = 1.1f;
                if (e.Delta > 0) { if (_zoom < 5.0f) _zoom *= zoomFactor; }
                else { if (_zoom > 0.1f) _zoom /= zoomFactor; }

                _offset.X = mousePos.X - (displayWidth / 2f) - (pdfX * _zoom);
                _offset.Y = mousePos.Y - (pdfY * _zoom);
            }
            else
            {
                if (_viewMode == PdfViewMode.SinglePage)
                {
                    // 【単ページ表示時】ホイール回転でページめくり
                    if (e.Delta > 0)
                    {
                        // 上に回したら前のページへ
                        ScrollToPage(_singlePageIndex - 1);
                    }
                    else
                    {
                        // 下に回したら次のページへ
                        ScrollToPage(_singlePageIndex + 1);
                    }
                }
                else
                {
                    // 【連続スクロール時】通常の上下スクロール
                    _offset.Y += e.Delta;
                }
            }

            // ★追加：動かした後に安全範囲にクランプし、スクロールバーのツマミを同期
            UpdateScrollBarRanges();
            Invalidate();
        }

        // ==============================
        // マウスクリック(左クリックを捕まえる)
        // ==============================
        private void PdfCustomViewer_MouseDown(object? sender, MouseEventArgs e)
        {
            // 左クリック
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                _lastMousePos = e.Location;
                this.Cursor = Cursors.Hand;
            }
        }

        // ==============================
        // マウスを動かしたとき(ドラッグを捕まえる)
        // ==============================
        private void PdfCustomViewer_MouseMove(object? sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                // ドラッグ移動量を加算
                _offset.X += (e.X - _lastMousePos.X);
                _offset.Y += (e.Y - _lastMousePos.Y);

                _lastMousePos = e.Location;

                // ドラッグした瞬間に安全範囲にクランプし、ツマミを動かす
                UpdateScrollBarRanges();
                Invalidate();
            }
        }

        // ==============================
        // マウスクリックを離したとき(左クリックを捕まえる)
        // ==============================
        private void PdfCustomViewer_MouseUp(object? sender, MouseEventArgs e)
        {
            // 左クリック
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = false;
                this.Cursor = Cursors.Default;
            }
        }

        // ==============================
        // 外部（サムネイルなど）から指定されたページ番号に一瞬でジャンプする
        // ==============================
        public void ScrollToPage(int pageIndex)
        {
            if (_pdfDocument == null || pageIndex < 0 || pageIndex >= PageCount) return;

            // もし「1ページ表示モード」なら、インデックスを直接書き換えて再描画
            if (_viewMode == PdfViewMode.SinglePage)
            {
                _singlePageIndex = pageIndex;
                _offset = new PointF(0, 20); // スクロール（ドラッグ位置）を中央（初期位置）に戻す
                UpdateScrollBarRanges();
                Invalidate();
                return; // ここで終了
            }

            // ※PdfCustomViewerが持つ変数（PageSpacing, _zoom）を使うため、必ずこのファイル内に記述します
            float spacing = PageSpacing * _zoom;
            float targetY = spacing;

            // 目的のページの手前までの「高さ」をすべて合計する
            for (int i = 0; i < pageIndex; i++)
            {
                var pageSizes = _pdfDocument?.PageSizes ?? 0;
                SizeF originalSize = pageSizes[i];
                float pageHeight = originalSize.Height * _zoom;

                targetY += pageHeight + spacing;
            }

            // _offset.Y をマイナス値にして、目的のページが最上部（少し余白あり）にくるようにセット
            _offset.Y = -targetY + 20;

            // スクロールバーの位置や限界値を再計算して同期
            UpdateScrollBarRanges();
            Invalidate(); // 再描画
        }

        // ==============================
        // ショートカットキー・キーボード操作の処理
        // ==============================
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // PDF開いてない、ページない場合
            if (_pdfDocument == null || PageCount == 0) return base.ProcessCmdKey(ref msg, keyData);

            // 現在のズーム倍率を考慮して1回あたりのスクロール量を計算（35pxベース）
            float scrollSpeed = 35f * _zoom;

            // PageDown、右矢印、下矢印、エンター：次のページへ進む
            if (keyData == Keys.PageDown || keyData == Keys.Right || keyData == Keys.Down || keyData == Keys.Enter)
            {
                if (_viewMode == PdfViewMode.SinglePage)
                {
                    // 【単一表示】ページインデックスを直接進める
                    if (_singlePageIndex < PageCount - 1)
                    {
                        _singlePageIndex++;
                        ResetBookmarkViewLocation();
                    }
                }
                else
                {
                    // 【連続スクロール】次のページへ自動スクロールさせる
                    // 下矢印キー（Down）：ドキュメントを上に押し上げる（スクロールダウン）
                    if (keyData == Keys.Down)
                    {
                        _offset.Y -= scrollSpeed;

                        UpdateScrollBarRanges(); // スクロールバー更新
                        Invalidate();// 再描画
                        return true; // キー処理を完了
                    }

                    // ※現在のページ（CurrentPage）が最終ページ未満なら次へ進む
                    int nextPage = this.CurrentPage + 1;
                    if (nextPage < PageCount)
                    {
                        // 以前サムネイルクリック時などに使った「ページジャンプ関数」を呼び出す
                        ScrollToPage(nextPage);
                    }
                }
                return true; // キー処理を完了
            }

            // PageUp、左矢印、上矢印、バックスペース：前のページへ戻る
            if (keyData == Keys.PageUp || keyData == Keys.Left || keyData == Keys.Up || keyData == Keys.Back)
            {
                if (_viewMode == PdfViewMode.SinglePage)
                {
                    // 【単一表示】ページインデックスを直接戻す
                    if (_singlePageIndex > 0)
                    {
                        _singlePageIndex--;
                        ResetBookmarkViewLocation();
                    }
                }
                else
                {
                    // 【連続スクロール】前のページへ自動スクロールさせる
                    // 上矢印キー（Up）：ドキュメントを下に押し下げる（スクロールアップ）
                    if (keyData == Keys.Up)
                    {
                        _offset.Y += scrollSpeed;

                        UpdateScrollBarRanges();
                        Invalidate();
                        return true; // キー処理を完了
                    }

                    int prevPage = this.CurrentPage - 1;
                    if (prevPage >= 0)
                    {
                        ScrollToPage(prevPage);
                    }
                }
                return true; // キー処理を完了
            }

            /*

            // PageDown、右矢印、エンター：次のページへ進む
            if (keyData == Keys.PageDown || keyData == Keys.Right || keyData == Keys.Enter)
            {
                if (_viewMode == PdfViewMode.SinglePage)
                {
                    // 【単一表示】ページインデックスを直接進める
                    if (_singlePageIndex < PageCount - 1)
                    {
                        _singlePageIndex++;
                        ResetBookmarkViewLocation();
                    }
                }
                else
                {
                    // 【連続スクロール】次のページへ自動スクロールさせる
                    // ※現在のページ（CurrentPage）が最終ページ未満なら次へ進む
                    int nextPage = this.CurrentPage + 1;
                    if (nextPage < PageCount)
                    {
                        // 以前サムネイルクリック時などに使った「ページジャンプ関数」を呼び出す
                        ScrollToPage(nextPage);
                    }
                }
                return true; // キー処理を完了
            }

            // PageUp、左矢印、バックスペース：前のページへ戻る
            if (keyData == Keys.PageUp || keyData == Keys.Left || keyData == Keys.Back)
            {
                if (_viewMode == PdfViewMode.SinglePage)
                {
                    // 【単一表示】ページインデックスを直接戻す
                    if (_singlePageIndex > 0)
                    {
                        _singlePageIndex--;
                        ResetBookmarkViewLocation();
                    }
                }
                else
                {
                    // 【連続スクロール】前のページへ自動スクロールさせる
                    int prevPage = this.CurrentPage - 1;
                    if (prevPage >= 0)
                    {
                        ScrollToPage(prevPage);
                    }
                }
                return true; // キー処理を完了
            }

            // 連続スクロール表示時の、上下矢印キーによるスルスル移動処理
            if (_viewMode == PdfViewMode.Continuous)
            {
                // 現在のズーム倍率を考慮して1回あたりのスクロール量を計算（35pxベース）
                float scrollSpeed = 35f * _zoom;

                // 下矢印キー（Down）：ドキュメントを上に押し上げる（スクロールダウン）
                if (keyData == Keys.Down)
                {
                    _offset.Y -= scrollSpeed;

                    UpdateScrollBarRanges(); // スクロールバー更新
                    Invalidate();// 再描画
                    return true; // キー処理を完了
                }

                // 上矢印キー（Up）：ドキュメントを下に押し下げる（スクロールアップ）
                if (keyData == Keys.Up)
                {
                    _offset.Y += scrollSpeed;

                    UpdateScrollBarRanges();
                    Invalidate();
                    return true; // キー処理を完了
                }
            }

            */

            return base.ProcessCmdKey(ref msg, keyData);
        }

        // ==============================
        // ページが切り替わったときに描画をリセットする共通関数
        // ==============================
        private void ResetBookmarkViewLocation()
        {
            _offset = new PointF(0, 20); // オフセットリセット
            UpdateScrollBarRanges(); // スクロールバー更新
            Invalidate(); // 再描画
        }
    }
}

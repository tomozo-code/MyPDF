using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Navigation;
using iText.Kernel.XMP;
using iText.Kernel.XMP.Options;
using iText.StyledXmlParser.Jsoup.Nodes;
using Org.BouncyCastle.Asn1.Cms;
using PdfiumViewer;
using System.Buffers;
using System.Diagnostics;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DrawingColor = System.Drawing.Color;
using IOPath = System.IO.Path;
using ITextDoc = iText.Kernel.Pdf.PdfDocument;
using PdfiTextReader = iText.Kernel.Pdf.PdfReader;
using PdfiumDoc = PdfiumViewer.PdfDocument;


// ==============================
// パッケージ：iText、PdfiumViewer.core
// PDFを表示、編集
// ==============================

// PDF表示 → PdfiumViewer.core
// しおり操作 → TreeView
// 保存 → iText

// セキュリティ設定の仕様
// パスなし → 編集可能
// 開くパス(User) → パス要求 → 閲覧モード(編集不可)
// 制限パス(Owner) → パス要求 → 編集モード(編集可能)
// 開くパス(User) + 制限パス(Owner) → どのパスで開いたかで　閲覧 or 編集

namespace MyPDF
{
    public partial class Form1 : Form
    {
        // 現在開いているPDFのパス(保存・再読み込みで使う)
        private string currentPdfPath = "";
        // ドラッグ＆ドロップ用
        // dropTargetNode → ドロップ先
        private TreeNode? dropTargetNode = null;
        // nsertAfter → 下に入れるか
        private bool insertAfter = false;
        // insertAsChild → 子にするか
        private bool insertAsChild = false;
        // 右クリック時に「ページジャンプしないようにするフラグ」
        private bool isRightClickSelecting = false;
        // ページ監視用（Pdfiumにイベントないので自前監視）
        private int lastPage = -1;
        private System.Windows.Forms.Timer pageTimer = new System.Windows.Forms.Timer();
        // 更新フラグ(true:更新あり、false:更新なし)
        public bool isDirty = false;
        // 今PDF開いてる？のフラグ(ture:開いてる、false:開いてない)
        private bool isOpening = false;
        // PDFがセキュリティありかチェック用のフラグ(true:なし、false:あり)
        private bool canEdit = true;

        // サムネイル関連
        // サムネイルを順番に読み込むためのインデックス
        private int thumbnailLoadIndex = 0;
        // サムネイルを非同期っぽく分割読み込みするためのタイマー
        private System.Windows.Forms.Timer thumbnailTimer = new System.Windows.Forms.Timer();

        // PDF設定・セキュリティ
        // 現在読み込んでいるPDFの各種設定（メタデータ・表示設定など）
        private PdfSettings currentSettings = new PdfSettings();
        // 現在のセキュリティ設定（パスワード・権限など）
        private SecuritySettings? currentSecurity;

        // UI補助
        // ツールチップに表示するヒント文字列
        private string? toolHintTxt = null;
        // サムネイルとページ表示の同期中かどうか（無限ループ防止用）
        private bool isSyncingThumbnail = false;

        // パスワード関連
        // 入力されたパスワード（一時用)
        private string? password = null;
        // 現在PDFを開いているときのパスワード
        private string? currentPassword;

        // アプリ名（タイトルバー表示用）
        private string myName = "ともさんのPDF編集帖";
        // ツールチップ表示用：直前にマウスが乗っていたノード
        private TreeNode? lastNode = null;

        public Form1()
        {
            InitializeComponent();

            // フォームサイズ
            this.Width = 900;
            this.Height = 600;

            // フォーム最大化
            this.WindowState = FormWindowState.Maximized;
            // アプリ名（タイトルバー表示）
            this.Text = myName;

            panel2.Dock = DockStyle.Fill;
            pdfViewer1.Dock = DockStyle.Fill;

            // pdfViewerのしおり表示を無効
            pdfViewer1.ShowBookmarks = false;
            // pdfViewerのツールバー表示を無効
            pdfViewer1.ShowToolbar = false;

            panel1.Width = 300;
            tabControl1.Dock = DockStyle.Fill;
            treeView1.Dock = DockStyle.Fill;
            listView1.Dock = DockStyle.Fill;

            toolStripStatusLabel1.Text = "ファイル: PDF未選択";
            TotalPageLabel.Text = "/ 総ページ数";
            toolHintTxt = "ファイル: PDF未選択";

            // しおり列選択 代わりに線なし
            treeView1.FullRowSelect = true;
            treeView1.ShowLines = false;

        }

        // ==============================
        // フォームが読み込まれたとき    
        // ==============================
        private async void Form1_Load(object sender, EventArgs e)
        {

            // ショートカットキーの設定
            // Ctrl+O(開く)
            OpenToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            // Ctrl + G(既定のPDFアプリで開く)
            AcrobatOpenToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.G;
            // Ctrl+S(上書き保存)
            SaveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            // Ctrl+B(しおり作成)
            AddShioriToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.B;
            // Ctrl+D(PDFのプロパティ)
            PdfSetToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D;
            // Ctrl+T(セキュリティ設定)
            SecurityToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.T;
            // Ctrl+A(しおりのプロパティ)
            ShioriProToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.A;
            // Deleteキー(しおり削除)
            //DelShioriToolStripMenuItem.ShortcutKeys = Keys.Delete;
            // Ctrl+L(ページ割り当て)数値キーを上段(テンキーではない)
            SetShioriToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.L;
            // 全てのしおりを展開
            AllShioriTenkaiToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D1;
            // 全てのしおりを縮小
            AllShioriSyukusyouToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D2;
            // 選択中のしおりを展開
            ShioriTenkaiToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D3;
            // 選択中のしおりを縮小
            ShioriSyukusyouToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.D4;
            // しおりインポート
            ImportShioriToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.I;
            // しおりエクスポート
            ExportShioriToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.E;


            // 0.1秒ごとにページを監視
            pageTimer.Interval = 100; // 0.1秒ごと
            pageTimer.Tick += PageTimer_Tick;
            pageTimer.Start();

            // サムネイル用 0.05秒ごと
            thumbnailTimer.Interval = 50; // 0.05秒ごと（調整OK）
            thumbnailTimer.Tick += ThumbnailTimer_Tick;

            // ZoomModeの選択
            ZoomComboBox.Items.AddRange(new object[] { "自動調整", "高さに合わせる", "幅に合わせる" });
            // 自動調整
            ZoomComboBox.SelectedIndex = 0;

            // 更新をリセット
            isDirty = false;
        }

        // ==============================
        // ページスクロールで今のページ番号を取得    
        // ==============================
        private void PageTimer_Tick(object? sender, EventArgs e)
        {
            if (pdfViewer1.Document == null) return;

            // 現在ページ取得（0始まり）
            int current = pdfViewer1.Renderer.Page;

            // ページ変わった時だけ更新
            if (current != lastPage)
            {
                lastPage = current;
                // NowPageTxtにページ番号を表示
                NowPageTxt.Text = (current + 1).ToString();

                // 表示ページのサムネイルを強調
                SyncThumbnailSelection();

            }
        }

        // ==============================
        // 開くを押したとき    
        // ==============================
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 更新があれば保存するか聞く
            if (!ConfirmSave(() => SavePdfCore(currentPdfPath, currentPdfPath, true)))
                return;

            // PDF今開いてる?(trueなら無視(戻る))
            if (isOpening) return;
            isOpening = true;

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
                        // フォーム全体を操作不可にする
                        this.Enabled = false;

                        try
                        {
                            // 開く処理へ
                            OpenPdf(ofd.FileName);
                        }
                        finally
                        {
                            // 例外が出てもフォームを操作可能にする
                            this.Enabled = true;
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
        // 開く処理
        // ==============================
        private void OpenPdf(string path)
        {

            ITextDoc? iTextDoc = null;
            PdfReader? reader = null;
            // パスを一回リセット
            password = null;
            currentPassword = null;

            while (true)
            {
                try
                {
                    if (password == null)
                    {
                        // まずはパス無しで開く
                        reader = new PdfReader(path);
                    }
                    else
                    {
                        // パス入力済みなら
                        var props = new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(password));
                        // パス付きで開く
                        reader = new PdfReader(path, props);
                    }

                    // PDFを実際に開く
                    iTextDoc = new ITextDoc(reader);

                    // 管理者(制限パス)で開いてる true:制限パス false:以外)
                    bool isOwner = reader.IsOpenedWithFullPermission();

                    // 暗号化されてる？
                    bool isEncrypted_c = reader.IsEncrypted();

                    Debug.WriteLine("パスワード: " + password + " isEncrypted_c: " + isEncrypted_c + " isOwner: " + isOwner);

                    // ここが追加ポイント
                    if (password == null && isEncrypted_c && !isOwner)
                    {
                        // Ownerパスだけ設定されているPDF
                        // → 強制的にパス入力させる
                        iTextDoc.Close();

                        password = ShowPasswordDialog();

                        if (password == null)
                            return;

                        continue; // 再トライ
                    }

                    canEdit = isOwner;

                    Debug.WriteLine("-----セキュリティ------------------------");
                    Debug.WriteLine("どのパスで開いた(true:制限パスまたはパスなし false:以外): " + isOwner);

                    // セキュリティ情報保持
                    currentSecurity ??= new SecuritySettings();
                    currentSecurity.IsOwnerOpened = isOwner;

                    if (isOwner)
                    {
                        // Ownerパスで開いた
                        currentSecurity.OwnerPassword = password ?? "";
                        currentSecurity.UserPassword = null;

                    }
                    else
                    {
                        // Userパスで開いた
                        currentSecurity.UserPassword = password ?? "";
                        currentSecurity.OwnerPassword = null;

                    }

                    // ループ抜ける
                    break;

                }
                catch
                {
                    // 失敗したらパス入力
                    password = ShowPasswordDialog();

                    if (password == null)
                    {
                        // キャンセル
                        return;
                    }
                }
            }

            treeView1.LabelEdit = canEdit;

            iTextDoc.Close();

            // 開いているPDFファイルが有れば解放
            if (pdfViewer1.Document != null)
            {
                pdfViewer1.Document.Dispose();
                pdfViewer1.Document = null;

                for (int i = 0; i < 3; i++)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }

            }

            // ファイルを記憶
            currentPdfPath = path;

            // Pdfiumで表示
            PdfiumViewer.PdfDocument document;

            if (password == null)
            {
                document = PdfiumViewer.PdfDocument.Load(path);
            }
            else
            {
                document = PdfiumViewer.PdfDocument.Load(path, password);
            }

            pdfViewer1.Document = document;

            string? openPassword = password ?? currentPassword;

            // iTextでしおり取得
            ShowBookmarks(currentPdfPath, openPassword);

            // ツリービューの右クリックメニュー ON/OFF
            UpdateContextMenuState();

            // 自動調整
            ZoomComboBox.SelectedIndex = 0;
            pdfViewer1.ZoomMode = PdfViewerZoomMode.FitBest;

            // サムネイル表示
            GenerateThumbnails();

            // 表示ページのサムネイルを強調
            SyncThumbnailSelection();

            // ページ番号「1」を表示
            NowPageTxt.Text = "1";

            // 保存との整合性
            currentSettings = LoadPdfSettings(currentPdfPath, openPassword);

            currentPassword = password;

            Debug.WriteLine("入力されたパスワード: " + password);

            // 暗号化されてる？
            bool isEncrypted = reader.IsEncrypted();

            // 権限取得
            int perm = reader.GetPermissions();

            // 暗号方式
            currentSecurity.Encryption = reader.GetCryptoMode();

            currentSecurity.Check_Owner = false;
            currentSecurity.Check_User = false;

            string fileName = IOPath.GetFileName(path);

            this.Text = myName + " - [ " + fileName + " ]";


            if (!canEdit)
            {
                this.Text = myName + " - [ " + fileName + "(閲覧モード) ]";

                MessageBox.Show(
                    "[ " + fileName + " ] は制限が設定されています。" + Environment.NewLine + "編集不可です。",
                    "制限確認(編集不可)",
                    MessageBoxButtons.OK, MessageBoxIcon.Information
                );
            }

            // 更新をリセット
            isDirty = false;

        }

        // ==============================
        // 開くときにパスワードがある場合のダイアログ
        // ==============================
        private string? ShowPasswordDialog()
        {
            using (var f = new Form5())
            {
                var result = f.ShowDialog();

                if (result == DialogResult.OK)
                {
                    return f.Password;
                }

                return null;
            }
        }


        // ==============================
        // PDFの設定情報を読み込む
        // ==============================
        private PdfSettings LoadPdfSettings(string path, string? password = null)
        {
            var settings = new PdfSettings();

            // ファイル名
            settings.PdfFileName = IOPath.GetFileName(path);
            // パス
            settings.PdfPath = IOPath.GetDirectoryName(path);

            PdfReader reader;

            try
            {
                if (!string.IsNullOrEmpty(password))
                {
                    reader = new PdfReader(
                        path,
                        new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(password))
                    );
                }
                else
                {
                    reader = new PdfReader(path);
                }
            }
            catch
            {
                // フォールバック（念のため）
                reader = new PdfReader(path);
            }

            using (var pdf = new ITextDoc(reader))
            {
                // iTextでプロパティ取得
                var info = pdf.GetDocumentInfo();

                // iTextでPDFのページ数取得
                int pageCount = pdf.GetNumberOfPages();

                // ステータスバーにファイル名と総ページ数
                UpdateStatus(path, pageCount);

                // 総ページ数
                settings.TotalPage = pageCount;

                // ファイルサイズ
                var fi = new FileInfo(path);
                long bytes = fi.Length;
                double kb = bytes / 1024.0;
                settings.FileSize_bytes = bytes;
                settings.FileSize_Kb = kb;

                // PDFファイルのバージョン
                settings.PdfFileVer = pdf.GetPdfVersion().ToString();

                // ページサイズ取得(pt)
                var page = pdf.GetFirstPage();
                var size = page.GetPageSize();
                // ptをmmに変換
                float PtToMm(float pt)
                {
                    return pt * 25.4f / 72f;
                }
                float widthMm = PtToMm(size.GetWidth());
                float heightMm = PtToMm(size.GetHeight());
                settings.PageSize_W = widthMm;
                settings.PageSize_H = heightMm;

                // 概要
                // タイトル
                settings.Title = info.GetTitle() ?? "";
                // 作成者
                settings.Author = info.GetAuthor() ?? "";
                // サブタイトル
                settings.Subject = info.GetSubject() ?? "";
                // キーワード
                //settings.Keywords = info.GetKeywords() ?? "";
                string keywords = "";

                var xmp = pdf.GetXmpMetadata(); // ←これXMPMeta

                if (xmp != null)
                {
                    try
                    {
                        int count = xmp.CountArrayItems(XMPConst.NS_DC, "subject");

                        List<string> list = new List<string>();

                        for (int i = 1; i <= count; i++)
                        {
                            var item = xmp.GetArrayItem(XMPConst.NS_DC, "subject", i);
                            if (item != null)
                            {
                                list.Add(item.GetValue());
                            }
                        }

                        keywords = string.Join(Environment.NewLine, list);
                    }
                    catch
                    {
                        keywords = info.GetKeywords() ?? "";
                    }
                }
                else
                {
                    keywords = info.GetKeywords() ?? "";
                }

                settings.Keywords = keywords;

                // アプリケーション
                settings.Creator = info.GetCreator() ?? "";
                // PDF変換
                settings.Producer = info.GetProducer() ?? "";
                // 作成日
                settings.CreationDate = FormatPdfDate(info.GetMoreInfo("CreationDate")) ?? "";
                // 更新日
                settings.ModDate = FormatPdfDate(info.GetMoreInfo("ModDate")) ?? "";


                // デバッグ出力確認
                Debug.WriteLine("-----LoadPdfSettings------------------------");
                Debug.WriteLine("ファイル名: " + settings.PdfFileName);
                Debug.WriteLine("パス名: " + settings.PdfPath);
                Debug.WriteLine("総ページ数: " + settings.TotalPage);
                Debug.WriteLine("ファイルサイズ_b: " + settings.FileSize_bytes);
                Debug.WriteLine("ファイルサイズ_Kb: " + settings.FileSize_Kb);
                Debug.WriteLine("PDFファイルのバージョン: " + settings.PdfFileVer);
                Debug.WriteLine("ページサイズ_幅: " + settings.PageSize_W);
                Debug.WriteLine("ページサイズ_高さ: " + settings.PageSize_H);
                Debug.WriteLine("タイトル: " + settings.Title);
                Debug.WriteLine("作成者: " + settings.Author);
                Debug.WriteLine("サブタイトル: " + settings.Subject);
                Debug.WriteLine("キーワード: " + settings.Keywords);
                Debug.WriteLine("アプリケーション: " + settings.Creator);
                Debug.WriteLine("PDF変換: " + settings.Producer);
                Debug.WriteLine("作成日: " + settings.CreationDate);
                Debug.WriteLine("更新日: " + settings.ModDate);
                Debug.WriteLine("入力PW: " + password);

                // 開き方
                var catalog = pdf.GetCatalog();
                var catalogObj = catalog.GetPdfObject();

                // 表示モード
                var pageMode = catalogObj.GetAsName(PdfName.PageMode);
                settings.PageMode = pageMode?.GetValue();
                //var pageMode = catalogObj.GetAsName(PdfName.PageMode);
                //settings.PageMode = pageMode?.GetValue() ?? "UseNone";

                // レイアウト
                var layout = catalogObj.GetAsName(PdfName.PageLayout);
                settings.PageLayout = layout?.GetValue();
                //var layout = catalogObj.GetAsName(PdfName.PageLayout);
                //settings.PageLayout = layout?.GetValue() ?? "SinglePage";

                var openAction = catalogObj.Get(PdfName.OpenAction);

                if (openAction is PdfArray arr && arr.Size() >= 2)
                {
                    try
                    {
                        // ページ取得
                        var pageObj = arr.Get(0);

                        for (int i = 1; i <= pdf.GetNumberOfPages(); i++)
                        {
                            if (pdf.GetPage(i).GetPdfObject() == pageObj)
                            {
                                settings.OpenPage = i;
                                break;
                            }
                        }

                        // 表示タイプ
                        var type = arr.GetAsName(1);

                        if (type != null)
                        {
                            switch (type.GetValue())
                            {
                                case "XYZ":
                                    var zoomObj = arr.Size() > 4 ? arr.Get(4) : null;

                                    if (zoomObj is PdfNumber zoomNum)
                                    {
                                        settings.Zoom = (zoomNum.FloatValue() * 100).ToString("0") + "%";
                                    }
                                    else
                                    {
                                        // 100%
                                        settings.Zoom = "100%";
                                    }
                                    break;

                                case "Fit":
                                    // 全体表示
                                    settings.Zoom = "全体表示";
                                    break;

                                case "FitH":
                                    // 幅に合わせる
                                    settings.Zoom = "幅に合わせる";
                                    break;

                                case "FitV":
                                    // 高さに合わせる
                                    settings.Zoom = "高さに合わせる";
                                    break;

                                default:
                                    // デフォルト
                                    settings.Zoom = "デフォルト";
                                    break;
                            }
                        }
                    }
                    catch
                    {
                        // 壊れてるPDF対策
                        settings.OpenPage = 1;
                        settings.Zoom = "全体表示";

                    }

                    // デバッグ出力確認
                    Debug.WriteLine("-----開き方(LoadPdfSettings)------------------------");
                    Debug.WriteLine("表示: " + settings.PageMode);
                    Debug.WriteLine("ページレイアウト: " + settings.PageLayout);
                    Debug.WriteLine("倍率: " + settings.Zoom);
                    Debug.WriteLine("開始ページ" + settings.OpenPage);

                }

            }

            return settings;
        }

        // ==============================
        // 日付変換メソッド
        // ==============================
        private string FormatPdfDate(string pdfDate)
        {
            if (string.IsNullOrWhiteSpace(pdfDate))
                return "";

            try
            {
                // "D:" を除去
                if (pdfDate.StartsWith("D:"))
                    pdfDate = pdfDate.Substring(2);

                // タイムゾーン部分を除去（+09'00' など）
                int tzIndex = pdfDate.IndexOfAny(new char[] { '+', '-' });
                if (tzIndex > 0)
                    pdfDate = pdfDate.Substring(0, tzIndex);

                // 最低14桁必要（yyyyMMddHHmmss）
                if (pdfDate.Length < 14)
                    return pdfDate;

                string datePart = pdfDate.Substring(0, 14);

                var dt = DateTime.ParseExact(
                    datePart,
                    "yyyyMMddHHmmss",
                    System.Globalization.CultureInfo.InvariantCulture
                );

                return dt.ToString("yyyy/MM/dd HH:mm:ss");
            }
            catch
            {
                // 変換失敗したら元のまま
                return pdfDate;
            }
        }

        // ==============================
        // PDFのしおり取得
        // ==============================
        private void ShowBookmarks(string path, string? password = null)
        {
            // ツリービューを初期化
            treeView1.Nodes.Clear();
            // 選択解除（安全）
            treeView1.SelectedNode = null;
            // ツールチップ消す
            treeToolTip.SetToolTip(treeView1, "");

            try
            {
                ReaderProperties props = new ReaderProperties();
                if (!string.IsNullOrEmpty(password))
                {
                    props.SetPassword(Encoding.UTF8.GetBytes(password));
                }

                using (var pdf = new ITextDoc(new PdfiTextReader(path, props)))
                {
                    // iTextでPDFのしおりを取得
                    var outlines = pdf.GetOutlines(false);

                    if (outlines == null)
                    {
                        //MessageBox.Show("しおりなし");
                        return;
                    }

                    var root = outlines.GetAllChildren();

                    if (root.Count == 0)
                    {
                        //MessageBox.Show("しおりなし");
                        return;
                    }

                    // ルートから再帰でしおり追加
                    foreach (var item in root)
                    {
                        // 再帰でTreeViewに変換
                        AddBookmarkNode(item, treeView1.Nodes, pdf);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"エラー:\n{ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ==============================
        // PDFのしおりをツリービューに表示
        // PDFのしおり1個 → TreeViewのノード1個に変換
        // ==============================
        private void AddBookmarkNode(PdfOutline outline, TreeNodeCollection nodes, ITextDoc pdf)
        {
            try
            {
                int pageNumber = 0;

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

                // ページ番号取得
                // デバッグ出力確認
                Debug.WriteLine("----しおり----------------------");
                Debug.WriteLine(outline.GetTitle());
                Debug.WriteLine(destObj?.GetType());
                Debug.WriteLine(destObj);

                if (destObj is PdfArray arr && arr.Size() > 0)
                {
                    var pageDict = arr.GetAsDictionary(0);

                    if (pageDict != null)
                    {
                        var page = pdf.GetPage(pageDict);
                        pageNumber = pdf.GetPageNumber(page);
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
                                var page = pdf.GetPage(pageDict);
                                pageNumber = pdf.GetPageNumber(page);
                            }
                        }
                    }
                }

                // ノード作成
                string title = outline.GetTitle() ?? "(no title)";

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

                Debug.WriteLine("-----文字スタイル1------------------------");
                Debug.WriteLine("selectedStyle: " + selectedStyle);

                var node = new TreeNode(title)
                {
                    Tag = new BookmarkInfo
                    {
                        // しおり名
                        BmTitle = title,
                        //　ページ番号
                        Page = pageNumber,
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

                nodes.Add(node);

                Debug.WriteLine("-----しおり名 → ページ番号------------------------");
                Debug.WriteLine($"{title} → Page:{pageNumber}");

                // 再帰
                foreach (var child in outline.GetAllChildren())
                {
                    AddBookmarkNode(child, node.Nodes, pdf);
                }

                // 最後にCollapse（順番重要）
                if (isOpen)
                {
                    node.Expand();
                }
                else
                {
                    node.Collapse();
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"しおり解析エラー: {ex.Message}");
            }
        }


        // ==============================
        // ツリービューでキーを押したとき
        // ==============================
        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            // F2で編集
            if (e.KeyCode == Keys.F2 && treeView1.SelectedNode != null)
            {
                treeView1.SelectedNode.BeginEdit();
            }

            // Deleteで削除
            if (e.KeyCode == Keys.Delete && treeView1.SelectedNode != null)
            {
                var node = treeView1.SelectedNode;

                if (MessageBox.Show($"「{node.Text}」を削除しますか？" + Environment.NewLine + "配下のしおりも削除されます。",
                    "しおり削除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    return;

                node.Remove();

                // ツリービューの右クリックメニュー ON/OFF
                UpdateContextMenuState();

                // 更新フラグ(falseならtrueに)
                //if (!isDirty)
                isDirty = true;
            }

        }

        // ==============================
        // ツリービューのしおり編集後
        // ==============================
        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            // 念のため nullチェック（保険）
            if (e.Node == null) return;

            // キャンセルされた場合
            if (e.Label == null) return;

            // 空文字防止
            if (string.IsNullOrWhiteSpace(e.Label))
            {
                MessageBox.Show("しおり名は空にできません。", "しおり名確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.CancelEdit = true;
                return;
            }

            // BookmarkInfoに反映
            var info = e.Node.Tag as BookmarkInfo;
            if (info != null)
            {
                var title = string.IsNullOrWhiteSpace(e.Label)
                    ? e.Node.Text
                    : e.Label;

                // 表示とデータのズレがないように
                info.BmTitle = title;
            }

            // 変更されてない場合は無視
            if (e.Node.Text == e.Label)
                return;

            // 変更確定 → フラグON
            //if (!isDirty)
            isDirty = true;

            // デバッグ確認
            Debug.WriteLine($"しおり名変更: {e.Node.Text} → {e.Label}");


        }



        // ==============================
        // ツリービューのしおりを押したとき
        // ==============================
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

            if (isRightClickSelecting)
            {
                // 右クリック時はジャンプしない(ページ移動させない)
                isRightClickSelecting = false;
                return; // ←ジャンプさせない
            }

            if (isOpening) return;

            Debug.WriteLine(e.Node?.Tag?.GetType());

            // クリックしたしおりのページを取得
            if (e.Node?.Tag is BookmarkInfo info && info.Page > 0)
            {
                // 取得したページへジャンプ
                JumpToPage(info.Page);
            }

        }


        // ==============================
        // しおりのページにジャンプ
        // ==============================
        private void JumpToPage(int page)
        {

            if (pdfViewer1.Document == null) return;

            // Pdfiumは0始まりなので -1
            pdfViewer1.Renderer.Page = page - 1;

            //MessageBox.Show("クリックしたしおりのページ番号:" + pdfViewer1.Renderer.Page.ToString());
        }



        // ==============================
        // ステータスバーにファイル名、総ページ数
        // ==============================
        private void UpdateStatus(string path, int pageCount)
        {

            // ステータスバーにフルパスと総ページ数を表示
            toolStripStatusLabel1.Text = $"パス: {path} | 総ページ数: {pageCount}";
            TotalPageLabel.Text = $"/ {pageCount}";

            toolHintTxt = $"パス: {path} | 総ページ数: {pageCount}";

            // フォームタイトル
            //this.Text = myName + " - [ " + IOPath.GetFileName(path) + " ]"; 

        }


        // ==============================
        // 上書き保存を押したとき
        // ==============================
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(currentPdfPath)) return;

            if (SavePdfCore(currentPdfPath, currentPdfPath, true))
            {
                MessageBox.Show("上書き保存しました。", "上書き保存確認", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }

        // ==============================
        // 名前を付けて保存を押したとき
        // ==============================
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(currentPdfPath)) return;

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "名前を付けてPDFファイルを保存";
                sfd.Filter = "PDFファイル (*.pdf)|*.pdf";
                sfd.FileName = IOPath.GetFileName(currentPdfPath);

                if (sfd.ShowDialog() != DialogResult.OK) return;

                if (SavePdfCore(currentPdfPath, sfd.FileName, false))
                {
                    MessageBox.Show("名前を付けて保存しました。", "名前を付けて保存確認", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }

        // ==============================
        // 保存確認ダイアログ
        // ==============================
        private bool ConfirmSave(Func<bool> saveAction)
        {
            if (!isDirty)
                return true;

            var result = MessageBox.Show(
                "更新されています。上書き保存しますか？" + Environment.NewLine + "はい(Y)：上書き保存、いいえ(N)：保存しない(更新を破棄)",
                "更新確認",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                // ここがミソ
                return saveAction();
            }
            else if (result == DialogResult.No)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        // ==============================
        // 保存処理
        // ==============================
        private bool SavePdfCore(string inputPath, string outputPath, bool overwrite)
        {
            string tempPath = overwrite ? inputPath + ".tmp" : outputPath;
            // パスが設定されたフラグ(true:パスあり、false:パスなし)
            bool MsgFlag = false;

            try
            {
                if (overwrite && File.Exists(tempPath))
                    File.Delete(tempPath);

                int currentPage = pdfViewer1.Document != null ? pdfViewer1.Renderer.Page : 0;

                // Pdfium解放、ファイルロック解除
                pdfViewer1.Document?.Dispose();
                pdfViewer1.Document = null;

                for (int i = 0; i < 3; i++)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }

                // PDFを書き出すためのオブジェクトを用意
                PdfWriter writer;

                // 制限チェックと開くのどっちかにチェックが入ってたらセキュリティ設定する
                if (currentSecurity != null && (currentSecurity.Check_Owner || currentSecurity.Check_User))
                {
                    // パスワード設定(開くパス)（UTF-8バイト配列に変換）
                    byte[]? userPass = currentSecurity.Check_User && !string.IsNullOrEmpty(currentSecurity.UserPassword)
                        ? Encoding.UTF8.GetBytes(currentSecurity.UserPassword)
                        : null;

                    // パスワード設定(権限パス)（UTF-8バイト配列に変換）
                    byte[]? ownerPass = currentSecurity.Check_Owner && !string.IsNullOrEmpty(currentSecurity.OwnerPassword)
                        ? Encoding.UTF8.GetBytes(currentSecurity.OwnerPassword)
                        : null;

                    writer = new PdfWriter(
                        tempPath,
                        new WriterProperties().SetStandardEncryption(
                            // パスワード設定(開くパス)
                            userPass,
                            // パスワード設定(権限パス)
                            ownerPass,
                            // 操作制御(印刷許可とか)
                            currentSecurity.Permissions,
                            // 暗号方式
                            currentSecurity.Encryption
                        )
                    );
                    // パスありなのでtrueに
                    MsgFlag = true;
                }
                else
                {
                    // セキュリティなし
                    writer = new PdfWriter(tempPath);
                }

                // 新ファイル(tmp)に書き出し
                ReaderProperties props;
                // パスが空白なら事故るので空白チェック
                if (string.IsNullOrEmpty(currentPassword))
                {
                    props = new ReaderProperties();
                }
                else
                {
                    props = new ReaderProperties()
                        .SetPassword(Encoding.UTF8.GetBytes(currentPassword));
                }

                using (var pdf = new ITextDoc(new PdfiTextReader(inputPath, props), writer))

                {
                    // 既存しおりを削除
                    pdf.GetCatalog().Remove(PdfName.Outlines);
                    // しおりを新規作成
                    var outlines = pdf.GetOutlines(true);
                    outlines.SetOpen(true);

                    // ツリービューをPDFへ
                    foreach (TreeNode node in treeView1.Nodes)
                    {
                        AddOutlineFromNode(pdf, outlines, node);
                    }

                    // 既存メタデータを完全クリア(info)
                    pdf.GetTrailer().Remove(PdfName.Info);
                    // 既存メタデータを完全クリア(XMP)
                    pdf.GetCatalog().Remove(PdfName.Metadata);

                    // 新しいinfoを作り直す
                    var info = pdf.GetDocumentInfo();

                    // 空対策
                    string Clean(string? s) => string.IsNullOrWhiteSpace(s) ? "" : s.Trim();

                    // タイトル
                    string title = Clean(currentSettings.Title);
                    // 作成者
                    string author = Clean(currentSettings.Author);
                    // サブタイトル
                    string subject = Clean(currentSettings.Subject);
                    // キーワード
                    string keywordsRaw = Clean(currentSettings.Keywords);
                    // PDF変換
                    //string producer = myName + " + iText(AGPL)";
                    // アプリケーション
                    //string appName = myName;


                    // 改行で分割
                    var keywordList = keywordsRaw
                        // Acrobatの改行入力に対応
                        .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(k => k.Trim())
                        .Where(k => !string.IsNullOrEmpty(k))
                        .Distinct()
                        .ToList();

                    info.SetTitle(title);
                    info.SetAuthor(author);
                    info.SetSubject(subject);

                    // Info（セミコロン区切り）
                    info.SetKeywords(string.Join("; ", keywordList));

                    //info.SetProducer(producer);
                    //info.SetCreator(appName);


                    // XMP作成
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
                    // キーワード（一旦消して配列で正しく入れる）
                    //xmp.SetProperty(XMPConst.NS_DC, "subject", keywords);
                    var opt = new PropertyOptions(PropertyOptions.ARRAY_ORDERED);

                    foreach (var k in keywordList.Distinct())
                    {
                        xmp.AppendArrayItem(
                            XMPConst.NS_DC,
                            "subject",
                            opt,
                            k,
                            null
                        );
                    }

                    // PDF情報
                    //xmp.SetProperty(XMPConst.NS_PDF, "Producer", producer);
                    // CreatorTool(クリエーターツール)
                    //xmp.SetProperty(XMPConst.NS_XMP, "CreatorTool", producer);
                    // PDFにXMP設定
                    pdf.SetXmpMetadata(xmp);


                    // 表示設定
                    var catalog = pdf.GetCatalog();
                    // 表示モード
                    switch (currentSettings.PageMode)
                    {
                        case "UseNone":
                            catalog.SetPageMode(PdfName.UseNone);
                            break;

                        case "UseOutlines":
                            catalog.SetPageMode(PdfName.UseOutlines);
                            break;

                        case "UseThumbs":
                            catalog.SetPageMode(PdfName.UseThumbs);
                            break;

                        case "UseAttachments":
                            catalog.SetPageMode(PdfName.UseAttachments);
                            break;

                        case "UseOC":
                            catalog.SetPageMode(PdfName.UseOC);
                            break;
                    }

                    // レイアウト
                    switch (currentSettings.PageLayout)
                    {
                        case "SinglePage":
                            catalog.SetPageLayout(PdfName.SinglePage);
                            break;

                        case "OneColumn":
                            catalog.SetPageLayout(PdfName.OneColumn);
                            break;

                        case "TwoColumnLeft":
                            catalog.SetPageLayout(PdfName.TwoColumnLeft);
                            break;

                        case "TwoPageLeft":
                            catalog.SetPageLayout(PdfName.TwoPageLeft);
                            break;
                    }

                    // ページ範囲チェック
                    int max = pdf.GetNumberOfPages();
                    int page = Math.Max(1, Math.Min(currentSettings.OpenPage, max));
                    var p = pdf.GetPage(page);

                    // 倍率
                    switch (currentSettings.Zoom)
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

                }

                // 上書きの場合だけ置き換え
                if (overwrite)
                {
                    File.Delete(inputPath);
                    File.Move(tempPath, inputPath);
                }

                // 再読み込み
                string loadPath = overwrite ? inputPath : outputPath;

                // OK
                string? openPassword = null;
                string? setOwnerPassword = null;
                string? setUserPassword = null;
                string? MessageEdit = null;

                if (currentSecurity != null)
                {

                    if (!string.IsNullOrEmpty(currentSecurity.UserPassword))
                    {
                        setUserPassword = currentSecurity.UserPassword;
                    }
                    if (!string.IsNullOrEmpty(currentSecurity.OwnerPassword))
                    {
                        setOwnerPassword = currentSecurity.OwnerPassword;
                    }

                    // 権限パスを優先
                    if (!string.IsNullOrEmpty(currentSecurity.OwnerPassword))
                    {
                        openPassword = currentSecurity.OwnerPassword;
                    }
                    else if (!string.IsNullOrEmpty(currentSecurity.UserPassword))
                    {
                        openPassword = currentSecurity.UserPassword;
                    }
                }

                PdfiumViewer.PdfDocument doc;

                try
                {
                    if (!string.IsNullOrEmpty(openPassword))
                        // パスあり
                        doc = PdfiumViewer.PdfDocument.Load(loadPath, openPassword);
                    else
                        // パスなし
                        doc = PdfiumViewer.PdfDocument.Load(loadPath);
                }
                catch
                {
                    // 念のためフォールバック
                    doc = PdfiumViewer.PdfDocument.Load(loadPath);
                }

                pdfViewer1.Document = doc;

                if (currentPage < doc.PageCount)
                    pdfViewer1.Renderer.Page = currentPage;

                // PDFのプロパティを読み込み
                currentPdfPath = loadPath;

                currentSettings = LoadPdfSettings(currentPdfPath, openPassword);

                // ツリービューの右クリックメニュー ON/OFF
                UpdateContextMenuState();

                string fileName = IOPath.GetFileName(loadPath);

                this.Text = myName + " - [ " + fileName + " ]";

                if (MsgFlag)
                {

                    if (string.IsNullOrEmpty(setUserPassword))
                    {
                        MessageEdit = "権限パスワードは、 " + setOwnerPassword + " です。" + Environment.NewLine +
                            "閲覧パスワードは、設定されていません。";
                    }
                    else
                    {

                        MessageEdit = "権限パスワードは、 " + setOwnerPassword + " です。" + Environment.NewLine +
                            "閲覧パスワードは、 " + setUserPassword + " です。";

                    }

                    MessageBox.Show(
                        "[ " + fileName + " ] はセキュリティが設定されました。" + Environment.NewLine + MessageEdit,
                        "セキュリティ確認",
                        MessageBoxButtons.OK, MessageBoxIcon.Information
                    );
                }

                currentPassword = openPassword;
                currentSecurity?.IsOwnerOpened = !string.IsNullOrEmpty(currentSecurity.OwnerPassword);


                // 更新をリセット
                isDirty = false;

                return true;
            }
            catch (IOException ioEx)
            {
                // ログ出す
                Debug.WriteLine(ioEx.ToString());

                // ファイルロック系
                MessageBox.Show(
                    "他のアプリケーションで開かれているため保存できません。" + Environment.NewLine
                    + "PDFを閉じてから再度保存してください。" + Environment.NewLine + Environment.NewLine
                    + inputPath,
                    "保存エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return false;
            }
            catch (Exception ex)
            {
                // それ以外
                // ログ出す
                Debug.WriteLine(ex.ToString());

                MessageBox.Show(
                    "保存エラー:\n" + ex.ToString(),
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return false;
            }
            finally
            {
                try
                {
                    if (overwrite && File.Exists(tempPath))
                        File.Delete(tempPath);
                }
                catch { }
            }
        }


        // ==============================
        // ツリービューをPDFへ の処理
        // ==============================
        private void AddOutlineFromNode(ITextDoc pdf, PdfOutline parent, TreeNode node)
        {

            // 安全に取り出す
            if (node.Tag is not BookmarkInfo info) return;

            int page = info.Page;

            // しおり作成
            var outline = parent.AddOutline(node.Text);

            // ページリンク設定
            outline.AddDestination(PdfExplicitDestination.CreateFit(pdf.GetPage(page)));


            // 色の設定
            var c = info.SelectedColor;
            outline.SetColor(new DeviceRgb(c.R, c.G, c.B));

            int style = 0;

            // 両方通ると ボールドイタリックになるはず
            // しおり ボールド
            if (info.SelectedStyle.HasFlag(FontStyle.Bold))
                style |= PdfOutline.FLAG_BOLD;
            //しおり イタリック
            if (info.SelectedStyle.HasFlag(FontStyle.Italic))
                style |= PdfOutline.FLAG_ITALIC;

            Debug.WriteLine("-----文字スタイル2------------------------");
            Debug.WriteLine("style: " + style);

            outline.SetStyle(style);

            // "/F"を強制的に書き込む
            outline.GetContent().Put(
                PdfName.F,
                new PdfNumber(style)
            );

            Debug.WriteLine("保存直後 style: " + outline.GetStyle());

            var dict = outline.GetContent();
            var f = dict.GetAsNumber(PdfName.F);

            Debug.WriteLine("F raw: " + f);

            // しおりの展開状態
            outline.SetOpen(info.IsOpen);

            // 子ノード再帰(階層構造維持)(再帰)
            foreach (TreeNode child in node.Nodes)
            {
                AddOutlineFromNode(pdf, outline, child);
            }

        }

        // ==============================
        // ツリービューの右クリックメニュー 「しおり追加」「しおり削除」のON/OFF
        // ==============================
        private void UpdateContextMenuState()
        {
            bool hasNodes = treeView1.Nodes.Count > 0;

            if (!canEdit)
            {
                // セキュリティありの場合
                // しおり作成
                AddShioriToolStripMenuItem.Enabled = false;
                // しおり削除
                DelShioriToolStripMenuItem.Enabled = false;
                // 全てのしおり削除
                AllDelToolStripMenuItem.Enabled = false;
                // 現在のページ番号をしおりに設定
                SetShioriToolStripMenuItem.Enabled = false;

                // 上書き保存
                SaveToolStripMenuItem.Enabled = false;
                // 名前を付けて保存
                SaveAsToolStripMenuItem.Enabled = false;
                // 既定のPDFアプリで開く
                AcrobatOpenToolStripMenuItem.Enabled = false;
                // PDFのプロパティ
                PdfSetToolStripMenuItem.Enabled = false;
                // セキュリティ設定
                SecurityToolStripMenuItem.Enabled = false;
                // しおりのプロパティ
                ShioriProToolStripMenuItem.Enabled = false;
                // しおりインポート
                ImportShioriToolStripMenuItem.Enabled = false;
                // しおりエクスポート
                ExportShioriToolStripMenuItem.Enabled = false;

            }
            else
            {
                // セキュリティなしの場合
                // しおり作成
                AddShioriToolStripMenuItem.Enabled = true;     // 常にOK
                // しおり削除
                DelShioriToolStripMenuItem.Enabled = hasNodes; // ノードある時だけ
                // 全てのしおり削除
                AllDelToolStripMenuItem.Enabled = hasNodes; // ノードある時だけ
                // 現在のページ番号をしおりに設定
                SetShioriToolStripMenuItem.Enabled = hasNodes; // ノードある時だけ

                // 上書き保存
                SaveToolStripMenuItem.Enabled = true;
                // 名前を付けて保存
                SaveAsToolStripMenuItem.Enabled = true;
                // 既定のPDFアプリで開く
                AcrobatOpenToolStripMenuItem.Enabled = true;
                // PDFのプロパティ
                PdfSetToolStripMenuItem.Enabled = true;
                // セキュリティ設定
                SecurityToolStripMenuItem.Enabled = true;
                //SecurityToolStripMenuItem.Enabled = false;
                // しおりのプロパティ
                ShioriProToolStripMenuItem.Enabled = hasNodes; // ノードある時だけ
                // しおりインポート                               
                ImportShioriToolStripMenuItem.Enabled = hasNodes; // ノードある時だけ
                // しおりエクスポート
                ExportShioriToolStripMenuItem.Enabled = hasNodes; // ノードある時だけ



            }

            // 全てのしおりを展開
            AllShioriTenkaiToolStripMenuItem.Enabled = hasNodes;
            // 全てのしおりを縮小
            AllShioriSyukusyouToolStripMenuItem.Enabled = hasNodes;
            // 選択中のしおりを展開
            ShioriTenkaiToolStripMenuItem.Enabled = hasNodes;
            // 選択中のしおりを縮小
            ShioriSyukusyouToolStripMenuItem.Enabled = hasNodes;

            // ページ番号
            NowPageTxt.Enabled = true;
            // 表示方法
            ZoomComboBox.Enabled = true;

        }

        // ==============================
        // 右クリックメニューの「しおり追加」を押したとき
        // ==============================
        private void AddShioriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode newNode = new TreeNode("新しいしおり");

            // 現在のページを取得（これ神機能）
            int currentPage = 1;

            if (pdfViewer1.Document != null)
            {
                currentPage = pdfViewer1.Renderer.Page + 1;
            }

            newNode.Tag = new BookmarkInfo
            {
                // しおり名
                BmTitle = "",
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
                // 兄弟(同レベル)として追加
                TreeNode selected = treeView1.SelectedNode;

                TreeNode? parent = selected.Parent;

                TreeNodeCollection nodes =
                    parent == null ? treeView1.Nodes : parent.Nodes;

                // 下に追加
                int index = selected.Index + 1;

                nodes.Insert(index, newNode);

            }
            else
            {
                // ルートに追加
                treeView1.Nodes.Add(newNode);
            }

            // 追加したノードを選択
            treeView1.SelectedNode = newNode;

            // そのまま名前編集（これ気持ちいい）
            newNode.BeginEdit();

            // ツリービューの右クリックメニュー ON/OFF
            UpdateContextMenuState();

            // 更新フラグ
            isDirty = true;

        }

        // ==============================
        // ツリービューを右クリックしたとき
        // 右クリックしたしおりを選択状態にする
        // ==============================
        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                var node = treeView1.GetNodeAt(e.X, e.Y);
                if (node != null)
                {
                    // AfterSelectでジャンプ防止
                    isRightClickSelecting = true;
                    // 右クリックでも選択状態に
                    treeView1.SelectedNode = node;
                }
            }

        }

        // ==============================
        // 右クリックメニューの「しおり削除」を押したとき
        // ==============================
        private void DelShioriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;

            var node = treeView1.SelectedNode;

            if (MessageBox.Show($"「{node.Text}」を削除しますか？" + Environment.NewLine + "配下のしおりも削除されます。",
                "しおり削除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            treeView1.SelectedNode.Remove();

            // ツリービューの右クリックメニュー ON/OFF
            UpdateContextMenuState();

            // 更新フラグ
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

            // ツリービューの右クリックメニュー ON/OFF
            UpdateContextMenuState();

            // 更新フラグ
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
            e.DrawDefault = true;

            if (dropTargetNode == null) return;
            if (e.Node != dropTargetNode) return;

            var g = e.Graphics;
            var bounds = e.Bounds;

            // 子にする場合
            if (insertAsChild)
            {
                // 背景ハイライト
                using (Brush b = new SolidBrush(DrawingColor.FromArgb(80, DrawingColor.LightBlue)))
                {
                    g.FillRectangle(b, bounds);
                }

                // 枠線
                using (Pen pen = new Pen(DrawingColor.Blue, 2))
                {
                    g.DrawRectangle(pen,
                        bounds.X,
                        bounds.Y,
                        bounds.Width,
                        bounds.Height);
                }

                // ▶マーク（左に表示 青枠 + ▶）
                g.DrawString("▶",
                    this.Font,
                    Brushes.Blue,
                    bounds.X - 20,
                    bounds.Y);
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
        // 現在のページ番号表示(NowPageTxt)でキーを押したとき
        // ==============================
        private void NowPageTxt_KeyDown(object sender, KeyEventArgs e)
        {
            // エンター以外なら戻る
            if (e.KeyCode != Keys.Enter) return;
            // PDFにページがないなら戻る
            if (pdfViewer1.Document == null) return;

            // 数値チェック
            if (!int.TryParse(NowPageTxt.Text, out int page))
                return;

            // ページ範囲チェック
            int maxPage = pdfViewer1.Document.PageCount;
            // ページ範囲外なら戻る
            if (page < 1 || page > maxPage)
                return;

            // ジャンプ
            JumpToPage(page);

            // Enter音防止
            e.SuppressKeyPress = true;
        }


        // ==============================
        // NowPageTxtにフォーカスが当たったら全選択
        // ==============================
        private void NowPageTxt_Enter(object sender, EventArgs e)
        {
            // 全選択
            NowPageTxt.SelectAll();

        }


        // ==============================
        // 表示されているページをしおりにセット
        // ==============================
        private void SetShioriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // しおりが選択されていないなら何もしない
            if (treeView1.SelectedNode == null) return;
            // しおりが選択されていないなら何もしない
            if (pdfViewer1.Document == null) return;

            // 今表示しているページ番号を取得(0始まりなので+1)
            int currentPage = pdfViewer1.Renderer.Page + 1;

            // 選択中のしおりにページ番号を保存
            if (treeView1.SelectedNode.Tag is BookmarkInfo info)
            {
                info.Page = currentPage;
            }

            // 更新フラグ(falseならtrueに)
            //if (!isDirty)
            isDirty = true;

            // 視覚的にわかるように(チェック用)
            //treeView1.SelectedNode.Text = $"{treeView1.SelectedNode.Text.Split('(')[0].Trim()} ({currentPage})";
        }

        // ==============================
        // 終了を押したとき
        // ==============================
        private void XToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // 現在のフォームを閉じる
            this.Close();

        }

        // ==============================
        // フォームが閉じられる直前の処理
        // ==============================
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (!ConfirmSave(() => SavePdfCore(currentPdfPath, currentPdfPath, true)))
            {
                e.Cancel = true;
                return;
            }

        }



        // ==============================
        // サムネイルを表示
        // ==============================
        private void GenerateThumbnails()
        {
            // PDF開いてなかったら何もしない
            if (pdfViewer1.Document == null) return;

            // サムネイルを全部消す(初期化)
            listView1.Items.Clear();
            // メモリ解放
            foreach (Image img in thumbnailImageList.Images)
            {
                img.Dispose();
            }

            thumbnailImageList.Images.Clear();

            // 総ページ数を取得
            int pageCount = pdfViewer1.Document.PageCount;

            // サムネイルの横幅 表示DPIの幅に合わしている(小さいほど軽い)
            int thumbWidth = 128;

            // まず50枚だけ表示
            int firstLoad = Math.Min(pageCount, 50);

            // 初期設定枚数分のサムネイル(画像)を作る(今は50枚)
            for (int i = 0; i < firstLoad; i++)
            {
                // 画像生成
                AddThumbnail(i, thumbWidth);
            }

            // 続きの開始位置(次のページ番号を記録)今回は51ページ目
            thumbnailLoadIndex = firstLoad;

            // 残りがあればタイマー開始
            if (thumbnailLoadIndex < pageCount)
            {
                thumbnailTimer.Start();
            }
        }

        // ==============================
        // サムネイル画像を作るメソッド
        // ==============================
        private void AddThumbnail(int i, int thumbWidth)
        {
            // 念のためチェック
            if (pdfViewer1.Document == null) return;

            // 元ページのサイズ取得
            var size = pdfViewer1.Document.PageSizes[i];

            // 縮小率(今回は横幅128にしたときに縮小率)
            float ratio = thumbWidth / size.Width;
            // 縦も同じ倍率で縮小
            int thumbHeight = (int)(size.Height * ratio);

            // PDFを画像変換
            // 表示DPI128x128(72x72にすると軽くなるが表示が荒くなる)
            // i→ページ番号、thumbWidth/thumbHeight→サイズ、DPI、注釈も描画)
            var img = pdfViewer1.Document.Render(
                i,
                thumbWidth,
                thumbHeight,
                128,
                128,
                PdfRenderFlags.Annotations);

            // 作ったサムネイル(画像)をImageListに登録
            thumbnailImageList.Images.Add(img);

            // サムネイルの下にページ番号を表示
            var item = new ListViewItem($"{i + 1}");

            // 画像とページ番号を紐づけ
            item.ImageIndex = i;
            // 裏データにも
            item.Tag = i;

            // ListViewにサムネイルを表示
            listView1.Items.Add(item);
        }

        // ==============================
        // サムネイルをじわじわ追加
        // タイマーで定期的に呼ばれる
        // ==============================
        private void ThumbnailTimer_Tick(object? sender, EventArgs e)
        {
            // PDF閉じたら停止
            if (pdfViewer1.Document == null)
            {
                thumbnailTimer.Stop();
                return;
            }

            // 必要な情報再取得
            int pageCount = pdfViewer1.Document.PageCount;

            // サムネイルの横幅 表示DPIの幅に合わしている(小さいほど軽い)
            int thumbWidth = 128;

            // 1回で追加する枚数（調整OK）
            int batch = 10;

            // 最大10枚作る(でもページ超えない) スクロールが早いと追いつかない可能性あり
            for (int j = 0; j < batch && thumbnailLoadIndex < pageCount; j++)
            {
                // 1枚追加
                AddThumbnail(thumbnailLoadIndex, thumbWidth);
                // 次のページへ進む
                thumbnailLoadIndex++;
            }

            // 全部終わったら停止
            if (thumbnailLoadIndex >= pageCount)
            {
                thumbnailTimer.Stop();
            }
        }


        // ==============================
        // サムネイルをクリックしたとき
        // ==============================
        private void listView1_Click(object sender, EventArgs e)
        {
            // 何も選択されてなければ戻る
            if (listView1.SelectedItems.Count == 0) return;

            // Tagからページ番号取り出す
            if (listView1.SelectedItems[0].Tag is int pageIndex)
            {
                // 該当ページへジャンプ
                pdfViewer1.Renderer.Page = pageIndex;
            }

        }

        // ==============================
        // PDFプロパティを表示
        // ==============================
        private void PdfSetToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (var f = new Form2(currentSettings))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    currentSettings = f.Settings;

                    isDirty = true;

                }
            }
        }

        // ==============================
        // マウスONで説明 
        // ==============================
        private void menuStrip1_MouseEnter(object? sender, EventArgs e)
        {
            if (sender is ToolStripItem item) toolStripStatusLabel1.Text = item.ToolTipText;
        }

        // ==============================
        // マウス離脱 
        // ==============================
        private void menuStrip1_MouseLeave(object? sender, EventArgs e)
        {
            // 戻す
            toolStripStatusLabel1.Text = toolHintTxt;

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

            if (e.Node?.Tag is BookmarkInfo info)
            {
                info.IsOpen = false;
            }

        }

        // ==============================
        // バージョン情報フォームを呼び出す
        // ==============================
        private void VerToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (var aboutform = new Form3())
            {
                aboutform.ShowDialog(this);
            }


        }

        // ==============================
        // サムネイル上下矢印でページ移動
        // ==============================
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (isSyncingThumbnail) return;

            if (listView1.SelectedIndices.Count == 0) return;

            int index = listView1.SelectedIndices[0];

            // ページは1始まりに
            pdfViewer1.Renderer.Page = index;

            // ページは1始まりなので表示は +1 する
            NowPageTxt.Text = (index + 1).ToString();

        }

        // ==============================
        // 選択ページをサムネイル強調
        // ==============================
        private void SyncThumbnailSelection()
        {
            int pageIndex = pdfViewer1.Renderer.Page;

            if (pageIndex < 0 || pageIndex >= listView1.Items.Count) return;

            isSyncingThumbnail = true;


            listView1.SelectedIndices.Clear();
            listView1.Items[pageIndex].Selected = true;
            listView1.Items[pageIndex].Focused = true;

            // スクロール追従（これ超大事）
            listView1.EnsureVisible(pageIndex);

            isSyncingThumbnail = false;
        }

        // ==============================
        // サムネイルの選択枠を自前で
        // ==============================
        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawBackground();

            // 画像描画
            if (e.Item.ImageIndex >= 0)
            {
                var imageList = listView1.LargeImageList;

                if (imageList != null &&
                    e.Item.ImageIndex >= 0 &&
                    e.Item.ImageIndex < imageList.Images.Count)
                {
                    Image img = imageList.Images[e.Item.ImageIndex];

                    // 中央に描画
                    int x = e.Bounds.X + (e.Bounds.Width - img.Width) / 2;
                    int y = e.Bounds.Y + 5;

                    e.Graphics.DrawImage(img, x, y, img.Width, img.Height);
                }


            }

            // テキスト（ページ番号など）
            TextRenderer.DrawText(
                e.Graphics,
                e.Item.Text,
                listView1.Font,
                new System.Drawing.Rectangle(e.Bounds.X, e.Bounds.Bottom - 20, e.Bounds.Width, 20),
                DrawingColor.Black,
                TextFormatFlags.HorizontalCenter
            );

            // 現在ページ or 選択なら枠描画
            int currentPage = pdfViewer1.Renderer.Page;

            // 今のページを判定
            if (e.Item.Index == currentPage)
            {
                // 赤枠で3
                using (Pen pen = new Pen(DrawingColor.Red, 3))
                {
                    var rect = e.Bounds;
                    // 少し内側に
                    rect.Inflate(-2, -2);
                    e.Graphics.DrawRectangle(pen, rect);
                }
            }
        }

        // ==============================
        // 使い方を開く
        // ==============================
        private void UseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string htmlPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "help",
                    "manual.html"
                    );

                if (!File.Exists(htmlPath))
                {
                    MessageBox.Show(
                        "マニュアルファイルが見当たりません。",
                        "確認：マニュアルがない",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                        );
                    return;
                }

                // 既定ブラウザで開く
                Process.Start(new ProcessStartInfo
                {
                    FileName = htmlPath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "マニュアルファイルが開けません。"
                    + Environment.NewLine
                    + ""
                    + ex.Message,
                    "確認：マニュアルが開けない",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                    );
            }

        }

        // ==============================
        // ZoomComboBoxを選択したとき
        // ==============================
        private void ZoomComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pdfViewer1.Document == null) return;

            // 先にZoomをリセット（ここが核心）
            pdfViewer1.Renderer.Zoom = 1.0f;


            switch (ZoomComboBox.SelectedIndex)
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

        }


        // ==============================
        // セキュリティ設定を開く
        // ==============================
        private void SecurityToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (var f = new Form4(currentSecurity))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    // 設定を受け取る
                    currentSecurity = f.Settings;

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
            DrawingColor currentColor = info.SelectedColor;
            FontStyle currentStyle = info.SelectedStyle;

            // Form6起動
            using (var f = new Form6(currentColor, currentStyle))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    // データ更新（本体）
                    info.SelectedColor = f.SelectedColor;
                    info.SelectedStyle = f.SelectedStyle;

                    // UIにも反映
                    node.ForeColor = f.SelectedColor;
                    node.NodeFont = new Font(treeView1.Font, f.SelectedStyle);

                    treeView1.Refresh();

                    // 更新フラグ
                    isDirty = true;

                }
            }
        }

        // ==============================
        // しおりツールチップ
        // ==============================
        private void treeView1_MouseMove(object sender, MouseEventArgs e)
        {
            var node = treeView1.GetNodeAt(e.Location);

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
        private void ImportShioriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "しおりファイル(CSV)をインポート";
                ofd.Filter = "CSVファイル (*.csv)|*.csv";

                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    var list = LoadCsvBookmarks(ofd.FileName);

                    // 既存しおり削除（上書き）
                    treeView1.Nodes.Clear();

                    BuildTreeFromCsv(list);

                    MessageBox.Show("しおりをインポートしました。", "インポート", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("インポート失敗:\n" + ex.Message, "インポート失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        // ==============================
        private void BuildTreeFromCsv(List<CsvBookmark> list)
        {
            Stack<TreeNode> stack = new Stack<TreeNode>();

            foreach (var item in list)
            {
                TreeNode node = new TreeNode(item.Title);

                // しおりクラスに入れる
                node.Tag = new BookmarkInfo
                {
                    BmTitle = item.Title,
                    Page = item.Page,
                    IsOpen = true,
                    SelectedColor = item.Color,
                    SelectedStyle = item.Style
                };

                // 見た目
                node.ForeColor = item.Color;
                node.NodeFont = new Font(treeView1.Font, item.Style);

                int level = item.Level;

                while (stack.Count > level)
                    stack.Pop();

                if (stack.Count == 0)
                    treeView1.Nodes.Add(node);
                else
                    stack.Peek().Nodes.Add(node);

                stack.Push(node);
            }

            treeView1.ExpandAll();
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
                    MessageBox.Show("エクスポート失敗:\n" + ex.Message, "エクスポート失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                // ツリービューの右クリックメニュー ON/OFF
                UpdateContextMenuState();

                // 更新フラグ
                isDirty = true;

            }

        }

        // ==============================
        // 既定のPDFアプリで開く
        // ==============================
        private void AcrobatOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentPdfPath))
                return;

            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = currentPdfPath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("外部アプリで開けませんでした。\n" + ex.Message, "外部アプリオープンエラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

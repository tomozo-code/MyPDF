using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Navigation;
using iText.Kernel.XMP;
using iText.Kernel.XMP.Impl.XPath;
using iText.Kernel.XMP.Options;
using iText.StyledXmlParser.Jsoup.Nodes;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Image;
using Org.BouncyCastle.Asn1.Cms;
using PdfiumViewer;
using System.Buffers;
using System.Diagnostics;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DrawingColor = System.Drawing.Color;
using SysRectangle = System.Drawing.Rectangle;
using SysImage = System.Drawing.Image;
using IOPath = System.IO.Path;
using ITextDoc = iText.Kernel.Pdf.PdfDocument;
using PdfiTextReader = iText.Kernel.Pdf.PdfReader;
using PdfiumDoc = PdfiumViewer.PdfDocument;
using ITextImage = iText.Layout.Element.Image;



// ==============================
// ライブラリ：iText、PdfiumViewer.Core
// PDFの閲覧・編集・管理をシンプルに行う

// --- 役割 ---
// PDF表示 → PdfiumViewer.Core
// しおり操作 → TreeView
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
        // 更新フラグ(true:更新あり、false:更新なし)
        public bool isDirty = false;
        // 今PDF開いてる？のフラグ(ture:開いてる、false:開いてない)
        private bool isOpening = false;
        // PDFがセキュリティありかチェック用のフラグ(true:なし、false:あり)
        private bool canEdit = true;

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
        private string? password = null;
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


        // アプリ名（タイトルバー表示用）
        private string myName = "ともさんのPDF編集帖";
        // ツールチップ表示用：直前にマウスが乗っていたノード
        private TreeNode? lastNode = null;

        private string PassMessage = "パスワード入力(Form5)のメッセージ用";



        public Form1()
        {
            InitializeComponent();

            // フォームサイズ
            this.Width = 900;
            this.Height = 600;
            this.MinimumSize = new Size(400, 400);

            // フォーム最大化
            //this.WindowState = FormWindowState.Maximized;
            // アプリ名（タイトルバー表示）
            this.Text = myName;

            pdfViewer1.Dock = DockStyle.Fill;

            // pdfViewerのしおり表示を無効
            pdfViewer1.ShowBookmarks = false;
            // pdfViewerのツールバー表示を無効
            pdfViewer1.ShowToolbar = false;

            panel1.Width = 300;
            treeView1.Dock = DockStyle.Fill;

            toolStripStatusLabel1.Text = "ファイル: PDF未選択";
            TotalPagetoolStripLabel.Text = "/ 1 ";
            toolHintTxt = "ファイル: PDF未選択";

            // しおり列選択 代わりに線なし
            treeView1.FullRowSelect = true;
            treeView1.ShowLines = false;

            // エラー表示用
            //Extxt.Visible = true;
            //Extxt.Dock = DockStyle.Bottom;

            // しおりドラッグ中のちラクチ防止
            typeof(TreeView).InvokeMember(
                "DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.SetProperty,
                null,
                treeView1,
                new object[] { true });

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
            // Ctrl+D(PDFのプロパティ)
            PdfPropertyMenu.ShortcutKeys = Keys.Control | Keys.D;
            // Ctrl+T(セキュリティ設定)
            SecurityMenu.ShortcutKeys = Keys.Control | Keys.T;
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
            // 指定して回転
            RotatePagesSetting.ShortcutKeys = Keys.Shift | Keys.Control | Keys.R;
            // 移動
            PageMove.ShortcutKeys = Keys.Shift | Keys.Control | Keys.M;
            // 挿入
            PageInsert.ShortcutKeys = Keys.Shift | Keys.Control | Keys.I;
            // 置換
            ReplacementMenu.ShortcutKeys = Keys.Shift | Keys.Control | Keys.K;
            // 指定して抽出
            PageExtractSetting.ShortcutKeys = Keys.Shift | Keys.Control | Keys.X;
            // 指定して削除
            PageDeleteSetting.ShortcutKeys = Keys.Shift | Keys.Control | Keys.D;


            // 0.1秒ごとにページを監視
            pageTimer.Interval = 100; // 0.1秒ごと
            pageTimer.Tick += PageTimer_Tick;
            pageTimer.Start();

            // サムネイル用 0.05秒ごと
            //thumbnailTimer.Interval = 50; // 0.05秒ごと（調整OK）
            //thumbnailTimer.Tick += ThumbnailTimer_Tick;

            // ZoomModeの選択
            ZoomtoolStripComboBox.Items.AddRange(new object[] { "自動調整", "高さに合わせる", "幅に合わせる" });
            // 自動調整
            ZoomtoolStripComboBox.SelectedIndex = 0;

            // ツールチップ設定(通常コントロール用:Tagに表示させたい内容を書く)
            SetTooltipAll(this);

            // メニューリセット
            MenuReset();

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
                // NewPagetoolStripTextBoxにページ番号を表示
                NewPagetoolStripTextBox.Text = (current + 1).ToString();

            }

        }

        // ==============================
        // 既定のPDFアプリで開く
        // ==============================
        private void AcrobatOpenMenu_Click(object sender, EventArgs e)
        {
            // パスがからならやめる
            if (string.IsNullOrEmpty(originalPath)) return;

            try
            {
                // 既定のアプリで開く(Acrobat Reader とか)
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = originalPath,
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
        // 開くを押したとき    
        // ==============================
        private void OpenMenu_Click(object sender, EventArgs e)
        {

            // 変更がある場合(未保存確認ダイアログ)
            if (!ConfirmDiscard())
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
                            // 作業用ファイルを破棄
                            CleanupWorkingFile();

                            // 開く処理へ
                            OpenPdf(ofd.FileName);
                        }
                        finally
                        {
                            // 例外が出てもフォームを操作可能にする
                            this.Enabled = true;
                            UseWaitCursor = false;
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

            PassMessage = "閲覧パスワードで開いた場合、編集不可(閲覧モード)になります。" + Environment.NewLine +
                "権限パスワードで開いた場合、編集可能(編集モード)になります。" + Environment.NewLine +
                "権限パスワードで開いたPDFファイルは、保存時に制限やパスワードが破棄されますので、" + Environment.NewLine +
                "保護を設定したい場合は" + Environment.NewLine +
                "「ファイル(F) - セキュリティ設定(T)...」から再設定してください。" + Environment.NewLine +
                "権限パスワードのみ設定されているPDFファイルは、パスワードなしで開くことができます。" + Environment.NewLine +
                "その場合は、編集不可(閲覧モード)になります。";

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

                    Debug.WriteLine("--- PoenPDFメソッド ---");
                    Debug.WriteLine("パスワード: " + password + " isEncrypted_c: " + isEncrypted_c + " isOwner: " + isOwner);

                    // パス:null、暗号化、管理者じゃない
                    if (password == null && isEncrypted_c && !isOwner)
                    {
                        // Ownerパスだけ設定されているPDF
                        // → 強制的にパス入力させる
                        iTextDoc.Close();
                        reader.Close();

                        password = ShowPasswordDialog();

                        if (password == null)
                            return;

                        continue; // 再トライ
                    }

                    canEdit = isOwner;

                    // セキュリティ情報保持
                    currentSecurity ??= new SecuritySettings();
                    currentSecurity.IsOwnerOpened = isOwner;

                    if (isOwner)
                    {
                        // Ownerパスで開いた
                        currentSecurity.OwnerPassword = password ?? "";
                        currentSecurity.UserPassword = null;
                        currentPassword = password;
                    }
                    else
                    {
                        // Userパスで開いた
                        currentSecurity.UserPassword = password ?? "";
                        currentSecurity.OwnerPassword = null;
                        currentPassword = null;
                    }

                    // ループ抜ける
                    break;

                }
                catch (iText.Kernel.Exceptions.BadPasswordException)
                {
                    Debug.WriteLine("失敗したので再度パス入力");
                    // 失敗したらパス入力
                    password = ShowPasswordDialog();
                    if (password == null)
                    {
                        // キャンセル
                        return;
                    }
                }
                catch (iText.Kernel.Exceptions.PdfException)
                {
                    // PDF破損
                    MessageBox.Show("PDFファイルが壊れている可能性があります。", "ファイルエラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            treeView1.LabelEdit = canEdit;

            iTextDoc.Close();
            reader.Close();

            // 作業用ファイルを破棄
            CleanupWorkingFile();

            try
            {
                // 元ファイルパス
                originalPath = path;

                // 作業ファイル作成
                // C:\Users\<ユーザー名>\AppData\Local\Temp\ に作業用ファイルを置く
                workingPath = IOPath.Combine(IOPath.GetTempPath(), $"MyPDFwork_{Guid.NewGuid()}.pdf");
                // 元ファイルを作業用ファイルにコピー true:同じ名前は上書き
                File.Copy(path, workingPath, true);

                // Pdfiumで表示
                PdfiumViewer.PdfDocument document;

                if (password == null)
                {
                    // パスワードなし
                    document = PdfiumDoc.Load(workingPath);
                }
                else
                {
                    // パスワードあり
                    document = PdfiumDoc.Load(workingPath, password);
                }

                pdfViewer1.Document = document;

                string? openPassword = password ?? currentPassword;

                // iTextでしおり取得
                ShowBookmarks(workingPath, openPassword);

                // ツリービューの右クリックメニュー ON/OFF
                UpdateContextMenuState();

                // 自動調整
                ZoomtoolStripComboBox.SelectedIndex = 0;

                pdfViewer1.ZoomMode = PdfViewerZoomMode.FitBest;

                // ページ番号「1」を表示
                NewPagetoolStripTextBox.Text = "1";

                // 保存との整合性 作業用ファイルのデータを入れる
                currentSettings = LoadPdfSettings(workingPath, openPassword);

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
#if DEBUG
                // パスワード確認用
                Extxt.Text = currentPassword;
#endif
                // 編集可能か false:不可
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

                // デバッグ用
                Debug.WriteLine("--- OpenPdfメソッド ---");
                Debug.WriteLine("originalPath(元ファイルパス): " + originalPath);
                Debug.WriteLine("workingPath(作業用ファイルパス): " + workingPath);
                Debug.WriteLine("入力されたパスワード: " + password);
            }
            catch (Exception ex)
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
        // 開くときにパスワードがある場合のダイアログ
        // ==============================
        private string? ShowPasswordDialog()
        {
            // 流用しているので、出すメッセージをセットしている
            using (var f = new Form5(PassMessage))
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
        // 作業用ファイルの情報を読むがファイル名とパスは元ファイルにする
        // ==============================
        private PdfSettings LoadPdfSettings(string path, string? password = null)
        {
            var settings = new PdfSettings();

            // ファイル名 元ファイル名を取得
            settings.PdfFileName = IOPath.GetFileName(originalPath);
            // パス
            settings.PdfPath = IOPath.GetDirectoryName(originalPath);

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

                // pdfViewerのページ数取得
                int pageCount = pdfViewer1.Document.PageCount;
                // 総ページ数をデータへ
                settings.TotalPage = pageCount;

                // ステータスバーにファイル名(元ファイル)と総ページ数
                UpdateStatus(originalPath, pageCount);


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

                // レイアウト
                var layout = catalogObj.GetAsName(PdfName.PageLayout);
                settings.PageLayout = layout?.GetValue();

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
#if DEBUG
                Extxt.Text = ex.Message;
                MessageBox.Show($"エラー:\n{ex.Message}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
#else

                MessageBox.Show("しおりの取得に失敗しました。", "しおり取得失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
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
                    ImageIndex = 0,
                    SelectedImageIndex = 1,

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
                    "しおり削除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No)
                    return;

                node.Remove();

                // ツリービューの右クリックメニュー ON/OFF
                UpdateContextMenuState();

                // 更新フラグ(falseならtrueに)
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
                //isRightClickSelecting = false;
                return; // ←ジャンプさせない
            }

            if (isOpening) return;

            // クリックしたしおりのページを取得
            if (e.Node?.Tag is BookmarkInfo info && info.Page > 0)
            {
                // 取得したページへジャンプ
                JumpToPage(info.Page);
            }

        }

        // ==============================
        // ツリービューのしおりを押したとき(選択中のしおりを押したとき)
        // ==============================
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (isRightClickSelecting)
            {
                // 右クリック時はジャンプしない(ページ移動させない)
                //isRightClickSelecting = false;
                return; // ←ジャンプさせない
            }

            if (isOpening) return;

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
            //toolStripStatusLabel1.Text = $"パス: {path} | 総ページ数: {pageCount}";
            toolStripStatusLabel1.Text = $"パス: {path}";
            TotalPagetoolStripLabel.Text = $"/ {pageCount} ";

            //toolHintTxt = $"パス: {path} | 総ページ数: {pageCount}";
            toolHintTxt = $"パス: {path}";

        }


        // ==============================
        // 上書き保存を押したとき
        // ==============================
        private void SaveMenu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(originalPath)) return;

            SavePdf(originalPath);

        }

        // ==============================
        // 名前を付けて保存を押したとき
        // ==============================
        private void SaveAsMenu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(originalPath)) return;

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                string baseName = IOPath.GetFileNameWithoutExtension(originalPath);
                sfd.Title = "名前を付けてPDFファイルを保存";
                sfd.Filter = "PDFファイル (*.pdf)|*.pdf";
                sfd.FileName = baseName + "_new.pdf";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    SavePdf(sfd.FileName);
                }
            }
        }

        // ==============================
        // 保存処理(上書き保存、名前を付けて保存 共通)
        // ==============================
        private void SavePdf(string savePath)
        {

            if (string.IsNullOrEmpty(originalPath)) return;

            // 名前を付けて保存だったら保存できるかチェックしない
            if (savePath == originalPath)
            {
                // 保存できるかチェック(他のアプリで開いてる？)
                try
                {
                    using (var test = new FileStream(
                        savePath,
                        FileMode.Open,
                        FileAccess.ReadWrite,
                        FileShare.None))
                    {
                    }
                }
                catch (IOException)
                {
                    MessageBox.Show(
                        "PDFファイルが他のアプリで開かれています。" + Environment.NewLine +
                        "閉じてから保存してください。",
                        "保存失敗",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }
            }

            // 現在ページ退避し保存後に表示しているページに戻す
            int currentPage = 0;
            if (pdfViewer1.Document != null)
            {
                currentPage = pdfViewer1.Renderer.Page;
            }

            // パスが設定されたフラグ(true:パスあり、false:パスなし)
            bool MsgFlag = false;

            try
            {
                // 一時作業用ファイルを作成
                string tempPath = workingPath + ".tmp";

                // Viewer解放（ロック防止）
                if (pdfViewer1.Document != null)
                {
                    pdfViewer1.Document.Dispose();
                    pdfViewer1.Document = null;
                }

                // UI更新してロック解除を待つ
                //Application.DoEvents();

                // PDFを書き出すためのオブジェクトを用意
                PdfWriter writer;

                // 一時作業用ファイルに書き込む
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

                // パスワードがあるかどうかで読み込み設定を変える(nullまたは空白:trure、パスあり:false)
                string? readPassword = null;

                // 最優先：Ownerパス
                if (currentSecurity?.Check_Owner == true && !string.IsNullOrEmpty(currentSecurity.OwnerPassword))
                {
                    readPassword = currentSecurity.OwnerPassword;
                }
                // fallback：Userパス
                else if (currentSecurity?.Check_User == true && !string.IsNullOrEmpty(currentSecurity.UserPassword))
                {
                    readPassword = currentSecurity.UserPassword;
                }

                // 今開いているPDFのパスで開く
                ReaderProperties props = string.IsNullOrEmpty(currentPassword)
                    ? new ReaderProperties()
                    : new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(currentPassword));

                using (var fs = new FileStream(workingPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new PdfiTextReader(fs, props))
                using (var pdf = new ITextDoc(reader, writer))
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

                    if (currentSettings == null)
                    {
                        MessageBox.Show("設定が読み込まれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
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
                    //string keywordsJoined = string.Join("; ", keywordList);
                    //info.SetKeywords(keywordsJoined);
                    //info.SetMoreInfo("Keywords", keywordsJoined);

                    //info.SetProducer(producer);
                    info.SetCreator(appName);

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

                    if (keywordList.Count > 0)
                    {
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
                    }

                    // PDF情報
                    xmp.SetProperty(XMPConst.NS_PDF, "Producer", producer);
                    // CreatorTool(クリエーターツール)
                    xmp.SetProperty(XMPConst.NS_XMP, "CreatorTool", producer);
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

                // 一時作業用ファイルを作業用ファイルにコピーして一時ファイルを消す
                File.Copy(tempPath, workingPath, true);
                File.Delete(tempPath);

                // 保存できなかっったとき(念のため)
                try
                {

                    // 作業用ファイルを元ファイルにコピー true:同じ名前は上書き
                    File.Copy(workingPath, savePath, true);
                }
                catch (IOException)
                {
                    MessageBox.Show(
                        "PDFファイルが他のアプリで開かれています。" + Environment.NewLine +
                        "閉じてから保存してください。",
                        "保存失敗",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }

                originalPath = savePath;

                string? openPassword = null;
                // 権限パスが設定されている場合はパスを取得
                if (!string.IsNullOrEmpty(currentSecurity?.OwnerPassword))
                    openPassword = currentSecurity.OwnerPassword;
                else if (!string.IsNullOrEmpty(currentSecurity?.UserPassword))
                    openPassword = currentSecurity.UserPassword;

                // pdfiumViewer読み込み(パスがある場合はパスで読み込み)
                var doc = string.IsNullOrEmpty(openPassword)
                    ? PdfiumDoc.Load(workingPath)
                    : PdfiumDoc.Load(workingPath, openPassword);
                pdfViewer1.Document = doc;

                // パスワードを更新
                currentPassword = openPassword;

                Extxt.Text = currentPassword;

                // 保存との整合性 作業用ファイルのデータを入れる
                currentSettings = LoadPdfSettings(workingPath, openPassword);

                // ステータスバーにファイル名(元ファイル)と総ページ数
                UpdateStatus(originalPath, pdfViewer1.Document.PageCount);

                // 元のページに戻す
                // 元ページへ戻す
                if (currentPage >= 0 &&
                    currentPage < pdfViewer1.Document.PageCount)
                {
                    pdfViewer1.Renderer.Page = currentPage;
                }

                string fileName = IOPath.GetFileName(originalPath);

                this.Text = myName + " - [ " + fileName + " ]";

                // パスワード設定のメッセージ用
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

                }

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


                // 保存したので未変更
                isDirty = false;
                //MessageBox.Show("保存完了", "保存確認", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
#if DEBUG
                Extxt.Text = ex.ToString();
                MessageBox.Show("保存エラー:\n" + ex.ToString());
#else
                MessageBox.Show("保存に失敗しました。", "保存失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                System.Diagnostics.Debug.WriteLine(ex.ToString());

#endif
            }

        }


        // ==============================
        // 作業用ファイル破棄（共通）
        // ==============================
        private void CleanupWorkingFile()
        {
            try
            {
                // Viewer解放（ロック防止）
                if (pdfViewer1.Document != null)
                {
                    pdfViewer1.Document.Dispose();
                    pdfViewer1.Document = null;
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
            catch (Exception ex)
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
        private bool ConfirmDiscard()
        {
            if (!isDirty) return true;

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
                SavePdf(originalPath);

                // 作業用ファイルを破棄
                //CleanupWorkingFile();

            }
            else if (result == DialogResult.No)
            {
                // いいえ

                // 保存しない
                isDirty = false;

                // 作業用ファイルを破棄
                CleanupWorkingFile();
            }

            return true;
        }



        // ==============================
        // ツリービューをPDFへ の処理
        // ==============================
        private void AddOutlineFromNode(ITextDoc pdf, PdfOutline parent, TreeNode node)
        {

            // 安全に取り出す
            if (node.Tag is not BookmarkInfo info) return;

            int page = info.Page;
            int max = pdf.GetNumberOfPages();

            // しおり範囲チェック
            if (page < 1 || page > max)
            {
                Debug.WriteLine($"しおりスキップ: {node.Text} page={page} max={max}");
                return;
            }

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
        // しおりページ補正（削除対応）
        // ==============================
        private void AdjustBookmarksAfterDelete(TreeNodeCollection nodes, int start, int end)
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                var node = nodes[i];

                if (node.Tag is BookmarkInfo info)
                {
                    // 削除範囲内 → ノード削除
                    if (info.Page >= start && info.Page <= end)
                    {
                        nodes.RemoveAt(i);
                        continue;
                    }

                    // 削除範囲より後ろ → 前に詰める
                    if (info.Page > end)
                    {
                        info.Page -= (end - start + 1);
                    }
                }

                // 子ノードも再帰処理
                if (node.Nodes.Count > 0)
                {
                    AdjustBookmarksAfterDelete(node.Nodes, start, end);
                }
            }
        }

        // ==============================
        // コンテキストメニューの表示をリセット
        // ==============================
        private void MenuReset()
        {
            // しおり作成
            AddShioriToolStripMenuItem.Enabled = false;
            // しおり削除
            DelShioriToolStripMenuItem.Enabled = false;
            // 全てのしおり削除
            AllDelToolStripMenuItem.Enabled = false;
            // 現在のページ番号をしおりに設定
            SetShioriToolStripMenuItem.Enabled = false;

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
            // しおりインポート
            ImportShioriToolStripMenuItem.Enabled = false;
            // しおりエクスポート
            ExportShioriToolStripMenuItem.Enabled = false;

            // 移動
            PageMove.Enabled = false;
            // 挿入
            PageInsert.Enabled = false;
            // 置換
            ReplacementMenu.Enabled = false;
            // 抽出
            PageExtract.Enabled = false;
            PageExtractSetting.Enabled = false;
            // ページ削除
            PageDelete.Enabled = false;
            PageDeleteSetting.Enabled = false;
            // 回転
            LeftRotate90.Enabled = false;
            RightRotate90.Enabled = false;
            Rotate180.Enabled = false;
            RotatePagesSetting.Enabled = false;

            // 全てのしおりを展開
            AllShioriTenkaiToolStripMenuItem.Enabled = false;
            // 全てのしおりを縮小
            AllShioriSyukusyouToolStripMenuItem.Enabled = false;
            // 選択中のしおりを展開
            ShioriTenkaiToolStripMenuItem.Enabled = false;
            // 選択中のしおりを縮小
            ShioriSyukusyouToolStripMenuItem.Enabled = false;

            // ページ番号
            NewPagetoolStripTextBox.Enabled = false;
            // 表示方法
            ZoomtoolStripComboBox.Enabled = false;
            // 閉じる
            CloseMenu.Enabled = false;

        }



        // ==============================
        // ツリービューの右クリックメニュー 「しおり追加」「しおり削除」のON/OFF
        // ==============================
        private void UpdateContextMenuState()
        {
            // ツリービューノードが0以上:true
            bool hasNodes = treeView1.Nodes.Count > 0;

            int pageCount = pdfViewer1.Document.PageCount;

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
                // しおりインポート
                ImportShioriToolStripMenuItem.Enabled = false;
                // しおりエクスポート
                ExportShioriToolStripMenuItem.Enabled = false;

                // 移動
                PageMove.Enabled = false;
                // 挿入
                PageInsert.Enabled = false;
                // 置換
                ReplacementMenu.Enabled = false;
                // 抽出
                PageExtract.Enabled = false;
                PageExtractSetting.Enabled = false;
                // ページ削除
                PageDelete.Enabled = false;
                PageDeleteSetting.Enabled = false;
                // 回転
                LeftRotate90.Enabled = false;
                RightRotate90.Enabled = false;
                Rotate180.Enabled = false;
                RotatePagesSetting.Enabled = false;

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
                // しおりインポート                               
                ImportShioriToolStripMenuItem.Enabled = true;     // 常にOK
                // しおりエクスポート
                ExportShioriToolStripMenuItem.Enabled = hasNodes; // ノードある時だけ

                // 移動
                PageMove.Enabled = true;
                // 挿入
                PageInsert.Enabled = true;
                // 置換
                ReplacementMenu.Enabled = true;
                // 抽出
                PageExtract.Enabled = true;
                PageExtractSetting.Enabled = true;
                // ページ削除
                if (pageCount <= 1)
                {
                    PageDelete.Enabled = false;
                    PageDeleteSetting.Enabled = false;
                }
                else
                {
                    PageDelete.Enabled = true;
                    PageDeleteSetting.Enabled = true;

                }
                // 回転
                LeftRotate90.Enabled = true;
                RightRotate90.Enabled = true;
                Rotate180.Enabled = true;
                RotatePagesSetting.Enabled = true;

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
            NewPagetoolStripTextBox.Enabled = true;
            // 表示方法
            ZoomtoolStripComboBox.Enabled = true;
            // 閉じる
            CloseMenu.Enabled = true;

        }

        // ==============================
        // 右クリックメニューの「しおり追加」を押したとき
        // ==============================
        private void AddShioriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode? newNode = null;

            treeView1.BeginUpdate();

            try
            {
                newNode = new TreeNode("新しいしおり")
                {
                    ImageIndex = 0,
                    SelectedImageIndex = 1
                };

                // 現在の表示ページを取得
                int currentPage = 1;

                if (pdfViewer1.Document != null)
                {
                    currentPage = pdfViewer1.Renderer.Page + 1;
                }

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
                newNode.EnsureVisible();
                treeView1.Focus();

                // 更新フラグ
                isDirty = true;

            }
            finally
            {
                treeView1.EndUpdate();

                // 作成すると何故かノードの一番上で編集状態になるので苦肉の策(ちょっと遅らせる)
                BeginInvoke(new Action(() =>
                {
                    BeginInvoke(new Action(() =>
                    {
                        treeView1.Refresh();
                        // そのまま名前編集
                        newNode?.BeginEdit();
                    }));

                    // ツリービューの右クリックメニュー ON/OFF
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
            if (e.Button == MouseButtons.Right)
            {
                isRightClickSelecting = true;

                var node = treeView1.GetNodeAt(e.X, e.Y);

                if (node != null)
                {
                    // 右クリックでも選択状態に
                    treeView1.SelectedNode = node;
                }
            }

            /*

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

            */

        }

        // ==============================
        // ツリービューを右クリックしたとき
        // マウスアップしたときにfalseにする
        // ==============================
        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                isRightClickSelecting = false;
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
                "しおり削除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.No)
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
            //e.DrawDefault = true;

            // --- 自前で描画 ---------------
            e.DrawDefault = false;
            Graphics g = e.Graphics;

            // null対策
            if (e.Node == null) return;

            // 選択状態
            bool isSelected = (treeView1.SelectedNode == e.Node);

            // 背景色
            DrawingColor backColor;

            if (isSelected)
            {
                backColor = SystemColors.Highlight;
            }
            else if (e.Node == hoverNode)
            {
                // ホバー色
                //backColor = DrawingColor.FromArgb(230, 240, 250);
                backColor = DrawingColor.FromArgb(200, 200, 200);
            }
            else
            {
                backColor = treeView1.BackColor;
            }

            // 背景塗り
            using (Brush backBrush = new SolidBrush(backColor))
            {
                //g.FillRectangle(backBrush, e.Bounds);
                g.FillRectangle(
                    backBrush,
                    0,
                    e.Bounds.Top,
                    treeView1.Width,
                    treeView1.ItemHeight
                    );
            }

            // + - ボタン描画
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
            DrawingColor foreColor =
                (e.State & TreeNodeStates.Selected) != 0
                ? SystemColors.HighlightText
                : e.Node.ForeColor;

            // ノードフォント
            Font nodeFont =
                e.Node.NodeFont ?? treeView1.Font;

            // 上下中央用Rect
            SysRectangle textRect = new SysRectangle(
                e.Bounds.X + 20,
                e.Bounds.Y,
                e.Bounds.Width,
                treeView1.ItemHeight
            );

            // 文字描画
            TextRenderer.DrawText(
                g,
                e.Node?.Text,
                nodeFont,
                textRect,
                foreColor,
                TextFormatFlags.VerticalCenter |
                TextFormatFlags.Left
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
        private void NowPageTxt_KeyDown(object sender, KeyEventArgs e)
        {
            // エンター以外なら戻る
            if (e.KeyCode != Keys.Enter) return;
            // PDFにページがないなら戻る
            if (pdfViewer1.Document == null) return;

            // 数値チェック
            if (!int.TryParse(NewPagetoolStripTextBox.Text, out int page))
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

        private void NewPagetoolStripTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // エンター以外なら戻る
            if (e.KeyCode != Keys.Enter) return;
            // PDFにページがないなら戻る
            if (pdfViewer1.Document == null) return;

            // 数値チェック
            if (!int.TryParse(NewPagetoolStripTextBox.Text, out int page))
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
        // NewPagetoolStripTextBoxにフォーカスが当たったら全選択
        // ==============================
        private void NowPageTxt_Enter(object sender, EventArgs e)
        {
            // 全選択
            NewPagetoolStripTextBox.SelectAll();

        }

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
            if (pdfViewer1.Document == null) return;

            // 今表示しているページ番号を取得(0始まりなので+1)
            int currentPage = pdfViewer1.Renderer.Page + 1;

            // 選択中のしおりにページ番号を保存
            if (treeView1.SelectedNode.Tag is BookmarkInfo info)
            {
                info.Page = currentPage;
            }

            // 更新フラグ(falseならtrueに)
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
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            // 未保存チェック
            if (!ConfirmDiscard())
            {
                e.Cancel = true;
                return;
            }

            // 作業用ファイルを破棄
            CleanupWorkingFile();


        }

        // ==============================
        // PDFプロパティを表示
        // ==============================
        private void PdfPropertyMenu_Click(object sender, EventArgs e)
        {
            if (currentSettings == null)
            {
                MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
        // マウスONで説明(ToolStrip系) 
        // ==============================
        private void menuStrip1_MouseEnter(object? sender, EventArgs e)
        {
            if (sender is ToolStripItem item) toolStripStatusLabel1.Text = item.ToolTipText;
        }

        // ==============================
        // マウス離脱(ToolStrip系)
        // ==============================
        private void menuStrip1_MouseLeave(object? sender, EventArgs e)
        {
            // 戻す
            toolStripStatusLabel1.Text = toolHintTxt;

        }

        // ==============================
        // マウスONで説明(通常コントロール) Tagに書く 
        // ==============================
        private void Control_MouseEnter(object? sender, EventArgs e)
        {
            if (sender is Control ctrl)
            {
                toolStripStatusLabel1.Text = ctrl.Tag?.ToString() ?? "";
            }
        }

        // ==============================
        // マウス離脱(通常コントロール)
        // ==============================
        private void Control_MouseLeave(object? sender, EventArgs e)
        {
            // 戻す
            toolStripStatusLabel1.Text = toolHintTxt;
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
        // ZoomtoolStripComboBoxを選択したとき
        // ==============================
        private void ZoomtoolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pdfViewer1.Document == null) return;

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

            if (currentSettings == null)
            {
                MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    // 更新フラグ
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

                    // 更新
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
                // ツリービューの右クリックメニュー ON/OFF
                UpdateContextMenuState();

                // 更新フラグ
                isDirty = true;

            }

        }

        // ==============================
        // ページ回転共通処理(単一、複数対応)
        // ==============================
        private void RotatePages(int startPage, int endPage, int addRotation)
        {
            if (pdfViewer1.Document == null) return;

            try
            {
                int currentPage = pdfViewer1.Renderer.Page;

                // Viewer解放
                pdfViewer1.Document.Dispose();
                pdfViewer1.Document = null;

                string tempPath = workingPath + ".tmp";

                ReaderProperties props = string.IsNullOrEmpty(currentPassword)
                    ? new ReaderProperties()
                    : new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(currentPassword));

                using (var reader = new PdfReader(workingPath, props))
                using (var writer = new PdfWriter(tempPath))
                using (var pdf = new ITextDoc(reader, writer))
                {
                    int total = pdf.GetNumberOfPages();

                    // 範囲補正（安全対策）
                    startPage = Math.Max(1, Math.Min(startPage, total));
                    endPage = Math.Max(1, Math.Min(endPage, total));

                    for (int i = startPage; i <= endPage; i++)
                    {
                        var page = pdf.GetPage(i);
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
                pdfViewer1.Document = doc;

                if (currentPage < doc.PageCount)
                    pdfViewer1.Renderer.Page = currentPage;

                // 変更フラグ
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
        // 表示ページを左へ90°回転
        // ==============================
        private void LeftRotate90_Click(object sender, EventArgs e)
        {
            // 左なので-90°だがプラスで設定 → 270°
            int page = pdfViewer1.Renderer.Page + 1;
            RotatePages(page, page, 270);
        }

        // ==============================
        // 表示ページを右へ90°回転
        // ==============================
        private void RightRotate90_Click(object sender, EventArgs e)
        {
            int page = pdfViewer1.Renderer.Page + 1;
            RotatePages(page, page, 90);
        }

        // ==============================
        // 表示ページを180°回転
        // ==============================
        private void Rotate180_Click(object sender, EventArgs e)
        {
            int page = pdfViewer1.Renderer.Page + 1;
            RotatePages(page, page, 180);
        }

        // ==============================
        // ページを指定して回転フォームを呼ぶ
        // ==============================
        private void RotatePagesSetting_Click(object sender, EventArgs e)
        {
            if (currentSettings == null)
            {
                MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int page = pdfViewer1.Renderer.Page + 1;

            // Form7起動
            using (var f = new Form7(page, currentSettings.TotalPage))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {

                    RotatePages(f.StartPage, f.EndPage, f.RotationAngle);

                }
            }

        }

        // ==============================
        // ページ削除共通処理(単一、複数対応)
        // ==============================
        private void DeletePages(int startPage, int endPage)
        {
            if (pdfViewer1.Document == null) return;

            try
            {
                int currentPage = pdfViewer1.Renderer.Page;

                // Viewer解放
                pdfViewer1.Document.Dispose();
                pdfViewer1.Document = null;

                string tempPath = workingPath + ".tmp";

                ReaderProperties props = string.IsNullOrEmpty(currentPassword)
                    ? new ReaderProperties()
                    : new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(currentPassword));

                using (var reader = new PdfReader(workingPath, props))
                using (var writer = new PdfWriter(tempPath))
                using (var pdf = new ITextDoc(reader, writer))
                {
                    int total = pdf.GetNumberOfPages();

                    // 範囲補正（安全対策）
                    startPage = Math.Max(1, Math.Min(startPage, total));
                    endPage = Math.Max(1, Math.Min(endPage, total));

                    // start > end 対策
                    if (startPage > endPage)
                    {
                        var tmp = startPage;
                        startPage = endPage;
                        endPage = tmp;
                    }

                    // 後ろから削除
                    for (int i = endPage; i >= startPage; i--)
                    {
                        pdf.RemovePage(i);
                    }
                }

                // しおり補正
                AdjustBookmarksAfterDelete(treeView1.Nodes, startPage, endPage);

                // 一時ファイルを作業用ファイルにコピー
                File.Copy(tempPath, workingPath, true);
                File.Delete(tempPath);

                // 再表示
                var doc = string.IsNullOrEmpty(currentPassword)
                    ? PdfiumDoc.Load(workingPath)
                    : PdfiumDoc.Load(workingPath, currentPassword);
                pdfViewer1.Document = doc;

                // ページ位置補正
                if (doc.PageCount > 0)
                {
                    pdfViewer1.Renderer.Page = Math.Min(currentPage, doc.PageCount - 1);
                }

                // ツリービューの右クリックメニュー ON/OFF
                UpdateContextMenuState();

                // 保存との整合性 作業用ファイルのデータを入れる
                currentSettings = LoadPdfSettings(workingPath, currentPassword);

                // 変更フラグ
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
        // 表示ページ削除
        // ==============================
        private void PageDelete_Click(object sender, EventArgs e)
        {
            if (pdfViewer1.Document == null) return;

            int page = pdfViewer1.Renderer.Page + 1; // 0始まり→1始まり
            DeletePages(page, page);
        }

        // ==============================
        // ページを指定して削除
        // ==============================
        private void PageDeleteSetting_Click(object sender, EventArgs e)
        {

            if (currentSettings == null)
            {
                MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int page = pdfViewer1.Renderer.Page + 1;

            // Form8起動
            using (var f = new Form8(page, currentSettings.TotalPage))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {

                    DeletePages(f.StartPage, f.EndPage);

                }
            }

        }

        // ==============================
        // 閉じるを押したとき
        // ==============================
        private void CloseMenu_Click(object sender, EventArgs e)
        {
            if (!ConfirmDiscard())
                return;

            // 閉じる処理を呼ぶ
            CloseCurrentPdf();

        }

        // ==============================
        // 閉じる処理
        // ==============================
        private void CloseCurrentPdf()
        {
            if (!ConfirmDiscard())
                return;

            // Viewer完全リセット
            ResetPdfViewer();

            // 作業ファイル削除
            CleanupWorkingFile();

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

            toolStripStatusLabel1.Text = "ファイル: PDF未選択";
            NewPagetoolStripTextBox.Text = "1";
            TotalPagetoolStripLabel.Text = "/ 1 ";
            toolHintTxt = "ファイル: PDF未選択";

            isDirty = false;
        }

        // ==============================
        // Viewer完全リセット処理
        // ==============================
        private void ResetPdfViewer()
        {
            // Panel2から外す
            panel2.Controls.Remove(pdfViewer1);

            // 念のため破棄
            try
            {
                pdfViewer1.Document?.Dispose();
                pdfViewer1.Dispose();
            }
            catch { }

            // 表示が消えないので新しく作り直す
            pdfViewer1 = new PdfViewer();

            // 設定を再適用
            pdfViewer1.Dock = DockStyle.Fill;
            // pdfViewerのしおり表示を無効
            pdfViewer1.ShowBookmarks = false;
            // pdfViewerのツールバー表示を無効
            pdfViewer1.ShowToolbar = false;
            pdfViewer1.BorderStyle = BorderStyle.FixedSingle;
            pdfViewer1.ContextMenuStrip = contextMenuStrip2;

            // 再追加
            panel2.Controls.Add(pdfViewer1);
            pdfViewer1.BringToFront();

            // メニューリセット
            MenuReset();

        }

        // ==============================
        // ページ抽出共通処理(単一、複数対応)
        // ==============================
        private void ExtractPages(int startPage, int endPage)
        {
            if (pdfViewer1.Document == null) return;

            try
            {
                // 保存ダイアログ
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    string baseName = IOPath.GetFileNameWithoutExtension(originalPath);
                    sfd.Title = "抽出PDFを保存";
                    sfd.Filter = "PDFファイル (*.pdf)|*.pdf";
                    sfd.FileName = baseName + "_Extract.pdf";

                    if (sfd.ShowDialog() != DialogResult.OK)
                        return;

                    string savePath = sfd.FileName;

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

                        // 範囲補正
                        startPage = Math.Max(1, Math.Min(startPage, total));
                        endPage = Math.Max(1, Math.Min(endPage, total));

                        if (startPage > endPage)
                            return;

                        // ページコピー
                        srcPdf.CopyPagesTo(startPage, endPage, destPdf);

                        // しおり抽出
                        AdjustBookmarksForExtract(startPage, endPage);

                        var destOutlines = destPdf.GetOutlines(true);

                        foreach (TreeNode node in treeView1.Nodes)
                        {
                            AddOutlineFromNode(destPdf, destOutlines, node);
                        }

                    }

                    MessageBox.Show("抽出完了", "抽出確認", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
        private void AdjustBookmarksForExtract(int start, int end)
        {
            for (int i = treeView1.Nodes.Count - 1; i >= 0; i--)
            {
                AdjustNodeForExtract(treeView1.Nodes[i], start, end);
            }
        }

        // ==============================
        // しおり抽出処理2
        // ==============================
        private void AdjustNodeForExtract(TreeNode node, int start, int end)
        {
            if (node.Tag is BookmarkInfo info)
            {
                // 範囲外は削除
                if (info.Page < start || info.Page > end)
                {
                    node.Remove();
                    return;
                }

                // ページ再マッピング
                info.Page = info.Page - start + 1;
            }

            for (int i = node.Nodes.Count - 1; i >= 0; i--)
            {
                AdjustNodeForExtract(node.Nodes[i], start, end);
            }
        }


        // ==============================
        // 抽出を押したとき
        // ==============================
        private void PageExtract_Click(object sender, EventArgs e)
        {

            if (pdfViewer1.Document == null) return;

            int page = pdfViewer1.Renderer.Page + 1; // 0始まり→1始まり
            ExtractPages(page, page);


        }

        // ==============================
        // ページを指定して抽出を押したとき
        // ==============================
        private void PageExtractSetting_Click(object sender, EventArgs e)
        {
            if (currentSettings == null)
            {
                MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int page = pdfViewer1.Renderer.Page + 1;

            // Form9起動
            using (var f = new Form9(page, currentSettings.TotalPage))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {

                    ExtractPages(f.StartPage, f.EndPage);

                }
            }

        }

        // ==============================
        // 挿入を押したとき(ファイルから)
        // ==============================
        private void PageInsert_Click(object sender, EventArgs e)
        {
            if (pdfViewer1.Document == null) return;

            if (currentSettings == null)
            {
                MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    // 表示しているページを取得
                    int nowPage = pdfViewer1.Renderer.Page + 1;

                    ofd.Title = "挿入するPDFを選択";
                    ofd.Filter = "PDFファイル (*.pdf)|*.pdf";

                    if (ofd.ShowDialog() != DialogResult.OK)
                        return;

                    string insertPath = ofd.FileName;

                    // Form10起動
                    using (var f = new Form10(insertPath, nowPage, currentSettings.TotalPage))
                    {
                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            InsertPdf(insertPath, f.TargetPage, f.InsertBefore);
                        }
                    }

                }
            }
            catch (Exception ex)
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
        private void InsertPdf(string insertPath, int targetPage, bool insertBefore)
        {

            string? insertPassword = null;

            ReaderProperties insertProps;

            PdfReader? reader = null;
            ITextDoc? iTextDoc = null;

            bool canInsert = false;

            PassMessage = "挿入するPDFファイルは保護されています。" + Environment.NewLine +
                "権限パスワードの場合は挿入可能ですが、閲覧パスワードの場合は挿入できません。";

            while (true)
            {
                try
                {
                    if (insertPassword == null)
                    {
                        // まずはパス無しで開く
                        reader = new PdfReader(insertPath);
                    }
                    else
                    {
                        // パス入力済みなら
                        var props = new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(insertPassword));
                        // パス付きで開く
                        reader = new PdfReader(insertPath, props);
                    }

                    // PDFを実際に開く
                    iTextDoc = new ITextDoc(reader);
                    // 管理者(制限パス)で開いてる true:制限パス false:以外)
                    bool isOwner = reader.IsOpenedWithFullPermission();
                    // 暗号化されてる？
                    bool isEncrypted_c = reader.IsEncrypted();

                    if (insertPassword == null && isEncrypted_c && !isOwner)
                    {
                        // Ownerパスだけ設定されているPDF
                        // → 強制的にパス入力させる
                        iTextDoc.Close();
                        reader.Close();

                        insertPassword = ShowPasswordDialog();

                        if (insertPassword == null)
                            return;

                        continue; // 再トライ
                    }

                    if (isOwner)
                    {
                        canInsert = reader.IsOpenedWithFullPermission();
                    }

                    // ループ抜ける
                    break;

                }
                catch (iText.Kernel.Exceptions.BadPasswordException)
                {
                    insertPassword = ShowPasswordDialog();
                    if (insertPassword == null) return;
                }
                catch (Exception ex)
                {
#if DEBUG
                    Extxt.Text = ex.ToString();
                    MessageBox.Show("PDF確認エラー:\n" + ex.Message);
#else
                    MessageBox.Show("挿入するファイルが開けませんでした。", "挿入するファイルオープン失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
                    return;
                }
            }


            iTextDoc.Close();
            reader.Close();


            // チェック(閲覧パスなら挿入しない)
            if (!canInsert)
            {
                MessageBox.Show(
                    "入力したパスワードは閲覧パスワードです。" + Environment.NewLine +
                    "ページ挿入するには、権限パスワードが必要です。",
                    "挿入不可",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            string tempPath = workingPath + ".tmp";

            insertProps = string.IsNullOrEmpty(insertPassword)
                ? new ReaderProperties()
                : new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(insertPassword));

            try
            {
                // Viewer解放
                if (pdfViewer1.Document != null)
                {
                    pdfViewer1.Document.Dispose();
                    pdfViewer1.Document = null;
                }

                //Application.DoEvents();

                int insertPage;
                int insertCount;

                ReaderProperties props = string.IsNullOrEmpty(currentPassword)
                    ? new ReaderProperties()
                    : new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(currentPassword));

                using (var mainReader = new PdfReader(workingPath, props))
                using (var insertReader = new PdfReader(insertPath, insertProps))
                using (var writer = new PdfWriter(tempPath))
                using (var mainPdf = new ITextDoc(mainReader, writer))
                using (var insertPdf = new ITextDoc(insertReader))
                {
                    int total = mainPdf.GetNumberOfPages();

                    // 挿入位置決定
                    insertPage = insertBefore
                        ? targetPage
                        : targetPage + 1;

                    // 補正
                    insertPage = Math.Max(1, Math.Min(insertPage, total + 1));

                    insertCount = insertPdf.GetNumberOfPages();

                    insertPdf.CopyPagesTo(
                        1,
                        insertCount,
                        mainPdf,
                        insertPage
                    );
                }

                // しおり補正
                FixBookmarksForInsert(insertPage, insertCount);

                // 挿入PDFのしおり取得
                //using (var insertReader2 = new PdfReader(insertPath))
                using (var insertReader2 = new PdfReader(insertPath, insertProps))
                using (var insertPdfDoc = new ITextDoc(insertReader2))
                {
                    ImportBookmarksFromPdf(insertPdfDoc, insertPage, treeView1, false);

                    Debug.WriteLine("--- 挿入 -----------------");
                    Debug.WriteLine("inserPage：" + insertPage);

                }

                // 上書き
                File.Delete(workingPath);
                File.Move(tempPath, workingPath);
                File.Delete(tempPath);


                // 再表示
                var doc = string.IsNullOrEmpty(currentPassword)
                    ? PdfiumDoc.Load(workingPath)
                    : PdfiumDoc.Load(workingPath, currentPassword);
                pdfViewer1.Document = doc;

                // 保存との整合性 作業用ファイルのデータを入れる
                currentSettings = LoadPdfSettings(workingPath, currentPassword);

                // フラグ
                isDirty = true;
            }
            catch (Exception ex)
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
        private void ImportBookmarksFromPdf(ITextDoc insertPdf, int insertPage, TreeView treeView, bool isReplace)
        {
            var outlines = insertPdf.GetOutlines(false);
            if (outlines == null) return;

            int insertIndex = FindInsertIndex(treeView.Nodes, insertPage);

            foreach (var child in outlines.GetAllChildren())
            {
                var node = CreateNodeFromOutline(insertPdf, child, insertPage, isReplace);
                if (node != null)
                {
                    treeView.Nodes.Insert(insertIndex, node);
                    insertIndex++;
                }
            }

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
        private TreeNode? CreateNodeFromOutline(ITextDoc pdf, PdfOutline outline, int insertPage, bool isReplace)
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
                    newPage = insertPage + page - 1;
                }
                else
                {
                    // 挿入
                    newPage = insertPage + page - 1;
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

                Debug.WriteLine("-----文字スタイル1------------------------");
                Debug.WriteLine("selectedStyle: " + selectedStyle);

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

                //nodes.Add(node);

                Debug.WriteLine("-----しおり名 → ページ番号------------------------");
                Debug.WriteLine($"{title} → Page:{newPage}");

                // 子も再帰
                foreach (var child in outline.GetAllChildren())
                {
                    var childNode = CreateNodeFromOutline(pdf, child, insertPage, isReplace);
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
        // ページ移動
        // ==============================
        private void PageMove_Click(object sender, EventArgs e)
        {
            if (currentSettings == null)
            {
                MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 表示しているページを取得
            int nowPage = pdfViewer1.Renderer.Page + 1;

            // Form11起動
            using (var f = new Form11(nowPage, currentSettings.TotalPage))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {

                    MovePdfPage(f.StartPage, f.EndPage, f.TargetPage, f.MoveBefore);

                }
            }

        }

        // ==============================
        // ページ移動処理
        // ==============================
        private void MovePdfPage(int start, int end, int target, bool before)
        {
            string tempPath = workingPath + ".tmp";

            Dictionary<int, int> pageMap = new Dictionary<int, int>();

            Debug.WriteLine("--- ページ移動 ---------------------");
            Debug.WriteLine("start: " + start);
            Debug.WriteLine("end: " + end);
            Debug.WriteLine("target: " + target);
            Debug.WriteLine("before: " + before);

            try
            {
                // Viewer解放
                pdfViewer1.Document?.Dispose();
                pdfViewer1.Document = null;

                ReaderProperties props = string.IsNullOrEmpty(currentPassword)
                    ? new ReaderProperties()
                    : new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(currentPassword));

                using (var reader = new PdfReader(workingPath, props))
                using (var writer = new PdfWriter(tempPath))
                using (var srcPdf = new ITextDoc(reader))
                using (var destPdf = new ITextDoc(writer))
                {
                    int total = srcPdf.GetNumberOfPages();
                    int count = end - start + 1;

                    // 移動先決定
                    int insertIndex = before ? target : target + 1;

                    // 同一範囲チェック
                    if (insertIndex >= start && insertIndex <= end + 1)
                        return;

                    // 後ろに移動する場合補正
                    if (insertIndex > end)
                    {
                        insertIndex -= count;
                    }

                    int currentPos = 1;

                    for (int i = 1; i <= total; i++)
                    {
                        // 挿入位置に来たら先に挿入
                        if (currentPos == insertIndex)
                        {
                            srcPdf.CopyPagesTo(start, end, destPdf);
                            currentPos += count;
                        }

                        // 移動対象はスキップ
                        if (i >= start && i <= end) continue;

                        srcPdf.CopyPagesTo(i, i, destPdf);
                        currentPos++;
                    }

                    // 最後に入れるケース
                    if (currentPos <= insertIndex)
                    {
                        srcPdf.CopyPagesTo(start, end, destPdf);
                    }

                    // しおり補正ロジック
                    int newPage = 1;

                    for (int i = 1; i <= total; i++)
                    {
                        // 挿入位置
                        if (newPage == insertIndex)
                        {
                            for (int p = start; p <= end; p++)
                            {
                                pageMap[p] = newPage++;
                            }
                        }

                        if (i >= start && i <= end) continue;

                        pageMap[i] = newPage++;
                    }

                    // 最後に入る場合
                    if (newPage <= total)
                    {
                        for (int p = start; p <= end; p++)
                        {
                            pageMap[p] = newPage++;
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

                pdfViewer1.Document = doc;

                currentSettings = LoadPdfSettings(workingPath, currentPassword);

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
        // 置換
        // ==============================
        private void ReplacementMenu_Click(object sender, EventArgs e)
        {
            if (pdfViewer1.Document == null) return;

            if (currentSettings == null)
            {
                MessageBox.Show("PDFが開かれていません。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    // 表示しているページを取得
                    int nowPage = pdfViewer1.Renderer.Page + 1;


                    // Form12起動
                    using (var f = new Form12(replacementPath, nowPage, currentSettings.TotalPage))
                    {
                        if (f.ShowDialog() == DialogResult.OK)
                        {

                            OkikaePdfPage(replacementPath, f.StartPage, f.EndPage);
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
        private void OkikaePdfPage(string okikaePath, int start, int end)
        {

            string? okikaePassword = null;

            ReaderProperties okikaeProps;

            PdfReader? reader = null;
            ITextDoc? iTextDoc = null;

            bool canOkikae = false;

            PassMessage = "置換するPDFファイルは保護されています。" + Environment.NewLine +
                "権限パスワードの場合は置換可能ですが、閲覧パスワードの場合は置換できません。";

            while (true)
            {
                try
                {
                    if (okikaePassword == null)
                    {
                        // まずはパス無しで開く
                        reader = new PdfReader(okikaePath);
                    }
                    else
                    {
                        // パス入力済みなら
                        var props = new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(okikaePassword));
                        // パス付きで開く
                        reader = new PdfReader(okikaePath, props);
                    }

                    // PDFを実際に開く
                    iTextDoc = new ITextDoc(reader);
                    // 管理者(制限パス)で開いてる true:制限パス false:以外)
                    bool isOwner = reader.IsOpenedWithFullPermission();
                    // 暗号化されてる？
                    bool isEncrypted_c = reader.IsEncrypted();

                    if (okikaePassword == null && isEncrypted_c && !isOwner)
                    {
                        // Ownerパスだけ設定されているPDF
                        // → 強制的にパス入力させる
                        iTextDoc.Close();
                        reader.Close();

                        okikaePassword = ShowPasswordDialog();

                        if (okikaePassword == null)
                            return;

                        continue; // 再トライ
                    }

                    if (isOwner)
                    {
                        canOkikae = reader.IsOpenedWithFullPermission();
                    }

                    // ループ抜ける
                    break;


                }
                catch (iText.Kernel.Exceptions.BadPasswordException)
                {
                    okikaePassword = ShowPasswordDialog();
                    if (okikaePassword == null) return;
                }
                catch (Exception ex)
                {
#if DEBUG
                    Extxt.Text = ex.ToString();
                    MessageBox.Show("PDF確認エラー:\n" + ex.Message);
#else
                    MessageBox.Show("置換ファイルが開けませんでした。", "置換ファイルオープン失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
                    return;
                }

            }


            iTextDoc.Close();
            reader.Close();

            // チェック(閲覧パスなら挿入しない)
            if (!canOkikae)
            {
                MessageBox.Show(
                    "入力したパスワードは閲覧パスワードです。" + Environment.NewLine +
                    "ページ置換するには、権限パスワードが必要です。",
                    "挿入不可",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }


            string tempPath = workingPath + ".tmp";


            okikaeProps = string.IsNullOrEmpty(okikaePassword)
                ? new ReaderProperties()
                : new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(okikaePassword));

            try
            {
                pdfViewer1.Document?.Dispose();
                pdfViewer1.Document = null;

                ReaderProperties props = string.IsNullOrEmpty(currentPassword)
                    ? new ReaderProperties()
                    : new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(currentPassword));

                using (var mainReader = new PdfReader(workingPath, props))
                using (var repReader = new PdfReader(okikaePath, okikaeProps))
                using (var writer = new PdfWriter(tempPath))
                using (var mainPdf = new ITextDoc(mainReader))
                using (var repPdf = new ITextDoc(repReader))
                using (var destPdf = new ITextDoc(writer))
                {
                    int total = mainPdf.GetNumberOfPages();
                    int repCount = repPdf.GetNumberOfPages();

                    for (int i = 1; i <= total; i++)
                    {
                        // 置換開始位置
                        if (i == start)
                        {
                            // 置換PDFを全部コピー
                            repPdf.CopyPagesTo(1, repCount, destPdf);
                        }

                        // 置換対象はスキップ
                        if (i >= start && i <= end)
                            continue;

                        // 通常コピー
                        mainPdf.CopyPagesTo(i, i, destPdf);
                    }

                    // しおり補正
                    RemoveBookmarksInRange(start, end);
                    ShiftBookmarksAfter(start, end, repCount);

                    // 置換PDFのしおり追加
                    using (var repReader2 = new PdfReader(okikaePath, okikaeProps))
                    using (var repPdfDoc = new ITextDoc(repReader2))
                    {
                        ImportBookmarksFromPdf(repPdfDoc, start, treeView1, true);

                        Debug.WriteLine("--- 置換 -----------------");
                        Debug.WriteLine("start：" + start);

                    }

                }

                // 上書き
                File.Delete(workingPath);
                File.Move(tempPath, workingPath);
                File.Delete(tempPath);

                // 再表示
                var doc = string.IsNullOrEmpty(currentPassword)
                    ? PdfiumDoc.Load(workingPath)
                    : PdfiumDoc.Load(workingPath, currentPassword);

                pdfViewer1.Document = doc;

                currentSettings = LoadPdfSettings(workingPath, currentPassword);

                isDirty = true;
            }
            catch (Exception ex)
            {
#if DEBUG
                Extxt.Text = ex.Message;
                MessageBox.Show("置換エラー:\n" + ex.ToString());
#else
                MessageBox.Show("置換中にエラーが発生しました。", "置換失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
            }
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
            if (node.Tag is BookmarkInfo info)
            {
                if (info.Page >= start && info.Page <= end)
                {
                    node.Remove();
                    return;
                }
            }

            for (int i = node.Nodes.Count - 1; i >= 0; i--)
            {
                RemoveNode(node.Nodes[i], start, end);
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
        // 画像をPDFに変換
        // ==============================
        private void ConvPdf_Click(object sender, EventArgs e)
        {

            // 変更がある場合(未保存確認ダイアログ)
            if (!ConfirmDiscard())
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

                        // 閉じる処理を呼ぶ
                        CloseCurrentPdf();

                        // 念のためしおり消す
                        treeView1.Nodes.Clear();

                        int pageNum = 1;

                        // PDF作成
                        using (PdfWriter writer = new PdfWriter(sfd.FileName))
                        using (ITextDoc pdf = new ITextDoc(writer))
                        using (iText.Layout.Document document = new iText.Layout.Document(pdf))
                        {

                            treeView1.BeginUpdate();

                            try
                            {
                                //foreach (string imagePath in ofd.FileNames)
                                // 画像ファイルを名前順に並び替えてPDFに変換
                                //foreach (string imagePath in ofd.FileNames.OrderBy(x => x))
                                foreach (string imagePath in ofd.FileNames.OrderBy(x => x, new NaturalStringComparer()))
                                {
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

                                    Debug.WriteLine("--- 画像PDF変換 --------------");
                                    Debug.WriteLine("pageWidth: " + pageWidth.ToString());
                                    Debug.WriteLine("PdfMarginLeft: " + PdfMarginLeft.ToString());
                                    Debug.WriteLine("PdfMarginRight: " + PdfMarginRight.ToString());
                                    Debug.WriteLine("availableWidth: " + availableWidth.ToString());

                                    Debug.WriteLine("pageHeight: " + pageHeight.ToString());
                                    Debug.WriteLine("PdfMarginTop: " + PdfMarginTop.ToString());
                                    Debug.WriteLine("PdfMarginBottom: " + PdfMarginBottom.ToString());
                                    Debug.WriteLine("availableHeight: " + availableHeight.ToString());

                                    Debug.WriteLine("image.GetImageScaledWidth(): " + image.GetImageScaledWidth().ToString());
                                    Debug.WriteLine("image.GetImageScaledHeight(): " + image.GetImageScaledHeight().ToString());

                                    Debug.WriteLine("x: " + x.ToString());
                                    Debug.WriteLine("y: " + y.ToString());

                                    // 追加
                                    document.Add(image);

                                    // ファイル名をしおりに
                                    string bookmarkName = IOPath.GetFileNameWithoutExtension(imagePath);

                                    TreeNode newNode = new TreeNode(bookmarkName)
                                    {
                                        ImageIndex = 0,
                                        SelectedImageIndex = 1
                                    };

                                    newNode.Tag = new BookmarkInfo
                                    {
                                        // しおり名
                                        BmTitle = bookmarkName,
                                        // 表示されているページ
                                        Page = pageNum,
                                        // 色は黒(デフォルト)
                                        SelectedColor = DrawingColor.Black,
                                        // スタイルは標準(デフォルト)
                                        SelectedStyle = FontStyle.Regular,
                                        // 展開
                                        IsOpen = true
                                    };
                                    // ルートに追加
                                    treeView1.Nodes.Add(newNode);
                                    pageNum++;
                                }
                            }
                            finally
                            {
                                // しおり追加終了
                                treeView1.EndUpdate();
                            }

                        }

                        // 保存したファイルパス
                        originalPath = sfd.FileName;

                        try
                        {
                            // 作業用ファイルを破棄(念のため)
                            CleanupWorkingFile();

                            // 作業ファイル作成
                            // C:\Users\<ユーザー名>\AppData\Local\Temp\ に作業用ファイルを置く
                            workingPath = IOPath.Combine(IOPath.GetTempPath(), $"MyPDFwork_{Guid.NewGuid()}.pdf");
                            // 元ファイルを作業用ファイルにコピー true:同じ名前は上書き
                            File.Copy(originalPath, workingPath, true);

                            // Pdfiumで表示
                            PdfiumViewer.PdfDocument document;

                            // パスワードなし
                            document = PdfiumDoc.Load(workingPath);

                            pdfViewer1.Document = document;


                            // 保存との整合性 作業用ファイルのデータを入れる
                            currentSettings = LoadPdfSettings(workingPath, null);

                            // 念のため
                            pdfViewer1.Document?.Dispose();
                            pdfViewer1.Document = null;

                            SavePdf(sfd.FileName);

                            // 自動調整
                            ZoomtoolStripComboBox.SelectedIndex = 0;

                            pdfViewer1.ZoomMode = PdfViewerZoomMode.FitBest;

                            // セキュリティなし
                            canEdit = true;

                            treeView1.LabelEdit = true;

                            // ツリービューの右クリックメニュー ON/OFF
                            UpdateContextMenuState();

                            // 更新をリセット
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
        }

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

    }
}

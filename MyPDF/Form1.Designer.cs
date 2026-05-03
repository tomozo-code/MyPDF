namespace MyPDF
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            splitter1 = new Splitter();
            pdfViewer1 = new PdfiumViewer.PdfViewer();
            contextMenuStrip2 = new ContextMenuStrip(components);
            PageMove = new ToolStripMenuItem();
            toolStripMenuItem13 = new ToolStripSeparator();
            PageInsert = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            ReplacementMenu = new ToolStripMenuItem();
            toolStripMenuItem14 = new ToolStripSeparator();
            PageExtract = new ToolStripMenuItem();
            PageExtractSetting = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripSeparator();
            LeftRotate90 = new ToolStripMenuItem();
            RightRotate90 = new ToolStripMenuItem();
            Rotate180 = new ToolStripMenuItem();
            RotatePagesSetting = new ToolStripMenuItem();
            toolStripMenuItem12 = new ToolStripSeparator();
            PageDelete = new ToolStripMenuItem();
            PageDeleteSetting = new ToolStripMenuItem();
            PageEditMenu = new ToolStripDropDownButton();
            treeView1 = new TreeView();
            contextMenuStrip1 = new ContextMenuStrip(components);
            AddShioriToolStripMenuItem = new ToolStripMenuItem();
            DelShioriToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            AllDelToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem8 = new ToolStripSeparator();
            SetShioriToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripSeparator();
            AllShioriTenkaiToolStripMenuItem = new ToolStripMenuItem();
            AllShioriSyukusyouToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem5 = new ToolStripSeparator();
            ShioriTenkaiToolStripMenuItem = new ToolStripMenuItem();
            ShioriSyukusyouToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem6 = new ToolStripSeparator();
            ImportShioriToolStripMenuItem = new ToolStripMenuItem();
            ExportShioriToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem7 = new ToolStripSeparator();
            ShioriProToolStripMenuItem = new ToolStripMenuItem();
            ShioriMenu = new ToolStripDropDownButton();
            thumbnailImageList = new ImageList(components);
            panel1 = new Panel();
            panel3 = new Panel();
            label1 = new Label();
            treeToolTip = new ToolTip(components);
            panel4 = new Panel();
            label2 = new Label();
            Extxt = new TextBox();
            FileMenu = new ToolStripDropDownButton();
            OpenMenu = new ToolStripMenuItem();
            AcrobatOpenMenu = new ToolStripMenuItem();
            toolStripMenuItem10 = new ToolStripSeparator();
            SaveMenu = new ToolStripMenuItem();
            SaveAsMenu = new ToolStripMenuItem();
            toolStripMenuItem9 = new ToolStripSeparator();
            PdfPropertyMenu = new ToolStripMenuItem();
            SecurityMenu = new ToolStripMenuItem();
            toolStripMenuItem11 = new ToolStripSeparator();
            CloseMenu = new ToolStripMenuItem();
            EndMenu = new ToolStripMenuItem();
            HelpMenu = new ToolStripDropDownButton();
            UseMenu = new ToolStripMenuItem();
            VerMenu = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            NewPagetoolStripTextBox = new ToolStripTextBox();
            TotalPagetoolStripLabel = new ToolStripLabel();
            toolStripSeparator2 = new ToolStripSeparator();
            ZoomtoolStripComboBox = new ToolStripComboBox();
            toolStripSeparator3 = new ToolStripSeparator();
            toolStrip1 = new ToolStrip();
            statusStrip1.SuspendLayout();
            contextMenuStrip2.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 388);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 16, 0);
            statusStrip1.Size = new Size(964, 26);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(138, 21);
            toolStripStatusLabel1.Text = "ファイル: PDF未選択";
            // 
            // splitter1
            // 
            splitter1.Cursor = Cursors.SizeWE;
            splitter1.Location = new Point(199, 35);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(8, 353);
            splitter1.TabIndex = 3;
            splitter1.TabStop = false;
            // 
            // pdfViewer1
            // 
            pdfViewer1.BorderStyle = BorderStyle.FixedSingle;
            pdfViewer1.ContextMenuStrip = contextMenuStrip2;
            pdfViewer1.Location = new Point(543, 97);
            pdfViewer1.Margin = new Padding(4, 4, 4, 4);
            pdfViewer1.Name = "pdfViewer1";
            pdfViewer1.Size = new Size(310, 225);
            pdfViewer1.TabIndex = 0;
            pdfViewer1.ZoomMode = PdfiumViewer.PdfViewerZoomMode.FitBest;
            // 
            // contextMenuStrip2
            // 
            contextMenuStrip2.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            contextMenuStrip2.Items.AddRange(new ToolStripItem[] { PageMove, toolStripMenuItem13, PageInsert, toolStripMenuItem2, ReplacementMenu, toolStripMenuItem14, PageExtract, PageExtractSetting, toolStripMenuItem3, LeftRotate90, RightRotate90, Rotate180, RotatePagesSetting, toolStripMenuItem12, PageDelete, PageDeleteSetting });
            contextMenuStrip2.Name = "contextMenuStrip2";
            contextMenuStrip2.Size = new Size(246, 320);
            // 
            // PageMove
            // 
            PageMove.Enabled = false;
            PageMove.Name = "PageMove";
            PageMove.Size = new Size(245, 26);
            PageMove.Text = "移動(&M)...";
            PageMove.ToolTipText = "ページを指定して移動します";
            PageMove.Click += PageMove_Click;
            PageMove.MouseEnter += menuStrip1_MouseEnter;
            PageMove.MouseLeave += menuStrip1_MouseLeave;
            // 
            // toolStripMenuItem13
            // 
            toolStripMenuItem13.Name = "toolStripMenuItem13";
            toolStripMenuItem13.Size = new Size(242, 6);
            // 
            // PageInsert
            // 
            PageInsert.Enabled = false;
            PageInsert.Name = "PageInsert";
            PageInsert.Size = new Size(245, 26);
            PageInsert.Text = "挿入(&I)...";
            PageInsert.ToolTipText = "ファイルからページを挿入します";
            PageInsert.Click += PageInsert_Click;
            PageInsert.MouseEnter += menuStrip1_MouseEnter;
            PageInsert.MouseLeave += menuStrip1_MouseLeave;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(242, 6);
            // 
            // ReplacementMenu
            // 
            ReplacementMenu.Enabled = false;
            ReplacementMenu.Name = "ReplacementMenu";
            ReplacementMenu.Size = new Size(245, 26);
            ReplacementMenu.Text = "置換(&K)...";
            ReplacementMenu.ToolTipText = "ファイルからページを置換します";
            ReplacementMenu.Click += ReplacementMenu_Click;
            ReplacementMenu.MouseEnter += menuStrip1_MouseEnter;
            ReplacementMenu.MouseLeave += menuStrip1_MouseLeave;
            // 
            // toolStripMenuItem14
            // 
            toolStripMenuItem14.Name = "toolStripMenuItem14";
            toolStripMenuItem14.Size = new Size(242, 6);
            // 
            // PageExtract
            // 
            PageExtract.Enabled = false;
            PageExtract.Name = "PageExtract";
            PageExtract.Size = new Size(245, 26);
            PageExtract.Text = "抽出(&Q)";
            PageExtract.ToolTipText = "表示しているページを抽出します";
            PageExtract.Click += PageExtract_Click;
            PageExtract.MouseEnter += menuStrip1_MouseEnter;
            PageExtract.MouseLeave += menuStrip1_MouseLeave;
            // 
            // PageExtractSetting
            // 
            PageExtractSetting.Enabled = false;
            PageExtractSetting.Name = "PageExtractSetting";
            PageExtractSetting.Size = new Size(245, 26);
            PageExtractSetting.Text = "ページを指定して抽出(&X)...";
            PageExtractSetting.ToolTipText = "ページを指定して抽出します";
            PageExtractSetting.Click += PageExtractSetting_Click;
            PageExtractSetting.MouseEnter += menuStrip1_MouseEnter;
            PageExtractSetting.MouseLeave += menuStrip1_MouseLeave;
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(242, 6);
            // 
            // LeftRotate90
            // 
            LeftRotate90.Enabled = false;
            LeftRotate90.Name = "LeftRotate90";
            LeftRotate90.Size = new Size(245, 26);
            LeftRotate90.Text = "左へ90°回転";
            LeftRotate90.ToolTipText = "表示しているページを左へ90°回転します";
            LeftRotate90.Click += LeftRotate90_Click;
            LeftRotate90.MouseEnter += menuStrip1_MouseEnter;
            LeftRotate90.MouseLeave += menuStrip1_MouseLeave;
            // 
            // RightRotate90
            // 
            RightRotate90.Enabled = false;
            RightRotate90.Name = "RightRotate90";
            RightRotate90.Size = new Size(245, 26);
            RightRotate90.Text = "右へ90°回転";
            RightRotate90.ToolTipText = "表示しているページを右へ90°回転します";
            RightRotate90.Click += RightRotate90_Click;
            RightRotate90.MouseEnter += menuStrip1_MouseEnter;
            RightRotate90.MouseLeave += menuStrip1_MouseLeave;
            // 
            // Rotate180
            // 
            Rotate180.Enabled = false;
            Rotate180.Name = "Rotate180";
            Rotate180.Size = new Size(245, 26);
            Rotate180.Text = "180°回転";
            Rotate180.ToolTipText = "表示しているページを180°回転します";
            Rotate180.Click += Rotate180_Click;
            Rotate180.MouseEnter += menuStrip1_MouseEnter;
            Rotate180.MouseLeave += menuStrip1_MouseLeave;
            // 
            // RotatePagesSetting
            // 
            RotatePagesSetting.Enabled = false;
            RotatePagesSetting.Name = "RotatePagesSetting";
            RotatePagesSetting.Size = new Size(245, 26);
            RotatePagesSetting.Text = "ページを指定して回転(&R)...";
            RotatePagesSetting.ToolTipText = "ページを指定して回転します";
            RotatePagesSetting.Click += RotatePagesSetting_Click;
            RotatePagesSetting.MouseEnter += menuStrip1_MouseEnter;
            RotatePagesSetting.MouseLeave += menuStrip1_MouseLeave;
            // 
            // toolStripMenuItem12
            // 
            toolStripMenuItem12.Name = "toolStripMenuItem12";
            toolStripMenuItem12.Size = new Size(242, 6);
            // 
            // PageDelete
            // 
            PageDelete.Enabled = false;
            PageDelete.Name = "PageDelete";
            PageDelete.Size = new Size(245, 26);
            PageDelete.Text = "削除(&W)";
            PageDelete.ToolTipText = "表示しているページを削除します";
            PageDelete.Click += PageDelete_Click;
            PageDelete.MouseEnter += menuStrip1_MouseEnter;
            PageDelete.MouseLeave += menuStrip1_MouseLeave;
            // 
            // PageDeleteSetting
            // 
            PageDeleteSetting.Enabled = false;
            PageDeleteSetting.Name = "PageDeleteSetting";
            PageDeleteSetting.Size = new Size(245, 26);
            PageDeleteSetting.Text = "ページを指定して削除(&D)...";
            PageDeleteSetting.ToolTipText = "ページを指定して削除します";
            PageDeleteSetting.Click += PageDeleteSetting_Click;
            PageDeleteSetting.MouseEnter += menuStrip1_MouseEnter;
            PageDeleteSetting.MouseLeave += menuStrip1_MouseLeave;
            // 
            // PageEditMenu
            // 
            PageEditMenu.DisplayStyle = ToolStripItemDisplayStyle.Text;
            PageEditMenu.DropDown = contextMenuStrip2;
            PageEditMenu.Enabled = false;
            PageEditMenu.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            PageEditMenu.Image = (Image)resources.GetObject("PageEditMenu.Image");
            PageEditMenu.ImageTransparentColor = Color.Magenta;
            PageEditMenu.Name = "PageEditMenu";
            PageEditMenu.Size = new Size(91, 32);
            PageEditMenu.Text = "ページ編集";
            PageEditMenu.ToolTipText = "ページ編集メニュー";
            PageEditMenu.MouseEnter += menuStrip1_MouseEnter;
            PageEditMenu.MouseLeave += menuStrip1_MouseLeave;
            // 
            // treeView1
            // 
            treeView1.AllowDrop = true;
            treeView1.BackColor = SystemColors.Control;
            treeView1.BorderStyle = BorderStyle.FixedSingle;
            treeView1.ContextMenuStrip = contextMenuStrip1;
            treeView1.DrawMode = TreeViewDrawMode.OwnerDrawText;
            treeView1.LabelEdit = true;
            treeView1.Location = new Point(33, 132);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(121, 97);
            treeView1.TabIndex = 0;
            treeView1.AfterLabelEdit += treeView1_AfterLabelEdit;
            treeView1.AfterCollapse += treeView1_AfterCollapse;
            treeView1.AfterExpand += treeView1_AfterExpand;
            treeView1.DrawNode += treeView1_DrawNode;
            treeView1.ItemDrag += treeView1_ItemDrag;
            treeView1.AfterSelect += treeView1_AfterSelect;
            treeView1.DragDrop += treeView1_DragDrop;
            treeView1.DragEnter += treeView1_DragEnter;
            treeView1.DragOver += treeView1_DragOver;
            treeView1.DragLeave += treeView1_DragLeave;
            treeView1.KeyDown += treeView1_KeyDown;
            treeView1.MouseDown += treeView1_MouseDown;
            treeView1.MouseMove += treeView1_MouseMove;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { AddShioriToolStripMenuItem, DelShioriToolStripMenuItem, toolStripMenuItem1, AllDelToolStripMenuItem, toolStripMenuItem8, SetShioriToolStripMenuItem, toolStripMenuItem4, AllShioriTenkaiToolStripMenuItem, AllShioriSyukusyouToolStripMenuItem, toolStripMenuItem5, ShioriTenkaiToolStripMenuItem, ShioriSyukusyouToolStripMenuItem, toolStripMenuItem6, ImportShioriToolStripMenuItem, ExportShioriToolStripMenuItem, toolStripMenuItem7, ShioriProToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(303, 326);
            // 
            // AddShioriToolStripMenuItem
            // 
            AddShioriToolStripMenuItem.Enabled = false;
            AddShioriToolStripMenuItem.Name = "AddShioriToolStripMenuItem";
            AddShioriToolStripMenuItem.Size = new Size(302, 26);
            AddShioriToolStripMenuItem.Text = "しおり作成(&B)";
            AddShioriToolStripMenuItem.ToolTipText = "しおりを作成します";
            AddShioriToolStripMenuItem.Click += AddShioriToolStripMenuItem_Click;
            AddShioriToolStripMenuItem.MouseEnter += menuStrip1_MouseEnter;
            AddShioriToolStripMenuItem.MouseLeave += menuStrip1_MouseLeave;
            // 
            // DelShioriToolStripMenuItem
            // 
            DelShioriToolStripMenuItem.Enabled = false;
            DelShioriToolStripMenuItem.Name = "DelShioriToolStripMenuItem";
            DelShioriToolStripMenuItem.Size = new Size(302, 26);
            DelShioriToolStripMenuItem.Text = "しおり削除";
            DelShioriToolStripMenuItem.ToolTipText = "しおりを削除します";
            DelShioriToolStripMenuItem.Click += DelShioriToolStripMenuItem_Click;
            DelShioriToolStripMenuItem.MouseEnter += menuStrip1_MouseEnter;
            DelShioriToolStripMenuItem.MouseLeave += menuStrip1_MouseLeave;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(299, 6);
            // 
            // AllDelToolStripMenuItem
            // 
            AllDelToolStripMenuItem.Enabled = false;
            AllDelToolStripMenuItem.Name = "AllDelToolStripMenuItem";
            AllDelToolStripMenuItem.Size = new Size(302, 26);
            AllDelToolStripMenuItem.Text = "全てのしおり削除";
            AllDelToolStripMenuItem.ToolTipText = "全てのしおりを削除します";
            AllDelToolStripMenuItem.Click += AllDelToolStripMenuItem_Click;
            AllDelToolStripMenuItem.MouseEnter += menuStrip1_MouseEnter;
            AllDelToolStripMenuItem.MouseLeave += menuStrip1_MouseLeave;
            // 
            // toolStripMenuItem8
            // 
            toolStripMenuItem8.Name = "toolStripMenuItem8";
            toolStripMenuItem8.Size = new Size(299, 6);
            // 
            // SetShioriToolStripMenuItem
            // 
            SetShioriToolStripMenuItem.Enabled = false;
            SetShioriToolStripMenuItem.Name = "SetShioriToolStripMenuItem";
            SetShioriToolStripMenuItem.Size = new Size(302, 26);
            SetShioriToolStripMenuItem.Text = "現在のページ番号をしおりに設定(L)";
            SetShioriToolStripMenuItem.ToolTipText = "表示しているページ番号を選択しているしおりに設定します";
            SetShioriToolStripMenuItem.Click += SetShioriToolStripMenuItem_Click;
            SetShioriToolStripMenuItem.MouseEnter += menuStrip1_MouseEnter;
            SetShioriToolStripMenuItem.MouseLeave += menuStrip1_MouseLeave;
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new Size(299, 6);
            // 
            // AllShioriTenkaiToolStripMenuItem
            // 
            AllShioriTenkaiToolStripMenuItem.Enabled = false;
            AllShioriTenkaiToolStripMenuItem.Name = "AllShioriTenkaiToolStripMenuItem";
            AllShioriTenkaiToolStripMenuItem.Size = new Size(302, 26);
            AllShioriTenkaiToolStripMenuItem.Text = "全てのしおりを展開(&1)";
            AllShioriTenkaiToolStripMenuItem.ToolTipText = "全てのしおりを展開します";
            AllShioriTenkaiToolStripMenuItem.Click += AllShioriTenkaiToolStripMenuItem_Click;
            AllShioriTenkaiToolStripMenuItem.MouseEnter += menuStrip1_MouseEnter;
            AllShioriTenkaiToolStripMenuItem.MouseLeave += menuStrip1_MouseLeave;
            // 
            // AllShioriSyukusyouToolStripMenuItem
            // 
            AllShioriSyukusyouToolStripMenuItem.Enabled = false;
            AllShioriSyukusyouToolStripMenuItem.Name = "AllShioriSyukusyouToolStripMenuItem";
            AllShioriSyukusyouToolStripMenuItem.Size = new Size(302, 26);
            AllShioriSyukusyouToolStripMenuItem.Text = "全てのしおりを縮小(2)";
            AllShioriSyukusyouToolStripMenuItem.ToolTipText = "全てのしおりを縮小します(全て畳みます)";
            AllShioriSyukusyouToolStripMenuItem.Click += AllShioriSyukusyouToolStripMenuItem_Click;
            AllShioriSyukusyouToolStripMenuItem.MouseEnter += menuStrip1_MouseEnter;
            AllShioriSyukusyouToolStripMenuItem.MouseLeave += menuStrip1_MouseLeave;
            // 
            // toolStripMenuItem5
            // 
            toolStripMenuItem5.Name = "toolStripMenuItem5";
            toolStripMenuItem5.Size = new Size(299, 6);
            // 
            // ShioriTenkaiToolStripMenuItem
            // 
            ShioriTenkaiToolStripMenuItem.Enabled = false;
            ShioriTenkaiToolStripMenuItem.Name = "ShioriTenkaiToolStripMenuItem";
            ShioriTenkaiToolStripMenuItem.Size = new Size(302, 26);
            ShioriTenkaiToolStripMenuItem.Text = "選択中のしおりを展開(&3)";
            ShioriTenkaiToolStripMenuItem.ToolTipText = "選択中のしおりを展開します";
            ShioriTenkaiToolStripMenuItem.Click += ShioriTenkaiToolStripMenuItem_Click;
            ShioriTenkaiToolStripMenuItem.MouseEnter += menuStrip1_MouseEnter;
            ShioriTenkaiToolStripMenuItem.MouseLeave += menuStrip1_MouseLeave;
            // 
            // ShioriSyukusyouToolStripMenuItem
            // 
            ShioriSyukusyouToolStripMenuItem.Enabled = false;
            ShioriSyukusyouToolStripMenuItem.Name = "ShioriSyukusyouToolStripMenuItem";
            ShioriSyukusyouToolStripMenuItem.Size = new Size(302, 26);
            ShioriSyukusyouToolStripMenuItem.Text = "選択中のしおりを縮小(&4)";
            ShioriSyukusyouToolStripMenuItem.ToolTipText = "選択中のしおりを縮小します(畳みます)";
            ShioriSyukusyouToolStripMenuItem.Click += ShioriSyukusyouToolStripMenuItem_Click;
            ShioriSyukusyouToolStripMenuItem.MouseEnter += menuStrip1_MouseEnter;
            ShioriSyukusyouToolStripMenuItem.MouseLeave += menuStrip1_MouseLeave;
            // 
            // toolStripMenuItem6
            // 
            toolStripMenuItem6.Name = "toolStripMenuItem6";
            toolStripMenuItem6.Size = new Size(299, 6);
            // 
            // ImportShioriToolStripMenuItem
            // 
            ImportShioriToolStripMenuItem.Enabled = false;
            ImportShioriToolStripMenuItem.Name = "ImportShioriToolStripMenuItem";
            ImportShioriToolStripMenuItem.Size = new Size(302, 26);
            ImportShioriToolStripMenuItem.Text = "しおりインポート(&I)...";
            ImportShioriToolStripMenuItem.ToolTipText = "CSV形式のしおりデータをインポート(読み込み)";
            ImportShioriToolStripMenuItem.Click += ImportShioriToolStripMenuItem_Click;
            ImportShioriToolStripMenuItem.MouseEnter += menuStrip1_MouseEnter;
            ImportShioriToolStripMenuItem.MouseLeave += menuStrip1_MouseLeave;
            // 
            // ExportShioriToolStripMenuItem
            // 
            ExportShioriToolStripMenuItem.Enabled = false;
            ExportShioriToolStripMenuItem.Name = "ExportShioriToolStripMenuItem";
            ExportShioriToolStripMenuItem.Size = new Size(302, 26);
            ExportShioriToolStripMenuItem.Text = "しおりエクスポート(&E)...";
            ExportShioriToolStripMenuItem.ToolTipText = "しおりデータをCSV形式でエクスポート(書き出し)";
            ExportShioriToolStripMenuItem.Click += ExportShioriToolStripMenuItem_Click;
            ExportShioriToolStripMenuItem.MouseEnter += menuStrip1_MouseEnter;
            ExportShioriToolStripMenuItem.MouseLeave += menuStrip1_MouseLeave;
            // 
            // toolStripMenuItem7
            // 
            toolStripMenuItem7.Name = "toolStripMenuItem7";
            toolStripMenuItem7.Size = new Size(299, 6);
            // 
            // ShioriProToolStripMenuItem
            // 
            ShioriProToolStripMenuItem.Enabled = false;
            ShioriProToolStripMenuItem.Name = "ShioriProToolStripMenuItem";
            ShioriProToolStripMenuItem.Size = new Size(302, 26);
            ShioriProToolStripMenuItem.Text = "しおりのプロパティ(&A)...";
            ShioriProToolStripMenuItem.ToolTipText = "しおりのスタイル／色を設定します";
            ShioriProToolStripMenuItem.Click += ShioriProToolStripMenuItem_Click;
            ShioriProToolStripMenuItem.MouseEnter += menuStrip1_MouseEnter;
            ShioriProToolStripMenuItem.MouseLeave += menuStrip1_MouseLeave;
            // 
            // ShioriMenu
            // 
            ShioriMenu.DisplayStyle = ToolStripItemDisplayStyle.Text;
            ShioriMenu.DropDown = contextMenuStrip1;
            ShioriMenu.Enabled = false;
            ShioriMenu.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            ShioriMenu.Image = (Image)resources.GetObject("ShioriMenu.Image");
            ShioriMenu.ImageTransparentColor = Color.Magenta;
            ShioriMenu.Name = "ShioriMenu";
            ShioriMenu.Size = new Size(90, 32);
            ShioriMenu.Text = "しおり編集";
            ShioriMenu.ToolTipText = "しおり編集メニュー";
            // 
            // thumbnailImageList
            // 
            thumbnailImageList.ColorDepth = ColorDepth.Depth32Bit;
            thumbnailImageList.ImageSize = new Size(128, 128);
            thumbnailImageList.TransparentColor = Color.Transparent;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.Control;
            panel1.Controls.Add(treeView1);
            panel1.Controls.Add(panel3);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 35);
            panel1.Name = "panel1";
            panel1.Size = new Size(199, 353);
            panel1.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel3.Controls.Add(label1);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(199, 30);
            panel3.TabIndex = 7;
            panel3.Tag = "しおりの表示、追加、削除等を行うことができます";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(4, 4);
            label1.Name = "label1";
            label1.Size = new Size(84, 21);
            label1.TabIndex = 0;
            label1.Tag = "しおりの表示、追加、削除等を行うことができます";
            label1.Text = "しおりパネル";
            // 
            // panel4
            // 
            panel4.BorderStyle = BorderStyle.FixedSingle;
            panel4.Controls.Add(label2);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(207, 35);
            panel4.Name = "panel4";
            panel4.Size = new Size(757, 30);
            panel4.TabIndex = 7;
            panel4.Tag = "PDFファイルを表示します";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 5);
            label2.Name = "label2";
            label2.Size = new Size(79, 21);
            label2.TabIndex = 8;
            label2.Tag = "PDFファイルを表示します";
            label2.Text = "ビューパネル";
            // 
            // Extxt
            // 
            Extxt.Location = new Point(213, 71);
            Extxt.Multiline = true;
            Extxt.Name = "Extxt";
            Extxt.ReadOnly = true;
            Extxt.Size = new Size(252, 85);
            Extxt.TabIndex = 8;
            Extxt.Visible = false;
            // 
            // FileMenu
            // 
            FileMenu.DisplayStyle = ToolStripItemDisplayStyle.Text;
            FileMenu.DropDownItems.AddRange(new ToolStripItem[] { OpenMenu, AcrobatOpenMenu, toolStripMenuItem10, SaveMenu, SaveAsMenu, toolStripMenuItem9, PdfPropertyMenu, SecurityMenu, toolStripMenuItem11, CloseMenu, EndMenu });
            FileMenu.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            FileMenu.ImageTransparentColor = Color.Magenta;
            FileMenu.Name = "FileMenu";
            FileMenu.Size = new Size(86, 32);
            FileMenu.Text = "ファイル(&F)";
            FileMenu.ToolTipText = "ファイルメニュー";
            FileMenu.MouseEnter += menuStrip1_MouseEnter;
            FileMenu.MouseLeave += menuStrip1_MouseLeave;
            // 
            // OpenMenu
            // 
            OpenMenu.Name = "OpenMenu";
            OpenMenu.Size = new Size(256, 26);
            OpenMenu.Text = "開く(&O)...";
            OpenMenu.ToolTipText = "PDFファイルを開きます";
            OpenMenu.Click += OpenMenu_Click;
            OpenMenu.MouseEnter += menuStrip1_MouseEnter;
            OpenMenu.MouseLeave += menuStrip1_MouseLeave;
            // 
            // AcrobatOpenMenu
            // 
            AcrobatOpenMenu.Enabled = false;
            AcrobatOpenMenu.Name = "AcrobatOpenMenu";
            AcrobatOpenMenu.Size = new Size(256, 26);
            AcrobatOpenMenu.Text = "既定のPDFアプリで開く(&G)...";
            AcrobatOpenMenu.ToolTipText = "開いているPDFファイルを既定のアプリで開きます";
            AcrobatOpenMenu.Click += AcrobatOpenMenu_Click;
            AcrobatOpenMenu.MouseEnter += menuStrip1_MouseEnter;
            AcrobatOpenMenu.MouseLeave += menuStrip1_MouseLeave;
            // 
            // toolStripMenuItem10
            // 
            toolStripMenuItem10.Name = "toolStripMenuItem10";
            toolStripMenuItem10.Size = new Size(253, 6);
            // 
            // SaveMenu
            // 
            SaveMenu.Enabled = false;
            SaveMenu.Name = "SaveMenu";
            SaveMenu.Size = new Size(256, 26);
            SaveMenu.Text = "上書き保存(&S)";
            SaveMenu.ToolTipText = "開いているPDFファイルを上書き保存します";
            SaveMenu.Click += SaveMenu_Click;
            SaveMenu.MouseEnter += menuStrip1_MouseEnter;
            SaveMenu.MouseLeave += menuStrip1_MouseLeave;
            // 
            // SaveAsMenu
            // 
            SaveAsMenu.Enabled = false;
            SaveAsMenu.Name = "SaveAsMenu";
            SaveAsMenu.Size = new Size(256, 26);
            SaveAsMenu.Text = "名前を付けて保存(&A)...";
            SaveAsMenu.ToolTipText = "開いているPDFファイルに名前を付けて保存します";
            SaveAsMenu.Click += SaveAsMenu_Click;
            SaveAsMenu.MouseEnter += menuStrip1_MouseEnter;
            SaveAsMenu.MouseLeave += menuStrip1_MouseLeave;
            // 
            // toolStripMenuItem9
            // 
            toolStripMenuItem9.Name = "toolStripMenuItem9";
            toolStripMenuItem9.Size = new Size(253, 6);
            // 
            // PdfPropertyMenu
            // 
            PdfPropertyMenu.Enabled = false;
            PdfPropertyMenu.Name = "PdfPropertyMenu";
            PdfPropertyMenu.Size = new Size(256, 26);
            PdfPropertyMenu.Text = "PDFのプロパティ(&D)...";
            PdfPropertyMenu.ToolTipText = "PDFファイルのプロパティを編集します";
            PdfPropertyMenu.Click += PdfPropertyMenu_Click;
            PdfPropertyMenu.MouseEnter += menuStrip1_MouseEnter;
            PdfPropertyMenu.MouseLeave += menuStrip1_MouseLeave;
            // 
            // SecurityMenu
            // 
            SecurityMenu.Enabled = false;
            SecurityMenu.Name = "SecurityMenu";
            SecurityMenu.Size = new Size(256, 26);
            SecurityMenu.Text = "セキュリティ設定(&T)...";
            SecurityMenu.ToolTipText = "PDFファイルにセキュリティを設定します";
            SecurityMenu.Click += SecurityMenu_Click;
            SecurityMenu.MouseEnter += menuStrip1_MouseEnter;
            SecurityMenu.MouseLeave += menuStrip1_MouseLeave;
            // 
            // toolStripMenuItem11
            // 
            toolStripMenuItem11.Name = "toolStripMenuItem11";
            toolStripMenuItem11.Size = new Size(253, 6);
            // 
            // CloseMenu
            // 
            CloseMenu.Enabled = false;
            CloseMenu.Name = "CloseMenu";
            CloseMenu.Size = new Size(256, 26);
            CloseMenu.Text = "閉じる(&Y)";
            CloseMenu.ToolTipText = "開いているPDFファイルを閉じます";
            CloseMenu.Click += CloseMenu_Click;
            CloseMenu.MouseEnter += menuStrip1_MouseEnter;
            CloseMenu.MouseLeave += menuStrip1_MouseLeave;
            // 
            // EndMenu
            // 
            EndMenu.Name = "EndMenu";
            EndMenu.Size = new Size(256, 26);
            EndMenu.Text = "終了(&X)";
            EndMenu.ToolTipText = "ともさんのPDF編集帖を終了します";
            EndMenu.Click += EndMenu_Click;
            EndMenu.MouseEnter += menuStrip1_MouseEnter;
            EndMenu.MouseLeave += menuStrip1_MouseLeave;
            // 
            // HelpMenu
            // 
            HelpMenu.DisplayStyle = ToolStripItemDisplayStyle.Text;
            HelpMenu.DropDownItems.AddRange(new ToolStripItem[] { UseMenu, VerMenu });
            HelpMenu.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            HelpMenu.ImageTransparentColor = Color.Magenta;
            HelpMenu.Name = "HelpMenu";
            HelpMenu.Size = new Size(82, 32);
            HelpMenu.Text = "ヘルプ(&H)";
            HelpMenu.ToolTipText = "ヘルプメニュー";
            HelpMenu.MouseEnter += menuStrip1_MouseEnter;
            HelpMenu.MouseLeave += menuStrip1_MouseLeave;
            // 
            // UseMenu
            // 
            UseMenu.Name = "UseMenu";
            UseMenu.Size = new Size(199, 26);
            UseMenu.Text = "使い方(&U)...";
            UseMenu.ToolTipText = "使い方を表示します";
            UseMenu.Click += UseMenu_Click;
            UseMenu.MouseEnter += menuStrip1_MouseEnter;
            UseMenu.MouseLeave += menuStrip1_MouseLeave;
            // 
            // VerMenu
            // 
            VerMenu.Name = "VerMenu";
            VerMenu.Size = new Size(199, 26);
            VerMenu.Text = "バージョン情報(&A)...";
            VerMenu.ToolTipText = "バージョン情報を表示します";
            VerMenu.Click += VerMenu_Click;
            VerMenu.MouseEnter += menuStrip1_MouseEnter;
            VerMenu.MouseLeave += menuStrip1_MouseLeave;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 35);
            // 
            // NewPagetoolStripTextBox
            // 
            NewPagetoolStripTextBox.Enabled = false;
            NewPagetoolStripTextBox.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            NewPagetoolStripTextBox.Name = "NewPagetoolStripTextBox";
            NewPagetoolStripTextBox.Size = new Size(100, 35);
            NewPagetoolStripTextBox.Tag = "";
            NewPagetoolStripTextBox.Text = "1";
            NewPagetoolStripTextBox.TextBoxTextAlign = HorizontalAlignment.Right;
            NewPagetoolStripTextBox.ToolTipText = "特定のページ番号に移動します";
            NewPagetoolStripTextBox.KeyDown += NewPagetoolStripTextBox_KeyDown;
            NewPagetoolStripTextBox.Click += NewPagetoolStripTextBox_Click;
            NewPagetoolStripTextBox.MouseEnter += menuStrip1_MouseEnter;
            NewPagetoolStripTextBox.MouseLeave += menuStrip1_MouseLeave;
            // 
            // TotalPagetoolStripLabel
            // 
            TotalPagetoolStripLabel.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            TotalPagetoolStripLabel.Name = "TotalPagetoolStripLabel";
            TotalPagetoolStripLabel.Size = new Size(88, 32);
            TotalPagetoolStripLabel.Text = "/ 総ページ数";
            TotalPagetoolStripLabel.ToolTipText = "開いているPDFファイルの総ページ数です";
            TotalPagetoolStripLabel.MouseEnter += menuStrip1_MouseEnter;
            TotalPagetoolStripLabel.MouseLeave += menuStrip1_MouseLeave;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 35);
            // 
            // ZoomtoolStripComboBox
            // 
            ZoomtoolStripComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ZoomtoolStripComboBox.Enabled = false;
            ZoomtoolStripComboBox.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            ZoomtoolStripComboBox.Name = "ZoomtoolStripComboBox";
            ZoomtoolStripComboBox.Size = new Size(150, 35);
            ZoomtoolStripComboBox.ToolTipText = "開いているPDFファイルの表示方法を選択します";
            ZoomtoolStripComboBox.SelectedIndexChanged += ZoomtoolStripComboBox_SelectedIndexChanged;
            ZoomtoolStripComboBox.MouseEnter += menuStrip1_MouseEnter;
            ZoomtoolStripComboBox.MouseLeave += menuStrip1_MouseLeave;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 35);
            // 
            // toolStrip1
            // 
            toolStrip1.AutoSize = false;
            toolStrip1.Items.AddRange(new ToolStripItem[] { FileMenu, ShioriMenu, PageEditMenu, HelpMenu, toolStripSeparator1, NewPagetoolStripTextBox, TotalPagetoolStripLabel, toolStripSeparator2, ZoomtoolStripComboBox, toolStripSeparator3 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(964, 35);
            toolStrip1.TabIndex = 6;
            toolStrip1.Text = "toolStrip1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(964, 414);
            Controls.Add(pdfViewer1);
            Controls.Add(Extxt);
            Controls.Add(panel4);
            Controls.Add(splitter1);
            Controls.Add(panel1);
            Controls.Add(statusStrip1);
            Controls.Add(toolStrip1);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ともさんのPDF編集帖";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            contextMenuStrip2.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private StatusStrip statusStrip1;
        private Splitter splitter1;
        private Panel panel1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private TreeView treeView1;
        private PdfiumViewer.PdfViewer pdfViewer1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem AddShioriToolStripMenuItem;
        private ToolStripMenuItem DelShioriToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem SetShioriToolStripMenuItem;
        private ImageList thumbnailImageList;
        private ToolStripSeparator toolStripMenuItem4;
        private ToolStripMenuItem ShioriTenkaiToolStripMenuItem;
        private ToolStripMenuItem ShioriSyukusyouToolStripMenuItem;
        private ToolStripMenuItem AllShioriTenkaiToolStripMenuItem;
        private ToolStripMenuItem AllShioriSyukusyouToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem5;
        private ToolStripSeparator toolStripMenuItem6;
        private ToolStripMenuItem ShioriProToolStripMenuItem;
        private ToolTip treeToolTip;
        private ToolStripMenuItem ImportShioriToolStripMenuItem;
        private ToolStripMenuItem ExportShioriToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem7;
        private ToolStripMenuItem AllDelToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem8;
        private Panel panel3;
        private Label label1;
        private Panel panel4;
        private Label label2;
        private TextBox Extxt;
        private ToolStripDropDownButton FileMenu;
        private ToolStripMenuItem OpenMenu;
        private ToolStripMenuItem AcrobatOpenMenu;
        private ToolStripSeparator toolStripMenuItem10;
        private ToolStripMenuItem SaveMenu;
        private ToolStripMenuItem SaveAsMenu;
        private ToolStripSeparator toolStripMenuItem9;
        private ToolStripMenuItem PdfPropertyMenu;
        private ToolStripMenuItem SecurityMenu;
        private ToolStripSeparator toolStripMenuItem11;
        private ToolStripMenuItem EndMenu;
        private ToolStripDropDownButton PageEditMenu;
        private ToolStripDropDownButton HelpMenu;
        private ToolStripMenuItem UseMenu;
        private ToolStripMenuItem VerMenu;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripTextBox NewPagetoolStripTextBox;
        private ToolStripLabel TotalPagetoolStripLabel;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripComboBox ZoomtoolStripComboBox;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStrip toolStrip1;
        private ToolStripMenuItem CloseMenu;
        private ContextMenuStrip contextMenuStrip2;
        private ToolStripMenuItem PageInsert;
        private ToolStripMenuItem PageExtract;
        private ToolStripMenuItem PageExtractSetting;
        private ToolStripMenuItem PageDelete;
        private ToolStripMenuItem PageDeleteSetting;
        private ToolStripMenuItem LeftRotate90;
        private ToolStripMenuItem RightRotate90;
        private ToolStripMenuItem Rotate180;
        private ToolStripMenuItem RotatePagesSetting;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripSeparator toolStripMenuItem12;
        private ToolStripDropDownButton ShioriMenu;
        private ToolStripMenuItem PageMove;
        private ToolStripSeparator toolStripMenuItem13;
        private ToolStripMenuItem ReplacementMenu;
        private ToolStripSeparator toolStripMenuItem14;
    }
}

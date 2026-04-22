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
            menuStrip1 = new MenuStrip();
            FileToolStripMenuItem = new ToolStripMenuItem();
            OpenToolStripMenuItem = new ToolStripMenuItem();
            AcrobatOpenToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem9 = new ToolStripSeparator();
            SaveToolStripMenuItem = new ToolStripMenuItem();
            SaveAsToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            PdfSetToolStripMenuItem = new ToolStripMenuItem();
            SecurityToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem3 = new ToolStripSeparator();
            XToolStripMenuItem = new ToolStripMenuItem();
            HelpToolStripMenuItem = new ToolStripMenuItem();
            UseToolStripMenuItem = new ToolStripMenuItem();
            VerToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            splitter1 = new Splitter();
            panel2 = new Panel();
            pdfViewer1 = new PdfiumViewer.PdfViewer();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
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
            tabPage2 = new TabPage();
            listView1 = new ListView();
            thumbnailImageList = new ImageList(components);
            panel1 = new Panel();
            panel3 = new Panel();
            ZoomComboBox = new ComboBox();
            NowPageTxt = new TextBox();
            TotalPageLabel = new Label();
            treeToolTip = new ToolTip(components);
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            panel2.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            tabPage2.SuspendLayout();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            menuStrip1.Items.AddRange(new ToolStripItem[] { FileToolStripMenuItem, HelpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(7, 2, 0, 2);
            menuStrip1.Size = new Size(964, 29);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            menuStrip1.MouseEnter += menuStrip1_MouseEnter;
            menuStrip1.MouseLeave += menuStrip1_MouseLeave;
            // 
            // FileToolStripMenuItem
            // 
            FileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { OpenToolStripMenuItem, AcrobatOpenToolStripMenuItem, toolStripMenuItem9, SaveToolStripMenuItem, SaveAsToolStripMenuItem, toolStripMenuItem2, PdfSetToolStripMenuItem, SecurityToolStripMenuItem, toolStripMenuItem3, XToolStripMenuItem });
            FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            FileToolStripMenuItem.Size = new Size(85, 25);
            FileToolStripMenuItem.Text = "ファイル(&F)";
            // 
            // OpenToolStripMenuItem
            // 
            OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
            OpenToolStripMenuItem.Size = new Size(256, 26);
            OpenToolStripMenuItem.Text = "開く(&O)...";
            OpenToolStripMenuItem.ToolTipText = "PDFファイルを開きます";
            OpenToolStripMenuItem.Click += OpenToolStripMenuItem_Click;
            OpenToolStripMenuItem.MouseEnter += menuStrip1_MouseEnter;
            OpenToolStripMenuItem.MouseLeave += menuStrip1_MouseLeave;
            // 
            // AcrobatOpenToolStripMenuItem
            // 
            AcrobatOpenToolStripMenuItem.Enabled = false;
            AcrobatOpenToolStripMenuItem.Name = "AcrobatOpenToolStripMenuItem";
            AcrobatOpenToolStripMenuItem.Size = new Size(256, 26);
            AcrobatOpenToolStripMenuItem.Text = "既定のPDFアプリで開く(&G)...";
            AcrobatOpenToolStripMenuItem.ToolTipText = "開いているPDFファイルを既定のアプリで開きます";
            AcrobatOpenToolStripMenuItem.Click += AcrobatOpenToolStripMenuItem_Click;
            AcrobatOpenToolStripMenuItem.MouseEnter += menuStrip1_MouseEnter;
            AcrobatOpenToolStripMenuItem.MouseLeave += menuStrip1_MouseLeave;
            // 
            // toolStripMenuItem9
            // 
            toolStripMenuItem9.Name = "toolStripMenuItem9";
            toolStripMenuItem9.Size = new Size(253, 6);
            // 
            // SaveToolStripMenuItem
            // 
            SaveToolStripMenuItem.Enabled = false;
            SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            SaveToolStripMenuItem.Size = new Size(256, 26);
            SaveToolStripMenuItem.Text = "上書き保存(&S)";
            SaveToolStripMenuItem.ToolTipText = "開いているPDFを上書き保存します";
            SaveToolStripMenuItem.Click += SaveToolStripMenuItem_Click;
            SaveToolStripMenuItem.MouseEnter += menuStrip1_MouseEnter;
            SaveToolStripMenuItem.MouseLeave += menuStrip1_MouseLeave;
            // 
            // SaveAsToolStripMenuItem
            // 
            SaveAsToolStripMenuItem.Enabled = false;
            SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem";
            SaveAsToolStripMenuItem.Size = new Size(256, 26);
            SaveAsToolStripMenuItem.Text = "名前を付けて保存(&A)...";
            SaveAsToolStripMenuItem.ToolTipText = "開いているPDFに名前を付けて保存します";
            SaveAsToolStripMenuItem.Click += SaveAsToolStripMenuItem_Click;
            SaveAsToolStripMenuItem.MouseEnter += menuStrip1_MouseEnter;
            SaveAsToolStripMenuItem.MouseLeave += menuStrip1_MouseLeave;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(253, 6);
            // 
            // PdfSetToolStripMenuItem
            // 
            PdfSetToolStripMenuItem.Enabled = false;
            PdfSetToolStripMenuItem.Name = "PdfSetToolStripMenuItem";
            PdfSetToolStripMenuItem.Size = new Size(256, 26);
            PdfSetToolStripMenuItem.Text = "PDFのプロパティ(&D)...";
            PdfSetToolStripMenuItem.ToolTipText = "PDFのプロパティを編集します";
            PdfSetToolStripMenuItem.Click += PdfSetToolStripMenuItem_Click;
            PdfSetToolStripMenuItem.MouseEnter += menuStrip1_MouseEnter;
            PdfSetToolStripMenuItem.MouseLeave += menuStrip1_MouseLeave;
            // 
            // SecurityToolStripMenuItem
            // 
            SecurityToolStripMenuItem.Enabled = false;
            SecurityToolStripMenuItem.Name = "SecurityToolStripMenuItem";
            SecurityToolStripMenuItem.Size = new Size(256, 26);
            SecurityToolStripMenuItem.Text = "セキュリティ設定(&T)...";
            SecurityToolStripMenuItem.ToolTipText = "PDFにセキュリティを設定します";
            SecurityToolStripMenuItem.Click += SecurityToolStripMenuItem_Click;
            SecurityToolStripMenuItem.MouseEnter += menuStrip1_MouseEnter;
            SecurityToolStripMenuItem.MouseLeave += menuStrip1_MouseLeave;
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(253, 6);
            // 
            // XToolStripMenuItem
            // 
            XToolStripMenuItem.Name = "XToolStripMenuItem";
            XToolStripMenuItem.Size = new Size(256, 26);
            XToolStripMenuItem.Text = "終了(&X)";
            XToolStripMenuItem.ToolTipText = "PDF編集帖を終了します";
            XToolStripMenuItem.Click += XToolStripMenuItem_Click;
            XToolStripMenuItem.MouseEnter += menuStrip1_MouseEnter;
            XToolStripMenuItem.MouseLeave += menuStrip1_MouseLeave;
            // 
            // HelpToolStripMenuItem
            // 
            HelpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { UseToolStripMenuItem, VerToolStripMenuItem });
            HelpToolStripMenuItem.Name = "HelpToolStripMenuItem";
            HelpToolStripMenuItem.Size = new Size(81, 25);
            HelpToolStripMenuItem.Text = "ヘルプ(&H)";
            // 
            // UseToolStripMenuItem
            // 
            UseToolStripMenuItem.Name = "UseToolStripMenuItem";
            UseToolStripMenuItem.Size = new Size(199, 26);
            UseToolStripMenuItem.Text = "使い方(&U)...";
            UseToolStripMenuItem.ToolTipText = "使い方を表示します";
            UseToolStripMenuItem.Click += UseToolStripMenuItem_Click;
            UseToolStripMenuItem.MouseEnter += menuStrip1_MouseEnter;
            UseToolStripMenuItem.MouseLeave += menuStrip1_MouseLeave;
            // 
            // VerToolStripMenuItem
            // 
            VerToolStripMenuItem.Name = "VerToolStripMenuItem";
            VerToolStripMenuItem.Size = new Size(199, 26);
            VerToolStripMenuItem.Text = "バージョン情報(&A)...";
            VerToolStripMenuItem.ToolTipText = "バージョン情報を表示します";
            VerToolStripMenuItem.Click += VerToolStripMenuItem_Click;
            VerToolStripMenuItem.MouseEnter += menuStrip1_MouseEnter;
            VerToolStripMenuItem.MouseLeave += menuStrip1_MouseLeave;
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
            splitter1.Location = new Point(199, 29);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(8, 359);
            splitter1.TabIndex = 3;
            splitter1.TabStop = false;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.Control;
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(pdfViewer1);
            panel2.Location = new Point(263, 116);
            panel2.Name = "panel2";
            panel2.Size = new Size(293, 199);
            panel2.TabIndex = 4;
            // 
            // pdfViewer1
            // 
            pdfViewer1.Location = new Point(32, 22);
            pdfViewer1.Margin = new Padding(4, 4, 4, 4);
            pdfViewer1.Name = "pdfViewer1";
            pdfViewer1.Size = new Size(150, 141);
            pdfViewer1.TabIndex = 0;
            pdfViewer1.ZoomMode = PdfiumViewer.PdfViewerZoomMode.FitBest;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(24, 79);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(145, 207);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(treeView1);
            tabPage1.Location = new Point(4, 30);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(137, 173);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "しおり";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // treeView1
            // 
            treeView1.AllowDrop = true;
            treeView1.BackColor = SystemColors.Control;
            treeView1.BorderStyle = BorderStyle.FixedSingle;
            treeView1.ContextMenuStrip = contextMenuStrip1;
            treeView1.DrawMode = TreeViewDrawMode.OwnerDrawText;
            treeView1.LabelEdit = true;
            treeView1.Location = new Point(6, 70);
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
            contextMenuStrip1.Size = new Size(303, 348);
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
            // tabPage2
            // 
            tabPage2.Controls.Add(listView1);
            tabPage2.Location = new Point(4, 28);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(137, 175);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "サムネイル";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            listView1.BackColor = SystemColors.Control;
            listView1.BorderStyle = BorderStyle.FixedSingle;
            listView1.LargeImageList = thumbnailImageList;
            listView1.Location = new Point(6, 20);
            listView1.MultiSelect = false;
            listView1.Name = "listView1";
            listView1.OwnerDraw = true;
            listView1.Size = new Size(104, 108);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.DrawItem += listView1_DrawItem;
            listView1.SelectedIndexChanged += listView1_SelectedIndexChanged;
            listView1.Click += listView1_Click;
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
            panel1.Controls.Add(tabControl1);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 29);
            panel1.Name = "panel1";
            panel1.Size = new Size(199, 359);
            panel1.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.Controls.Add(ZoomComboBox);
            panel3.Controls.Add(NowPageTxt);
            panel3.Controls.Add(TotalPageLabel);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(207, 29);
            panel3.Name = "panel3";
            panel3.Size = new Size(757, 36);
            panel3.TabIndex = 5;
            // 
            // ZoomComboBox
            // 
            ZoomComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ZoomComboBox.Enabled = false;
            ZoomComboBox.FormattingEnabled = true;
            ZoomComboBox.Location = new Point(248, 3);
            ZoomComboBox.Name = "ZoomComboBox";
            ZoomComboBox.Size = new Size(169, 29);
            ZoomComboBox.TabIndex = 2;
            ZoomComboBox.SelectedIndexChanged += ZoomComboBox_SelectedIndexChanged;
            // 
            // NowPageTxt
            // 
            NowPageTxt.Enabled = false;
            NowPageTxt.Location = new Point(4, 4);
            NowPageTxt.Name = "NowPageTxt";
            NowPageTxt.Size = new Size(78, 29);
            NowPageTxt.TabIndex = 1;
            NowPageTxt.Text = "1";
            NowPageTxt.TextAlign = HorizontalAlignment.Right;
            NowPageTxt.WordWrap = false;
            NowPageTxt.Enter += NowPageTxt_Enter;
            NowPageTxt.KeyDown += NowPageTxt_KeyDown;
            // 
            // TotalPageLabel
            // 
            TotalPageLabel.AutoSize = true;
            TotalPageLabel.Location = new Point(88, 7);
            TotalPageLabel.Name = "TotalPageLabel";
            TotalPageLabel.Size = new Size(94, 21);
            TotalPageLabel.TabIndex = 0;
            TotalPageLabel.Text = "総ページ番号";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(964, 414);
            Controls.Add(panel2);
            Controls.Add(panel3);
            Controls.Add(splitter1);
            Controls.Add(panel1);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ともさんのPDF編集帖";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            panel2.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem FileToolStripMenuItem;
        private ToolStripMenuItem OpenToolStripMenuItem;
        private StatusStrip statusStrip1;
        private Splitter splitter1;
        private Panel panel2;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Panel panel1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private TreeView treeView1;
        private ToolStripMenuItem HelpToolStripMenuItem;
        private ToolStripMenuItem UseToolStripMenuItem;
        private ToolStripMenuItem VerToolStripMenuItem;
        private PdfiumViewer.PdfViewer pdfViewer1;
        private ToolStripMenuItem SaveToolStripMenuItem;
        private ToolStripMenuItem SaveAsToolStripMenuItem;
        private ToolStripMenuItem XToolStripMenuItem;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem AddShioriToolStripMenuItem;
        private ToolStripMenuItem DelShioriToolStripMenuItem;
        private Panel panel3;
        private Label TotalPageLabel;
        private TextBox NowPageTxt;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem SetShioriToolStripMenuItem;
        private ListView listView1;
        private ImageList thumbnailImageList;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem PdfSetToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripSeparator toolStripMenuItem4;
        private ToolStripMenuItem ShioriTenkaiToolStripMenuItem;
        private ToolStripMenuItem ShioriSyukusyouToolStripMenuItem;
        private ToolStripMenuItem AllShioriTenkaiToolStripMenuItem;
        private ToolStripMenuItem AllShioriSyukusyouToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem5;
        private ComboBox ZoomComboBox;
        private ToolStripMenuItem SecurityToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem6;
        private ToolStripMenuItem ShioriProToolStripMenuItem;
        private ToolTip treeToolTip;
        private ToolStripMenuItem ImportShioriToolStripMenuItem;
        private ToolStripMenuItem ExportShioriToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem7;
        private ToolStripMenuItem AllDelToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem8;
        private ToolStripMenuItem AcrobatOpenToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem9;
    }
}

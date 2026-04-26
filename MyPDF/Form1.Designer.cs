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
            panel2 = new Panel();
            pdfViewer1 = new PdfiumViewer.PdfViewer();
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
            thumbnailImageList = new ImageList(components);
            panel1 = new Panel();
            panel3 = new Panel();
            label1 = new Label();
            treeToolTip = new ToolTip(components);
            toolStrip1 = new ToolStrip();
            FiletoolStripDropDownButton = new ToolStripDropDownButton();
            OpenToolStripMenuItem = new ToolStripMenuItem();
            AcrobatOpenToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem10 = new ToolStripSeparator();
            SaveToolStripMenuItem = new ToolStripMenuItem();
            SaveAsToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem9 = new ToolStripSeparator();
            PdfSetToolStripMenuItem = new ToolStripMenuItem();
            SecurityToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem11 = new ToolStripSeparator();
            XToolStripMenuItem = new ToolStripMenuItem();
            HelptoolStripDropDownButton = new ToolStripDropDownButton();
            UseToolStripMenuItem = new ToolStripMenuItem();
            VerToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            NewPagetoolStripTextBox = new ToolStripTextBox();
            TotalPagetoolStripLabel = new ToolStripLabel();
            toolStripSeparator2 = new ToolStripSeparator();
            ZoomtoolStripComboBox = new ToolStripComboBox();
            toolStripSeparator3 = new ToolStripSeparator();
            PageRoll = new ToolStripDropDownButton();
            Left90Roll = new ToolStripMenuItem();
            Right90Roll = new ToolStripMenuItem();
            Roll180 = new ToolStripMenuItem();
            PageSetRoll = new ToolStripMenuItem();
            PageEdit = new ToolStripDropDownButton();
            PageInsert = new ToolStripMenuItem();
            PageExtract = new ToolStripMenuItem();
            PageDelete = new ToolStripMenuItem();
            panel4 = new Panel();
            label2 = new Label();
            statusStrip1.SuspendLayout();
            panel2.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            toolStrip1.SuspendLayout();
            panel4.SuspendLayout();
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
            splitter1.Location = new Point(199, 29);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(8, 359);
            splitter1.TabIndex = 3;
            splitter1.TabStop = false;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.Control;
            panel2.Controls.Add(pdfViewer1);
            panel2.Location = new Point(263, 116);
            panel2.Name = "panel2";
            panel2.Size = new Size(293, 199);
            panel2.TabIndex = 4;
            // 
            // pdfViewer1
            // 
            pdfViewer1.BorderStyle = BorderStyle.FixedSingle;
            pdfViewer1.Location = new Point(32, 22);
            pdfViewer1.Margin = new Padding(4, 4, 4, 4);
            pdfViewer1.Name = "pdfViewer1";
            pdfViewer1.Size = new Size(150, 141);
            pdfViewer1.TabIndex = 0;
            pdfViewer1.ZoomMode = PdfiumViewer.PdfViewerZoomMode.FitBest;
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
            panel1.Location = new Point(0, 29);
            panel1.Name = "panel1";
            panel1.Size = new Size(199, 359);
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
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { FiletoolStripDropDownButton, HelptoolStripDropDownButton, toolStripSeparator1, NewPagetoolStripTextBox, TotalPagetoolStripLabel, toolStripSeparator2, ZoomtoolStripComboBox, toolStripSeparator3, PageRoll, PageEdit });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(964, 29);
            toolStrip1.TabIndex = 6;
            toolStrip1.Text = "toolStrip1";
            // 
            // FiletoolStripDropDownButton
            // 
            FiletoolStripDropDownButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            FiletoolStripDropDownButton.DropDownItems.AddRange(new ToolStripItem[] { OpenToolStripMenuItem, AcrobatOpenToolStripMenuItem, toolStripMenuItem10, SaveToolStripMenuItem, SaveAsToolStripMenuItem, toolStripMenuItem9, PdfSetToolStripMenuItem, SecurityToolStripMenuItem, toolStripMenuItem11, XToolStripMenuItem });
            FiletoolStripDropDownButton.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            FiletoolStripDropDownButton.ImageTransparentColor = Color.Magenta;
            FiletoolStripDropDownButton.Name = "FiletoolStripDropDownButton";
            FiletoolStripDropDownButton.Size = new Size(86, 26);
            FiletoolStripDropDownButton.Text = "ファイル(&F)";
            FiletoolStripDropDownButton.ToolTipText = "ファイルメニュー";
            FiletoolStripDropDownButton.MouseEnter += menuStrip1_MouseEnter;
            FiletoolStripDropDownButton.MouseLeave += menuStrip1_MouseLeave;
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
            // toolStripMenuItem10
            // 
            toolStripMenuItem10.Name = "toolStripMenuItem10";
            toolStripMenuItem10.Size = new Size(253, 6);
            // 
            // SaveToolStripMenuItem
            // 
            SaveToolStripMenuItem.Enabled = false;
            SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            SaveToolStripMenuItem.Size = new Size(256, 26);
            SaveToolStripMenuItem.Text = "上書き保存(&S)";
            SaveToolStripMenuItem.ToolTipText = "開いているPDFファイルを上書き保存します";
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
            SaveAsToolStripMenuItem.ToolTipText = "開いているPDFファイルに名前を付けて保存します";
            SaveAsToolStripMenuItem.Click += SaveAsToolStripMenuItem_Click;
            SaveAsToolStripMenuItem.MouseEnter += menuStrip1_MouseEnter;
            SaveAsToolStripMenuItem.MouseLeave += menuStrip1_MouseLeave;
            // 
            // toolStripMenuItem9
            // 
            toolStripMenuItem9.Name = "toolStripMenuItem9";
            toolStripMenuItem9.Size = new Size(253, 6);
            // 
            // PdfSetToolStripMenuItem
            // 
            PdfSetToolStripMenuItem.Enabled = false;
            PdfSetToolStripMenuItem.Name = "PdfSetToolStripMenuItem";
            PdfSetToolStripMenuItem.Size = new Size(256, 26);
            PdfSetToolStripMenuItem.Text = "PDFのプロパティ(&D)...";
            PdfSetToolStripMenuItem.ToolTipText = "PDFファイルのプロパティを編集します";
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
            SecurityToolStripMenuItem.ToolTipText = "PDFファイルにセキュリティを設定します";
            SecurityToolStripMenuItem.Click += SecurityToolStripMenuItem_Click;
            SecurityToolStripMenuItem.MouseEnter += menuStrip1_MouseEnter;
            SecurityToolStripMenuItem.MouseLeave += menuStrip1_MouseLeave;
            // 
            // toolStripMenuItem11
            // 
            toolStripMenuItem11.Name = "toolStripMenuItem11";
            toolStripMenuItem11.Size = new Size(253, 6);
            // 
            // XToolStripMenuItem
            // 
            XToolStripMenuItem.Name = "XToolStripMenuItem";
            XToolStripMenuItem.Size = new Size(256, 26);
            XToolStripMenuItem.Text = "終了(&X)";
            XToolStripMenuItem.ToolTipText = "ともさんのPDF編集帖を終了します";
            XToolStripMenuItem.Click += XToolStripMenuItem_Click;
            XToolStripMenuItem.MouseEnter += menuStrip1_MouseEnter;
            XToolStripMenuItem.MouseLeave += menuStrip1_MouseLeave;
            // 
            // HelptoolStripDropDownButton
            // 
            HelptoolStripDropDownButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            HelptoolStripDropDownButton.DropDownItems.AddRange(new ToolStripItem[] { UseToolStripMenuItem, VerToolStripMenuItem });
            HelptoolStripDropDownButton.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            HelptoolStripDropDownButton.ImageTransparentColor = Color.Magenta;
            HelptoolStripDropDownButton.Name = "HelptoolStripDropDownButton";
            HelptoolStripDropDownButton.Size = new Size(82, 26);
            HelptoolStripDropDownButton.Text = "ヘルプ(&H)";
            HelptoolStripDropDownButton.ToolTipText = "ヘルプメニュー";
            HelptoolStripDropDownButton.MouseEnter += menuStrip1_MouseEnter;
            HelptoolStripDropDownButton.MouseLeave += menuStrip1_MouseLeave;
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
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 29);
            // 
            // NewPagetoolStripTextBox
            // 
            NewPagetoolStripTextBox.Enabled = false;
            NewPagetoolStripTextBox.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            NewPagetoolStripTextBox.Name = "NewPagetoolStripTextBox";
            NewPagetoolStripTextBox.Size = new Size(100, 29);
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
            TotalPagetoolStripLabel.Size = new Size(88, 26);
            TotalPagetoolStripLabel.Text = "/ 総ページ数";
            TotalPagetoolStripLabel.ToolTipText = "開いているPDFファイルの総ページ数です";
            TotalPagetoolStripLabel.MouseEnter += menuStrip1_MouseEnter;
            TotalPagetoolStripLabel.MouseLeave += menuStrip1_MouseLeave;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(6, 29);
            // 
            // ZoomtoolStripComboBox
            // 
            ZoomtoolStripComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ZoomtoolStripComboBox.Enabled = false;
            ZoomtoolStripComboBox.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            ZoomtoolStripComboBox.Name = "ZoomtoolStripComboBox";
            ZoomtoolStripComboBox.Size = new Size(150, 29);
            ZoomtoolStripComboBox.ToolTipText = "開いているPDFファイルの表示方法を選択します";
            ZoomtoolStripComboBox.SelectedIndexChanged += ZoomtoolStripComboBox_SelectedIndexChanged;
            ZoomtoolStripComboBox.MouseEnter += menuStrip1_MouseEnter;
            ZoomtoolStripComboBox.MouseLeave += menuStrip1_MouseLeave;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 29);
            // 
            // PageRoll
            // 
            PageRoll.DisplayStyle = ToolStripItemDisplayStyle.Text;
            PageRoll.DropDownItems.AddRange(new ToolStripItem[] { Left90Roll, Right90Roll, Roll180, PageSetRoll });
            PageRoll.Enabled = false;
            PageRoll.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            PageRoll.Image = (Image)resources.GetObject("PageRoll.Image");
            PageRoll.ImageTransparentColor = Color.Magenta;
            PageRoll.Name = "PageRoll";
            PageRoll.Size = new Size(91, 26);
            PageRoll.Text = "ページ回転";
            PageRoll.ToolTipText = "ページ回転方法を指定します";
            PageRoll.MouseEnter += menuStrip1_MouseEnter;
            PageRoll.MouseLeave += menuStrip1_MouseLeave;
            // 
            // Left90Roll
            // 
            Left90Roll.Name = "Left90Roll";
            Left90Roll.Size = new Size(244, 26);
            Left90Roll.Text = "左へ90°回転";
            Left90Roll.ToolTipText = "表示しているページを左へ90°回転します";
            Left90Roll.Click += Left90Roll_Click;
            Left90Roll.MouseEnter += menuStrip1_MouseEnter;
            Left90Roll.MouseLeave += menuStrip1_MouseLeave;
            // 
            // Right90Roll
            // 
            Right90Roll.Name = "Right90Roll";
            Right90Roll.Size = new Size(244, 26);
            Right90Roll.Text = "右へ90°回転";
            Right90Roll.ToolTipText = "表示しているページを右へ90°回転します";
            Right90Roll.Click += Right90Roll_Click;
            Right90Roll.MouseEnter += menuStrip1_MouseEnter;
            Right90Roll.MouseLeave += menuStrip1_MouseLeave;
            // 
            // Roll180
            // 
            Roll180.Name = "Roll180";
            Roll180.Size = new Size(244, 26);
            Roll180.Text = "180°回転";
            Roll180.ToolTipText = "表示しているページを180°回転します";
            Roll180.Click += Roll180_Click;
            Roll180.MouseEnter += menuStrip1_MouseEnter;
            Roll180.MouseLeave += menuStrip1_MouseLeave;
            // 
            // PageSetRoll
            // 
            PageSetRoll.Name = "PageSetRoll";
            PageSetRoll.Size = new Size(244, 26);
            PageSetRoll.Text = "ページを指定して回転(&R)...";
            PageSetRoll.ToolTipText = "ページと回転方法を指定してページを回転します";
            PageSetRoll.Click += PageSetRoll_Click;
            PageSetRoll.MouseEnter += menuStrip1_MouseEnter;
            PageSetRoll.MouseLeave += menuStrip1_MouseLeave;
            // 
            // PageEdit
            // 
            PageEdit.DisplayStyle = ToolStripItemDisplayStyle.Text;
            PageEdit.DropDownItems.AddRange(new ToolStripItem[] { PageInsert, PageExtract, PageDelete });
            PageEdit.Enabled = false;
            PageEdit.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            PageEdit.Image = (Image)resources.GetObject("PageEdit.Image");
            PageEdit.ImageTransparentColor = Color.Magenta;
            PageEdit.Name = "PageEdit";
            PageEdit.Size = new Size(91, 26);
            PageEdit.Text = "ページ編集";
            PageEdit.MouseEnter += menuStrip1_MouseEnter;
            PageEdit.MouseLeave += menuStrip1_MouseLeave;
            // 
            // PageInsert
            // 
            PageInsert.Name = "PageInsert";
            PageInsert.Size = new Size(112, 26);
            PageInsert.Text = "挿入";
            PageInsert.MouseEnter += menuStrip1_MouseEnter;
            PageInsert.MouseLeave += menuStrip1_MouseLeave;
            // 
            // PageExtract
            // 
            PageExtract.Name = "PageExtract";
            PageExtract.Size = new Size(112, 26);
            PageExtract.Text = "抽出";
            PageExtract.MouseEnter += menuStrip1_MouseEnter;
            PageExtract.MouseLeave += menuStrip1_MouseLeave;
            // 
            // PageDelete
            // 
            PageDelete.Name = "PageDelete";
            PageDelete.Size = new Size(112, 26);
            PageDelete.Text = "削除";
            PageDelete.MouseEnter += menuStrip1_MouseEnter;
            PageDelete.MouseLeave += menuStrip1_MouseLeave;
            // 
            // panel4
            // 
            panel4.BorderStyle = BorderStyle.FixedSingle;
            panel4.Controls.Add(label2);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(207, 29);
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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(964, 414);
            Controls.Add(panel2);
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
            panel2.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private StatusStrip statusStrip1;
        private Splitter splitter1;
        private Panel panel2;
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
        private ToolStrip toolStrip1;
        private ToolStripDropDownButton FiletoolStripDropDownButton;
        private ToolStripMenuItem OpenToolStripMenuItem;
        private ToolStripDropDownButton HelptoolStripDropDownButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripTextBox NewPagetoolStripTextBox;
        private ToolStripLabel TotalPagetoolStripLabel;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripComboBox ZoomtoolStripComboBox;
        private Panel panel3;
        private Label label1;
        private Panel panel4;
        private Label label2;
        private ToolStripMenuItem AcrobatOpenToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem10;
        private ToolStripMenuItem SaveToolStripMenuItem;
        private ToolStripMenuItem SaveAsToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem9;
        private ToolStripMenuItem PdfSetToolStripMenuItem;
        private ToolStripMenuItem SecurityToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem11;
        private ToolStripMenuItem XToolStripMenuItem;
        private ToolStripMenuItem UseToolStripMenuItem;
        private ToolStripMenuItem VerToolStripMenuItem;
        private ToolStripDropDownButton PageRoll;
        private ToolStripMenuItem Left90Roll;
        private ToolStripMenuItem Right90Roll;
        private ToolStripMenuItem Roll180;
        private ToolStripDropDownButton PageEdit;
        private ToolStripMenuItem PageInsert;
        private ToolStripMenuItem PageExtract;
        private ToolStripMenuItem PageDelete;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem PageSetRoll;
    }
}

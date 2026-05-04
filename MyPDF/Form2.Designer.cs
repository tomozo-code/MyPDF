namespace MyPDF
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            panel1 = new Panel();
            CancelBtn = new Button();
            OkBtn = new Button();
            label10 = new Label();
            label6 = new Label();
            label9 = new Label();
            label8 = new Label();
            label7 = new Label();
            TotalPageLabel1 = new Label();
            PageTxt = new TextBox();
            MagComboBox = new ComboBox();
            PageLayoutComboBox = new ComboBox();
            ViewComboBox = new ComboBox();
            label15 = new Label();
            label14 = new Label();
            label13 = new Label();
            label12 = new Label();
            label18 = new Label();
            label17 = new Label();
            label16 = new Label();
            KeywordTxt = new TextBox();
            label1 = new Label();
            TitleTxt = new TextBox();
            SubTitleTxt = new TextBox();
            label5 = new Label();
            label4 = new Label();
            AuthorTxt = new TextBox();
            label3 = new Label();
            label2 = new Label();
            PageSizeLabel = new TextBox();
            FileSizeLabel = new TextBox();
            PasLabel = new TextBox();
            PdfVerLabel = new TextBox();
            CreatorLabel = new TextBox();
            ModDateLabel = new TextBox();
            CreationDateLabel = new TextBox();
            PdfConvertLabel = new TextBox();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            toolTip1 = new ToolTip(components);
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            FileNamelabel2 = new TextBox();
            label11 = new Label();
            tabPage2 = new TabPage();
            panel1.SuspendLayout();
            statusStrip1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(CancelBtn);
            panel1.Controls.Add(OkBtn);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 465);
            panel1.Name = "panel1";
            panel1.Size = new Size(584, 50);
            panel1.TabIndex = 1;
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(118, 6);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(100, 32);
            CancelBtn.TabIndex = 10;
            CancelBtn.Tag = "編集を中止してウィンドウを閉じます";
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // OkBtn
            // 
            OkBtn.Location = new Point(12, 6);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(100, 32);
            OkBtn.TabIndex = 9;
            OkBtn.Tag = "PDFのプロパティを確定しウィンドウを閉じます";
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(35, 232);
            label10.Name = "label10";
            label10.Size = new Size(85, 21);
            label10.TabIndex = 58;
            label10.Text = "ページサイズ:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(47, 15);
            label6.Name = "label6";
            label6.Size = new Size(73, 21);
            label6.TabIndex = 59;
            label6.Text = "PDF変換:";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(26, 201);
            label9.Name = "label9";
            label9.Size = new Size(94, 21);
            label9.TabIndex = 60;
            label9.Text = "ファイルサイズ:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(75, 170);
            label8.Name = "label8";
            label8.Size = new Size(45, 21);
            label8.TabIndex = 62;
            label8.Text = "場所:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(8, 139);
            label7.Name = "label7";
            label7.Size = new Size(112, 21);
            label7.TabIndex = 64;
            label7.Text = "PDFのバージョン:";
            // 
            // TotalPageLabel1
            // 
            TotalPageLabel1.AutoSize = true;
            TotalPageLabel1.Location = new Point(230, 352);
            TotalPageLabel1.Name = "TotalPageLabel1";
            TotalPageLabel1.Size = new Size(132, 21);
            TotalPageLabel1.TabIndex = 56;
            TotalPageLabel1.Text = "/ 総ページ数を表示";
            // 
            // PageTxt
            // 
            PageTxt.Location = new Point(124, 349);
            PageTxt.Name = "PageTxt";
            PageTxt.Size = new Size(100, 29);
            PageTxt.TabIndex = 8;
            PageTxt.Tag = "PDFファイルを開いたときの初期ページを設定します";
            PageTxt.TextAlign = HorizontalAlignment.Right;
            PageTxt.WordWrap = false;
            // 
            // MagComboBox
            // 
            MagComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MagComboBox.FormattingEnabled = true;
            MagComboBox.Location = new Point(124, 314);
            MagComboBox.Name = "MagComboBox";
            MagComboBox.Size = new Size(300, 29);
            MagComboBox.TabIndex = 7;
            MagComboBox.Tag = "PDFファイルを開いたときの倍率を指定します";
            // 
            // PageLayoutComboBox
            // 
            PageLayoutComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            PageLayoutComboBox.FormattingEnabled = true;
            PageLayoutComboBox.Location = new Point(124, 279);
            PageLayoutComboBox.Name = "PageLayoutComboBox";
            PageLayoutComboBox.Size = new Size(300, 29);
            PageLayoutComboBox.TabIndex = 6;
            PageLayoutComboBox.Tag = "PDFファイルを開いたときのページレイアウトを指定します";
            // 
            // ViewComboBox
            // 
            ViewComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ViewComboBox.FormattingEnabled = true;
            ViewComboBox.Location = new Point(124, 244);
            ViewComboBox.Name = "ViewComboBox";
            ViewComboBox.Size = new Size(300, 29);
            ViewComboBox.TabIndex = 5;
            ViewComboBox.Tag = "PDFファイルを開いたときの表示方法を指定します";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(43, 352);
            label15.Name = "label15";
            label15.Size = new Size(75, 21);
            label15.TabIndex = 48;
            label15.Text = "開くページ:";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(73, 317);
            label14.Name = "label14";
            label14.Size = new Size(45, 21);
            label14.TabIndex = 49;
            label14.Text = "倍率:";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(12, 282);
            label13.Name = "label13";
            label13.Size = new Size(106, 21);
            label13.TabIndex = 50;
            label13.Text = "ページレイアウト:";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(73, 247);
            label12.Name = "label12";
            label12.Size = new Size(45, 21);
            label12.TabIndex = 51;
            label12.Text = "表示:";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(16, 108);
            label18.Name = "label18";
            label18.Size = new Size(104, 21);
            label18.TabIndex = 44;
            label18.Text = "アプリケーション:";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(59, 77);
            label17.Name = "label17";
            label17.Size = new Size(61, 21);
            label17.TabIndex = 43;
            label17.Text = "更新日:";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(59, 46);
            label16.Name = "label16";
            label16.Size = new Size(61, 21);
            label16.TabIndex = 42;
            label16.Text = "作成日:";
            // 
            // KeywordTxt
            // 
            KeywordTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            KeywordTxt.Location = new Point(124, 152);
            KeywordTxt.Multiline = true;
            KeywordTxt.Name = "KeywordTxt";
            KeywordTxt.ScrollBars = ScrollBars.Both;
            KeywordTxt.Size = new Size(440, 80);
            KeywordTxt.TabIndex = 4;
            KeywordTxt.Tag = "キーワードを設定します";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(44, 16);
            label1.Name = "label1";
            label1.Size = new Size(74, 21);
            label1.TabIndex = 36;
            label1.Text = "ファイル名:";
            // 
            // TitleTxt
            // 
            TitleTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            TitleTxt.Location = new Point(124, 46);
            TitleTxt.Name = "TitleTxt";
            TitleTxt.Size = new Size(440, 29);
            TitleTxt.TabIndex = 1;
            TitleTxt.Tag = "タイトルを設定します";
            TitleTxt.WordWrap = false;
            // 
            // SubTitleTxt
            // 
            SubTitleTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            SubTitleTxt.Location = new Point(124, 117);
            SubTitleTxt.Name = "SubTitleTxt";
            SubTitleTxt.Size = new Size(440, 29);
            SubTitleTxt.TabIndex = 3;
            SubTitleTxt.Tag = "サブタイトルを設定します";
            SubTitleTxt.WordWrap = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(50, 152);
            label5.Name = "label5";
            label5.Size = new Size(68, 21);
            label5.TabIndex = 35;
            label5.Text = "キーワード:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(34, 117);
            label4.Name = "label4";
            label4.Size = new Size(84, 21);
            label4.TabIndex = 33;
            label4.Text = "サブタイトル:";
            // 
            // AuthorTxt
            // 
            AuthorTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            AuthorTxt.Location = new Point(124, 82);
            AuthorTxt.Name = "AuthorTxt";
            AuthorTxt.Size = new Size(440, 29);
            AuthorTxt.TabIndex = 2;
            AuthorTxt.Tag = "作成者を設定します";
            AuthorTxt.WordWrap = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(57, 82);
            label3.Name = "label3";
            label3.Size = new Size(61, 21);
            label3.TabIndex = 37;
            label3.Text = "作成者:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(58, 48);
            label2.Name = "label2";
            label2.Size = new Size(60, 21);
            label2.TabIndex = 32;
            label2.Text = "タイトル:";
            // 
            // PageSizeLabel
            // 
            PageSizeLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PageSizeLabel.Location = new Point(126, 233);
            PageSizeLabel.Name = "PageSizeLabel";
            PageSizeLabel.Size = new Size(440, 29);
            PageSizeLabel.TabIndex = 76;
            PageSizeLabel.Text = "ページサイズを表示(B x H mm)";
            // 
            // FileSizeLabel
            // 
            FileSizeLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            FileSizeLabel.Location = new Point(126, 202);
            FileSizeLabel.Name = "FileSizeLabel";
            FileSizeLabel.Size = new Size(440, 29);
            FileSizeLabel.TabIndex = 75;
            FileSizeLabel.Text = "ファイルサイズを表示(MB / バイト)";
            // 
            // PasLabel
            // 
            PasLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PasLabel.Location = new Point(126, 171);
            PasLabel.Name = "PasLabel";
            PasLabel.Size = new Size(440, 29);
            PasLabel.TabIndex = 74;
            PasLabel.Text = "場所を表示";
            // 
            // PdfVerLabel
            // 
            PdfVerLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PdfVerLabel.Location = new Point(126, 140);
            PdfVerLabel.Name = "PdfVerLabel";
            PdfVerLabel.Size = new Size(440, 29);
            PdfVerLabel.TabIndex = 73;
            PdfVerLabel.Text = "PDFのバージョンを表示";
            // 
            // CreatorLabel
            // 
            CreatorLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            CreatorLabel.Location = new Point(126, 109);
            CreatorLabel.Name = "CreatorLabel";
            CreatorLabel.Size = new Size(440, 29);
            CreatorLabel.TabIndex = 72;
            CreatorLabel.Text = "アプリケーションを表示";
            // 
            // ModDateLabel
            // 
            ModDateLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ModDateLabel.Location = new Point(126, 78);
            ModDateLabel.Name = "ModDateLabel";
            ModDateLabel.Size = new Size(440, 29);
            ModDateLabel.TabIndex = 71;
            ModDateLabel.Text = "更新日";
            // 
            // CreationDateLabel
            // 
            CreationDateLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            CreationDateLabel.Location = new Point(126, 47);
            CreationDateLabel.Name = "CreationDateLabel";
            CreationDateLabel.Size = new Size(440, 29);
            CreationDateLabel.TabIndex = 70;
            CreationDateLabel.Text = "作成日";
            // 
            // PdfConvertLabel
            // 
            PdfConvertLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PdfConvertLabel.Location = new Point(126, 16);
            PdfConvertLabel.Name = "PdfConvertLabel";
            PdfConvertLabel.Size = new Size(440, 29);
            PdfConvertLabel.TabIndex = 69;
            PdfConvertLabel.Text = "PDF変換";
            // 
            // statusStrip1
            // 
            statusStrip1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 515);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(584, 26);
            statusStrip1.TabIndex = 70;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(188, 21);
            toolStripStatusLabel1.Text = "PDFのプロパティを設定します";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(584, 465);
            tabControl1.TabIndex = 71;
            // 
            // tabPage1
            // 
            tabPage1.AutoScroll = true;
            tabPage1.BackColor = SystemColors.Control;
            tabPage1.BorderStyle = BorderStyle.FixedSingle;
            tabPage1.Controls.Add(FileNamelabel2);
            tabPage1.Controls.Add(label11);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(SubTitleTxt);
            tabPage1.Controls.Add(label5);
            tabPage1.Controls.Add(TotalPageLabel1);
            tabPage1.Controls.Add(TitleTxt);
            tabPage1.Controls.Add(PageTxt);
            tabPage1.Controls.Add(KeywordTxt);
            tabPage1.Controls.Add(MagComboBox);
            tabPage1.Controls.Add(label4);
            tabPage1.Controls.Add(PageLayoutComboBox);
            tabPage1.Controls.Add(AuthorTxt);
            tabPage1.Controls.Add(label12);
            tabPage1.Controls.Add(ViewComboBox);
            tabPage1.Controls.Add(label3);
            tabPage1.Controls.Add(label13);
            tabPage1.Controls.Add(label15);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(label14);
            tabPage1.Location = new Point(4, 30);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(576, 431);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "概要";
            // 
            // FileNamelabel2
            // 
            FileNamelabel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            FileNamelabel2.Location = new Point(124, 15);
            FileNamelabel2.Name = "FileNamelabel2";
            FileNamelabel2.Size = new Size(440, 29);
            FileNamelabel2.TabIndex = 58;
            FileNamelabel2.Text = "ファイル名を表示";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(12, 393);
            label11.Name = "label11";
            label11.Size = new Size(384, 21);
            label11.TabIndex = 57;
            label11.Text = "※上書き保存もしくは名前を付けて保存すると適用されます";
            // 
            // tabPage2
            // 
            tabPage2.BackColor = SystemColors.Control;
            tabPage2.BorderStyle = BorderStyle.FixedSingle;
            tabPage2.Controls.Add(PageSizeLabel);
            tabPage2.Controls.Add(label6);
            tabPage2.Controls.Add(FileSizeLabel);
            tabPage2.Controls.Add(label16);
            tabPage2.Controls.Add(PasLabel);
            tabPage2.Controls.Add(label7);
            tabPage2.Controls.Add(PdfVerLabel);
            tabPage2.Controls.Add(label17);
            tabPage2.Controls.Add(CreatorLabel);
            tabPage2.Controls.Add(label18);
            tabPage2.Controls.Add(ModDateLabel);
            tabPage2.Controls.Add(label8);
            tabPage2.Controls.Add(CreationDateLabel);
            tabPage2.Controls.Add(label9);
            tabPage2.Controls.Add(PdfConvertLabel);
            tabPage2.Controls.Add(label10);
            tabPage2.Location = new Point(4, 28);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(576, 433);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "詳細情報";
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(584, 541);
            Controls.Add(tabControl1);
            Controls.Add(panel1);
            Controls.Add(statusStrip1);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form2";
            SizeGripStyle = SizeGripStyle.Show;
            StartPosition = FormStartPosition.CenterParent;
            Text = "PDFのプロパティ";
            Load += Form2_Load;
            panel1.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel panel1;
        private Button OkBtn;
        private Button CancelBtn;
        private Label label10;
        private Label label6;
        private Label label9;
        private Label label8;
        private Label label7;
        private Label TotalPageLabel1;
        private TextBox PageTxt;
        private ComboBox MagComboBox;
        private ComboBox PageLayoutComboBox;
        private ComboBox ViewComboBox;
        private Label label15;
        private Label label14;
        private Label label13;
        private Label label12;
        private Label label18;
        private Label label17;
        private Label label16;
        private TextBox KeywordTxt;
        private Label label1;
        private TextBox TitleTxt;
        private TextBox SubTitleTxt;
        private Label label5;
        private Label label4;
        private TextBox AuthorTxt;
        private Label label3;
        private Label label2;
        private TextBox PdfConvertLabel;
        private TextBox CreationDateLabel;
        private TextBox ModDateLabel;
        private TextBox CreatorLabel;
        private TextBox PdfVerLabel;
        private TextBox PasLabel;
        private TextBox FileSizeLabel;
        private TextBox PageSizeLabel;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolTip toolTip1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Label label11;
        private TextBox FileNamelabel2;
    }
}
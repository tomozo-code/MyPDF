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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            panel1 = new Panel();
            CancelBtn = new Button();
            SaveBtn = new Button();
            panel2 = new Panel();
            label10 = new Label();
            label6 = new Label();
            label9 = new Label();
            PdfConvertLabel = new Label();
            label8 = new Label();
            PdfVerLabel = new Label();
            label7 = new Label();
            PasLabel = new Label();
            FileSizeLabel = new Label();
            PageSizeLabel = new Label();
            TotalPageLabel1 = new Label();
            PageTxt = new TextBox();
            MagComboBox = new ComboBox();
            PageLayoutComboBox = new ComboBox();
            ViewComboBox = new ComboBox();
            label15 = new Label();
            label14 = new Label();
            label13 = new Label();
            label12 = new Label();
            CreatorLabel = new Label();
            ModDateLabel = new Label();
            CreationDateLabel = new Label();
            label18 = new Label();
            label17 = new Label();
            label16 = new Label();
            KeywordTxt = new TextBox();
            label1 = new Label();
            TitleTxt = new TextBox();
            SubTitleTxt = new TextBox();
            label5 = new Label();
            FileNamelabel = new Label();
            label4 = new Label();
            AuthorTxt = new TextBox();
            label3 = new Label();
            label2 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(CancelBtn);
            panel1.Controls.Add(SaveBtn);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 692);
            panel1.Name = "panel1";
            panel1.Size = new Size(989, 41);
            panel1.TabIndex = 1;
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(118, 6);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(100, 32);
            CancelBtn.TabIndex = 9;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // SaveBtn
            // 
            SaveBtn.Location = new Point(12, 6);
            SaveBtn.Name = "SaveBtn";
            SaveBtn.Size = new Size(100, 32);
            SaveBtn.TabIndex = 10;
            SaveBtn.Text = "OK";
            SaveBtn.UseVisualStyleBackColor = true;
            SaveBtn.Click += SaveBtn_Click;
            // 
            // panel2
            // 
            panel2.AutoScroll = true;
            panel2.Controls.Add(label10);
            panel2.Controls.Add(label6);
            panel2.Controls.Add(label9);
            panel2.Controls.Add(PdfConvertLabel);
            panel2.Controls.Add(label8);
            panel2.Controls.Add(PdfVerLabel);
            panel2.Controls.Add(label7);
            panel2.Controls.Add(PasLabel);
            panel2.Controls.Add(FileSizeLabel);
            panel2.Controls.Add(PageSizeLabel);
            panel2.Controls.Add(TotalPageLabel1);
            panel2.Controls.Add(PageTxt);
            panel2.Controls.Add(MagComboBox);
            panel2.Controls.Add(PageLayoutComboBox);
            panel2.Controls.Add(ViewComboBox);
            panel2.Controls.Add(label15);
            panel2.Controls.Add(label14);
            panel2.Controls.Add(label13);
            panel2.Controls.Add(label12);
            panel2.Controls.Add(CreatorLabel);
            panel2.Controls.Add(ModDateLabel);
            panel2.Controls.Add(CreationDateLabel);
            panel2.Controls.Add(label18);
            panel2.Controls.Add(label17);
            panel2.Controls.Add(label16);
            panel2.Controls.Add(KeywordTxt);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(TitleTxt);
            panel2.Controls.Add(SubTitleTxt);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(FileNamelabel);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(AuthorTxt);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(label2);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(989, 692);
            panel2.TabIndex = 3;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(46, 620);
            label10.Name = "label10";
            label10.Size = new Size(85, 21);
            label10.TabIndex = 58;
            label10.Text = "ページサイズ:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(58, 403);
            label6.Name = "label6";
            label6.Size = new Size(73, 21);
            label6.TabIndex = 59;
            label6.Text = "PDF変換:";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(37, 587);
            label9.Name = "label9";
            label9.Size = new Size(94, 21);
            label9.TabIndex = 60;
            label9.Text = "ファイルサイズ:";
            // 
            // PdfConvertLabel
            // 
            PdfConvertLabel.AutoSize = true;
            PdfConvertLabel.Location = new Point(137, 403);
            PdfConvertLabel.Name = "PdfConvertLabel";
            PdfConvertLabel.Size = new Size(114, 21);
            PdfConvertLabel.TabIndex = 61;
            PdfConvertLabel.Text = "PDF変換を表示";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(86, 556);
            label8.Name = "label8";
            label8.Size = new Size(45, 21);
            label8.TabIndex = 62;
            label8.Text = "場所:";
            // 
            // PdfVerLabel
            // 
            PdfVerLabel.AutoSize = true;
            PdfVerLabel.Location = new Point(137, 526);
            PdfVerLabel.Name = "PdfVerLabel";
            PdfVerLabel.Size = new Size(153, 21);
            PdfVerLabel.TabIndex = 63;
            PdfVerLabel.Text = "PDFのバージョンを表示";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(19, 526);
            label7.Name = "label7";
            label7.Size = new Size(112, 21);
            label7.TabIndex = 64;
            label7.Text = "PDFのバージョン:";
            // 
            // PasLabel
            // 
            PasLabel.AutoSize = true;
            PasLabel.Location = new Point(137, 556);
            PasLabel.Name = "PasLabel";
            PasLabel.Size = new Size(86, 21);
            PasLabel.TabIndex = 65;
            PasLabel.Text = "場所を表示";
            // 
            // FileSizeLabel
            // 
            FileSizeLabel.AutoSize = true;
            FileSizeLabel.Location = new Point(137, 587);
            FileSizeLabel.Name = "FileSizeLabel";
            FileSizeLabel.Size = new Size(217, 21);
            FileSizeLabel.TabIndex = 67;
            FileSizeLabel.Text = "ファイルサイズを表示(MB / バイト)";
            // 
            // PageSizeLabel
            // 
            PageSizeLabel.AutoSize = true;
            PageSizeLabel.Location = new Point(137, 620);
            PageSizeLabel.Name = "PageSizeLabel";
            PageSizeLabel.Size = new Size(203, 21);
            PageSizeLabel.TabIndex = 68;
            PageSizeLabel.Text = "ページサイズを表示(B x H mm)";
            // 
            // TotalPageLabel1
            // 
            TotalPageLabel1.AutoSize = true;
            TotalPageLabel1.Location = new Point(253, 359);
            TotalPageLabel1.Name = "TotalPageLabel1";
            TotalPageLabel1.Size = new Size(132, 21);
            TotalPageLabel1.TabIndex = 56;
            TotalPageLabel1.Text = "/ 総ページ数を表示";
            // 
            // PageTxt
            // 
            PageTxt.Location = new Point(137, 356);
            PageTxt.Name = "PageTxt";
            PageTxt.Size = new Size(100, 29);
            PageTxt.TabIndex = 8;
            PageTxt.TextAlign = HorizontalAlignment.Right;
            PageTxt.WordWrap = false;
            // 
            // MagComboBox
            // 
            MagComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MagComboBox.FormattingEnabled = true;
            MagComboBox.Location = new Point(137, 321);
            MagComboBox.Name = "MagComboBox";
            MagComboBox.Size = new Size(300, 29);
            MagComboBox.TabIndex = 7;
            // 
            // PageLayoutComboBox
            // 
            PageLayoutComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            PageLayoutComboBox.FormattingEnabled = true;
            PageLayoutComboBox.Location = new Point(137, 286);
            PageLayoutComboBox.Name = "PageLayoutComboBox";
            PageLayoutComboBox.Size = new Size(300, 29);
            PageLayoutComboBox.TabIndex = 6;
            // 
            // ViewComboBox
            // 
            ViewComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ViewComboBox.FormattingEnabled = true;
            ViewComboBox.Location = new Point(137, 251);
            ViewComboBox.Name = "ViewComboBox";
            ViewComboBox.Size = new Size(300, 29);
            ViewComboBox.TabIndex = 5;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(56, 359);
            label15.Name = "label15";
            label15.Size = new Size(75, 21);
            label15.TabIndex = 48;
            label15.Text = "開くページ:";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(86, 324);
            label14.Name = "label14";
            label14.Size = new Size(45, 21);
            label14.TabIndex = 49;
            label14.Text = "倍率:";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(25, 289);
            label13.Name = "label13";
            label13.Size = new Size(106, 21);
            label13.TabIndex = 50;
            label13.Text = "ページレイアウト:";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(86, 254);
            label12.Name = "label12";
            label12.Size = new Size(45, 21);
            label12.TabIndex = 51;
            label12.Text = "表示:";
            // 
            // CreatorLabel
            // 
            CreatorLabel.AutoSize = true;
            CreatorLabel.Location = new Point(137, 496);
            CreatorLabel.Name = "CreatorLabel";
            CreatorLabel.Size = new Size(145, 21);
            CreatorLabel.TabIndex = 47;
            CreatorLabel.Text = "アプリケーションを表示";
            // 
            // ModDateLabel
            // 
            ModDateLabel.AutoSize = true;
            ModDateLabel.Location = new Point(137, 466);
            ModDateLabel.Name = "ModDateLabel";
            ModDateLabel.Size = new Size(102, 21);
            ModDateLabel.TabIndex = 46;
            ModDateLabel.Text = "更新日を表示";
            // 
            // CreationDateLabel
            // 
            CreationDateLabel.AutoSize = true;
            CreationDateLabel.Location = new Point(137, 436);
            CreationDateLabel.Name = "CreationDateLabel";
            CreationDateLabel.Size = new Size(102, 21);
            CreationDateLabel.TabIndex = 45;
            CreationDateLabel.Text = "作成日を表示";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(27, 496);
            label18.Name = "label18";
            label18.Size = new Size(104, 21);
            label18.TabIndex = 44;
            label18.Text = "アプリケーション:";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(70, 466);
            label17.Name = "label17";
            label17.Size = new Size(61, 21);
            label17.TabIndex = 43;
            label17.Text = "更新日:";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(70, 436);
            label16.Name = "label16";
            label16.Size = new Size(61, 21);
            label16.TabIndex = 42;
            label16.Text = "作成日:";
            // 
            // KeywordTxt
            // 
            KeywordTxt.Location = new Point(137, 158);
            KeywordTxt.Multiline = true;
            KeywordTxt.Name = "KeywordTxt";
            KeywordTxt.ScrollBars = ScrollBars.Both;
            KeywordTxt.Size = new Size(600, 80);
            KeywordTxt.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(57, 18);
            label1.Name = "label1";
            label1.Size = new Size(74, 21);
            label1.TabIndex = 36;
            label1.Text = "ファイル名:";
            // 
            // TitleTxt
            // 
            TitleTxt.Location = new Point(137, 48);
            TitleTxt.Name = "TitleTxt";
            TitleTxt.Size = new Size(600, 29);
            TitleTxt.TabIndex = 1;
            TitleTxt.WordWrap = false;
            // 
            // SubTitleTxt
            // 
            SubTitleTxt.Location = new Point(137, 119);
            SubTitleTxt.Name = "SubTitleTxt";
            SubTitleTxt.Size = new Size(600, 29);
            SubTitleTxt.TabIndex = 3;
            SubTitleTxt.WordWrap = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(63, 154);
            label5.Name = "label5";
            label5.Size = new Size(68, 21);
            label5.TabIndex = 35;
            label5.Text = "キーワード:";
            // 
            // FileNamelabel
            // 
            FileNamelabel.AutoSize = true;
            FileNamelabel.Location = new Point(137, 18);
            FileNamelabel.Name = "FileNamelabel";
            FileNamelabel.Size = new Size(115, 21);
            FileNamelabel.TabIndex = 34;
            FileNamelabel.Text = "ファイル名を表示";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(47, 119);
            label4.Name = "label4";
            label4.Size = new Size(84, 21);
            label4.TabIndex = 33;
            label4.Text = "サブタイトル:";
            // 
            // AuthorTxt
            // 
            AuthorTxt.Location = new Point(137, 84);
            AuthorTxt.Name = "AuthorTxt";
            AuthorTxt.Size = new Size(600, 29);
            AuthorTxt.TabIndex = 2;
            AuthorTxt.WordWrap = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(70, 84);
            label3.Name = "label3";
            label3.Size = new Size(61, 21);
            label3.TabIndex = 37;
            label3.Text = "作成者:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(71, 50);
            label2.Name = "label2";
            label2.Size = new Size(60, 21);
            label2.TabIndex = 32;
            label2.Text = "タイトル:";
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(989, 733);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form2";
            SizeGripStyle = SizeGripStyle.Show;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PDFのプロパティ";
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Panel panel1;
        private Button SaveBtn;
        private Button CancelBtn;
        private Panel panel2;
        private Label label10;
        private Label label6;
        private Label label9;
        private Label PdfConvertLabel;
        private Label label8;
        private Label PdfVerLabel;
        private Label label7;
        private Label PasLabel;
        private Label FileSizeLabel;
        private Label PageSizeLabel;
        private Label TotalPageLabel1;
        private TextBox PageTxt;
        private ComboBox MagComboBox;
        private ComboBox PageLayoutComboBox;
        private ComboBox ViewComboBox;
        private Label label15;
        private Label label14;
        private Label label13;
        private Label label12;
        private Label CreatorLabel;
        private Label ModDateLabel;
        private Label CreationDateLabel;
        private Label label18;
        private Label label17;
        private Label label16;
        private TextBox KeywordTxt;
        private Label label1;
        private TextBox TitleTxt;
        private TextBox SubTitleTxt;
        private Label label5;
        private Label FileNamelabel;
        private Label label4;
        private TextBox AuthorTxt;
        private Label label3;
        private Label label2;
    }
}
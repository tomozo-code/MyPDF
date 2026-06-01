namespace MyPDF
{
    partial class Form14
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form14));
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            toolTip1 = new ToolTip(components);
            panel1 = new Panel();
            label7 = new Label();
            groupBox1 = new GroupBox();
            radioColor = new RadioButton();
            radioButton2 = new RadioButton();
            ImageTypeComboBox = new ComboBox();
            label4 = new Label();
            label3 = new Label();
            DpiComboBox = new ComboBox();
            label1 = new Label();
            TotalPage = new Label();
            FileNameTxtBox = new TextBox();
            label5 = new Label();
            label2 = new Label();
            ExtractTxt = new TextBox();
            label6 = new Label();
            panel2 = new Panel();
            OkBtn = new Button();
            CancelBtn = new Button();
            statusStrip1.SuspendLayout();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 434);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(373, 26);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(162, 21);
            toolStripStatusLabel1.Text = "PDFを画像に変換します";
            // 
            // panel1
            // 
            panel1.Controls.Add(label7);
            panel1.Controls.Add(groupBox1);
            panel1.Controls.Add(ImageTypeComboBox);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(DpiComboBox);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(TotalPage);
            panel1.Controls.Add(FileNameTxtBox);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(ExtractTxt);
            panel1.Controls.Add(label6);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(373, 434);
            panel1.TabIndex = 1;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(18, 302);
            label7.Name = "label7";
            label7.Size = new Size(315, 63);
            label7.TabIndex = 90;
            label7.Text = "変換するページ数、解像度、PCの性能等により、\r\n変換にかなり時間がかかる場合があります。\r\n10～20ページ程度を目安に変換して下さい。";
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(radioColor);
            groupBox1.Controls.Add(radioButton2);
            groupBox1.Location = new Point(12, 218);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(349, 72);
            groupBox1.TabIndex = 89;
            groupBox1.TabStop = false;
            groupBox1.Text = "色";
            // 
            // radioColor
            // 
            radioColor.AutoSize = true;
            radioColor.Checked = true;
            radioColor.Location = new Point(18, 28);
            radioColor.Name = "radioColor";
            radioColor.Size = new Size(81, 25);
            radioColor.TabIndex = 87;
            radioColor.TabStop = true;
            radioColor.Tag = "元のまま画像に変換します";
            radioColor.Text = "元のまま";
            radioColor.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(118, 28);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(108, 25);
            radioButton2.TabIndex = 88;
            radioButton2.Tag = "グレースケールの画像に変換します";
            radioButton2.Text = "グレースケール";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // ImageTypeComboBox
            // 
            ImageTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ImageTypeComboBox.FormattingEnabled = true;
            ImageTypeComboBox.Location = new Point(111, 171);
            ImageTypeComboBox.Name = "ImageTypeComboBox";
            ImageTypeComboBox.Size = new Size(121, 29);
            ImageTypeComboBox.TabIndex = 86;
            ImageTypeComboBox.Tag = "画像形式を選択します";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(15, 174);
            label4.Name = "label4";
            label4.Size = new Size(90, 21);
            label4.TabIndex = 85;
            label4.Text = "画像形式：";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(238, 130);
            label3.Name = "label3";
            label3.Size = new Size(32, 21);
            label3.TabIndex = 84;
            label3.Text = "dpi";
            // 
            // DpiComboBox
            // 
            DpiComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            DpiComboBox.FormattingEnabled = true;
            DpiComboBox.Location = new Point(111, 127);
            DpiComboBox.Name = "DpiComboBox";
            DpiComboBox.Size = new Size(121, 29);
            DpiComboBox.TabIndex = 83;
            DpiComboBox.Tag = "解像度を選択します";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(31, 130);
            label1.Name = "label1";
            label1.Size = new Size(74, 21);
            label1.TabIndex = 82;
            label1.Text = "解像度：";
            // 
            // TotalPage
            // 
            TotalPage.AutoSize = true;
            TotalPage.Location = new Point(217, 18);
            TotalPage.Name = "TotalPage";
            TotalPage.Size = new Size(72, 21);
            TotalPage.TabIndex = 78;
            TotalPage.Text = "/ 総ページ";
            // 
            // FileNameTxtBox
            // 
            FileNameTxtBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            FileNameTxtBox.Location = new Point(111, 84);
            FileNameTxtBox.Name = "FileNameTxtBox";
            FileNameTxtBox.Size = new Size(250, 29);
            FileNameTxtBox.TabIndex = 3;
            FileNameTxtBox.Tag = "ファイル名を指定します(ファイル名の後にページ番号が付与されます)";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(61, 47);
            label5.Name = "label5";
            label5.Size = new Size(150, 21);
            label5.TabIndex = 81;
            label5.Text = "入力方法：1,2,3,5-9";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(18, 87);
            label2.Name = "label2";
            label2.Size = new Size(87, 21);
            label2.TabIndex = 1;
            label2.Text = "ファイル名：";
            // 
            // ExtractTxt
            // 
            ExtractTxt.Location = new Point(111, 15);
            ExtractTxt.Name = "ExtractTxt";
            ExtractTxt.Size = new Size(100, 29);
            ExtractTxt.TabIndex = 2;
            ExtractTxt.Tag = "画像変換するページを指定します(入力方法：1,2,3,5-9)";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(11, 18);
            label6.Name = "label6";
            label6.Size = new Size(94, 21);
            label6.TabIndex = 79;
            label6.Text = "ページ指定：";
            // 
            // panel2
            // 
            panel2.Controls.Add(OkBtn);
            panel2.Controls.Add(CancelBtn);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 384);
            panel2.Name = "panel2";
            panel2.Size = new Size(373, 50);
            panel2.TabIndex = 2;
            // 
            // OkBtn
            // 
            OkBtn.Location = new Point(11, 8);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(112, 35);
            OkBtn.TabIndex = 41;
            OkBtn.Tag = "ページ指定を確定し画像変換を実行します";
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(130, 8);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(112, 35);
            CancelBtn.TabIndex = 42;
            CancelBtn.Tag = "中止してウィンドウを閉じます";
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // Form14
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(373, 460);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(statusStrip1);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form14";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ページを指定して画像に変換";
            Load += Form14_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolTip toolTip1;
        private Panel panel1;
        private Panel panel2;
        private Label TotalPage;
        private Label label5;
        private TextBox ExtractTxt;
        private Label label6;
        private Button OkBtn;
        private Button CancelBtn;
        private TextBox FileNameTxtBox;
        private Label label2;
        private ComboBox DpiComboBox;
        private Label label1;
        private ComboBox ImageTypeComboBox;
        private Label label4;
        private Label label3;
        private RadioButton radioButton2;
        private RadioButton radioColor;
        private GroupBox groupBox1;
        private Label label7;
    }
}
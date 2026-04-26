namespace MyPDF
{
    partial class Form6
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form6));
            label1 = new Label();
            label2 = new Label();
            comboBox1 = new ComboBox();
            colorDialog1 = new ColorDialog();
            btnOk = new Button();
            btnCancel = new Button();
            btnColor = new Button();
            label3 = new Label();
            BmTitleTxtBox = new TextBox();
            label4 = new Label();
            PageNoTxtBox = new TextBox();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            toolTip1 = new ToolTip(components);
            TotalPageLabel = new Label();
            panel1 = new Panel();
            statusStrip1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(29, 104);
            label1.Name = "label1";
            label1.Size = new Size(74, 21);
            label1.TabIndex = 0;
            label1.Text = "スタイル：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(29, 143);
            label2.Name = "label2";
            label2.Size = new Size(74, 21);
            label2.TabIndex = 1;
            label2.Text = "色選択：";
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(113, 101);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(161, 29);
            comboBox1.TabIndex = 3;
            comboBox1.Tag = "しおりの文字スタイルを指定します";
            // 
            // btnOk
            // 
            btnOk.Location = new Point(12, 5);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(100, 32);
            btnOk.TabIndex = 5;
            btnOk.Tag = "しおりのプロパティを確定しウィンドウを閉じます";
            btnOk.Text = "OK";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(123, 5);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 32);
            btnCancel.TabIndex = 6;
            btnCancel.Tag = "編集を中止してウィンドウを閉じます";
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnColor
            // 
            btnColor.Location = new Point(113, 137);
            btnColor.Name = "btnColor";
            btnColor.Size = new Size(38, 32);
            btnColor.TabIndex = 4;
            btnColor.Tag = "しおりの文字色を指定します";
            btnColor.UseVisualStyleBackColor = true;
            btnColor.Click += btnColor_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(26, 19);
            label3.Name = "label3";
            label3.Size = new Size(77, 21);
            label3.TabIndex = 5;
            label3.Text = "しおり名：";
            // 
            // BmTitleTxtBox
            // 
            BmTitleTxtBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            BmTitleTxtBox.Location = new Point(113, 16);
            BmTitleTxtBox.Name = "BmTitleTxtBox";
            BmTitleTxtBox.Size = new Size(259, 29);
            BmTitleTxtBox.TabIndex = 1;
            BmTitleTxtBox.Tag = "しおり名を修正します";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 61);
            label4.Name = "label4";
            label4.Size = new Size(94, 21);
            label4.TabIndex = 7;
            label4.Text = "ページ番号：";
            // 
            // PageNoTxtBox
            // 
            PageNoTxtBox.ImeMode = ImeMode.NoControl;
            PageNoTxtBox.Location = new Point(113, 58);
            PageNoTxtBox.Name = "PageNoTxtBox";
            PageNoTxtBox.Size = new Size(79, 29);
            PageNoTxtBox.TabIndex = 2;
            PageNoTxtBox.Tag = "ページ番号を指定します";
            PageNoTxtBox.TextAlign = HorizontalAlignment.Right;
            // 
            // statusStrip1
            // 
            statusStrip1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 235);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(384, 26);
            statusStrip1.TabIndex = 9;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(195, 21);
            toolStripStatusLabel1.Text = "しおりのプロパティを設定します";
            // 
            // TotalPageLabel
            // 
            TotalPageLabel.AutoSize = true;
            TotalPageLabel.Location = new Point(198, 61);
            TotalPageLabel.Name = "TotalPageLabel";
            TotalPageLabel.Size = new Size(88, 21);
            TotalPageLabel.TabIndex = 10;
            TotalPageLabel.Tag = "開いているPDFファイルの総ページ数です";
            TotalPageLabel.Text = "/ 総ページ数";
            // 
            // panel1
            // 
            panel1.Controls.Add(btnOk);
            panel1.Controls.Add(btnCancel);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 185);
            panel1.Name = "panel1";
            panel1.Size = new Size(384, 50);
            panel1.TabIndex = 11;
            // 
            // Form6
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(384, 261);
            Controls.Add(panel1);
            Controls.Add(statusStrip1);
            Controls.Add(TotalPageLabel);
            Controls.Add(PageNoTxtBox);
            Controls.Add(label4);
            Controls.Add(BmTitleTxtBox);
            Controls.Add(label3);
            Controls.Add(btnColor);
            Controls.Add(comboBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form6";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "しおりのプロパティ";
            Load += Form6_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private ComboBox comboBox1;
        private ColorDialog colorDialog1;
        private Button btnOk;
        private Button btnCancel;
        private Button btnColor;
        private Label label3;
        private TextBox BmTitleTxtBox;
        private Label label4;
        private TextBox PageNoTxtBox;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolTip toolTip1;
        private Label TotalPageLabel;
        private Panel panel1;
    }
}
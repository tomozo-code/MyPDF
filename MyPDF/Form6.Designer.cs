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
            OkBtn = new Button();
            CancelBtn = new Button();
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
            panel2 = new Panel();
            label6 = new Label();
            ColorTxtBox2 = new TextBox();
            label5 = new Label();
            ColorTxtBox1 = new TextBox();
            statusStrip1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(32, 100);
            label1.Name = "label1";
            label1.Size = new Size(74, 21);
            label1.TabIndex = 0;
            label1.Text = "スタイル：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(32, 139);
            label2.Name = "label2";
            label2.Size = new Size(74, 21);
            label2.TabIndex = 1;
            label2.Text = "色選択：";
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(107, 97);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(161, 29);
            comboBox1.TabIndex = 3;
            comboBox1.Tag = "しおりの文字スタイルを指定します";
            // 
            // OkBtn
            // 
            OkBtn.Location = new Point(12, 5);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(100, 32);
            OkBtn.TabIndex = 5;
            OkBtn.Tag = "しおりのプロパティを確定しウィンドウを閉じます";
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(123, 5);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(100, 32);
            CancelBtn.TabIndex = 6;
            CancelBtn.Tag = "編集を中止してウィンドウを閉じます";
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // btnColor
            // 
            btnColor.Location = new Point(107, 133);
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
            label3.Location = new Point(29, 15);
            label3.Name = "label3";
            label3.Size = new Size(77, 21);
            label3.TabIndex = 5;
            label3.Text = "しおり名：";
            // 
            // BmTitleTxtBox
            // 
            BmTitleTxtBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            BmTitleTxtBox.Location = new Point(107, 12);
            BmTitleTxtBox.Name = "BmTitleTxtBox";
            BmTitleTxtBox.Size = new Size(273, 29);
            BmTitleTxtBox.TabIndex = 1;
            BmTitleTxtBox.Tag = "しおり名を修正します";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(15, 57);
            label4.Name = "label4";
            label4.Size = new Size(94, 21);
            label4.TabIndex = 7;
            label4.Text = "ページ番号：";
            // 
            // PageNoTxtBox
            // 
            PageNoTxtBox.ImeMode = ImeMode.NoControl;
            PageNoTxtBox.Location = new Point(107, 54);
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
            statusStrip1.Location = new Point(0, 327);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(404, 26);
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
            TotalPageLabel.Location = new Point(192, 57);
            TotalPageLabel.Name = "TotalPageLabel";
            TotalPageLabel.Size = new Size(88, 21);
            TotalPageLabel.TabIndex = 10;
            TotalPageLabel.Tag = "開いているPDFファイルの総ページ数です";
            TotalPageLabel.Text = "/ 総ページ数";
            // 
            // panel1
            // 
            panel1.Controls.Add(OkBtn);
            panel1.Controls.Add(CancelBtn);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 277);
            panel1.Name = "panel1";
            panel1.Size = new Size(404, 50);
            panel1.TabIndex = 11;
            // 
            // panel2
            // 
            panel2.AutoScroll = true;
            panel2.Controls.Add(label6);
            panel2.Controls.Add(ColorTxtBox2);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(ColorTxtBox1);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(TotalPageLabel);
            panel2.Controls.Add(comboBox1);
            panel2.Controls.Add(PageNoTxtBox);
            panel2.Controls.Add(btnColor);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(BmTitleTxtBox);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(404, 277);
            panel2.TabIndex = 12;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(245, 168);
            label6.Name = "label6";
            label6.Size = new Size(50, 21);
            label6.TabIndex = 14;
            label6.Text = "(RGB)";
            // 
            // ColorTxtBox2
            // 
            ColorTxtBox2.Location = new Point(151, 168);
            ColorTxtBox2.Name = "ColorTxtBox2";
            ColorTxtBox2.Size = new Size(90, 29);
            ColorTxtBox2.TabIndex = 13;
            ColorTxtBox2.Tag = "R(赤),G(緑),B(青)表記";
            ColorTxtBox2.Text = "RGB";
            ColorTxtBox2.TextAlign = HorizontalAlignment.Right;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(245, 137);
            label5.Name = "label5";
            label5.Size = new Size(54, 21);
            label5.TabIndex = 12;
            label5.Text = "(16進)";
            // 
            // ColorTxtBox1
            // 
            ColorTxtBox1.Location = new Point(151, 138);
            ColorTxtBox1.Name = "ColorTxtBox1";
            ColorTxtBox1.Size = new Size(90, 29);
            ColorTxtBox1.TabIndex = 11;
            ColorTxtBox1.Tag = "16進数表記(カラーコード)";
            ColorTxtBox1.Text = "#123456";
            ColorTxtBox1.TextAlign = HorizontalAlignment.Right;
            // 
            // Form6
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(404, 353);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(statusStrip1);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form6";
            StartPosition = FormStartPosition.CenterParent;
            Text = "しおりのプロパティ";
            Load += Form6_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private ComboBox comboBox1;
        private ColorDialog colorDialog1;
        private Button OkBtn;
        private Button CancelBtn;
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
        private Panel panel2;
        private TextBox ColorTxtBox1;
        private Label label5;
        private TextBox ColorTxtBox2;
        private Label label6;
    }
}
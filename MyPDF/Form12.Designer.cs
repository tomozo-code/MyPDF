namespace MyPDF
{
    partial class Form12
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form12));
            toolTip1 = new ToolTip(components);
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            CancelBtn = new Button();
            OkBtn = new Button();
            groupBox1 = new GroupBox();
            nowFileName = new TextBox();
            label2 = new Label();
            label4 = new Label();
            ExtractTxt = new TextBox();
            label7 = new Label();
            TotalPage = new Label();
            panel1 = new Panel();
            panel2 = new Panel();
            groupBox2 = new GroupBox();
            InsTotalPageLabel = new Label();
            txtPage = new TextBox();
            label6 = new Label();
            label1 = new Label();
            KaeFileName = new TextBox();
            statusStrip1.SuspendLayout();
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 352);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 16, 0);
            statusStrip1.Size = new Size(535, 26);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(195, 21);
            toolStripStatusLabel1.Text = "ファイルからページを置換します";
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(127, 7);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(112, 35);
            CancelBtn.TabIndex = 40;
            CancelBtn.Tag = "中止してウィンドウを閉じます";
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // OkBtn
            // 
            OkBtn.Location = new Point(8, 7);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(112, 35);
            OkBtn.TabIndex = 30;
            OkBtn.Tag = "ページ指定を確定し挿入を実行します";
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(nowFileName);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(ExtractTxt);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(TotalPage);
            groupBox1.Location = new Point(12, 147);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(507, 140);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "置換先(現在表示しているPDFファイル)";
            // 
            // nowFileName
            // 
            nowFileName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            nowFileName.Location = new Point(141, 32);
            nowFileName.Name = "nowFileName";
            nowFileName.Size = new Size(358, 29);
            nowFileName.TabIndex = 0;
            nowFileName.Text = "今開いてるファイル名が表示される";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 34);
            label2.Name = "label2";
            label2.Size = new Size(119, 21);
            label2.TabIndex = 93;
            label2.Text = "置換先ファイル：";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(127, 106);
            label4.Name = "label4";
            label4.Size = new Size(114, 21);
            label4.TabIndex = 92;
            label4.Text = "入力方法：1-9";
            // 
            // ExtractTxt
            // 
            ExtractTxt.Location = new Point(141, 74);
            ExtractTxt.Name = "ExtractTxt";
            ExtractTxt.Size = new Size(100, 29);
            ExtractTxt.TabIndex = 1;
            ExtractTxt.Tag = "置換先のページを指定します(入力方法：1-9)";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(41, 77);
            label7.Name = "label7";
            label7.Size = new Size(94, 21);
            label7.TabIndex = 90;
            label7.Text = "ページ指定：";
            // 
            // TotalPage
            // 
            TotalPage.AutoSize = true;
            TotalPage.Location = new Point(247, 77);
            TotalPage.Name = "TotalPage";
            TotalPage.Size = new Size(72, 21);
            TotalPage.TabIndex = 25;
            TotalPage.Text = "/ 総ページ";
            // 
            // panel1
            // 
            panel1.Controls.Add(OkBtn);
            panel1.Controls.Add(CancelBtn);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 302);
            panel1.Name = "panel1";
            panel1.Size = new Size(535, 50);
            panel1.TabIndex = 76;
            // 
            // panel2
            // 
            panel2.AutoScroll = true;
            panel2.Controls.Add(groupBox2);
            panel2.Controls.Add(groupBox1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(535, 302);
            panel2.TabIndex = 77;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(InsTotalPageLabel);
            groupBox2.Controls.Add(txtPage);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(KaeFileName);
            groupBox2.Location = new Point(12, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(507, 120);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "置換するPDFファイル";
            // 
            // InsTotalPageLabel
            // 
            InsTotalPageLabel.AutoSize = true;
            InsTotalPageLabel.Location = new Point(247, 82);
            InsTotalPageLabel.Name = "InsTotalPageLabel";
            InsTotalPageLabel.Size = new Size(72, 21);
            InsTotalPageLabel.TabIndex = 86;
            InsTotalPageLabel.Text = "/ 総ページ";
            // 
            // txtPage
            // 
            txtPage.Location = new Point(141, 79);
            txtPage.Name = "txtPage";
            txtPage.Size = new Size(100, 29);
            txtPage.TabIndex = 0;
            txtPage.Tag = "置換するファイルのページを指定します(入力方法：1,2,3,5-9)";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(41, 82);
            label6.Name = "label6";
            label6.Size = new Size(94, 21);
            label6.TabIndex = 87;
            label6.Text = "開始ページ：";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(7, 37);
            label1.Name = "label1";
            label1.Size = new Size(128, 21);
            label1.TabIndex = 73;
            label1.Text = "置換するファイル：";
            // 
            // KaeFileName
            // 
            KaeFileName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            KaeFileName.Location = new Point(141, 36);
            KaeFileName.Name = "KaeFileName";
            KaeFileName.Size = new Size(359, 29);
            KaeFileName.TabIndex = 0;
            KaeFileName.Text = "置換するファイル名が表示される";
            // 
            // Form12
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(535, 378);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(statusStrip1);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form12";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ページを置換";
            Load += Form12_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolTip toolTip1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private Button CancelBtn;
        private Button OkBtn;
        private GroupBox groupBox1;
        private Label TotalPage;
        private Panel panel1;
        private Panel panel2;
        private GroupBox groupBox2;
        private Label label1;
        private TextBox KaeFileName;
        private Label InsTotalPageLabel;
        private TextBox txtPage;
        private Label label6;
        private Label label4;
        private TextBox ExtractTxt;
        private Label label7;
        private Label label2;
        private TextBox nowFileName;
    }
}
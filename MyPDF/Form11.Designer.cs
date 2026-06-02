namespace MyPDF
{
    partial class Form11
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form11));
            toolTip1 = new ToolTip(components);
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            groupBox1 = new GroupBox();
            TotalPage = new Label();
            label5 = new Label();
            ExtractTxt = new TextBox();
            label6 = new Label();
            label3 = new Label();
            label4 = new Label();
            CancelBtn = new Button();
            OkBtn = new Button();
            TargetPageTxt = new TextBox();
            panel1 = new Panel();
            panel2 = new Panel();
            groupBox2 = new GroupBox();
            Next = new RadioButton();
            Prev = new RadioButton();
            TotalPage2 = new Label();
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
            statusStrip1.Location = new Point(0, 291);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(392, 26);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(182, 21);
            toolStripStatusLabel1.Text = "指定したページを移動します";
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(TotalPage);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(ExtractTxt);
            groupBox1.Controls.Add(label6);
            groupBox1.Location = new Point(12, 7);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(361, 95);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "移動するページ";
            // 
            // TotalPage
            // 
            TotalPage.AutoSize = true;
            TotalPage.Location = new Point(232, 34);
            TotalPage.Name = "TotalPage";
            TotalPage.Size = new Size(72, 21);
            TotalPage.TabIndex = 78;
            TotalPage.Text = "/ 総ページ";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(76, 63);
            label5.Name = "label5";
            label5.Size = new Size(150, 21);
            label5.TabIndex = 81;
            label5.Text = "入力方法：1,2,3,5-9";
            // 
            // ExtractTxt
            // 
            ExtractTxt.Location = new Point(126, 31);
            ExtractTxt.Name = "ExtractTxt";
            ExtractTxt.Size = new Size(100, 29);
            ExtractTxt.TabIndex = 0;
            ExtractTxt.Tag = "抽出するページを指定します(入力方法：1,2,3,5-9)";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(26, 34);
            label6.Name = "label6";
            label6.Size = new Size(94, 21);
            label6.TabIndex = 79;
            label6.Text = "ページ指定：";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(10, 31);
            label3.Name = "label3";
            label3.Size = new Size(110, 21);
            label3.TabIndex = 2;
            label3.Text = "移動先ページ：";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(5, 66);
            label4.Name = "label4";
            label4.Size = new Size(115, 21);
            label4.TabIndex = 74;
            label4.Text = "移動する場所：";
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(126, 7);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(112, 35);
            CancelBtn.TabIndex = 1;
            CancelBtn.Tag = "中止してウィンドウを閉じます";
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // OkBtn
            // 
            OkBtn.Location = new Point(7, 7);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(112, 35);
            OkBtn.TabIndex = 0;
            OkBtn.Tag = "設定を確定し移動を実行します";
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // TargetPageTxt
            // 
            TargetPageTxt.Location = new Point(126, 28);
            TargetPageTxt.Name = "TargetPageTxt";
            TargetPageTxt.Size = new Size(100, 29);
            TargetPageTxt.TabIndex = 0;
            TargetPageTxt.Tag = "移動先ページを指定します";
            // 
            // panel1
            // 
            panel1.Controls.Add(OkBtn);
            panel1.Controls.Add(CancelBtn);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 241);
            panel1.Name = "panel1";
            panel1.Size = new Size(392, 50);
            panel1.TabIndex = 75;
            // 
            // panel2
            // 
            panel2.AutoScroll = true;
            panel2.Controls.Add(groupBox2);
            panel2.Controls.Add(groupBox1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(392, 241);
            panel2.TabIndex = 76;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(TargetPageTxt);
            groupBox2.Controls.Add(Next);
            groupBox2.Controls.Add(Prev);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(TotalPage2);
            groupBox2.Controls.Add(label3);
            groupBox2.Location = new Point(12, 108);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(361, 102);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "移動先ページ";
            // 
            // Next
            // 
            Next.AutoSize = true;
            Next.Checked = true;
            Next.Location = new Point(182, 65);
            Next.Name = "Next";
            Next.Size = new Size(44, 25);
            Next.TabIndex = 2;
            Next.TabStop = true;
            Next.Tag = "移動先ページの後へ移動します";
            Next.Text = "後";
            Next.UseVisualStyleBackColor = true;
            // 
            // Prev
            // 
            Prev.AutoSize = true;
            Prev.Location = new Point(126, 65);
            Prev.Name = "Prev";
            Prev.Size = new Size(44, 25);
            Prev.TabIndex = 1;
            Prev.Tag = "移動先ページの前へ移動します";
            Prev.Text = "前";
            Prev.UseVisualStyleBackColor = true;
            // 
            // TotalPage2
            // 
            TotalPage2.AutoSize = true;
            TotalPage2.Location = new Point(232, 31);
            TotalPage2.Name = "TotalPage2";
            TotalPage2.Size = new Size(72, 21);
            TotalPage2.TabIndex = 75;
            TotalPage2.Text = "/ 総ページ";
            // 
            // Form11
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(392, 317);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(statusStrip1);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form11";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ページを指定して移動";
            Load += Form11_Load;
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
        private GroupBox groupBox1;
        private Label label3;
        private Label label4;
        private Button CancelBtn;
        private Button OkBtn;
        private TextBox TargetPageTxt;
        private Panel panel1;
        private Panel panel2;
        private Label TotalPage;
        private Label label5;
        private TextBox ExtractTxt;
        private Label label6;
        private GroupBox groupBox2;
        private RadioButton Next;
        private RadioButton Prev;
        private Label TotalPage2;
    }
}
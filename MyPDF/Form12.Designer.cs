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
            KaeFileName = new TextBox();
            label1 = new Label();
            CancelBtn = new Button();
            OkBtn = new Button();
            groupBox1 = new GroupBox();
            TotalPage = new Label();
            EndKaeTxt = new TextBox();
            StartKaeTxt = new TextBox();
            label2 = new Label();
            label3 = new Label();
            statusStrip1.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 215);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 16, 0);
            statusStrip1.Size = new Size(384, 26);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(195, 21);
            toolStripStatusLabel1.Text = "ファイルからページを置換します";
            // 
            // KaeFileName
            // 
            KaeFileName.Location = new Point(134, 8);
            KaeFileName.Name = "KaeFileName";
            KaeFileName.Size = new Size(216, 29);
            KaeFileName.TabIndex = 72;
            KaeFileName.Text = "置換するファイル名が表示される";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(128, 21);
            label1.TabIndex = 71;
            label1.Text = "置換するファイル：";
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(176, 168);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(112, 35);
            CancelBtn.TabIndex = 74;
            CancelBtn.Tag = "中止してウィンドウを閉じます";
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // OkBtn
            // 
            OkBtn.Location = new Point(57, 168);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(112, 35);
            OkBtn.TabIndex = 73;
            OkBtn.Tag = "ページ指定を確定し挿入を実行します";
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(TotalPage);
            groupBox1.Controls.Add(EndKaeTxt);
            groupBox1.Controls.Add(StartKaeTxt);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label3);
            groupBox1.Location = new Point(12, 43);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(338, 110);
            groupBox1.TabIndex = 75;
            groupBox1.TabStop = false;
            groupBox1.Text = "元のページ";
            // 
            // TotalPage
            // 
            TotalPage.AutoSize = true;
            TotalPage.Location = new Point(209, 66);
            TotalPage.Name = "TotalPage";
            TotalPage.Size = new Size(72, 21);
            TotalPage.TabIndex = 25;
            TotalPage.Text = "/ 総ページ";
            // 
            // EndKaeTxt
            // 
            EndKaeTxt.Location = new Point(123, 63);
            EndKaeTxt.Name = "EndKaeTxt";
            EndKaeTxt.Size = new Size(80, 29);
            EndKaeTxt.TabIndex = 22;
            EndKaeTxt.Tag = "終了ページを指定します";
            // 
            // StartKaeTxt
            // 
            StartKaeTxt.Location = new Point(123, 22);
            StartKaeTxt.Name = "StartKaeTxt";
            StartKaeTxt.Size = new Size(80, 29);
            StartKaeTxt.TabIndex = 21;
            StartKaeTxt.Tag = "開始ページを指定します";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(34, 66);
            label2.Name = "label2";
            label2.Size = new Size(94, 21);
            label2.TabIndex = 24;
            label2.Text = "終了ページ：";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(34, 25);
            label3.Name = "label3";
            label3.Size = new Size(94, 21);
            label3.TabIndex = 23;
            label3.Text = "開始ページ：";
            // 
            // Form12
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(384, 241);
            Controls.Add(groupBox1);
            Controls.Add(CancelBtn);
            Controls.Add(OkBtn);
            Controls.Add(KaeFileName);
            Controls.Add(label1);
            Controls.Add(statusStrip1);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form12";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ページを置換";
            Load += Form12_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolTip toolTip1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private TextBox KaeFileName;
        private Label label1;
        private Button CancelBtn;
        private Button OkBtn;
        private GroupBox groupBox1;
        private Label TotalPage;
        private TextBox EndKaeTxt;
        private TextBox StartKaeTxt;
        private Label label2;
        private Label label3;
    }
}
namespace MyPDF
{
    partial class Form9
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form9));
            toolTip1 = new ToolTip(components);
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            TotalPage = new Label();
            CancelBtn = new Button();
            OkBtn = new Button();
            StartExtractTxt = new TextBox();
            label2 = new Label();
            label1 = new Label();
            EndExtractTxt = new TextBox();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 139);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(273, 26);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(181, 21);
            toolStripStatusLabel1.Text = "ページを指定して抽出します";
            // 
            // TotalPage
            // 
            TotalPage.AutoSize = true;
            TotalPage.Location = new Point(187, 50);
            TotalPage.Name = "TotalPage";
            TotalPage.Size = new Size(72, 21);
            TotalPage.TabIndex = 20;
            TotalPage.Text = "/ 総ページ";
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(145, 92);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(112, 35);
            CancelBtn.TabIndex = 4;
            CancelBtn.Tag = "中止してウィンドウを閉じます";
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // OkBtn
            // 
            OkBtn.Location = new Point(26, 92);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(112, 35);
            OkBtn.TabIndex = 3;
            OkBtn.Tag = "ページ指定を確定し抽出を実行します";
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // StartExtractTxt
            // 
            StartExtractTxt.Location = new Point(101, 6);
            StartExtractTxt.Name = "StartExtractTxt";
            StartExtractTxt.Size = new Size(80, 29);
            StartExtractTxt.TabIndex = 1;
            StartExtractTxt.Tag = "開始ページを指定します";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 50);
            label2.Name = "label2";
            label2.Size = new Size(94, 21);
            label2.TabIndex = 17;
            label2.Text = "終了ページ：";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(94, 21);
            label1.TabIndex = 15;
            label1.Text = "開始ページ：";
            // 
            // EndExtractTxt
            // 
            EndExtractTxt.Location = new Point(101, 47);
            EndExtractTxt.Name = "EndExtractTxt";
            EndExtractTxt.Size = new Size(80, 29);
            EndExtractTxt.TabIndex = 2;
            EndExtractTxt.Tag = "終了ページを指定します";
            // 
            // Form9
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(273, 165);
            Controls.Add(TotalPage);
            Controls.Add(CancelBtn);
            Controls.Add(OkBtn);
            Controls.Add(EndExtractTxt);
            Controls.Add(StartExtractTxt);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(statusStrip1);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form9";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ページを指定して抽出";
            Load += Form9_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolTip toolTip1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private Label TotalPage;
        private Button CancelBtn;
        private Button OkBtn;
        private TextBox StartExtractTxt;
        private Label label2;
        private Label label1;
        private TextBox EndExtractTxt;
    }
}
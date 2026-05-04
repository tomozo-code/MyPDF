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
            panel1 = new Panel();
            panel2 = new Panel();
            statusStrip1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 165);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(295, 26);
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
            TotalPage.Location = new Point(198, 52);
            TotalPage.Name = "TotalPage";
            TotalPage.Size = new Size(72, 21);
            TotalPage.TabIndex = 20;
            TotalPage.Text = "/ 総ページ";
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(126, 8);
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
            OkBtn.Location = new Point(7, 8);
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
            StartExtractTxt.Location = new Point(112, 8);
            StartExtractTxt.Name = "StartExtractTxt";
            StartExtractTxt.Size = new Size(80, 29);
            StartExtractTxt.TabIndex = 1;
            StartExtractTxt.Tag = "開始ページを指定します";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 52);
            label2.Name = "label2";
            label2.Size = new Size(94, 21);
            label2.TabIndex = 17;
            label2.Text = "終了ページ：";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 11);
            label1.Name = "label1";
            label1.Size = new Size(94, 21);
            label1.TabIndex = 15;
            label1.Text = "開始ページ：";
            // 
            // EndExtractTxt
            // 
            EndExtractTxt.Location = new Point(112, 49);
            EndExtractTxt.Name = "EndExtractTxt";
            EndExtractTxt.Size = new Size(80, 29);
            EndExtractTxt.TabIndex = 2;
            EndExtractTxt.Tag = "終了ページを指定します";
            // 
            // panel1
            // 
            panel1.Controls.Add(OkBtn);
            panel1.Controls.Add(CancelBtn);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 115);
            panel1.Name = "panel1";
            panel1.Size = new Size(295, 50);
            panel1.TabIndex = 21;
            // 
            // panel2
            // 
            panel2.AutoScroll = true;
            panel2.Controls.Add(label1);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(StartExtractTxt);
            panel2.Controls.Add(TotalPage);
            panel2.Controls.Add(EndExtractTxt);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(295, 115);
            panel2.TabIndex = 22;
            // 
            // Form9
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(295, 191);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(statusStrip1);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form9";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ページを指定して抽出";
            Load += Form9_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
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
        private Panel panel1;
        private Panel panel2;
    }
}
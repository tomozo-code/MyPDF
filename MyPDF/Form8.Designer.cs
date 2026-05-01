namespace MyPDF
{
    partial class Form8
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form8));
            CancelBtn = new Button();
            OkBtn = new Button();
            EndDelTxt = new TextBox();
            StartDelTxt = new TextBox();
            label2 = new Label();
            label1 = new Label();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            toolTip1 = new ToolTip(components);
            TotalPage = new Label();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(143, 94);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(112, 35);
            CancelBtn.TabIndex = 12;
            CancelBtn.Tag = "中止してウィンドウを閉じます";
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // OkBtn
            // 
            OkBtn.Location = new Point(24, 94);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(112, 35);
            OkBtn.TabIndex = 11;
            OkBtn.Tag = "ページ指定を確定し削除を実行します";
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // EndDelTxt
            // 
            EndDelTxt.Location = new Point(99, 49);
            EndDelTxt.Name = "EndDelTxt";
            EndDelTxt.Size = new Size(80, 29);
            EndDelTxt.TabIndex = 9;
            EndDelTxt.Tag = "終了ページを指定します";
            // 
            // StartDelTxt
            // 
            StartDelTxt.Location = new Point(99, 8);
            StartDelTxt.Name = "StartDelTxt";
            StartDelTxt.Size = new Size(80, 29);
            StartDelTxt.TabIndex = 7;
            StartDelTxt.Tag = "開始ページを指定します";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(10, 52);
            label2.Name = "label2";
            label2.Size = new Size(94, 21);
            label2.TabIndex = 10;
            label2.Text = "終了ページ：";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 11);
            label1.Name = "label1";
            label1.Size = new Size(94, 21);
            label1.TabIndex = 8;
            label1.Text = "開始ページ：";
            // 
            // statusStrip1
            // 
            statusStrip1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 135);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 16, 0);
            statusStrip1.Size = new Size(284, 26);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 6;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(181, 21);
            toolStripStatusLabel1.Text = "ページを指定して削除します";
            // 
            // TotalPage
            // 
            TotalPage.AutoSize = true;
            TotalPage.Location = new Point(185, 52);
            TotalPage.Name = "TotalPage";
            TotalPage.Size = new Size(72, 21);
            TotalPage.TabIndex = 13;
            TotalPage.Text = "/ 総ページ";
            // 
            // Form8
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 161);
            Controls.Add(TotalPage);
            Controls.Add(CancelBtn);
            Controls.Add(OkBtn);
            Controls.Add(EndDelTxt);
            Controls.Add(StartDelTxt);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(statusStrip1);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form8";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ページを指定して削除";
            Load += Form8_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button CancelBtn;
        private Button OkBtn;
        private TextBox EndDelTxt;
        private TextBox StartDelTxt;
        private Label label2;
        private Label label1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolTip toolTip1;
        private Label TotalPage;
    }
}
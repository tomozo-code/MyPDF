namespace MyPDF
{
    partial class Form7
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form7));
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            label3 = new Label();
            RollSelect = new ComboBox();
            CancelBtn = new Button();
            OkBtn = new Button();
            toolTip1 = new ToolTip(components);
            panel1 = new Panel();
            panel2 = new Panel();
            label4 = new Label();
            ExtractTxt = new TextBox();
            label5 = new Label();
            TotalPage = new Label();
            statusStrip1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 293);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(427, 26);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(181, 21);
            toolStripStatusLabel1.Text = "ページを指定して回転します";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 83);
            label3.Name = "label3";
            label3.Size = new Size(90, 21);
            label3.TabIndex = 5;
            label3.Text = "回転方法：";
            // 
            // RollSelect
            // 
            RollSelect.DropDownStyle = ComboBoxStyle.DropDownList;
            RollSelect.FormattingEnabled = true;
            RollSelect.Location = new Point(108, 80);
            RollSelect.Name = "RollSelect";
            RollSelect.Size = new Size(130, 29);
            RollSelect.TabIndex = 3;
            RollSelect.Tag = "回転方法を選択します";
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(114, 8);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(100, 32);
            CancelBtn.TabIndex = 5;
            CancelBtn.Tag = "中止してウィンドウを閉じます";
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // OkBtn
            // 
            OkBtn.Location = new Point(8, 8);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(100, 32);
            OkBtn.TabIndex = 4;
            OkBtn.Tag = "ページ指定を確定し回転を実行します";
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(OkBtn);
            panel1.Controls.Add(CancelBtn);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 243);
            panel1.Name = "panel1";
            panel1.Size = new Size(427, 50);
            panel1.TabIndex = 7;
            // 
            // panel2
            // 
            panel2.AutoScroll = true;
            panel2.Controls.Add(label4);
            panel2.Controls.Add(ExtractTxt);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(TotalPage);
            panel2.Controls.Add(RollSelect);
            panel2.Controls.Add(label3);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(427, 243);
            panel2.TabIndex = 8;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(58, 44);
            label4.Name = "label4";
            label4.Size = new Size(150, 21);
            label4.TabIndex = 30;
            label4.Text = "入力方法：1,2,3,5-9";
            // 
            // ExtractTxt
            // 
            ExtractTxt.Location = new Point(108, 12);
            ExtractTxt.Name = "ExtractTxt";
            ExtractTxt.Size = new Size(100, 29);
            ExtractTxt.TabIndex = 1;
            ExtractTxt.Tag = "回転するページを指定します(入力方法：1,2,3,5-9)";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(8, 15);
            label5.Name = "label5";
            label5.Size = new Size(94, 21);
            label5.TabIndex = 28;
            label5.Text = "ページ指定：";
            // 
            // TotalPage
            // 
            TotalPage.AutoSize = true;
            TotalPage.Location = new Point(214, 15);
            TotalPage.Name = "TotalPage";
            TotalPage.Size = new Size(72, 21);
            TotalPage.TabIndex = 27;
            TotalPage.Text = "/ 総ページ";
            // 
            // Form7
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(427, 319);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(statusStrip1);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form7";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ページを指定して回転";
            Load += Form7_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip statusStrip1;
        private Label label3;
        private ComboBox RollSelect;
        private Button CancelBtn;
        private Button OkBtn;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolTip toolTip1;
        private Panel panel1;
        private Panel panel2;
        private Label label4;
        private TextBox ExtractTxt;
        private Label label5;
        private Label TotalPage;
    }
}
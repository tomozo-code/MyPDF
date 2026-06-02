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
            CancelBtn = new Button();
            OkBtn = new Button();
            toolTip1 = new ToolTip(components);
            panel1 = new Panel();
            panel2 = new Panel();
            groupBox1 = new GroupBox();
            radio180 = new RadioButton();
            radioRight90 = new RadioButton();
            radioLeft90 = new RadioButton();
            label4 = new Label();
            ExtractTxt = new TextBox();
            label5 = new Label();
            TotalPage = new Label();
            statusStrip1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 377);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(434, 26);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(181, 21);
            toolStripStatusLabel1.Text = "ページを指定して回転します";
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(114, 8);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(100, 32);
            CancelBtn.TabIndex = 4;
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
            OkBtn.TabIndex = 3;
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
            panel1.Location = new Point(0, 327);
            panel1.Name = "panel1";
            panel1.Size = new Size(434, 50);
            panel1.TabIndex = 7;
            // 
            // panel2
            // 
            panel2.AutoScroll = true;
            panel2.Controls.Add(groupBox1);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(ExtractTxt);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(TotalPage);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(434, 327);
            panel2.TabIndex = 8;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(radio180);
            groupBox1.Controls.Add(radioRight90);
            groupBox1.Controls.Add(radioLeft90);
            groupBox1.Location = new Point(8, 74);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(414, 67);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "回転方法：";
            // 
            // radio180
            // 
            radio180.AutoSize = true;
            radio180.Location = new Point(248, 28);
            radio180.Name = "radio180";
            radio180.Size = new Size(93, 25);
            radio180.TabIndex = 2;
            radio180.Text = "180°回転";
            radio180.UseVisualStyleBackColor = true;
            // 
            // radioRight90
            // 
            radioRight90.AutoSize = true;
            radioRight90.Location = new Point(129, 28);
            radioRight90.Name = "radioRight90";
            radioRight90.Size = new Size(113, 25);
            radioRight90.TabIndex = 1;
            radioRight90.Text = "右へ90°回転";
            radioRight90.UseVisualStyleBackColor = true;
            // 
            // radioLeft90
            // 
            radioLeft90.AutoSize = true;
            radioLeft90.Checked = true;
            radioLeft90.Location = new Point(10, 28);
            radioLeft90.Name = "radioLeft90";
            radioLeft90.Size = new Size(113, 25);
            radioLeft90.TabIndex = 2;
            radioLeft90.TabStop = true;
            radioLeft90.Text = "左へ90°回転";
            radioLeft90.UseVisualStyleBackColor = true;
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
            ClientSize = new Size(434, 403);
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
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip statusStrip1;
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
        private GroupBox groupBox1;
        private RadioButton radio180;
        private RadioButton radioRight90;
        private RadioButton radioLeft90;
    }
}
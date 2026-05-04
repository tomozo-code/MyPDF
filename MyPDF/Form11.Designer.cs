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
            EndMoveTxt = new TextBox();
            StartMoveTxt = new TextBox();
            label2 = new Label();
            label1 = new Label();
            label3 = new Label();
            MovePlace = new ComboBox();
            label4 = new Label();
            CancelBtn = new Button();
            OkBtn = new Button();
            TargetPageTxt = new TextBox();
            panel1 = new Panel();
            panel2 = new Panel();
            statusStrip1.SuspendLayout();
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 282);
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
            groupBox1.Controls.Add(EndMoveTxt);
            groupBox1.Controls.Add(StartMoveTxt);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(12, 7);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(361, 107);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "移動するページ";
            // 
            // TotalPage
            // 
            TotalPage.AutoSize = true;
            TotalPage.Location = new Point(202, 66);
            TotalPage.Name = "TotalPage";
            TotalPage.Size = new Size(72, 21);
            TotalPage.TabIndex = 25;
            TotalPage.Text = "/ 総ページ";
            // 
            // EndMoveTxt
            // 
            EndMoveTxt.Location = new Point(116, 63);
            EndMoveTxt.Name = "EndMoveTxt";
            EndMoveTxt.Size = new Size(80, 29);
            EndMoveTxt.TabIndex = 2;
            EndMoveTxt.Tag = "終了ページを指定します";
            // 
            // StartMoveTxt
            // 
            StartMoveTxt.Location = new Point(116, 22);
            StartMoveTxt.Name = "StartMoveTxt";
            StartMoveTxt.Size = new Size(80, 29);
            StartMoveTxt.TabIndex = 1;
            StartMoveTxt.Tag = "開始ページを指定します";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 66);
            label2.Name = "label2";
            label2.Size = new Size(94, 21);
            label2.TabIndex = 24;
            label2.Text = "終了ページ：";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 25);
            label1.Name = "label1";
            label1.Size = new Size(94, 21);
            label1.TabIndex = 23;
            label1.Text = "開始ページ：";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 128);
            label3.Name = "label3";
            label3.Size = new Size(110, 21);
            label3.TabIndex = 2;
            label3.Text = "移動先ページ：";
            // 
            // MovePlace
            // 
            MovePlace.DropDownStyle = ComboBoxStyle.DropDownList;
            MovePlace.FormattingEnabled = true;
            MovePlace.Location = new Point(127, 160);
            MovePlace.Name = "MovePlace";
            MovePlace.Size = new Size(121, 29);
            MovePlace.TabIndex = 4;
            MovePlace.Tag = "移動する場所を指定します";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(7, 163);
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
            CancelBtn.TabIndex = 6;
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
            OkBtn.TabIndex = 5;
            OkBtn.Tag = "設定を確定し移動を実行します";
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // TargetPageTxt
            // 
            TargetPageTxt.Location = new Point(128, 125);
            TargetPageTxt.Name = "TargetPageTxt";
            TargetPageTxt.Size = new Size(80, 29);
            TargetPageTxt.TabIndex = 3;
            TargetPageTxt.Tag = "移動先ページを指定します";
            // 
            // panel1
            // 
            panel1.Controls.Add(OkBtn);
            panel1.Controls.Add(CancelBtn);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 232);
            panel1.Name = "panel1";
            panel1.Size = new Size(392, 50);
            panel1.TabIndex = 75;
            // 
            // panel2
            // 
            panel2.AutoScroll = true;
            panel2.Controls.Add(label3);
            panel2.Controls.Add(groupBox1);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(TargetPageTxt);
            panel2.Controls.Add(MovePlace);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(392, 232);
            panel2.TabIndex = 76;
            // 
            // Form11
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(392, 308);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(statusStrip1);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form11";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ページを移動";
            Load += Form11_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
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
        private GroupBox groupBox1;
        private Label TotalPage;
        private TextBox EndMoveTxt;
        private TextBox StartMoveTxt;
        private Label label2;
        private Label label1;
        private Label label3;
        private ComboBox MovePlace;
        private Label label4;
        private Button CancelBtn;
        private Button OkBtn;
        private TextBox TargetPageTxt;
        private Panel panel1;
        private Panel panel2;
    }
}
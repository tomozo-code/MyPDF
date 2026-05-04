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
            label1 = new Label();
            label2 = new Label();
            StartRollTxt = new TextBox();
            EndRollTxt = new TextBox();
            label3 = new Label();
            RollSelect = new ComboBox();
            CancelBtn = new Button();
            OkBtn = new Button();
            toolTip1 = new ToolTip(components);
            TotalPage = new Label();
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
            statusStrip1.Location = new Point(0, 193);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(305, 26);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(181, 21);
            toolStripStatusLabel1.Text = "ページを指定して回転します";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 10);
            label1.Name = "label1";
            label1.Size = new Size(94, 21);
            label1.TabIndex = 1;
            label1.Text = "開始ページ：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 47);
            label2.Name = "label2";
            label2.Size = new Size(94, 21);
            label2.TabIndex = 2;
            label2.Text = "終了ページ：";
            // 
            // StartRollTxt
            // 
            StartRollTxt.Location = new Point(103, 7);
            StartRollTxt.Name = "StartRollTxt";
            StartRollTxt.Size = new Size(80, 29);
            StartRollTxt.TabIndex = 1;
            StartRollTxt.Tag = "開始ページを指定します";
            // 
            // EndRollTxt
            // 
            EndRollTxt.Location = new Point(103, 44);
            EndRollTxt.Name = "EndRollTxt";
            EndRollTxt.Size = new Size(80, 29);
            EndRollTxt.TabIndex = 2;
            EndRollTxt.Tag = "終了ページを指定します";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(7, 87);
            label3.Name = "label3";
            label3.Size = new Size(90, 21);
            label3.TabIndex = 5;
            label3.Text = "回転方法：";
            // 
            // RollSelect
            // 
            RollSelect.DropDownStyle = ComboBoxStyle.DropDownList;
            RollSelect.FormattingEnabled = true;
            RollSelect.Location = new Point(103, 84);
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
            // TotalPage
            // 
            TotalPage.AutoSize = true;
            TotalPage.Location = new Point(189, 47);
            TotalPage.Name = "TotalPage";
            TotalPage.Size = new Size(72, 21);
            TotalPage.TabIndex = 6;
            TotalPage.Text = "/ 総ページ";
            // 
            // panel1
            // 
            panel1.Controls.Add(OkBtn);
            panel1.Controls.Add(CancelBtn);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 143);
            panel1.Name = "panel1";
            panel1.Size = new Size(305, 50);
            panel1.TabIndex = 7;
            // 
            // panel2
            // 
            panel2.AutoScroll = true;
            panel2.Controls.Add(label1);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(StartRollTxt);
            panel2.Controls.Add(TotalPage);
            panel2.Controls.Add(EndRollTxt);
            panel2.Controls.Add(RollSelect);
            panel2.Controls.Add(label3);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(305, 143);
            panel2.TabIndex = 8;
            // 
            // Form7
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(305, 219);
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
        private Label label1;
        private Label label2;
        private TextBox StartRollTxt;
        private TextBox EndRollTxt;
        private Label label3;
        private ComboBox RollSelect;
        private Button CancelBtn;
        private Button OkBtn;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolTip toolTip1;
        private Label TotalPage;
        private Panel panel1;
        private Panel panel2;
    }
}
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
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 209);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(284, 26);
            statusStrip1.SizingGrip = false;
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
            label1.Location = new Point(12, 17);
            label1.Name = "label1";
            label1.Size = new Size(126, 21);
            label1.TabIndex = 1;
            label1.Text = "開始ページ番号：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 54);
            label2.Name = "label2";
            label2.Size = new Size(126, 21);
            label2.TabIndex = 2;
            label2.Text = "終了ページ番号：";
            // 
            // StartRollTxt
            // 
            StartRollTxt.Location = new Point(144, 14);
            StartRollTxt.Name = "StartRollTxt";
            StartRollTxt.Size = new Size(130, 29);
            StartRollTxt.TabIndex = 1;
            StartRollTxt.Tag = "開始ページを指定します";
            // 
            // EndRollTxt
            // 
            EndRollTxt.Location = new Point(144, 51);
            EndRollTxt.Name = "EndRollTxt";
            EndRollTxt.Size = new Size(130, 29);
            EndRollTxt.TabIndex = 2;
            EndRollTxt.Tag = "終了ページを指定します";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(48, 94);
            label3.Name = "label3";
            label3.Size = new Size(90, 21);
            label3.TabIndex = 5;
            label3.Text = "回転方法：";
            // 
            // RollSelect
            // 
            RollSelect.DropDownStyle = ComboBoxStyle.DropDownList;
            RollSelect.FormattingEnabled = true;
            RollSelect.Location = new Point(144, 91);
            RollSelect.Name = "RollSelect";
            RollSelect.Size = new Size(130, 29);
            RollSelect.TabIndex = 3;
            RollSelect.Tag = "回転方法を選択します";
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(144, 136);
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
            OkBtn.Location = new Point(38, 136);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(100, 32);
            OkBtn.TabIndex = 4;
            OkBtn.Tag = "ページ指定を確定し回転を実行します";
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // Form7
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 235);
            Controls.Add(CancelBtn);
            Controls.Add(OkBtn);
            Controls.Add(RollSelect);
            Controls.Add(label3);
            Controls.Add(EndRollTxt);
            Controls.Add(StartRollTxt);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(statusStrip1);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form7";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ページを指定して回転";
            Load += Form7_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
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
    }
}
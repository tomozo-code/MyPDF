namespace MyPDF
{
    partial class Form5
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form5));
            label1 = new Label();
            PasswordTxt = new TextBox();
            CancelBtn = new Button();
            panel1 = new Panel();
            OkBtn = new Button();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            toolTip1 = new ToolTip(components);
            NoteTxt = new TextBox();
            panel2 = new Panel();
            panel1.SuspendLayout();
            statusStrip1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(11, 8);
            label1.Name = "label1";
            label1.Size = new Size(215, 42);
            label1.TabIndex = 0;
            label1.Text = "PDFファイルは保護されています。\r\nパスワードを入力してください。";
            // 
            // PasswordTxt
            // 
            PasswordTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            PasswordTxt.ImeMode = ImeMode.Disable;
            PasswordTxt.Location = new Point(11, 59);
            PasswordTxt.Name = "PasswordTxt";
            PasswordTxt.Size = new Size(469, 29);
            PasswordTxt.TabIndex = 0;
            PasswordTxt.Tag = "パスワードを入力して下さい";
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(118, 6);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(100, 32);
            CancelBtn.TabIndex = 2;
            CancelBtn.Tag = "中止してウィンドウを閉じます";
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(OkBtn);
            panel1.Controls.Add(CancelBtn);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 226);
            panel1.Name = "panel1";
            panel1.Size = new Size(492, 50);
            panel1.TabIndex = 3;
            // 
            // OkBtn
            // 
            OkBtn.Location = new Point(12, 6);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(100, 32);
            OkBtn.TabIndex = 1;
            OkBtn.Tag = "正しいパスワードであればPDFファイルを開きます";
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 276);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(492, 26);
            statusStrip1.TabIndex = 4;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(175, 21);
            toolStripStatusLabel1.Text = "パスワードを入力して下さい";
            // 
            // NoteTxt
            // 
            NoteTxt.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            NoteTxt.Location = new Point(11, 94);
            NoteTxt.Multiline = true;
            NoteTxt.Name = "NoteTxt";
            NoteTxt.Size = new Size(469, 126);
            NoteTxt.TabIndex = 5;
            NoteTxt.TabStop = false;
            NoteTxt.Text = "閲覧パスワードで開いた場合、編集不可(閲覧モード)になります。\r\n権限パスワードで開いた場合、編集可能(編集モード)になります。\r\n権限パスワードで開いたファイルは、保護なしで保存されます。\r\n保護を設定したい場合は、\r\n「ファイル(F) - セキュリティ設定(T)...」から再設定してください。\r\n";
            // 
            // panel2
            // 
            panel2.AutoScroll = true;
            panel2.Controls.Add(label1);
            panel2.Controls.Add(PasswordTxt);
            panel2.Controls.Add(NoteTxt);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(492, 226);
            panel2.TabIndex = 6;
            // 
            // Form5
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(492, 302);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(statusStrip1);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form5";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "パスワード入力";
            Load += Form5_Load;
            panel1.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox PasswordTxt;
        private Button CancelBtn;
        private Panel panel1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolTip toolTip1;
        private Button OkBtn;
        private TextBox NoteTxt;
        private Panel panel2;
    }
}
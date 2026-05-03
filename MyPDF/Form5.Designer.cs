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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form5));
            label1 = new Label();
            PasswordTxt = new TextBox();
            OkBtn = new Button();
            CancelBtn = new Button();
            label2 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 10);
            label1.Name = "label1";
            label1.Size = new Size(215, 42);
            label1.TabIndex = 0;
            label1.Text = "PDFファイルは保護されています。\r\nパスワードを入力してください。";
            // 
            // PasswordTxt
            // 
            PasswordTxt.ImeMode = ImeMode.Disable;
            PasswordTxt.Location = new Point(14, 61);
            PasswordTxt.Name = "PasswordTxt";
            PasswordTxt.Size = new Size(458, 29);
            PasswordTxt.TabIndex = 1;
            // 
            // OkBtn
            // 
            OkBtn.Location = new Point(14, 217);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(100, 32);
            OkBtn.TabIndex = 2;
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(120, 217);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(100, 32);
            CancelBtn.TabIndex = 2;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 101);
            label2.Name = "label2";
            label2.Size = new Size(426, 105);
            label2.TabIndex = 0;
            label2.Text = "※閲覧パスワードで開いた場合、編集不可(閲覧モード)になります。\r\n　権限パスワードで開いた場合、編集可能(編集モード)になります。\r\n　権限パスワードで開いたファイルは、保護なしで保存されます。\r\n　保護を設定したい場合は、\r\n　「ファイル(F) - セキュリティ設定(T)...」から再設定してください。";
            // 
            // Form5
            // 
            AcceptButton = OkBtn;
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 261);
            Controls.Add(CancelBtn);
            Controls.Add(OkBtn);
            Controls.Add(PasswordTxt);
            Controls.Add(label2);
            Controls.Add(label1);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form5";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "保護パスワード入力";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox PasswordTxt;
        private Button OkBtn;
        private Button CancelBtn;
        private Label label2;
    }
}
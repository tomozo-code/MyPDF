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
            btnOk = new Button();
            btnCancel = new Button();
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
            // btnOk
            // 
            btnOk.Location = new Point(14, 217);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(100, 32);
            btnOk.TabIndex = 2;
            btnOk.Text = "OK";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(120, 217);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 32);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
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
            AcceptButton = btnOk;
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 261);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
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
        private Button btnOk;
        private Button btnCancel;
        private Label label2;
    }
}
namespace MyPDF
{
    partial class Form4
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form4));
            panel3 = new Panel();
            txtUserPassConfirm = new TextBox();
            txtUserPass = new TextBox();
            label2 = new Label();
            label7 = new Label();
            panel2 = new Panel();
            label6 = new Label();
            txtOwnerPass = new TextBox();
            label1 = new Label();
            txtOwnerPassConfirm = new TextBox();
            label4 = new Label();
            chkAnnot = new CheckBox();
            chkPrint = new CheckBox();
            chkCopy = new CheckBox();
            chkForm = new CheckBox();
            chkExtract = new CheckBox();
            chkEdit = new CheckBox();
            rbRC4 = new RadioButton();
            rbAES128 = new RadioButton();
            rbAES256 = new RadioButton();
            label5 = new Label();
            label3 = new Label();
            checkBox1 = new CheckBox();
            panel1 = new Panel();
            CancelBtn = new Button();
            OkBtn = new Button();
            checkBox2 = new CheckBox();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            toolTip1 = new ToolTip(components);
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // panel3
            // 
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel3.Controls.Add(txtUserPassConfirm);
            panel3.Controls.Add(txtUserPass);
            panel3.Controls.Add(label2);
            panel3.Controls.Add(label7);
            panel3.Location = new Point(10, 246);
            panel3.Name = "panel3";
            panel3.Size = new Size(460, 95);
            panel3.TabIndex = 24;
            // 
            // txtUserPassConfirm
            // 
            txtUserPassConfirm.Enabled = false;
            txtUserPassConfirm.ImeMode = ImeMode.Disable;
            txtUserPassConfirm.Location = new Point(135, 57);
            txtUserPassConfirm.Name = "txtUserPassConfirm";
            txtUserPassConfirm.Size = new Size(300, 29);
            txtUserPassConfirm.TabIndex = 12;
            txtUserPassConfirm.Tag = "確認のため、閲覧パスワードと同じパスワードを入力します";
            txtUserPassConfirm.TextChanged += UserPass_TextChanged;
            // 
            // txtUserPass
            // 
            txtUserPass.Enabled = false;
            txtUserPass.ImeMode = ImeMode.Disable;
            txtUserPass.Location = new Point(135, 21);
            txtUserPass.Name = "txtUserPass";
            txtUserPass.Size = new Size(300, 29);
            txtUserPass.TabIndex = 11;
            txtUserPass.Tag = "閲覧パスワードを設定し、PDFファイルを保護します";
            txtUserPass.TextChanged += UserPass_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(13, 24);
            label2.Name = "label2";
            label2.Size = new Size(116, 21);
            label2.TabIndex = 17;
            label2.Text = "閲覧パスワード：";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(13, 60);
            label7.Name = "label7";
            label7.Size = new Size(116, 21);
            label7.TabIndex = 16;
            label7.Text = "パスワード確認：";
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(label6);
            panel2.Controls.Add(txtOwnerPass);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(txtOwnerPassConfirm);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(chkAnnot);
            panel2.Controls.Add(chkPrint);
            panel2.Controls.Add(chkCopy);
            panel2.Controls.Add(chkForm);
            panel2.Controls.Add(chkExtract);
            panel2.Controls.Add(chkEdit);
            panel2.Location = new Point(10, 15);
            panel2.Name = "panel2";
            panel2.Size = new Size(460, 214);
            panel2.TabIndex = 23;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(13, 58);
            label6.Name = "label6";
            label6.Size = new Size(116, 21);
            label6.TabIndex = 20;
            label6.Text = "パスワード確認：";
            // 
            // txtOwnerPass
            // 
            txtOwnerPass.Enabled = false;
            txtOwnerPass.ImeMode = ImeMode.Disable;
            txtOwnerPass.Location = new Point(135, 16);
            txtOwnerPass.Name = "txtOwnerPass";
            txtOwnerPass.Size = new Size(300, 29);
            txtOwnerPass.TabIndex = 2;
            txtOwnerPass.Tag = "権限パスワードを設定し、PDFファイルを保護します";
            txtOwnerPass.TextChanged += OwnerPass_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(13, 19);
            label1.Name = "label1";
            label1.Size = new Size(116, 21);
            label1.TabIndex = 21;
            label1.Text = "権限パスワード：";
            // 
            // txtOwnerPassConfirm
            // 
            txtOwnerPassConfirm.Enabled = false;
            txtOwnerPassConfirm.ImeMode = ImeMode.Disable;
            txtOwnerPassConfirm.Location = new Point(135, 55);
            txtOwnerPassConfirm.Name = "txtOwnerPassConfirm";
            txtOwnerPassConfirm.Size = new Size(300, 29);
            txtOwnerPassConfirm.TabIndex = 3;
            txtOwnerPassConfirm.Tag = "確認のため、権限パスワードと同じパスワードを入力します";
            txtOwnerPassConfirm.TextChanged += OwnerPass_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(14, 92);
            label4.Name = "label4";
            label4.Size = new Size(115, 21);
            label4.TabIndex = 11;
            label4.Tag = "許可する項目にチェックします";
            label4.Text = "許可する項目：";
            // 
            // chkAnnot
            // 
            chkAnnot.AutoSize = true;
            chkAnnot.Enabled = false;
            chkAnnot.Location = new Point(288, 116);
            chkAnnot.Name = "chkAnnot";
            chkAnnot.Size = new Size(93, 25);
            chkAnnot.TabIndex = 7;
            chkAnnot.Tag = "注釈追加を許可します";
            chkAnnot.Text = "注釈追加";
            chkAnnot.UseVisualStyleBackColor = true;
            // 
            // chkPrint
            // 
            chkPrint.AutoSize = true;
            chkPrint.Checked = true;
            chkPrint.CheckState = CheckState.Checked;
            chkPrint.Enabled = false;
            chkPrint.Location = new Point(70, 116);
            chkPrint.Name = "chkPrint";
            chkPrint.Size = new Size(61, 25);
            chkPrint.TabIndex = 4;
            chkPrint.Tag = "印刷を許可します";
            chkPrint.Text = "印刷";
            chkPrint.UseVisualStyleBackColor = true;
            // 
            // chkCopy
            // 
            chkCopy.AutoSize = true;
            chkCopy.Enabled = false;
            chkCopy.Location = new Point(70, 147);
            chkCopy.Name = "chkCopy";
            chkCopy.Size = new Size(161, 25);
            chkCopy.TabIndex = 5;
            chkCopy.Tag = "テキスト・画像のコピーを許可します";
            chkCopy.Text = "テキスト・画像のコピー";
            chkCopy.UseVisualStyleBackColor = true;
            // 
            // chkForm
            // 
            chkForm.AutoSize = true;
            chkForm.Enabled = false;
            chkForm.Location = new Point(288, 147);
            chkForm.Name = "chkForm";
            chkForm.Size = new Size(105, 25);
            chkForm.TabIndex = 8;
            chkForm.Tag = "フォーム入力を許可します";
            chkForm.Text = "フォーム入力";
            chkForm.UseVisualStyleBackColor = true;
            // 
            // chkExtract
            // 
            chkExtract.AutoSize = true;
            chkExtract.Enabled = false;
            chkExtract.Location = new Point(288, 178);
            chkExtract.Name = "chkExtract";
            chkExtract.Size = new Size(106, 25);
            chkExtract.TabIndex = 9;
            chkExtract.Tag = "内容の抽出を許可します";
            chkExtract.Text = "内容の抽出";
            chkExtract.UseVisualStyleBackColor = true;
            // 
            // chkEdit
            // 
            chkEdit.AutoSize = true;
            chkEdit.Enabled = false;
            chkEdit.Location = new Point(70, 178);
            chkEdit.Name = "chkEdit";
            chkEdit.Size = new Size(190, 25);
            chkEdit.TabIndex = 6;
            chkEdit.Tag = "ページの挿入・削除・回転を許可します";
            chkEdit.Text = "ページの挿入・削除・回転";
            chkEdit.UseVisualStyleBackColor = true;
            // 
            // rbRC4
            // 
            rbRC4.AutoSize = true;
            rbRC4.Location = new Point(304, 372);
            rbRC4.Name = "rbRC4";
            rbRC4.Size = new Size(132, 25);
            rbRC4.TabIndex = 15;
            rbRC4.Tag = "互換性重視(脆弱性が見つかっているため非推奨)";
            rbRC4.Text = "RC4-128(互換)";
            rbRC4.UseVisualStyleBackColor = true;
            // 
            // rbAES128
            // 
            rbAES128.AutoSize = true;
            rbAES128.Location = new Point(191, 372);
            rbAES128.Name = "rbAES128";
            rbAES128.Size = new Size(88, 25);
            rbAES128.TabIndex = 14;
            rbAES128.Tag = "鍵長128bitの暗号方式で保護します";
            rbAES128.Text = "AES-128";
            rbAES128.UseVisualStyleBackColor = true;
            // 
            // rbAES256
            // 
            rbAES256.AutoSize = true;
            rbAES256.Checked = true;
            rbAES256.Location = new Point(44, 372);
            rbAES256.Name = "rbAES256";
            rbAES256.Size = new Size(130, 25);
            rbAES256.TabIndex = 13;
            rbAES256.TabStop = true;
            rbAES256.Tag = "鍵長256bitの暗号方式で保護します(推奨)";
            rbAES256.Text = "AES-256(推奨)";
            rbAES256.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(25, 400);
            label5.Name = "label5";
            label5.Size = new Size(384, 42);
            label5.TabIndex = 11;
            label5.Text = "※制限は完全ではありません\r\n　上書き保存もしくは名前を付けて保存すると適用されます";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(25, 348);
            label3.Name = "label3";
            label3.Size = new Size(90, 21);
            label3.TabIndex = 5;
            label3.Tag = "PDFファイルを保護する暗号方式を指定します";
            label3.Text = "暗号方式：";
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(25, 3);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(172, 25);
            checkBox1.TabIndex = 1;
            checkBox1.Tag = "PDFファイルをパスワードで保護します";
            checkBox1.Text = "PDFをパスワードで保護";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // panel1
            // 
            panel1.Controls.Add(CancelBtn);
            panel1.Controls.Add(OkBtn);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 455);
            panel1.Name = "panel1";
            panel1.Size = new Size(543, 50);
            panel1.TabIndex = 2;
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(118, 6);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(100, 32);
            CancelBtn.TabIndex = 16;
            CancelBtn.Tag = "編集を中止してウィンドウを閉じます";
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // OkBtn
            // 
            OkBtn.Location = new Point(12, 6);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(100, 32);
            OkBtn.TabIndex = 17;
            OkBtn.Tag = "保護設定を確定しウィンドウを閉じます";
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(25, 235);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(193, 25);
            checkBox2.TabIndex = 10;
            checkBox2.Tag = "開くときのパスワードを設定します";
            checkBox2.Text = "開くときのパスワードを設定";
            checkBox2.UseVisualStyleBackColor = true;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            // 
            // statusStrip1
            // 
            statusStrip1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 505);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(543, 26);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 25;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(242, 21);
            toolStripStatusLabel1.Text = "PDFファイルにセキュリティを設定します";
            // 
            // Form4
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(543, 531);
            Controls.Add(checkBox2);
            Controls.Add(checkBox1);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(rbRC4);
            Controls.Add(panel1);
            Controls.Add(rbAES128);
            Controls.Add(rbAES256);
            Controls.Add(label3);
            Controls.Add(label5);
            Controls.Add(statusStrip1);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form4";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "セキュリティ設定";
            Load += Form4_Load;
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel panel1;
        private Button CancelBtn;
        private Button OkBtn;
        private CheckBox checkBox1;
        private CheckBox chkCopy;
        private CheckBox chkPrint;
        private CheckBox chkForm;
        private CheckBox chkAnnot;
        private CheckBox chkEdit;
        private CheckBox chkExtract;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox txtOwnerPassConfirm;
        private Label label1;
        private TextBox txtOwnerPass;
        private Label label7;
        private TextBox txtUserPassConfirm;
        private Label label2;
        private TextBox txtUserPass;
        private RadioButton rbAES256;
        private RadioButton rbRC4;
        private RadioButton rbAES128;
        private Panel panel2;
        private Panel panel3;
        private CheckBox checkBox2;
        private StatusStrip statusStrip1;
        private ToolTip toolTip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
    }
}
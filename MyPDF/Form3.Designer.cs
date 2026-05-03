namespace MyPDF
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            OkBtn = new Button();
            labelTitle = new Label();
            labelVersion = new Label();
            labelCopyright = new Label();
            pictureBox1 = new PictureBox();
            LicenseTxtBox = new TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // OkBtn
            // 
            OkBtn.Location = new Point(314, 44);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(100, 30);
            OkBtn.TabIndex = 1;
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Location = new Point(139, 32);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(146, 21);
            labelTitle.TabIndex = 1;
            labelTitle.Text = "ともさんのPDF編集帖";
            // 
            // labelVersion
            // 
            labelVersion.AutoSize = true;
            labelVersion.Location = new Point(139, 53);
            labelVersion.Name = "labelVersion";
            labelVersion.Size = new Size(68, 21);
            labelVersion.TabIndex = 2;
            labelVersion.Text = "バージョン";
            // 
            // labelCopyright
            // 
            labelCopyright.AutoSize = true;
            labelCopyright.Location = new Point(139, 74);
            labelCopyright.Name = "labelCopyright";
            labelCopyright.Size = new Size(75, 21);
            labelCopyright.TabIndex = 3;
            labelCopyright.Text = "コピーライト";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 15);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(112, 111);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // LicenseTxtBox
            // 
            LicenseTxtBox.Location = new Point(12, 132);
            LicenseTxtBox.Multiline = true;
            LicenseTxtBox.Name = "LicenseTxtBox";
            LicenseTxtBox.Size = new Size(442, 181);
            LicenseTxtBox.TabIndex = 6;
            LicenseTxtBox.Text = resources.GetString("LicenseTxtBox.Text");
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(676, 560);
            Controls.Add(OkBtn);
            Controls.Add(LicenseTxtBox);
            Controls.Add(pictureBox1);
            Controls.Add(labelCopyright);
            Controls.Add(labelVersion);
            Controls.Add(labelTitle);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form3";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "バージョン情報";
            Load += Form3_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button OkBtn;
        private Label labelTitle;
        private Label labelVersion;
        private Label labelCopyright;
        private PictureBox pictureBox1;
        private TextBox LicenseTxtBox;
    }
}
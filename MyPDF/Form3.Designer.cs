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
            button1 = new Button();
            labelTitle = new Label();
            labelVersion = new Label();
            labelCopyright = new Label();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(107, 241);
            button1.Name = "button1";
            button1.Size = new Size(100, 30);
            button1.TabIndex = 0;
            button1.Text = "OK";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
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
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 141);
            label1.Name = "label1";
            label1.Size = new Size(314, 21);
            label1.TabIndex = 5;
            label1.Text = "本ソフトウェアは以下のライブラリを使用しています";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(35, 161);
            label2.Name = "label2";
            label2.Size = new Size(96, 21);
            label2.TabIndex = 6;
            label2.Text = "・iText(AGPL)";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(35, 182);
            label3.Name = "label3";
            label3.Size = new Size(292, 42);
            label3.TabIndex = 7;
            label3.Text = "・PdfiumViewer.core(Apache License 2.0),\r\n  based on PdfiumViewer.";
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(432, 302);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(labelCopyright);
            Controls.Add(labelVersion);
            Controls.Add(labelTitle);
            Controls.Add(button1);
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

        private Button button1;
        private Label labelTitle;
        private Label labelVersion;
        private Label labelCopyright;
        private PictureBox pictureBox1;
        private Label label1;
        private Label label2;
        private Label label3;
    }
}
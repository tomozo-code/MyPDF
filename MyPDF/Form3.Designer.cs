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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            OkBtn = new Button();
            labelTitle = new Label();
            labelVersion = new Label();
            labelCopyright = new Label();
            pictureBox1 = new PictureBox();
            LicenseTxtBox = new TextBox();
            panel1 = new Panel();
            label2 = new Label();
            linkLabel1 = new LinkLabel();
            toolTip1 = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // OkBtn
            // 
            OkBtn.Location = new Point(11, 8);
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
            labelVersion.Location = new Point(139, 59);
            labelVersion.Name = "labelVersion";
            labelVersion.Size = new Size(68, 21);
            labelVersion.TabIndex = 2;
            labelVersion.Text = "バージョン";
            // 
            // labelCopyright
            // 
            labelCopyright.AutoSize = true;
            labelCopyright.Location = new Point(139, 86);
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
            LicenseTxtBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            LicenseTxtBox.Location = new Point(12, 146);
            LicenseTxtBox.Multiline = true;
            LicenseTxtBox.Name = "LicenseTxtBox";
            LicenseTxtBox.Size = new Size(517, 258);
            LicenseTxtBox.TabIndex = 6;
            LicenseTxtBox.Text = resources.GetString("LicenseTxtBox.Text");
            // 
            // panel1
            // 
            panel1.Controls.Add(OkBtn);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 411);
            panel1.Name = "panel1";
            panel1.Size = new Size(539, 50);
            panel1.TabIndex = 7;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(139, 112);
            label2.Name = "label2";
            label2.Size = new Size(62, 21);
            label2.TabIndex = 9;
            label2.Text = "GitHub:";
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new Point(200, 112);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(292, 21);
            linkLabel1.TabIndex = 10;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "https://github.com/tomozo-code/MyPDF";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(539, 461);
            Controls.Add(linkLabel1);
            Controls.Add(label2);
            Controls.Add(panel1);
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
            StartPosition = FormStartPosition.CenterParent;
            Text = "バージョン情報";
            Load += Form3_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
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
        private Panel panel1;
        private Label label2;
        private LinkLabel linkLabel1;
        private ToolTip toolTip1;
    }
}
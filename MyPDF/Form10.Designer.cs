namespace MyPDF
{
    partial class Form10
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form10));
            label1 = new Label();
            InsertFileName = new TextBox();
            toolTip1 = new ToolTip(components);
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            label2 = new Label();
            InsertPlace = new ComboBox();
            label3 = new Label();
            setPage = new TextBox();
            CancelBtn = new Button();
            OkBtn = new Button();
            TotalPage = new Label();
            panel1 = new Panel();
            panel2 = new Panel();
            statusStrip1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(11, 10);
            label1.Name = "label1";
            label1.Size = new Size(128, 21);
            label1.TabIndex = 0;
            label1.Text = "挿入するファイル：";
            // 
            // InsertFileName
            // 
            InsertFileName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            InsertFileName.Location = new Point(145, 7);
            InsertFileName.Name = "InsertFileName";
            InsertFileName.Size = new Size(238, 29);
            InsertFileName.TabIndex = 70;
            InsertFileName.Text = "挿入するファイル名が表示される";
            // 
            // statusStrip1
            // 
            statusStrip1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 187);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(395, 26);
            statusStrip1.TabIndex = 71;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(195, 21);
            toolStripStatusLabel1.Text = "ファイルからページを挿入します";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 93);
            label2.Name = "label2";
            label2.Size = new Size(115, 21);
            label2.TabIndex = 72;
            label2.Text = "挿入する場所：";
            // 
            // InsertPlace
            // 
            InsertPlace.DropDownStyle = ComboBoxStyle.DropDownList;
            InsertPlace.FormattingEnabled = true;
            InsertPlace.Location = new Point(145, 88);
            InsertPlace.Name = "InsertPlace";
            InsertPlace.Size = new Size(121, 29);
            InsertPlace.TabIndex = 2;
            InsertPlace.Tag = "挿入する場所を指定します";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(45, 51);
            label3.Name = "label3";
            label3.Size = new Size(94, 21);
            label3.TabIndex = 74;
            label3.Text = "ページ指定：";
            // 
            // setPage
            // 
            setPage.Location = new Point(145, 46);
            setPage.Name = "setPage";
            setPage.Size = new Size(100, 29);
            setPage.TabIndex = 1;
            setPage.Tag = "挿入先のページを指定します";
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(128, 7);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(112, 35);
            CancelBtn.TabIndex = 4;
            CancelBtn.Tag = "中止してウィンドウを閉じます";
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // OkBtn
            // 
            OkBtn.Location = new Point(9, 7);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(112, 35);
            OkBtn.TabIndex = 3;
            OkBtn.Tag = "ページ指定を確定し挿入を実行します";
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // TotalPage
            // 
            TotalPage.AutoSize = true;
            TotalPage.Location = new Point(260, 49);
            TotalPage.Name = "TotalPage";
            TotalPage.Size = new Size(72, 21);
            TotalPage.TabIndex = 77;
            TotalPage.Text = "/ 総ページ";
            // 
            // panel1
            // 
            panel1.Controls.Add(OkBtn);
            panel1.Controls.Add(CancelBtn);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 137);
            panel1.Name = "panel1";
            panel1.Size = new Size(395, 50);
            panel1.TabIndex = 78;
            // 
            // panel2
            // 
            panel2.AutoScroll = true;
            panel2.Controls.Add(label1);
            panel2.Controls.Add(InsertFileName);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(TotalPage);
            panel2.Controls.Add(InsertPlace);
            panel2.Controls.Add(setPage);
            panel2.Controls.Add(label3);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(395, 137);
            panel2.TabIndex = 79;
            // 
            // Form10
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(395, 213);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(statusStrip1);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form10";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ページを挿入";
            Load += Form10_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox InsertFileName;
        private ToolTip toolTip1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private Label label2;
        private ComboBox InsertPlace;
        private Label label3;
        private TextBox setPage;
        private Button CancelBtn;
        private Button OkBtn;
        private Label TotalPage;
        private Panel panel1;
        private Panel panel2;
    }
}
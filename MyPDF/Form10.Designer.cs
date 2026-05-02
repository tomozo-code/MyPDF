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
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(128, 21);
            label1.TabIndex = 0;
            label1.Text = "挿入するファイル：";
            // 
            // InsertFileName
            // 
            InsertFileName.Location = new Point(134, 6);
            InsertFileName.Name = "InsertFileName";
            InsertFileName.Size = new Size(216, 29);
            InsertFileName.TabIndex = 70;
            InsertFileName.Text = "挿入するファイル名が表示される";
            // 
            // statusStrip1
            // 
            statusStrip1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 185);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(384, 26);
            statusStrip1.SizingGrip = false;
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
            label2.Location = new Point(25, 92);
            label2.Name = "label2";
            label2.Size = new Size(115, 21);
            label2.TabIndex = 72;
            label2.Text = "挿入する場所：";
            // 
            // InsertPlace
            // 
            InsertPlace.DropDownStyle = ComboBoxStyle.DropDownList;
            InsertPlace.FormattingEnabled = true;
            InsertPlace.Location = new Point(134, 89);
            InsertPlace.Name = "InsertPlace";
            InsertPlace.Size = new Size(121, 29);
            InsertPlace.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(46, 50);
            label3.Name = "label3";
            label3.Size = new Size(94, 21);
            label3.TabIndex = 74;
            label3.Text = "ページ指定：";
            // 
            // setPage
            // 
            setPage.Location = new Point(134, 47);
            setPage.Name = "setPage";
            setPage.Size = new Size(100, 29);
            setPage.TabIndex = 1;
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(165, 137);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(112, 35);
            CancelBtn.TabIndex = 76;
            CancelBtn.Tag = "中止してウィンドウを閉じます";
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // OkBtn
            // 
            OkBtn.Location = new Point(46, 137);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(112, 35);
            OkBtn.TabIndex = 75;
            OkBtn.Tag = "ページ指定を確定し削除を実行します";
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // TotalPage
            // 
            TotalPage.AutoSize = true;
            TotalPage.Location = new Point(249, 50);
            TotalPage.Name = "TotalPage";
            TotalPage.Size = new Size(72, 21);
            TotalPage.TabIndex = 77;
            TotalPage.Text = "/ 総ページ";
            // 
            // Form10
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(384, 211);
            Controls.Add(TotalPage);
            Controls.Add(CancelBtn);
            Controls.Add(OkBtn);
            Controls.Add(setPage);
            Controls.Add(label3);
            Controls.Add(InsertPlace);
            Controls.Add(label2);
            Controls.Add(statusStrip1);
            Controls.Add(InsertFileName);
            Controls.Add(label1);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form10";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ページを挿入";
            Load += Form10_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
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
    }
}
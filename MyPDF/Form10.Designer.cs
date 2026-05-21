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
            toolTip1 = new ToolTip(components);
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            CancelBtn = new Button();
            OkBtn = new Button();
            panel1 = new Panel();
            panel2 = new Panel();
            groupBox2 = new GroupBox();
            label2 = new Label();
            TotalPage = new Label();
            InsertPlace = new ComboBox();
            setPage = new TextBox();
            label3 = new Label();
            groupBox1 = new GroupBox();
            InsTotalPageLabel = new Label();
            label5 = new Label();
            ExtractTxt = new TextBox();
            label6 = new Label();
            label1 = new Label();
            InsertFileName = new TextBox();
            statusStrip1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 355);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(384, 26);
            statusStrip1.TabIndex = 71;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(195, 21);
            toolStripStatusLabel1.Text = "ファイルからページを挿入します";
            // 
            // CancelBtn
            // 
            CancelBtn.Location = new Point(128, 7);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(112, 35);
            CancelBtn.TabIndex = 7;
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
            OkBtn.TabIndex = 6;
            OkBtn.Tag = "ページ指定を確定し挿入を実行します";
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(OkBtn);
            panel1.Controls.Add(CancelBtn);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 305);
            panel1.Name = "panel1";
            panel1.Size = new Size(384, 50);
            panel1.TabIndex = 78;
            // 
            // panel2
            // 
            panel2.AutoScroll = true;
            panel2.Controls.Add(groupBox2);
            panel2.Controls.Add(groupBox1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(384, 305);
            panel2.TabIndex = 79;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(TotalPage);
            groupBox2.Controls.Add(InsertPlace);
            groupBox2.Controls.Add(setPage);
            groupBox2.Controls.Add(label3);
            groupBox2.Location = new Point(9, 157);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(363, 118);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "挿入先";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(15, 78);
            label2.Name = "label2";
            label2.Size = new Size(115, 21);
            label2.TabIndex = 80;
            label2.Text = "挿入する場所：";
            // 
            // TotalPage
            // 
            TotalPage.AutoSize = true;
            TotalPage.Location = new Point(251, 34);
            TotalPage.Name = "TotalPage";
            TotalPage.Size = new Size(72, 21);
            TotalPage.TabIndex = 82;
            TotalPage.Text = "/ 総ページ";
            // 
            // InsertPlace
            // 
            InsertPlace.DropDownStyle = ComboBoxStyle.DropDownList;
            InsertPlace.FormattingEnabled = true;
            InsertPlace.Location = new Point(136, 73);
            InsertPlace.Name = "InsertPlace";
            InsertPlace.Size = new Size(121, 29);
            InsertPlace.TabIndex = 5;
            InsertPlace.Tag = "挿入する場所を指定します";
            // 
            // setPage
            // 
            setPage.Location = new Point(136, 31);
            setPage.Name = "setPage";
            setPage.Size = new Size(100, 29);
            setPage.TabIndex = 4;
            setPage.Tag = "挿入先のページを指定します";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(36, 36);
            label3.Name = "label3";
            label3.Size = new Size(94, 21);
            label3.TabIndex = 81;
            label3.Text = "ページ指定：";
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(InsTotalPageLabel);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(ExtractTxt);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(InsertFileName);
            groupBox1.Location = new Point(9, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(363, 139);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "挿入するファイルの設定";
            // 
            // InsTotalPageLabel
            // 
            InsTotalPageLabel.AutoSize = true;
            InsTotalPageLabel.Location = new Point(242, 76);
            InsTotalPageLabel.Name = "InsTotalPageLabel";
            InsTotalPageLabel.Size = new Size(72, 21);
            InsTotalPageLabel.TabIndex = 82;
            InsTotalPageLabel.Text = "/ 総ページ";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(86, 105);
            label5.Name = "label5";
            label5.Size = new Size(150, 21);
            label5.TabIndex = 85;
            label5.Text = "入力方法：1,2,3,5-9";
            // 
            // ExtractTxt
            // 
            ExtractTxt.Location = new Point(136, 73);
            ExtractTxt.Name = "ExtractTxt";
            ExtractTxt.Size = new Size(100, 29);
            ExtractTxt.TabIndex = 2;
            ExtractTxt.Tag = "挿入するファイルのページを指定します(入力方法：1,2,3,5-9)";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(36, 76);
            label6.Name = "label6";
            label6.Size = new Size(94, 21);
            label6.TabIndex = 83;
            label6.Text = "ページ指定：";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 32);
            label1.Name = "label1";
            label1.Size = new Size(128, 21);
            label1.TabIndex = 71;
            label1.Text = "挿入するファイル：";
            // 
            // InsertFileName
            // 
            InsertFileName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            InsertFileName.Location = new Point(137, 32);
            InsertFileName.Name = "InsertFileName";
            InsertFileName.Size = new Size(212, 29);
            InsertFileName.TabIndex = 72;
            InsertFileName.Text = "挿入するファイル名が表示される";
            // 
            // Form10
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(384, 381);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(statusStrip1);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form10";
            StartPosition = FormStartPosition.CenterParent;
            Text = "ページを指定して挿入";
            Load += Form10_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ToolTip toolTip1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private Button CancelBtn;
        private Button OkBtn;
        private Panel panel1;
        private Panel panel2;
        private GroupBox groupBox1;
        private Label label1;
        private TextBox InsertFileName;
        private Label InsTotalPageLabel;
        private Label label5;
        private TextBox ExtractTxt;
        private Label label6;
        private GroupBox groupBox2;
        private Label label2;
        private Label TotalPage;
        private ComboBox InsertPlace;
        private TextBox setPage;
        private Label label3;
    }
}
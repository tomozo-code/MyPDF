namespace MyPDF
{
    partial class Form13
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form13));
            toolTip1 = new ToolTip(components);
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            panel1 = new Panel();
            groupBox1 = new GroupBox();
            MarginRight = new TextBox();
            MarginLeft = new TextBox();
            MarginBottom = new TextBox();
            MarginTop = new TextBox();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            Place = new ComboBox();
            label2 = new Label();
            PdfImageSize = new ComboBox();
            label1 = new Label();
            panel2 = new Panel();
            OkBtn = new Button();
            statusStrip1.SuspendLayout();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 315);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 16, 0);
            statusStrip1.Size = new Size(334, 26);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(158, 21);
            toolStripStatusLabel1.Text = "変換サイズを設定します";
            // 
            // panel1
            // 
            panel1.Controls.Add(groupBox1);
            panel1.Controls.Add(Place);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(PdfImageSize);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(334, 265);
            panel1.TabIndex = 1;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(MarginRight);
            groupBox1.Controls.Add(MarginLeft);
            groupBox1.Controls.Add(MarginBottom);
            groupBox1.Controls.Add(MarginTop);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Location = new Point(16, 106);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(260, 127);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "余白(0～40mmで設定)";
            // 
            // MarginRight
            // 
            MarginRight.Location = new Point(177, 77);
            MarginRight.Name = "MarginRight";
            MarginRight.Size = new Size(50, 29);
            MarginRight.TabIndex = 6;
            MarginRight.Text = "0";
            MarginRight.TextAlign = HorizontalAlignment.Right;
            // 
            // MarginLeft
            // 
            MarginLeft.Location = new Point(58, 77);
            MarginLeft.Name = "MarginLeft";
            MarginLeft.Size = new Size(50, 29);
            MarginLeft.TabIndex = 5;
            MarginLeft.Text = "0";
            MarginLeft.TextAlign = HorizontalAlignment.Right;
            // 
            // MarginBottom
            // 
            MarginBottom.Location = new Point(177, 28);
            MarginBottom.Name = "MarginBottom";
            MarginBottom.Size = new Size(50, 29);
            MarginBottom.TabIndex = 4;
            MarginBottom.Text = "0";
            MarginBottom.TextAlign = HorizontalAlignment.Right;
            // 
            // MarginTop
            // 
            MarginTop.Location = new Point(58, 28);
            MarginTop.Name = "MarginTop";
            MarginTop.Size = new Size(50, 29);
            MarginTop.TabIndex = 3;
            MarginTop.Text = "0";
            MarginTop.TextAlign = HorizontalAlignment.Right;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(129, 80);
            label6.Name = "label6";
            label6.Size = new Size(42, 21);
            label6.TabIndex = 3;
            label6.Text = "右：";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(11, 80);
            label5.Name = "label5";
            label5.Size = new Size(42, 21);
            label5.TabIndex = 2;
            label5.Text = "左：";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(129, 31);
            label4.Name = "label4";
            label4.Size = new Size(42, 21);
            label4.TabIndex = 1;
            label4.Text = "下：";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(11, 31);
            label3.Name = "label3";
            label3.Size = new Size(42, 21);
            label3.TabIndex = 0;
            label3.Text = "上：";
            // 
            // Place
            // 
            Place.DropDownStyle = ComboBoxStyle.DropDownList;
            Place.FormattingEnabled = true;
            Place.Location = new Point(112, 61);
            Place.Name = "Place";
            Place.Size = new Size(121, 29);
            Place.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(48, 64);
            label2.Name = "label2";
            label2.Size = new Size(58, 21);
            label2.TabIndex = 2;
            label2.Text = "配置：";
            // 
            // PdfImageSize
            // 
            PdfImageSize.DropDownStyle = ComboBoxStyle.DropDownList;
            PdfImageSize.FormattingEnabled = true;
            PdfImageSize.Location = new Point(112, 17);
            PdfImageSize.Name = "PdfImageSize";
            PdfImageSize.Size = new Size(121, 29);
            PdfImageSize.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 20);
            label1.Name = "label1";
            label1.Size = new Size(94, 21);
            label1.TabIndex = 0;
            label1.Text = "変換サイズ：";
            // 
            // panel2
            // 
            panel2.Controls.Add(OkBtn);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 265);
            panel2.Name = "panel2";
            panel2.Size = new Size(334, 50);
            panel2.TabIndex = 2;
            // 
            // OkBtn
            // 
            OkBtn.Location = new Point(12, 6);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(112, 35);
            OkBtn.TabIndex = 7;
            OkBtn.Tag = "ページ指定を確定し挿入を実行します";
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // Form13
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(334, 341);
            ControlBox = false;
            Controls.Add(panel1);
            Controls.Add(panel2);
            Controls.Add(statusStrip1);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form13";
            StartPosition = FormStartPosition.CenterParent;
            Text = "画像PDF変換設定";
            Load += Form13_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolTip toolTip1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private Panel panel1;
        private Panel panel2;
        private Button OkBtn;
        private ComboBox PdfImageSize;
        private Label label1;
        private ComboBox Place;
        private Label label2;
        private GroupBox groupBox1;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private TextBox MarginRight;
        private TextBox MarginLeft;
        private TextBox MarginBottom;
        private TextBox MarginTop;
    }
}
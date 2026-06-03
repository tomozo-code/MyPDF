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
            groupBox3 = new GroupBox();
            radioRight = new RadioButton();
            radioLeft = new RadioButton();
            radioBottom = new RadioButton();
            radioTop = new RadioButton();
            radioCenter = new RadioButton();
            groupBox2 = new GroupBox();
            radioA4Landscape = new RadioButton();
            radioA4Portrait = new RadioButton();
            radioOriginalSize = new RadioButton();
            groupBox1 = new GroupBox();
            MarginRight = new TextBox();
            MarginLeft = new TextBox();
            MarginBottom = new TextBox();
            MarginTop = new TextBox();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            panel2 = new Panel();
            OkBtn = new Button();
            statusStrip1.SuspendLayout();
            panel1.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 677);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 16, 0);
            statusStrip1.Size = new Size(443, 26);
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
            panel1.Controls.Add(groupBox3);
            panel1.Controls.Add(groupBox2);
            panel1.Controls.Add(groupBox1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(443, 627);
            panel1.TabIndex = 1;
            // 
            // groupBox3
            // 
            groupBox3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox3.Controls.Add(radioRight);
            groupBox3.Controls.Add(radioLeft);
            groupBox3.Controls.Add(radioBottom);
            groupBox3.Controls.Add(radioTop);
            groupBox3.Controls.Add(radioCenter);
            groupBox3.Location = new Point(16, 85);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(415, 118);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "配置";
            // 
            // radioRight
            // 
            radioRight.AutoSize = true;
            radioRight.Location = new Point(183, 49);
            radioRight.Name = "radioRight";
            radioRight.Size = new Size(73, 25);
            radioRight.TabIndex = 4;
            radioRight.TabStop = true;
            radioRight.Text = "右詰め";
            radioRight.UseVisualStyleBackColor = true;
            // 
            // radioLeft
            // 
            radioLeft.AutoSize = true;
            radioLeft.Location = new Point(7, 49);
            radioLeft.Name = "radioLeft";
            radioLeft.Size = new Size(73, 25);
            radioLeft.TabIndex = 2;
            radioLeft.TabStop = true;
            radioLeft.Text = "左詰め";
            radioLeft.UseVisualStyleBackColor = true;
            // 
            // radioBottom
            // 
            radioBottom.AutoSize = true;
            radioBottom.Location = new Point(98, 80);
            radioBottom.Name = "radioBottom";
            radioBottom.Size = new Size(73, 25);
            radioBottom.TabIndex = 3;
            radioBottom.TabStop = true;
            radioBottom.Text = "下詰め";
            radioBottom.UseVisualStyleBackColor = true;
            // 
            // radioTop
            // 
            radioTop.AutoSize = true;
            radioTop.Location = new Point(98, 18);
            radioTop.Name = "radioTop";
            radioTop.Size = new Size(73, 25);
            radioTop.TabIndex = 1;
            radioTop.TabStop = true;
            radioTop.Text = "上詰め";
            radioTop.UseVisualStyleBackColor = true;
            // 
            // radioCenter
            // 
            radioCenter.AutoSize = true;
            radioCenter.Checked = true;
            radioCenter.Location = new Point(98, 49);
            radioCenter.Name = "radioCenter";
            radioCenter.Size = new Size(60, 25);
            radioCenter.TabIndex = 0;
            radioCenter.TabStop = true;
            radioCenter.Text = "中央";
            radioCenter.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(radioA4Landscape);
            groupBox2.Controls.Add(radioA4Portrait);
            groupBox2.Controls.Add(radioOriginalSize);
            groupBox2.Location = new Point(16, 12);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(415, 67);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "変換サイズ";
            // 
            // radioA4Landscape
            // 
            radioA4Landscape.AutoSize = true;
            radioA4Landscape.Location = new Point(203, 28);
            radioA4Landscape.Name = "radioA4Landscape";
            radioA4Landscape.Size = new Size(63, 25);
            radioA4Landscape.TabIndex = 2;
            radioA4Landscape.Text = "A4横";
            radioA4Landscape.UseVisualStyleBackColor = true;
            // 
            // radioA4Portrait
            // 
            radioA4Portrait.AutoSize = true;
            radioA4Portrait.Location = new Point(119, 28);
            radioA4Portrait.Name = "radioA4Portrait";
            radioA4Portrait.Size = new Size(63, 25);
            radioA4Portrait.TabIndex = 1;
            radioA4Portrait.Text = "A4縦";
            radioA4Portrait.UseVisualStyleBackColor = true;
            // 
            // radioOriginalSize
            // 
            radioOriginalSize.AutoSize = true;
            radioOriginalSize.Checked = true;
            radioOriginalSize.Location = new Point(23, 28);
            radioOriginalSize.Name = "radioOriginalSize";
            radioOriginalSize.Size = new Size(80, 25);
            radioOriginalSize.TabIndex = 0;
            radioOriginalSize.TabStop = true;
            radioOriginalSize.Text = "元サイズ";
            radioOriginalSize.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(MarginRight);
            groupBox1.Controls.Add(MarginLeft);
            groupBox1.Controls.Add(MarginBottom);
            groupBox1.Controls.Add(MarginTop);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Location = new Point(16, 209);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(415, 107);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "余白(0～40mmで設定)";
            // 
            // MarginRight
            // 
            MarginRight.Location = new Point(177, 66);
            MarginRight.Name = "MarginRight";
            MarginRight.Size = new Size(50, 29);
            MarginRight.TabIndex = 3;
            MarginRight.Text = "0";
            MarginRight.TextAlign = HorizontalAlignment.Right;
            // 
            // MarginLeft
            // 
            MarginLeft.Location = new Point(58, 66);
            MarginLeft.Name = "MarginLeft";
            MarginLeft.Size = new Size(50, 29);
            MarginLeft.TabIndex = 2;
            MarginLeft.Text = "0";
            MarginLeft.TextAlign = HorizontalAlignment.Right;
            // 
            // MarginBottom
            // 
            MarginBottom.Location = new Point(177, 28);
            MarginBottom.Name = "MarginBottom";
            MarginBottom.Size = new Size(50, 29);
            MarginBottom.TabIndex = 1;
            MarginBottom.Text = "0";
            MarginBottom.TextAlign = HorizontalAlignment.Right;
            // 
            // MarginTop
            // 
            MarginTop.Location = new Point(58, 28);
            MarginTop.Name = "MarginTop";
            MarginTop.Size = new Size(50, 29);
            MarginTop.TabIndex = 0;
            MarginTop.Text = "0";
            MarginTop.TextAlign = HorizontalAlignment.Right;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(129, 69);
            label6.Name = "label6";
            label6.Size = new Size(42, 21);
            label6.TabIndex = 3;
            label6.Text = "右：";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(11, 69);
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
            // panel2
            // 
            panel2.Controls.Add(OkBtn);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 627);
            panel2.Name = "panel2";
            panel2.Size = new Size(443, 50);
            panel2.TabIndex = 2;
            // 
            // OkBtn
            // 
            OkBtn.Location = new Point(12, 6);
            OkBtn.Name = "OkBtn";
            OkBtn.Size = new Size(112, 35);
            OkBtn.TabIndex = 0;
            OkBtn.Tag = "ページ指定を確定し挿入を実行します";
            OkBtn.Text = "OK";
            OkBtn.UseVisualStyleBackColor = true;
            OkBtn.Click += OkBtn_Click;
            // 
            // Form13
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(443, 703);
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
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
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
        private GroupBox groupBox3;
        private GroupBox groupBox2;
        private RadioButton radioA4Landscape;
        private RadioButton radioA4Portrait;
        private RadioButton radioOriginalSize;
        private RadioButton radioBottom;
        private RadioButton radioLeft;
        private RadioButton radioRight;
        private RadioButton radioTop;
        private RadioButton radioCenter;
        private GroupBox groupBox1;
        private TextBox MarginRight;
        private TextBox MarginLeft;
        private TextBox MarginBottom;
        private TextBox MarginTop;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
    }
}
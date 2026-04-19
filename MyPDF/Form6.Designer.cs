namespace MyPDF
{
    partial class Form6
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form6));
            label1 = new Label();
            label2 = new Label();
            comboBox1 = new ComboBox();
            colorDialog1 = new ColorDialog();
            btnOk = new Button();
            btnCancel = new Button();
            btnColor = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 18);
            label1.Name = "label1";
            label1.Size = new Size(74, 21);
            label1.TabIndex = 0;
            label1.Text = "スタイル：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 57);
            label2.Name = "label2";
            label2.Size = new Size(74, 21);
            label2.TabIndex = 1;
            label2.Text = "色選択：";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(92, 15);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(161, 29);
            comboBox1.TabIndex = 2;
            // 
            // btnOk
            // 
            btnOk.Location = new Point(30, 117);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(100, 32);
            btnOk.TabIndex = 3;
            btnOk.Text = "OK";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(153, 117);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 32);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnColor
            // 
            btnColor.Location = new Point(92, 51);
            btnColor.Name = "btnColor";
            btnColor.Size = new Size(38, 32);
            btnColor.TabIndex = 4;
            btnColor.UseVisualStyleBackColor = true;
            btnColor.Click += btnColor_Click;
            // 
            // Form6
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 161);
            Controls.Add(btnColor);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Controls.Add(comboBox1);
            Controls.Add(label2);
            Controls.Add(label1);
            Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form6";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "しおりのプロパティ";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private ComboBox comboBox1;
        private ColorDialog colorDialog1;
        private Button btnOk;
        private Button btnCancel;
        private Button btnColor;
    }
}
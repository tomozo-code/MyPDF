using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

// ==============================
// 処理中ですを出すフォーム
// ==============================

namespace MyPDF
{
    public partial class Form15 : Form
    {
        public Form15(string title, string msg)
        {
            InitializeComponent();

            // フォームサイズ
            this.Width = 300;
            this.Height = 150;
            this.MinimumSize = new Size(300, 150);

            // フォームタイトル
            this.Text = title;
            // メッセージ
            label1.Text = msg;

            this.StartPosition = FormStartPosition.CenterScreen;

        }

        public void SetProgress(int value)
        {
            Action update = () =>
            {
                progressBar1.Value = value;
                labelPercent.Text = $"{value}%";
            };

            if (InvokeRequired)
                Invoke(update);
            else
                update();
        }

    }
}

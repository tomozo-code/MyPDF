using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;


// ==============================
// バージョン情報フォーム
// ==============================

namespace MyPDF
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            // レイアウト
            //this.AutoSize = true;
            //this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Width = 500;
            this.Height = 400;
            this.MinimumSize = new Size(400, 300);
            //this.AutoScaleDimensions = new SizeF(96F, 96F);

            // PDF変換用TextBox
            LicenseTxtBox.ReadOnly = true;
            LicenseTxtBox.BorderStyle = BorderStyle.FixedSingle;
            LicenseTxtBox.BackColor = this.BackColor;
            LicenseTxtBox.TabStop = false;

            // EnterキーをOKボタンに割り当て
            this.AcceptButton = OkBtn;
            //  EscキーをOKボタンに割り当て
            this.CancelButton = OkBtn;

        }

        // ==============================
        // Formをロードしたとき 
        // ==============================
        private void Form3_Load(object sender, EventArgs e)
        {

            // アセンブリ情報を取得して表示
            var name = Assembly.GetExecutingAssembly().GetName().Name;
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version?.ToString() ?? "不明";

            labelTitle.Text = "ともさんのPDF編集帖";
            labelVersion.Text = $"Version: {version}";
            labelCopyright.Text = "Copyright(c) 2026 ともさん";

        }

        // ==============================
        // OKボタンを押したとき 
        // ==============================
        private void OkBtn_Click(object sender, EventArgs e)
        {
            // 現在のフォームを閉じる
            this.Close();

        }
    }
}

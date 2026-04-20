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
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Width = 400;
            this.Height = 200;


        }

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

        private void button1_Click(object sender, EventArgs e)
        {
            // 現在のフォームを閉じる
            this.Close();

        }
    }
}

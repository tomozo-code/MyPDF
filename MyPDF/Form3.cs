using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


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
            this.Width = 555;
            this.Height = 500;
            this.MinimumSize = new Size(300, 300);
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

            linkLabel1.Tag = "既定のブラウザで " + linkLabel1.Text + " を開きます";


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

            // ツールチップ設定(通常コントロール用:Tagに表示させたい内容を書く)
            SetTooltipAll(this);


        }

        // ==============================
        // OKボタンを押したとき 
        // ==============================
        private void OkBtn_Click(object sender, EventArgs e)
        {
            // 現在のフォームを閉じる
            this.Close();

        }

        // ==============================
        // ラベルをクリックしてGitHubを開く 
        // ==============================
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // ブラウザを開くためのURLを指定
            string url = linkLabel1.Text;

            // 既定のブラウザで開く
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }


        // ==============================
        // マウスONで説明の実行(通常コントロール) Tagに書いたもの 
        // ==============================
        private void SetTooltipAll(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl.Tag != null)
                {
                    // ツールチップにヒント
                    toolTip1.SetToolTip(ctrl, ctrl.Tag.ToString());
                }

                // 子コントロールも再帰
                if (ctrl.HasChildren)
                {
                    SetTooltipAll(ctrl);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MyPDF
{
    public partial class Form11 : Form
    {
        private string? toolHintTxt = null;

        // 総ページ数
        private int maxPage;


        public Form11(int nowPage, int maxPage)
        {
            InitializeComponent();

            // フォームサイズ
            this.Width = 340;

            this.Height = 320;
            // 総ページ数をセット
            this.maxPage = maxPage;

            // 今表示しているページをセット
            StartMoveTxt.Text = nowPage.ToString();

            // 総ページ
            TotalPage.Text = "/ " + maxPage.ToString();

            // コンボボックス初期化
            MovePlace.Items.AddRange(new string[]
            {
                "前",  "後"
            });
            MovePlace.SelectedIndex = 1;


            toolHintTxt = "指定したページを移動します";

            toolTip1.InitialDelay = 500;   // 表示までの時間(ms)
            toolTip1.AutoPopDelay = 5000;  // 表示時間
            toolTip1.ReshowDelay = 100;    // 次の表示まで

        }

        // ==============================
        // Formをロードしたとき 
        // ==============================
        private void Form11_Load(object sender, EventArgs e)
        {
            // ツールチップ設定(通常コントロール用:Tagに表示させたい内容を書く)
            SetTooltipAll(this);

        }

        // ==============================
        // OKボタン 
        // ==============================
        private void OkBtn_Click(object sender, EventArgs e)
        {

        }

        // ==============================
        // Cancelボタン 
        // ==============================
        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();

        }

        // ==============================
        // マウスONで説明(通常コントロール) Tagに書く 
        // ==============================
        private void Control_MouseEnter(object? sender, EventArgs e)
        {
            if (sender is Control ctrl)
            {
                toolStripStatusLabel1.Text = ctrl.Tag?.ToString() ?? "";
            }
        }

        // ==============================
        // マウス離脱(通常コントロール)
        // ==============================
        private void Control_MouseLeave(object? sender, EventArgs e)
        {
            // 戻す
            toolStripStatusLabel1.Text = toolHintTxt;
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
                    // ツールバーにヒント
                    ctrl.MouseEnter += Control_MouseEnter;
                    ctrl.MouseLeave += Control_MouseLeave;
                    // ツールチップにもヒント
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

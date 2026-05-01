using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

// ==============================
// 指定ページの削除フォーム
// ==============================


namespace MyPDF
{
    public partial class Form8 : Form
    {
        // ツールチップに表示するヒント文字列
        private string? toolHintTxt = null;

        // 削除設定（外から取得用）
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        // 総ページ数
        private int maxPage;


        public Form8(int maxPage)
        {
            InitializeComponent();

            // フォームサイズ
            this.Width = 300;
            this.Height = 200;

            // 総ページ数をセット
            this.maxPage = maxPage;

            // 開始ページ初期値
            StartDelTxt.Text = "1";

            // 終了ページ初期値
            EndDelTxt.Text = maxPage.ToString();

            // 総ページ
            TotalPage.Text = "/ " + maxPage.ToString();

            toolHintTxt = "ページを指定して削除します";

            toolTip1.InitialDelay = 500;   // 表示までの時間(ms)
            toolTip1.AutoPopDelay = 5000;  // 表示時間
            toolTip1.ReshowDelay = 100;    // 次の表示まで



        }
        // ==============================
        // Formをロードしたとき 
        // ==============================

        private void Form8_Load(object sender, EventArgs e)
        {
            // ツールチップ設定(通常コントロール用:Tagに表示させたい内容を書く)
            SetTooltipAll(this);

        }

        // ==============================
        // OKボタン 
        // ==============================
        private void OkBtn_Click(object sender, EventArgs e)
        {
            int start, end;

            if (!int.TryParse(StartDelTxt.Text, out start) || !int.TryParse(EndDelTxt.Text, out end))
            {
                MessageBox.Show("数値を入力してください。", "ページ入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (start < 1 || end < 1)
            {
                MessageBox.Show("1以上の値を入力してください。", "ページ入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (start > end)
            {
                MessageBox.Show("開始ページは終了ページ以下にしてください。", "ページ入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (start > maxPage || end > maxPage)
            {
                MessageBox.Show("総ページ数以下の値を入力してください。", "ページ入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            // 削除後0ページを回避
            int deleteCount = end - start + 1;
            int remain = maxPage - deleteCount;

            if (remain <= 0)
            {
                MessageBox.Show("最低1ページは残す必要があります。", "削除エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            StartPage = start;
            EndPage = end;

            this.DialogResult = DialogResult.OK;
            this.Close();

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

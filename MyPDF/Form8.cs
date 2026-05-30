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
        public string ExtractText { get; private set; } = "";

        // 今のページ
        private int nowPage;
        // 総ページ数
        private int maxPage;


        public Form8(int nowPage, int maxPage)
        {
            InitializeComponent();

            // フォームサイズ
            this.Width = 400;
            this.Height = 220;
            this.MinimumSize = new Size(300, 200);
            //this.AutoScaleDimensions = new SizeF(96F, 96F);

            // 今のページセット
            this.nowPage = nowPage;
            // 総ページ数をセット
            this.maxPage = maxPage;
            // 今のページをセット
            ExtractTxt.Text = nowPage.ToString();

            // 総ページ
            TotalPage.Text = "/ " + maxPage.ToString();

            toolHintTxt = "ページを指定して削除します";

            toolTip1.InitialDelay = 500;   // 表示までの時間(ms)
            toolTip1.AutoPopDelay = 5000;  // 表示時間
            toolTip1.ReshowDelay = 100;    // 次の表示まで

            // EnterキーをOKボタンに割り当て
            this.AcceptButton = OkBtn;
            //  EscキーをCancelボタンに割り当て
            this.CancelButton = CancelBtn;


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

            string text = ExtractTxt.Text.Trim();

            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("削除するページを入力してください。", "ページ入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                // 構文チェック(PageRangeHelper.csを呼ぶ)
                var pages = PageRangeHelper.ParsePageRanges(text, maxPage);

                // 全ページ削除禁止
                if (pages.Count >= maxPage)
                {
                    MessageBox.Show("最低1ページは残してください。", "ページ入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ExtractText = text;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ページ入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


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

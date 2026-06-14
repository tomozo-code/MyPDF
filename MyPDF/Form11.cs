using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

// ==============================
// 指定ページの移動フォーム
// ==============================

namespace MyPDF
{
    public partial class Form11 : Form
    {
        private string? toolHintTxt = null;
        // 移動設定（外から取得用）
        public string ExtractText { get; private set; } = "";

        public int TargetPage { get; private set; }
        // 前 or 後
        public bool MoveBefore { get; private set; }

        // 総ページ数
        private int maxPage;

        public Form11(string PageText, int maxPage)
        {
            InitializeComponent();

            // フォームサイズ
            this.Width = 400;
            this.Height = 350;
            this.MinimumSize = new Size(300, 200);

            // 総ページ数をセット
            this.maxPage = maxPage;

            // 今のページをセット
            ExtractTxt.Text = PageText;

            // 総ページ
            TotalPage.Text = "/ " + maxPage.ToString();
            TotalPage2.Text = "/ " + maxPage.ToString();

            toolHintTxt = "指定したページを移動します";

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
        private void Form11_Load(object sender, EventArgs e)
        {
            // ツールチップ設定(通常コントロール用:Tagに表示させたい内容を書く)
            SetTooltipAll(this);
            ActiveControl = ExtractTxt;

        }

        // ==============================
        // OKボタン 
        // ==============================
        private void OkBtn_Click(object sender, EventArgs e)
        {

            int target;

            if (!int.TryParse(TargetPageTxt.Text, out target))
            {
                MessageBox.Show("移動先ページは数値を入力してください。", "ページ入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (target < 1)
            {
                MessageBox.Show("移動先ページは1以上の値を入力してください。", "ページ入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (target > maxPage)
            {
                MessageBox.Show("移動先ページは総ページ数以下の値を入力してください。", "ページ入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string text = ExtractTxt.Text.Trim();

            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("ページを入力してください。", "ページ入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                // 構文チェック(PageRangeHelper.csを呼ぶ)
                PageRangeHelper.ParsePageRanges(text, maxPage);

                ExtractText = text;

                TargetPage = target;

                // 前 or 後(Prev=0(true)、Next=1(false))
                MoveBefore = Prev.Checked;

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

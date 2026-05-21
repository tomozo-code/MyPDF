using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;


// ==============================
// 指定ページの置換フォーム
// ==============================

namespace MyPDF
{
    public partial class Form12 : Form
    {
        private string? toolHintTxt = null;

        // 挿入するファイルのページ指定用（外から取得用）
        public string ExtractText { get; private set; } = "";

        // 置換するPDF側
        public int SourceStartPage { get; private set; }

        // 現在PDF側
        public int TargetStartPage { get; private set; }
        public int TargetEndPage { get; private set; }
        // 総ページ数
        private int maxPage;
        // 置換するファイルの総ページ数
        private int InsPage;

        public string ReplaceText => ExtractTxt.Text;


        public Form12(string NowFileName, string ReplacementFile, int nowPage, int maxPage, int InsTotalPage)
        {
            InitializeComponent();

            // フォームサイズ
            this.Width = 400;
            this.Height = 420;
            this.MinimumSize = new Size(300, 200);
            //this.AutoScaleDimensions = new SizeF(96F, 96F);

            // 置換するファイル名をセット
            KaeFileName.Text = Path.GetFileName(ReplacementFile);

            // 置換するファイル名用TextBox
            KaeFileName.ReadOnly = true;
            KaeFileName.BorderStyle = BorderStyle.None;
            KaeFileName.BackColor = this.BackColor;
            KaeFileName.TabStop = false;

            // 今開いているPDFのページ
            txtPage.Text = "1";

            // 挿入するファイルのページ指定をセット
            this.InsPage = InsTotalPage;
            InsTotalPageLabel.Text = "/ " + InsPage.ToString();


            // 現在表示しているファイル名をセット
            nowFileName.Text = Path.GetFileName(NowFileName);

            // 現在表示しているファイル名用TextBox
            nowFileName.ReadOnly = true;
            nowFileName.BorderStyle = BorderStyle.None;
            nowFileName.BackColor = this.BackColor;
            nowFileName.TabStop = false;

            // 総ページ数をセット
            this.maxPage = maxPage;
            ExtractTxt.Text = "1-" + maxPage.ToString();
            // 総ページ
            TotalPage.Text = "/ " + maxPage.ToString();



            toolHintTxt = "ファイルからページを置換します";

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
        private void Form12_Load(object sender, EventArgs e)
        {
            // ツールチップ設定(通常コントロール用:Tagに表示させたい内容を書く)
            SetTooltipAll(this);

        }

        // ==============================
        // OKボタン 
        // ==============================
        private void OkBtn_Click(object sender, EventArgs e)
        {
            int sourceStart;

            if (!int.TryParse(txtPage.Text, out sourceStart))
            {
                MessageBox.Show("開始ページは数値を入力してください。", "ページ入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (sourceStart < 1)
            {
                MessageBox.Show("開始ページは1以上の値を入力してください。", "ページ入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (sourceStart > maxPage)
            {
                MessageBox.Show("開始ページは総ページ数以下の値を入力してください。", "ページ入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 現在表示しているPDFファイル
            string text = ExtractTxt.Text.Trim();

            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("置換先のページを入力してください。", "ページ入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                SourceStartPage = sourceStart;

                // 構文チェック(PageRangeHelper.csを呼ぶ)
                var range = PageRangeHelper.ParseReplaceRange(text, maxPage);

                TargetStartPage = range.Start;
                TargetEndPage = range.End;

                // 現在PDFで置換するページ数
                int targetCount = TargetEndPage - TargetStartPage + 1;

                // 置換PDFの残りページ数
                int remainSourceCount = InsPage - sourceStart + 1;

                // 実際に置換可能な数
                int actualCount = Math.Min(targetCount, remainSourceCount);

                // 現在PDF側の終了位置補正
                TargetEndPage = TargetStartPage + actualCount - 1;

                // 置換PDF側の終了位置
                int sourceEnd = sourceStart + actualCount - 1;

                // 実際にコピーするページ文字列
                ExtractText = $"{sourceStart}-{sourceEnd}";



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

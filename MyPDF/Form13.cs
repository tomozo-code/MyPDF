using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MyPDF
{
    public partial class Form13 : Form
    {
        private string? toolHintTxt = null;


        // 画像PDFのサイズ
        public int PdfImageMode { get; private set; }
        // 場所
        public int PdfPlace { get; private set; }
        // 余白上
        public int PdfMarginTop { get; private set; }
        // 余白下
        public int PdfMarginBottom { get; private set; }
        // 余白左
        public int PdfMarginLeft { get; private set; }
        // 余白右
        public int PdfMarginRight { get; private set; }


        public Form13()
        {
            InitializeComponent();

            // フォームサイズ
            this.Width = 380;
            this.Height = 380;
            this.MinimumSize = new Size(250, 280);

            // 変換サイズ初期化
            PdfImageSize.Items.AddRange(new string[]
            {
                "A4縦",  "A4横", "元サイズ"
            });
            PdfImageSize.SelectedIndex = 2;

            // 配置初期化
            Place.Items.AddRange(new string[]
            {
                "中央",  "上詰め", "下詰め", "左詰め", "右詰め"
            });
            Place.SelectedIndex = 0;

            PdfMarginTop = 0;
            PdfMarginBottom = 0;
            PdfMarginLeft = 0;
            PdfMarginRight = 0;


            toolHintTxt = "変換サイズを設定します";

            toolTip1.InitialDelay = 500;   // 表示までの時間(ms)
            toolTip1.AutoPopDelay = 5000;  // 表示時間
            toolTip1.ReshowDelay = 100;    // 次の表示まで

            // EnterキーをOKボタンに割り当て
            this.AcceptButton = OkBtn;


        }

        // ==============================
        // Formをロードしたとき 
        // ==============================
        private void Form13_Load(object sender, EventArgs e)
        {
            // ツールチップ設定(通常コントロール用:Tagに表示させたい内容を書く)
            SetTooltipAll(this);

        }

        // ==============================
        // OKボタン 
        // ==============================
        private void OkBtn_Click(object sender, EventArgs e)
        {
            int top, bottom, left, right;

            if (!int.TryParse(MarginTop.Text, out top) || !int.TryParse(MarginBottom.Text, out bottom) ||
                !int.TryParse(MarginLeft.Text, out left) || !int.TryParse(MarginRight.Text, out right))
            {
                MessageBox.Show("数値を入力してください。", "余白入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (top < 0 || bottom < 0 || left < 0 || right < 0)
            {
                MessageBox.Show("0以上の値を入力してください。(0～40の範囲)", "余白入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (top > 40 || bottom > 40 || left > 40 || right > 40)
            {
                MessageBox.Show("40以下の値を入力してください。(0～40の範囲)", "余白入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //余白
            PdfMarginTop = top;
            PdfMarginBottom = bottom;
            PdfMarginLeft = left;
            PdfMarginRight = right;

            // 変換サイズ
            PdfImageMode = PdfImageSize.SelectedIndex;

            // 配置
            PdfPlace = Place.SelectedIndex;

            this.DialogResult = DialogResult.OK;
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

using iText.Kernel.Colors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DrawingColor = System.Drawing.Color;



// ==============================
// しおりプロパティ設定フォーム
// ==============================


namespace MyPDF
{
    public partial class Form6 : Form
    {

        private ColorDialog colorDialog;

        // しおり名
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public String SelectedBmTitle { get; set; }

        // ページ番号
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedPage { get; set; }

        // 色設定
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DrawingColor SelectedColor { get; set; }

        // 文字スタイル
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FontStyle SelectedStyle { get; set; }

        // ツールチップに表示するヒント文字列
        private string? toolHintTxt = null;

        // 総ページ数
        private int maxPage;



        public Form6(String currentBmTitle, int currentPage, DrawingColor currentColor, FontStyle currentStyle, int maxPage)
        {
            InitializeComponent();

            // フォームサイズ
            this.Width = 400;
            this.Height = 350;
            this.MinimumSize = new Size(300, 300);
            //this.AutoScaleDimensions = new SizeF(96F, 96F);

            colorDialog = new ColorDialog();

            // 初期値セット
            SelectedBmTitle = currentBmTitle;
            SelectedPage = currentPage;
            SelectedColor = currentColor;
            SelectedStyle = currentStyle;

            // 現在のしおり名
            BmTitleTxtBox.Text = SelectedBmTitle;
            // ページ番号
            PageNoTxtBox.Text = SelectedPage.ToString();
            // コンボボックス初期化
            comboBox1.Items.AddRange(new string[]
            {
                "標準", "ボールド", "イタリック", "ボールドイタリック"
            });

            comboBox1.SelectedIndex = StyleToIndex(currentStyle);

            // 初期表示
            btnColor.BackColor = SelectedColor;

            // 総ページ番号を保持
            this.maxPage = maxPage;

            TotalPageLabel.Text = "/ " + maxPage.ToString();

            // 色16進用TextBox
            ColorTxtBox1.ReadOnly = true;
            ColorTxtBox1.BorderStyle = BorderStyle.None;
            ColorTxtBox1.BackColor = this.BackColor;
            ColorTxtBox1.TabStop = false;

            // 色を16進に変換
            ColorTxtBox1.Text = $"#{SelectedColor.R:X2}{SelectedColor.G:X2}{SelectedColor.B:X2}";

            // 色RGB用TextBox
            ColorTxtBox2.ReadOnly = true;
            ColorTxtBox2.BorderStyle = BorderStyle.None;
            ColorTxtBox2.BackColor = this.BackColor;
            ColorTxtBox2.TabStop = false;

            // 色RGB
            ColorTxtBox2.Text = $"{SelectedColor.R},{SelectedColor.G},{SelectedColor.B}";


            toolHintTxt = "しおりのプロパティを設定します";

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
        private void Form6_Load(object sender, EventArgs e)
        {
            // ツールチップ設定(通常コントロール用:Tagに表示させたい内容を書く)
            SetTooltipAll(this);

        }

        // ==============================
        // 色選択
        // ==============================
        private void btnColor_Click(object sender, EventArgs e)
        {
            colorDialog.Color = SelectedColor;

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                SelectedColor = colorDialog.Color;

                // ボタンの色でプレビュー
                btnColor.BackColor = SelectedColor;
                // 色を16進に変換
                ColorTxtBox1.Text = $"#{SelectedColor.R:X2}{SelectedColor.G:X2}{SelectedColor.B:X2}";
                // 色RGB
                ColorTxtBox2.Text = $"{SelectedColor.R},{SelectedColor.G},{SelectedColor.B}";


            }
        }

        // ==============================
        // スタイルインデックスをセット
        // ==============================
        private int StyleToIndex(FontStyle style)
        {
            if (style.HasFlag(FontStyle.Bold) && style.HasFlag(FontStyle.Italic))
                return 3;
            if (style.HasFlag(FontStyle.Bold))
                return 1;
            if (style.HasFlag(FontStyle.Italic))
                return 2;

            return 0;
        }

        // ==============================
        // フォントスタイルをセット
        // ==============================
        private FontStyle IndexToStyle(int index)
        {
            switch (index)
            {
                case 1: return FontStyle.Bold;
                case 2: return FontStyle.Italic;
                case 3: return FontStyle.Bold | FontStyle.Italic;
                default: return FontStyle.Regular;
            }
        }

        // ==============================
        // OKボタンをクリックしたとき
        // ==============================
        private void OkBtn_Click(object sender, EventArgs e)
        {

            // しおり名チェック（空白・スペースのみNG）
            if (string.IsNullOrWhiteSpace(BmTitleTxtBox.Text))
            {
                MessageBox.Show("しおり名を入力してください。", "しおり名入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // フォーカスを指定
                BmTitleTxtBox.Focus();
                return;
            }
            // 文字列の先頭と末尾の空白除去
            SelectedBmTitle = BmTitleTxtBox.Text.Trim();

            if (!int.TryParse(PageNoTxtBox.Text, out int page))
            {
                MessageBox.Show("ページ番号が不正です", "ページ番号入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ページ範囲チェック
            if (page < 1 || page > maxPage)
            {
                MessageBox.Show(
                    $"ページ番号は 1 ～ {maxPage} の範囲で入力してください。",
                    "ページ範囲エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                // フォーカスを指定
                PageNoTxtBox.Focus();
                return;
            }

            SelectedPage = page;

            SelectedStyle = IndexToStyle(comboBox1.SelectedIndex);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // ==============================
        // Cancelボタンをクリックしたとき
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

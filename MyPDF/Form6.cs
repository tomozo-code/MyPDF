using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;



// ==============================
// しおりプロパティ設定フォーム
// ==============================


namespace MyPDF
{
    public partial class Form6 : Form
    {

        private ColorDialog colorDialog;

        // 色設定
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color SelectedColor { get; set; }

        // 文字スタイル
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FontStyle SelectedStyle { get; set; }


        public Form6(Color currentColor, FontStyle currentStyle)
        {
            InitializeComponent();

            // フォームサイズ
            this.Width = 300;
            this.Height = 200;

            colorDialog = new ColorDialog();

            // 初期値セット
            SelectedColor = currentColor;
            SelectedStyle = currentStyle;

            // コンボボックス初期化
            comboBox1.Items.AddRange(new string[]
            {
                "標準", "ボールド", "イタリック", "ボールドイタリック"
            });

            comboBox1.SelectedIndex = StyleToIndex(currentStyle);

            // 初期表示
            btnColor.BackColor = SelectedColor;

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
        private void btnOk_Click(object sender, EventArgs e)
        {
            SelectedStyle = IndexToStyle(comboBox1.SelectedIndex);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // ==============================
        // Cancelボタンをクリックしたとき
        // ==============================
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

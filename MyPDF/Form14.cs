using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

// ==============================
// PDFを画像に変換設定フォーム
// ==============================

namespace MyPDF
{
    public partial class Form14 : Form
    {
        private string? toolHintTxt = null;

        // 移動設定（外から取得用）
        // 変換するページ
        public string ExtractText { get; private set; } = "";
        // 変換するファイル名
        public string ImgFileName { get; private set; } = "";
        // 解像度
        public int ImgDpi { get; private set; }
        // 画像形式
        public string ImgType { get; private set; } = "";
        // true:カラー or false:グレースケール
        public bool IsColor { get; private set; }


        // ファイル名
        private string fileName;

        // 総ページ数
        private int maxPage;


        public Form14(int nowPage, string fileName, int maxPage)
        {
            InitializeComponent();

            // フォームサイズ
            this.Width = 400;
            this.Height = 500;
            this.MinimumSize = new Size(300, 200);

            // ファイル名
            this.fileName = fileName;

            // 総ページ数をセット
            this.maxPage = maxPage;

            // 今のページをセット
            ExtractTxt.Text = nowPage.ToString();

            // 総ページ
            TotalPage.Text = "/ " + maxPage.ToString();

            // ファイル名
            FileNameTxtBox.Text = fileName + "_Image";

            // コンボボックス初期化
            DpiComboBox.Items.AddRange(new string[]
            {
                "75",  "96", "100", "200", "300", "400", "500", "600"
            });
            DpiComboBox.SelectedIndex = 4;

            // コンボボックス初期化
            ImageTypeComboBox.Items.AddRange(new string[]
            {
                "jpg",  "png", "bmp", "tif"
            });
            ImageTypeComboBox.SelectedIndex = 1;


            toolHintTxt = "PDFを画像に変換します";

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
        private void Form14_Load(object sender, EventArgs e)
        {
            // ツールチップ設定(通常コントロール用:Tagに表示させたい内容を書く)
            SetTooltipAll(this);

        }

        // ==============================
        // OKボタン 
        // ==============================
        private void OkBtn_Click(object sender, EventArgs e)
        {
            string FName = FileNameTxtBox.Text;
            // null、空チェック
            if (string.IsNullOrEmpty(FName) || string.IsNullOrWhiteSpace(FName))
            {
                MessageBox.Show("ファイル名を入力してください。", "ファイル名入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // ファイル名に使えない文字チェック
            char[] invalidChars = Path.GetInvalidFileNameChars();
            if (FName.IndexOfAny(invalidChars) > 0)
            {
                MessageBox.Show("ファイル名に使用できない文字が含まれています。", "ファイル名入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                // ページ
                ExtractText = ExtractTxt.Text.Trim();

                // ファイル名
                ImgFileName = FileNameTxtBox.Text.Trim();

                // 解像度
                ImgDpi = int.Parse(DpiComboBox.Text);

                // 画像形式
                ImgType = ImageTypeComboBox.Text;

                // 色(true:カラー or false:グレースケール)
                IsColor = radioColor.Checked;

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

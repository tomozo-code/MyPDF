using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

// ==============================
// ファイル挿入フォーム
// ==============================


namespace MyPDF
{
    public partial class Form10 : Form
    {

        private string? toolHintTxt = null;

        // 挿入設定（外から取得用）
        // 挿入する場所のページ番号
        public int TargetPage { get; private set; }
        // 前 or 後
        public bool InsertBefore { get; private set; }  
        // 総ページ数
        private int maxPage;


        public Form10(string InsertFile, int InsertPage, int maxPage)
        {
            InitializeComponent();

            // フォームサイズ
            this.Width = 400;
            this.Height = 350;
            this.MinimumSize = new Size(220, 200);
            //this.AutoScaleDimensions = new SizeF(96F, 96F);

            // 挿入するファイル名をセット
            InsertFileName.Text = Path.GetFileName(InsertFile);

            // 挿入するファイル名用TextBox
            InsertFileName.ReadOnly = true;
            InsertFileName.BorderStyle = BorderStyle.None;
            InsertFileName.BackColor = this.BackColor;
            InsertFileName.TabStop = false;


            // 総ページ数をセット
            this.maxPage = maxPage;

            // コンボボックス初期化
            InsertPlace.Items.AddRange(new string[]
            {
                "前",  "後"
            });
            InsertPlace.SelectedIndex = 1;

            // 総ページ
            TotalPage.Text = "/ " + maxPage.ToString();

            // ページ指定の初期値
            setPage.Text = InsertPage.ToString();



            toolHintTxt = "ファイルからページを挿入します";

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
        private void Form10_Load(object sender, EventArgs e)
        {
            // ツールチップ設定(通常コントロール用:Tagに表示させたい内容を書く)
            SetTooltipAll(this);

        }

        // ==============================
        // OKボタン 
        // ==============================
        private void OkBtn_Click(object sender, EventArgs e)
        {
            int page;

            if (!int.TryParse(setPage.Text, out page))
            {
                MessageBox.Show("数値を入力してください。", "ページ入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (page < 1)
            {
                MessageBox.Show("1以上の値を入力してください。", "ページ入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (page > maxPage)
            {
                MessageBox.Show("総ページ数以下の値を入力してください。", "ページ入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            // ターゲットページ番号
            TargetPage = page;

            // 前 or 後
            InsertBefore = (InsertPlace.SelectedIndex == 0);

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

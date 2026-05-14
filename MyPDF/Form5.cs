using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


// ==============================
// 保護パスワード入力フォーム
// ==============================

namespace MyPDF
{
    public partial class Form5 : Form
    {
        // ツールチップに表示するヒント文字列
        private string? toolHintTxt = null;

        public string? Password { get; private set; }

        public Form5(string PassMessage)
        {
            InitializeComponent();

            this.Width = 650;
            this.Height = 400;
            this.MinimumSize = new Size(400, 200);
            //this.AutoScaleDimensions = new SizeF(96F, 96F);

            // PDF変換用TextBox
            NoteTxt.ReadOnly = true;
            NoteTxt.BorderStyle = BorderStyle.FixedSingle;
            NoteTxt.BackColor = this.BackColor;
            NoteTxt.TabStop = false;

            NoteTxt.Text = PassMessage;

            toolHintTxt = "パスワードを入力してください";

            toolTip1.InitialDelay = 500;   // 表示までの時間(ms)
            toolTip1.AutoPopDelay = 5000;  // 表示時間
            toolTip1.ReshowDelay = 100;    // 次の表示まで



            // EnterキーをOKボタンに割り当て
            this.AcceptButton = OkBtn;
            //  EscキーをCancelボタンに割り当て
            this.CancelButton = CancelBtn;

        }

        // ==============================
        // OKボタンをクリックしたとき
        // ==============================
        private void OkBtn_Click(object sender, EventArgs e)
        {
            Password = PasswordTxt.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        // ==============================
        // Cancelボタンをクリックしたとき
        // ==============================
        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Password = null;
            this.DialogResult = DialogResult.Cancel;
            this.Close();

        }

        // ==============================
        // Formをロードしたとき 
        // ==============================
        private void Form5_Load(object sender, EventArgs e)
        {
            // ツールチップ設定(通常コントロール用:Tagに表示させたい内容を書く)
            SetTooltipAll(this);

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

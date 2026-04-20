using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

// ==============================
// セキュリティ設定フォーム
// ==============================

namespace MyPDF
{
    public partial class Form4 : Form
    {
        public SecuritySettings Settings { get; private set; }


        public Form4(SecuritySettings? settings)
        {
            InitializeComponent();

            // フォームサイズ
            this.Width = 500;
            this.Height = 550;

            // null対策
            settings ??= new SecuritySettings();

            // コピー（参照渡し事故防止）
            Settings = new SecuritySettings
            {
                Check_Owner = settings.Check_Owner,
                Check_User = settings.Check_User,
                UserPassword = settings.UserPassword,
                OwnerPassword = settings.OwnerPassword,
                Permissions = settings.Permissions,
                Encryption = settings.Encryption,
                Check_chkPrint = settings.Check_chkPrint,
                Check_chkCopy = settings.Check_chkCopy,
                Check_chkEdit = settings.Check_chkEdit,
                Check_chkAnnot = settings.Check_chkAnnot,
                Check_chkForm = settings.Check_chkForm,
                Check_chkExtract = settings.Check_chkExtract
            };




        }

        // ==============================
        // Cancelボタンを押したとき
        // ==============================
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();

        }

        // ==============================
        // PDFをパスワードで保護のチェック
        // ==============================
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            foreach (Control item in panel2.Controls)
            {

                if (item is CheckBox)
                {
                    item.Enabled = checkBox1.Checked;
                }

                if (item is RadioButton)
                {
                    item.Enabled = checkBox1.Checked;
                }

            }

            if (!checkBox1.Checked && checkBox2.Checked)
            {
                checkBox2.Checked = false;
            }


            txtOwnerPass.Enabled = checkBox1.Checked;
            txtOwnerPassConfirm.Enabled = checkBox1.Checked;

            if (!checkBox1.Checked)
            {
                //txtOwnerPass.Text = "";
                txtOwnerPassConfirm.Text = "";
            }

        }

        // ==============================
        // 開くときのパスワード設定のチェック
        // ==============================
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control item in panel3.Controls)
            {

                if (item is TextBox)
                {
                    item.Enabled = checkBox2.Checked;

                }

            }

            if (checkBox2.Checked && !checkBox1.Checked)
            {
                checkBox1.Checked = true;
            }


            if (!checkBox2.Checked)
            {
                //txtUserPass.Text = "";
                txtUserPassConfirm.Text = "";
            }


        }


        // ==============================
        // OKボタンを押したとき
        // ==============================
        private void btnOK_Click(object sender, EventArgs e)
        {
            // パスワード一致チェック
            if (txtOwnerPass.Text != txtOwnerPassConfirm.Text)
            {
                MessageBox.Show("権限パスワードが一致しません。", "確認");
                return;
            }

            if (txtUserPass.Text != txtUserPassConfirm.Text)
            {
                MessageBox.Show("閲覧パスワードが一致しません。", "確認");
                return;
            }

            // Owner未入力チェック
            if (checkBox1.Checked)
            {

                if (string.IsNullOrEmpty(txtOwnerPass.Text))
                {
                    MessageBox.Show("権限パスワードは必須です。", "確認");
                    return;
                }
            }

            // 開くパスのみは禁止
            if (checkBox2.Checked && !checkBox1.Checked)
            {
                MessageBox.Show(
                    "閲覧パスワードのみの設定はできません。" + Environment.NewLine + "権限パスワードも設定してください。",
                    "確認"
                );
                return;
            }


            if (txtUserPass.Text == txtOwnerPass.Text && !string.IsNullOrEmpty(txtUserPass.Text))
            {
                MessageBox.Show("権限パスワードと閲覧のパスワードは同じにできません。", "確認");
                return;
            }



            // 設定をまとめる
            SecuritySettings settings = new SecuritySettings();

            settings.UserPassword = checkBox2.Checked ? txtUserPass.Text : "";
            settings.OwnerPassword = checkBox1.Checked ? txtOwnerPass.Text : "";

            // 暗号方式
            if (rbAES256.Checked)
                // AES-256(推奨) = 1
                settings.Encryption = EncryptionConstants.ENCRYPTION_AES_256;
            else if (rbAES128.Checked)
                // AES-128 = 3
                settings.Encryption = EncryptionConstants.ENCRYPTION_AES_128;
            else
                // RC4-128(互換) = 2
                settings.Encryption = EncryptionConstants.STANDARD_ENCRYPTION_128;

            // 権限
            int perm;

            if (checkBox1.Checked)
            {

                // "|="はビットOR(論理和)
                // イメージ(例)
                // ALLOW_PRINTING        = 0001
                // ALLOW_COPY            = 0010
                // ALLOW_MODIFY_CONTENTS = 0100
                // チェック入れたとき
                // 0000 | 0001 = 0001
                // 0001 | 0010 = 0011
                // 最終結果：0011（印刷 + コピー許可）

                // 権限パスあり → チェックのみ許可
                perm = 0;

                if (chkPrint.Checked)
                    // 印刷許可
                    perm |= EncryptionConstants.ALLOW_PRINTING;

                if (chkCopy.Checked)
                    // コピー許可(テキスト・画像コピーOK)
                    perm |= EncryptionConstants.ALLOW_COPY;

                if (chkEdit.Checked)
                    // 編集許可(ページ編集・削除などOK)
                    perm |= EncryptionConstants.ALLOW_MODIFY_CONTENTS;

                if (chkAnnot.Checked)
                    // 注釈(コメント・ハイライトOK)
                    perm |= EncryptionConstants.ALLOW_MODIFY_ANNOTATIONS;

                if (chkForm.Checked)
                    // フォーム入力
                    perm |= EncryptionConstants.ALLOW_FILL_IN;

                if (chkExtract.Checked)
                    // 抽出(アクセシビリティ)(スクリーンリーダーによる読み取りOK)
                    perm |= EncryptionConstants.ALLOW_SCREENREADERS;
            }
            else
            {

                // 権限パスなし → フル許可
                perm =
                    EncryptionConstants.ALLOW_PRINTING |
                    EncryptionConstants.ALLOW_COPY |
                    EncryptionConstants.ALLOW_MODIFY_CONTENTS |
                    EncryptionConstants.ALLOW_MODIFY_ANNOTATIONS |
                    EncryptionConstants.ALLOW_FILL_IN |
                    EncryptionConstants.ALLOW_SCREENREADERS |
                    EncryptionConstants.ALLOW_ASSEMBLY |
                    EncryptionConstants.ALLOW_DEGRADED_PRINTING;
            }

            settings.Permissions = perm;

            settings.Check_Owner = checkBox1.Checked;
            settings.Check_User = checkBox2.Checked;
            settings.Check_chkPrint = chkPrint.Checked;
            settings.Check_chkCopy = chkCopy.Checked;
            settings.Check_chkEdit = chkEdit.Checked;
            settings.Check_chkAnnot = chkAnnot.Checked;
            settings.Check_chkForm = chkForm.Checked;
            settings.Check_chkExtract = chkExtract.Checked;

            // デバッグ出力確認
            Debug.WriteLine("-----SecuritySettings------------------------");
            Debug.WriteLine("権限パス: " + txtOwnerPass.Text);
            Debug.WriteLine("開くパス: " + txtUserPass.Text);

            Debug.WriteLine("PDFをパスワードで保護: " + settings.Check_Owner);
            Debug.WriteLine("開くときのパスワードを設定: " + settings.Check_User);

            Debug.WriteLine("印刷: " + settings.Check_chkPrint);
            Debug.WriteLine("テキスト・画像のコピー: " + settings.Check_chkCopy);
            Debug.WriteLine("ページ挿入・削除・回転: " + settings.Check_chkEdit);
            Debug.WriteLine("注釈追加: " + settings.Check_chkAnnot);
            Debug.WriteLine("フォーム入力: " + settings.Check_chkForm);
            Debug.WriteLine("内容の抽出: " + settings.Check_chkExtract);

            Debug.WriteLine("制限許可: " + settings.Permissions);
            Debug.WriteLine("暗号方式: " + settings.Encryption);


            // 親へ渡す
            Settings = settings;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // ==============================
        // 入力許可判定(PDFをパスワードで保護のチェックと管理パス一致)
        // ==============================
        private void OwnerPass_TextChanged(object sender, EventArgs e)
        {
            // チェックOFFなら全部無効
            //if (!checkBox1.Checked)
            //{
            //    txtUserPass.Enabled = false;
            //    txtUserPassConfirm.Enabled = false;
            //    return;
            //}

            // 権限パス一致判定
            bool isMatch =
                !string.IsNullOrEmpty(txtOwnerPass.Text) &&
                txtOwnerPass.Text == txtOwnerPassConfirm.Text;

            //txtUserPass.Enabled = isMatch;
            //txtUserPassConfirm.Enabled = isMatch;
            //btnOK.Enabled = isMatch;
        }

        // ==============================
        // 入力許可判定(PDFを開くときのパスワードで保護のチェックと管理パス一致)
        // ==============================
        private void UserPass_TextChanged(object sender, EventArgs e)
        {

            // 開くパス一致判定
            bool isMatch =
                !string.IsNullOrEmpty(txtUserPass.Text) &&
                txtUserPass.Text == txtUserPassConfirm.Text;

        }

        // ==============================
        // Form4を開いたとき
        // ==============================
        private void Form4_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = Settings.Check_Owner;
            txtOwnerPass.Text = Settings.OwnerPassword;
            chkPrint.Checked = Settings.Check_chkPrint;
            chkCopy.Checked = Settings.Check_chkCopy;
            chkEdit.Checked = Settings.Check_chkEdit;
            chkAnnot.Checked = Settings.Check_chkAnnot;
            chkForm.Checked = Settings.Check_chkForm;
            chkExtract.Checked = Settings.Check_chkExtract;
            checkBox2.Checked = Settings.Check_User;
            txtUserPass.Text = Settings.UserPassword;

            switch (Settings.Encryption)
                {
                // AES-256(推奨) = 1
                case EncryptionConstants.ENCRYPTION_AES_256:
                    rbAES256.Checked = true;
                    break;
                // AES-128 = 3
                case EncryptionConstants.ENCRYPTION_AES_128:
                    rbAES128.Checked = true;
                    break;
                // RC4-128(互換) = 2
                case EncryptionConstants.STANDARD_ENCRYPTION_128:
                    rbRC4.Checked = true;
                    break;
                }

        }
    }
}

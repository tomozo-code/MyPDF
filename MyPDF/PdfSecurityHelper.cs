using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.Text;
using ITextDoc = iText.Kernel.Pdf.PdfDocument;

// ==============================
// PDFを開いて権限確認(開く・挿入・置換用)
// パス入力、PDFオープン、権限確認、暗号方式取得
// ==============================

namespace MyPDF
{
    public static class PdfSecurityHelper
    {
        public static PdfOpenResult CheckPdfPermission(string pdfPath, string message, Func<string?> passwordProvider)
        {
            // 入力されたパス保持
            string? password = null;
            // パスワード入力ダイアログへ表示する説明文
            string PassMessage = message;

            // 無限ループ開始(パスワード間違い時に「再入力」を繰り返すため)
            while (true)
            {
                // finallyで安全にCloseするため先に宣言
                PdfReader? reader = null;
                ITextDoc? pdf = null;

                try
                {
                    if (password == null)
                    {
                        // まずはパス無しで開く
                        reader = new PdfReader(pdfPath);
                    }
                    else
                    {
                        // パス入力済みなら
                        var props = new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(password));
                        // パス付きで開く
                        reader = new PdfReader(pdfPath, props);
                    }

                    // PDFを実際に開く
                    pdf = new ITextDoc(reader);
                    // 管理者(制限パス)で開いてる true:制限パス false:以外)
                    bool isOwner = reader.IsOpenedWithFullPermission();
                    // 暗号化されてる？(true:暗号化、false:なし)
                    bool isEncrypted = reader.IsEncrypted();

                    // OwnerパスのみPDF対策(パス未入力 かつ 暗号化PDF かつ 編集制限なし)
                    if (password == null && isEncrypted && !isOwner)
                    {
                        // Ownerパスだけ設定されているPDF
                        // → 強制的にパス入力させる
                        // 一旦閉じる
                        pdf.Close();
                        reader.Close();
                        // パスワード入力ダイアログ表示
                        //password = ShowPasswordDialog();
                        //password = ShowPasswordDialog(PassMessage);
                        password = passwordProvider();

                        if (password == null)
                        {
                            // キャンセルされたら開く失敗として終了
                            return new PdfOpenResult
                            {
                                Success = false // 失敗
                            };

                        }
                        // 再トライ
                        continue;
                    }
                    // 一旦閉じる
                    pdf.Close();
                    reader.Close();

                    // パス有無判定
                    ReaderProperties props2 = string.IsNullOrEmpty(password)
                        ? new ReaderProperties() // パスなし
                        : new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(password)); // パスあり

                    // 呼び出し元へ情報を返す
                    return new PdfOpenResult
                    {
                        Success = true, // 成功
                        IsOwner = isOwner, // 編集権限ある？(true:ある、false:ない)
                        IsEncrypted = isEncrypted, // 暗号化されてる？(true:ある、false:ない)
                        Permissions = reader.GetPermissions(), // PDF権限情報
                        CryptoMode = reader.GetCryptoMode(), // AES256など暗号方式
                        Password = password, // 入力されたパス
                        ReaderProps = props2 // 後で再利用する認証情報
                    };
                }
                // iText専用 パス違う時だけここへ来る
                catch (iText.Kernel.Exceptions.BadPasswordException)
                {
                    // パスワード再入力(ダイアログ表示)
                    //password = ShowPasswordDialog();
                    //password = ShowPasswordDialog(PassMessage);
                    password = passwordProvider();

                    if (password == null)
                    {
                        // キャンセルされたら開く失敗として終了
                        return new PdfOpenResult
                        {
                            Success = false // 失敗
                        };
                    }
                }
                catch (Exception ex)　// エラー捕捉
                {
#if DEBUG
                    MessageBox.Show(ex.ToString());
#else
                    MessageBox.Show("PDFファイルを開けませんでした。", "PDFオープン失敗", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
#endif
                    // 開く失敗として終了
                    return new PdfOpenResult
                    {
                        Success = false // 失敗
                    };
                }
                // 途中でエラーが出てもファイルロック、メモリリーク防止
                finally
                {
                    pdf?.Close();
                    reader?.Close();
                }
            }
        }
    }
}

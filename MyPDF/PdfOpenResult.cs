using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.Text;

// ==============================
// 開く・挿入・置換用 パスワード入力処理
// ==============================

namespace MyPDF
{
    public class PdfOpenResult
    {
        // 開けた？
        public bool Success { get; set; }
        // 管理者(制限パス)で開いてる true:制限パス false:以外)
        public bool IsOwner { get; set; }
        // 暗号化されてる？( true:暗号化 false:なし)
        public bool IsEncrypted { get; set; }
        // 権限
        public int Permissions { get; set; }
        // 暗号化方式
        public int CryptoMode { get; set; }
        // パスワード
        public string? Password { get; set; }
        // iText Reader設定
        public ReaderProperties ReaderProps { get; set; }
            = new ReaderProperties();
    }
}

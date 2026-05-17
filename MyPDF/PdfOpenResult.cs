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
        public bool Success { get; set; }
        // 管理者(制限パス)で開いてる true:制限パス false:以外)
        public bool IsOwner { get; set; }
        // 暗号化されてる？( true:暗号化 false:なし)
        public bool IsEncrypted { get; set; }

        public int Permissions { get; set; }

        public int CryptoMode { get; set; }
        // パスワード
        public string? Password { get; set; }

        public ReaderProperties ReaderProps { get; set; }
            = new ReaderProperties();
    }
}

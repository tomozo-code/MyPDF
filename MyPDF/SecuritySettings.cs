using System;
using System.Collections.Generic;
using System.Text;

// ==============================
// セキュリティ設定用(メタデータ)
// ==============================

namespace MyPDF
{
    public class SecuritySettings
    {
        // 開くパス
        public string? UserPassword { get; set; } = "";
        // 制限パス
        public string? OwnerPassword { get; set; } = "";
        // 権限
        public int Permissions { get; set; }
        // 暗号方式
        public int Encryption { get; set; } = 3;
        // 権限パスのチェックマーク
        public bool Check_Owner { get; set; }
        // 開くパスのチェックマーク
        public bool Check_User { get; set; }
        // 印刷
        public bool Check_chkPrint { get; set; } = true;
        // テキスト・画像のコピー
        public bool Check_chkCopy { get; set; }
        // ページ挿入・削除・回転
        public bool Check_chkEdit { get; set; }
        // 注釈追加
        public bool Check_chkAnnot { get; set; }
        // フォーム入力
        public bool Check_chkForm { get; set; }
        // 内容の抽出
        public bool Check_chkExtract { get; set; }
        // PDFを権限(オーナー)パス開いた(true:オーナー、false:それ以外)
        public bool IsOwnerOpened { get; set; }
    }
}

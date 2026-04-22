using iText.Layout;
using System;
using System.Collections.Generic;
using System.Text;
using static iText.Signatures.LtvVerification;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

// ==============================
// CSVしおり用(「CSV → しおり」に変換する中間データ)
// ==============================

// 例: タイトル,1,0,標準,#ff0000
// これが
// Title = "タイトル"
// Page = 1
// Level = 0
// Style = FontStyle.Regular
// Color = Color.Red

namespace MyPDF
{
    internal class CsvBookmark
    {
        // しおり名
        public string? Title;
        //  ページ番号
        public int Page;
        // 階層レベル
        public int Level;
        // 文字スタイル
        public FontStyle Style;
        // 文字色
        public Color Color;
    }
}

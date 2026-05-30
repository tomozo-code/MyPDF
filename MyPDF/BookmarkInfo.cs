using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

// ==============================
// しおり用(メタデータ)
// ==============================

namespace MyPDF
{
    public class BookmarkInfo
    {
        // しおり名
        public string? BmTitle { get; set; }
        // ページ
        public int Page { get; set; }
        // しおり展開(展開:true、縮小:false)
        public bool IsOpen { get; set; }
        // 選択された色
        public Color SelectedColor { get; set; }
        // 選択されたスタイル
        public FontStyle SelectedStyle { get; set; }
        // リスト化
        public List<BookmarkInfo> Children { get; set; } = new();
    }
}

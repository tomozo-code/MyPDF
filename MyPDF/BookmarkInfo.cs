using System;
using System.Collections.Generic;
using System.Text;

// ==============================
// しおり用(メタデータ)
// ==============================

namespace MyPDF
{
    internal class BookmarkInfo
    {
        // しおり名
        public string? BmTitle { get; set; }
        // ページ
        public int Page { get; set; }
        // しおり展開(展開:true、縮小:false)
        public bool IsOpen { get; set; }
        
        public bool IsRootOpen { get; set; }

        // 選択された色
        public Color SelectedColor { get; set; }

        // 選択されたスタイル
        public FontStyle SelectedStyle { get; set; }
    }
}

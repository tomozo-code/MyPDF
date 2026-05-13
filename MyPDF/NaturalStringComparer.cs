using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

// ==============================
// 画像をPDFに変換のファイル名ソート用クラス
// 1.jpg、2.jpg、10.jpg、11.jpg のような並びにする
// ==============================

namespace MyPDF
{
    public class NaturalStringComparer : IComparer<string>
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        private static extern int StrCmpLogicalW(
            string x,
            string y
        );

        public int Compare(string? x, string? y)
        {
            return StrCmpLogicalW(x ?? "", y ?? "");
        }
    }
}

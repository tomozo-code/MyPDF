using System;
using System.Collections.Generic;
using System.Text;

// ==============================
// PDF開く結果格納用
// ==============================

namespace MyPDF
{
    internal class PdfLoadResult
    {
        // 作業用ファイルパス用
        public string WorkingPath { get; set; } = "";
        // PDFの設定値
        public PdfSettings Settings { get; set; } = null!;
        // 表示用
        public PdfiumViewer.PdfDocument Document { get; set; } = null!;
    }
}

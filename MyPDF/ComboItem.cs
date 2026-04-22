using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.Text;

// ==============================
// PDFプロパティ用(開き方コンボボックス)
// ==============================

namespace MyPDF
{
    internal class ComboItem
    {
        // 表示用の文字列「ページのみ」「しおりパネルとページ」など
        public string? Text { get; set; }
        public iText.Kernel.Pdf.PdfName? Value { get; set; }

        //コンボボックスは「ToString()の結果」を表示する
        public override string? ToString() => Text;

    }
}

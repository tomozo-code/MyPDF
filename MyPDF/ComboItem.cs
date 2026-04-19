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

        public string? Text { get; set; }
        public iText.Kernel.Pdf.PdfName? Value { get; set; }

        public override string? ToString() => Text;

    }
}

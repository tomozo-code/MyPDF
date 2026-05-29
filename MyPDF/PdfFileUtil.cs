using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using IOPath = System.IO.Path;

// ==============================
// 作業用ファイル作成
// ==============================

namespace MyPDF
{
    public static class PdfFileUtil
    {
        public static string CreateTempPdfCopy(string sourcePath)
        {
            // 作業ファイル作成
            // C:\Users\<ユーザー名>\AppData\Local\Temp\ に作業用ファイルを置く
            // $"MyPDFwork_{Guid.NewGuid()}.pdf"はランダムファイル名生成
            string workingPath = IOPath.Combine(IOPath.GetTempPath(), $"MyPDFwork_{Guid.NewGuid()}.pdf");
            // 元ファイルを作業用ファイルにコピー true:同じ名前は上書き
            File.Copy(sourcePath, workingPath, true);

            return workingPath;
        }

    }
}

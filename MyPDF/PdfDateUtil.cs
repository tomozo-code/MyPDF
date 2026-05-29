using System;
using System.Collections.Generic;
using System.Text;

// ==============================
// 日付変換
// PDF内部では、D:20260419153022+09'00'　な感じ
// で 2026/04/19 15:30:22 こうする
// ==============================

namespace MyPDF
{
    public static class PdfDateUtil
    {
        public static string FormatPdfDate(string pdfDate)
        {
            // pdfDateが null、空文字、空白だったら、空文字を返す
            if (string.IsNullOrWhiteSpace(pdfDate))
                return "";

            try
            {
                // "D:" がある？
                if (pdfDate.StartsWith("D:"))
                    // あったら先頭2文字削除("D:"を削除)
                    pdfDate = pdfDate.Substring(2);

                // タイムゾーン部分を除去（+09'00' など） + または - を探す
                int tzIndex = pdfDate.IndexOfAny(new char[] { '+', '-' });
                // タイムゾーン見つかった？
                if (tzIndex > 0)
                    // タイムゾーン以降を切り捨て
                    pdfDate = pdfDate.Substring(0, tzIndex);

                // 最低14桁必要（yyyyMMddHHmmss）
                if (pdfDate.Length < 14)
                    // 足りないなら変換不能
                    return pdfDate;

                // 先頭14文字だけ取得
                string datePart = pdfDate.Substring(0, 14);
                // DateTime変換 System.Globalization.CultureInfo.InvariantCultureはカルチャ非依存(日本語環境でも海外環境でも同じ動作)
                var dt = DateTime.ParseExact(datePart, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                // 見やすい形式へ変換
                return dt.ToString("yyyy/MM/dd HH:mm:ss");
            }
            catch // エラー補足
            {
                // 変換失敗したら元のまま
                return pdfDate;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

// ==============================
// ページ指定入力のチェック 1,2,3,5-9のような入力をチェック
// ==============================

namespace MyPDF
{
    internal class PageRangeHelper
    {
        public static List<int> ParsePageRanges(string text, int maxPage)
        {
            var result = new List<int>();

            text = text.Replace(" ", "");

            var parts = text.Split(',');

            foreach (var partRaw in parts)
            {
                string part = partRaw.Trim();

                if (part.Contains('-'))
                {
                    var range = part.Split('-');

                    if (range.Length != 2)
                        throw new Exception("範囲指定が不正です。");

                    if (!int.TryParse(range[0], out int start) ||
                        !int.TryParse(range[1], out int end))
                    {
                        throw new Exception("数値変換に失敗しました。");
                    }

                    if (start > end)
                        throw new Exception("開始ページは終了ページ以下にしてください。");

                    if (start < 1 || end > maxPage)
                        throw new Exception("ページ範囲が不正です。");

                    for (int i = start; i <= end; i++)
                    {
                        result.Add(i);
                    }
                }
                else
                {
                    if (!int.TryParse(part, out int page))
                        throw new Exception("数値変換に失敗しました。");

                    if (page < 1 || page > maxPage)
                        throw new Exception("ページ範囲が不正です。");

                    result.Add(page);
                }
            }

            return result
                .Distinct()
                .OrderBy(p => p)
                .ToList();
        }

    }
}

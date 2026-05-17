using System;
using System.Collections.Generic;
using System.Text;

// ==============================
// ページ指定入力のチェック
// ParsePageRanges:1,2,3,5-9のような入力をチェック
// ParseReplaceRange:1-3のみ
// ==============================

namespace MyPDF
{
    internal class PageRangeHelper
    {
        // ==============================
        // 1,2,3,5-9等のチェック
        // ==============================
        public static List<int> ParsePageRanges(string text, int maxPage)
        {
            // ページをリストで返す
            // ページ番号格納用リスト作成
            var result = new List<int>();

            // スペース除去
            text = text.Replace(" ", "");
            // カンマで分割
            var parts = text.Split(',');

            // 分割した要素を1個ずつ処理
            foreach (var partRaw in parts)
            {
                // 前後の空白を除去
                string part = partRaw.Trim();

                // - があるか判定
                if (part.Contains('-'))
                {
                    // - がある場合
                    // - で分割
                    var range = part.Split('-');
                    // - が複数ある異常入力をチェック(1-3-5などはOUT)
                    if (range.Length != 2)
                        throw new Exception("範囲指定が不正です。");

                    // 数値変換 変換成功ならtrue、結果をstartとendに入れる
                    if (!int.TryParse(range[0], out int start) ||
                        !int.TryParse(range[1], out int end))
                    {
                        throw new Exception("数値変換に失敗しました。");
                    }

                    // 開始 > 終了 を禁止
                    if (start > end)
                        throw new Exception("開始ページは終了ページ以下にしてください。");

                    // 範囲外チェック
                    if (start < 1 || end > maxPage)
                        throw new Exception("ページ範囲が不正です。");

                    // start～end をループ
                    for (int i = start; i <= end; i++)
                    {
                        // リストへ追加
                        result.Add(i);
                    }
                }
                else
                {
                    // - がない場合(単一ページ)
                    // 数値変換
                    if (!int.TryParse(part, out int page))
                        throw new Exception("数値変換に失敗しました。");
                    // 範囲外禁止
                    if (page < 1 || page > maxPage)
                        throw new Exception("ページ範囲が不正です。");
                    // リストへ追加
                    result.Add(page);
                }
            }

            // result を返す(重複除去、昇順ソート、Listへ変換)
            return result.Distinct().OrderBy(p => p).ToList();
        }

        // ==============================
        // 置換用ページ範囲取得
        // 1-3 または 5 のみ許可
        // ==============================
        public static (int Start, int End) ParseReplaceRange(string text, int maxPage)
        {
            // 戻り値は、開始ページと終了ページをセットで返す
            // スペース除去
            text = text.Replace(" ", "");

            // カンマ禁止
            if (text.Contains(','))
                throw new Exception("置換ページは連続範囲のみ指定できます。");

            // - があるか判定
            if (text.Contains('-'))
            {
                // - がある場合
                // - で分割
                var range = text.Split('-');

                // - が複数ある異常入力をチェック(1-3-5などはOUT)
                if (range.Length != 2)
                    throw new Exception("範囲指定が不正です。");

                // 数値変換 変換成功ならtrue、結果をstartとendに入れる
                if (!int.TryParse(range[0], out int start) ||
                    !int.TryParse(range[1], out int end))
                {
                    throw new Exception("数値変換に失敗しました。");
                }

                // 開始 > 終了 を禁止
                if (start > end)
                    throw new Exception("開始ページは終了ページ以下にしてください。");

                // 範囲外チェック
                if (start < 1 || end > maxPage)
                    throw new Exception("ページ範囲が不正です。");

                // 開始と終了を返す
                return (start, end);
            }
            else
            {
                // - がない場合(単一ページ)
                // 数値変換
                if (!int.TryParse(text, out int page))
                    throw new Exception("数値変換に失敗しました。");

                // 範囲外禁止
                if (page < 1 || page > maxPage)
                    throw new Exception("ページ範囲が不正です。");
                // 単一ページを返す
                return (page, page);
            }
        }

    }
}

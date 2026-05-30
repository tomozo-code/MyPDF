using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ITextDoc = iText.Kernel.Pdf.PdfDocument;
using DrawingColor = System.Drawing.Color;
using PdfiTextReader = iText.Kernel.Pdf.PdfReader;

// ==============================
// PDFのしおり取得用
// PDFからしおりを読み取り、TreeView(treeView1) に表示する
// ==============================

namespace MyPDF
{
    public static class PdfBookmarkLoader
    {
        //public static List<BookmarkInfo> Load(string path, string? password)
        //{

        //}

        // ==============================
        // PDFのしおり取得
        // PDFからしおりを読み取り、TreeView(treeView1) に表示するメソッド
        // ==============================
        public static void Load(string path, string? password, TreeNodeCollection nodes, Font treeFont)
        {
            // iText用のPDF読み込み設定作成
            ReaderProperties props = new ReaderProperties();
            // パスワードある？
            if (!string.IsNullOrEmpty(password))
            {
                // パスあり パスワードを byte[] に変換して設定
                props.SetPassword(Encoding.UTF8.GetBytes(password));
            }
            // PDF開く(PDF読み込み→PDFドキュメント化)
            using (var pdf = new ITextDoc(new PdfiTextReader(path, props)))
            {
                // iTextでPDFのしおりを取得 falseは展開しない
                var outlines = pdf.GetOutlines(false);
                // しおりがない？
                if (outlines == null)
                    return;
                // ルート(親)しおり取得
                var root = outlines.GetAllChildren();
                // 子しおり数0？
                if (root.Count == 0)
                    return;
                // ルート(親)から再帰でしおり追加
                foreach (var item in root)
                {
                    // 再帰でTreeViewに変換
                    AddBookmarkNode(item, nodes, pdf, treeFont);
                }
            }
        }

        // ==============================
        // PDFのしおりをツリービューに表示
        // PDFのしおり1個 → TreeViewのノード1個に変換
        // ==============================
        private static void AddBookmarkNode(PdfOutline outline, TreeNodeCollection nodes, ITextDoc pdf, Font treeFont)
        {
            // しおりのページ番号を初期化
            int pageNumber = 0;

            // Outlineの生データ取得（これが本体）
            // PDFの /Title /Dest /A /Count 等が入ってる
            var dict = outline.GetContent();
            //しおりジャンプ先情報格納用(最初はnull)
            PdfObject? destObj = null;

            // /Dest を優先取得
            if (dict.ContainsKey(PdfName.Dest))
            {
                // あるので /Dest を取得
                destObj = dict.Get(PdfName.Dest);
            }
            // Dest が無い場合
            // /A（Action）から取得
            else if (dict.ContainsKey(PdfName.A))
            {
                // Action辞書取得
                var action = dict.GetAsDictionary(PdfName.A);
                // Action内に /D がある？
                if (action != null && action.ContainsKey(PdfName.D))
                {
                    // あるので /D 取得
                    destObj = action.Get(PdfName.D);
                }
            }

            // ページ番号取得
            // デバッグ出力確認
            Debug.WriteLine("----しおり----------------------");
            Debug.WriteLine(outline.GetTitle());
            Debug.WriteLine(destObj?.GetType());
            Debug.WriteLine(destObj);

            // ジャンプ先が配列形式？
            if (destObj is PdfArray arr && arr.Size() > 0)
            {
                // 配列0番目からページ辞書取得
                var pageDict = arr.GetAsDictionary(0);
                // ページ辞書ある？
                if (pageDict != null)
                {
                    // ページオブジェクト取得
                    var page = pdf.GetPage(pageDict);
                    // 実際のページ番号へ変換
                    pageNumber = pdf.GetPageNumber(page);
                }
            }
            // Named Destination（文字列）
            else if (destObj is PdfString name)
            {
                // PDFのNameTree取得
                var nameTree = pdf.GetCatalog().GetNameTree(PdfName.Dests);
                // 名前一覧取得
                var names = nameTree.GetNames();

                // 検索キー化（string → PdfString）
                var key = name;
                // NameTreeに存在？
                if (names.ContainsKey(key))
                {
                    // 対応配列取得
                    var obj = names[key] as PdfArray;
                    // 有効？
                    if (obj != null && obj.Size() > 0)
                    {
                        // ページ辞書取得
                        var pageDict = obj.GetAsDictionary(0);
                        // nullじゃない？
                        if (pageDict != null)
                        {
                            // ページ取得
                            var page = pdf.GetPage(pageDict);
                            // ページ番号へ変換
                            pageNumber = pdf.GetPageNumber(page);
                        }
                    }
                }
            }

            // しおり名取得 nullなら 「しおり名なし」
            //string title = outline.GetTitle() ?? "(no title)";
            string title = outline.GetTitle() ?? "(しおり名なし)";
            // しおりの展開状態(初期値は展開) true:展開、false:縮小
            bool isOpen = true;
            // /Count 取得
            var countObj = dict.GetAsNumber(PdfName.Count);
            // Countある？
            if (countObj != null)
            {
                // 0以上なら展開状態
                isOpen = countObj.IntValue() >= 0;
            }

            // 文字の色 初期値は黒
            DrawingColor selectedColor = DrawingColor.Black;
            // しおり色取得
            var color = outline.GetColor();
            // 色ある？
            if (color != null)
            {
                // iTextのColor → RGB取得
                var rgb = color.GetColorValue(); // ←これがfloat[]
                                                 // RGB有効？
                if (rgb != null && rgb.Length >= 3)
                {
                    // WinForms Colorへ変換
                    selectedColor = DrawingColor.FromArgb(
                        (int)(rgb[0] * 255),
                        (int)(rgb[1] * 255),
                        (int)(rgb[2] * 255)
                    );
                }
            }

            // 文字スタイル取得
            int style = outline.GetStyle() ?? 0;
            // 初期通常文字
            FontStyle fontStyle = FontStyle.Regular;

            // 両方通るとボールドイタリックになるはず
            // ボールド
            if ((style & PdfOutline.FLAG_BOLD) != 0)
                fontStyle |= FontStyle.Bold;
            // イタリック
            if ((style & PdfOutline.FLAG_ITALIC) != 0)
                fontStyle |= FontStyle.Italic;
            // 文字スタイル保存
            FontStyle selectedStyle = fontStyle;

            Debug.WriteLine("-----文字スタイル1------------------------");
            Debug.WriteLine("selectedStyle: " + selectedStyle);
            // TreeNode作成
            var node = new TreeNode(title)
            {
                // 通常アイコン(桃豚アイコン)
                ImageIndex = 0,
                // 選択時アイコン(白豚アイコン)
                SelectedImageIndex = 1,
                // しおり情報をTagへ保存
                Tag = new BookmarkInfo
                {
                    // しおり名
                    BmTitle = title,
                    //　ページ番号
                    Page = pageNumber,
                    // 展開 or 縮小
                    IsOpen = isOpen,
                    // 色
                    SelectedColor = selectedColor,
                    // スタイル
                    SelectedStyle = selectedStyle
                }
            };

            // 色をUIに反映
            node.ForeColor = selectedColor;
            // フォントをUIに反映
            node.NodeFont = new Font(treeFont, selectedStyle);
            // ノード追加
            nodes.Add(node);

            Debug.WriteLine("-----しおり名 → ページ番号------------------------");
            Debug.WriteLine($"{title} → Page:{pageNumber}");

            // 再帰
            foreach (var child in outline.GetAllChildren())
            {
                // 子を再帰追加
                AddBookmarkNode(child, node.Nodes, pdf, treeFont);
            }

            // 最後にCollapse（順番重要）
            // 展開状態反映開始
            if (isOpen)
            {
                // trueなので展開
                node.Expand();
            }
            else
            {
                // falseなので縮小
                node.Collapse();
            }

        }

    }
}

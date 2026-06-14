using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.XMP;
using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using IOPath = System.IO.Path;
using ITextDoc = iText.Kernel.Pdf.PdfDocument;

// ==============================
// PDFの設定情報を読み込む
// 作業用ファイルの情報を読むがファイル名とパスは元ファイルにする
// path = 実際に読むPDF（作業用PDF）
// password = PDFのパスワード（無ければnull）
// ==============================

namespace MyPDF
{
    public static class PdfSettingsLoader
    {
        public static PdfSettings LoadPdfSettings(string path, string originalPath, string? password = null)
        {
            // PdfSettings クラスの新しいインスタンス作成
            // 読み取った情報をここへ保存
            var settings = new PdfSettings();

            // ファイル名 元ファイル名を取得
            settings.PdfFileName = IOPath.GetFileName(originalPath);
            // 元PDFのフォルダパス取得
            settings.PdfPath = IOPath.GetDirectoryName(originalPath);
            // iTextでPDFを読むための PdfReader 変数宣言(まだ作ってない)
            PdfReader reader;

            try
            {
                // パスワードあり？(true:あり、false:無し)
                if (!string.IsNullOrEmpty(password))
                {
                    // パスあり
                    // パスワードを UTF8 byte[] に変換
                    reader = new PdfReader(path, new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(password)));
                }
                else
                {
                    // パスなし
                    reader = new PdfReader(path);
                }
            }
            catch // エラー補足(失敗した場合)
            {
                // フォールバック(念のためパスなし再挑戦)
                reader = new PdfReader(path);
            }
            // PdfDocument生成
            using (var pdf = new ITextDoc(reader))
            {
                // iTextでプロパティ取得
                var info = pdf.GetDocumentInfo();

                // pdfViewerのページ数取得
                //int pageCount = pdfViewer1.Document.PageCount;
                // 総ページ数をデータへ
                settings.TotalPage = pdf.GetNumberOfPages();


                //settings.TotalPage = pageCount;

                // ステータスバーにファイル名(元ファイル)と総ページ数
                //UpdateStatus(originalPath, pageCount);

                // ファイルサイズ
                var fi = new FileInfo(path);
                long bytes = fi.Length; // byte単位サイズ
                double kb = bytes / 1024.0; // kb換算
                settings.FileSize_bytes = bytes; //byte保存
                settings.FileSize_Kb = kb; // kb保存

                // PDFファイルのバージョン
                settings.PdfFileVer = pdf.GetPdfVersion().ToString();

                // ページサイズ取得(pt)
                var page = pdf.GetFirstPage();
                var size = page.GetPageSize();
                // ptをmmに変換
                float PtToMm(float pt)
                {
                    // 72pt = 1inch inch→mm変換
                    return pt * 25.4f / 72f;
                }
                // 幅高さmm変換
                float widthMm = PtToMm(size.GetWidth());
                float heightMm = PtToMm(size.GetHeight());
                // 幅高さを保存
                settings.PageSize_W = widthMm;
                settings.PageSize_H = heightMm;

                // 概要
                // タイトル
                settings.Title = info.GetTitle() ?? "";
                // 作成者
                settings.Author = info.GetAuthor() ?? "";
                // サブタイトル
                settings.Subject = info.GetSubject() ?? "";
                // キーワード
                //settings.Keywords = info.GetKeywords() ?? "";
                string keywords = "";
                // XMPメタデータ取得(Adobe系PDFはここに入ってる事が多い)
                var xmp = pdf.GetXmpMetadata(); // ←これXMPMeta
                // XMP存在する？
                if (xmp != null)
                {
                    try
                    {
                        // キーワード数取得
                        int count = xmp.CountArrayItems(XMPConst.NS_DC, "subject");
                        // キーワード保存用List
                        List<string> list = new List<string>();
                        // XMP配列は1開始
                        for (int i = 1; i <= count; i++)
                        {
                            // i番目取得
                            var item = xmp.GetArrayItem(XMPConst.NS_DC, "subject", i);
                            // null？
                            if (item != null)
                            {
                                // nullではないので、キーワード追加
                                list.Add(item.GetValue());
                            }
                        }
                        // 改行結合
                        keywords = string.Join(Environment.NewLine, list);
                    }
                    // 失敗した場合
                    catch
                    {
                        // 通常メタデータから取得
                        keywords = info.GetKeywords() ?? "";
                    }
                }
                else
                {
                    // XMP無いので、通常メタデータから取得
                    keywords = info.GetKeywords() ?? "";
                }
                // キーワードを保持
                settings.Keywords = keywords;

                // アプリケーション
                settings.Creator = info.GetCreator() ?? "";
                // PDF変換
                settings.Producer = info.GetProducer() ?? "";
                // 作成日
                settings.CreationDate = PdfDateUtil.FormatPdfDate(info.GetMoreInfo("CreationDate")) ?? "";
                // 更新日
                settings.ModDate = PdfDateUtil.FormatPdfDate(info.GetMoreInfo("ModDate")) ?? "";

                
                // デバッグ出力確認
                Debug.WriteLine("-----LoadPdfSettings------------------------");
                Debug.WriteLine("ファイル名: " + settings.PdfFileName);
                Debug.WriteLine("パス名: " + settings.PdfPath);
                Debug.WriteLine("総ページ数: " + settings.TotalPage);
                Debug.WriteLine("ファイルサイズ_b: " + settings.FileSize_bytes);
                Debug.WriteLine("ファイルサイズ_Kb: " + settings.FileSize_Kb);
                Debug.WriteLine("PDFファイルのバージョン: " + settings.PdfFileVer);
                Debug.WriteLine("ページサイズ_幅: " + settings.PageSize_W);
                Debug.WriteLine("ページサイズ_高さ: " + settings.PageSize_H);
                Debug.WriteLine("タイトル: " + settings.Title);
                Debug.WriteLine("作成者: " + settings.Author);
                Debug.WriteLine("サブタイトル: " + settings.Subject);
                Debug.WriteLine("キーワード: " + settings.Keywords);
                Debug.WriteLine("アプリケーション: " + settings.Creator);
                Debug.WriteLine("PDF変換: " + settings.Producer);
                Debug.WriteLine("作成日: " + settings.CreationDate);
                Debug.WriteLine("更新日: " + settings.ModDate);
                Debug.WriteLine("入力PW: " + password);
                
                // Catalog取得
                var catalog = pdf.GetCatalog();
                // 生PDF辞書取得
                var catalogObj = catalog.GetPdfObject();

                // 表示モード
                var pageMode = catalogObj.GetAsName(PdfName.PageMode);
                settings.PageMode = pageMode?.GetValue();

                // レイアウト
                var layout = catalogObj.GetAsName(PdfName.PageLayout);
                settings.PageLayout = layout?.GetValue();

                // PDF開いた時の表示設定取得
                var openAction = catalogObj.Get(PdfName.OpenAction);

                // 配列形式？
                if (openAction is PdfArray arr && arr.Size() >= 2)
                {
                    try
                    {
                        // 対象ページ取得
                        var pageObj = arr.Get(0);
                        // 全ページ探索
                        for (int i = 1; i <= pdf.GetNumberOfPages(); i++)
                        {
                            // 同じページ辞書？
                            if (pdf.GetPage(i).GetPdfObject() == pageObj)
                            {
                                // 開始ページ確定
                                settings.OpenPage = i;
                                break;
                            }
                        }

                        // 表示タイプ(XYZ/Fit等取得)
                        var type = arr.GetAsName(1);
                        // nullじゃない？
                        if (type != null)
                        {
                            // 表示方法分岐
                            switch (type.GetValue())
                            {
                                // 任意倍率
                                case "XYZ":
                                    // 倍率取得
                                    var zoomObj = arr.Size() > 4 ? arr.Get(4) : null;
                                    // 数値？
                                    if (zoomObj is PdfNumber zoomNum)
                                    {
                                        // 数値なら%変換
                                        settings.Zoom = (zoomNum.FloatValue() * 100).ToString("0") + "%";
                                    }
                                    else
                                    {
                                        // 100%
                                        settings.Zoom = "100%";
                                    }
                                    break;

                                case "Fit":
                                    // 全体表示
                                    settings.Zoom = "全体表示";
                                    break;

                                case "FitH":
                                    // 幅に合わせる
                                    settings.Zoom = "幅に合わせる";
                                    break;

                                case "FitV":
                                    // 高さに合わせる
                                    settings.Zoom = "高さに合わせる";
                                    break;

                                default:
                                    // デフォルト
                                    settings.Zoom = "デフォルト";
                                    break;
                            }
                        }
                    }
                    catch //エラー補足
                    {
                        // 壊れてるPDF対策
                        settings.OpenPage = 1;
                        settings.Zoom = "全体表示";

                    }

                    // デバッグ出力確認
                    Debug.WriteLine("-----開き方(LoadPdfSettings)------------------------");
                    Debug.WriteLine("表示: " + settings.PageMode);
                    Debug.WriteLine("ページレイアウト: " + settings.PageLayout);
                    Debug.WriteLine("倍率: " + settings.Zoom);
                    Debug.WriteLine("開始ページ" + settings.OpenPage);

                }

            }
            // 完成した設定情報返す
            return settings;
        }
    }
}

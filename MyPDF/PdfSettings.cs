using System;
using System.Collections.Generic;
using System.Text;

// ==============================
// PDFプロパティ用(メタデータ)
// ==============================

namespace MyPDF
{
    public class PdfSettings
    {
        // PDFのファイル名
        public string? PdfFileName { get; set; } = "";
        // PDFのパス
        public string? PdfPath { get; set; } = "";
        // 総ページ数
        public int TotalPage { get; set; } = 1;
        // ファイルサイズ_バイト
        public long FileSize_bytes { get; set; } =1;
        // ファイルサイズ_KB
        public double FileSize_Kb { get; set; } = 1;
        // PDFファイルバージョン
        public string PdfFileVer { get; set; } = "";

        // ページサイズ
        public float PageSize_W { get; set; } = 1;
        public float PageSize_H { get; set; } = 1;


        // 概要(標準メタデータ)
        // タイトル
        public string Title { get; set; } = "";
        // 作成者
        public string Author { get; set; } = "";
        // サブタイトル
        public string Subject { get; set; } = "";
        // キーワード
        public string Keywords { get; set; } = "";
        // アプリケーション
        public string Creator { get; set; } = "";
        // PDF変換
        public string Producer { get; set; } = "";
        // 作成日
        public string CreationDate { get; set; } = "";
        // 更新日
        public string ModDate { get; set; } = "";

        // カスタムメタデータ
        public string CustomKey { get; set; } = "";

        // 開き方
        public string? PageMode { get; set; } = "UseOutlines";
        public string? PageLayout { get; set; } = "SinglePage";
        public string? Zoom { get; set; } = "Fit";
        public float? ZoomValue { get; set; } = 1.0f;
        // 開くときのスタートページ
        public int OpenPage { get; set; } = 1;



    }
}

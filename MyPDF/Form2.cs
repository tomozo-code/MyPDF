using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Navigation;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

// ==============================
// PDFのプロパティ用
// ==============================

namespace MyPDF
{
    public partial class Form2 : Form
    {
        public PdfSettings Settings { get; private set; }

        //private string pdfPath = "";

        public Form2(PdfSettings settings)
        {
            InitializeComponent();
            
            // フォームサイズ
            this.Width = 800;
            this.Height = 500;

            // コピー（参照渡し事故防止）
            Settings = new PdfSettings
            {
                PdfFileName = settings.PdfFileName,
                PdfPath = settings.PdfPath,
                TotalPage = settings.TotalPage,
                FileSize_bytes = settings.FileSize_bytes,
                FileSize_Kb = settings.FileSize_Kb,
                PdfFileVer = settings.PdfFileVer,
                PageSize_W = settings.PageSize_W,
                PageSize_H = settings.PageSize_H,
                Title = settings.Title,
                Author = settings.Author,
                Subject = settings.Subject,
                Keywords = settings.Keywords,
                PageMode = settings.PageMode,
                PageLayout = settings.PageLayout,
                OpenPage = settings.OpenPage,
                Zoom = settings.Zoom,
                ZoomValue = settings.ZoomValue,
                CreationDate = settings.CreationDate,
                ModDate = settings.ModDate,
                Creator = settings.Creator,
                Producer = settings.Producer
            };
                                    
            // コンボボックスの初期設定
            InitComboBoxes();
            // UIに反映
            LoadToUI();

        }

        // ==============================
        // 読み込んだPDFプロパティをUIに反映(概要)
        // ==============================
        private void LoadToUI()
        {
            // ファイル名
            FileNamelabel.Text = Settings.PdfFileName;
            // パス
            PasLabel.Text = Settings.PdfPath;
            // 総ページ数
            //TotalPageLabel1.Text = Settings.TotalPage.ToString();
            TotalPageLabel1.Text = "/ " + Settings.TotalPage.ToString();
            // ファイルサイズ
            FileSizeLabel.Text = $"{Settings.FileSize_Kb:#,##0.00} KB ({Settings.FileSize_bytes:N0} バイト)";
            // PDFファイルのバージョン
            PdfVerLabel.Text = Settings.PdfFileVer;
            // ページサイズ
            PageSizeLabel.Text = $"{Math.Round(Settings.PageSize_W)} x {Math.Round(Settings.PageSize_H)} mm";

            // タイトル
            TitleTxt.Text = Settings.Title;
            // 作成者
            AuthorTxt.Text = Settings.Author;
            // サブタイトル
            SubTitleTxt.Text = Settings.Subject;
            // キーワード
            KeywordTxt.Text = Settings.Keywords;
            // 作成日
            CreationDateLabel.Text = Settings.CreationDate;
            // 更新日
            ModDateLabel.Text = Settings.ModDate;
            // アプリケーション
            CreatorLabel.Text = Settings.Creator;
            // PDF変換
            PdfConvertLabel.Text = Settings.Producer;

            // 表示
            var mode = Settings.PageMode;
            if (mode == null)
            {
                ViewComboBox.SelectedIndex = 0;
            }
            else
            {
                SelectCombo(ViewComboBox, mode);
            }

            // ページレイアウト
            var layout = Settings.PageLayout;
            if (layout == null)
            {
                PageLayoutComboBox.SelectedIndex = 0;
            }
            else
            {
                SelectCombo(PageLayoutComboBox, layout);
            }

            // 倍率
            MagComboBox.Text = Settings.Zoom;

            // 開くページ
            PageTxt.Text = Settings.OpenPage.ToString();
        }



        // ==============================
        // OKボタンを押したとき
        // ==============================
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            Settings.Title = TitleTxt.Text;
            Settings.Author = AuthorTxt.Text;
            Settings.Subject = SubTitleTxt.Text;
            Settings.Keywords = KeywordTxt.Text;

            // 表示
            switch (ViewComboBox.Text)
            {
                case "ページのみ":
                    Settings.PageMode = "UseNone";
                    break;

                case "しおりパネルとページ":
                    Settings.PageMode = "UseOutlines";
                    break;

                case "ページパネルとページ":
                    Settings.PageMode = "UseThumbs";
                    break;

                case "添付ファイルパネルとページ":
                    Settings.PageMode = "UseAttachments";
                    break;

                case "レイヤパネルとページ":
                    Settings.PageMode = "UseOC";
                    break;

                default:
                    Settings.PageMode = null;
                    break;

            }

            // ページレイアウト
            switch (PageLayoutComboBox.Text)
            {
                case "単一ページ":
                    Settings.PageLayout = "SinglePage";
                    break;

                case "連続ページ":
                    Settings.PageLayout = "OneColumn";
                    break;

                case "見開きページ":
                //case "連続見開きページ":
                    Settings.PageLayout = "TwoColumnLeft";
                    break;

                case "見開きページ(表紙)":
                //case "連続見開きページ(表紙)":
                    Settings.PageLayout = "TwoPageLeft";
                    break;

                default:
                    // デフォルト
                    Settings.PageLayout = null;
                    break;

            }

            //Settings.Zoom = MagComboBox.Text;
            // 倍率
            switch (MagComboBox.Text)
            {
                case "全体表示":
                    Settings.Zoom = "Fit";
                    Settings.ZoomValue = null;
                    break;

                case "幅に合わせる":
                    Settings.Zoom = "FitH";
                    Settings.ZoomValue = null;
                    break;

                case "高さに合わせる":
                    Settings.Zoom = "FitV";
                    Settings.ZoomValue = null;
                    break;

                case "100%":
                    Settings.Zoom = "XYZ";
                    Settings.ZoomValue = 1.0f;
                    break;

                case "25%":
                    Settings.Zoom = "XYZ";
                    Settings.ZoomValue = 0.25f;
                    break;

                case "50%":
                    Settings.Zoom = "XYZ";
                    Settings.ZoomValue = 0.5f;
                    break;

                case "75%":
                    Settings.Zoom = "XYZ";
                    Settings.ZoomValue = 0.75f;
                    break;

                case "125%":
                    Settings.Zoom = "XYZ";
                    Settings.ZoomValue = 1.25f;
                    break;

                case "150%":
                    Settings.Zoom = "XYZ";
                    Settings.ZoomValue = 1.50f;
                    break;

                case "200%":
                    Settings.Zoom = "XYZ";
                    Settings.ZoomValue = 2.0f;
                    break;

                default:
                    Settings.Zoom = null;
                    Settings.ZoomValue = null;
                    break;

            }

            // デバッグ出力確認
            Debug.WriteLine("-----PDFプロパティ(OK)------------------------");
            Debug.WriteLine("タイトル: " + Settings.Title);
            Debug.WriteLine("作成者: " + Settings.Author);
            Debug.WriteLine("サブタイトル: " + Settings.Subject);
            Debug.WriteLine("キーワード: " + Settings.Keywords);

            if (int.TryParse(PageTxt.Text, out int page))
                Settings.OpenPage = page;

            this.DialogResult = DialogResult.OK;

            this.Close();
        }


        // ==============================
        // Cancelボタンを押したとき
        // ==============================
        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // ==============================
        // コンボボックスの初期設定
        // ==============================
        private void InitComboBoxes()
        {
            // 表示（PageMode）
            ViewComboBox.Items.AddRange(new object[]
            {
        new ComboItem { Text = "ページのみ", Value = PdfName.UseNone },
        new ComboItem { Text = "しおりパネルとページ", Value = PdfName.UseOutlines },
        new ComboItem { Text = "ページパネルとページ", Value = PdfName.UseThumbs },
        new ComboItem { Text = "添付ファイルパネルとページ", Value = PdfName.UseAttachments },
        new ComboItem { Text = "レイヤパネルとページ", Value = PdfName.UseOC }
            });

            // ページレイアウト
            PageLayoutComboBox.Items.AddRange(new object[]
            {
        new ComboItem { Text = "単一ページ", Value = PdfName.SinglePage },
        new ComboItem { Text = "連続ページ", Value = PdfName.OneColumn },
        new ComboItem { Text = "見開きページ", Value = PdfName.TwoColumnLeft },
        //new ComboItem { Text = "連続見開きページ", Value = PdfName.TwoColumnLeft },
        new ComboItem { Text = "見開きページ(表紙)", Value = PdfName.TwoPageLeft },
        //new ComboItem { Text = "連続見開きページ(表紙)", Value = PdfName.TwoColumnLeft }
            });

            // 倍率
            MagComboBox.Items.AddRange(new object[]
            {
        "デフォルト",
        "100%",
        "全体表示",
        "幅に合わせる",
        "高さに合わせる",
        "25%",
        "50%",
        "75%",
        "125%",
        "150%",
        "200%"
            });
        }


        // ==============================
        // コンボボックス選択ヘルパー
        // combo:対象のコンボボックス、value:探したい値（例："UseOutlines"）
        // ==============================
        private void SelectCombo(ComboBox combo, string value)
        {
            // コンボボックスに入っている全項目を1つずつ取り出す
            foreach (var item in combo.Items)
            {
                // item が ComboItem で、その中の Value が PdfName なら処理する
                if (item is ComboItem ci && ci.Value is PdfName name)
                {
                    // PdfName → string に変換して比較
                    if (name.GetValue() == value)
                    {
                        // 一致したら、その項目を選択状態にする
                        combo.SelectedItem = item;
                        return;
                    }
                }
            }
        }

    }
}

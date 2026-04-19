namespace MyPDF
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // ここでpdfium.dllがあるかチェック
            // 発行するときは /* と */ を外す
            
            if (!CheckPdfiumDll())
            {
                MessageBox.Show(
                    "pdfium.dll が見つかりません。" + Environment.NewLine + "MyPDF.exeと同じフォルダに配置してください。",
                    "起動エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            

            // 例外ハンドリング（最重要）
            Application.ThreadException += (s, e) =>
            {
                MessageBox.Show(
                    "エラーが発生しました。\n\n" + e.Exception.Message,
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            };

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                MessageBox.Show(
                    "致命的エラーが発生しました。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            };

            Application.Run(new Form1());
        }

        // ==============================
        // pdfium.dll チェック関数
        // ==============================
        private static bool CheckPdfiumDll()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string dllPath = Path.Combine(baseDir, "pdfium.dll");

            return File.Exists(dllPath);
        }
    }
}
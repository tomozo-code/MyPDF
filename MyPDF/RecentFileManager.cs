
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

// ==============================
// 最近使ったファイルのクラス
// ==============================

namespace MyPDF
{
    public class RecentFileManager
    {
        // 最大履歴件数
        private const int MaxHistoryCount = 10;

        // 履歴ファイルの保存先
        private readonly string _historyFilePath;

        // 履歴一覧
        private readonly List<string> _recentFiles = new();

        // 履歴一覧を取得
        public IReadOnlyList<string> RecentFiles => _recentFiles;

        // コンストラクタ
        public RecentFileManager()
        {
            // AppData\Local\MyPDF
            string folderPath = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "MyPDF");

            // フォルダがなければ作成
            Directory.CreateDirectory(folderPath);

            // 履歴ファイルのパス
            _historyFilePath = Path.Combine(
                folderPath,
                "RecentFiles.txt");

            // 履歴を読み込む
            Load();
        }

        // 履歴ファイルの保存先フォルダ
        public string HistoryFolderPath =>
            Path.GetDirectoryName(_historyFilePath)!;

        // ========================================================
        // 履歴読み込み
        // ========================================================
        private void Load()
        {
            _recentFiles.Clear();

            if (!File.Exists(_historyFilePath))
                return;

            try
            {
                foreach (string path in File.ReadAllLines(_historyFilePath))
                {
                    if (string.IsNullOrWhiteSpace(path))
                        continue;

                    if (_recentFiles.Contains(
                        path,
                        StringComparer.OrdinalIgnoreCase))
                        continue;

                    _recentFiles.Add(path);

                    if (_recentFiles.Count >= MaxHistoryCount)
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("履歴読み込みエラー: " + ex.Message);
            }
        }

        // ========================================================
        // 履歴追加
        // ========================================================
        public void Add(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;

            try
            {
                path = Path.GetFullPath(path);
            }
            catch
            {
                return;
            }

            // 同じファイルがあれば削除
            _recentFiles.RemoveAll(
                p => string.Equals(
                    p,
                    path,
                    StringComparison.OrdinalIgnoreCase));

            // 先頭に追加
            _recentFiles.Insert(0, path);

            // 10件を超えたら古い履歴を削除
            if (_recentFiles.Count > MaxHistoryCount)
            {
                _recentFiles.RemoveRange(
                    MaxHistoryCount,
                    _recentFiles.Count - MaxHistoryCount);
            }

            // ファイルに保存
            Save();
        }

        // ========================================================
        // 履歴保存
        // ========================================================
        private void Save()
        {
            try
            {
                File.WriteAllLines(
                    _historyFilePath,
                    _recentFiles);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(
                    "履歴保存エラー: " + ex.Message);
            }
        }

        // ========================================================
        // 履歴をすべてクリア
        // ========================================================
        public void Clear()
        {
            _recentFiles.Clear();
            Save();
        }
    }
}

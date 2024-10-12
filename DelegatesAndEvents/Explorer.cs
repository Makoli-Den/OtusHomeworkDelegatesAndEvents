using System;
using System.IO;

namespace DelegatesAndEvents
{
    internal class Explorer
    {
        public event EventHandler<ExplorerArgs> OnItemFound;

        public void Search(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Console.WriteLine("Указанный путь не существует.");
                return;
            }

            // Поиск файлов
            var files = Directory.GetFiles(directory);
            foreach (var file in files)
            {
                var args = new ExplorerArgs(file, true);
                ItemFound(args);

                if (args.Cancel)
                {
                    BaseConsoleActions.DisplayMessageWithSpacing("Поиск файлов отменен.");
                    return;
                }
            }

            // Поиск папок
            var directories = Directory.GetDirectories(directory);
            foreach (var folder in directories)
            {
                var args = new ExplorerArgs(folder, false);
                ItemFound(args);

                if (args.Cancel)
                {
                    BaseConsoleActions.DisplayMessageWithSpacing("Поиск папок отменен.");
                    return;
                }
            }
        }

        public List<string> GetAvailableDrives()
        {
            List<string> drives = new List<string>();

            // Получаем все доступные диски
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (var drive in allDrives)
            {
                // Проверяем, что диск доступен
                if (drive.IsReady)
                {
                    drives.Add(drive.Name);
                }
            }

            return drives;
        }

        // Новый метод для поиска дисков
        public void SearchDrives()
        {
            var drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                if (drive.IsReady) // Проверяем, готов ли диск
                {
                    var args = new ExplorerArgs(drive.Name, false, false, true); // Диск рассматриваем как папку
                    ItemFound(args);
                }
            }
        }

        protected virtual void ItemFound(ExplorerArgs e)
        {
            OnItemFound?.Invoke(this, e);
        }
    }
}

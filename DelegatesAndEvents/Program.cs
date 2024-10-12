using System.Diagnostics;

namespace DelegatesAndEvents
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BaseConsoleActions.PrepareConsole("Делегаты и события");

            ShowMainMenu();
        }

        private static void ShowMainMenu()
        {
            ConsoleMenu menu = new ConsoleMenu("Меню настроек")
            {
                MenuItems =
                {
                    new ConsoleMenuItem("Расширение IEnumerable"),
                    new ConsoleMenuItem("Вызвать GetMin", 1, (s, e) => { BaseIEnumerableAction(list => CallIEnumerableGetMin(list)); }),
                    new ConsoleMenuItem("Вызвать GetMax", 1, (s, e) => { BaseIEnumerableAction(list => CallIEnumerableGetMax(list)); }),
                    new ConsoleMenuItem("Вызвать GetAverage", 1, (s, e) => { BaseIEnumerableAction(list => CallIEnumerableGetAverage(list)); }),
                    new ConsoleMenuItem("Работа с файлами"),
                    new ConsoleMenuItem("Отобразить файлы из папки по пути", 1, (s, e) => { SearchFilesByFile(); }),
                    new ConsoleMenuItem("Проводник", 1, (s, e) => { ShowExplorer(); }),
                    new ConsoleMenuItem("Выход", 0, (s, e) => { BaseConsoleActions.Exit(); })
                }
            };
            menu.OnItemAdded += (sender, e) =>
            {
                menu.DisplayMenu();
            };
            menu.DisplayMenu();
        }
        
        private static void ShowExplorer(string path = "")
        {
            string label = string.IsNullOrEmpty(path) ? "Проводник" : $"Проводник: {path}";

            ConsoleMenu menu = new ConsoleMenu(label);

            Explorer explorer = new Explorer();
            explorer.OnItemFound += (object sender, ExplorerArgs e) => { OnExplorerItemFound(sender, e, menu); };

            if (string.IsNullOrEmpty(path))
            {
                menu.MenuItems.Add(new ConsoleMenuItem("Назад", 0, (s, e) => { ShowMainMenu(); }));
                explorer.SearchDrives();
            }
            else
            {
                if (explorer.GetAvailableDrives().Contains(path))
                {
                    menu.MenuItems.Add(new ConsoleMenuItem("..", 0, (s, e) => { ShowExplorer(); }));
                }
                else
                {
                    menu.MenuItems.Add(new ConsoleMenuItem("..", 0, (s, e) => { ShowExplorer(Directory.GetParent(path)?.FullName); }));
                }
                explorer.Search(path);
            }

            menu.OnItemAdded += (sender, e) =>
            {
                menu.DisplayMenu();
            };
            menu.DisplayMenu();
        }

        private static void OnFileFound(object sender, FileArgs e)
        {
            BaseConsoleActions.DisplayMessage($"\nФайл найден: {e.FileName}");

            // Cancelation logic
            if (e.FileName.Contains("specific_file_name"))
            {
                e.Cancel = true;
            }
        }

        private static void OnExplorerItemFound(object sender, ExplorerArgs explorerArgs, ConsoleMenu menu)
        {
            // Cancelation logic
            if (explorerArgs.Name.Contains("specific_file_name"))
            {
                explorerArgs.Cancel = true;
            }

            if (explorerArgs.IsDrive || explorerArgs.IsFolder)
            {
                menu.MenuItems.Add(new ConsoleMenuItem(explorerArgs.Name, 0, (s, e) => { ShowExplorer(explorerArgs.FullPath); }));
            }
            else
            {
                menu.MenuItems.Add(new ConsoleMenuItem(explorerArgs.Name, 0, (s, e) => { OpenFile(explorerArgs.FullPath); }));
            }
        }

        private static void SearchFilesByFile()
        {
            string path = BaseConsoleActions.AskForValidDirectoryPath();

            FileSearcher fileSearcher = new FileSearcher();
            fileSearcher.OnFileFound += OnFileFound;
            fileSearcher.Search(path);

            BaseConsoleActions.PressAnyToContinue(ShowMainMenu);
        }

        private static void OpenFile(string filePath)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при открытии файла: {ex.Message}");
            }
        }

        private static void CallIEnumerableGetMin(List<TestDataClass> list)
        {
            var result = list.GetMin(item => item.Value);
            BaseConsoleActions.DisplayMessageWithSpacing($"Наимешьшее: {result.Value}");
            BaseConsoleActions.PressAnyToContinue(ShowMainMenu);
        }

        private static void CallIEnumerableGetMax(List<TestDataClass> list)
        {
            var result = list.GetMax(item => item.Value);
            BaseConsoleActions.DisplayMessageWithSpacing($"Наибольшее: {result.Value}");
            BaseConsoleActions.PressAnyToContinue(ShowMainMenu);
        }

        private static void CallIEnumerableGetAverage(List<TestDataClass> list)
        {
            var result = list.GetAverage(item => item.Value);
            BaseConsoleActions.DisplayMessageWithSpacing($"Среднее значение: {result}");
            BaseConsoleActions.PressAnyToContinue(ShowMainMenu);
        }

        private static void BaseIEnumerableAction(Action<List<TestDataClass>> invokeAction)
        {
            int count = BaseConsoleActions.AskForValidIntegerInput("Элементов в коллекции (больше нуля): ", value => value > 0);
            int rangeStart = BaseConsoleActions.AskForValidIntegerInput("Начальное значение диапазона значений коллекции: ");
            int rangeEnd = BaseConsoleActions.AskForValidIntegerInput("Конечное значение диапазона значений коллекции (больше начального): ", value => value > rangeStart);

            List<TestDataClass> list = new List<TestDataClass>();
            for (int i = 0; i <= count - 1; i++)
            {
                list.Add(new TestDataClass(rangeStart, rangeEnd));
            }

            BaseConsoleActions.DisplayMessageWithSpacing($"Сгенерированные данные:\n{string.Join("; ", list.Select(x => $"Name: {x.Name}, Value: {x.Value}"))}");
            invokeAction(list);
        }
    }
}
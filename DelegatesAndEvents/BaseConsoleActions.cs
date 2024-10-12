namespace DelegatesAndEvents
{
    internal class BaseConsoleActions
    {
        public static int AskForValidIntegerInput(string question, Func<int, bool> additionalCheck = null)
        {
            DisplayMessage(question);

            int result;
            while (!(int.TryParse(Console.ReadLine(), out result) &&
                (additionalCheck == null || additionalCheck(result))))
            {
            }

            return result;
        }

        public static string AskForValidDirectoryPath()
        {
            DisplayMessageWithSpacing("Введите путь до папки: ");

            while (true)
            {
                
                string path = Console.ReadLine();

                if (Directory.Exists(path))
                {
                    return path;
                }
                else
                {
                }
            }
        }

        public static void DisplayMessage(string message)
        {
            Console.Write(message);
        }

        public static void DisplayMessageWithSpacing(string message)
        {
            Console.WriteLine($"\n{message}");
        }

        public static void PrepareConsole(string caption)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = caption;
        }

        public static void PressAnyToContinue(Action continueAction)
        {
            DisplayMessageWithSpacing("Нажмите любую клавишу для продолжения...");
            Console.ReadKey();

            continueAction();
        }

        public static void Exit()
        {
            DisplayMessageWithSpacing("Выход из программы.");
            Environment.Exit(0);
        }
    }
}

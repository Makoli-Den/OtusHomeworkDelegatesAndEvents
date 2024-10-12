namespace DelegatesAndEvents
{

    internal class FileSearcher
    {
        public event EventHandler<FileArgs> OnFileFound;

        public void Search(string directory)
        {
            var files = Directory.GetFiles(directory);

            foreach (var file in files)
            {
                var args = new FileArgs(file);
                FileFound(args);

                if (args.Cancel)
                {
                    BaseConsoleActions.DisplayMessageWithSpacing("Поиск файлов отменен.");
                    break;
                }
            }
        }
        protected virtual void FileFound(FileArgs e)
        {
            OnFileFound?.Invoke(this, e);
        }
    }
}

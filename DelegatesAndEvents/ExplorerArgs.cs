namespace DelegatesAndEvents
{
    internal class ExplorerArgs : EventArgs
    {
        public string Name { get; }
        public bool IsFile { get; }
        public bool IsDrive { get; }
        public bool IsFolder { get; }
        public string FullPath { get; }
        public string ShortPath { get; }
        public bool Cancel { get; set; }

        public ExplorerArgs(string fullPath, bool isFile = false, bool isFolder = false, bool isDrive = false)
        {
            IsFile = isFile;
            IsFolder = isFolder;
            IsDrive = isDrive;

            Name = Path.GetFileName(fullPath);
            FullPath = fullPath;
            ShortPath = Path.GetDirectoryName(fullPath) + "\\";

            if (IsDrive)
            {
                Name = fullPath;
            }
            
            Cancel = false;
        }
    }
}
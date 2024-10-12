namespace DelegatesAndEvents
{
    internal class ConsoleMenu
    {
        private const int ItemsPerPage = 10;
        private ObservableList<ConsoleMenuItem> _menuItems;
        private int _currentIndex;
        private int _currentPage;
        private int _totalPages;
        private string _title;

        public event EventHandler<ItemAddedEventArgs<ConsoleMenuItem>> OnItemAdded;
        public ObservableList<ConsoleMenuItem> MenuItems => _menuItems;

        public ConsoleMenu(string title, ObservableList<ConsoleMenuItem> menuItems)
        {
            _menuItems = menuItems;
            _menuItems.OnItemAdded += (sender, e) => OnItemAdded?.Invoke(sender, e);
            _title = title;
            UpdateTotalPages();
            _currentPage = 0;
            _currentIndex = 0;
        }

        public ConsoleMenu(string title)
        {
            _menuItems = new ObservableList<ConsoleMenuItem>();
            _menuItems.OnItemAdded += (sender, e) => OnItemAdded?.Invoke(sender, e);
            _title = title;
            UpdateTotalPages();
            _currentPage = 0;
            _currentIndex = 0;
        }

        public void DisplayMenu()
        {
            while (true)
            {
                Console.Clear();
                UpdateTotalPages();
                ShowTitle();
                ShowMenuItems();
                ShowPageInfo();
                HandleUserInput();
            }
        }

        private void ShowTitle()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(_title);
            Console.WriteLine(new string('-', _title.Length));
            Console.ResetColor();
        }

        private void ShowMenuItems()
        {
            int startIndex = _currentPage * ItemsPerPage;
            int endIndex = Math.Min(startIndex + ItemsPerPage, _menuItems.Count);

            for (int i = startIndex; i < endIndex; i++)
            {
                ShowMenuItem(_menuItems[i], i == _currentIndex);
            }
        }

        private void ShowMenuItem(ConsoleMenuItem item, bool isSelected)
        {
            string tabs = new string('\t', item.CaptionLevel);

            if (isSelected)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (!item.IsEnabled)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }

            Console.WriteLine($"{tabs}{item.Name}");
            Console.ResetColor();
        }

        private void ShowPageInfo()
        {
            if (_totalPages > 1)
            {
                Console.WriteLine($"\nСтраница {_currentPage + 1} из {_totalPages}");
                Console.WriteLine("Используйте стрелки влево/вправо для переключения страниц.");
            }
        }

        private void HandleUserInput()
        {
            if (!_menuItems.Any())
            {
                return;
            }

            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    MoveSelection(-1);
                    break;
                case ConsoleKey.DownArrow:
                    MoveSelection(1);
                    break;
                case ConsoleKey.Enter:
                    SelectItem();
                    break;
                case ConsoleKey.LeftArrow:
                    if (_currentPage > 0)
                    {
                        _currentPage--;
                        UpdateCurrentIndex();
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (_currentPage < _totalPages - 1)
                    {
                        _currentPage++;
                        UpdateCurrentIndex();
                    }
                    break;
            }
        }

        private void MoveSelection(int direction)
        {
            int itemsOnCurrentPage = Math.Min(ItemsPerPage, _menuItems.Count - (_currentPage * ItemsPerPage));
            do
            {
                _currentIndex = (_currentIndex + direction + _menuItems.Count) % _menuItems.Count;
            } while (!_menuItems[_currentIndex].IsEnabled ||
                     (_currentIndex / ItemsPerPage != _currentPage));
        }

        private void SelectItem()
        {
            if (_menuItems[_currentIndex].IsEnabled && _menuItems[_currentIndex].Selected != null)
            {
                _menuItems[_currentIndex].Selected.Invoke(_menuItems[_currentIndex], EventArgs.Empty);
                Console.ReadKey();
            }
        }

        private void UpdateTotalPages()
        {
            _totalPages = (int)Math.Ceiling((double)_menuItems.Count / ItemsPerPage);
        }

        private void UpdateCurrentIndex()
        {
            _currentIndex = _currentPage * ItemsPerPage;
        }
    }
}

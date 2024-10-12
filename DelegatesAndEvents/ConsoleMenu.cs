namespace DelegatesAndEvents
{
    internal class ConsoleMenu
    {
        private ObservableList<ConsoleMenuItem> _menuItems;
        private int _currentIndex;
        private string _title;

        public event EventHandler<ItemAddedEventArgs<ConsoleMenuItem>> OnItemAdded;
        public ObservableList<ConsoleMenuItem> MenuItems => _menuItems;
        public int CurrentIndex => _currentIndex;
        public string Title => _title;

        public ConsoleMenu(string title, ObservableList<ConsoleMenuItem> menuItems)
        {
            _menuItems = menuItems;
            _menuItems.OnItemAdded += (sender, e) =>
            {
                OnItemAdded?.Invoke(sender, e);
            };

            _title = title;
            _currentIndex = FindFirstEnabledIndex();
        }

        public ConsoleMenu(string title)
        {
            _menuItems = new ObservableList<ConsoleMenuItem>();
            _menuItems.OnItemAdded += (sender, e) =>
            {
                OnItemAdded?.Invoke(sender, e);
            };

            _title = title;
            _currentIndex = FindFirstEnabledIndex();
        }

        public void DisplayMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.Clear();
                ShowTitle();
                ShowMenuItems();
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
            for (int i = 0; i < _menuItems.Count; i++)
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
            }
        }

        private void MoveSelection(int direction)
        {
            do
            {
                _currentIndex = (_currentIndex + direction + _menuItems.Count) % _menuItems.Count;
            } while (!_menuItems[_currentIndex].IsEnabled);
        }

        private int FindFirstEnabledIndex()
        {
            for (int i = 0; i < _menuItems.Count; i++)
            {
                if (_menuItems[i].IsEnabled)
                {
                    return i;
                }
            }
            return 0;
        }

        private void SelectItem()
        {
            if (_menuItems[_currentIndex].IsEnabled && _menuItems[_currentIndex].Selected != null)
            {
                _menuItems[_currentIndex].Selected.Invoke(_menuItems[_currentIndex], EventArgs.Empty);
                Console.ReadKey();
            }
        }
    }
}

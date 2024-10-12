using System.Collections;

namespace DelegatesAndEvents
{
    internal class ObservableList<T> : IEnumerable<T>
    {
        private readonly List<T> _list = new List<T>();

        public event EventHandler<ItemAddedEventArgs<T>> OnItemAdded;

        public void Add(T item)
        {
            _list.Add(item);
            ItemAdded(item);
        }

        protected virtual void ItemAdded(T item)
        {
            OnItemAdded?.Invoke(this, new ItemAddedEventArgs<T>(item));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public List<T> ToList()
        {
            return _list.ToList();
        }

        public T[] ToArray()
        {
            return _list.ToArray();
        }

        public IEnumerable<T> AsEnumerable()
        {
            return _list.AsEnumerable();
        }

        public int Count => _list.Count;

        public T this[int index]
        {
            get => _list[index];
            set => _list[index] = value;
        }
    }
}

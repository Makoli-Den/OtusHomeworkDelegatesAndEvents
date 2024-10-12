namespace DelegatesAndEvents
{
    internal class ItemAddedEventArgs<T> : EventArgs
    {
        public T AddedItem { get; }

        public ItemAddedEventArgs(T addedItem)
        {
            AddedItem = addedItem;
        }
    }
}

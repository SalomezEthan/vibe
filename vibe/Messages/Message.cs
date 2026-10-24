namespace vibe.Messages
{
    public abstract class Message<T>
    {
        public Message(T item)
        {
            this.Value = item;
        }

        public T Value { get; }
    }
}

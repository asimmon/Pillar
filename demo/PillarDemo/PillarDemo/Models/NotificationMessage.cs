namespace PillarDemo.Models
{
    public class NotificationMessage<T>
    {
        public T Content { get; private set; }

        public string Notification { get; private set; }

        public NotificationMessage(T content, string notification)
        {
            Content = content;
            Notification = notification;
        }

        public NotificationMessage(T content)
            : this(content, string.Empty)
        { }
    }
}

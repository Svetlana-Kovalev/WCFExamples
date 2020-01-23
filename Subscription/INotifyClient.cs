namespace Subscription
{
    public delegate void NotifyClientEventHandler(object sender, object data, ServiceType type);

    public interface INotifyClient
    {
        event NotifyClientEventHandler NotifyClient;
    }
}

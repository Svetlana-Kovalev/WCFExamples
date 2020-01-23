namespace Subscription
{
    public delegate void NotifyUiEventHandler(object sender, string data, ServiceType type);

    public interface INotifyEvent
    {
        event NotifyUiEventHandler NotifyUi;
    }
}

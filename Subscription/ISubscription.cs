using System.ServiceModel;

namespace Subscription
{
    [ServiceContract(CallbackContract = typeof(IPublishing))]
    public interface ISubscription
    {
        [OperationContract]
        void Subscribe(int clientId);

        [OperationContract]
        void UnSubscribe(int clientId);

        [OperationContract]
        void KeepAlive(int clientId);

        [OperationContract]
        void SendData(int clientId, string data);

        // ReflectionService
        [OperationContract]
        void RegisterClient(ServiceType type, IManager manager);
        [OperationContract]
        void Invoke(ServiceType type, string data);
        [OperationContract]
        object Get(ServiceType type, string data);
        [OperationContract]
        object GetPropertyData(ServiceType type, string data);
        [OperationContract]
        string GetDeviceType(ServiceType type);
    }
}

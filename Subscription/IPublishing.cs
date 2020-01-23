using System.ServiceModel;

namespace Subscription
{
    [ServiceContract]
    public interface IPublishing
    {
        [OperationContract(IsOneWay = true)]
        void ServerNotify(string state);

        // Reflection Service
        [OperationContract]
        void ReflectionNotify(ServiceType type, string data);
    }
}

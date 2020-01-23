using System.ServiceModel;

namespace MessagesPublisher
{
    [ServiceContract]
    public interface IClientCallback
    {
        [OperationContract(IsOneWay = true)]
        void CallToMyClient(string message);
    }
}
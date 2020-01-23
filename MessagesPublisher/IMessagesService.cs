using System.Runtime.Serialization;
using System.ServiceModel;

namespace MessagesPublisher
{
    [ServiceContract(CallbackContract = typeof(IClientCallback))]
    public interface IMessagesService
    {
        [OperationContract(IsOneWay = true)]
        void SubscribeClient();

        [OperationContract(IsOneWay = true)]
        void UnsubscribeClient();

        [OperationContract]
        string GetData();

        [OperationContract]
        void SendData(string message);
        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
    }

    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }


}
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace MessagesPublisher
{
   [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class MessagesService : IMessagesService
    {
        static string _msgenum = "";
        static string _message = "";
        static string _msgtype = "";
        static string _msgtime = "";

        private readonly List<IClientCallback> CallbackChannels = new List<IClientCallback>();


        public MessagesService()
        {

        }
        public void SubscribeClient()
        {
            IClientCallback channel = OperationContext.Current.GetCallbackChannel<IClientCallback>();
            if (!CallbackChannels.Contains(channel))
            {
                CallbackChannels.Add(channel);
            }
        }

        public void UnsubscribeClient()
        {
            IClientCallback channel = OperationContext.Current.GetCallbackChannel<IClientCallback>();
            if (CallbackChannels.Contains(channel))
            {
                CallbackChannels.Remove(channel);
            }
        }
        public string GetData()
        {
            return $"{_msgenum}|{_msgtype}|{_message}|{_msgtime}";
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
        public void SendData(string message)
        {
            if (CallbackChannels.Count > 0)
            {

                IClientCallback callbackChannel = CallbackChannels[0];
                callbackChannel.CallToMyClient(message);
            }
        }
    }
}

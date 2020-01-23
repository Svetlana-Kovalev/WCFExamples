using System;
using System.Collections.ObjectModel;
using System.IO;
using System.ServiceModel;
using Caliburn.Micro;
using MessagesPublisher;

namespace WCFSimpleExample.ViewModels
{
public class WCFSimpleExampleClientCallback : IClientCallback
{
    ShellViewModel _client;

    public WCFSimpleExampleClientCallback(ShellViewModel client)
    {
        _client = client;
    }

    public void CallToMyClient(string message)
    {
        _client.UpdateMessages(message);
        Console.WriteLine(@"CallToMyClient ShellViewModel");
    }

}
    public class ShellViewModel : Screen
    {
        IClientCallback _clientCallback;
        IMessagesService _messagesService;
        DuplexChannelFactory<IMessagesService> _factory;

        private string displayName = "Main Client";
        public override string DisplayName
        {
            get => displayName;
            set
            {
                displayName = value;
                NotifyOfPropertyChange();
            }
        }


        private string displayText = "WCF EXAMPLE";
        public string DisplayText
        {
            get => displayText;
            set
            {
                displayText = value;
                NotifyOfPropertyChange();
            }
        }
        public ObservableCollection<string> Messages { get; set; }
        public ShellViewModel()
        {
            //string dir = Path.GetDirectoryName(typeof(AppMainViewModel).Assembly.Location);
            //if (dir != null) Environment.CurrentDirectory = dir;

            DisplayText = "My text";
            try
            {
                EndpointAddress ep = null;

                string strAdr = "net.tcp://localhost:6611/MessagesService/";

                NetTcpBinding tcpb = new NetTcpBinding();
                _factory = new DuplexChannelFactory<IMessagesService>(tcpb);
                ep = new EndpointAddress(strAdr);

                _clientCallback = new WCFSimpleExampleClientCallback(this);

                DuplexChannelFactory<IMessagesService> factory =
                    new DuplexChannelFactory<IMessagesService>(
                        _clientCallback,
                        tcpb,
                        ep);

                _messagesService = factory.CreateChannel();
                _messagesService?.SubscribeClient();

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                
            }

        }
        public void UpdateMessages(string message)
        {
            // Messages.Add(message);
            DisplayText = message;
        }
    }
}

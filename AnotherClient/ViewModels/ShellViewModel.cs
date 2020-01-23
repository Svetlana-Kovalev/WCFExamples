using System;
using System.ServiceModel;
using System.Timers;
using Caliburn.Micro;
using MessagesPublisher;

namespace AnotherClient.ViewModels
{
    public class AnotherClientCallback : IClientCallback
    {
        ShellViewModel _client;

        public AnotherClientCallback(ShellViewModel client)
        {
            _client = client;
        }
        public void CallToMyClient(string message)
        {

        }
    }
    public class ShellViewModel : Screen
    {
        IMessagesService _logMessagesService;

        private string displayName = "Another Client";
        public override string DisplayName
        {
            get => displayName;
            set
            {
                displayName = value;
                NotifyOfPropertyChange();
            }
        }

        public ShellViewModel()
        {
            Timer timer = new Timer(1000);
            timer.Elapsed += Timer_Elapsed;

            AnotherClientCallback _clientCallback;
            string strAdr = "net.tcp://localhost:6611/MessagesService/";
            EndpointAddress _ep = null;

            DuplexChannelFactory<IMessagesService> factory = null;
            try
            {
                NetTcpBinding tcpb = new NetTcpBinding();
                _ep = new EndpointAddress(strAdr);

                _clientCallback = new AnotherClientCallback(this);

                factory =
                    new DuplexChannelFactory<IMessagesService>(
                        _clientCallback,
                        tcpb,
                        _ep);

                _logMessagesService = factory.CreateChannel();
                _logMessagesService?.SendData("My message");
                timer.Start();
            }
            catch (Exception exception)
            {
                factory?.Close();
                Console.WriteLine(exception);
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _logMessagesService?.SendData("My message on " + DateTime.Now);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using MessagesPublisher;

namespace HostService
{
    class Program
    {
        private static ServiceHost _svcHost;
        private static ServiceHost _subscriptionServiceHost;

        static void Main(string[] args)
        {
            StartTCPService();
        }

        private static void StartTCPService()
        {
            //_subscriptionServiceHost = new ServiceHost(reflectionServiceManager);

            string strAdr = "net.tcp://localhost:6611/MessagesService/";
            NetTcpBinding tcpb = new NetTcpBinding();
            try
            {
                Uri adrbase = new Uri(strAdr);
                _svcHost = new ServiceHost(typeof(MessagesService), adrbase);
                //  NetTcpBinding tcpb = new NetTcpBinding();

                ServiceMetadataBehavior mBehave = new ServiceMetadataBehavior();
                _svcHost.Description.Behaviors.Add(mBehave);
                _svcHost.AddServiceEndpoint(typeof(IMetadataExchange),
                    MetadataExchangeBindings.CreateMexTcpBinding(), "mex");

                _svcHost.AddServiceEndpoint(typeof(IMessagesService), tcpb, strAdr);
                _svcHost.Open();
                Console.WriteLine("\n\nService is Running as >> " + strAdr);
                Console.ReadKey();
            }
            catch (Exception eX)
            {
                _svcHost = null;
                Console.WriteLine("Service can not be started as >> [" +
                                  strAdr + "] \n\nError Message [" + eX.Message + "]");
            }
        }
    }

}

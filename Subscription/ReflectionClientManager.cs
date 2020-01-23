using Castle.DynamicProxy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Subscription
{
    public class ReflectionClientManager : IPublishing, INotifyClient
    {
        #region Properties
        ISubscription _proxy;
        string _endpoint = string.Empty;
        public string IPAddress { get; set; }
        public event NotifyClientEventHandler NotifyClient;
        #endregion

        public ReflectionClientManager()
        {

        }

        public void MakeProxy(string ip, object callbackinstance)
        {
            IPAddress = ip;
            _endpoint = $"net.tcp://{IPAddress}:7052/ReflectionServiceManager";//ConfigurationManager.AppSettings["EndpointAddress"]; 

            NetTcpBinding netTcpbinding = new NetTcpBinding(SecurityMode.None);
            netTcpbinding.MaxBufferPoolSize = 50000000;
            netTcpbinding.MaxBufferSize = 50000000;
            netTcpbinding.MaxReceivedMessageSize = 50000000;
            netTcpbinding.ReliableSession.Enabled = true;
            netTcpbinding.ReliableSession.InactivityTimeout = System.TimeSpan.FromMinutes(10);
            netTcpbinding.ReceiveTimeout = System.TimeSpan.FromMinutes(10);
            EndpointAddress endpointAddress = new EndpointAddress(_endpoint);
            InstanceContext context = new InstanceContext(callbackinstance);

            DuplexChannelFactory<ISubscription> channelFactory = new DuplexChannelFactory<ISubscription>(new InstanceContext(this), netTcpbinding, _endpoint);
            _proxy = channelFactory.CreateChannel();
        }

        #region Client Methods
        public bool SendCommandToManager(ServiceType type, string data)
        {
            try
            {
                if (data != null && !data.Equals(String.Empty))
                {
                    _proxy.Invoke(type, data);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public object GetFromManager(ServiceType type, string data)
        {
            try
            {
                if (data != null && !data.Equals(String.Empty))
                {
                    var temp = _proxy.Get(type, data);
                    DataBoundary serialized = JsonConvert.DeserializeObject<DataBoundary>(temp.ToString());
                    var vals = serialized.Attributes.FirstOrDefault();
                    object reflected;
                    if (vals.Value.GetType().IsEquivalentTo(typeof(Newtonsoft.Json.Linq.JArray)))
                    {
                        reflected = ((Newtonsoft.Json.Linq.JArray)vals.Value).ToObject(Type.GetType(vals.Key.ToString()));
                    }
                    else if (vals.Value.GetType().IsEquivalentTo(typeof(Newtonsoft.Json.Linq.JObject)))
                        reflected = ((Newtonsoft.Json.Linq.JObject)vals.Value).ToObject(Type.GetType(vals.Key.ToString()));
                    else
                    {
                        var typo = Type.GetType(vals.Key.ToString());
                        if (typo.IsEnum)
                            reflected = Enum.Parse(typo, vals.Value.ToString());
                        else
                            reflected = vals.Value;
                    }
                    return reflected;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public object GetPropertyFromManager(ServiceType type, string data)
        {
            try
            {
                if (data != null && !data.Equals(String.Empty))
                {
                    var temp = _proxy.GetPropertyData(type, data);
                    DataBoundary serialized = JsonConvert.DeserializeObject<DataBoundary>(temp.ToString());
                    var vals = serialized.Attributes.FirstOrDefault();
                    object reflected;
                    if (Type.GetType(vals.Key.ToString()).IsEnum)
                        reflected = Enum.Parse(Type.GetType(vals.Key.ToString()), vals.Value.ToString());
                    else
                        reflected = ((Newtonsoft.Json.Linq.JArray)vals.Value).ToObject(Type.GetType(vals.Key.ToString()));
                    return reflected;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public object GetDevice(ServiceType type)
        {
            string s = _proxy.GetDeviceType(type);

            var typeClass = JsonConvert.DeserializeObject<Type>(s);
            //new JsonSerializerSettings
            //{
            //    NullValueHandling = NullValueHandling.Ignore
            //});  

            ProxyGenerator generator = new ProxyGenerator();
            var proxyClass = generator.CreateClassProxy(typeClass, new DeviceInterceptor());
            return proxyClass;
        }

        public void RegisterReflectedDevice(ServiceType type, IManager manager)
        {
            _proxy.RegisterClient(type, manager);
        }

        public void ReflectionNotify(ServiceType type, string data)
        {
            var result = JsonConvert.DeserializeObject<DataBoundary>(data);

            var returnedAnswer = result.Attributes.FirstOrDefault(r => r.Key.Equals("Result"));
            if (!returnedAnswer.IsNull())
                NotifyClient?.Invoke(this, returnedAnswer.Value, type);
            else
                NotifyClient?.Invoke(this, result, type);
        }

        public void ServerNotify(string state)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    #region Utilites
    public static class KeyValuePairExtensions
    {
        public static bool IsNull<T, TU>(this KeyValuePair<T, TU> pair)
        {
            return pair.Equals(new KeyValuePair<T, TU>());
        }
    }
    #endregion

}

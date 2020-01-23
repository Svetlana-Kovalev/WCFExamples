using Castle.DynamicProxy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Subscription
{
    public class DeviceInterceptor : Interceptor
    {
        ReflectionClientManager _reflectionClientManager;
        public DeviceInterceptor()
        {
            _reflectionClientManager = new ReflectionClientManager();
        }
        protected override void ExecuteAfter(IInvocation invocation)
        {
            Console.WriteLine("End");
        }

        protected override void ExecuteBefore(IInvocation invocation)
        {
            Console.WriteLine("Start");

            DataBoundary data = new DataBoundary()
            {
                CommandName = invocation.Method.Name,
                Attributes = new Dictionary<object, object>()
            };

            invocation.Arguments.ToList().ForEach(arg => data.Attributes.Add(arg.GetType(), arg));
            if (data.CommandName.Contains("get_") || data.CommandName.Contains("set_"))
            {
                data.CommandName = data.CommandName.Remove(0, 4);
                invocation.ReturnValue = _reflectionClientManager.GetPropertyFromManager(GetServiceType(invocation.Proxy.ToString()), JsonConvert.SerializeObject(data));
            }
            else
            {
                invocation.ReturnValue = _reflectionClientManager.GetFromManager(GetServiceType(invocation.Proxy.ToString()), JsonConvert.SerializeObject(data));
            }
        }

        private ServiceType GetServiceType(string name) => Enum.GetValues(typeof(ServiceType)).Cast<ServiceType>().FirstOrDefault(e => name.ToUpper().Contains(e.GetDescription()));
    }

}

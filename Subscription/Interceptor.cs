using Castle.DynamicProxy;

namespace Subscription
{
    public abstract class Interceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            ExecuteBefore(invocation);
            //invocation.Proceed();
            ExecuteAfter(invocation);
        }

        protected abstract void ExecuteAfter(IInvocation invocation);
        protected abstract void ExecuteBefore(IInvocation invocation);
    }

}

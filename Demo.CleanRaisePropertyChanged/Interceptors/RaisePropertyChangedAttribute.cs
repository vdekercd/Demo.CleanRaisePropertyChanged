using System;
using Unity;
using Unity.Interception.PolicyInjection.Pipeline;
using Unity.Interception.PolicyInjection.Policies;

namespace Demo.CleanRaisePropertyChanged.Interceptors
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RaisePropertyChangedAttribute : HandlerAttribute
    {
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return new RaisePropertyChangedCallHandler();
        }
    }
}

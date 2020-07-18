using System;
using System.ComponentModel;
using System.Reflection;
using Unity.Interception.PolicyInjection.Pipeline;

namespace Demo.CleanRaisePropertyChanged.Interceptors
{
    public class RaisePropertyChangedCallHandler : ICallHandler
    {
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            if (ShouldRaiseEvent(input)) RaiseEvent(input);
            return getNext()(input, getNext);
        }

        public int Order { get; set; }

        private bool ShouldRaiseEvent(IMethodInvocation input)
        {
            var methodBase = input.MethodBase;

            if (!methodBase.IsSpecialName || !methodBase.Name.StartsWith("set_"))
                return false;
            
            var property = methodBase.ReflectedType.GetProperty(methodBase.Name.Substring(4));
            var getMethod = property.GetGetMethod();

            if (getMethod == null)
                return false;

            var oldValue = getMethod.Invoke(input.Target, null);
            var value = input.Arguments[0];

            if (value != null && value.Equals(oldValue) == false)
                return true;

            return null != oldValue;
        }

        private void RaiseEvent(IMethodInvocation input)
        {
            FieldInfo field = null;

            var type = input.MethodBase.ReflectedType;
            while (field == null && type != null)
            {
                field = type.GetField("PropertyChanged", BindingFlags.Instance | BindingFlags.NonPublic);
                type = type.BaseType;
            }

            if (field == null) return;
            if (field.GetValue(input.Target) is Delegate eventDelegate)
            {
                var propertyName = input.MethodBase.Name.Substring(4);
                eventDelegate.DynamicInvoke(input.Target, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

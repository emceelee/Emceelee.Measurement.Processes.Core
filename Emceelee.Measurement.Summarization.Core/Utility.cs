using System;
using System.Collections.Generic;
using System.Text;

namespace Emceelee.Measurement.Summarization.Core
{
    public static class Utility
    {
        //Create Delegates for performance improvements vs Reflection
        public static Func<TObj, TProp> CreateGetterDelegate<TObj, TProp>(string propName)
        {
            var propInfo = typeof(TObj).GetProperty(propName);
            if(propInfo != null)
            {
                return (Func<TObj, TProp>)Delegate.CreateDelegate(typeof(Func<TObj, TProp>), null, propInfo.GetGetMethod());
            }
            throw new ArgumentException($"{propName} is not a property of {typeof(TObj).Name}");
        }
        public static Action<TObj, TProp> CreateSetterDelegate<TObj, TProp>(string propName)
        {
            var propInfo = typeof(TObj).GetProperty(propName);
            if (propInfo != null)
            {
                return (Action<TObj, TProp>)Delegate.CreateDelegate(typeof(Action<TObj, TProp>), null, propInfo.GetSetMethod());
            }
            throw new ArgumentException($"{propName} is not a property of {typeof(TObj).Name}");
        }

    }
}

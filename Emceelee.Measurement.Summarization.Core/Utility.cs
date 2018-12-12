using System;
using System.Collections.Generic;
using System.Text;

namespace Emceelee.Measurement.Summarization.Core
{
    public static class Utility
    {
        public static Func<TObj, TProp> CreateGetterDelegate<TObj, TProp>(string propName)
        {
            return (Func<TObj, TProp>)Delegate.CreateDelegate(typeof(Func<TObj, TProp>), null, typeof(TObj).GetProperty(propName).GetGetMethod());
        }
        public static Action<TObj, TProp> CreateSetterDelegate<TObj, TProp>(string propName)
        {
            return (Action<TObj, TProp>)Delegate.CreateDelegate(typeof(Action<TObj, TProp>), null, typeof(TObj).GetProperty(propName).GetSetMethod());
        }

    }
}

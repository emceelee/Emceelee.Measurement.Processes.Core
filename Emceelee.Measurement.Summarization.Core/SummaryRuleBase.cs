using System;
using System.Collections.Generic;
using System.Text;

namespace Emceelee.Measurement.Summarization.Core
{
    public abstract class SummaryRuleBase<TObj, TProperty>
    {
        public TProperty Execute(IEnumerable<TObj> records, Func<TObj, TProperty> func)
        {
            return InternalExecute(records, func);
        }

        protected abstract TProperty InternalExecute(IEnumerable<TObj> records, Func<TObj, TProperty> func);
    }
}

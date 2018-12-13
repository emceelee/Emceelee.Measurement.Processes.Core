using System;
using System.Collections.Generic;
using System.Text;

namespace Emceelee.Measurement.Summarization.Core
{
    public abstract class SummaryRuleBase<TObj, TProperty>
    {
        public bool Execute(IEnumerable<TObj> records, Func<TObj, TProperty> func, out TProperty result)
        {
            return InternalExecute(records, func, out result);
        }

        //Return false to indicate rule execution failed
        protected abstract bool InternalExecute(IEnumerable<TObj> records, Func<TObj, TProperty> func, out TProperty result);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Emceelee.Summarization.Core
{
    public abstract class SummaryRuleBase<TObj, TProperty>
    {
        public bool Execute(IEnumerable<TObj> records, Func<TObj, TProperty> func, ISummaryContext context, out TProperty result)
        {
            return InternalExecute(records, func, context, out result);
        }

        //Return false to indicate rule execution failed
        protected abstract bool InternalExecute(IEnumerable<TObj> records, Func<TObj, TProperty> func, ISummaryContext context, out TProperty result);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Emceelee.Summarization.Core.Rules
{
    public abstract class SummaryRuleBase<TObj, TProperty>
    {
        public bool Execute(IEnumerable<TObj> records, Func<TObj, TProperty> func, out TProperty result)
        {
            return Execute(records, func, null, out result);
        }

        public bool Execute(IEnumerable<TObj> records, Func<TObj, TProperty> func, ISummaryContext context, out TProperty result)
        {
            if(records == null)
            {
                throw new ArgumentNullException($"SummaryRuleBase: {nameof(records)} must not be null");
            }
            return InternalExecute(records, func, context, out result);
        }

        //Return false to indicate rule execution failed
        protected abstract bool InternalExecute(IEnumerable<TObj> records, Func<TObj, TProperty> func, ISummaryContext context, out TProperty result);
    }
}

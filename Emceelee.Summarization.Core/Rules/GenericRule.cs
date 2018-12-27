using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emceelee.Summarization.Core;

namespace Emceelee.Summarization.Core.Rules
{
    public class GenericRule<TObj, TProperty> : SummaryRuleBase<TObj, TProperty>
    {
        public Func<IEnumerable<TObj>, ISummaryContext, TProperty> SummaryFunction { get; }
        public GenericRule(Func<IEnumerable<TObj>, ISummaryContext, TProperty> func)
        {
            SummaryFunction = func;
        }
        protected override bool InternalExecute(IEnumerable<TObj> records, Func<TObj, TProperty> func, ISummaryContext context, out TProperty result)
        {
            result = default(TProperty);

            if(SummaryFunction != null)
            {
                result = SummaryFunction(records, context);
                return true;
            }

            return false;
        }
    }
}

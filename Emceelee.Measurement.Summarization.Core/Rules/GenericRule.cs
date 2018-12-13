using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emceelee.Measurement.Summarization.Core.Rules
{
    public class GenericRule<TObj, TProperty> : SummaryRuleBase<TObj, TProperty>
    {
        public Func<IEnumerable<TObj>, TProperty> SummaryFunction { get; }
        public GenericRule(Func<IEnumerable<TObj>, TProperty> func)
        {
            SummaryFunction = func;
        }
        protected override bool InternalExecute(IEnumerable<TObj> records, Func<TObj, TProperty> func, out TProperty result)
        {
            result = default(TProperty);

            if(SummaryFunction != null)
            {
                result = SummaryFunction(records);
                return true;
            }

            return false;
        }
    }
}

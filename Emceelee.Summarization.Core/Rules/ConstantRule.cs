using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emceelee.Summarization.Core.Rules
{
    public class ConstantRule<TObj, TProp> : SummaryRuleBase<TObj, TProp>
    {
        private TProp Value;

        public ConstantRule(TProp value)
        {
            Value = value;
        }

        protected override bool InternalExecute(IEnumerable<TObj> records, Func<TObj, TProp> func, ISummaryContext context, out TProp result)
        {
            result = Value;
            return true;
        }
    }
}

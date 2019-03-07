using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emceelee.Summarization.Core.Rules
{
    public class FloorRule<T> : SummaryRuleBase<T, double?>
    {
        private SummaryRuleBase<T, double?> InternalRule { get; }

        public FloorRule(SummaryRuleBase<T, double?> internalRule)
        {
            InternalRule = internalRule;
        }
        
        protected override bool InternalExecute(IEnumerable<T> records, Func<T, double?> func, ISummaryContext context, out double? result)
        {
            result = null;
            bool success = InternalRule?.Execute(records, func, context, out result) ?? false;

            if (success)
            {
                if(result != null)
                {
                    result = Math.Floor(result ?? 0);
                }
            }

            return success;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emceelee.Summarization.Core.Rules
{
    public class RoundRule<T> : SummaryRuleBase<T, double?>
    {
        private SummaryRuleBase<T, double?> InternalRule { get; }
        private int Decimals { get; }
        private MidpointRounding Mode { get; }

        public RoundRule(SummaryRuleBase<T, double?> internalRule, int decimals = 0, MidpointRounding mode = MidpointRounding.ToEven)
        {
            InternalRule = internalRule;
            Decimals = decimals;
            Mode = mode;
        }
        
        protected override bool InternalExecute(IEnumerable<T> records, Func<T, double?> func, ISummaryContext context, out double? result)
        {
            result = null;
            bool success = InternalRule?.Execute(records, func, context, out result) ?? false;

            if (success)
            {
                if(result != null)
                {
                    result = Math.Round(result ?? 0, Decimals, Mode);
                }
            }

            return success;
        }
    }
}

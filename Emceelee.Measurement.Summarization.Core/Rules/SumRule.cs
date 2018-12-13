using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emceelee.Measurement.Summarization.Core.Rules
{
    public class SumRule<T> : SummaryRuleBase<T, double?>
    {
        protected override bool InternalExecute(IEnumerable<T> records, Func<T, double?> func, out double? result)
        {
            result = null;

            if(records.Any(t => func(t) != null))
            {
                result = records.Sum(t => func(t) ?? 0);
            }

            return true;
        }
    }
}

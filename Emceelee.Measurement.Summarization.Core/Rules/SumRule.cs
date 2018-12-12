using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emceelee.Measurement.Summarization.Core.Rules
{
    public class SumRule<T> : SummaryRuleBase<T, double?>
    {
        protected override double? InternalExecute(IEnumerable<T> records, Func<T, double?> func)
        {
            if(records.Any(t => func(t) != null))
            {
                double sum = records.Sum(t => func(t) ?? 0);
                return sum;
            }
            return null;
        }
    }
}

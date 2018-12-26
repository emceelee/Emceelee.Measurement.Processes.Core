using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Emceelee.Summarization.Core;

namespace Emceelee.Summarization.Core.Rules
{
    public class SumRule<T> : SummaryRuleBase<T, double?>
    {
        protected override bool InternalExecute(IEnumerable<T> records, Func<T, double?> func, ISummaryContext context, out double? result)
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

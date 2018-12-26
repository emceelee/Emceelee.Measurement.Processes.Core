using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emceelee.Summarization.Core;

namespace Emceelee.Summarization.Core.Rules
{
    public class SimpleAverageRule<T> : SummaryRuleBase<T, double?>
    {
        protected override bool InternalExecute(IEnumerable<T> records, Func<T, double?> func, ISummaryContext context, out double? result)
        {
            result = null;

            if (records.Any(t => func(t) != null))
            {
                double sum = records.Sum(t => (func(t) ?? 0));
                double count = records.Count(t => func(t) != null);

                if (count != 0)
                {
                    result = sum / count;
                }
            }

            return true;
        }
    }
}

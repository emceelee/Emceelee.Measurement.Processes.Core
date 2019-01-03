using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emceelee.Summarization.Core;

namespace Emceelee.Measurement.Summarization.Core.Rules
{
    public abstract class MeasurementSummaryRuleBase<T> : SummaryRuleBase<T, double?>
    {
        protected override bool InternalExecute(IEnumerable<T> records, Func<T, double?> func, ISummaryContext context, out double? result)
        {
            result = null;

            if(context is SummaryContext summaryContext)
            {
                return InternalExecute(records, func, summaryContext, out result);
            }

            return false;
        }

        protected abstract bool InternalExecute(IEnumerable<T> records, Func<T, double?> func, SummaryContext context, out double? result);
    }
}

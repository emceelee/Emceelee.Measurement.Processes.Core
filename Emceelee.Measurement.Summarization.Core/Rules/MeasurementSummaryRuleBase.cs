using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emceelee.Summarization.Core;
using Emceelee.Summarization.Core.Rules;

namespace Emceelee.Measurement.Summarization.Core.Rules
{
    public abstract class MeasurementSummaryRuleBase<TObj, TProp> : SummaryRuleBase<TObj, TProp>
    {
        protected override bool InternalExecute(IEnumerable<TObj> records, Func<TObj, TProp> func, ISummaryContext context, out TProp result)
        {
            result = default(TProp);

            if(context is SummaryContext summaryContext)
            {
                return InternalExecute(records, func, summaryContext, out result);
            }

            return false;
        }

        protected abstract bool InternalExecute(IEnumerable<TObj> records, Func<TObj, TProp> func, SummaryContext context, out TProp result);
    }
}

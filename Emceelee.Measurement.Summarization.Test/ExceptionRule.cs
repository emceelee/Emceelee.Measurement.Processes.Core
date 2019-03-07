using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emceelee.Summarization.Core;
using Emceelee.Summarization.Core.Rules;

namespace Emceelee.Measurement.Summarization.Test
{
    public class ExceptionRule<TObj, TProperty> : SummaryRuleBase<TObj, TProperty>
    {
        protected override bool InternalExecute(IEnumerable<TObj> records, Func<TObj, TProperty> func, ISummaryContext context, out TProperty result)
        {
            throw new NotImplementedException();
        }
    }
}

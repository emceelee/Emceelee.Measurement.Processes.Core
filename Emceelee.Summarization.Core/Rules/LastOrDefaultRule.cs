using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emceelee.Summarization.Core.Rules
{
    public class LastOrDefaultRule<TObj, TProp> : SummaryRuleBase<TObj, TProp>
    {
        private Func<TObj, dynamic> OrderByFunction { get; }

        public LastOrDefaultRule(Func<TObj, dynamic> orderByFunction)
        {
            OrderByFunction = orderByFunction;
        }

        protected override bool InternalExecute(IEnumerable<TObj> records, Func<TObj, TProp> func, ISummaryContext context, out TProp result)
        {
            result = default(TProp);
            var obj = records.OrderByDescending(OrderByFunction).FirstOrDefault();

            if (obj != null)
            {
                result = func(obj);
            }

            return true;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emceelee.Summarization.Core.Rules
{
    public class FirstOrDefaultRule<TObj, TProp> : SummaryRuleBase<TObj, TProp>
    {
        private Func<TObj, dynamic> OrderByFunction { get; }

        public FirstOrDefaultRule(Func<TObj, dynamic> orderByFunction)
        {
            OrderByFunction = orderByFunction;
        }
        
        protected override bool InternalExecute(IEnumerable<TObj> records, Func<TObj, TProp> func, ISummaryContext context, out TProp result)
        {
            result = default(TProp);
            var obj = records.OrderBy(OrderByFunction).FirstOrDefault();

            if(obj != null)
            {
                result = func(obj);
            }

            return true;
        }
    }
}
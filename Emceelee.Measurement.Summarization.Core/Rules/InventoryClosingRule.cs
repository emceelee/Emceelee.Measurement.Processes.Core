using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emceelee.Summarization.Core;

namespace Emceelee.Measurement.Summarization.Core.Rules
{
    public class InventoryClosingRule<TObj, TProp> : MeasurementSummaryRuleBase<TObj, TProp> where TObj : IMeasurementGroupable
    {
        public Func<TObj, DateTime> DateFunction { get; }

        public InventoryClosingRule(Func<TObj, DateTime> dateFunc)
        {
            DateFunction = dateFunc;
        }

        protected override bool InternalExecute(IEnumerable<TObj> records, Func<TObj, TProp> func, SummaryContext summaryContext, out TProp result)
        {
            result = default(TProp);

            TObj record = records.FirstOrDefault(t => DateFunction(t) == summaryContext.ProductionDateEnd);
            if(record != null)
            {
                result = func(record);
                return true;
            }

            return false;
        }
    }
}

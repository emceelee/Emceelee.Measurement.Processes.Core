﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emceelee.Summarization.Core;

namespace Emceelee.Measurement.Summarization.Core.Rules
{
    public class InventoryClosingRule<T> : MeasurementSummaryRuleBase<T> where T : IMeasurementGroupable
    {
        public Func<T, DateTime> DateFunction { get; }

        public InventoryClosingRule(Func<T, DateTime> dateFunc)
        {
            DateFunction = dateFunc;
        }

        protected override bool InternalExecute(IEnumerable<T> records, Func<T, double?> func, SummaryContext summaryContext, out double? result)
        {
            result = null;

            T record = records.FirstOrDefault(t => DateFunction(t) == summaryContext.ProductionDateEnd);
            if(record != null)
            {
                result = func(record);
                return true;
            }

            return false;
        }
    }
}

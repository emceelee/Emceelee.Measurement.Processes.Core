using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Emceelee.Summarization.Core;

namespace Emceelee.Summarization.Core.Rules
{
    public class WeightedAverageRule<T> : SummaryRuleBase<T, double?>
    {
        private Func<T, double?> WeightFunction { get; }

        public WeightedAverageRule(Func<T, double?> weightFunc)
        {
            WeightFunction = weightFunc;
        }

        protected override bool InternalExecute(IEnumerable<T> records, Func<T, double?> func, ISummaryContext context, out double? result)
        {
            result = null;

            if (records.Any(t => func(t) != null))
            {
                if(WeightFunction != null)
                {
                    double weightedSum = records.Sum(t => (func(t) ?? 0) * (WeightFunction(t) ?? 0));
                    double denominator = records.Sum(t => WeightFunction(t) ?? 0);
                    if (denominator != 0)
                    {
                        result = weightedSum / denominator;
                        return true;
                    }

                    //Can't calculate a weighted average
                    return false;
                }
                //If no weight function is provided, fall back on a simple average
                else
                {
                    return new SimpleAverageRule<T>().Execute(records, func, context, out result);
                }
            }
            return true;
        }
    }
}

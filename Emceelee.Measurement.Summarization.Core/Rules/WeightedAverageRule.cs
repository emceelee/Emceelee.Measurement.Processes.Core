using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emceelee.Measurement.Summarization.Core.Rules
{
    public class WeightedAverageRule<T> : SummaryRuleBase<T, double?>
    {
        public Func<T, double?> WeightFunction { get; }

        public WeightedAverageRule(Func<T, double?> weightFunc)
        {
            WeightFunction = weightFunc;
        }

        protected override double? InternalExecute(IEnumerable<T> records, Func<T, double?> func)
        {
            double? average = null;

            if (records.Any(t => func(t) != null))
            {
                double weightedSum = records.Sum(t => (func(t) ?? 0) * (WeightFunction(t) ?? 0));
                double denominator = records.Sum(t => WeightFunction(t) ?? 0);
                if(denominator != 0)
                {
                    average = weightedSum / denominator;
                }
                else
                {
                    var backup = new SimpleAverageRule<T>();
                    average = backup.Execute(records, func);
                }

                return average;
            }
            return null;
        }
    }
}

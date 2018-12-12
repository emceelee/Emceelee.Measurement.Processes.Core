using System;
using System.Collections.Generic;

namespace Emceelee.Measurement.Summarization.Core
{
    public class Summarization<TObj>
    {
        private List<TObj> _records = new List<TObj>();
        private List<ISummaryConfig<TObj>> _configs = new List<ISummaryConfig<TObj>>();

        public Summarization(IEnumerable<TObj> records)
        {
            _records.AddRange(records);
        }

        public void Configure<TProperty>(string summaryProperty, string inputProperty, SummaryRuleBase<TObj, TProperty> rule)
        {
            _configs.Add(new SummaryConfig<TObj, TProperty>(summaryProperty, inputProperty, rule));
        }

        public void Configure<TProperty>(string summaryProperty, Func<TObj, TProperty> inputDelegate, SummaryRuleBase<TObj, TProperty> rule)
        {
            _configs.Add(new SummaryConfig<TObj, TProperty>(summaryProperty, inputDelegate, rule));
        }

        public Summary Execute()
        {
            if(_records.Count > 0)
            {
                var summary = new Summary();

                foreach(var config in _configs)
                {
                    config.Execute(summary, _records);
                }

                return summary;
            }

            return null;
        }
    }
}

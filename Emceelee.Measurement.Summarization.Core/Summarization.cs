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

        public void Configure<TProperty>(string summaryProperty, string inputProperty, params SummaryRuleBase<TObj, TProperty>[] rules)
        {
            _configs.Add(new SummaryConfig<TObj, TProperty>(summaryProperty, inputProperty, rules));
        }

        public void Configure<TProperty>(string summaryProperty, Func<TObj, TProperty> inputDelegate, params SummaryRuleBase<TObj, TProperty>[] rules)
        {
            _configs.Add(new SummaryConfig<TObj, TProperty>(summaryProperty, inputDelegate, rules));
        }

        public void Configure<TProperty>(Action<Summary, TProperty> summaryDelegate, string inputProperty, params SummaryRuleBase<TObj, TProperty>[] rules)
        {
            _configs.Add(new SummaryConfig<TObj, TProperty>(summaryDelegate, inputProperty, rules));
        }

        public void Configure<TProperty>(Action<Summary, TProperty> summaryDelegate, Func<TObj, TProperty> inputDelegate, params SummaryRuleBase<TObj, TProperty>[] rules)
        {
            _configs.Add(new SummaryConfig<TObj, TProperty>(summaryDelegate, inputDelegate, rules));
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

        public Func<TObj, TProperty> GetNullDelegate<TProperty>()
        {
            return null;
        }
    }
}

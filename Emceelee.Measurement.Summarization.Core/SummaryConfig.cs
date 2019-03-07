using System;
using System.Collections.Generic;
using System.Text;

using Emceelee.Summarization.Core;
using Emceelee.Summarization.Core.Rules;

namespace Emceelee.Measurement.Summarization.Core
{
    public interface ISummaryConfig<TObj>
    {
        string SummaryProperty { get; }

        void Execute(Summary summary, IEnumerable<TObj> records);
    }

    public class SummaryConfig<TObj, TProperty> : SummaryConfigBase<TObj, TProperty>, ISummaryConfig<TObj>
    {
        public SummaryConfig(string summaryProperty, string inputProperty, params SummaryRuleBase<TObj, TProperty>[] rules)
            : base(inputProperty, rules)
        {
            SummaryProperty = summaryProperty;
            Setter = Utility.CreateSetterDelegate<Summary, TProperty>(summaryProperty);
        }

        public SummaryConfig(string summaryProperty, Func<TObj, TProperty> inputDelegate, params SummaryRuleBase<TObj, TProperty>[] rules)
            : base(inputDelegate, rules)
        {
            SummaryProperty = summaryProperty;
            Setter = Utility.CreateSetterDelegate<Summary, TProperty>(summaryProperty);
        }

        public SummaryConfig(string summaryProperty, Action<Summary, TProperty> summaryDelegate, string inputProperty, params SummaryRuleBase<TObj, TProperty>[] rules)
            : base(inputProperty, rules)
        {
            SummaryProperty = summaryProperty;
            Setter = summaryDelegate;
        }

        public SummaryConfig(string summaryProperty, Action<Summary, TProperty> summaryDelegate, Func<TObj, TProperty> inputDelegate, params SummaryRuleBase<TObj, TProperty>[] rules)
            : base(inputDelegate, rules)
        {
            SummaryProperty = summaryProperty;
            Setter = summaryDelegate;
        }

        public string SummaryProperty { get; }
        public Action<Summary, TProperty> Setter { get; }

        public void Execute(Summary summary, IEnumerable<TObj> records)
        {
            SummaryResult<TProperty> result = base.Execute(records, summary.Context);
            
            Setter?.Invoke(summary, result.Value);
        }
    }
}

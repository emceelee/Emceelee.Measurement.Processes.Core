using System;
using System.Collections.Generic;
using System.Text;

using Emceelee.Summarization.Core;

namespace Emceelee.Measurement.Summarization.Core
{
    public interface ISummaryConfig<TObj>
    {
        void Execute(Summary summary, IEnumerable<TObj> records, SummaryContext sc = null);
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

        public SummaryConfig(Action<Summary, TProperty> summaryDelegate, string inputProperty, params SummaryRuleBase<TObj, TProperty>[] rules)
            : base(inputProperty, rules)
        {
            SummaryProperty = string.Empty;
            Setter = summaryDelegate;
        }

        public SummaryConfig(Action<Summary, TProperty> summaryDelegate, Func<TObj, TProperty> inputDelegate, params SummaryRuleBase<TObj, TProperty>[] rules)
            : base(inputDelegate, rules)
        {
            SummaryProperty = string.Empty;
            Setter = summaryDelegate;
        }

        public string SummaryProperty { get; }
        public Action<Summary, TProperty> Setter { get; }

        public void Execute(Summary summary, IEnumerable<TObj> records, SummaryContext sc)
        {
            SummaryResult<TProperty> result = base.Execute(records, sc);
            
            Setter?.Invoke(summary, result.Value);
        }
    }
}

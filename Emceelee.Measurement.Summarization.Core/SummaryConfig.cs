using System;
using System.Collections.Generic;
using System.Text;

namespace Emceelee.Measurement.Summarization.Core
{
    public interface ISummaryConfig<TObj>
    {
        void Execute(Summary summary, IEnumerable<TObj> records);
    }

    public class SummaryConfig<TObj, TProperty> : ISummaryConfig<TObj>
    {
        public SummaryConfig(string summaryProperty, string inputProperty, SummaryRuleBase<TObj, TProperty> rule)
        {
            SummaryProperty = summaryProperty;
            InputProperty = inputProperty;
            Rule = rule;

            Getter = Utility.CreateGetterDelegate<TObj, TProperty>(inputProperty);
            Setter = Utility.CreateSetterDelegate<Summary, TProperty>(summaryProperty);
        }

        public SummaryConfig(string summaryProperty, Func<TObj, TProperty> inputDelegate, SummaryRuleBase<TObj, TProperty> rule)
        {
            SummaryProperty = summaryProperty;
            InputProperty = String.Empty;
            Rule = rule;

            Getter = inputDelegate;
            Setter = Utility.CreateSetterDelegate<Summary, TProperty>(summaryProperty);
        }
        
        public string SummaryProperty { get; }
        public string InputProperty { get; }
        public SummaryRuleBase<TObj, TProperty> Rule { get; }
        public Func<TObj, TProperty> Getter { get; }
        public Action<Summary, TProperty> Setter { get; }

        public void Execute(Summary summary, IEnumerable<TObj> records)
        {
            TProperty result = Rule.Execute(records, Getter);
            Setter(summary, result);
        }
    }
}

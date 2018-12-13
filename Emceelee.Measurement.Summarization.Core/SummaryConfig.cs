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
        public SummaryConfig(string summaryProperty, string inputProperty, params SummaryRuleBase<TObj, TProperty>[] rules)
        {
            SummaryProperty = summaryProperty;
            InputProperty = inputProperty;
            Rules = rules;

            Getter = Utility.CreateGetterDelegate<TObj, TProperty>(inputProperty);
            Setter = Utility.CreateSetterDelegate<Summary, TProperty>(summaryProperty);
        }

        public SummaryConfig(string summaryProperty, Func<TObj, TProperty> inputDelegate, params SummaryRuleBase<TObj, TProperty>[] rules)
        {
            SummaryProperty = summaryProperty;
            InputProperty = String.Empty;
            Rules = rules;

            Getter = inputDelegate;
            Setter = Utility.CreateSetterDelegate<Summary, TProperty>(summaryProperty);
        }

        public SummaryConfig(Action<Summary, TProperty> summaryDelegate, string inputProperty, params SummaryRuleBase<TObj, TProperty>[] rules)
        {
            SummaryProperty = string.Empty;
            InputProperty = inputProperty;
            Rules = rules;

            Getter = Utility.CreateGetterDelegate<TObj, TProperty>(inputProperty);
            Setter = summaryDelegate;
        }

        public SummaryConfig(Action<Summary, TProperty> summaryDelegate, Func<TObj, TProperty> inputDelegate, params SummaryRuleBase<TObj, TProperty>[] rules)
        {
            SummaryProperty = string.Empty;
            InputProperty = string.Empty;
            Rules = rules;

            Getter = inputDelegate;
            Setter = summaryDelegate;
        }

        public string SummaryProperty { get; }
        public string InputProperty { get; }
        public SummaryRuleBase<TObj, TProperty>[] Rules { get; }
        public Func<TObj, TProperty> Getter { get; }
        public Action<Summary, TProperty> Setter { get; }

        public void Execute(Summary summary, IEnumerable<TObj> records)
        {
            TProperty result = default(TProperty);
            
            foreach (var rule in Rules)
            {
                if (rule.Execute(records, Getter, out result))
                    break;
            }

            Setter?.Invoke(summary, result);
        }
    }
}

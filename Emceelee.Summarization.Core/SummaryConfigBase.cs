using System;
using System.Collections.Generic;
using System.Text;

using Emceelee.Summarization.Core.Rules;

namespace Emceelee.Summarization.Core
{
    public class SummaryConfigBase<TObj, TProperty>
    {
        public SummaryConfigBase(string inputProperty, params SummaryRuleBase<TObj, TProperty>[] rules)
        {
            InputProperty = inputProperty;
            Rules = rules;

            Getter = Utility.CreateGetterDelegate<TObj, TProperty>(inputProperty);
        }

        public SummaryConfigBase(Func<TObj, TProperty> inputDelegate, params SummaryRuleBase<TObj, TProperty>[] rules)
        {
            InputProperty = String.Empty;
            Rules = rules;

            Getter = inputDelegate;
        }        
        
        public string InputProperty { get; }
        public SummaryRuleBase<TObj, TProperty>[] Rules { get; }
        public Func<TObj, TProperty> Getter { get; }

        public virtual SummaryResult<TProperty> Execute(IEnumerable<TObj> records, ISummaryContext context)
        {
            TProperty result = default(TProperty);
            bool success = false;

            //TODO: Handle exceptions by rolling into an AggregateException here
            //execute rules until success
            foreach (var rule in Rules)
            {
                TProperty temp;
                success |= rule.Execute(records, Getter, context, out temp);

                if(success)
                {
                    result = temp;
                    break;
                }
            }

            return new SummaryResult<TProperty>(success, result);
        }
    }
}

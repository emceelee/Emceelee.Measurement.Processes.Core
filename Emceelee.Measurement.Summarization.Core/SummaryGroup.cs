using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emceelee.Measurement.Summarization.Core
{
    public class SummaryGroup<TObj>
    {
        public SummaryContext Context { get; }
        public IEnumerable<TObj> Records { get; }

        public SummaryGroup(IEnumerable<TObj> records, SummaryContext context)
        {
            Records = records ?? new List<TObj>();
            Context = context ?? throw new ArgumentNullException("SummaryContext must not be null. ");
        }
    }
}

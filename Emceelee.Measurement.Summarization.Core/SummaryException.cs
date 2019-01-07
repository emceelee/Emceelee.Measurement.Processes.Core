using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emceelee.Measurement.Summarization.Core
{
    public class SummaryException : Exception
    {
        public SummaryException(string property, Exception exc = null) :
            base($"Unhandled exception encountered when generating summary for property {property}.", exc)
        {
        }
    }
}

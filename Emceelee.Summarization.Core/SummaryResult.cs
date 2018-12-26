using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emceelee.Summarization.Core
{
    public interface ISummaryResult
    {
    }

    public class SummaryResult<T> : ISummaryResult
    {
        public bool Success { get; }
        public T Value { get; }

        public SummaryResult(bool success, T value)
        {
            Success = success;
            Value = value;
        }
    }
}

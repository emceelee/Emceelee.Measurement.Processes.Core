using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emceelee.Summarization.Core.Rules;

namespace Emceelee.Measurement.Summarization.Test.Rules
{
    [TestClass]
    public class SummaryRulelBaseTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Execute_NullRecords_ArgumentNullException()
        {
            var rule = new SimpleAverageRule<Quantity>();

            double? result = null;
            var success = rule.Execute(null, (q) => q.HeatingValue, null, out result);
        }
    }
}

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
    public class SimpleAverageRuleTest
    {
        [TestMethod]
        public void Execute_Success()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { HeatingValue = 1000 });
            records.Add(new Quantity() { HeatingValue = 2000 });

            var rule = new SimpleAverageRule<Quantity>();

            double? result = null;
            var success = rule.Execute(records, (q) => q.HeatingValue, out result);

            Assert.IsTrue(success);
            Assert.AreEqual(1500, result);
        }

        [TestMethod]
        public void Execute_Success_NullRecords()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { HeatingValue = 1000 });
            records.Add(new Quantity() { HeatingValue = 2000 });
            records.Add(new Quantity());

            var rule = new SimpleAverageRule<Quantity>();

            double? result = null;
            var success = rule.Execute(records, (q) => q.HeatingValue, out result);

            Assert.IsTrue(success);
            Assert.AreEqual(1500, result);
        }
    }
}

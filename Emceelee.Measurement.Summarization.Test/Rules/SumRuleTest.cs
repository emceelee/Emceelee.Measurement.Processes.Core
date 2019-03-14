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
    public class SumRuleTest
    {

        [TestMethod]
        public void Execute_Success()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { FlowTime = 60 });
            records.Add(new Quantity() { FlowTime = 40 });
            records.Add(new Quantity() { FlowTime = 50 });

            var rule = new SumRule<Quantity>();

            double? result = null;
            var success = rule.Execute(records, (q) => q.FlowTime, out result);

            Assert.IsTrue(success);
            Assert.AreEqual(150, result);
        }

        [TestMethod]
        public void Execute_Success_NullValue()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { FlowTime = 60 });
            records.Add(new Quantity() { FlowTime = 40 });
            records.Add(new Quantity());

            var rule = new SumRule<Quantity>();

            double? result = null;
            var success = rule.Execute(records, (q) => q.FlowTime, out result);

            Assert.IsTrue(success);
            Assert.AreEqual(100, result);
        }

        [TestMethod]
        public void Execute_Success_NullResult()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity());
            records.Add(new Quantity());
            records.Add(new Quantity());

            var rule = new SumRule<Quantity>();

            double? result = null;
            var success = rule.Execute(records, (q) => q.FlowTime, out result);

            Assert.IsTrue(success);
            Assert.IsNull(result);
        }
    }
}

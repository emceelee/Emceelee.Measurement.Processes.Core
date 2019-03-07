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
    public class RoundRuleTest
    {
        [TestMethod]
        public void Execute_Success()
        {
            var records = new List<Quantity>();

            var rule = new RoundRule<Quantity>(new ConstantRule<Quantity, double?>(1.555), 2, MidpointRounding.AwayFromZero);

            double? result = null;
            var success = rule.Execute(records, (q) => 0, out result);

            Assert.IsTrue(success);
            Assert.AreEqual(1.56, result);
        }

        [TestMethod]
        public void Execute_Success_NullResult()
        {
            var records = new List<Quantity>();

            var rule = new RoundRule<Quantity>(new ConstantRule<Quantity, double?>(null), 2, MidpointRounding.AwayFromZero);

            double? result = null;
            var success = rule.Execute(records, (q) => 0, out result);

            Assert.IsTrue(success);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Execute_Fail()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { GasVolume = 3, HeatingValue = 1000 });
            records.Add(new Quantity() { GasVolume = 1, HeatingValue = 2000 });

            var rule = new RoundRule<Quantity>(new WeightedAverageRule<Quantity>(q => q.FlowTime), 2, MidpointRounding.AwayFromZero);

            double? result = null;
            var success = rule.Execute(records, (q) => q.HeatingValue, out result);

            Assert.IsFalse(success);
            Assert.IsNull(result);
        }
    }
}

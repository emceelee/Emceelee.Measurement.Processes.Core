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
    public class WeightedAverageRuleTest
    {
        private static List<Quantity> Records;

        [ClassInitialize]
        public static void Initialize(TestContext tc)
        {
            Records = new List<Quantity>();
            Records.Add(new Quantity() { FlowTime = 1, GasVolume = 3, HeatingValue = 1000 });
            Records.Add(new Quantity() { FlowTime = 3, GasVolume = 1, HeatingValue = 2000 });
        }

        [TestMethod]
        public void Execute_Success_Example_FlowTime()
        {
            //weight HV by FlowTime
            var rule = new WeightedAverageRule<Quantity>(q => q.FlowTime);

            double? result = null;
            var success = rule.Execute(Records, (q) => q.HeatingValue, out result);

            Assert.IsTrue(success);
            Assert.AreEqual(1750, result);
        }

        [TestMethod]
        public void Execute_Success_Example_GasVolume()
        {
            //weight HV by GasVolume
            var rule = new WeightedAverageRule<Quantity>(q => q.GasVolume);

            double? result = null;
            var success = rule.Execute(Records, (q) => q.HeatingValue, out result);

            Assert.IsTrue(success);
            Assert.AreEqual(1250, result);
        }

        [TestMethod]
        public void Execute_Success_NullValue()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { FlowTime = 1, GasVolume = 3, HeatingValue = 1000 });
            records.Add(new Quantity() { FlowTime = 3, GasVolume = 1, HeatingValue = 2000 });
            records.Add(new Quantity());

            //weight HV by FlowTime
            var rule = new WeightedAverageRule<Quantity>(q => q.FlowTime);

            double? result = null;
            var success = rule.Execute(records, (q) => q.HeatingValue, out result);

            Assert.IsTrue(success);
            Assert.AreEqual(1750, result);
        }

        [TestMethod]
        public void Execute_Success_NullResult()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { FlowTime = 1, GasVolume = 3 });
            records.Add(new Quantity() { FlowTime = 3, GasVolume = 1 });
            records.Add(new Quantity());

            //weight HV by FlowTime
            var rule = new WeightedAverageRule<Quantity>(q => q.FlowTime);

            double? result = null;
            var success = rule.Execute(records, (q) => q.HeatingValue, out result);

            Assert.IsTrue(success);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Execute_Success_NullWeightFunction()
        {
            //weight HV by FlowTime
            var rule = new WeightedAverageRule<Quantity>(null);

            double? result = null;
            var success = rule.Execute(Records, (q) => q.HeatingValue, out result);

            Assert.IsTrue(success);
            Assert.AreEqual(1500, result);
        }

        public void Execute_Fail()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { GasVolume = 3, HeatingValue = 1000 });
            records.Add(new Quantity() { GasVolume = 1, HeatingValue = 2000 });

            //weight HV by FlowTime
            var rule = new WeightedAverageRule<Quantity>(q => q.FlowTime);

            double? result = null;
            var success = rule.Execute(Records, (q) => q.HeatingValue, out result);

            Assert.IsFalse(success);
            Assert.IsNull(result);
        }
    }
}

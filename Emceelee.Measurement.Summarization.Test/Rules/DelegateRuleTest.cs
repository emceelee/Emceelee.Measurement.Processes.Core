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
    public class DelegateRuleTest
    {
        private static List<Quantity> Records;

        [ClassInitialize]
        public static void Initialize(TestContext tc)
        {
            Records = new List<Quantity>();
            Records.Add(new Quantity() { GasVolume = 150, ProductionDateStart = new DateTime(2018, 1, 1) });
            Records.Add(new Quantity() { GasVolume = 200, ProductionDateStart = new DateTime(2018, 1, 2) });
            Records.Add(new Quantity() { GasVolume = 250, ProductionDateStart = new DateTime(2018, 1, 3) });
        }

        [TestMethod]
        public void Execute_Success_Double()
        {
            var rule = new DelegateRule<Quantity, double?>((r, context) => 10.0);

            double? result = null;
            var success = rule.Execute(Records, rule.NullDelegate, out result);

            Assert.IsTrue(success);
            Assert.AreEqual(10.0, result);
        }

        [TestMethod]
        public void Execute_Success_String()
        {
            var rule = new DelegateRule<Quantity, string>((r, context) => "String");

            string result = null;
            var success = rule.Execute(Records, rule.NullDelegate, out result);

            Assert.IsTrue(success);
            Assert.AreEqual("String", result);
        }

        [TestMethod]
        public void Execute_Success_Int()
        {
            var rule = new DelegateRule<Quantity, int>((r, context) => r.Count());

            int result = 0;
            var success = rule.Execute(Records, rule.NullDelegate, out result);

            Assert.IsTrue(success);
            Assert.AreEqual(3, result);
        }


        [TestMethod]
        public void Execute_Success__Example_IndexOn()
        {
            var rule = new DelegateRule<Quantity, double?>((r, context) => r.OrderBy(q => q.ProductionDateStart).FirstOrDefault()?.GasVolume);

            double? result = null;
            var success = rule.Execute(Records, rule.NullDelegate, out result);

            Assert.IsTrue(success);
            Assert.AreEqual(150, result);
        }
    }
}

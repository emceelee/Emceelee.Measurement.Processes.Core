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
    public class CeilingRuleTest
    {
        [TestMethod]
        public void Execute_Success()
        {
            var records = new List<Quantity>();

            var rule = new CeilingRule<Quantity>(new ConstantRule<Quantity, double?>(1.555));

            double? result = null;
            var success = rule.Execute(records, (q) => 0, out result);

            Assert.IsTrue(success);
            Assert.AreEqual(2, result);
        }
    }
}

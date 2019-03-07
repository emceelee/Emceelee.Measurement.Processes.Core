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
    public class ConstantRuleTest
    {
        [TestMethod]
        public void Execute_Success_Int()
        {
            var records = new List<Quantity>();

            var rule = new ConstantRule<Quantity, int>(1);

            int result = 0;
            var success = rule.Execute(records, (q) => 0, out result);

            Assert.IsTrue(success);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Execute_Success_Double()
        {
            var records = new List<Quantity>();

            var rule = new ConstantRule<Quantity, double>(0.5);

            double result = 0;
            var success = rule.Execute(records, (q) => 0, out result);

            Assert.IsTrue(success);
            Assert.AreEqual(0.5, result);
        }

        [TestMethod]
        public void Execute_Success_String()
        {
            var records = new List<Quantity>();

            var rule = new ConstantRule<Quantity, string>("string");

            string result;
            var success = rule.Execute(records, (q) => null, out result);

            Assert.IsTrue(success);
            Assert.AreEqual("string", result);
        }

        [TestMethod]
        public void Execute_Success_Object()
        {
            var records = new List<Quantity>();

            var obj = new Object();
            var rule = new ConstantRule<Quantity, Object>(obj);

            Object result;
            var success = rule.Execute(records, (q) => null, out result);

            Assert.IsTrue(success);
            Assert.AreSame(obj, result);
        }
    }
}

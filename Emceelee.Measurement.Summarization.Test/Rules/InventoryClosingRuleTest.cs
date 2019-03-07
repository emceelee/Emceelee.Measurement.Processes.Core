using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emceelee.Measurement.Summarization.Core;
using Emceelee.Measurement.Summarization.Core.Rules;

namespace Emceelee.Measurement.Summarization.Test.Rules
{
    [TestClass]
    public class InventoryClosingRuleTest
    {
        [TestMethod]
        public void Execute_Success()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { GasVolume = 150, ProductionDateEnd = new DateTime(2018, 2, 1, 6, 48, 0, DateTimeKind.Utc) });
            records.Add(new Quantity() { GasVolume = 200, ProductionDateEnd = new DateTime(2018, 2, 1, 6, 54, 0, DateTimeKind.Utc) });
            records.Add(new Quantity() { GasVolume = 250, ProductionDateEnd = new DateTime(2018, 2, 1, 7, 0, 0, DateTimeKind.Utc) });

            var context = new SummaryContext(7,
                    new SummaryInfo("Meter",
                        new DateTime(2018, 1, 1)));

            var rule = new InventoryClosingRule<Quantity>(q => q.ProductionDateEnd);

            double? result = null;
            var success = rule.Execute(records, (q) => q.GasVolume, context, out result);

            Assert.IsTrue(success);
            Assert.AreEqual(250, result);
        }

        [TestMethod]
        public void Execute_Fail()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { GasVolume = 150, ProductionDateEnd = new DateTime(2018, 1, 2, 6, 48, 0, DateTimeKind.Utc) });
            records.Add(new Quantity() { GasVolume = 200, ProductionDateEnd = new DateTime(2018, 1, 2, 6, 54, 0, DateTimeKind.Utc) });
            //records.Add(new Quantity() { GasVolume = 250, ProductionDateEnd = new DateTime(2018, 1, 2, 7, 0, 0, DateTimeKind.Utc) });

            var context = new SummaryContext(7,
                    new SummaryInfo("Meter",
                        new DateTime(2018, 1, 1)));

            var rule = new InventoryClosingRule<Quantity>(q => q.ProductionDateEnd);

            double? result = null;
            var success = rule.Execute(records, (q) => q.GasVolume, context, out result);

            Assert.IsFalse(success);
            Assert.IsNull(result);
        }
    }
}

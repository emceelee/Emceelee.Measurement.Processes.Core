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
    public class LastOrDefaultRuleTest
    {
        [TestMethod]
        public void Execute_Success()
        {
            var productionDates = new DateTime[]
            {
                new DateTime(2018, 1, 1, 7, 6, 0, DateTimeKind.Utc),
                new DateTime(2018, 1, 1, 7, 12, 0, DateTimeKind.Utc),
                new DateTime(2018, 1, 1, 7, 18, 0, DateTimeKind.Utc),
                new DateTime(2018, 1, 1, 7, 0, 0, DateTimeKind.Utc)
            };

            var records = new List<Quantity>();
            records.Add(new Quantity() { ProductionDateEnd = productionDates[0] });
            records.Add(new Quantity() { ProductionDateEnd = productionDates[1] });
            records.Add(new Quantity() { ProductionDateEnd = productionDates[2] });
            records.Add(new Quantity() { ProductionDateEnd = productionDates[3] });

            var rule = new LastOrDefaultRule<Quantity, DateTime>(q => q.ProductionDateEnd);

            DateTime result = default(DateTime);
            var success = rule.Execute(records, (q) => q.ProductionDateEnd, out result);

            Assert.IsTrue(success);
            Assert.AreEqual(productionDates[2], result);
        }

        [TestMethod]
        public void Execute_Success_NoRecords()
        {
            var records = new List<Quantity>();

            var rule = new LastOrDefaultRule<Quantity, DateTime>(q => q.ProductionDateEnd);

            DateTime result = default(DateTime);
            var success = rule.Execute(records, (q) => q.ProductionDateEnd, out result);

            Assert.IsTrue(success);
            Assert.AreEqual(default(DateTime), result);
        }
    }
}

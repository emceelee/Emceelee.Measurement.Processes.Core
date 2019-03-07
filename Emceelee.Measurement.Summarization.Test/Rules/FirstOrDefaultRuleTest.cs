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
    public class FirstOrDefaultRuleTest
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
            records.Add(new Quantity() { ProductionDateStart = productionDates[0] });
            records.Add(new Quantity() { ProductionDateStart = productionDates[1] });
            records.Add(new Quantity() { ProductionDateStart = productionDates[2] });
            records.Add(new Quantity() { ProductionDateStart = productionDates[3] });

            var rule = new FirstOrDefaultRule<Quantity, DateTime>(q => q.ProductionDateStart);

            DateTime result = default(DateTime);
            var success = rule.Execute(records, (q) => q.ProductionDateStart, out result);

            Assert.IsTrue(success);
            Assert.AreEqual(productionDates[3], result);
        }

        [TestMethod]
        public void Execute_Success_NoRecords()
        {
            var records = new List<Quantity>();

            var rule = new FirstOrDefaultRule<Quantity, DateTime>(q => q.ProductionDateStart);

            DateTime result = default(DateTime);
            var success = rule.Execute(records, (q) => q.ProductionDateStart, out result);

            Assert.IsTrue(success);
            Assert.AreEqual(default(DateTime), result);
        }
    }
}

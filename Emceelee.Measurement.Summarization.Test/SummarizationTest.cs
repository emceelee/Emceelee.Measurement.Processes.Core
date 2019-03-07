using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Emceelee.Measurement.Summarization.Core;
using Emceelee.Summarization.Core.Rules;
using Emceelee.Measurement.Summarization.Core.Rules;

namespace Emceelee.Measurement.Summarization.Test
{
    [TestClass]
    public class SummarizationTest
    {
        [ClassInitialize]
        public static void Initialize(TestContext tc)
        {
            TestData.Initialize();
        }

        #region Configure
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Configure_BadProperty_Getter()
        {
            var records = new List<Quantity>();
            var group = CreateDummySummaryGroup(records);

            var summarization = new Summarization<Quantity>();
            summarization.Configure(nameof(Summary.FlowTime), string.Empty, new SumRule<Quantity>());
            var result = summarization.Execute(group);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Configure_BadProperty_Setter()
        {
            var records = new List<Quantity>();
            var group = CreateDummySummaryGroup(records);

            var summarization = new Summarization<Quantity>();
            summarization.Configure(string.Empty, nameof(Quantity.FlowTime), new SumRule<Quantity>());
            var result = summarization.Execute(group);
        }

        [TestMethod]
        public void Configure_Properties()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { FlowTime = 60 });
            records.Add(new Quantity() { FlowTime = 40 });
            records.Add(new Quantity() { FlowTime = 50 });

            var group = CreateDummySummaryGroup(records);

            var summarization = new Summarization<Quantity>();
            summarization.Configure(nameof(Summary.FlowTime), nameof(Quantity.FlowTime), new SumRule<Quantity>());
            var result = summarization.Execute(group);
            
            Assert.AreEqual(150, result.FlowTime);
        }

        [TestMethod]
        public void Configure_Setter_Delegate()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { FlowTime = 60});
            records.Add(new Quantity() { FlowTime = 40 });
            records.Add(new Quantity() { FlowTime = 50 });

            var group = CreateDummySummaryGroup(records);

            var summarization = new Summarization<Quantity>();
            summarization.Configure(nameof(Summary.FlowTime), (s, r) => s.FlowTime = r, 
                nameof(Quantity.FlowTime), 
                new SumRule<Quantity>());
            var result = summarization.Execute(group);

            Assert.AreEqual(150, result.FlowTime);
        }

        [TestMethod]
        public void Configure_Getter_Delegate()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { FlowTime = 60 });
            records.Add(new Quantity() { FlowTime = 40 });
            records.Add(new Quantity() { FlowTime = 50 });

            var group = CreateDummySummaryGroup(records);

            var summarization = new Summarization<Quantity>();
            summarization.Configure(nameof(Summary.FlowTime), 
                q => q.FlowTime, 
                new SumRule<Quantity>());
            var result = summarization.Execute(group);

            Assert.AreEqual(150, result.FlowTime);
        }

        [TestMethod]
        public void Configure_Setter_Getter_Delegate()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { FlowTime = 60 });
            records.Add(new Quantity() { FlowTime = 40 });
            records.Add(new Quantity() { FlowTime = 50 });

            var group = CreateDummySummaryGroup(records);

            var summarization = new Summarization<Quantity>();
            summarization.Configure(nameof(Summary.FlowTime), (s, r) => s.FlowTime = r, 
                q => q.FlowTime, 
                new SumRule<Quantity>());
            var result = summarization.Execute(group);

            Assert.AreEqual(150, result.FlowTime);
        }
        #endregion

        #region Execute

        [TestMethod]
        public void Execufte_NoRecords()
        {
            var records = new List<Quantity>();
            var group = CreateDummySummaryGroup(records);

            var summarization = new Summarization<Quantity>();
            var result = summarization.Execute(group);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Execute_RuleFallback()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { GasVolume = 3, HeatingValue = 1000 });
            records.Add(new Quantity() { GasVolume = 1, HeatingValue = 2000 });
            
            var group = CreateDummySummaryGroup(records);

            var summarization = new Summarization<Quantity>();
            //weight HV by FlowTime
            summarization.Configure(nameof(Summary.HeatingValue), nameof(Quantity.HeatingValue), new WeightedAverageRule<Quantity>(q => q.FlowTime), 
                                                                    new SimpleAverageRule<Quantity>());
            var result = summarization.Execute(group);

            Assert.AreEqual(1500, result.HeatingValue);
        }

        [TestMethod]
        public void Execute_AggregateException()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { FlowTime = 1, GasVolume = 3, HeatingValue = 1000 });
            records.Add(new Quantity() { FlowTime = 3, GasVolume = 1, HeatingValue = 2000 });

            var group = CreateDummySummaryGroup(records);

            var summarization = new Summarization<Quantity>();
            //weight HV by FlowTime
            summarization.Configure(nameof(Summary.FlowTime), nameof(Quantity.FlowTime), new ExceptionRule<Quantity, double?>());
            summarization.Configure(nameof(Summary.Volume), nameof(Quantity.GasVolume), new SumRule<Quantity>());
            summarization.Configure(nameof(Summary.HeatingValue), nameof(Quantity.HeatingValue), new ExceptionRule<Quantity, double?>());
            var result = summarization.Execute(group);

            Assert.IsTrue(result.HasExceptions);
            Assert.IsNotNull(result.Exceptions);
            Assert.AreEqual(2, result.Exceptions.InnerExceptions.Count());
        }
        #endregion

        #region Performance Tests

        [TestMethod]
        [Timeout(5000)]
        public void PerformanceTest_Hourly()
        {
            var results = TestData.Data.CreateHourlySummaryGroups(0);
            Assert.AreEqual(74400, results.Count());

            Parallel.ForEach(results, group =>
            {
                var result = TestData.StandardQtySummarization.Execute(group);
            });
        }

        [TestMethod]
        //[Timeout(2000)]
        public void PerformanceTest_Daily()
        {
            var results = TestData.Data.CreateDailySummaryGroups(0);
            Assert.AreEqual(3100, results.Count());

            Parallel.ForEach(results, group =>
            {
                var result = TestData.StandardQtySummarization.Execute(group);
            });
        }

        [TestMethod]
        [Timeout(2000)]
        public void PerformanceTest_Monthly()
        {
            var results = TestData.Data.CreateMonthlySummaryGroups(0);
            Assert.AreEqual(100, results.Count());

            Parallel.ForEach(results, group =>
            {
                var result = TestData.StandardQtySummarization.Execute(group);
            });
        }

        [TestMethod]
        [Timeout(2000)]
        public void PerformanceTest_Meter()
        {
            var results = TestData.Data.CreateEntitySummaryGroups(0);
            Assert.AreEqual(100, results.Count());

            Parallel.ForEach(results, group =>
            {
                var result = TestData.StandardQtySummarization.Execute(group);
            });
        }

        [TestMethod]
        [Timeout(2000)]
        public void PerformanceTest_Aggregate()
        {
            var results = TestData.Data.CreateAggregateSummaryGroups(0);
            Assert.AreEqual(1, results.Count());

            Parallel.ForEach(results, group =>
            {
                var result = TestData.StandardQtySummarization.Execute(group);
            });
        }


        [TestMethod]
        [Timeout(2000)]
        public void PerformanceTest_Tickets()
        {
            var results = TestData.DataTickets.CreateEntitySummaryGroups(0);
            Assert.AreEqual(10, results.Count());

            Parallel.ForEach(results, group =>
            {
                var result = TestData.StandardQtySummarization.Execute(group);
            });
        }

        #endregion

        #region Helper Methods
        private SummaryGroup<TObj> CreateDummySummaryGroup<TObj>(IEnumerable<TObj> records)
        {
            return new SummaryGroup<TObj>(records,
                new SummaryContext(7,
                    new SummaryInfo("Meter",
                        new DateTime(2018, 1, 1)
                )));
        }
        #endregion
    }

}

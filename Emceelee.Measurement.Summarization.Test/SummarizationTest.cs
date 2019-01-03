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
        public static List<Quantity> data;
        public static List<Quantity> data_tickets;

        [ClassInitialize]
        public static void Initialize(TestContext tc)
        {
            //1 month of 6-minute data
            data = GenerateTestData();
            data_tickets = GenerateTestData();
            foreach(var qty in data_tickets)
            {
                qty.GenerateSummaryInfo(qty.TicketId);
            }
        }

        [TestMethod]
        public void Summarization_NoRecords()
        {
            var records = new List<Quantity>();
            var group = new SummaryGroup<Quantity>(records, 
                new SummaryContext(7, 
                    new SummaryInfo("Meter", 
                        new DateTime(2018, 1, 1)
                )));

            var summarization = new Summarization<Quantity>();
            var result = summarization.Execute(group);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Summarization_SumRule()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { FlowTime = 60, GasVolume = 20 });
            records.Add(new Quantity() { FlowTime = 40, GasVolume = 30 });
            records.Add(new Quantity() { FlowTime = 50, GasVolume = 40 });

            var group = new SummaryGroup<Quantity>(records,
                new SummaryContext(7,
                    new SummaryInfo("Meter",
                        new DateTime(2018, 1, 1)
                )));

            var summarization = new Summarization<Quantity>();
            summarization.Configure("FlowTime", "FlowTime", new SumRule<Quantity>());
            summarization.Configure("Volume", "GasVolume", new SumRule<Quantity>());
            var result = summarization.Execute(group);

            Assert.AreEqual(150, result.FlowTime);
            Assert.AreEqual(90, result.Volume);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Summarization_BadProperty_Getter()
        {
            var records = new List<Quantity>();

            var group = new SummaryGroup<Quantity>(records,
                new SummaryContext(7,
                    new SummaryInfo("Meter",
                        new DateTime(2018, 1, 1)
                )));

            var summarization = new Summarization<Quantity>();
            summarization.Configure("FlowTime", "", new SumRule<Quantity>());
            var result = summarization.Execute(group);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Summarization_BadProperty_Setter()
        {
            var records = new List<Quantity>();

            var group = new SummaryGroup<Quantity>(records,
                new SummaryContext(7,
                    new SummaryInfo("Meter",
                        new DateTime(2018, 1, 1)
                )));

            var summarization = new Summarization<Quantity>();
            summarization.Configure("", "FlowTime", new SumRule<Quantity>());
            var result = summarization.Execute(group);
        }

        [TestMethod]
        public void Summarization_Delegates1()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { FlowTime = 60, GasVolume = 20 });
            records.Add(new Quantity() { FlowTime = 40, GasVolume = 30 });
            records.Add(new Quantity() { FlowTime = 50, GasVolume = 40 });

            var group = new SummaryGroup<Quantity>(records,
                new SummaryContext(7,
                    new SummaryInfo("Meter",
                        new DateTime(2018, 1, 1)
                )));

            var summarization = new Summarization<Quantity>();
            summarization.Configure("FlowTime", q => q.FlowTime, new SumRule<Quantity>());
            summarization.Configure((s, r) => s.Volume = r, "GasVolume", new SumRule<Quantity>());
            var result = summarization.Execute(group);

            Assert.AreEqual(150, result.FlowTime);
            Assert.AreEqual(90, result.Volume);
        }

        [TestMethod]
        public void Summarization_SumRule_Delegates2()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { FlowTime = 60, GasVolume = 20 });
            records.Add(new Quantity() { FlowTime = 40, GasVolume = 30 });
            records.Add(new Quantity() { FlowTime = 50, GasVolume = 40 });

            var group = new SummaryGroup<Quantity>(records,
                new SummaryContext(7,
                    new SummaryInfo("Meter",
                        new DateTime(2018, 1, 1)
                )));

            var summarization = new Summarization<Quantity>();
            summarization.Configure((s, r) => s.FlowTime = r, q => q.FlowTime, new SumRule<Quantity>());
            summarization.Configure((s, r) => s.Volume = r, q => q.GasVolume, new SumRule<Quantity>());
            var result = summarization.Execute(group);

            Assert.AreEqual(150, result.FlowTime);
            Assert.AreEqual(90, result.Volume);
        }

        [TestMethod]
        public void Summarization_SumRule_NullRecords()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { FlowTime = 60, GasVolume = 20 });
            records.Add(new Quantity() { FlowTime = 40, GasVolume = 30 });
            records.Add(new Quantity());

            var group = new SummaryGroup<Quantity>(records,
                new SummaryContext(7,
                    new SummaryInfo("Meter",
                        new DateTime(2018, 1, 1)
                )));

            var summarization = new Summarization<Quantity>();
            summarization.Configure("FlowTime", "FlowTime", new SumRule<Quantity>());
            summarization.Configure("Volume", "GasVolume", new SumRule<Quantity>());
            var result = summarization.Execute(group);

            Assert.AreEqual(100, result.FlowTime);
            Assert.AreEqual(50, result.Volume);
        }

        [TestMethod]
        public void Summarization_WeightedAverageRule_FlowTime()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { FlowTime = 1, GasVolume = 3, HeatingValue = 1000 });
            records.Add(new Quantity() { FlowTime = 3, GasVolume = 1, HeatingValue = 2000 });

            var group = new SummaryGroup<Quantity>(records,
                new SummaryContext(7,
                    new SummaryInfo("Meter",
                        new DateTime(2018, 1, 1)
                )));

            var summarization = new Summarization<Quantity>();
            //weight HV by FlowTime
            summarization.Configure("HeatingValue", "HeatingValue", new WeightedAverageRule<Quantity>(q => q.FlowTime),
                                                                    new SimpleAverageRule<Quantity>());
            var result = summarization.Execute(group);

            Assert.AreEqual(1750, result.HeatingValue);
        }

        [TestMethod]
        public void Summarization_WeightedAverageRule_GasVolume()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { FlowTime = 1, GasVolume = 3, HeatingValue = 1000 });
            records.Add(new Quantity() { FlowTime = 3, GasVolume = 1, HeatingValue = 2000 });

            var group = new SummaryGroup<Quantity>(records,
                new SummaryContext(7,
                    new SummaryInfo("Meter",
                        new DateTime(2018, 1, 1)
                )));

            var summarization = new Summarization<Quantity>();
            //weight HV by FlowTime
            summarization.Configure("HeatingValue", "HeatingValue", new WeightedAverageRule<Quantity>(q => q.GasVolume),
                                                                    new SimpleAverageRule<Quantity>());
            var result = summarization.Execute(group);

            Assert.AreEqual(1250, result.HeatingValue);
        }

        [TestMethod]
        public void Summarization_WeightedAverageRule_NullRecords()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { FlowTime = 1, GasVolume = 3, HeatingValue = 1000 });
            records.Add(new Quantity() { FlowTime = 3, GasVolume = 1, HeatingValue = 2000 });
            records.Add(new Quantity());

            var group = new SummaryGroup<Quantity>(records,
                new SummaryContext(7,
                    new SummaryInfo("Meter",
                        new DateTime(2018, 1, 1)
                )));

            var summarization = new Summarization<Quantity>();
            //weight HV by FlowTime
            summarization.Configure("HeatingValue", "HeatingValue", new WeightedAverageRule<Quantity>(q => q.FlowTime),
                                                                    new SimpleAverageRule<Quantity>());
            var result = summarization.Execute(group);

            Assert.AreEqual(1750, result.HeatingValue);
        }

        [TestMethod]
        public void Summarization_WeightedAverageRule_SimpleAverageBackup()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { GasVolume = 3, HeatingValue = 1000 });
            records.Add(new Quantity() { GasVolume = 1, HeatingValue = 2000 });

            var group = new SummaryGroup<Quantity>(records,
                new SummaryContext(7,
                    new SummaryInfo("Meter",
                        new DateTime(2018, 1, 1)
                )));

            var summarization = new Summarization<Quantity>();
            //weight HV by FlowTime
            summarization.Configure("HeatingValue", "HeatingValue", new WeightedAverageRule<Quantity>(q => q.FlowTime), 
                                                                    new SimpleAverageRule<Quantity>());
            var result = summarization.Execute(group);

            Assert.AreEqual(1500, result.HeatingValue);
        }

        [TestMethod]
        public void Summarization_SimpleAverageRule()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { HeatingValue = 1000 });
            records.Add(new Quantity() { HeatingValue = 2000 });

            var group = new SummaryGroup<Quantity>(records,
                new SummaryContext(7,
                    new SummaryInfo("Meter",
                        new DateTime(2018, 1, 1)
                )));

            var summarization = new Summarization<Quantity>();
            //weight HV by FlowTime
            summarization.Configure("HeatingValue", "HeatingValue", new SimpleAverageRule<Quantity>());
            var result = summarization.Execute(group);

            Assert.AreEqual(1500, result.HeatingValue);
        }

        [TestMethod]
        public void Summarization_SimpleAverageRule_NullRecords()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { HeatingValue = 1000 });
            records.Add(new Quantity() { HeatingValue = 2000 });
            records.Add(new Quantity());

            var group = new SummaryGroup<Quantity>(records,
                new SummaryContext(7,
                    new SummaryInfo("Meter",
                        new DateTime(2018, 1, 1)
                )));

            var summarization = new Summarization<Quantity>();
            //weight HV by FlowTime
            summarization.Configure("HeatingValue", "HeatingValue", new SimpleAverageRule<Quantity>());
            var result = summarization.Execute(group);

            Assert.AreEqual(1500, result.HeatingValue);
        }

        [TestMethod]
        public void Summarization_DelegateRule()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { GasVolume = 150, ProductionDateStart = new DateTime(2018, 1, 1) });
            records.Add(new Quantity() { GasVolume = 200, ProductionDateStart = new DateTime(2018, 1, 2) });
            records.Add(new Quantity() { GasVolume = 250, ProductionDateStart = new DateTime(2018, 1, 3) });

            var group = new SummaryGroup<Quantity>(records,
                new SummaryContext(7,
                    new SummaryInfo("Meter",
                        new DateTime(2018, 1, 1)
                )));

            var summarization = new Summarization<Quantity>();

            summarization.Configure("HeatingValue", summarization.GetNullDelegate<double?>(), new DelegateRule<Quantity, double?>((r, context) => 10.0));
            summarization.Configure("Comment", summarization.GetNullDelegate<string>(), new DelegateRule<Quantity, string>((r, context) => "String"));
            summarization.Configure("Count", summarization.GetNullDelegate<int>(), new DelegateRule<Quantity, int>((r, context) => r.Count()));
            summarization.Configure("IndexOn", summarization.GetNullDelegate<double?>(),
                new DelegateRule<Quantity, double?>((r, context) => r.OrderBy(q => q.ProductionDateStart).FirstOrDefault()?.GasVolume));

            var result = summarization.Execute(group);

            Assert.AreEqual(10.0, result.HeatingValue);
            Assert.AreEqual("String", result.Comment);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(150, result.IndexOn);
        }

        [TestMethod]
        public void Summarization_InventoryOpeningRule()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { GasVolume = 150, ProductionDateStart = new DateTime(2018, 1, 1, 7, 0, 0, DateTimeKind.Utc) });
            records.Add(new Quantity() { GasVolume = 200, ProductionDateStart = new DateTime(2018, 1, 1, 7, 6, 0, DateTimeKind.Utc) });
            records.Add(new Quantity() { GasVolume = 250, ProductionDateStart = new DateTime(2018, 1, 1, 7, 12, 0, DateTimeKind.Utc) });

            var group = new SummaryGroup<Quantity>(records,
                new SummaryContext(7,
                    new SummaryInfo("Meter",
                        new DateTime(2018, 1, 1)
                )));

            var summarization = new Summarization<Quantity>();

            summarization.Configure("InventoryVolume", "GasVolume", new InventoryOpeningRule<Quantity>(q => q.ProductionDateStart));

            var result = summarization.Execute(group);

            Assert.AreEqual(150, result.InventoryVolume);
        }

        [TestMethod]
        [Timeout(5000)]
        public void Summarization_PerformanceTest_Hourly()
        {
            var results = data.CreateHourlySummaryGroups(0);
            Assert.AreEqual(74400, results.Count());

            Parallel.ForEach(results, group =>
            {
                var summarization = new Summarization<Quantity>();
                summarization.Configure("FlowTime", "FlowTime", new SumRule<Quantity>());
                summarization.Configure("Volume", "GasVolume", new SumRule<Quantity>());
                summarization.Configure("HeatingValue", "HeatingValue", new WeightedAverageRule<Quantity>(q => q.GasVolume));
                var result = summarization.Execute(group);
            });
        }

        [TestMethod]
        [Timeout(2000)]
        public void Summarization_PerformanceTest_Daily()
        {
            var results = data.CreateDailySummaryGroups(0);
            Assert.AreEqual(3100, results.Count());

            Parallel.ForEach(results, group =>
            {
                var summarization = new Summarization<Quantity>();
                summarization.Configure("FlowTime", "FlowTime", new SumRule<Quantity>());
                summarization.Configure("Volume", "GasVolume", new SumRule<Quantity>());
                summarization.Configure("HeatingValue", "HeatingValue", new WeightedAverageRule<Quantity>(q => q.GasVolume));
                var result = summarization.Execute(group);
            });
        }

        [TestMethod]
        [Timeout(2000)]
        public void Summarization_PerformanceTest_Monthly()
        {
            var results = data.CreateMonthlySummaryGroups(0);
            Assert.AreEqual(100, results.Count());

            Parallel.ForEach(results, group =>
            {
                var summarization = new Summarization<Quantity>();
                summarization.Configure("FlowTime", "FlowTime", new SumRule<Quantity>());
                summarization.Configure("Volume", "GasVolume", new SumRule<Quantity>());
                summarization.Configure("HeatingValue", "HeatingValue", new WeightedAverageRule<Quantity>(q => q.GasVolume));
                var result = summarization.Execute(group);
            });
        }

        [TestMethod]
        [Timeout(2000)]
        public void Summarization_PerformanceTest_Meter()
        {
            var results = data.CreateEntitySummaryGroups(0);
            Assert.AreEqual(100, results.Count());

            Parallel.ForEach(results, group =>
            {
                var summarization = new Summarization<Quantity>();
                summarization.Configure("FlowTime", "FlowTime", new SumRule<Quantity>());
                summarization.Configure("Volume", "GasVolume", new SumRule<Quantity>());
                summarization.Configure("HeatingValue", "HeatingValue", new WeightedAverageRule<Quantity>(q => q.GasVolume));
                var result = summarization.Execute(group);
            });
        }

        [TestMethod]
        [Timeout(2000)]
        public void Summarization_PerformanceTest_Aggregate()
        {
            var results = data.CreateAggregateSummaryGroups(0);
            Assert.AreEqual(1, results.Count());

            Parallel.ForEach(results, group =>
            {
                var summarization = new Summarization<Quantity>();
                summarization.Configure("FlowTime", "FlowTime", new SumRule<Quantity>());
                summarization.Configure("Volume", "GasVolume", new SumRule<Quantity>());
                summarization.Configure("HeatingValue", "HeatingValue", new WeightedAverageRule<Quantity>(q => q.GasVolume));
                var result = summarization.Execute(group);
            });
        }


        [TestMethod]
        [Timeout(2000)]
        public void Summarization_PerformanceTest_Tickets()
        {
            var results = data_tickets.CreateEntitySummaryGroups(0);
            Assert.AreEqual(10, results.Count());

            Parallel.ForEach(results, group =>
            {
                var summarization = new Summarization<Quantity>();
                summarization.Configure("FlowTime", "FlowTime", new SumRule<Quantity>());
                summarization.Configure("Volume", "GasVolume", new SumRule<Quantity>());
                summarization.Configure("HeatingValue", "HeatingValue", new WeightedAverageRule<Quantity>(q => q.GasVolume));
                var result = summarization.Execute(group);
            });
        }

        //100 meters, 6 minute records, 1 month
        public static List<Quantity> GenerateTestData()
        {
            var dtCurrent = new DateTime(2018, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var r = new Random();

            var quantities = new List<Quantity>();
            
            while (dtCurrent.Year == 2018 && dtCurrent.Month == 1)
            {
                for (int i = 1; i <= 100; ++i)
                {
                    string meterId = $"Meter{i}";
                    string stationId = $"Station{i}";
                    string ticketId = $"Ticket{i % 10}";

                    var qty = new Quantity()
                    {
                        MeterId = meterId,
                        StationId = stationId,
                        TicketId = ticketId,

                        ProductionDateStart = dtCurrent,
                        ProductionDateEnd = dtCurrent.AddMinutes(6),

                        FlowTime = 10,
                        GasVolume = 9.0 + 2.0 * r.NextDouble(),
                        HeatingValue = 990.0 + 20.0 * r.NextDouble(),
                    };
                    qty.GenerateSummaryInfo(qty.MeterId);
                    quantities.Add(qty);
                }

                dtCurrent = dtCurrent.AddMinutes(6);
            }

            return quantities;
        }
    }

    public class Quantity : IMeasurementGroupable
    {
        public string MeterId { get; set; }
        public string StationId { get; set; }
        public string TicketId { get; set; }

        public DateTime ProductionDateStart { get; set; }
        public DateTime ProductionDateEnd { get; set; }

        public double? FlowTime { get; set; }
        public double? GasVolume { get; set; }
        public double? HeatingValue { get; set; }

        public SummaryInfo Info { get; set; }

        public void GenerateSummaryInfo(string entityId)
        {
            Info = new SummaryInfo(entityId,
                new DateTime(ProductionDateStart.Year, ProductionDateStart.Month, 1),
                new DateTime(ProductionDateStart.Year, ProductionDateStart.Month, ProductionDateStart.Day),
                ProductionDateStart.Hour);
        }
    }
}

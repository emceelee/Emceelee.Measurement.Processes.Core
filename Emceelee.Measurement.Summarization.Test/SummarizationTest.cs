﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Emceelee.Measurement.Summarization.Core;
using Emceelee.Measurement.Summarization.Core.Rules;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Emceelee.Measurement.Summarization.Test
{
    [TestClass]
    public class SummarizationTest
    {
        public static List<List<Quantity>> data;
        [ClassInitialize]
        public static void Initialize(TestContext tc)
        {
            //1 month of 6-minute data
            data = GenerateTestData();
        }

        [TestMethod]
        public void Summarization_NoRecords()
        {
            var records = new List<Quantity>();

            var summarization = new Summarization<Quantity>(records);
            var result = summarization.Execute();

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Summarization_SumRule()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { FlowTime = 60, GasVolume = 20 });
            records.Add(new Quantity() { FlowTime = 40, GasVolume = 30 });
            records.Add(new Quantity() { FlowTime = 50, GasVolume = 40 });

            var summarization = new Summarization<Quantity>(records);
            summarization.Configure("FlowTime", "FlowTime", new SumRule<Quantity>());
            summarization.Configure("Volume", "GasVolume", new SumRule<Quantity>());
            var result = summarization.Execute();

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

            var summarization = new Summarization<Quantity>(records);
            summarization.Configure("FlowTime", "FlowTime", new SumRule<Quantity>());
            summarization.Configure("Volume", "GasVolume", new SumRule<Quantity>());
            var result = summarization.Execute();

            Assert.AreEqual(100, result.FlowTime);
            Assert.AreEqual(50, result.Volume);
        }

        [TestMethod]
        public void Summarization_WeightedAverageRule_FlowTime()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { FlowTime = 1, GasVolume = 3, HeatingValue = 1000 });
            records.Add(new Quantity() { FlowTime = 3, GasVolume = 1, HeatingValue = 2000 });

            var summarization = new Summarization<Quantity>(records);
            //weight HV by FlowTime
            summarization.Configure("HeatingValue", "HeatingValue", new WeightedAverageRule<Quantity>(q => q.FlowTime));
            var result = summarization.Execute();

            Assert.AreEqual(1750, result.HeatingValue);
        }

        [TestMethod]
        public void Summarization_WeightedAverageRule_GasVolume()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { FlowTime = 1, GasVolume = 3, HeatingValue = 1000 });
            records.Add(new Quantity() { FlowTime = 3, GasVolume = 1, HeatingValue = 2000 });

            var summarization = new Summarization<Quantity>(records);
            //weight HV by FlowTime
            summarization.Configure("HeatingValue", "HeatingValue", new WeightedAverageRule<Quantity>(q => q.GasVolume));
            var result = summarization.Execute();

            Assert.AreEqual(1250, result.HeatingValue);
        }

        [TestMethod]
        public void Summarization_WeightedAverageRule_NullRecords()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { FlowTime = 1, GasVolume = 3, HeatingValue = 1000 });
            records.Add(new Quantity() { FlowTime = 3, GasVolume = 1, HeatingValue = 2000 });
            records.Add(new Quantity());

            var summarization = new Summarization<Quantity>(records);
            //weight HV by FlowTime
            summarization.Configure("HeatingValue", "HeatingValue", new WeightedAverageRule<Quantity>(q => q.FlowTime));
            var result = summarization.Execute();

            Assert.AreEqual(1750, result.HeatingValue);
        }

        [TestMethod]
        public void Summarization_WeightedAverageRule_SimpleAverageBackup()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { GasVolume = 3, HeatingValue = 1000 });
            records.Add(new Quantity() { GasVolume = 1, HeatingValue = 2000 });

            var summarization = new Summarization<Quantity>(records);
            //weight HV by FlowTime
            summarization.Configure("HeatingValue", "HeatingValue", new WeightedAverageRule<Quantity>(q => q.FlowTime));
            var result = summarization.Execute();

            Assert.AreEqual(1500, result.HeatingValue);
        }

        [TestMethod]
        public void Summarization_SimpleAverageRule()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { HeatingValue = 1000 });
            records.Add(new Quantity() { HeatingValue = 2000 });

            var summarization = new Summarization<Quantity>(records);
            //weight HV by FlowTime
            summarization.Configure("HeatingValue", "HeatingValue", new SimpleAverageRule<Quantity>());
            var result = summarization.Execute();

            Assert.AreEqual(1500, result.HeatingValue);
        }

        [TestMethod]
        public void Summarization_SimpleAverageRule_NullRecords()
        {
            var records = new List<Quantity>();
            records.Add(new Quantity() { HeatingValue = 1000 });
            records.Add(new Quantity() { HeatingValue = 2000 });
            records.Add(new Quantity());

            var summarization = new Summarization<Quantity>(records);
            //weight HV by FlowTime
            summarization.Configure("HeatingValue", "HeatingValue", new SimpleAverageRule<Quantity>());
            var result = summarization.Execute();

            Assert.AreEqual(1500, result.HeatingValue);
        }

        [TestMethod]
        [Timeout(10000)]
        public void Summarization_PerformanceTest()
        {
            //1000 meters, 6 minute records, 1 month
            for(int i = 0; i < 1000; ++i)
            {
                Parallel.ForEach(data, dailyGroup =>
                    {
                        var summarization = new Summarization<Quantity>(dailyGroup);
                        summarization.Configure("FlowTime", "FlowTime", new SumRule<Quantity>());
                        summarization.Configure("Volume", "GasVolume", new SumRule<Quantity>());
                        summarization.Configure("HeatingValue", "HeatingValue", new WeightedAverageRule<Quantity>(q => q.GasVolume));
                        var result = summarization.Execute();
                    });
            }
        }

        public static List<List<Quantity>> GenerateTestData()
        {
            var dtCurrent = new DateTime(2018, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var r = new Random();

            var groupings = new List<List<Quantity>>();

            while(dtCurrent.Year == 2018 && dtCurrent.Month == 1)
            {
                var grouping = new List<Quantity>();
                groupings.Add(grouping);
                for (int i = 0; i < 24; ++i)
                {
                    grouping.Add(new Quantity()
                        {
                            ContractDay = new DateTime(dtCurrent.Year, dtCurrent.Month, dtCurrent.Day),
                            FlowTime = 10,
                            GasVolume = 9.0 + 2.0 * r.NextDouble(),
                            HeatingValue = 990.0 + 20.0 * r.NextDouble()
                        });

                    dtCurrent = dtCurrent.AddMinutes(6);
                }
            }
            
            return groupings;
        }
    }

    public class Quantity
    {
        public DateTime ContractDay { get; set; }
        public double? FlowTime { get; set; }
        public double? GasVolume { get; set; }
        public double? HeatingValue { get; set; }
    }
}
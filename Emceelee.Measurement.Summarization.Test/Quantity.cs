using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emceelee.Measurement.Summarization.Core;
using Emceelee.Summarization.Core.Rules;

namespace Emceelee.Measurement.Summarization.Test
{
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

        public static Summarization<Quantity> GenerateStandardSummarization()
        {
            var summarization = new Summarization<Quantity>()
                .Configure(nameof(Summary.ProductionDateStart), nameof(Quantity.ProductionDateStart), new FirstOrDefaultRule<Quantity, DateTime>(q => q.ProductionDateStart))
                .Configure(nameof(Summary.ProductionDateEnd), nameof(Quantity.ProductionDateEnd), new FirstOrDefaultRule<Quantity, DateTime>(q => q.ProductionDateEnd))
                .Configure(nameof(Summary.FlowTime), nameof(Quantity.FlowTime), new SumRule<Quantity>())
                .Configure(nameof(Summary.Volume), nameof(Quantity.GasVolume), new SumRule<Quantity>())
                .Configure(nameof(Summary.HeatingValue), nameof(Quantity.HeatingValue), new WeightedAverageRule<Quantity>(q => q.GasVolume));

            return summarization;
        }
    }
}

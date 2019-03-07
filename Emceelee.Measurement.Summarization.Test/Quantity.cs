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
            var delegateProductionDateStart = new DelegateRule<Quantity, DateTime?>((r, context) => r.OrderBy(q => q.ProductionDateStart).FirstOrDefault()?.ProductionDateStart);
            var delegateProductionDateEnd = new DelegateRule<Quantity, DateTime?>((r, context) => r.OrderByDescending(q => q.ProductionDateEnd).FirstOrDefault()?.ProductionDateEnd);

            var summarization = new Summarization<Quantity>()
                .Configure(nameof(Summary.ProductionDateStart), delegateProductionDateStart.NullDelegate, delegateProductionDateStart)
                .Configure(nameof(Summary.ProductionDateEnd), delegateProductionDateEnd.NullDelegate, delegateProductionDateEnd)
                .Configure(nameof(Summary.FlowTime), nameof(Quantity.FlowTime), new SumRule<Quantity>())
                .Configure(nameof(Summary.Volume), nameof(Quantity.GasVolume), new SumRule<Quantity>())
                .Configure(nameof(Summary.HeatingValue), nameof(Quantity.HeatingValue), new WeightedAverageRule<Quantity>(q => q.GasVolume));

            return summarization;
        }
    }
}

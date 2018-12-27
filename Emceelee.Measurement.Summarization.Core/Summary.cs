using System;
using System.Collections.Generic;
using System.Text;

namespace Emceelee.Measurement.Summarization.Core
{
    public class Summary: IMeasurementGroupable
    {
        public SummaryContext Context { get; }
        public SummaryInfo Info { get { return Context.Info; } }

        public Summary(SummaryContext sc)
        {
            Context = sc;
        }

        public string ObjectId { get; set; }
        public double? FlowTime { get; set; }
        public double? Volume { get; set; }
        public double? InventoryVolume { get; set; }
        public double? IndexOn { get; set; }
        public double? IndexOff { get; set; }
        public double? HeatingValue { get; set; }
        public string Comment { get; set; }
        public int Count { get; set; }
    }
}

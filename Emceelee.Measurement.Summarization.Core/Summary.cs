﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Emceelee.Measurement.Summarization.Core
{
    public class Summary
    {
        public double? FlowTime { get; set; }
        public double? Volume { get; set; }
        public double? InventoryVolume { get; set; }
        public double? HeatingValue { get; set; }
        public string Comment { get; set; }
        public int Count { get; set; }
    }
}

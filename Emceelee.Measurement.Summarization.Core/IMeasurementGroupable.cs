﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emceelee.Measurement.Summarization.Core
{
    public interface IMeasurementGroupable
    {
        SummaryKey Key { get; }
    }
}

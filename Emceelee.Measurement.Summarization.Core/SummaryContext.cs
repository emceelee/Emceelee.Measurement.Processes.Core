using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emceelee.Summarization.Core;

namespace Emceelee.Measurement.Summarization.Core
{
    public class SummaryContext : ISummaryContext
    {
        public int ContractHour { get; }
        public SummaryKey Key { get; }

        public SummaryContext(int contractHour, SummaryKey key)
        {
            if (contractHour < 0 || contractHour > 23)
            {
                throw new ArgumentOutOfRangeException($"contractHour {contractHour} not between 0 and 23.");
            }

            ContractHour = contractHour;
            Key = key;
        }

        public DateTime ProductionDateStart
        {
            get
            {
                //aggregate
                if(Key.Month == default(DateTime))
                {
                    return DateTime.MinValue;
                }

                //monthly
                if(Key.Day == default(DateTime))
                {
                    return Key.Month.AddHours(ContractHour);
                }

                //daily
                if(Key.Hour == null)
                {
                    return Key.Day.AddHours(ContractHour);
                }

                //hourly
                return Key.Day.AddHours(Key.Hour ?? 0);
            }
        }
        public DateTime ProductionDateEnd
        {
            get
            {
                //aggregate
                if (Key.Month == default(DateTime))
                {
                    return DateTime.MaxValue;
                }

                //monthly
                if (Key.Day == default(DateTime))
                {
                    return Key.Month.AddMonths(1).AddHours(ContractHour);
                }

                //daily
                if (Key.Hour == null)
                {
                    return Key.Day.AddDays(1).AddHours(ContractHour);
                }

                //hourly
                return Key.Day.AddHours((Key.Hour ?? 0) + 1);
            }
        }
    }
}

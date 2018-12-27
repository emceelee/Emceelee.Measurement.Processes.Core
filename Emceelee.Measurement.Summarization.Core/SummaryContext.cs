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
        public SummaryInfo Info { get; }

        public SummaryContext(int contractHour, SummaryInfo info = null)
        {
            if (contractHour < 0 || contractHour > 23)
            {
                throw new ArgumentOutOfRangeException($"contractHour {contractHour} not between 0 and 23.");
            }

            ContractHour = contractHour;
            Info = info;
        }

        public DateTime ProductionDateStart
        {
            get
            {
                //aggregate
                if(Info.Month == default(DateTime))
                {
                    return DateTime.MinValue;
                }

                //monthly
                if(Info.Day == default(DateTime))
                {
                    return Info.Month.AddHours(ContractHour);
                }

                //daily
                if(Info.Hour == null)
                {
                    return Info.Day.AddHours(ContractHour);
                }

                //hourly
                return Info.Day.AddHours(Info.Hour ?? 0);
            }
        }
        public DateTime ProductionDateEnd
        {
            get
            {
                //aggregate
                if (Info.Month == default(DateTime))
                {
                    return DateTime.MaxValue;
                }

                //monthly
                if (Info.Day == default(DateTime))
                {
                    return Info.Month.AddMonths(1).AddHours(ContractHour);
                }

                //daily
                if (Info.Hour == null)
                {
                    return Info.Day.AddDays(1).AddHours(ContractHour);
                }

                //hourly
                return Info.Day.AddHours((Info.Hour ?? 0) + 1);
            }
        }
    }
}

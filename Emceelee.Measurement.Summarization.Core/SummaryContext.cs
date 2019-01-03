using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emceelee.Summarization.Core;

namespace Emceelee.Measurement.Summarization.Core
{
    public enum SummaryInterval
    {
        Aggregate,
        Monthly,
        Daily,
        Hourly
    }

    public class SummaryContext : ISummaryContext
    {
        public int ContractHour { get; }
        public SummaryInfo Info { get; }

        public SummaryContext(int contractHour, SummaryInfo info)
        {
            if (contractHour < 0 || contractHour > 23)
            {
                throw new ArgumentOutOfRangeException($"contractHour {contractHour} not between 0 and 23.");
            }

            ContractHour = contractHour;
            Info = info;
        }

        public SummaryInterval Interval
        {
            get
            {
                if (Info.Month == default(DateTime))
                {
                    return SummaryInterval.Aggregate;
                }

                if (Info.Day == default(DateTime))
                {
                    return SummaryInterval.Monthly;
                }

                if (Info.Hour == null)
                {
                    return SummaryInterval.Daily;
                }

                return SummaryInterval.Hourly;
            }
        }

        public DateTime ProductionDateStart
        {
            get
            {
                switch(Interval)
                {
                    case SummaryInterval.Aggregate:
                        return DateTime.MinValue;
                    case SummaryInterval.Monthly:
                        return Info.Month.AddHours(ContractHour);
                    case SummaryInterval.Daily:
                        return Info.Day.AddHours(ContractHour);
                    case SummaryInterval.Hourly:
                        return Info.Day.AddHours(Info.Hour ?? 0);
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public DateTime ProductionDateEnd
        {
            get
            {
                switch (Interval)
                {
                    case SummaryInterval.Aggregate:
                        return DateTime.MaxValue;
                    case SummaryInterval.Monthly:
                        return Info.Month.AddMonths(1).AddHours(ContractHour);
                    case SummaryInterval.Daily:
                        return Info.Day.AddDays(1).AddHours(ContractHour);
                    case SummaryInterval.Hourly:
                        return Info.Day.AddHours((Info.Hour ?? 0) + 1);
                    default:
                        throw new InvalidOperationException();
                }
            }
        }
    }
}

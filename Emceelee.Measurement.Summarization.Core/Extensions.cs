using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emceelee.Measurement.Summarization.Core
{
    public static class Extensions
    {
        public static List<SummaryGroup<T>> CreateSummaryGroups<T>(this IEnumerable<T> records, int contractHour, Func<T, SummaryInfo> keySelector)
            where T : IMeasurementGroupable
        {
            var groups = records.GroupBy
                (
                    t => keySelector(t),
                    t => t
                );

            var results = groups.Select(group =>
            {
                var summaryGroup = new SummaryGroup<T>(group.ToList(), new SummaryContext(contractHour, group.Key));

                return summaryGroup;
            }).ToList();

            return results;
        }

        public static List<SummaryGroup<T>> CreateAggregateSummaryGroups<T>(this IEnumerable<T> records, int contractHour)
            where T : IMeasurementGroupable
        {
            return CreateSummaryGroups(records, 
                    contractHour, 
                    t => new SummaryInfo(string.Empty)
                );
        }

        public static List<SummaryGroup<T>> CreateEntitySummaryGroups<T>(this IEnumerable<T> records, int contractHour)
            where T : IMeasurementGroupable
        {
            return CreateSummaryGroups(records,
                    contractHour,
                    t => new SummaryInfo(t.Info.EntityId)
                );
        }

        public static List<SummaryGroup<T>> CreateMonthlySummaryGroups<T>(this IEnumerable<T> records, int contractHour)
            where T : IMeasurementGroupable
        {
            return CreateSummaryGroups(records,
                    contractHour,
                    t => new SummaryInfo(t.Info.EntityId, t.Info.Month)
                );
        }

        public static List<SummaryGroup<T>> CreateDailySummaryGroups<T>(this IEnumerable<T> records, int contractHour)
            where T : IMeasurementGroupable
        {
            return CreateSummaryGroups(records,
                    contractHour,
                    t => new SummaryInfo(t.Info.EntityId, t.Info.Month, t.Info.Day)
                );
        }

        public static List<SummaryGroup<T>> CreateHourlySummaryGroups<T>(this IEnumerable<T> records, int contractHour)
            where T : IMeasurementGroupable
        {
            return CreateSummaryGroups(records,
                    contractHour,
                    t => new SummaryInfo(t.Info.EntityId, t.Info.Month, t.Info.Day, t.Info.Hour)
                );
        }
    }
}

using Emceelee.Measurement.Summarization.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emceelee.Measurement.Summarization.Test
{
    public static class TestData
    {
        public static List<Quantity> Data;
        public static List<Quantity> DataTickets;
        public static Summarization<Quantity> StandardQtySummarization;

        public static void Initialize()
        {
            //1 month of 6-minute data
            Data = GenerateTestData();
            DataTickets = GenerateTestData();
            foreach (var qty in DataTickets)
            {
                qty.GenerateSummaryInfo(qty.TicketId);
            }
            StandardQtySummarization = Quantity.GenerateStandardSummarization();
        }
        
        //100 meters, 6 minute records, 1 month
        private static List<Quantity> GenerateTestData()
        {
            var dtCurrent = new DateTime(2018, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var r = new Random();

            var quantities = new List<Quantity>();

            while (dtCurrent.Year == 2018 && dtCurrent.Month == 1)
            {
                for (int i = 1; i <= 100; ++i)
                {
                    string meterId = $"Meter{i}";
                    string stationId = $"Station{i}";
                    string ticketId = $"Ticket{i % 10}";

                    var qty = new Quantity()
                    {
                        MeterId = meterId,
                        StationId = stationId,
                        TicketId = ticketId,

                        ProductionDateStart = dtCurrent,
                        ProductionDateEnd = dtCurrent.AddMinutes(6),

                        FlowTime = 10,
                        GasVolume = 9.0 + 2.0 * r.NextDouble(),
                        HeatingValue = 990.0 + 20.0 * r.NextDouble(),
                    };
                    qty.GenerateSummaryInfo(qty.MeterId);
                    quantities.Add(qty);
                }

                dtCurrent = dtCurrent.AddMinutes(6);
            }

            return quantities;
        }
    }
}

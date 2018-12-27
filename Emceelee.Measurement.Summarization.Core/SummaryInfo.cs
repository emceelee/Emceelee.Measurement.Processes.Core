using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emceelee.Measurement.Summarization.Core
{
    public class SummaryInfo : IEquatable<SummaryInfo>
    {
        public string EntityId { get; }
        public DateTime Month { get; }
        public DateTime Day { get; }
        public int? Hour { get; }

        public SummaryInfo(string entityId, DateTime month = default(DateTime), DateTime day = default(DateTime), int? hour = null)
        {
            EntityId = entityId;
            Month = month;
            Day = day;
            Hour = hour;
        }

        //override for performance
        public bool Equals(SummaryInfo other)
        {
            return this.EntityId == other.EntityId &&
                this.Month == other.Month &&
                this.Day == other.Day &&
                this.Hour == other.Hour;
        }

        public override bool Equals(object obj)
        {
            return obj is SummaryInfo && this.Equals(obj);
        }

        public override int GetHashCode()
        {
            return EntityId.GetHashCode() ^
                ShiftAndWrap(Month.GetHashCode(), 2) ^
                ShiftAndWrap(Day.GetHashCode(), 4) ^
                ShiftAndWrap(Hour.GetHashCode(), 6);
        }

        private int ShiftAndWrap(int value, int positions)
        {
            positions = positions & 0x1F;

            // Save the existing bit pattern, but interpret it as an unsigned integer.
            uint number = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
            // Preserve the bits to be discarded.
            uint wrapped = number >> (32 - positions);
            // Shift and wrap the discarded bits.
            return BitConverter.ToInt32(BitConverter.GetBytes((number << positions) | wrapped), 0);
        }
    }
}

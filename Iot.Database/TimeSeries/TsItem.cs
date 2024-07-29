using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Database.TimeSeries
{
    public class TsItem
    {
        public DateTime Timestamp { get; set; }
        public BsonValue Value { get; set; }

        public TsItem() { }
        public TsItem(DateTime timestamp, BsonValue value) { Timestamp = timestamp; Value = value; }
    }
}

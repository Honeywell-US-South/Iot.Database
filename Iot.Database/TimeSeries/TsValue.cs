using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Database.TimeSeries
{
    public class TsValue
    {
        public string EntityId { get; set; }
        public List<TsItem> Series { get; set; } = new List<TsItem>();
    }
}

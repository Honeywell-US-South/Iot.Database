using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Database.Queries
{
    public class QueryDefinition
    {
        public string Where { get; set; }
        public string Select { get; set; }
        public string OrderBy { get; set; }
        public bool OrderByDescending { get; set; }
        public string Aggregate { get; set; }
        public int? Take { get; set; }
        public int? Skip { get; set; }
    }
}

using Iot.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Shared.Models
{
    public class IotPoint : IotValue
    {
        public Guid Id { get; set; } //all table must have an Id of type (int, long or System.Guid)

    }
}

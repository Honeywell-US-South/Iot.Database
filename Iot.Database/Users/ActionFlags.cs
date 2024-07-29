using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Database.Users
{
    [Flags]
    public enum ActionFlags
    {
        None = 0,
        Read = 1 << 0,
        Write = 1 << 1,
        Delete = 1 << 2,
        Update = 1 << 3,
        Execute = 1 << 4,
        FullControl = Read | Write | Delete | Update | Execute
    }
}

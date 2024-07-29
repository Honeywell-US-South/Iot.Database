using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using static Iot.Database.Constants;

namespace Iot.Database.Engine
{
    internal enum TransactionState
    {
        Active,
        Committed,
        Aborted,
        Disposed
    }
}
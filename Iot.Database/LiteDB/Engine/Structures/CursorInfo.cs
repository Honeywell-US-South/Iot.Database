using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using static Iot.Database.Constants;

namespace Iot.Database.Engine
{
    /// <summary>
    /// Represent a single query featching data from engine
    /// </summary>
    internal class CursorInfo
    {
        public CursorInfo(string collection, Query query)
        {
            this.Collection = collection;
            this.Query = query;
        }

        public string Collection { get; }

        public Query Query { get; set; }

        public int Fetched { get; set; }

        public Stopwatch Elapsed { get; } = new Stopwatch();
    }
}
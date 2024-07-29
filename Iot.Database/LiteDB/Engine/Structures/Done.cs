using System;
using System.Collections.Generic;
using static Iot.Database.Constants;

namespace Iot.Database.Engine
{
    /// <summary>
    /// Simple parameter class to be passed into IEnumerable classes loop ("ref" do not works)
    /// </summary>
    internal class Done
    {
        public bool Running = false;
        public int Count = 0;
    }
}
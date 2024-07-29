using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using static Iot.Database.Constants;

namespace Iot.Database
{
    /// <summary>
    /// When a method are decorated with this attribute means that this method are not immutable
    /// </summary>
    internal class VolatileAttribute: Attribute
    {
    }
}

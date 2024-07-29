using System;
using static Iot.Database.Constants;

namespace Iot.Database
{
    /// <summary>
    /// Indicate that property will not be persist in Bson serialization
    /// </summary>
    public class BsonIgnoreAttribute : Attribute
    {
    }
}
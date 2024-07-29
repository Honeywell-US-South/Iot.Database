using System;

namespace Iot.Database
{
    public interface IBsonDataReader : IDisposable
    {
        BsonValue this[string field] { get; }

        string Collection { get; }
        BsonValue Current { get; }
        bool HasValues { get; }

        bool Read();
    }
}
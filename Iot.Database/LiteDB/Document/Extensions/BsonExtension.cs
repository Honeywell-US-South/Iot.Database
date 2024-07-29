using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Database.LiteDB.Document.Extensions
{
    public static class BsonExtension
    {
        public static bool IsBsonT(this BsonValue bsonValue)
        {
            try
            {
                BsonT? bsonT = BsonT.ToBsonT(bsonValue.RawValue.ToString());
                return bsonT != null;

            } catch { }
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Iot.Database.LiteDB.Document
{
    /// <summary>
    /// Support class to store serializable object into BsonValue
    /// </summary>
    public class BsonT
    {
        public string Type { get; set; }
        public string SerializedValue { get; set; }

        public BsonT() { 
        
            Type = typeof(Nullable).FullName ?? string.Empty;
            SerializedValue = string.Empty;
        }
        public BsonT(object value)
        {
            Type = value.GetType().FullName ?? string.Empty;
            try
            {
                var json = JsonSerializer.SerializeObject(value);
                if (json == null) throw new NotSupportedException("Unsupport not serializable object.");
                SerializedValue = json;
            } catch { throw new NotSupportedException("Unsupport not serializable object."); }
        }

        [JsonIgnore]
        public bool IsNull => Type == typeof(Nullable).FullName || string.IsNullOrEmpty(SerializedValue);
        public string ToJson()
        {
            if (Type == typeof(Nullable).FullName) return string.Empty;
            var json = JsonSerializer.SerializeObject(this);
            return json;
        }

        public static bool IsBsonT(string? jsonString)
        {
            if (string.IsNullOrEmpty(jsonString)) return false;
            var bsonT = JsonSerializer.DeserializeObject<BsonT>(jsonString);
            if (bsonT?.IsNull??false) return false;
            return bsonT != null;
        }
        
        public static BsonT? ToBsonT(string? jsonString)
        {
            if (string.IsNullOrEmpty(jsonString)) return null;
            var bsonT = JsonSerializer.DeserializeObject<BsonT>(jsonString);
            if (bsonT?.IsNull??true) return null;
            return bsonT;
        }

        public T? ToObjectT<T>()
        {
            if (Type != typeof(T).FullName) return default(T?);
            var t = JsonSerializer.DeserializeObject<T>(SerializedValue);
            return t;
        }
    }
}

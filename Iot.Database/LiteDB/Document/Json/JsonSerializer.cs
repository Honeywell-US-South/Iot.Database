using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using static Iot.Database.Constants;

namespace Iot.Database
{
    /// <summary>
    /// Static class for serialize/deserialize BsonDocuments into json extended format
    /// </summary>
    public class JsonSerializer
    {
        #region Serialize

        /// <summary>
        /// Json serialize a BsonValue into a String
        /// </summary>
        public static string Serialize(BsonValue value)
        {
            var sb = new StringBuilder();

            Serialize(value, sb);

            return sb.ToString();
        }

        /// <summary>
        /// Json serialize a BsonValue into a TextWriter
        /// </summary>
        public static void Serialize(BsonValue value, TextWriter writer)
        {
            var json = new JsonWriter(writer);

            json.Serialize(value ?? BsonValue.Null);
        }

        /// <summary>
        /// Json serialize a BsonValue into a StringBuilder
        /// </summary>
        public static void Serialize(BsonValue value, StringBuilder sb)
        {
            using (var writer = new StringWriter(sb))
            {
                var w = new JsonWriter(writer);

                w.Serialize(value ?? BsonValue.Null);
            }
        }

        public static string SerializeObject<T>(T obj)
        {
            // JsonSerializerOptions can be customized for more control over serialization
            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // Pretty-print JSON
                IgnoreNullValues = true // Ignore null values
            };

            // Serialize the object to JSON string
            return System.Text.Json.JsonSerializer.Serialize(obj, options);
        }

        #endregion

        #region Deserialize

        /// <summary>
        /// Deserialize a Json string into a BsonValue
        /// </summary>
        public static BsonValue Deserialize(string json)
        {
            if (json == null) throw new ArgumentNullException(nameof(json));

            using (var sr = new StringReader(json))
            {
                var reader = new JsonReader(sr);

                return reader.Deserialize();
            }
        }

        /// <summary>
        /// Deserialize a Json TextReader into a BsonValue
        /// </summary>
        public static BsonValue Deserialize(TextReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            var jr = new JsonReader(reader);

            return jr.Deserialize();
        }

        /// <summary>
        /// Deserialize a json array as an IEnumerable of BsonValue
        /// </summary>
        public static IEnumerable<BsonValue> DeserializeArray(string json)
        {
            if (json == null) throw new ArgumentNullException(nameof(json));

            var sr = new StringReader(json);
            var reader = new JsonReader(sr);
            return reader.DeserializeArray();
        }

        /// <summary>
        /// Deserialize a json array as an IEnumerable of BsonValue reading on demand TextReader
        /// </summary>
        public static IEnumerable<BsonValue> DeserializeArray(TextReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            var jr = new JsonReader(reader);

            return jr.DeserializeArray();
        }

        public static T? DeserializeObject<T>(string jsonString)
        {
            // JsonSerializerOptions can be customized for more control over deserialization
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignore case when matching property names
            };

            // Deserialize the JSON string to an object
            //return System.Text.Json.JsonSerializer.Deserialize<T>(jsonString, options);

            try
            {
                using (JsonDocument.Parse(jsonString))
                {
                    return System.Text.Json.JsonSerializer.Deserialize<T>(jsonString, options);
                }
            } catch (JsonException)
            {
                if(typeof(T) == typeof(string))
                {
                    return (T)(object)jsonString;
                }
                return default;
            }
        }
        #endregion
    }
}
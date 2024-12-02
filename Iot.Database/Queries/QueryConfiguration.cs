using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Iot.Database.Queries
{

    public class QueryConfiguration : IDisposable
    {
        public string Query { get; set; } = string.Empty;
        [JsonIgnore]
        [BsonIgnore]
        [XmlIgnore]
        public Func<string, object>? ExecutionFunction { get; set; }
        public int IntervalMilliseconds { get; set; }
        public DateTime LastExecuted { get; set; }
        [JsonIgnore]
        [BsonIgnore]
        [XmlIgnore]
        public object? LastResult { get; set; }
        [JsonIgnore]
        [BsonIgnore]
        [XmlIgnore]
        public Action<QueryResultEventArgs>? OnSuccess { get; set; }
        [JsonIgnore]
        [BsonIgnore]
        [XmlIgnore]
        public Action<QueryFailureEventArgs>? OnFailure { get; set; }

        // Dispose pattern to automatically clean up resources
        public void Dispose()
        {
            ExecutionFunction = null;
            OnSuccess = null;
            OnFailure = null;
        }
    }
}


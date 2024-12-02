namespace Iot.Database.Queries
{
    public sealed partial class QueryExecutionService
    {
        public class QueryConfiguration
        {
            public string Query { get; set; }
            public Func<string, object> ExecutionFunction { get; set; }
            public int IntervalMilliseconds { get; set; }
            public DateTime LastExecuted { get; set; }
            public object LastResult { get; set; }
            public Action<QueryResultEventArgs>? OnSuccess { get; set; }
            public Action<QueryFailureEventArgs>? OnFailure { get; set; }
        }
    }
}

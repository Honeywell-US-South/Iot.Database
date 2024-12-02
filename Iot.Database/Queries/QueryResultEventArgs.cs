namespace Iot.Database.Queries
{
    public class QueryResultEventArgs : EventArgs
    {
        public string Key { get; }
        public object Result { get; }
        public DateTime ExecutedAt { get; }

        public QueryResultEventArgs(string key, object result, DateTime executedAt)
        {
            Key = key;
            Result = result;
            ExecutedAt = executedAt;
        }
    }
}

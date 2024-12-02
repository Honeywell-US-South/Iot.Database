namespace Iot.Database.Queries
{
    public class QueryFailureEventArgs : EventArgs
    {
        public string Key { get; }
        public Exception Exception { get; }

        public QueryFailureEventArgs(string key, Exception exception)
        {
            Key = key;
            Exception = exception;
        }
    }
}

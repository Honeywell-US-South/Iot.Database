using System.Collections.Concurrent;
using System.Timers;

namespace Iot.Database.Queries
{
    public sealed partial class QueryExecutionService
    {
        private static readonly Lazy<QueryExecutionService> _instance = new(() => new QueryExecutionService());
        private readonly ConcurrentDictionary<string, QueryConfiguration> _queries = new();
        private readonly System.Timers.Timer _timer;

        private QueryExecutionService()
        {
            _timer = new System.Timers.Timer(1000); // Tick every second
            _timer.Elapsed += ExecuteQueries;
            _timer.Start();
        }

        public static QueryExecutionService Instance => _instance.Value;

        public void AddQuery(string key,
                             string query,
                             Func<string, object> executionFunction,
                             Action<QueryResultEventArgs>? onSuccess = null,
                             Action<QueryFailureEventArgs>? onFailure = null,
                             int intervalMilliseconds = 0)
        {
            if (!_queries.TryAdd(key, new QueryConfiguration
            {
                Query = query,
                ExecutionFunction = executionFunction,
                IntervalMilliseconds = intervalMilliseconds,
                LastExecuted = DateTime.MinValue,
                OnSuccess = onSuccess,
                OnFailure = onFailure
            }))
            {
                throw new InvalidOperationException($"A query with the key '{key}' already exists.");
            }
        }

        public bool IsKeyExist(string key) { return _queries.ContainsKey(key); }

        public QueryConfiguration GetQueryConfiguration(string key)
        {
            if (IsKeyExist(key))
            {
                return _queries[key];
            }
            throw new KeyNotFoundException($"No query found with the key '{key}'.");
        }

        public void RemoveQuery(string key)
        {
            if (!_queries.TryRemove(key, out _))
            {
                throw new KeyNotFoundException($"No query found with the key '{key}'.");
            }
        }

        public object ExecuteQuery(string key)
        {
            if (_queries.TryGetValue(key, out var queryConfig))
            {
                if (queryConfig.ExecutionFunction == null)
                {
                    throw new InvalidOperationException("Execution function is not set.");
                }

                try
                {
                    var result = queryConfig.ExecutionFunction(queryConfig.Query);
                    queryConfig.LastResult = result;
                    queryConfig.LastExecuted = DateTime.Now;

                    // Notify only the specific subscriber
                    queryConfig.OnSuccess?.Invoke(new QueryResultEventArgs(key, result, queryConfig.LastExecuted));

                    return result;
                }
                catch (Exception ex)
                {
                    // Notify only the specific subscriber
                    queryConfig.OnFailure?.Invoke(new QueryFailureEventArgs(key, ex));
                    throw;
                }
            }
            throw new KeyNotFoundException($"No query found with the key '{key}'.");
        }

        private void ExecuteQueries(object sender, ElapsedEventArgs e)
        {
            foreach (var keyValue in _queries)
            {
                var key = keyValue.Key;
                var config = keyValue.Value;

                if (config.IntervalMilliseconds > 0 &&
                    (DateTime.Now - config.LastExecuted).TotalMilliseconds >= config.IntervalMilliseconds)
                {
                    try
                    {
                        var result = config.ExecutionFunction(config.Query);
                        config.LastResult = result;
                        config.LastExecuted = DateTime.Now;

                        // Notify only the specific subscriber
                        config.OnSuccess?.Invoke(new QueryResultEventArgs(key, result, config.LastExecuted));
                    }
                    catch (Exception ex)
                    {
                        // Notify only the specific subscriber
                        config.OnFailure?.Invoke(new QueryFailureEventArgs(key, ex));
                    }
                }
            }
        }


    }
}

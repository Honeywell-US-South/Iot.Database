namespace Iot.Database.Queries
{
    public class QueryBuilder<T> where T : class
    {
        private readonly IEnumerable<T> _data;
        private string _whereMethod;
        private List<object> _whereArguments = new();
        private string _selectMethod;
        private List<object> _selectArguments = new();
        private Func<T, object> _orderBySelector;
        private bool _orderByDescending;
        private Func<IEnumerable<object>, object> _aggregate;
        private int? _take;
        private int? _skip;

        public QueryBuilder(IEnumerable<T> data)
        {
            _data = data;
        }

        public QueryBuilder<T> Where(Func<T, bool> predicate, string method, params object[] arguments)
        {
            _whereMethod = method;
            _whereArguments = arguments.ToList();
            return this;
        }

        public QueryBuilder<T> Select(Func<T, object> selector, string method, params object[] arguments)
        {
            _selectMethod = method;
            _selectArguments = arguments.ToList();
            return this;
        }

        public QueryBuilder<T> OrderBy(Func<T, object> keySelector)
        {
            _orderBySelector = keySelector;
            _orderByDescending = false;
            return this;
        }

        public QueryBuilder<T> OrderByDescending(Func<T, object> keySelector)
        {
            _orderBySelector = keySelector;
            _orderByDescending = true;
            return this;
        }

        public QueryBuilder<T> Take(int count)
        {
            _take = count;
            return this;
        }

        public QueryBuilder<T> Skip(int count)
        {
            _skip = count;
            return this;
        }

        public QueryBuilder<T> Count()
        {
            _aggregate = query => query.Count();
            return this;
        }

        public QueryBuilder<T> Sum()
        {
            _aggregate = query => query.Sum(x => Convert.ToDouble(x));
            return this;
        }

        public QueryBuilder<T> Average()
        {
            _aggregate = query => query.Average(x => Convert.ToDouble(x));
            return this;
        }

        public QueryBuilder<T> Min()
        {
            _aggregate = query => query.Min();
            return this;
        }

        public QueryBuilder<T> Max()
        {
            _aggregate = query => query.Max();
            return this;
        }

        public IEnumerable<object> Execute()
        {
            var query = _data.AsEnumerable();

            if (!string.IsNullOrEmpty(_whereMethod))
            {
                query = query.Where(BuildPredicate(_whereMethod, _whereArguments));
            }

            if (_orderBySelector != null)
            {
                query = _orderByDescending
                    ? query.OrderByDescending(_orderBySelector)
                    : query.OrderBy(_orderBySelector);
            }

            if (_skip.HasValue)
            {
                query = query.Skip(_skip.Value);
            }

            if (_take.HasValue)
            {
                query = query.Take(_take.Value);
            }

            var result = !string.IsNullOrEmpty(_selectMethod)
                ? query.Select(BuildSelector(_selectMethod, _selectArguments)).Cast<object>()
                : query.Cast<object>();

            return _aggregate != null ? new List<object> { _aggregate(result) } : result;
        }

        public string SaveToJson()
        {
            var queryDefinition = new QueryDefinition
            {
                Where = _whereMethod + "|" + string.Join("|", _whereArguments),
                Select = _selectMethod + "|" + string.Join("|", _selectArguments),
                OrderBy = _orderBySelector?.Method.Name,
                OrderByDescending = _orderByDescending,
                Aggregate = _aggregate?.Method.Name,
                Take = _take,
                Skip = _skip
            };

            return System.Text.Json.JsonSerializer.Serialize(queryDefinition);
        }

        public IEnumerable<object> ExecuteQuery(string jsonQuery)
        {
            var queryDefinition = System.Text.Json.JsonSerializer.Deserialize<QueryDefinition>(jsonQuery);

            var query = _data.AsEnumerable();

            if (!string.IsNullOrEmpty(queryDefinition?.Where))
            {
                var whereParts = queryDefinition.Where.Split('|'); // Split into method and arguments
                var whereMethod = whereParts[0];
                var whereArguments = whereParts.Skip(1).Select(arg => (object)arg).ToList(); // Remaining parts as arguments
                query = query.Where(BuildPredicate(whereMethod, whereArguments));
            }

            if (!string.IsNullOrEmpty(queryDefinition?.OrderBy))
            {
                var orderByParts = queryDefinition.OrderBy.Split('|'); // Split into method and arguments
                var orderByMethod = orderByParts[0];
                var orderByArguments = orderByParts.Skip(1).Select(arg => (object)arg).ToList(); // Remaining parts as arguments
                var orderBySelector = BuildSelector(orderByMethod, orderByArguments);
                query = queryDefinition.OrderByDescending
                    ? query.OrderByDescending(orderBySelector)
                    : query.OrderBy(orderBySelector);
            }

            if (queryDefinition?.Skip.HasValue == true)
            {
                query = query.Skip(queryDefinition.Skip.Value);
            }

            if (queryDefinition?.Take.HasValue == true)
            {
                query = query.Take(queryDefinition.Take.Value);
            }

            var result = !string.IsNullOrEmpty(queryDefinition?.Select)
                ? query.Select(BuildSelector(queryDefinition.Select, queryDefinition.Select.Split('|').Skip(1).Select(arg => (object)arg).ToList()))
                : query.Cast<object>();

            if (!string.IsNullOrEmpty(queryDefinition?.Aggregate))
            {
                var aggregateParts = queryDefinition.Aggregate.Split('|'); // Split into method and arguments
                var aggregateMethod = aggregateParts[0];
                var aggregateArguments = aggregateParts.Skip(1).Select(arg => (object)arg).ToList(); // Remaining parts as arguments
                var aggregateFunction = BuildAggregate(aggregateMethod, aggregateArguments);
                return new List<object> { aggregateFunction(result) };
            }

            return result;
        }

        private Func<T, bool> BuildPredicate(string whereClause, List<object> arguments)
        {
            return whereClause switch
            {
                "StartsWith" => item => item.ToString().StartsWith(arguments[0].ToString()),
                "EndsWith" => item => item.ToString().EndsWith(arguments[0].ToString()),
                "Contains" => item => item.ToString().Contains(arguments[0].ToString()),
                _ => throw new NotImplementedException($"Predicate '{whereClause}' is not implemented.")
            };
        }

        private Func<T, object> BuildSelector(string selectClause, List<object> arguments)
        {
            return selectClause switch
            {
                "ToUpper" => item => item.ToString().ToUpper(),
                "ToLower" => item => item.ToString().ToLower(),
                "Substring" => item =>
                {
                    var start = int.Parse(arguments[0].ToString());
                    var length = arguments.Count > 1 ? int.Parse(arguments[1].ToString()) : item.ToString().Length - start;
                    return item.ToString().Substring(start, length);
                }
                ,
                _ => throw new NotImplementedException($"Selector '{selectClause}' is not implemented.")
            };
        }

        private Func<IEnumerable<object>, object> BuildAggregate(string aggregateClause, List<object> arguments)
        {
            return aggregateClause switch
            {
                "Count" => query => query.Count(),
                "Sum" => query => query.Sum(x => Convert.ToDouble(x)),
                "Average" => query => query.Average(x => Convert.ToDouble(x)),
                "Min" => query => query.Min(),
                "Max" => query => query.Max(),
                _ => throw new NotImplementedException($"Aggregate '{aggregateClause}' is not implemented.")
            };
        }
    }

    
}

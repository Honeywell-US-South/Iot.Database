using System;

namespace Iot.Database.Queries
{
    public class QueryBuilder<T> where T : class
    {
        private readonly IEnumerable<T> _data;
        private Func<T, bool> _predicate;
        private Func<T, object> _selector;
        private Func<T, object> _orderBySelector;
        private bool _orderByDescending;
        private Func<IEnumerable<object>, object> _aggregate;
        private int? _take;
        private int? _skip;

        public QueryBuilder(IEnumerable<T> data)
        {
            _data = data;
        }

        public QueryBuilder<T> Where(Func<T, bool> predicate)
        {
            _predicate = predicate;
            return this;
        }

        public QueryBuilder<T> Select(Func<T, object> selector)
        {
            _selector = selector;
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

            if (_predicate != null)
                query = query.Where(_predicate);

            if (_orderBySelector != null)
                query = _orderByDescending
                    ? query.OrderByDescending(_orderBySelector)
                    : query.OrderBy(_orderBySelector);

            if (_skip.HasValue)
                query = query.Skip(_skip.Value);

            if (_take.HasValue)
                query = query.Take(_take.Value);

            var result = _selector != null ? query.Select(_selector).Cast<object>() : query.Cast<object>();

            return _aggregate != null ? new List<object> { _aggregate(result) } : result;
        }

        public string SaveToJson()
        {
            var queryDefinition = new QueryDefinition
            {
                Where = _predicate != null ? SerializePredicate(_predicate) : null,
                Select = _selector != null ? SerializeSelector(_selector) : null,
                OrderBy = _orderBySelector != null ? SerializeSelector(_orderBySelector) : null,
                OrderByDescending = _orderByDescending,
                Aggregate = _aggregate != null ? SerializeAggregate(_aggregate) : null,
                Take = _take,
                Skip = _skip
            };

            return System.Text.Json.JsonSerializer.Serialize(queryDefinition);
        }

        private string SerializePredicate(Func<T, bool> predicate)
        {
            if (predicate.Target != null)
            {
                return $"{predicate.Method.Name}|Target:{predicate.Target.GetType().Name}";
            }
            return predicate.Method.Name;
        }

        private string SerializeSelector(Func<T, object> selector)
        {
            if (selector.Target != null)
            {
                return $"{selector.Method.Name}|Target:{selector.Target.GetType().Name}";
            }
            return selector.Method.Name;
        }

        private string SerializeAggregate(Func<IEnumerable<object>, object> aggregate)
        {
            if (aggregate.Target != null)
            {
                return $"{aggregate.Method.Name}|Target:{aggregate.Target.GetType().Name}";
            }
            return aggregate.Method.Name;
        }

        public IEnumerable<object> ExecuteQuery(string savedQuery)
        {
            var queryDefinition = System.Text.Json.JsonSerializer.Deserialize<QueryDefinition>(savedQuery);

            var query = _data.AsEnumerable();

            if (!string.IsNullOrEmpty(queryDefinition?.Where))
            {
                query = query.Where(BuildPredicate(queryDefinition.Where));
            }

            if (!string.IsNullOrEmpty(queryDefinition?.OrderBy))
            {
                var orderBySelector = BuildSelector(queryDefinition.OrderBy);
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
                ? query.Select(BuildSelector(queryDefinition.Select)).Cast<object>()
                : query.Cast<object>();

            if (!string.IsNullOrEmpty(queryDefinition?.Aggregate))
            {
                var aggregateFunction = BuildAggregate(queryDefinition.Aggregate);
                return new List<object> { aggregateFunction(result) };
            }

            return result;
        }

        private Func<T, bool> BuildPredicate(string whereClause)
        {
            if (string.IsNullOrWhiteSpace(whereClause))
                throw new ArgumentException("Where clause cannot be null or empty.", nameof(whereClause));

            var parts = whereClause.Split('|'); // Example: "StartsWith|Value"
            var method = parts[0];
            var argument = parts.Length > 1 ? parts[1] : null;

            return method switch
            {
                "StartsWith" => item => item?.ToString()?.StartsWith(argument??"")??false,
                "EndsWith" => item => item?.ToString()?.EndsWith(argument ?? "") ?? false,
                "Contains" => item => item?.ToString()?.Contains(argument ?? "") ?? false,
                "Equals" => item => item?.ToString()?.Equals(argument) ?? false,
                "LengthGreaterThan" => item => item?.ToString()?.Length > int.Parse(argument ?? ""),
                "LengthLessThan" => item => item?.ToString()?.Length < int.Parse(argument ?? ""),
                "IsNullOrEmpty" => item => string.IsNullOrEmpty(item?.ToString()),
                "IsNotNullOrEmpty" => item => !string.IsNullOrEmpty(item?.ToString()),
                _ => throw new NotImplementedException($"Predicate method '{method}' is not implemented.")
            };
        }

        private Func<T, object> BuildSelector(string selectClause)
        {
            if (string.IsNullOrWhiteSpace(selectClause))
                throw new ArgumentException("Select clause cannot be null or empty.", nameof(selectClause));

            var parts = selectClause.Split('|'); // Example: "Substring|0|3"
            var method = parts[0];
            var arguments = parts.Skip(1).ToArray();

            return method switch
            {
                "ToUpper" => item => item?.ToString().ToUpper(),
                "ToLower" => item => item?.ToString().ToLower(),
                "Length" => item => item?.ToString().Length,
                "Substring" => item => item?.ToString().Substring(
                    int.Parse(arguments[0]),
                    arguments.Length > 1 ? int.Parse(arguments[1]) : item?.ToString().Length ?? 0),
                "ReverseString" => item => new string(item?.ToString().Reverse().ToArray()),
                _ => throw new NotImplementedException($"Selector method '{method}' is not implemented.")
            };
        }

        private Func<IEnumerable<object>, object> BuildAggregate(string aggregateClause)
        {
            if (string.IsNullOrWhiteSpace(aggregateClause))
                throw new ArgumentException("Aggregate clause cannot be null or empty.", nameof(aggregateClause));

            return aggregateClause switch
            {
                "Count" => query => query.Count(),
                "Sum" => query => query.Sum(x => Convert.ToDouble(x)),
                "Average" => query => query.Average(x => Convert.ToDouble(x)),
                "Min" => query => query.Min(),
                "Max" => query => query.Max(),
                _ => throw new NotImplementedException($"Aggregate method '{aggregateClause}' is not implemented.")
            };
        }

    }


}

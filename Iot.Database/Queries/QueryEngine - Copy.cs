//using Iot.Database.Base;
//using System.Linq.Expressions;
//using System.Reflection;
//using System.Text.RegularExpressions;

//namespace Iot.Database
//{
//    public class QueryEngine
//    {
//        private readonly IotDatabase _iotDatabase;
//        public event EventHandler<ExceptionEventArgs>? ExceptionOccurred;

//        public QueryEngine(IotDatabase iotDatabase)
//        {
//            _iotDatabase = iotDatabase ?? throw new ArgumentNullException(nameof(iotDatabase));
//            _iotDatabase.ExceptionOccurred += (sender, args) => OnExceptionOccurred(args);
//        }

//        /// <summary>
//        /// Executes a natural language-like query string to retrieve data from the database.
//        /// </summary>
//        /// <param name="query">Query string in the format: FIND <table> [WHERE <condition>] [SELECT <columns>] [INCLUDE <related_table> WHERE <related_condition> [SELECT <related_columns>]] [ORDER BY <column> [ASC|DESC]] [LIMIT <n>]</param>
//        /// <returns>A list of QueryResult containing the query results.</returns>
//        public List<QueryResult> NaturalQuery(string query)
//        {
//            try
//            {
//                // Parse the query string
//                var parsedQuery = ParseNaturalQuery(query);

//                // Get the primary table type
//                Type primaryType = GetTableType(parsedQuery.TableName);

//                // Find the generic Find method
//                var findMethodInfo = typeof(QueryEngine)
//                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
//                    .FirstOrDefault(m => m.Name == nameof(Find) && m.IsGenericMethod &&
//                        m.GetParameters().Length == 3 &&
//                        m.GetParameters()[0].ParameterType == typeof(string) &&
//                        m.GetParameters()[2].ParameterType == typeof(string[]));

//                if (findMethodInfo == null)
//                    throw new InvalidOperationException("Find method not found on QueryEngine.");

//                var genericFindMethod = findMethodInfo.MakeGenericMethod(primaryType);

//                // Build the predicate for the primary table
//                var primaryPredicate = BuildPredicate(primaryType, parsedQuery.Condition);
//                var primaryColumns = parsedQuery.Columns.ToArray();

//                // Invoke Find to start the query
//                var queryBuilder = genericFindMethod.Invoke(this, new object[] { parsedQuery.TableName, primaryPredicate, primaryColumns })
//                    as dynamic; // Use dynamic to handle generic QueryBuilder<T>

//                // Handle INCLUDE clauses
//                foreach (var include in parsedQuery.Includes)
//                {
//                    Type relatedType = GetTableType(include.TableName);
//                    var includeMethodInfo = typeof(QueryBuilder<>)
//                        .MakeGenericType(primaryType)
//                        .GetMethods(BindingFlags.Public | BindingFlags.Instance)
//                        .FirstOrDefault(m => m.Name == nameof(QueryBuilder<object>.Include) && m.IsGenericMethod &&
//                            m.GetParameters().Length == 3 &&
//                            m.GetParameters()[0].ParameterType == typeof(string) &&
//                            m.GetParameters()[2].ParameterType == typeof(string[]));

//                    if (includeMethodInfo == null)
//                        throw new InvalidOperationException("Include method not found on QueryBuilder.");

//                    var genericIncludeMethod = includeMethodInfo.MakeGenericMethod(relatedType);

//                    var relatedPredicate = BuildPredicate(relatedType, include.Condition);
//                    var relatedColumns = include.Columns.ToArray();

//                    queryBuilder = genericIncludeMethod.Invoke(queryBuilder, new object[] { include.TableName, relatedPredicate, relatedColumns });
//                }

//                // Execute the query with sorting and limiting
//                return ExecuteQuery(queryBuilder, parsedQuery.OrderBy, parsedQuery.Limit);
//            }
//            catch (Exception ex)
//            {
//                OnExceptionOccurred(new ExceptionEventArgs(ex));
//                return new List<QueryResult>();
//            }
//        }

//        private (string TableName, string Condition, List<string> Columns, List<(string TableName, string Condition, List<string> Columns)> Includes, (string Field, bool IsAscending)? OrderBy, int? Limit) ParseNaturalQuery(string query)
//        {
//            query = query.Trim();
//            var includes = new List<(string TableName, string Condition, List<string> Columns)>();
//            (string Field, bool IsAscending)? orderBy = null;
//            int? limit = null;

//            // Split into FIND and other parts (INCLUDE, ORDER BY, LIMIT)
//            var parts = Regex.Split(query, @"\s+(INCLUDE|ORDER BY|LIMIT)\s+", RegexOptions.IgnoreCase);
//            var findPart = parts[0].Trim();

//            // Parse FIND part
//            var findMatch = Regex.Match(findPart, @"FIND\s+(\w+)(?:\s+WHERE\s+(.+?))?(?:\s+SELECT\s+(.+))?$", RegexOptions.IgnoreCase);
//            if (!findMatch.Success)
//                throw new ArgumentException("Invalid query format. Expected: FIND <table> [WHERE <condition>] [SELECT <columns>]");

//            string tableName = findMatch.Groups[1].Value;
//            string condition = findMatch.Groups[2].Success ? findMatch.Groups[2].Value.Trim() : "";
//            var columns = findMatch.Groups[3].Success ? findMatch.Groups[3].Value.Split(',').Select(c => c.Trim()).ToList() : new List<string>();

//            // Parse remaining parts
//            int i = 1;
//            while (i < parts.Length)
//            {
//                var keyword = parts[i].ToUpper();
//                var part = parts[i + 1].Trim();

//                if (keyword == "INCLUDE")
//                {
//                    var includeMatch = Regex.Match(part, @"(\w+)(?:\s+WHERE\s+(.+?))?(?:\s+SELECT\s+(.+))?$", RegexOptions.IgnoreCase);
//                    if (!includeMatch.Success)
//                        throw new ArgumentException($"Invalid INCLUDE format: {part}");

//                    string relatedTableName = includeMatch.Groups[1].Value;
//                    string relatedCondition = includeMatch.Groups[2].Success ? includeMatch.Groups[2].Value.Trim() : "";
//                    var relatedColumns = includeMatch.Groups[3].Success ? includeMatch.Groups[3].Value.Split(',').Select(c => c.Trim()).ToList() : new List<string>();

//                    includes.Add((relatedTableName, relatedCondition, relatedColumns));
//                    i += 2;
//                }
//                else if (keyword == "ORDER BY")
//                {
//                    var orderByMatch = Regex.Match(part, @"(\w+)\s*(ASC|DESC)?", RegexOptions.IgnoreCase);
//                    if (!orderByMatch.Success)
//                        throw new ArgumentException($"Invalid ORDER BY format: {part}");

//                    string field = orderByMatch.Groups[1].Value;
//                    bool isAscending = orderByMatch.Groups[2].Success ? orderByMatch.Groups[2].Value.ToUpper() != "DESC" : true;
//                    orderBy = (field, isAscending);
//                    i += 2;
//                }
//                else if (keyword == "LIMIT")
//                {
//                    if (!int.TryParse(part, out var limitValue))
//                        throw new ArgumentException($"Invalid LIMIT value: {part}");

//                    limit = limitValue;
//                    i += 2;
//                }
//                else
//                {
//                    i += 2; // Skip unknown keyword
//                }
//            }

//            return (tableName, condition, columns, includes, orderBy, limit);
//        }

//        private Type GetTableType(string tableName)
//        {
//            var table = _iotDatabase.GetTable(tableName);
//            if (table != null)
//                return table.TableInfo.Type;

//            throw new ArgumentException($"Unknown table name: {tableName}");
//        }

//        private LambdaExpression BuildPredicate(Type type, string condition)
//        {
//            if (string.IsNullOrEmpty(condition))
//                return Expression.Lambda(Expression.Constant(true), Expression.Parameter(type, "x"));

//            // Split conditions by AND or OR
//            var conditions = SplitConditions(condition);
//            var parameter = Expression.Parameter(type, "x");
//            Expression? finalExpression = null;

//            foreach (var (cond, connector) in conditions)
//            {
//                // Updated regex to include not startswith, not endswith, not contains
//                var match = Regex.Match(cond, @"(\w+)\s*(>=|<=|!=|=|>|<|contains|startswith|endswith|not startswith|not endswith|not contains|is null|is not null|is empty)(?:\s+([^\s]+))?", RegexOptions.IgnoreCase);
//                if (!match.Success)
//                    throw new ArgumentException($"Invalid condition format: {cond}");

//                string propertyName = match.Groups[1].Value;
//                string operatorStr = match.Groups[2].Value.ToLower();
//                string valueStr = match.Groups[3].Success ? match.Groups[3].Value.Trim('\'', '"') : null;

//                var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
//                    ?? throw new ArgumentException($"Property {propertyName} not found on type {type.Name}");

//                var propertyAccess = Expression.Property(parameter, property);
//                Expression? comparison = null;

//                if (property.PropertyType == typeof(string))
//                {
//                    // Handle operators that require a value
//                    if (operatorStr is "contains" or "startswith" or "endswith" or "not contains" or "not startswith" or "not endswith" or "=" or "!=")
//                    {
//                        if (string.IsNullOrEmpty(valueStr))
//                            throw new ArgumentException($"Operator {operatorStr} requires a value for condition: {cond}");

//                        var value = Expression.Constant(valueStr);
//                        var comparisonType = Expression.Constant(StringComparison.OrdinalIgnoreCase);

//                        if (operatorStr == "contains")
//                        {
//                            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string), typeof(StringComparison) });
//                            comparison = Expression.Call(propertyAccess, containsMethod, value, comparisonType);
//                        }
//                        else if (operatorStr == "not contains")
//                        {
//                            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string), typeof(StringComparison) });
//                            comparison = Expression.Not(Expression.Call(propertyAccess, containsMethod, value, comparisonType));
//                        }
//                        else if (operatorStr == "startswith")
//                        {
//                            var startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string), typeof(StringComparison) });
//                            comparison = Expression.Call(propertyAccess, startsWithMethod, value, comparisonType);
//                        }
//                        else if (operatorStr == "not startswith")
//                        {
//                            var startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string), typeof(StringComparison) });
//                            comparison = Expression.Not(Expression.Call(propertyAccess, startsWithMethod, value, comparisonType));
//                        }
//                        else if (operatorStr == "endswith")
//                        {
//                            var endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string), typeof(StringComparison) });
//                            comparison = Expression.Call(propertyAccess, endsWithMethod, value, comparisonType);
//                        }
//                        else if (operatorStr == "not endswith")
//                        {
//                            var endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string), typeof(StringComparison) });
//                            comparison = Expression.Not(Expression.Call(propertyAccess, endsWithMethod, value, comparisonType));
//                        }
//                        else if (operatorStr == "=")
//                        {
//                            var equalsMethod = typeof(string).GetMethod("Equals", new[] { typeof(string), typeof(string), typeof(StringComparison) });
//                            comparison = Expression.Call(null, equalsMethod, propertyAccess, value, comparisonType);
//                        }
//                        else if (operatorStr == "!=")
//                        {
//                            var equalsMethod = typeof(string).GetMethod("Equals", new[] { typeof(string), typeof(string), typeof(StringComparison) });
//                            comparison = Expression.Not(Expression.Call(null, equalsMethod, propertyAccess, value, comparisonType));
//                        }
//                    }
//                    // Handle operators that don't require a value
//                    else if (operatorStr == "is null")
//                    {
//                        comparison = Expression.Equal(propertyAccess, Expression.Constant(null));
//                    }
//                    else if (operatorStr == "is not null")
//                    {
//                        comparison = Expression.NotEqual(propertyAccess, Expression.Constant(null));
//                    }
//                    else if (operatorStr == "is empty")
//                    {
//                        var nullCheck = Expression.Equal(propertyAccess, Expression.Constant(null));
//                        var emptyCheck = Expression.Equal(propertyAccess, Expression.Constant(""));
//                        comparison = Expression.OrElse(nullCheck, emptyCheck);
//                    }
//                    else
//                    {
//                        throw new ArgumentException($"Operator {operatorStr} not supported for string properties");
//                    }
//                }
//                else if (property.PropertyType == typeof(int))
//                {
//                    if (operatorStr is "is null" or "is not null" or "is empty" or "contains" or "startswith" or "endswith" or "not contains" or "not startswith" or "not endswith")
//                        throw new ArgumentException($"Operator {operatorStr} not supported for integer properties");

//                    if (string.IsNullOrEmpty(valueStr))
//                        throw new ArgumentException($"Operator {operatorStr} requires a value for condition: {cond}");

//                    if (!int.TryParse(valueStr, out var valueInt))
//                        throw new ArgumentException($"Value {valueStr} is not a valid integer");

//                    var value = Expression.Constant(valueInt, typeof(int));
//                    comparison = operatorStr switch
//                    {
//                        "=" => Expression.Equal(propertyAccess, value),
//                        ">" => Expression.GreaterThan(propertyAccess, value),
//                        "<" => Expression.LessThan(propertyAccess, value),
//                        ">=" => Expression.GreaterThanOrEqual(propertyAccess, value),
//                        "<=" => Expression.LessThanOrEqual(propertyAccess, value),
//                        "!=" => Expression.NotEqual(propertyAccess, value),
//                        _ => throw new ArgumentException($"Operator {operatorStr} not supported for integer properties")
//                    };
//                }
//                else if (property.PropertyType == typeof(long))
//                {
//                    if (operatorStr is "is null" or "is not null" or "is empty" or "contains" or "startswith" or "endswith" or "not contains" or "not startswith" or "not endswith")
//                        throw new ArgumentException($"Operator {operatorStr} not supported for long integer properties");

//                    if (string.IsNullOrEmpty(valueStr))
//                        throw new ArgumentException($"Operator {operatorStr} requires a value for condition: {cond}");

//                    if (!long.TryParse(valueStr, out var valueLong))
//                        throw new ArgumentException($"Value {valueStr} is not a valid long integer");

//                    var value = Expression.Constant(valueLong, typeof(long));
//                    comparison = operatorStr switch
//                    {
//                        "=" => Expression.Equal(propertyAccess, value),
//                        ">" => Expression.GreaterThan(propertyAccess, value),
//                        "<" => Expression.LessThan(propertyAccess, value),
//                        ">=" => Expression.GreaterThanOrEqual(propertyAccess, value),
//                        "<=" => Expression.LessThanOrEqual(propertyAccess, value),
//                        "!=" => Expression.NotEqual(propertyAccess, value),
//                        _ => throw new ArgumentException($"Operator {operatorStr} not supported for long integer properties")
//                    };
//                }
//                else if (property.PropertyType == typeof(decimal))
//                {
//                    if (operatorStr is "is null" or "is not null" or "is empty" or "contains" or "startswith" or "endswith" or "not contains" or "not startswith" or "not endswith")
//                        throw new ArgumentException($"Operator {operatorStr} not supported for decimal properties");

//                    if (string.IsNullOrEmpty(valueStr))
//                        throw new ArgumentException($"Operator {operatorStr} requires a value for condition: {cond}");

//                    if (!decimal.TryParse(valueStr, out var valueDecimal))
//                        throw new ArgumentException($"Value {valueStr} is not a valid decimal");

//                    var value = Expression.Constant(valueDecimal, typeof(decimal));
//                    comparison = operatorStr switch
//                    {
//                        "=" => Expression.Equal(propertyAccess, value),
//                        ">" => Expression.GreaterThan(propertyAccess, value),
//                        "<" => Expression.LessThan(propertyAccess, value),
//                        ">=" => Expression.GreaterThanOrEqual(propertyAccess, value),
//                        "<=" => Expression.LessThanOrEqual(propertyAccess, value),
//                        "!=" => Expression.NotEqual(propertyAccess, value),
//                        _ => throw new ArgumentException($"Operator {operatorStr} not supported for decimal properties")
//                    };
//                }
//                else
//                {
//                    throw new ArgumentException($"Property type {property.PropertyType.Name} not supported for condition: {cond}");
//                }

//                finalExpression = finalExpression == null ? comparison :
//                    connector.ToUpper() == "AND" ? Expression.AndAlso(finalExpression, comparison) :
//                    Expression.OrElse(finalExpression, comparison);
//            }

//            var lambda = Expression.Lambda(finalExpression ?? Expression.Constant(true), parameter);
//            // Log the predicate for debugging
//            Console.WriteLine($"Generated Predicate: {lambda}");
//            return lambda;
//        }

//        private List<(string Condition, string Connector)> SplitConditions(string condition)
//        {
//            var conditions = new List<(string Condition, string Connector)>();
//            var currentCondition = "";
//            var currentConnector = "";
//            int parenCount = 0;
//            bool inQuotes = false;

//            for (int i = 0; i < condition.Length; i++)
//            {
//                char c = condition[i];

//                if (c == '\'' || c == '"')
//                {
//                    inQuotes = !inQuotes;
//                    currentCondition += c;
//                    continue;
//                }

//                if (inQuotes)
//                {
//                    currentCondition += c;
//                    continue;
//                }

//                if (c == '(')
//                {
//                    parenCount++;
//                    currentCondition += c;
//                }
//                else if (c == ')')
//                {
//                    parenCount--;
//                    currentCondition += c;
//                }
//                else if (parenCount == 0)
//                {
//                    if (condition.Substring(i).StartsWith(" AND ", StringComparison.OrdinalIgnoreCase))
//                    {
//                        conditions.Add((currentCondition.Trim(), currentConnector));
//                        currentConnector = "AND";
//                        i += 4; // Skip " AND "
//                        currentCondition = "";
//                        continue;
//                    }
//                    else if (condition.Substring(i).StartsWith(" OR ", StringComparison.OrdinalIgnoreCase))
//                    {
//                        conditions.Add((currentCondition.Trim(), currentConnector));
//                        currentConnector = "OR";
//                        i += 3; // Skip " OR "
//                        currentCondition = "";
//                        continue;
//                    }
//                }

//                currentCondition += c;
//            }

//            if (!string.IsNullOrEmpty(currentCondition))
//                conditions.Add((currentCondition.Trim(), currentConnector));

//            return conditions;
//        }

//        private List<QueryResult> ExecuteQuery(dynamic queryBuilder, (string Field, bool IsAscending)? orderBy, int? limit)
//        {
//            var results = (queryBuilder.Execute() as List<QueryResult>) ?? new List<QueryResult>();

//            if (orderBy.HasValue)
//            {
//                var field = orderBy.Value.Field;
//                var isAscending = orderBy.Value.IsAscending;

//                results = results.OrderBy(r =>
//                {
//                    var doc = r.Data[$"{r.TableName}_Data"] as BsonDocument;
//                    return doc?.ContainsKey(field) == true ? doc[field] : BsonValue.Null;
//                }, isAscending ? Comparer<BsonValue>.Default : Comparer<BsonValue>.Create((a, b) => Comparer<BsonValue>.Default.Compare(b, a))).ToList();
//            }

//            if (limit.HasValue)
//            {
//                results = results.Take(limit.Value).ToList();
//            }

//            return results;
//        }

//        public QueryBuilder<T> Find<T>(string tableName, Expression<Func<T, bool>> predicate, params string[] columns) where T : class
//        {
//            return new QueryBuilder<T>(_iotDatabase, tableName, predicate, columns, this);
//        }

//        public List<QueryResult> SearchAllTables(string columnName, string value, Comparison comparisonType = Comparison.Equals)
//        {
//            var results = new List<QueryResult>();
//            try
//            {
//                foreach (var table in _iotDatabase.Tables())
//                {
//                    var tableResults = SearchTable(table, columnName, value, comparisonType);
//                    results.AddRange(tableResults);
//                }
//            }
//            catch (Exception ex)
//            {
//                OnExceptionOccurred(new ExceptionEventArgs(ex));
//            }
//            return results;
//        }

//        private List<QueryResult> SearchTable(ITableCollection table, string columnName, string value, Comparison comparisonType)
//        {
//            var results = new List<QueryResult>();
//            try
//            {
//                var tableResults = table.Find(columnName, value, comparisonType);
//                foreach (var result in tableResults)
//                {
//                    results.Add(new QueryResult
//                    {
//                        TableName = table.TableInfo.Name,
//                        Data = result
//                    });
//                }
//            }
//            catch (Exception ex)
//            {
//                OnExceptionOccurred(new ExceptionEventArgs(ex));
//            }
//            return results;
//        }

//        public List<QueryResult> Join<T1, T2>(string primaryTableName, string relatedTableName, Expression<Func<T1, T2, bool>>? predicate = null)
//            where T1 : class
//            where T2 : class
//        {
//            var results = new List<QueryResult>();
//            try
//            {
//                var primaryTable = _iotDatabase.Tables<T1>(primaryTableName);
//                var relatedTable = _iotDatabase.Tables<T2>(relatedTableName);
//                var relatedTableInfo = relatedTable.TableInfo;

//                var foreignKey = relatedTableInfo.ForeignKeys.FirstOrDefault(fk =>
//                    fk.ForeignKeyAttribute?.TableName == primaryTableName);

//                if (foreignKey == null)
//                {
//                    throw new InvalidOperationException($"No foreign key relationship found from {relatedTableName} to {primaryTableName}.");
//                }

//                var fkName = foreignKey.Name;
//                var primaryRecords = primaryTable.FindAll();
//                foreach (var primaryRecord in primaryRecords)
//                {
//                    var primaryId = primaryTable.TableInfo.Id?.PropertyInfo.GetValue(primaryRecord);
//                    if (primaryId != null)
//                    {
//                        var relatedRecords = relatedTable.Find(BuildForeignKeyEqualsExpression<T2>(fkName, primaryId));
//                        foreach (var relatedRecord in relatedRecords)
//                        {
//                            var primaryDoc = primaryRecord;
//                            var relatedDoc = relatedRecord;

//                            if (predicate == null || predicate.Compile()(primaryDoc, relatedDoc))
//                            {
//                                var resultDoc = new BsonDocument
//                                {
//                                    { $"{primaryTableName}_Data", BsonMapper.Global.ToDocument(primaryDoc) },
//                                    { $"{relatedTableName}_Data", BsonMapper.Global.ToDocument(relatedDoc) }
//                                };

//                                results.Add(new QueryResult
//                                {
//                                    TableName = $"{primaryTableName}_{relatedTableName}_Join",
//                                    Data = resultDoc
//                                });
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                OnExceptionOccurred(new ExceptionEventArgs(ex));
//            }
//            return results;
//        }

//        private Expression<Func<T, bool>> BuildForeignKeyEqualsExpression<T>(string propertyName, object fkValue)
//        {
//            var parameter = Expression.Parameter(typeof(T), "x");
//            var property = typeof(T).GetProperty(propertyName);
//            if (property == null)
//            {
//                throw new InvalidOperationException($"Property {propertyName} not found on type {typeof(T).Name}.");
//            }

//            var propertyAccess = Expression.Property(parameter, property);
//            var constant = Expression.Constant(fkValue);
//            var equals = Expression.Equal(propertyAccess, constant);
//            return Expression.Lambda<Func<T, bool>>(equals, parameter);
//        }

//        public List<QueryResult> Query<T>(string tableName, Expression<Func<T, bool>> predicate) where T : class
//        {
//            var results = new List<QueryResult>();
//            try
//            {
//                var table = _iotDatabase.Tables<T>(tableName);
//                var records = table.Find(predicate);
//                foreach (var record in records)
//                {
//                    var bsonDoc = BsonMapper.Global.ToDocument(record);
//                    results.Add(new QueryResult
//                    {
//                        TableName = tableName,
//                        Data = bsonDoc
//                    });
//                }
//            }
//            catch (Exception ex)
//            {
//                OnExceptionOccurred(new ExceptionEventArgs(ex));
//            }
//            return results;
//        }

//        public List<QueryResult> GetAll(string tableName, int take = 1000, TakeOrder takeOrder = TakeOrder.Last)
//        {
//            var results = new List<QueryResult>();
//            try
//            {
//                var table = _iotDatabase.GetTable(tableName);
//                if (table == null)
//                {
//                    throw new InvalidOperationException($"Table {tableName} not found.");
//                }
//                var records = table.FindAll(take, takeOrder);
//                foreach (var result in records)
//                {
//                    results.Add(new QueryResult
//                    {
//                        TableName = tableName,
//                        Data = result
//                    });
//                }
//            }
//            catch (Exception ex)
//            {
//                OnExceptionOccurred(new ExceptionEventArgs(ex));
//            }
//            return results;
//        }

//        public class QueryResult
//        {
//            public string TableName { get; set; } = string.Empty;
//            public BsonDocument Data { get; set; } = new BsonDocument();
//        }

//        protected virtual void OnExceptionOccurred(ExceptionEventArgs e)
//        {
//            ExceptionOccurred?.Invoke(this, e);
//        }

//        public class QueryBuilder<T> where T : class
//        {
//            private readonly IotDatabase _iotDatabase;
//            private readonly string _tableName;
//            private readonly Expression<Func<T, bool>> _predicate;
//            private readonly string[] _columns;
//            private readonly QueryEngine _queryEngine;
//            private readonly List<(string RelatedTableName, Type RelatedType, LambdaExpression Predicate, string[] Columns)> _includes;

//            public QueryBuilder(IotDatabase iotDatabase, string tableName, Expression<Func<T, bool>> predicate, string[] columns, QueryEngine queryEngine)
//            {
//                _iotDatabase = iotDatabase;
//                _tableName = tableName;
//                _predicate = predicate;
//                _columns = columns ?? Array.Empty<string>();
//                _queryEngine = queryEngine;
//                _includes = new List<(string, Type, LambdaExpression, string[])>();
//            }

//            public QueryBuilder<T> Include<TRelated>(string relatedTableName, Expression<Func<TRelated, bool>> relatedPredicate, params string[] columns) where TRelated : class
//            {
//                _includes.Add((relatedTableName, typeof(TRelated), relatedPredicate, columns ?? Array.Empty<string>()));
//                return this;
//            }

//            public List<QueryResult> Execute()
//            {
//                var results = new List<QueryResult>();
//                try
//                {
//                    var primaryTable = _iotDatabase.Tables<T>(_tableName);
//                    // Use FindAll and apply predicate in memory to bypass potential ITableCollection.Find bug
//                    var primaryRecords = primaryTable.FindAll();
//                    var predicateFunc = _predicate.Compile();
//                    primaryRecords = primaryRecords.Where(predicateFunc).ToList();

//                    foreach (var primaryRecord in primaryRecords)
//                    {
//                        var primaryDoc = BsonMapper.Global.ToDocument(primaryRecord);
//                        var filteredPrimaryDoc = FilterColumns(primaryDoc, _columns, mapId: true);

//                        var resultDoc = new BsonDocument
//                        {
//                            { $"{_tableName}_Data", filteredPrimaryDoc }
//                        };

//                        foreach (var include in _includes)
//                        {
//                            var relatedTableName = include.RelatedTableName;
//                            var relatedType = include.RelatedType;
//                            var relatedPredicate = include.Predicate;
//                            var relatedColumns = include.Columns;

//                            var relatedTable = GetTable(relatedType, relatedTableName);
//                            var relatedTableInfo = relatedTable.TableInfo;

//                            var foreignKey = relatedTableInfo.ForeignKeys.FirstOrDefault(fk =>
//                                fk.ForeignKeyAttribute?.TableName == _tableName);

//                            if (foreignKey == null)
//                            {
//                                throw new InvalidOperationException($"No foreign key relationship found from {relatedTableName} to {_tableName}.");
//                            }

//                            var fkName = foreignKey.Name;
//                            var primaryId = primaryTable.TableInfo.Id?.PropertyInfo.GetValue(primaryRecord);
//                            if (primaryId != null)
//                            {
//                                var predicateType = typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(relatedType, typeof(bool)));
//                                var findMethod = typeof(ITableCollection<>)
//                                    .MakeGenericType(relatedType)
//                                    .GetMethod(nameof(ITableCollection<object>.Find), new[] { predicateType, typeof(int), typeof(int) });

//                                if (findMethod == null)
//                                {
//                                    throw new InvalidOperationException($"Find method not found on ITableCollection<{relatedType.Name}>.");
//                                }

//                                var fkPredicate = BuildForeignKeyEqualsExpression(relatedType, fkName, primaryId);
//                                var combinedPredicate = CombinePredicates(relatedType, fkPredicate, relatedPredicate);

//                                var relatedRecords = findMethod.Invoke(relatedTable, new object[] { combinedPredicate, 0, int.MaxValue }) as IEnumerable<object>;
//                                if (relatedRecords != null)
//                                {
//                                    var relatedDocs = relatedRecords.Select(r =>
//                                    {
//                                        var doc = BsonMapper.Global.ToDocument(r);
//                                        return FilterColumns(doc, relatedColumns, mapId: true);
//                                    }).ToList();
//                                    resultDoc[($"{relatedTableName}_Data")] = new BsonArray(relatedDocs);
//                                }
//                            }
//                        }

//                        results.Add(new QueryResult
//                        {
//                            TableName = _tableName,
//                            Data = resultDoc
//                        });
//                    }
//                }
//                catch (Exception ex)
//                {
//                    _queryEngine.OnExceptionOccurred(new ExceptionEventArgs(ex));
//                }
//                return results;
//            }

//            private BsonDocument FilterColumns(BsonDocument doc, string[] columns, bool mapId)
//            {
//                if (columns.Length == 0)
//                {
//                    // Return all columns if none are specified
//                    if (mapId && doc.ContainsKey("_id"))
//                    {
//                        doc["Id"] = doc["_id"];
//                        doc.Remove("_id");
//                    }
//                    return doc;
//                }

//                var filteredDoc = new BsonDocument();
//                foreach (var column in columns)
//                {
//                    var key = column;
//                    if (mapId && column.Equals("Id", StringComparison.OrdinalIgnoreCase))
//                    {
//                        key = "_id";
//                    }

//                    if (doc.ContainsKey(key))
//                    {
//                        filteredDoc[column] = doc[key];
//                    }
//                }

//                if (mapId && filteredDoc.ContainsKey("_id"))
//                {
//                    filteredDoc["Id"] = filteredDoc["_id"];
//                    doc.Remove("_id");
//                }

//                return filteredDoc;
//            }

//            private LambdaExpression BuildForeignKeyEqualsExpression(Type relatedType, string propertyName, object fkValue)
//            {
//                var parameter = Expression.Parameter(relatedType, "x");
//                var property = relatedType.GetProperty(propertyName);
//                if (property == null)
//                {
//                    throw new InvalidOperationException($"Property {propertyName} not found on type {relatedType.Name}.");
//                }

//                var propertyAccess = Expression.Property(parameter, property);
//                var constant = Expression.Constant(fkValue);
//                var equals = Expression.Equal(propertyAccess, constant);
//                return Expression.Lambda(equals, parameter);
//            }

//            private LambdaExpression CombinePredicates(Type relatedType, LambdaExpression fkPredicate, LambdaExpression userPredicate)
//            {
//                var parameter = Expression.Parameter(relatedType, "x");
//                var fkBody = Expression.Invoke(fkPredicate, parameter);
//                var userBody = Expression.Invoke(userPredicate, parameter);
//                var combinedBody = Expression.AndAlso(fkBody, userBody);
//                return Expression.Lambda(combinedBody, parameter);
//            }

//            private ITableCollection GetTable(Type type, string tableName)
//            {
//                var method = typeof(IotDatabase).GetMethod(nameof(IotDatabase.Tables), new[] { typeof(string) });
//                var genericMethod = method?.MakeGenericMethod(type);
//                return genericMethod?.Invoke(_iotDatabase, new object[] { tableName }) as ITableCollection
//                    ?? throw new InvalidOperationException($"Failed to get table {tableName} for type {type.Name}.");
//            }
//        }
//    }
//}
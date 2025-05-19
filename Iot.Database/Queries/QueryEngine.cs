using Iot.Database.Base;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Iot.Database
{
    // QueryEngine handles natural language-like queries for IoT database operations.
    public class QueryEngine
    {
        private readonly IotDatabase _iotDatabase;
        // Event to notify subscribers of exceptions during query execution.
        public event EventHandler<ExceptionEventArgs>? ExceptionOccurred;

        // Constructor initializes the QueryEngine with an IotDatabase instance.
        public QueryEngine(IotDatabase iotDatabase)
        {
            // Ensure the database instance is not null to prevent runtime errors.
            _iotDatabase = iotDatabase ?? throw new ArgumentNullException(nameof(iotDatabase));
            // Subscribe to database exceptions to propagate them through the QueryEngine.
            _iotDatabase.ExceptionOccurred += (sender, args) => OnExceptionOccurred(args);
        }

        /// <summary>
        /// Executes a natural language-like query string to retrieve data from the database.
        /// </summary>
        /// <param name="query">Query string in the format: FIND <table> [WHERE <condition>] [SELECT <columns>] [INCLUDE <related_table> WHERE <related_condition> [SELECT <related_columns>]] [ORDER BY <column> [ASC|DESC]] [LIMIT <n>]</param>
        /// <returns>A list of QueryResult containing the query results.</returns>
        public List<QueryResult> NaturalQuery(string query)
        {
            try
            {
                // Parse the query string into its components (table, conditions, columns, etc.).
                var parsedQuery = ParseNaturalQuery(query);

                // Resolve the primary table's type based on its name.
                Type primaryType = GetTableType(parsedQuery.TableName);

                // Locate the generic Find method for building the query.
                var findMethodInfo = typeof(QueryEngine)
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(m => m.Name == nameof(Find) && m.IsGenericMethod &&
                        m.GetParameters().Length == 3 &&
                        m.GetParameters()[0].ParameterType == typeof(string) &&
                        m.GetParameters()[2].ParameterType == typeof(string[]));

                // Ensure the Find method is found, or throw an exception.
                if (findMethodInfo == null)
                    throw new InvalidOperationException("Find method not found on QueryEngine.");

                // Create a generic version of the Find method for the primary table type.
                var genericFindMethod = findMethodInfo.MakeGenericMethod(primaryType);

                // Build the predicate expression for filtering the primary table.
                var primaryPredicate = BuildPredicate(primaryType, parsedQuery.Condition);
                var primaryColumns = parsedQuery.Columns.Select(c => $"{c.ColumnName}{(c.Alias != c.ColumnName ? $" as {c.Alias}" : "")}").ToArray();

                // Invoke the Find method to initialize the query builder.
                var queryBuilder = genericFindMethod.Invoke(this, new object[] { parsedQuery.TableName, primaryPredicate, primaryColumns })
                    as dynamic; // Use dynamic to handle generic QueryBuilder<T>

                // Process INCLUDE clauses to join related tables.
                foreach (var include in parsedQuery.Includes)
                {
                    // Resolve the related table's type.
                    Type relatedType = GetTableType(include.TableName);
                    // Locate the generic Include method on QueryBuilder.
                    var includeMethodInfo = typeof(QueryBuilder<>)
                        .MakeGenericType(primaryType)
                        .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .FirstOrDefault(m => m.Name == nameof(QueryBuilder<object>.Include) && m.IsGenericMethod &&
                            m.GetParameters().Length == 3 &&
                            m.GetParameters()[0].ParameterType == typeof(string) &&
                            m.GetParameters()[2].ParameterType == typeof(string[]));

                    // Ensure the Include method is found, or throw an exception.
                    if (includeMethodInfo == null)
                        throw new InvalidOperationException("Include method not found on QueryBuilder.");

                    // Create a generic version of the Include method for the related table type.
                    var genericIncludeMethod = includeMethodInfo.MakeGenericMethod(relatedType);

                    // Build the predicate for the related table.
                    var relatedPredicate = BuildPredicate(relatedType, include.Condition);
                    var relatedColumns = include.Columns.Select(c => $"{c.ColumnName}{(c.Alias != c.ColumnName ? $" as {c.Alias}" : "")}").ToArray();

                    // Invoke the Include method to add the related table to the query.
                    queryBuilder = genericIncludeMethod.Invoke(queryBuilder, new object[] { include.TableName, relatedPredicate, relatedColumns });
                }

                // Execute the query, applying sorting and limiting as specified.
                var results = ExecuteQuery(queryBuilder, parsedQuery.OrderBy, parsedQuery.Limit);

                // If a join command is present, process it with ParseExecuteCommand.
                if (!string.IsNullOrEmpty(parsedQuery.JoinCommand))
                {
                    return queryBuilder.ParseExecuteCommand(results, parsedQuery.JoinCommand);
                }

                return results;
            }
            catch (Exception ex)
            {
                // Handle exceptions by notifying subscribers and returning an empty result set.
                OnExceptionOccurred(new ExceptionEventArgs(ex));
                return new List<QueryResult>();
            }
        }

        // Parses a natural language query into its components, supporting column aliases and JOIN clause.
        private (string TableName, string Condition, List<(string ColumnName, string Alias)> Columns, List<(string TableName, string Condition, List<(string ColumnName, string Alias)> Columns)> Includes, (string Field, bool IsAscending)? OrderBy, int? Limit, string JoinCommand) ParseNaturalQuery(string query)
        {
            // Trim whitespace from the query for consistent parsing.
            query = query.Trim();
            var includes = new List<(string TableName, string Condition, List<(string ColumnName, string Alias)> Columns)>();
            (string Field, bool IsAscending)? orderBy = null;
            int? limit = null;
            string joinCommand = "";

            // Split the query into parts based on INCLUDE, JOIN, ORDER BY, and LIMIT clauses.
            var parts = Regex.Split(query, @"\s+(INCLUDE|JOIN|ORDER BY|LIMIT)\s+", RegexOptions.IgnoreCase);
            var findPart = parts[0].Trim();

            // Parse the FIND clause to extract table name, condition, and columns.
            var findMatch = Regex.Match(findPart, @"FIND\s+(\w+)(?:\s+WHERE\s+(.+?))?(?:\s+SELECT\s+(.+))?$", RegexOptions.IgnoreCase);
            if (!findMatch.Success)
                throw new ArgumentException("Invalid query format. Expected: FIND <table> [WHERE <condition>] [SELECT <columns>]");

            string tableName = findMatch.Groups[1].Value;
            string condition = findMatch.Groups[2].Success ? findMatch.Groups[2].Value.Trim() : "";
            // If SELECT clause is absent or empty, return an empty column list to include all columns.
            var columns = findMatch.Groups[3].Success && !string.IsNullOrWhiteSpace(findMatch.Groups[3].Value)
                ? QueryUtils.ParseColumns(findMatch.Groups[3].Value)
                : new List<(string ColumnName, string Alias)>();

            // Process remaining parts (INCLUDE, JOIN, ORDER BY, LIMIT).
            int i = 1;
            while (i < parts.Length)
            {
                var keyword = parts[i].ToUpper();
                var part = parts[i + 1].Trim();

                if (keyword == "INCLUDE")
                {
                    // Parse INCLUDE clause for related table details.
                    var includeMatch = Regex.Match(part, @"(\w+)(?:\s+WHERE\s+(.+?))?(?:\s+SELECT\s+(.+))?$", RegexOptions.IgnoreCase);
                    if (!includeMatch.Success)
                        throw new ArgumentException($"Invalid INCLUDE format: {part}");

                    string relatedTableName = includeMatch.Groups[1].Value;
                    string relatedCondition = includeMatch.Groups[2].Success ? includeMatch.Groups[2].Value.Trim() : "";
                    // If SELECT clause is absent or empty, return an empty column list to include all columns.
                    var relatedColumns = includeMatch.Groups[3].Success && !string.IsNullOrWhiteSpace(includeMatch.Groups[3].Value)
                        ? QueryUtils.ParseColumns(includeMatch.Groups[3].Value)
                        : new List<(string ColumnName, string Alias)>();

                    includes.Add((relatedTableName, relatedCondition, relatedColumns));
                    i += 2;
                }
                else if (keyword == "JOIN")
                {
                    // Parse INNERJOIN clause, capturing multi-word table names and select columns.
                    var joinMatch = Regex.Match(part, @"^(?:as\s+(.+?)\s+select\s+(.+)$)", RegexOptions.IgnoreCase);
                    if (!joinMatch.Success)
                        throw new ArgumentException($"Invalid JOIN format: {part}. Expected: JOIN [as <tableName>] select <columns>");

                    // Construct the join command, including optional 'as' clause.
                    joinCommand = joinMatch.Groups[1].Success
                        ? $"Join as {joinMatch.Groups[1].Value} Select {joinMatch.Groups[2].Value}"
                        : $"Join Select {joinMatch.Groups[2].Value}";
                    i += 2;
                }
                else if (keyword == "ORDER BY")
                {
                    // Parse ORDER BY clause for sorting field and direction.
                    var orderByMatch = Regex.Match(part, @"(\w+)\s*(ASC|DESC)?", RegexOptions.IgnoreCase);
                    if (!orderByMatch.Success)
                        throw new ArgumentException($"Invalid ORDER BY format: {part}");

                    string field = orderByMatch.Groups[1].Value;
                    bool isAscending = orderByMatch.Groups[2].Success ? orderByMatch.Groups[2].Value.ToUpper() != "DESC" : true;
                    orderBy = (field, isAscending);
                    i += 2;
                }
                else if (keyword == "LIMIT")
                {
                    // Parse LIMIT clause for result set size.
                    if (!int.TryParse(part, out var limitValue))
                        throw new ArgumentException($"Invalid LIMIT value: {part}");

                    limit = limitValue;
                    i += 2;
                }
                else
                {
                    i += 2; // Skip unknown keyword
                }
            }

            return (tableName, condition, columns, includes, orderBy, limit, joinCommand);
        }

        // Resolves the Type of a table based on its name.
        private Type GetTableType(string tableName)
        {
            var table = _iotDatabase.GetTable(tableName);
            if (table != null)
                return table.TableInfo.Type;

            throw new ArgumentException($"Unknown table name: {tableName}");
        }

        // Builds a predicate expression for filtering data based on a condition string.
        private LambdaExpression BuildPredicate(Type type, string condition)
        {
            // Return a true predicate if no condition is provided.
            if (string.IsNullOrEmpty(condition))
                return Expression.Lambda(Expression.Constant(true), Expression.Parameter(type, "x"));

            // Split conditions by AND or OR for processing.
            var conditions = SplitConditions(condition);
            var parameter = Expression.Parameter(type, "x");
            Expression? finalExpression = null;

            foreach (var (cond, connector) in conditions)
            {
                // Parse condition using regex to extract property, operator, and value.
                var match = Regex.Match(cond, @"(\w+)\s*(>=|<=|!=|=|>|<|contains|startswith|endswith|not startswith|not endswith|not contains|is null|is not null|is empty)(?:\s+([^\s]+))?", RegexOptions.IgnoreCase);
                if (!match.Success)
                    throw new ArgumentException($"Invalid condition format: {cond}");

                string propertyName = match.Groups[1].Value;
                string operatorStr = match.Groups[2].Value.ToLower();
                string valueStr = match.Groups[3].Success ? match.Groups[3].Value.Trim('\'', '"') : null;

                // Resolve the property on the type.
                var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                    ?? throw new ArgumentException($"Property {propertyName} not found on type {type.Name}");

                var propertyAccess = Expression.Property(parameter, property);
                Expression? comparison = null;

                if (property.PropertyType == typeof(string))
                {
                    // Handle string-specific operators.
                    if (operatorStr is "contains" or "startswith" or "endswith" or "not contains" or "not startswith" or "not endswith" or "=" or "!=")
                    {
                        if (string.IsNullOrEmpty(valueStr))
                            throw new ArgumentException($"Operator {operatorStr} requires a value for condition: {cond}");

                        var value = Expression.Constant(valueStr);
                        var comparisonType = Expression.Constant(StringComparison.OrdinalIgnoreCase);

                        if (operatorStr == "contains")
                        {
                            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string), typeof(StringComparison) });
                            comparison = Expression.Call(propertyAccess, containsMethod, value, comparisonType);
                        }
                        else if (operatorStr == "not contains")
                        {
                            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string), typeof(StringComparison) });
                            comparison = Expression.Not(Expression.Call(propertyAccess, containsMethod, value, comparisonType));
                        }
                        else if (operatorStr == "startswith")
                        {
                            var startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string), typeof(StringComparison) });
                            comparison = Expression.Call(propertyAccess, startsWithMethod, value, comparisonType);
                        }
                        else if (operatorStr == "not startswith")
                        {
                            var startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string), typeof(StringComparison) });
                            comparison = Expression.Not(Expression.Call(propertyAccess, startsWithMethod, value, comparisonType));
                        }
                        else if (operatorStr == "endswith")
                        {
                            var endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string), typeof(StringComparison) });
                            comparison = Expression.Call(propertyAccess, endsWithMethod, value, comparisonType);
                        }
                        else if (operatorStr == "not endswith")
                        {
                            var endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string), typeof(StringComparison) });
                            comparison = Expression.Not(Expression.Call(propertyAccess, endsWithMethod, value, comparisonType));
                        }
                        else if (operatorStr == "=")
                        {
                            var equalsMethod = typeof(string).GetMethod("Equals", new[] { typeof(string), typeof(string), typeof(StringComparison) });
                            comparison = Expression.Call(null, equalsMethod, propertyAccess, value, comparisonType);
                        }
                        else if (operatorStr == "!=")
                        {
                            var equalsMethod = typeof(string).GetMethod("Equals", new[] { typeof(string), typeof(string), typeof(StringComparison) });
                            comparison = Expression.Not(Expression.Call(null, equalsMethod, propertyAccess, value, comparisonType));
                        }
                    }
                    // Handle operators that don't require a value.
                    else if (operatorStr == "is null")
                    {
                        comparison = Expression.Equal(propertyAccess, Expression.Constant(null));
                    }
                    else if (operatorStr == "is not null")
                    {
                        comparison = Expression.NotEqual(propertyAccess, Expression.Constant(null));
                    }
                    else if (operatorStr == "is empty")
                    {
                        var nullCheck = Expression.Equal(propertyAccess, Expression.Constant(null));
                        var emptyCheck = Expression.Equal(propertyAccess, Expression.Constant(""));
                        comparison = Expression.OrElse(nullCheck, emptyCheck);
                    }
                    else
                    {
                        throw new ArgumentException($"Operator {operatorStr} not supported for string properties");
                    }
                }
                else if (property.PropertyType == typeof(int))
                {
                    // Handle integer-specific operators.
                    if (operatorStr is "is null" or "is not null" or "is empty" or "contains" or "startswith" or "endswith" or "not contains" or "not startswith" or "not endswith")
                        throw new ArgumentException($"Operator {operatorStr} not supported for integer properties");

                    if (string.IsNullOrEmpty(valueStr))
                        throw new ArgumentException($"Operator {operatorStr} requires a value for condition: {cond}");

                    if (!int.TryParse(valueStr, out var valueInt))
                        throw new ArgumentException($"Value {valueStr} is not a valid integer");

                    var value = Expression.Constant(valueInt, typeof(int));
                    comparison = operatorStr switch
                    {
                        "=" => Expression.Equal(propertyAccess, value),
                        ">" => Expression.GreaterThan(propertyAccess, value),
                        "<" => Expression.LessThan(propertyAccess, value),
                        ">=" => Expression.GreaterThanOrEqual(propertyAccess, value),
                        "<=" => Expression.LessThanOrEqual(propertyAccess, value),
                        "!=" => Expression.NotEqual(propertyAccess, value),
                        _ => throw new ArgumentException($"Operator {operatorStr} not supported for integer properties")
                    };
                }
                else if (property.PropertyType == typeof(long))
                {
                    // Handle long integer-specific operators.
                    if (operatorStr is "is null" or "is not null" or "is empty" or "contains" or "startswith" or "endswith" or "not contains" or "not startswith" or "not endswith")
                        throw new ArgumentException($"Operator {operatorStr} not supported for long integer properties");

                    if (string.IsNullOrEmpty(valueStr))
                        throw new ArgumentException($"Operator {operatorStr} requires a value for condition: {cond}");

                    if (!long.TryParse(valueStr, out var valueLong))
                        throw new ArgumentException($"Value {valueStr} is not a valid long integer");

                    var value = Expression.Constant(valueLong, typeof(long));
                    comparison = operatorStr switch
                    {
                        "=" => Expression.Equal(propertyAccess, value),
                        ">" => Expression.GreaterThan(propertyAccess, value),
                        "<" => Expression.LessThan(propertyAccess, value),
                        ">=" => Expression.GreaterThanOrEqual(propertyAccess, value),
                        "<=" => Expression.LessThanOrEqual(propertyAccess, value),
                        "!=" => Expression.NotEqual(propertyAccess, value),
                        _ => throw new ArgumentException($"Operator {operatorStr} not supported for long integer properties")
                    };
                }
                else if (property.PropertyType == typeof(decimal))
                {
                    // Handle decimal-specific operators.
                    if (operatorStr is "is null" or "is not null" or "is empty" or "contains" or "startswith" or "endswith" or "not contains" or "not startswith" or "not endswith")
                        throw new ArgumentException($"Operator {operatorStr} not supported for decimal properties");

                    if (string.IsNullOrEmpty(valueStr))
                        throw new ArgumentException($"Operator {operatorStr} requires a value for condition: {cond}");

                    if (!decimal.TryParse(valueStr, out var valueDecimal))
                        throw new ArgumentException($"Value {valueStr} is not a valid decimal");

                    var value = Expression.Constant(valueDecimal, typeof(decimal));
                    comparison = operatorStr switch
                    {
                        "=" => Expression.Equal(propertyAccess, value),
                        ">" => Expression.GreaterThan(propertyAccess, value),
                        "<" => Expression.LessThan(propertyAccess, value),
                        ">=" => Expression.GreaterThanOrEqual(propertyAccess, value),
                        "<=" => Expression.LessThanOrEqual(propertyAccess, value),
                        "!=" => Expression.NotEqual(propertyAccess, value),
                        _ => throw new ArgumentException($"Operator {operatorStr} not supported for decimal properties")
                    };
                }
                else
                {
                    throw new ArgumentException($"Property type {property.PropertyType.Name} not supported for condition: {cond}");
                }

                // Combine conditions using AND or OR connectors.
                finalExpression = finalExpression == null ? comparison :
                    connector.ToUpper() == "AND" ? Expression.AndAlso(finalExpression, comparison) :
                    Expression.OrElse(finalExpression, comparison);
            }

            // Create and log the final lambda expression for debugging.
            var lambda = Expression.Lambda(finalExpression ?? Expression.Constant(true), parameter);
#if DEBUG
            Console.WriteLine($"Generated Predicate: {lambda}");
#endif
            return lambda;
        }

        // Splits a condition string into individual conditions and their connectors (AND/OR).
        private List<(string Condition, string Connector)> SplitConditions(string condition)
        {
            var conditions = new List<(string Condition, string Connector)>();
            var currentCondition = "";
            var currentConnector = "";
            int parenCount = 0;
            bool inQuotes = false;

            for (int i = 0; i < condition.Length; i++)
            {
                char c = condition[i];

                if (c == '\'' || c == '"')
                {
                    inQuotes = !inQuotes;
                    currentCondition += c;
                    continue;
                }

                if (inQuotes)
                {
                    currentCondition += c;
                    continue;
                }

                if (c == '(')
                {
                    parenCount++;
                    currentCondition += c;
                }
                else if (c == ')')
                {
                    parenCount--;
                    currentCondition += c;
                }
                else if (parenCount == 0)
                {
                    if (condition.Substring(i).StartsWith(" AND ", StringComparison.OrdinalIgnoreCase))
                    {
                        conditions.Add((currentCondition.Trim(), currentConnector));
                        currentConnector = "AND";
                        i += 4; // Skip " AND "
                        currentCondition = "";
                        continue;
                    }
                    else if (condition.Substring(i).StartsWith(" OR ", StringComparison.OrdinalIgnoreCase))
                    {
                        conditions.Add((currentCondition.Trim(), currentConnector));
                        currentConnector = "OR";
                        i += 3; // Skip " OR "
                        currentCondition = "";
                        continue;
                    }
                }

                currentCondition += c;
            }

            if (!string.IsNullOrEmpty(currentCondition))
                conditions.Add((currentCondition.Trim(), currentConnector));

            return conditions;
        }

        // Executes the query with sorting and limiting.
        private List<QueryResult> ExecuteQuery(dynamic queryBuilder, (string Field, bool IsAscending)? orderBy, int? limit)
        {
            // Execute the query and retrieve results.
            var results = (queryBuilder.Execute() as List<QueryResult>) ?? new List<QueryResult>();

            if (orderBy.HasValue)
            {
                var field = orderBy.Value.Field;
                var isAscending = orderBy.Value.IsAscending;

                // Sort results based on the specified field and direction.
                results = results.OrderBy(r =>
                {
                    var doc = r.Data[$"{r.TableName}_Data"] as BsonDocument;
                    return doc?.ContainsKey(field) == true ? doc[field] : BsonValue.Null;
                }, isAscending ? Comparer<BsonValue>.Default : Comparer<BsonValue>.Create((a, b) => Comparer<BsonValue>.Default.Compare(b, a))).ToList();
            }

            if (limit.HasValue)
            {
                // Limit the number of results returned.
                results = results.Take(limit.Value).ToList();
            }

            return results;
        }

        // Initializes a query builder for a specific table and predicate, supporting column aliases via string specifications.
        public QueryBuilder<T> Find<T>(string tableName, Expression<Func<T, bool>> predicate, params string[] columns) where T : class
        {
            // Parse column strings to extract column names and aliases, or use empty list if no columns specified.
            var parsedColumns = columns.Length == 0 || columns.All(string.IsNullOrWhiteSpace)
                ? new List<(string ColumnName, string Alias)>()
                : QueryUtils.ParseColumns(string.Join(",", columns));
            return new QueryBuilder<T>(_iotDatabase, tableName, predicate, parsedColumns.ToArray(), this);
        }


        // Represents a single query result with table name and data.
        public class QueryResult
        {
            public string TableName { get; set; } = string.Empty;
            public BsonDocument Data { get; set; } = new BsonDocument();
        }

        // Raises the ExceptionOccurred event.
        protected virtual void OnExceptionOccurred(ExceptionEventArgs e)
        {
            ExceptionOccurred?.Invoke(this, e);
        }

        // Builds and executes queries with support for includes and column filtering.
        public class QueryBuilder<T> where T : class
        {
            private readonly IotDatabase _iotDatabase;
            private readonly string _tableName;
            private readonly Expression<Func<T, bool>> _predicate;
            private readonly (string ColumnName, string Alias)[] _columns;
            private readonly QueryEngine _queryEngine;
            private readonly List<(string RelatedTableName, Type RelatedType, LambdaExpression Predicate, (string ColumnName, string Alias)[] Columns)> _includes;

            public QueryBuilder(IotDatabase iotDatabase, string tableName, Expression<Func<T, bool>> predicate, (string ColumnName, string Alias)[] columns, QueryEngine queryEngine)
            {
                _iotDatabase = iotDatabase;
                _tableName = tableName;
                _predicate = predicate;
                _columns = columns ?? Array.Empty<(string, string)>();
                _queryEngine = queryEngine;
                _includes = new List<(string, Type, LambdaExpression, (string, string)[])>();
            }

            // Adds a related table to the query with a predicate and columns, supporting aliases via string specifications.
            public QueryBuilder<T> Include<TRelated>(string relatedTableName, Expression<Func<TRelated, bool>> relatedPredicate, params string[] columns) where TRelated : class
            {
                // Parse column strings to extract column names and aliases, or use empty list if no columns specified.
                var parsedColumns = columns.Length == 0 || columns.All(string.IsNullOrWhiteSpace)
                    ? new List<(string ColumnName, string Alias)>()
                    : QueryUtils.ParseColumns(string.Join(",", columns));
                _includes.Add((relatedTableName, typeof(TRelated), relatedPredicate, parsedColumns.ToArray()));
                return this;
            }

            // Executes the query, applying predicates and includes.
            public List<QueryResult> Execute(string executeCommand = "")
            {
                var results = new List<QueryResult>();
                try
                {
                    var primaryTable = _iotDatabase.Tables<T>(_tableName);
                    // Fetch all records and apply the predicate in memory to avoid potential issues.
                    var primaryRecords = primaryTable.FindAll();
                    var predicateFunc = _predicate.Compile();
                    primaryRecords = primaryRecords.Where(predicateFunc).ToList();

                    foreach (var primaryRecord in primaryRecords)
                    {
                        var primaryDoc = BsonMapper.Global.ToDocument(primaryRecord);
                        var filteredPrimaryDoc = FilterColumns(primaryDoc, _columns, mapId: true);

                        var resultDoc = new BsonDocument
                        {
                            { $"{_tableName}_Data", filteredPrimaryDoc }
                        };

                        foreach (var include in _includes)
                        {
                            var relatedTableName = include.RelatedTableName;
                            var relatedType = include.RelatedType;
                            var relatedPredicate = include.Predicate;
                            var relatedColumns = include.Columns;

                            var relatedTable = GetTable(relatedType, relatedTableName);
                            var relatedTableInfo = relatedTable.TableInfo;

                            // Find the foreign key relationship.
                            var foreignKey = relatedTableInfo.ForeignKeys.FirstOrDefault(fk =>
                                fk.ForeignKeyAttribute?.TableName == _tableName);

                            if (foreignKey == null)
                            {
                                throw new InvalidOperationException($"No foreign key relationship found from {relatedTableName} to {_tableName}.");
                            }

                            var fkName = foreignKey.Name;
                            var primaryId = primaryTable.TableInfo.Id?.PropertyInfo.GetValue(primaryRecord);
                            if (primaryId != null)
                            {
                                var predicateType = typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(relatedType, typeof(bool)));
                                var findMethod = typeof(ITableCollection<>)
                                    .MakeGenericType(relatedType)
                                    .GetMethod(nameof(ITableCollection<object>.Find), new[] { predicateType, typeof(int), typeof(int) });

                                if (findMethod == null)
                                {
                                    throw new InvalidOperationException($"Find method not found on ITableCollection<{relatedType.Name}>.");
                                }

                                var fkPredicate = BuildForeignKeyEqualsExpression(relatedType, fkName, primaryId);
                                var combinedPredicate = CombinePredicates(relatedType, fkPredicate, relatedPredicate);

                                // Fetch related records using the combined predicate.
                                var relatedRecords = findMethod.Invoke(relatedTable, new object[] { combinedPredicate, 0, int.MaxValue }) as IEnumerable<object>;
                                if (relatedRecords != null)
                                {
                                    var relatedDocs = relatedRecords.Select(r =>
                                    {
                                        var doc = BsonMapper.Global.ToDocument(r);
                                        return FilterColumns(doc, relatedColumns, mapId: true);
                                    }).ToList();
                                    resultDoc[$"{relatedTableName}_Data"] = new BsonArray(relatedDocs);
                                }
                            }
                        }

                        results.Add(new QueryResult
                        {
                            TableName = _tableName,
                            Data = resultDoc
                        });
                    }
                }
                catch (Exception ex)
                {
                    _queryEngine.OnExceptionOccurred(new ExceptionEventArgs(ex));
                }

                if (string.IsNullOrEmpty(executeCommand))
                {
                    // If no execute command is provided, return the results as is.
                    return results;
                }

                // If an execute command is provided, parse and execute it.
                return ParseExecuteCommand(results, executeCommand);
            }

            // Executes the query, applying predicates and includes, or performs an inner join on results based on the execute command.
            internal List<QueryResult> ParseExecuteCommand(List<QueryResult> results, string executeCommand)
            {
                try
                {
                    if (string.IsNullOrEmpty(executeCommand))
                    {
                        // If no execute command is provided, return the original results.
                        return results;
                    }

                    // Parse the execute command: "Join [as <tableName>] Select <columns>"
                    //var match = Regex.Match(executeCommand, @"^Join(?:\s+as\s+(\w+))?\s+Select\s+(.+)$", RegexOptions.IgnoreCase);
                    //if (!match.Success)
                    //    throw new ArgumentException($"Invalid execute command format: {executeCommand}. Expected: Join [as <tableName>] Select <columns>");

                    // Parse the execute command: "InnerJoin [as <tableName>] Select <columns>"
                    var match = Regex.Match(executeCommand, @"^Join(?:\s+as\s+(.+?)\s+Select\s+(.+)$)?", RegexOptions.IgnoreCase);
                    if (!match.Success)
                    {
                        // Handle no-as case separately
                        match = Regex.Match(executeCommand, @"^Join\s+Select\s+(.+)$", RegexOptions.IgnoreCase);
                        if (!match.Success)
                            throw new ArgumentException($"Invalid execute command format: {executeCommand}. Expected: InnerJoin [as <tableName>] Select <columns>");
                    }

                    // Determine the joined table name.
                    string joinedTableName = "";
                    
                    if (match.Groups[1].Success)
                    {
                        joinedTableName = match.Groups[1].Value;
                    }
                    else
                    {
                        joinedTableName = _tableName;
                        foreach (var include in _includes)
                        {
                            joinedTableName += $"_{include.RelatedTableName}";
                        }
                       
                    } 

                    string selectColumnsString = match.Groups[2].Value;

                    // Parse the select columns.
                    var selectColumns = QueryUtils.ParseColumns(selectColumnsString);

                    var joinedResults = new List<QueryResult>();

                    foreach (var result in results)
                    {
                        var outerData = result.Data[$"{_tableName}_Data"] as BsonDocument;
                        var joinedDoc = new BsonArray();
                        foreach (var include in _includes)
                        {
                            var innerData = result.Data[$"{include.RelatedTableName}_Data"] as BsonArray;

                            if (outerData == null || innerData == null || innerData.Count == 0)
                                continue; // Skip if no matching data for inner join.


                            var joinedRecord = new BsonDocument();
                            foreach (var orderData in innerData.Cast<BsonDocument>())
                            {
                                // Combine customer and order data into a single document.
                                
                                foreach (var element in outerData)
                                {
                                    joinedRecord[element.Key] = element.Value;
                                }
                                foreach (var element in orderData)
                                {
                                    joinedRecord[element.Key] = element.Value;
                                }

                                


                            }

                            // Apply final column selection from the execute command.
                            var filteredRecord = FilterColumns(joinedRecord, selectColumns.ToArray(), mapId: false);
                            joinedDoc.Add(filteredRecord);
                        }
                        joinedResults.Add(new QueryResult
                        {
                            TableName = joinedTableName,
                            Data = new BsonDocument
                            {
                                { $"{joinedTableName}_Data", joinedDoc }
                            }
                        });
                    }

                   
                    return joinedResults;
                }
                catch (Exception ex)
                {
                    _queryEngine.OnExceptionOccurred(new ExceptionEventArgs(ex));
                    return new List<QueryResult>();
                }
            }

           
            // Filters a BsonDocument to include only specified columns, applying aliases.
            private BsonDocument FilterColumns(BsonDocument doc, (string ColumnName, string Alias)[] columns, bool mapId)
            {
                if (columns.Length == 0)
                {
                    // Return all columns if none are specified, mapping _id to Id if required.
                    if (mapId && doc.ContainsKey("_id"))
                    {
                        doc["Id"] = doc["_id"];
                        doc.Remove("_id");
                    }
                    return doc;
                }

                var filteredDoc = new BsonDocument();
                foreach (var (columnName, alias) in columns)
                {
                    var key = columnName;
                    if (mapId && columnName.Equals("Id", StringComparison.OrdinalIgnoreCase))
                    {
                        key = "_id";
                    }

                    if (doc.ContainsKey(key))
                    {
                        filteredDoc[alias] = doc[key];
                    }
                }

                return filteredDoc;
            }

            // Builds an expression to match a foreign key value.
            private LambdaExpression BuildForeignKeyEqualsExpression(Type relatedType, string propertyName, object fkValue)
            {
                var parameter = Expression.Parameter(relatedType, "x");
                var property = relatedType.GetProperty(propertyName);
                if (property == null)
                {
                    throw new InvalidOperationException($"Property {propertyName} not found on type {relatedType.Name}.");
                }

                var propertyAccess = Expression.Property(parameter, property);
                var constant = Expression.Constant(fkValue);
                var equals = Expression.Equal(propertyAccess, constant);
                return Expression.Lambda(equals, parameter);
            }

            // Combines foreign key and user predicates using AND.
            private LambdaExpression CombinePredicates(Type relatedType, LambdaExpression fkPredicate, LambdaExpression userPredicate)
            {
                var parameter = Expression.Parameter(relatedType, "x");
                var fkBody = Expression.Invoke(fkPredicate, parameter);
                var userBody = Expression.Invoke(userPredicate, parameter);
                var combinedBody = Expression.AndAlso(fkBody, userBody);
                return Expression.Lambda(combinedBody, parameter);
            }

            // Retrieves a typed table from the database.
            private ITableCollection GetTable(Type type, string tableName)
            {
                var method = typeof(IotDatabase).GetMethod(nameof(IotDatabase.Tables), new[] { typeof(string) });
                var genericMethod = method?.MakeGenericMethod(type);
                return genericMethod?.Invoke(_iotDatabase, new object[] { tableName }) as ITableCollection
                    ?? throw new InvalidOperationException($"Failed to get table {tableName} for type {type.Name}.");
            }
        }
    }

    public static class QueryUtils
    {
        // Parses a comma-separated column list, handling aliases (e.g., "Name as Person"), returns empty list for empty input.
        public static List<(string ColumnName, string Alias)> ParseColumns(string columnString)
        {
            // Return empty list if input is null, empty, or whitespace, indicating all columns should be included.
            if (string.IsNullOrWhiteSpace(columnString))
                return new List<(string ColumnName, string Alias)>();

            var columns = new List<(string ColumnName, string Alias)>();
            var columnParts = columnString.Split(',').Select(c => c.Trim());

            foreach (var part in columnParts)
            {
                // Skip empty parts to avoid parsing errors.
                if (string.IsNullOrWhiteSpace(part))
                    continue;

                var match = Regex.Match(part, @"^(\w+)(?:\s+as\s+(\w+))?$", RegexOptions.IgnoreCase);
                if (!match.Success)
                    throw new ArgumentException($"Invalid column format: {part}");

                string columnName = match.Groups[1].Value;
                string alias = match.Groups[2].Success ? match.Groups[2].Value : columnName;
                columns.Add((columnName, alias));
            }

            return columns;
        }
    }
}
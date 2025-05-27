using Iot.Database;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Iot.Database
{
    public partial class QueryEngine
    {
        /// <summary>
        /// Initializes a query builder for three primary tables with predicates and column selections.
        /// </summary>
        /// <typeparam name="T1">Type of the first primary table.</typeparam>
        /// <typeparam name="T2">Type of the second primary table.</typeparam>
        /// <typeparam name="T3">Type of the third primary table.</typeparam>
        /// <param name="tableName1">Name of the first table.</param>
        /// <param name="tableName2">Name of the second table.</param>
        /// <param name="tableName3">Name of the third table.</param>
        /// <param name="predicate1">Predicate for filtering the first table.</param>
        /// <param name="predicate2">Predicate for filtering the second table.</param>
        /// <param name="predicate3">Predicate for filtering the third table.</param>
        /// <param name="columns1">Columns to select from the first table, with optional aliases.</param>
        /// <param name="columns2">Columns to select from the second table, with optional aliases.</param>
        /// <param name="columns3">Columns to select from the third table, with optional aliases.</param>
        /// <returns>A QueryBuilder for the three tables.</returns>
        public QueryBuilder<T1, T2, T3> Find<T1, T2, T3>(
            string tableName1,
            string tableName2,
            string tableName3,
            Expression<Func<T1, bool>> predicate1,
            Expression<Func<T2, bool>> predicate2,
            Expression<Func<T3, bool>> predicate3,
            string[] columns1,
            string[] columns2,
            string[] columns3) where T1 : class where T2 : class where T3 : class
        {
            var parsedColumns1 = columns1.Length == 0 || columns1.All(string.IsNullOrWhiteSpace)
                ? new List<(string ColumnName, string Alias)>()
                : QueryUtils.ParseColumns(string.Join(",", columns1));
            var parsedColumns2 = columns2.Length == 0 || columns2.All(string.IsNullOrWhiteSpace)
                ? new List<(string ColumnName, string Alias)>()
                : QueryUtils.ParseColumns(string.Join(",", columns2));
            var parsedColumns3 = columns3.Length == 0 || columns3.All(string.IsNullOrWhiteSpace)
                ? new List<(string ColumnName, string Alias)>()
                : QueryUtils.ParseColumns(string.Join(",", columns3));
            return new QueryBuilder<T1, T2, T3>(
                _iotDatabase,
                tableName1,
                tableName2,
                tableName3,
                predicate1,
                predicate2,
                predicate3,
                parsedColumns1.ToArray(),
                parsedColumns2.ToArray(),
                parsedColumns3.ToArray(),
                this);
        }

        /// <summary>
        /// Executes a natural language-like query for three primary tables with join conditions.
        /// </summary>
        /// <param name="query">Query string in the format: FIND <table1>,<table2>,<table3> [ON <join_condition1> AND <join_condition2>] [WHERE <condition1> AND <condition2> AND <condition3>] [SELECT <columns1>,<columns2>,<columns3>] [INCLUDE <related_table> WHERE <related_condition> [SELECT <related_columns>]] [ORDER BY <column> [ASC|DESC]] [LIMIT <n>]</param>
        /// <returns>A list of QueryResult containing the query results.</returns>
        public List<QueryResult> NaturalQueryTriple(string query)
        {
            try
            {
                // Parse the query for three tables
                var parsedQuery = ParseNaturalQueryTriple(query);

                // Resolve table types
                Type type1 = GetTableType(parsedQuery.TableName1);
                Type type2 = GetTableType(parsedQuery.TableName2);
                Type type3 = GetTableType(parsedQuery.TableName3);

                // Locate the generic Find method for three tables
                var findMethodInfo = typeof(QueryEngine)
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(m => m.Name == nameof(Find) && m.IsGenericMethod &&
                        m.GetParameters().Length == 9 &&
                        m.GetParameters()[0].ParameterType == typeof(string) &&
                        m.GetParameters()[1].ParameterType == typeof(string) &&
                        m.GetParameters()[2].ParameterType == typeof(string) &&
                        m.GetParameters()[6].ParameterType == typeof(string[]) &&
                        m.GetParameters()[7].ParameterType == typeof(string[]) &&
                        m.GetParameters()[8].ParameterType == typeof(string[]));

                if (findMethodInfo == null)
                    throw new InvalidOperationException("Find method for three tables not found on QueryEngine.");

                // Create generic method for the three table types
                var genericFindMethod = findMethodInfo.MakeGenericMethod(type1, type2, type3);

                // Build predicates
                var predicate1 = BuildPredicate(type1, parsedQuery.Condition1);
                var predicate2 = BuildPredicate(type2, parsedQuery.Condition2);
                var predicate3 = BuildPredicate(type3, parsedQuery.Condition3);
                var columns1 = parsedQuery.Columns1.Select(c => $"{c.ColumnName}{(c.Alias != c.ColumnName ? $" as {c.Alias}" : "")}").ToArray();
                var columns2 = parsedQuery.Columns2.Select(c => $"{c.ColumnName}{(c.Alias != c.ColumnName ? $" as {c.Alias}" : "")}").ToArray();
                var columns3 = parsedQuery.Columns3.Select(c => $"{c.ColumnName}{(c.Alias != c.ColumnName ? $" as {c.Alias}" : "")}").ToArray();

                // Invoke the Find method
                var queryBuilder = genericFindMethod.Invoke(this, new object[] { parsedQuery.TableName1, parsedQuery.TableName2, parsedQuery.TableName3, predicate1, predicate2, predicate3, columns1, columns2, columns3 })
                    as dynamic;

                // Process INCLUDE clauses
                foreach (var include in parsedQuery.Includes)
                {
                    Type relatedType = GetTableType(include.TableName);
                    var includeMethodInfo = typeof(QueryBuilder<,,>)
                        .MakeGenericType(type1, type2, type3)
                        .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .FirstOrDefault(m => m.Name == nameof(QueryBuilder<object, object, object>.Include) && m.IsGenericMethod &&
                            m.GetParameters().Length == 3 &&
                            m.GetParameters()[0].ParameterType == typeof(string) &&
                            m.GetParameters()[2].ParameterType == typeof(string[]));

                    if (includeMethodInfo == null)
                        throw new InvalidOperationException("Include method not found on QueryBuilder.");

                    var genericIncludeMethod = includeMethodInfo.MakeGenericMethod(relatedType);
                    var relatedPredicate = BuildPredicate(relatedType, include.Condition);
                    var relatedColumns = include.Columns.Select(c => $"{c.ColumnName}{(c.Alias != c.ColumnName ? $" as {c.Alias}" : "")}").ToArray();

                    queryBuilder = genericIncludeMethod.Invoke(queryBuilder, new object[] { include.TableName, relatedPredicate, relatedColumns });
                }

                // Execute the query
                var results = ExecuteQuery(queryBuilder, parsedQuery.OrderBy, parsedQuery.Limit, parsedQuery.JoinCondition1, parsedQuery.JoinCondition2);

                // Process JOIN command if present
                if (!string.IsNullOrEmpty(parsedQuery.JoinCommand))
                {
                    return queryBuilder.ParseExecuteCommand(results, parsedQuery.JoinCommand);
                }

                return results;
            }
            catch (Exception ex)
            {
                OnExceptionOccurred(new ExceptionEventArgs(ex));
                return new List<QueryResult>();
            }
        }

        /// <summary>
        /// Parses a natural language query for three primary tables with join conditions.
        /// </summary>
        private (string TableName1, string TableName2, string TableName3, string JoinCondition1, string JoinCondition2, string Condition1, string Condition2, string Condition3, List<(string ColumnName, string Alias)> Columns1, List<(string ColumnName, string Alias)> Columns2, List<(string ColumnName, string Alias)> Columns3, List<(string TableName, string Condition, List<(string ColumnName, string Alias)> Columns)> Includes, (string Field, bool IsAscending)? OrderBy, int? Limit, string JoinCommand) ParseNaturalQueryTriple(string query)
        {
            query = query.Trim();
            var includes = new List<(string TableName, string Condition, List<(string ColumnName, string Alias)> Columns)>();
            (string Field, bool IsAscending)? orderBy = null;
            int? limit = null;
            string joinCommand = "";
            string joinCondition1 = "";
            string joinCondition2 = "";

            // Split query into parts
            var parts = Regex.Split(query, @"\s+(ON|WHERE|INCLUDE|JOIN|ORDER BY|LIMIT)\s+", RegexOptions.IgnoreCase);
            var findPart = parts[0].Trim();

            // Parse FIND clause for three tables
            var findMatch = Regex.Match(findPart, @"FIND\s+(\w+),\s*(\w+),\s*(\w+)", RegexOptions.IgnoreCase);
            if (!findMatch.Success)
                throw new ArgumentException("Invalid query format. Expected: FIND <table1>,<table2>,<table3>");

            string tableName1 = findMatch.Groups[1].Value;
            string tableName2 = findMatch.Groups[2].Value;
            string tableName3 = findMatch.Groups[3].Value;

            // Initialize defaults
            string condition1 = "";
            string condition2 = "";
            string condition3 = "";
            var columns1 = new List<(string ColumnName, string Alias)>();
            var columns2 = new List<(string ColumnName, string Alias)>();
            var columns3 = new List<(string ColumnName, string Alias)>();

            // Process remaining parts
            int i = 1;
            while (i < parts.Length)
            {
                var keyword = parts[i].ToUpper();
                var part = parts[i + 1].Trim();

                if (keyword == "ON")
                {
                    var onMatch = Regex.Match(part, @"(.+?)\s+AND\s+(.+)", RegexOptions.IgnoreCase);
                    if (!onMatch.Success)
                        throw new ArgumentException("Invalid ON format. Expected: ON <join_condition1> AND <join_condition2>");

                    joinCondition1 = onMatch.Groups[1].Value.Trim();
                    joinCondition2 = onMatch.Groups[2].Value.Trim();
                    i += 2;
                }
                else if (keyword == "WHERE")
                {
                    var whereMatch = Regex.Match(part, @"(.+?)\s+AND\s+(.+?)\s+AND\s+(.+)", RegexOptions.IgnoreCase);
                    if (!whereMatch.Success)
                        throw new ArgumentException("Invalid WHERE format. Expected: WHERE <condition1> AND <condition2> AND <condition3>");

                    condition1 = whereMatch.Groups[1].Value.Trim();
                    condition2 = whereMatch.Groups[2].Value.Trim();
                    condition3 = whereMatch.Groups[3].Value.Trim();
                    i += 2;
                }
                else if (keyword == "SELECT")
                {
                    var selectParts = part.Split(',').Select(p => p.Trim()).ToList();
                    if (selectParts.Count < 3)
                        throw new ArgumentException("Invalid SELECT format. Expected: SELECT <columns1>,<columns2>,<columns3>");

                    columns1 = QueryUtils.ParseColumns(selectParts[0]);
                    columns2 = QueryUtils.ParseColumns(selectParts[1]);
                    columns3 = QueryUtils.ParseColumns(selectParts[2]);
                    i += 2;
                }
                else if (keyword == "INCLUDE")
                {
                    var includeMatch = Regex.Match(part, @"(\w+)(?:\s+WHERE\s+(.+?))?(?:\s+SELECT\s+(.+))?$", RegexOptions.IgnoreCase);
                    if (!includeMatch.Success)
                        throw new ArgumentException($"Invalid INCLUDE format: {part}");

                    string relatedTableName = includeMatch.Groups[1].Value;
                    string relatedCondition = includeMatch.Groups[2].Success ? includeMatch.Groups[2].Value.Trim() : "";
                    var relatedColumns = includeMatch.Groups[3].Success && !string.IsNullOrWhiteSpace(includeMatch.Groups[3].Value)
                        ? QueryUtils.ParseColumns(includeMatch.Groups[3].Value)
                        : new List<(string ColumnName, string Alias)>();

                    includes.Add((relatedTableName, relatedCondition, relatedColumns));
                    i += 2;
                }
                else if (keyword == "JOIN")
                {
                    var joinMatch = Regex.Match(part, @"^(?:as\s+(.+?)\s+select\s+(.+)$)", RegexOptions.IgnoreCase);
                    if (!joinMatch.Success)
                        throw new ArgumentException($"Invalid JOIN format: {part}. Expected: JOIN [as <tableName>] select <columns>");

                    joinCommand = joinMatch.Groups[1].Success
                        ? $"Join as {joinMatch.Groups[1].Value.Trim()} Select {joinMatch.Groups[2].Value}"
                        : $"Join Select {joinMatch.Groups[2].Value}";
                    i += 2;
                }
                else if (keyword == "ORDER BY")
                {
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
                    if (!int.TryParse(part, out var limitValue))
                        throw new ArgumentException($"Invalid LIMIT value: {part}");

                    limit = limitValue;
                    i += 2;
                }
                else
                {
                    i += 2;
                }
            }

            // Infer join conditions if not specified
            if (string.IsNullOrEmpty(joinCondition1))
            {
                var table2 = _iotDatabase.GetTable(tableName2);
                var foreignKey = table2?.TableInfo.ForeignKeys.FirstOrDefault(fk => fk.ForeignKeyAttribute?.TableName == tableName1);
                if (foreignKey != null)
                {
                    joinCondition1 = $"{tableName2}.{foreignKey.Name} = {tableName1}.Id";
                }
                else
                {
                    throw new ArgumentException($"No foreign key relationship found from {tableName2} to {tableName1}. Specify an ON clause.");
                }
            }
            if (string.IsNullOrEmpty(joinCondition2))
            {
                var table3 = _iotDatabase.GetTable(tableName3);
                var foreignKey = table3?.TableInfo.ForeignKeys.FirstOrDefault(fk => fk.ForeignKeyAttribute?.TableName == tableName2);
                if (foreignKey != null)
                {
                    joinCondition2 = $"{tableName3}.{foreignKey.Name} = {tableName2}.Id";
                }
                else
                {
                    foreignKey = table3?.TableInfo.ForeignKeys.FirstOrDefault(fk => fk.ForeignKeyAttribute?.TableName == tableName1);
                    if (foreignKey != null)
                    {
                        joinCondition2 = $"{tableName3}.{foreignKey.Name} = {tableName1}.Id";
                    }
                    else
                    {
                        throw new InvalidOperationException($"No foreign key relationship found from {tableName3} to {tableName2} or {tableName1}. Specify an ON clause.");
                    }
                }
            }

            return (tableName1, tableName2, tableName3, joinCondition1, joinCondition2, condition1, condition2, condition3, columns1, columns2, columns3, includes, orderBy, limit, joinCommand);
        }

        // Modified ExecuteQuery to include join conditions
        private List<QueryResult> ExecuteQuery(dynamic queryBuilder, (string Field, bool IsAscending)? orderBy, int? limit, string joinCondition1 = null, string joinCondition2 = null)
        {
            var results = (queryBuilder.Execute(joinCondition1, joinCondition2) as List<QueryResult>) ?? new List<QueryResult>();

            if (orderBy.HasValue)
            {
                var field = orderBy.Value.Field;
                var isAscending = orderBy.Value.IsAscending;

                results = results.OrderBy(r =>
                {
                    var doc = r.Data[$"{r.TableName}_Data"] as BsonDocument;
                    return doc?.ContainsKey(field) == true ? doc[field] : BsonValue.Null;
                }, isAscending ? Comparer<BsonValue>.Default : Comparer<BsonValue>.Create((a, b) => Comparer<BsonValue>.Default.Compare(b, a))).ToList();
            }

            if (limit.HasValue)
            {
                results = results.Take(limit.Value).ToList();
            }

            return results;
        }
    }

    public class QueryBuilder<T1, T2, T3> where T1 : class where T2 : class where T3 : class
    {
        private readonly IotDatabase _iotDatabase;
        private readonly string _tableName1;
        private readonly string _tableName2;
        private readonly string _tableName3;
        private readonly Expression<Func<T1, bool>> _predicate1;
        private readonly Expression<Func<T2, bool>> _predicate2;
        private readonly Expression<Func<T3, bool>> _predicate3;
        private readonly (string ColumnName, string Alias)[] _columns1;
        private readonly (string ColumnName, string Alias)[] _columns2;
        private readonly (string ColumnName, string Alias)[] _columns3;
        private readonly QueryEngine _queryEngine;
        private readonly List<(string RelatedTableName, Type RelatedType, LambdaExpression Predicate, (string ColumnName, string Alias)[] Columns)> _includes;

        public QueryBuilder(
            IotDatabase iotDatabase,
            string tableName1,
            string tableName2,
            string tableName3,
            Expression<Func<T1, bool>> predicate1,
            Expression<Func<T2, bool>> predicate2,
            Expression<Func<T3, bool>> predicate3,
            (string ColumnName, string Alias)[] columns1,
            (string ColumnName, string Alias)[] columns2,
            (string ColumnName, string Alias)[] columns3,
            QueryEngine queryEngine)
        {
            _iotDatabase = iotDatabase;
            _tableName1 = tableName1;
            _tableName2 = tableName2;
            _tableName3 = tableName3;
            _predicate1 = predicate1;
            _predicate2 = predicate2;
            _predicate3 = predicate3;
            _columns1 = columns1 ?? Array.Empty<(string, string)>();
            _columns2 = columns2 ?? Array.Empty<(string, string)>();
            _columns3 = columns3 ?? Array.Empty<(string, string)>();
            _queryEngine = queryEngine;
            _includes = new List<(string, Type, LambdaExpression, (string, string)[])>();
        }

        public QueryBuilder<T1, T2, T3> Include<TRelated>(
            string relatedTableName,
            Expression<Func<TRelated, bool>> relatedPredicate,
            params string[] columns) where TRelated : class
        {
            var parsedColumns = columns.Length == 0 || columns.All(string.IsNullOrWhiteSpace)
                ? new List<(string ColumnName, string Alias)>()
                : QueryUtils.ParseColumns(string.Join(",", columns));
            _includes.Add((relatedTableName, typeof(TRelated), relatedPredicate, parsedColumns.ToArray()));
            return this;
        }

        public List<QueryEngine.QueryResult> Execute(string joinCondition1 = "", string joinCondition2 = "")
        {
            var results = new List<QueryEngine.QueryResult>();
            try
            {
                // Fetch records from first table
                var table1 = _iotDatabase.Tables<T1>(_tableName1);
                var records1 = table1.FindAll();
                var predicateFunc1 = _predicate1.Compile();
                records1 = records1.Where(predicateFunc1).ToList();

                // Fetch records from second table
                var table2 = _iotDatabase.Tables<T2>(_tableName2);
                var records2 = table2.FindAll();
                var predicateFunc2 = _predicate2.Compile();
                records2 = records2.Where(predicateFunc2).ToList();

                // Fetch records from third table
                var table3 = _iotDatabase.Tables<T3>(_tableName3);
                var records3 = table3.FindAll();
                var predicateFunc3 = _predicate3.Compile();
                records3 = records3.Where(predicateFunc3).ToList();

                // Parse join condition 1 (e.g., "Orders.CustomerId = Customers.Id")
                (string table2Prop, string table1Prop)? joinProps1 = null;
                if (!string.IsNullOrEmpty(joinCondition1))
                {
                    var joinMatch = Regex.Match(joinCondition1, @"(\w+)\.(\w+)\s*=\s*(\w+)\.(\w+)", RegexOptions.IgnoreCase);
                    if (joinMatch.Success)
                    {
                        var leftTable = joinMatch.Groups[1].Value;
                        var leftProp = joinMatch.Groups[2].Value;
                        var rightTable = joinMatch.Groups[3].Value;
                        var rightProp = joinMatch.Groups[4].Value;

                        if (leftTable.Equals(_tableName2, StringComparison.OrdinalIgnoreCase) && rightTable.Equals(_tableName1, StringComparison.OrdinalIgnoreCase))
                        {
                            joinProps1 = (leftProp, rightProp);
                        }
                        else if (leftTable.Equals(_tableName1, StringComparison.OrdinalIgnoreCase) && rightTable.Equals(_tableName2, StringComparison.OrdinalIgnoreCase))
                        {
                            joinProps1 = (rightProp, leftProp);
                        }
                        else
                        {
                            throw new ArgumentException($"Invalid join condition: {joinCondition1}");
                        }
                    }
                }
                else
                {
                    var table2Info = table2.TableInfo;
                    var foreignKey = table2Info.ForeignKeys.FirstOrDefault(fk => fk.ForeignKeyAttribute?.TableName == _tableName1);
                    if (foreignKey != null)
                    {
                        joinProps1 = (foreignKey.Name, "Id");
                    }
                    else
                    {
                        throw new InvalidOperationException($"No foreign key relationship found from {_tableName2} to {_tableName1}. Specify an ON clause.");
                    }
                }

                // Parse join condition 2 (e.g., "OrderDetails.OrderId = Orders.Id" or "OrderDetails.CustomerId = Customers.Id")
                (string table3Prop, string targetProp, string targetTable)? joinProps2 = null;
                bool joinsWithTable1 = false;
                if (!string.IsNullOrEmpty(joinCondition2))
                {
                    var joinMatch = Regex.Match(joinCondition2, @"(\w+)\.(\w+)\s*=\s*(\w+)\.(\w+)", RegexOptions.IgnoreCase);
                    if (joinMatch.Success)
                    {
                        var leftTable = joinMatch.Groups[1].Value;
                        var leftProp = joinMatch.Groups[2].Value;
                        var rightTable = joinMatch.Groups[3].Value;
                        var rightProp = joinMatch.Groups[4].Value;

                        if (leftTable.Equals(_tableName3, StringComparison.OrdinalIgnoreCase))
                        {
                            joinProps2 = (leftProp, rightProp, rightTable);
                            joinsWithTable1 = rightTable.Equals(_tableName1, StringComparison.OrdinalIgnoreCase);
                        }
                        else if (rightTable.Equals(_tableName3, StringComparison.OrdinalIgnoreCase))
                        {
                            joinProps2 = (rightProp, leftProp, leftTable);
                            joinsWithTable1 = leftTable.Equals(_tableName1, StringComparison.OrdinalIgnoreCase);
                        }
                        else
                        {
                            throw new ArgumentException($"Invalid join condition: {joinCondition2}");
                        }
                    }
                }
                else
                {
                    var table3Info = table3.TableInfo;
                    var foreignKey = table3Info.ForeignKeys.FirstOrDefault(fk => fk.ForeignKeyAttribute?.TableName == _tableName2);
                    if (foreignKey != null)
                    {
                        joinProps2 = (foreignKey.Name, "Id", _tableName2);
                        joinsWithTable1 = false;
                    }
                    else
                    {
                        foreignKey = table3Info.ForeignKeys.FirstOrDefault(fk => fk.ForeignKeyAttribute?.TableName == _tableName1);
                        if (foreignKey != null)
                        {
                            joinProps2 = (foreignKey.Name, "Id", _tableName1);
                            joinsWithTable1 = true;
                        }
                        else
                        {
                            throw new InvalidOperationException($"No foreign key relationship found from {_tableName3} to {_tableName2} or {_tableName1}. Specify an ON clause.");
                        }
                    }
                }

                // Join records based on foreign key relationships
                foreach (var record1 in records1)
                {
                    var doc1 = BsonMapper.Global.ToDocument(record1);
                    var filteredDoc1 = FilterColumns(doc1, _columns1, mapId: true);
                    var key1 = table1.TableInfo.Id?.PropertyInfo.GetValue(record1);

                    // Find matching records in table2
                    var matchingRecords2 = records2.Where(r2 =>
                    {
                        var propInfo = table2.TableInfo.Type.GetProperty(joinProps1.Value.table2Prop, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                        if (propInfo == null)
                            throw new InvalidOperationException($"Property {joinProps1.Value.table2Prop} not found on type {table2.TableInfo.Type.Name}.");
                        var key2 = propInfo.GetValue(r2);
                        return key1 != null && key2 != null && key1.Equals(key2);
                    }).ToList();

                    foreach (var record2 in matchingRecords2)
                    {
                        var doc2 = BsonMapper.Global.ToDocument(record2);
                        var filteredDoc2 = FilterColumns(doc2, _columns2, mapId: true);
                        var key2 = table2.TableInfo.Id?.PropertyInfo.GetValue(record2);

                        // Find matching records in table3
                        var matchingRecords3 = records3.Where(r3 =>
                        {
                            var propInfo = table3.TableInfo.Type.GetProperty(joinProps2.Value.table3Prop, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                            if (propInfo == null)
                                throw new InvalidOperationException($"Property {joinProps2.Value.table3Prop} not found on type {table3.TableInfo.Type.Name}.");
                            var key3 = propInfo.GetValue(r3);
                            if (joinsWithTable1)
                            {
                                return key1 != null && key3 != null && key1.Equals(key3);
                            }
                            else
                            {
                                return key2 != null && key3 != null && key2.Equals(key3);
                            }
                        }).ToList();

                        foreach (var record3 in matchingRecords3)
                        {
                            var doc3 = BsonMapper.Global.ToDocument(record3);
                            var filteredDoc3 = FilterColumns(doc3, _columns3, mapId: true);

                            var resultDoc = new BsonDocument
                            {
                                { $"{_tableName1}_Data", filteredDoc1 },
                                { $"{_tableName2}_Data", filteredDoc2 },
                                { $"{_tableName3}_Data", filteredDoc3 }
                            };

                            // Process includes
                            foreach (var include in _includes)
                            {
                                var relatedTableName = include.RelatedTableName;
                                var relatedType = include.RelatedType;
                                var relatedPredicate = include.Predicate;
                                var relatedColumns = include.Columns;

                                var relatedTable = GetTable(relatedType, relatedTableName);
                                var relatedTableInfo = relatedTable.TableInfo;

                                // Check for foreign key relationships to T1, T2, or T3
                                var foreignKey = relatedTableInfo.ForeignKeys.FirstOrDefault(fk =>
                                    fk.ForeignKeyAttribute?.TableName == _tableName1 ||
                                    fk.ForeignKeyAttribute?.TableName == _tableName2 ||
                                    fk.ForeignKeyAttribute?.TableName == _tableName3);

                                if (foreignKey == null)
                                    continue;

                                var fkName = foreignKey.Name;
                                object primaryId = foreignKey.ForeignKeyAttribute?.TableName switch
                                {
                                    var t when t == _tableName1 => table1.TableInfo.Id?.PropertyInfo.GetValue(record1),
                                    var t when t == _tableName2 => table2.TableInfo.Id?.PropertyInfo.GetValue(record2),
                                    var t when t == _tableName3 => table3.TableInfo.Id?.PropertyInfo.GetValue(record3),
                                    _ => null
                                };

                                if (primaryId != null)
                                {
                                    var predicateType = typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(relatedType, typeof(bool)));
                                    var findMethod = typeof(ITableCollection<>)
                                        .MakeGenericType(relatedType)
                                        .GetMethod(nameof(ITableCollection<object>.Find), new[] { predicateType, typeof(int), typeof(int) });

                                    if (findMethod == null)
                                        throw new InvalidOperationException($"Find method not found on ITableCollection<{relatedType.Name}>.");

                                    var fkPredicate = BuildForeignKeyEqualsExpression(relatedType, fkName, primaryId);
                                    var combinedPredicate = CombinePredicates(relatedType, fkPredicate, relatedPredicate);

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

                            results.Add(new QueryEngine.QueryResult
                            {
                                TableName = $"{_tableName1}_{_tableName2}_{_tableName3}",
                                Data = resultDoc
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //_queryEngine.OnExceptionOccurred(new ExceptionEventArgs(ex));
            }

            return results;
        }

        internal List<QueryEngine.QueryResult> ParseExecuteCommand(List<QueryEngine.QueryResult> results, string executeCommand)
        {
            try
            {
                if (string.IsNullOrEmpty(executeCommand))
                    return results;

                var match = Regex.Match(executeCommand, @"^Join(?:\s+as\s+(.+?))?\s+Select\s+(.+)$", RegexOptions.IgnoreCase);
                if (!match.Success)
                    throw new ArgumentException($"Invalid execute command format: {executeCommand}. Expected: Join [as <tableName>] Select <columns>");

                string joinedTableName = match.Groups[1].Success ? match.Groups[1].Value : $"{_tableName1}_{_tableName2}_{_tableName3}";
                string selectColumnsString = match.Groups[2].Value;
                var selectColumns = QueryUtils.ParseColumns(selectColumnsString);

                var joinedResults = new List<QueryEngine.QueryResult>();

                foreach (var result in results)
                {
                    var data1 = result.Data[$"{_tableName1}_Data"] as BsonDocument;
                    var data2 = result.Data[$"{_tableName2}_Data"] as BsonDocument;
                    var data3 = result.Data[$"{_tableName3}_Data"] as BsonDocument;
                    if (data1 == null || data2 == null || data3 == null)
                        continue;

                    var combinedRecord = new BsonDocument();
                    foreach (var element in data1)
                        combinedRecord[element.Key] = element.Value;
                    foreach (var element in data2)
                        if (!combinedRecord.Contains(element))
                            combinedRecord[element.Key] = element.Value;
                    foreach (var element in data3)
                        if (!combinedRecord.Contains(element))
                            combinedRecord[element.Key] = element.Value;

                    foreach (var include in _includes)
                    {
                        var innerData = result.Data[$"{include.RelatedTableName}_Data"] as BsonArray;
                        if (innerData == null || innerData.Count == 0)
                            continue;

                        foreach (var inData in innerData.Cast<BsonDocument>())
                        {
                            foreach (var element in inData)
                            {
                                if (!combinedRecord.Contains(element))
                                    combinedRecord[element.Key] = element.Value;
                            }
                        }
                    }

                    var filteredRecord = FilterColumns(combinedRecord, selectColumns.ToArray(), mapId: false);
                    var joinedArray = new BsonArray { filteredRecord };

                    joinedResults.Add(new QueryEngine.QueryResult
                    {
                        TableName = joinedTableName,
                        Data = new BsonDocument
                        {
                            { $"{joinedTableName}_Data", joinedArray }
                        }
                    });
                }

                return joinedResults;
            }
            catch (Exception ex)
            {
                //_queryEngine.OnExceptionOccurred(new ExceptionEventArgs(ex));
                return new List<QueryEngine.QueryResult>();
            }
        }

        private BsonDocument FilterColumns(BsonDocument doc, (string ColumnName, string Alias)[] columns, bool mapId)
        {
            if (columns.Length == 0)
            {
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
                    key = "_id";

                if (doc.ContainsKey(key))
                    filteredDoc[alias] = doc[key];
            }

            return filteredDoc;
        }

        private LambdaExpression BuildForeignKeyEqualsExpression(Type relatedType, string propertyName, object fkValue)
        {
            var parameter = Expression.Parameter(relatedType, "x");
            var property = relatedType.GetProperty(propertyName);
            if (property == null)
                throw new InvalidOperationException($"Property {propertyName} not found on type {relatedType.Name}.");

            var propertyAccess = Expression.Property(parameter, property);
            var constant = Expression.Constant(fkValue);
            var equals = Expression.Equal(propertyAccess, constant);
            return Expression.Lambda(equals, parameter);
        }

        private LambdaExpression CombinePredicates(Type relatedType, LambdaExpression fkPredicate, LambdaExpression userPredicate)
        {
            var parameter = Expression.Parameter(relatedType, "x");
            var fkBody = Expression.Invoke(fkPredicate, parameter);
            var userBody = Expression.Invoke(userPredicate, parameter);
            var combinedBody = Expression.AndAlso(fkBody, userBody);
            return Expression.Lambda(combinedBody, parameter);
        }

        private ITableCollection GetTable(Type type, string tableName)
        {
            var method = typeof(IotDatabase).GetMethod(nameof(IotDatabase.Tables), new[] { typeof(string) });
            var genericMethod = method?.MakeGenericMethod(type);
            return genericMethod?.Invoke(_iotDatabase, new object[] { tableName }) as ITableCollection
                ?? throw new InvalidOperationException($"Failed to get table {tableName} for type {type.Name}.");
        }
    }
}
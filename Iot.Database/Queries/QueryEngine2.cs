using Iot.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Iot.Database
{
    public partial class QueryEngine
    {
        /// <summary>
        /// Initializes a query builder for two primary tables with predicates and column selections.
        /// </summary>
        /// <typeparam name="T1">Type of the first primary table.</typeparam>
        /// <typeparam name="T2">Type of the second primary table.</typeparam>
        /// <param name="tableName1">Name of the first table.</param>
        /// <param name="tableName2">Name of the second table.</param>
        /// <param name="predicate1">Predicate for filtering the first table.</param>
        /// <param name="predicate2">Predicate for filtering the second table.</param>
        /// <param name="columns1">Columns to select from the first table, with optional aliases.</param>
        /// <param name="columns2">Columns to select from the second table, with optional aliases.</param>
        /// <returns>A QueryBuilder for the two tables.</returns>
        public QueryBuilder<T1, T2> Find<T1, T2>(
            string tableName1,
            string tableName2,
            Expression<Func<T1, bool>> predicate1,
            Expression<Func<T2, bool>> predicate2,
            string[] columns1,
            string[] columns2) where T1 : class where T2 : class
        {
            var parsedColumns1 = columns1.Length == 0 || columns1.All(string.IsNullOrWhiteSpace)
                ? new List<(string ColumnName, string Alias)>()
                : QueryUtils.ParseColumns(string.Join(",", columns1));
            var parsedColumns2 = columns2.Length == 0 || columns2.All(string.IsNullOrWhiteSpace)
                ? new List<(string ColumnName, string Alias)>()
                : QueryUtils.ParseColumns(string.Join(",", columns2));
            return new QueryBuilder<T1, T2>(
                _iotDatabase,
                tableName1,
                tableName2,
                predicate1,
                predicate2,
                parsedColumns1.ToArray(),
                parsedColumns2.ToArray(),
                this);
        }

        /// <summary>
        /// Executes a natural language-like query for two primary tables with a join condition.
        /// </summary>
        /// <param name="query">Query string in the format: FIND <table1>,<table2> [ON <join_condition>] [WHERE <condition1> AND <condition2>] [SELECT <columns1>,<columns2>] [INCLUDE <related_table> WHERE <related_condition> [SELECT <related_columns>]] [ORDER BY <column> [ASC|DESC]] [LIMIT <n>]</param>
        /// <returns>A list of QueryResult containing the query results.</returns>
        public List<QueryResult> NaturalQueryDual(string query)
        {
            try
            {
                // Parse the query for two tables
                var parsedQuery = ParseNaturalQueryDual(query);

                // Resolve table types
                Type type1 = GetTableType(parsedQuery.TableName1);
                Type type2 = GetTableType(parsedQuery.TableName2);

                // Locate the generic Find method for two tables
                var findMethodInfo = typeof(QueryEngine)
                    .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(m => m.Name == nameof(Find) && m.IsGenericMethod &&
                        m.GetParameters().Length == 6 &&
                        m.GetParameters()[0].ParameterType == typeof(string) &&
                        m.GetParameters()[1].ParameterType == typeof(string) &&
                        m.GetParameters()[4].ParameterType == typeof(string[]) &&
                        m.GetParameters()[5].ParameterType == typeof(string[]));

                if (findMethodInfo == null)
                    throw new InvalidOperationException("Find method for two tables not found on QueryEngine.");

                // Create generic method for the two table types
                var genericFindMethod = findMethodInfo.MakeGenericMethod(type1, type2);

                // Build predicates
                var predicate1 = BuildPredicate(type1, parsedQuery.Condition1);
                var predicate2 = BuildPredicate(type2, parsedQuery.Condition2);
                var columns1 = parsedQuery.Columns1.Select(c => $"{c.ColumnName}{(c.Alias != c.ColumnName ? $" as {c.Alias}" : "")}").ToArray();
                var columns2 = parsedQuery.Columns2.Select(c => $"{c.ColumnName}{(c.Alias != c.ColumnName ? $" as {c.Alias}" : "")}").ToArray();

                // Invoke the Find method
                var queryBuilder = genericFindMethod.Invoke(this, new object[] { parsedQuery.TableName1, parsedQuery.TableName2, predicate1, predicate2, columns1, columns2 })
                    as dynamic;

                // Process INCLUDE clauses
                foreach (var include in parsedQuery.Includes)
                {
                    Type relatedType = GetTableType(include.TableName);
                    var includeMethodInfo = typeof(QueryBuilder<,>)
                        .MakeGenericType(type1, type2)
                        .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .FirstOrDefault(m => m.Name == nameof(QueryBuilder<object, object>.Include) && m.IsGenericMethod &&
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
                var results = ExecuteQuery(queryBuilder, parsedQuery.OrderBy, parsedQuery.Limit, parsedQuery.JoinCondition);

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
        /// Parses a natural language query for two primary tables with a join condition.
        /// </summary>
        private (string TableName1, string TableName2, string JoinCondition, string Condition1, string Condition2, List<(string ColumnName, string Alias)> Columns1, List<(string ColumnName, string Alias)> Columns2, List<(string TableName, string Condition, List<(string ColumnName, string Alias)> Columns)> Includes, (string Field, bool IsAscending)? OrderBy, int? Limit, string JoinCommand) ParseNaturalQueryDual(string query)
        {
            query = query.Trim();
            var includes = new List<(string TableName, string Condition, List<(string ColumnName, string Alias)> Columns)>();
            (string Field, bool IsAscending)? orderBy = null;
            int? limit = null;
            string joinCommand = "";
            string joinCondition = "";

            // Split query into parts
            var parts = Regex.Split(query, @"\s+(ON|WHERE|INCLUDE|JOIN|ORDER BY|LIMIT)\s+", RegexOptions.IgnoreCase);
            var findPart = parts[0].Trim();

            // Parse FIND clause for two tables
            var findMatch = Regex.Match(findPart, @"FIND\s+(\w+),\s*(\w+)", RegexOptions.IgnoreCase);
            if (!findMatch.Success)
                throw new ArgumentException("Invalid query format. Expected: FIND <table1>,<table2>");

            string tableName1 = findMatch.Groups[1].Value;
            string tableName2 = findMatch.Groups[2].Value;

            // Initialize defaults
            string condition1 = "";
            string condition2 = "";
            var columns1 = new List<(string ColumnName, string Alias)>();
            var columns2 = new List<(string ColumnName, string Alias)>();

            // Process remaining parts
            int i = 1;
            while (i < parts.Length)
            {
                var keyword = parts[i].ToUpper();
                var part = parts[i + 1].Trim();

                if (keyword == "ON")
                {
                    joinCondition = part;
                    i += 2;
                }
                else if (keyword == "WHERE")
                {
                    var whereMatch = Regex.Match(part, @"(.+?)\s+AND\s+(.+)", RegexOptions.IgnoreCase);
                    if (!whereMatch.Success)
                        throw new ArgumentException("Invalid WHERE format. Expected: WHERE <condition1> AND <condition2>");

                    condition1 = whereMatch.Groups[1].Value.Trim();
                    condition2 = whereMatch.Groups[2].Value.Trim();
                    i += 2;
                }
                else if (keyword == "SELECT")
                {
                    var selectParts = part.Split(',').Select(p => p.Trim()).ToList();
                    if (selectParts.Count < 2)
                        throw new ArgumentException("Invalid SELECT format. Expected: SELECT <columns1>,<columns2>");

                    columns1 = QueryUtils.ParseColumns(selectParts[0]);
                    columns2 = QueryUtils.ParseColumns(selectParts[1]);
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

            // If no join condition specified, infer from metadata
            if (string.IsNullOrEmpty(joinCondition))
            {
                var table2 = _iotDatabase.GetTable(tableName2);
                var foreignKey = table2?.TableInfo.ForeignKeys.FirstOrDefault(fk => fk.ForeignKeyAttribute?.TableName == tableName1);
                if (foreignKey != null)
                {
                    joinCondition = $"{tableName2}.{foreignKey.Name} = {tableName1}.Id";
                }
                else
                {
                    throw new ArgumentException($"No foreign key relationship found from {tableName2} to {tableName1}. Specify an ON clause.");
                }
            }

            return (tableName1, tableName2, joinCondition, condition1, condition2, columns1, columns2, includes, orderBy, limit, joinCommand);
        }

        // Modified ExecuteQuery to include joinCondition
        private List<QueryResult> ExecuteQuery(dynamic queryBuilder, (string Field, bool IsAscending)? orderBy, int? limit, string joinCondition = null)
        {
            var results = (queryBuilder.Execute(joinCondition) as List<QueryResult>) ?? new List<QueryResult>();

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

    public class QueryBuilder<T1, T2> where T1 : class where T2 : class
    {
        private readonly IotDatabase _iotDatabase;
        private readonly string _tableName1;
        private readonly string _tableName2;
        private readonly Expression<Func<T1, bool>> _predicate1;
        private readonly Expression<Func<T2, bool>> _predicate2;
        private readonly (string ColumnName, string Alias)[] _columns1;
        private readonly (string ColumnName, string Alias)[] _columns2;
        private readonly QueryEngine _queryEngine;
        private readonly List<(string RelatedTableName, Type RelatedType, LambdaExpression Predicate, (string ColumnName, string Alias)[] Columns)> _includes;

        public QueryBuilder(
            IotDatabase iotDatabase,
            string tableName1,
            string tableName2,
            Expression<Func<T1, bool>> predicate1,
            Expression<Func<T2, bool>> predicate2,
            (string ColumnName, string Alias)[] columns1,
            (string ColumnName, string Alias)[] columns2,
            QueryEngine queryEngine)
        {
            _iotDatabase = iotDatabase;
            _tableName1 = tableName1;
            _tableName2 = tableName2;
            _predicate1 = predicate1;
            _predicate2 = predicate2;
            _columns1 = columns1 ?? Array.Empty<(string, string)>();
            _columns2 = columns2 ?? Array.Empty<(string, string)>();
            _queryEngine = queryEngine;
            _includes = new List<(string, Type, LambdaExpression, (string, string)[])>();
        }

        public QueryBuilder<T1, T2> Include<TRelated>(
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

        public List<QueryEngine.QueryResult> Execute(string joinCondition = "")
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

                // Parse join condition (e.g., "Orders.CustomerId = Customers.Id")
                (string table2Prop, string table1Prop)? joinProps = null;
                if (!string.IsNullOrEmpty(joinCondition))
                {
                    var joinMatch = Regex.Match(joinCondition, @"(\w+)\.(\w+)\s*=\s*(\w+)\.(\w+)", RegexOptions.IgnoreCase);
                    if (joinMatch.Success)
                    {
                        var leftTable = joinMatch.Groups[1].Value;
                        var leftProp = joinMatch.Groups[2].Value;
                        var rightTable = joinMatch.Groups[3].Value;
                        var rightProp = joinMatch.Groups[4].Value;

                        if (leftTable.Equals(_tableName2, StringComparison.OrdinalIgnoreCase) && rightTable.Equals(_tableName1, StringComparison.OrdinalIgnoreCase))
                        {
                            joinProps = (leftProp, rightProp);
                        }
                        else if (leftTable.Equals(_tableName1, StringComparison.OrdinalIgnoreCase) && rightTable.Equals(_tableName2, StringComparison.OrdinalIgnoreCase))
                        {
                            joinProps = (rightProp, leftProp);
                        }
                        else
                        {
                            throw new ArgumentException($"Invalid join condition: {joinCondition}");
                        }
                    }
                }
                else
                {
                    // Infer join condition from foreign keys
                    var table2Info = table2.TableInfo;
                    var foreignKey = table2Info.ForeignKeys.FirstOrDefault(fk => fk.ForeignKeyAttribute?.TableName == _tableName1);
                    if (foreignKey != null)
                    {
                        joinProps = (foreignKey.Name, "Id");
                    }
                    else
                    {
                        throw new InvalidOperationException($"No foreign key relationship found from {_tableName2} to {_tableName1}. Specify an ON clause.");
                    }
                }

                // Join records based on foreign key relationship
                foreach (var record1 in records1)
                {
                    var doc1 = BsonMapper.Global.ToDocument(record1);
                    var filteredDoc1 = FilterColumns(doc1, _columns1, mapId: true);
                    var key1 = table1.TableInfo.Id?.PropertyInfo.GetValue(record1);

                    // Find matching records in table2
                    var matchingRecords2 = records2.Where(r2 =>
                    {
                        var propInfo = table2.TableInfo.Type.GetProperty(joinProps.Value.table2Prop, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                        if (propInfo == null)
                            throw new InvalidOperationException($"Property {joinProps.Value.table2Prop} not found on type {table2.TableInfo.Type.Name}.");
                        var key2 = propInfo.GetValue(r2);
                        return key1 != null && key2 != null && key1.Equals(key2);
                    }).ToList();

                    foreach (var record2 in matchingRecords2)
                    {
                        var doc2 = BsonMapper.Global.ToDocument(record2);
                        var filteredDoc2 = FilterColumns(doc2, _columns2, mapId: true);

                        var resultDoc = new BsonDocument
                        {
                            { $"{_tableName1}_Data", filteredDoc1 },
                            { $"{_tableName2}_Data", filteredDoc2 }
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

                            // Check for foreign key relationships to either T1 or T2
                            var foreignKey = relatedTableInfo.ForeignKeys.FirstOrDefault(fk =>
                                fk.ForeignKeyAttribute?.TableName == _tableName1 ||
                                fk.ForeignKeyAttribute?.TableName == _tableName2);

                            if (foreignKey == null)
                                continue;

                            var fkName = foreignKey.Name;
                            var primaryId = foreignKey.ForeignKeyAttribute?.TableName == _tableName1
                                ? table1.TableInfo.Id?.PropertyInfo.GetValue(record1)
                                : table2.TableInfo.Id?.PropertyInfo.GetValue(record2);

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
                            TableName = $"{_tableName1}_{_tableName2}",
                            Data = resultDoc
                        });
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

                string joinedTableName = match.Groups[1].Success ? match.Groups[1].Value : $"{_tableName1}_{_tableName2}";
                string selectColumnsString = match.Groups[2].Value;
                var selectColumns = QueryUtils.ParseColumns(selectColumnsString);

                var joinedResults = new List<QueryEngine.QueryResult>();

                foreach (var result in results)
                {
                    var data1 = result.Data[$"{_tableName1}_Data"] as BsonDocument;
                    var data2 = result.Data[$"{_tableName2}_Data"] as BsonDocument;
                    if (data1 == null || data2 == null)
                        continue;

                    var combinedRecord = new BsonDocument();
                    foreach (var element in data1)
                        combinedRecord[element.Key] = element.Value;
                    foreach (var element in data2)
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
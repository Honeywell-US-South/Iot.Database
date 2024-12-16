using Iot.Database.IotValueUnits;
using Remote.Linq;
using Remote.Linq.Text.Json;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace Iot.Database.Queries
{
    public class QueryBuilder<T>
    {
        private IEnumerable<T> _data;
        private Expression<Func<IQueryable<T>, IQueryable<T>>> _savedQuery;

        public QueryBuilder(IEnumerable<T> data)
        {
            _data = data;
        }

        public IotValue ExecuteQuery(string queryJson)
        {
            // Deserialize the query
            var remoteExpression = DeserializeQuery(queryJson);
            var c = remoteExpression.Compile();
            var result = c.DynamicInvoke(_data.AsQueryable());
        
            IotValue iotValue = new("ExecuteQuery", "Execute Query Result", result, Units.no_unit);
            return iotValue;
        }


        private LambdaExpression DeserializeQuery(string serializedQuery)
        {
            
            JsonSerializerOptions serializerOptions = new JsonSerializerOptions().ConfigureRemoteLinq();
            var remoteExpression = System.Text.Json.JsonSerializer.Deserialize<Remote.Linq.Expressions.LambdaExpression>(serializedQuery, serializerOptions);
       

            return remoteExpression.ToLinqExpression();
        }


        // Method to execute a custom query and return the result
        public TResult Execute<TResult>(Expression<Func<IQueryable<T>, TResult>> query)
        {
            var compiledQuery = query.Compile();
            return compiledQuery(_data.AsQueryable());
        }

        public string Build<TResult>(Expression<Func<IQueryable<T>, TResult>> query)
        {
            JsonSerializerOptions serializerOptions = new JsonSerializerOptions().ConfigureRemoteLinq();
            string json = System.Text.Json.JsonSerializer.Serialize(query.ToRemoteLinqExpression(), serializerOptions);
            return json;
        }

        #region Schema
        public static string SerializeQueryableStructure(IQueryable<T> queryable)
        {
            var type = typeof(T);
            var schema = GenerateSchema(type, new HashSet<Type>());
            return System.Text.Json.JsonSerializer.Serialize(schema, new JsonSerializerOptions { WriteIndented = true });
        }

        private static object GenerateSchema(Type type, HashSet<Type> processedTypes)
        {
            // Avoid circular references
            if (processedTypes.Contains(type))
                return new { Type = type.Name, Properties = "Circular reference detected" };

            processedTypes.Add(type);

            // If the type is a collection, process the element type
            if (type.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
            {
                var elementType = type.GetGenericArguments().First();
                return new
                {
                    Type = $"Collection of {elementType.Name}",
                    ElementType = GenerateSchema(elementType, processedTypes)
                };
            }

            // Handle primitive types
            if (type.IsPrimitive || type == typeof(string) || type == typeof(decimal))
                return new { Type = type.Name };

            // Handle complex types
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(
                    prop => prop.Name,
                    prop => GenerateSchema(prop.PropertyType, processedTypes)
                );

            return new
            {
                Type = type.Name,
                Properties = properties
            };
        }
        #endregion

    }
}

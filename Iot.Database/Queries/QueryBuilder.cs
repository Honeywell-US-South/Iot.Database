using Remote.Linq;
using Remote.Linq.Text.Json;
using System.Linq.Expressions;
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

        public object? ExecuteQuery(string queryJson)
        {
            // Deserialize the query
            var remoteExpression = DeserializeQuery(queryJson);
            var c = remoteExpression.Compile();
            var result = c.DynamicInvoke(_data.AsQueryable());
       
            

            return result;
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

        
    }
}

using Remote.Linq;
using System.Linq.Expressions;

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

        public IEnumerable<T> ExecuteQuery(string queryJson)
        {
            var query = DeserializeQuery(queryJson);
            var compiledQuery = query.Compile();
            return compiledQuery(_data.AsQueryable()).ToList();
        }

       private Expression<Func<IQueryable<T>, IQueryable<T>>> DeserializeQuery(string serializedQuery)
        {
            var options = new JsonSerializerOptions
            {
                Converters = { new ExpressionConverter() }
            };
            var remoteExpression = JsonSerializer.Deserialize<Remote.Linq.Expressions.LambdaExpression>(serializedQuery, options);
            var outerExpression = remoteExpression.ToLinqExpression<Expression<Func<Expression<Func<IQueryable<T>, IQueryable<T>>>>>>();
        
            // Extract the inner expression
            var innerExpression = (Expression<Func<IQueryable<T>, IQueryable<T>>>)outerExpression.Body;
        
            return innerExpression;
        }


        // Method to execute a custom query and return the result
        public TResult Execute<TResult>(Expression<Func<IQueryable<T>, TResult>> query)
        {
            var compiledQuery = query.Compile();
            return compiledQuery(_data.AsQueryable());
        }

        public string Build<TResult>(Expression<Func<IQueryable<T>, TResult>> query)
        {
            var remoteExpression = query.ToRemoteLinqExpression();
            return System.Text.Json.JsonSerializer.Serialize(remoteExpression);
        }

        
    }
}

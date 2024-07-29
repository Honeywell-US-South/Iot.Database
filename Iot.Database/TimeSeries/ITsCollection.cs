
namespace Iot.Database.TimeSeries
{
    internal interface ITsCollection
    {
        List<TsItem> Get(DateTime start, DateTime end);
        List<TsItem> Get(DateTime start, DateTime end, TimeSpan interval);
        void Insert(string entityId, BsonValue data, DateTime? timestamp);
    }
}
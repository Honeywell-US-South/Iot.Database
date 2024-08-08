
namespace Iot.Database.TimeSeries
{
    internal interface ITsCollection
    {
        long Count();
        TsValue? Get(DateTime start, DateTime end);
        TsValue? Get(DateTime start, DateTime end, TimeSpan interval);
        void Insert(IotValue value);
    }
}
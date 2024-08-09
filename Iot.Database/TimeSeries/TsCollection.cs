using Iot.Database;
using Iot.Database.TimeSeries.ValueDeltaT;
using System.Collections.Concurrent;

namespace Iot.Database.TimeSeries;

internal class TsCollection : BaseDatabase, ITsCollection
{
    #region Global Variables
    private readonly string _valueCollection = "BaseValue";
    private readonly string _deltaTCollection = "DeltaT";
    //private bool _processingQueue = false;
    private ConcurrentQueue<TsValue> _entityQueue = new();
    private bool _queueProcessing = false;
    private ILiteCollection<BaseValue> _valueTable;
    #endregion

    #region Constructors
    public TsCollection(string dbPath, string name, string password = "") : base(dbPath, name, password)
    {
        if (!HasIdProperty(typeof(BaseValue)))
        {
            throw new KeyNotFoundException("Table missing Id property with int, long, or Guid data type.");
        }
    }
    #endregion

    #region Base Abstract
    protected override void InitializeDatabase()
    {

        var col = Database.GetCollection<BaseValue>(_valueCollection);
        // Ensure there is an index on the value field to make the query efficient
        col.EnsureIndex(x => x.Value);
    }

    protected override void PerformBackgroundWork(CancellationToken cancellationToken)
    {
        if (_queueProcessing) return;
        else FlushQueue();
    }

    private void FlushQueue()
    {
        lock (SyncRoot)
        {
            _queueProcessing = true;
            try
            {

                const int MaxItemsPerFlush = 5000; // Adjust this value as needed
                int itemsProcessed = 0;

                var collection = Database.GetCollection<BaseValue>(_valueCollection);

                while (_entityQueue.TryDequeue(out var item) && itemsProcessed <= MaxItemsPerFlush)
                {
                    IotValue? ti = item.Series.FirstOrDefault();
                    if (ti == null) continue;
                    var baseValues = collection.Find(x => x.Value.Guid.Equals(item.IotValueGuid, StringComparison.OrdinalIgnoreCase)
                        && x.Value.Values == ti.Values
                        && x.Value.Flags == ti.Flags
                        && x.Value.Name.Equals(ti.Name)
                        && (x.Value.Description.Equals(ti.Description) || (string.IsNullOrEmpty(x.Value.Description) && string.IsNullOrEmpty(ti.Description)))
                        && x.Value.Unit == ti.Unit
                        && x.Value.StrictDataType == ti.StrictDataType).ToList();
                    var baseValue = baseValues.FirstOrDefault(x=>x.Value.Priority == ti.Priority);
                    if (baseValue == null)
                    {
                        baseValue = new BaseValue(ti);
                        var id = collection.Insert(baseValue);
                        if (id.IsNull) baseValue = null;
                    }
                    if (baseValue != null)
                    {
                        (var group, var t) = baseValue.CalculateDeltaT(ti.Timestamp);
                        var deltaTTable = Database.GetCollection<DeltaT>(_deltaTCollection);
                        var dt = deltaTTable.FindOne(x => x.BaseValueId == baseValue.Id && x.Group == group);
                        if (dt == null)
                        {
                            dt = new DeltaT();
                            dt.BaseValueId = baseValue.Id;
                            dt.Group = group;
                            var dtId = deltaTTable.Insert(dt);
                            if (dtId.IsNull) dtId = null;
                        }

                        if (dt != null)
                        {
                            dt.AddTimestamp(t);
                            deltaTTable.Update(dt);
                        }
                    }
                    itemsProcessed++;
                }



            }
            catch (Exception ex) { OnExceptionOccurred(new(ex)); }
            _queueProcessing = false;
        }
    }
    #endregion

    #region C
    /// <summary>
    /// Get document count using property on collection.
    /// </summary>
    public long Count()
    {
        return Database.GetCollection<BaseValue>(_valueCollection).LongCount();
    }
    #endregion
    #region I
    /// <summary>
    /// Insert a new entity to this collection. Document Id must be a new value in collection - Returns document Id
    /// </summary>
    public void Insert(IotValue iotValue)
    {

        TsValue tsValue = new(iotValue);
        _entityQueue.Enqueue(tsValue);
    }
    #endregion

    #region G

    // Function to get data for a specified time frame
    public TsValue? Get(DateTime start, DateTime end)
    {
        
        var Values = Database.GetCollection<BaseValue>(_valueCollection).FindAll().ToList();
        TsValue? tsValue = null;
        foreach (var value in Values)
        {
            if (tsValue == null) tsValue = new TsValue(value.Value);
            (var startGroup, _) = value.CalculateDeltaT(start);
            (var endGroup, _) = value.CalculateDeltaT(end);
            for (var i = startGroup; i <= endGroup; i++)
            {
                var timeDeltas = Database.GetCollection<DeltaT>(_deltaTCollection).FindOne(x => x.BaseValueId == value.Id && x.Group == i);
                if (timeDeltas == null) continue;
                int cumulativeTime = 0;
                foreach (var delta in timeDeltas.GetTimeDeltas())
                {
                    cumulativeTime += delta;
                    var timestamp = value.Start.AddMilliseconds(cumulativeTime);
                    if (timestamp >= start && timestamp <= end)
                    {
                        value.Value.Timestamps[value.Value.Priority -1] = timestamp;
                        tsValue.Series.Add(value.Value);
                    }
                }
            }


        }
        if (tsValue == null) return null;

        TsValue? result = null;
        IotValue? lastValue = null;
        foreach (var iotValue in tsValue.Series.OrderBy(x=> x.Timestamp))
        {
            if (lastValue == null)
            {
                if (result == null) result = new(iotValue);
                else result.Series.Add(iotValue);
                lastValue = iotValue;
            } else
            {
                var v = iotValue;
                v.Values = lastValue.Values;
                v.Timestamps = lastValue.Timestamps;
                v.Values[iotValue.Priority-1] = iotValue.Value;
                v.Timestamps[iotValue.Priority - 1] = iotValue.Timestamp;
                if (result == null) result = new(v);
                else result.Series.Add(v);
                lastValue = v;
            }
        }


        return result;
    }

    // Function to get data for a specified time frame with a set interval, filling missing data
    public TsValue? Get(DateTime start, DateTime end, TimeSpan interval)
    {
        var data = Get(start, end);
        if (data == null) return null;
        TsValue? result = null;
        DateTime current = start;

        while (current <= end)
        {
            var nearestBefore = data.Series.LastOrDefault(d => d.Timestamp <= current);
            var nearestAfter = data.Series.FirstOrDefault(d => d.Timestamp >= current);

            if (nearestBefore != default && nearestAfter != default 
                && nearestBefore.IsNumeric && nearestAfter.IsNumeric
                && !nearestBefore.IsNull && !nearestAfter.IsNull)
            {
                
                if (nearestBefore.Timestamp == nearestAfter.Timestamp)
                {
                    if (result == null) result = new(nearestBefore);
                    else result.Series.Add(nearestBefore);
                }
                else 
                {
                    double t = (current - nearestBefore.Timestamp).TotalMilliseconds / (nearestAfter.Timestamp - nearestBefore.Timestamp).TotalMilliseconds;
                    double interpolatedValue = double.Parse(nearestBefore.Value) * (1 - t) + double.Parse(nearestAfter.Value) * t;
                    
                    var iv = nearestBefore;
                    iv.Values[0] = interpolatedValue.ToString();
                    iv.Timestamps[0] = nearestBefore.Timestamp.AddMilliseconds(t);
                    iv.Flags.Enable(IotValueFlags.ValueInterpolated);
                    if (result == null) result = new(iv);
                    else result.Series.Add(iv);
                }
            }
            else if (nearestBefore != default)
            {
                if (result == null) result = new(nearestBefore);
                else result.Series.Add(nearestBefore);
            }

            current = current.Add(interval);
        }

        return result;
    }



    #endregion

}

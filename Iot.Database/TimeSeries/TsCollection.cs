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

        var dt = Database.GetCollection<DeltaT>(_deltaTCollection);
        dt.EnsureIndex(x => x.TimeDeltas);
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
                    TsItem? ti = item.Series.FirstOrDefault();
                    if (ti == null) continue;
                    var baseValue = collection.FindOne(x => x.EntityId.Equals(item.EntityId, StringComparison.OrdinalIgnoreCase)
                        && x.Value == ti.Value);
                    if (baseValue == null)
                    {
                        baseValue = new BaseValue(item.EntityId, ti.Value, ti.Timestamp);
                        var id = collection.Insert(baseValue);
                        if (id.IsNull) baseValue = null;

                    }
                    if (baseValue != null)
                    {
                        (var group, var t) = baseValue.CalculateDeltaT(ti.Timestamp);
                        var deltaTTable = Database.GetCollection<DeltaT>($"{group}.{baseValue.EntityId}");
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
    public void Insert(string entityId, BsonValue data, DateTime? timestamp)
    {
        DateTime ts = timestamp == null ? DateTime.UtcNow : timestamp.Value.ToUniversalTime();
        TsValue tsValue = new()
        {
            EntityId = entityId
        };
        tsValue.Series.Add(new() { Value = data, Timestamp = ts });
        _entityQueue.Enqueue(tsValue);
    }
    #endregion

    #region G

    // Function to get data for a specified time frame
    public List<TsItem> Get(DateTime start, DateTime end)
    {
        var result = new List<TsItem>();
        var Values = Database.GetCollection<BaseValue>(_valueCollection).FindAll();
        foreach (var value in Values)
        {
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
                    var timestamp = value.Start.AddSeconds(cumulativeTime);
                    if (timestamp >= start && timestamp <= end)
                    {
                        result.Add(new(timestamp, value.Value));
                    }
                }
            }


        }
        return result.OrderBy(t => t.Timestamp).ToList();
    }

    // Function to get data for a specified time frame with a set interval, filling missing data
    public List<TsItem> Get(DateTime start, DateTime end, TimeSpan interval)
    {
        var data = Get(start, end);
        var result = new List<TsItem>();
        DateTime current = start;

        while (current <= end)
        {
            var nearestBefore = data.LastOrDefault(d => d.Timestamp <= current);
            var nearestAfter = data.FirstOrDefault(d => d.Timestamp >= current);

            if (nearestBefore != default && nearestAfter != default && nearestBefore.Value.IsNumber && nearestAfter.Value.IsNumber)
            {
                if (nearestBefore.Timestamp == nearestAfter.Timestamp)
                {
                    result.Add(nearestBefore);
                }
                else
                {
                    double t = (current - nearestBefore.Timestamp).TotalSeconds / (nearestAfter.Timestamp - nearestBefore.Timestamp).TotalSeconds;
                    double interpolatedValue = double.Parse(nearestBefore.Value) * (1 - t) + double.Parse(nearestAfter.Value) * t;
                    result.Add(new(current, interpolatedValue));
                }
            }
            else if (nearestBefore != default)
            {
                result.Add(nearestBefore);
            }

            current = current.Add(interval);
        }

        return result;
    }



    #endregion

}

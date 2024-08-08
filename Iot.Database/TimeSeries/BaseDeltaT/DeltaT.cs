using Iot.Database.Attributes;

namespace Iot.Database.TimeSeries.ValueDeltaT;

public class DeltaT
{
    public int Id { get; set; }
    [TableForeignKey(typeof(BaseValue), TableConstraint.Cascading, RelationshipOneTo.Many,"")]
    public Guid BaseValueId { get; set; }
    public int Group { get; set; }
    public List<int> TimeDeltas { get; set; } = new List<int>();
    public int LastTimestamp { get; set; } = -1;

    public void AddTimestamp(int timestamp)
    {
        if (LastTimestamp == -1)
        {
            TimeDeltas.Add(0); // First timestamp, delta is 0
        }
        else
        {
            int delta = timestamp - LastTimestamp;
            TimeDeltas.Add(delta);
        }
        LastTimestamp = timestamp;
    }

    public List<int> GetTimeDeltas()
    {
        return TimeDeltas.ToList();
    }

    public void SetTimeDeltas(List<int> deltas)
    {
        TimeDeltas = new List<int>(deltas);
    }
}

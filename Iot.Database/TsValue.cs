namespace Iot.Database;

public class TsValue
{
    public string IotValueGuid { get; set; }
    public List<IotValue> Series { get; set; } = new List<IotValue>();

    public TsValue () { }
    public TsValue (IotValue iotValue)
    {
        IotValueGuid = iotValue.Guid;
        Series = new List<IotValue>();
        Series.Add (iotValue);
    }
}

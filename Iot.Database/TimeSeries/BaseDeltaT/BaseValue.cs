namespace Iot.Database.TimeSeries.ValueDeltaT;

internal class BaseValue
{
    public Guid Id { get; set; }
    public IotValue Value { get; set; }
    public DateTime Start { get; set; } = DateTime.UtcNow;


    public BaseValue(IotValue value)
    {
        Value = value;
        Start = value.Timestamp == DateTime.MinValue?DateTime.UtcNow:value.Timestamp;
    }

    public (int Group, int Milliseconds) CalculateDeltaT(DateTime timestamp)
    {
        const int millisecondsPerDay = 24 * 60 * 60 * 1000; // Milliseconds in a day
        const int maxMilliseconds = 20 * millisecondsPerDay; // Maximum milliseconds for 20 days

        // Calculate the total milliseconds since the start
        long totalMilliseconds = (long)(timestamp - Start.ToUniversalTime()).TotalMilliseconds;

        // Determine the group and the milliseconds within the current group
        int group = (int)(totalMilliseconds / maxMilliseconds) + 1;
        int millisecondsInGroup = (int)(totalMilliseconds % maxMilliseconds);

        return (group, millisecondsInGroup);
    }

}

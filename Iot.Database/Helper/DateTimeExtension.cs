namespace Iot.Database.Helper;

public static class DateTimeExtension
{
    public static ulong MsTimeDiff(this DateTime start, DateTime end)
    {
        
        // Calculate the difference
        TimeSpan difference = end - start;

        // Return the difference in milliseconds as ulong
        return (ulong)difference.TotalMilliseconds;
    }
}

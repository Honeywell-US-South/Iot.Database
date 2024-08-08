namespace Iot.Database;

[Flags]
public enum IotValueFlags
{
    None = 0,
    AllowManualOperator = 1 << 0,
    TimeSeries = 1 << 1,
    BlockChain = 1 << 2,
    PasswordValue = 1 << 3,
    LogChange = 1 << 4,
    ValueInterpolated = 1 << 5,
    Priority9Only = 1 << 6, //Control strategy only - this flag disable all other inputs except for 16 (fall back)
}

public static class IotValueFlagsExtensions
{
    public static IotValueFlags Enable(this IotValueFlags flags, IotValueFlags flagToEnable)
    {
        return flags | flagToEnable;
    }

    public static IotValueFlags Disable(this IotValueFlags flags, IotValueFlags flagToDisable)
    {
        return flags & ~flagToDisable;
    }

    public static bool IsEnabled(this IotValueFlags flags, IotValueFlags flagToCheck)
    {
        return (flags & flagToCheck) == flagToCheck;
    }
}
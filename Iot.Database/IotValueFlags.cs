namespace Iot.Database;

[Flags]
public enum IotValueFlags
{
    None = 0,
    AllowManualOperator = 1 << 0,
    TimeSeries = 1 << 1,
    BlockChain = 1 << 2,
    PasswordValue = 1 << 3,
    LogChange = 1 << 4
}

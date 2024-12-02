using Iot.Database.IotValueUnits;

namespace Iot.Database;

public partial struct IotUnit : IEquatable<IotUnit>
{
    public string Name { get; set; }
    public string Symbol { get; set; } 
    public string Group { get; set; }
    public string AsStringFormat { get; set; } = string.Empty;

    public IotUnit() {
    
        this = Units.no_unit;
    }
    public IotUnit(string group, string name, string symbol, string asStringFormat = "")
    {
        Group = group ?? throw new ArgumentNullException(nameof(group));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
        AsStringFormat = asStringFormat ?? string.Empty;
    }

    // Implementing IEquatable<IotUnit>
    public bool Equals(IotUnit other)
    {
        return Name == other.Name && Symbol == other.Symbol && Group == other.Group && AsStringFormat == other.AsStringFormat;
    }

    // Overriding Equals method
    public override bool Equals(object obj)
    {
        if (obj is IotUnit otherUnit)
        {
            return Equals(otherUnit);
        }
        return false;
    }

    // Overriding GetHashCode
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Symbol, Group, AsStringFormat);
    }

    // Implementing equality operator
    public static bool operator ==(IotUnit left, IotUnit right)
    {
        return left.Equals(right);
    }

    // Implementing inequality operator !=
    public static bool operator !=(IotUnit left, IotUnit right)
    {
        return !(left == right);
    }

    // Conversion method
    public double ConvertTo(IotUnit targetUnit, double value)
    {
        if (this.Group != targetUnit.Group)
        {
            throw new InvalidOperationException("Cannot convert between different unit groups.");
        }

        if (this.Symbol == targetUnit.Symbol)
        {
            return value; // No conversion needed
        }

        if (this.Group == Units.Temperature.degrees_fahrenheit.Group)
        {
            return ConvertTemperature(targetUnit, value);
        }
        
        if (this.Group == Units.TemperatureRate.degrees_fahrenheit_per_hour.Group)
        {
            return ConvertTemperatureRate(targetUnit, value);
        }

        throw new NotImplementedException($"Conversion from {this.Symbol} to {targetUnit.Symbol} is not implemented.");
    }

    // Overriding ToString to display the unit's symbol
    public override string ToString()
    {
        return Symbol;
    }
}

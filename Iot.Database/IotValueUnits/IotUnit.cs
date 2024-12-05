using Iot.Database.IotValueUnits;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Iot.Database;

public partial struct IotUnit
{
    public string Name { get; set; }
    public string Symbol { get; set; } 
    public string Group { get; set; }
    public string AsStringFormat { get; set; } = string.Empty;
    public Dictionary<string, string> ConversionRules { get; set; }
    public IotUnit() {
    
        this = Units.no_unit;
    }
    public IotUnit(string group, string name, string symbol, Dictionary<string, string>? conversionRules, string asStringFormat = "")
    {
        Group = group ?? throw new ArgumentNullException(nameof(group));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
        AsStringFormat = asStringFormat ?? string.Empty;
        ConversionRules = conversionRules ?? new();
    }

    public T Convert<T>(T source) where T : IotValue
    {
        if (!source.Unit.Group.Equals(this.Group))
            throw new InvalidCastException($"Cannot convert {source.Unit.Group} to {this.Group}");
        // Get the conversion rule
        if (source.Unit.IsUnit(this))
        {
            return source;
        }
        else if (ConversionRules.TryGetValue(source.Unit.Name, out var rule))
        {
            // Compile the rule into a lambda
            var parameter = Expression.Parameter(typeof(double), "value");
            var expression = DynamicExpressionParser
                .ParseLambda(new[] { parameter }, typeof(double), rule);

            var newVal = (T)(new IotValue());
            newVal.CopyFrom(source);
            newVal.Unit = this;

            // Execute the compiled lambda
            for (int i = 0; i < source.Values.Length; i++)
            {
                if (i != 16 && double.TryParse(source.Values[i], out var value))
                {
                    var normalizedValue = (double)expression.Compile().DynamicInvoke(value);
                    newVal.Values[i] = normalizedValue.ToString();
                }
            }

            // Return normalized IotValue
            return newVal;
        }

        throw new NotSupportedException($"Convertion from {source.Unit.Name} to {this.Name} is not supported.");
    }

    // Implementing IEquatable<IotUnit>
    public bool Equals(IotUnit other)
    {
        return Name == other.Name && Symbol == other.Symbol && Group == other.Group && AsStringFormat == other.AsStringFormat;
    }

    public bool IsUnit(IotUnit other)
    {
        return Name == other.Name && Symbol == other.Symbol && Group == other.Group;
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

    // Overriding ToString to display the unit's symbol
    public override string ToString()
    {
        return Symbol;
    }
}

using Iot.Database.IotValueUnits;

namespace Iot.Database
{
    public partial struct IotUnit
    {
        private double ConvertTemperatureRate(IotUnit targetUnit, double value)
        {
            if (this == Units.TemperatureRate.degrees_celsius_per_hour && targetUnit == Units.TemperatureRate.degrees_fahrenheit_per_hour)
            {
                return value * 9.0 / 5.0; // Celsius/hour to Fahrenheit/hour
            }

            if (this == Units.TemperatureRate.degrees_fahrenheit_per_hour && targetUnit == Units.TemperatureRate.degrees_celsius_per_hour)
            {
                return value * 5.0 / 9.0; // Fahrenheit/hour to Celsius/hour
            }

            if (this == Units.TemperatureRate.degrees_celsius_per_minute && targetUnit == Units.TemperatureRate.degrees_fahrenheit_per_minute)
            {
                return value * 9.0 / 5.0; // Celsius/minute to Fahrenheit/minute
            }

            if (this == Units.TemperatureRate.degrees_fahrenheit_per_minute && targetUnit == Units.TemperatureRate.degrees_celsius_per_minute)
            {
                return value * 5.0 / 9.0; // Fahrenheit/minute to Celsius/minute
            }

            throw new NotImplementedException($"Temperature rate conversion from {this.Symbol} to {targetUnit.Symbol} is not implemented.");
        }
    }
}

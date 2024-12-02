using Iot.Database.IotValueUnits;

namespace Iot.Database
{
    public partial struct IotUnit
    {
        private double ConvertTemperature(IotUnit targetUnit, double value)
        {
            if (this == Units.Temperature.degrees_fahrenheit && targetUnit == Units.Temperature.degrees_celsius)
            {
                return (value - 32) * 5.0 / 9.0; // Fahrenheit to Celsius
            }

            if (this == Units.Temperature.degrees_celsius && targetUnit == Units.Temperature.degrees_fahrenheit)
            {
                return (value * 9.0 / 5.0) + 32; // Celsius to Fahrenheit
            }

            if (this == Units.Temperature.degrees_celsius && targetUnit == Units.Temperature.degrees_kelvin)
            {
                return value + 273.15; // Celsius to Kelvin
            }

            if (this == Units.Temperature.degrees_kelvin && targetUnit == Units.Temperature.degrees_celsius)
            {
                return value - 273.15; // Kelvin to Celsius
            }

            if (this == Units.Temperature.degrees_fahrenheit && targetUnit == Units.Temperature.degrees_kelvin)
            {
                return (value - 32) * 5.0 / 9.0 + 273.15; // Fahrenheit to Kelvin
            }

            if (this == Units.Temperature.degrees_kelvin && targetUnit == Units.Temperature.degrees_fahrenheit)
            {
                return (value - 273.15) * 9.0 / 5.0 + 32; // Kelvin to Fahrenheit
            }

            if (this == Units.Temperature.degrees_rankine && targetUnit == Units.Temperature.degrees_celsius)
            {
                return (value - 491.67) * 5.0 / 9.0; // Rankine to Celsius
            }

            if (this == Units.Temperature.degrees_celsius && targetUnit == Units.Temperature.degrees_rankine)
            {
                return (value * 9.0 / 5.0) + 491.67; // Celsius to Rankine
            }

            if (this == Units.Temperature.degrees_rankine && targetUnit == Units.Temperature.degrees_kelvin)
            {
                return value * 5.0 / 9.0; // Rankine to Kelvin
            }

            if (this == Units.Temperature.degrees_kelvin && targetUnit == Units.Temperature.degrees_rankine)
            {
                return value * 9.0 / 5.0; // Kelvin to Rankine
            }

            if (this == Units.Temperature.degrees_rankine && targetUnit == Units.Temperature.degrees_fahrenheit)
            {
                return value - 459.67; // Rankine to Fahrenheit
            }

            if (this == Units.Temperature.degrees_fahrenheit && targetUnit == Units.Temperature.degrees_rankine)
            {
                return value + 459.67; // Fahrenheit to Rankine
            }

            throw new NotImplementedException($"Temperature conversion from {this.Symbol} to {targetUnit.Symbol} is not implemented.");
        }
    }
}

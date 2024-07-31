using Iot.Database.Extensions;
using Iot.Database.Helper;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Iot.Database;

[Serializable]
public class IotValue
{

    public string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public string?[] Values { get; set; } = new string?[16];
    public DateTime?[] Timestamps { get; set; } = new DateTime?[16];
    public string Unit { get; set; } = Units.no_units;
    public string? StrictDataType { get; set; } = null;
    public IotValueFlags Flags { get; set; } = IotValueFlags.None;

    #region Constructors & Init

    public IotValue()
    {
        InitValues();
    }

    public IotValue(string name, string description)
    {
        InitValues();
        Name = name;
        Description = description;
        AllowManualOperator = true;
    }

    public IotValue (string name, string description, object? value, string unit)
    {
        InitValues();
        Name = name;
        Description = description;
        SetValue(16, value);
        Unit = unit;
        AllowManualOperator = true;
    }
    public IotValue(string name, string description, object? value, string unit, bool isPasswordValue, bool allowManualOperator, bool timeSeries, bool blockChain, bool logChange)
    {
        InitValues();
        Name = name;
        Description = description;
        Unit = unit;
        AllowManualOperator = allowManualOperator;
        TimeSeries = timeSeries;
        BlockChain = blockChain;
        LogChange = logChange;

        if (isPasswordValue && value?.GetType() != typeof(string))
        {

            throw new InvalidConstraintException("Password value type is not of type string.");
        } else if (isPasswordValue && value != null)
        {
            SetPassword(16, value.ToString());
        }
        else
        {
            SetValue(16, value);
        }
    }

    private void InitValues()
    {
        for (int i = 0; i < Values.Length; i++)
        {
            Values[i] = null;
            Timestamps[i] = null;
        }
    }

    [JsonIgnore]
    [BsonIgnore]
    private static JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    public string SerializeToJson()
    {
        return System.Text.Json.JsonSerializer.Serialize(this, _serializerOptions);
    }
    
    public virtual IotValue Copy()
    {
        return ObjectHelper.DeepCopy(this);
    }

    #endregion

    
    private bool SetRawValue(int index, string? value)
    {
        if (index < 0 && index >= Values.Length) return false;

        if (AllowManualOperator && index == 7)
        {
            Values[index] = null;
            Timestamps[index] = null;
            //HasValues[index] = false;
            return false; //manual operator 
        }

        Values[index] = value;
        Timestamps[index] = DateTime.UtcNow;
        //HasValues[index] = value != null;

        return true;
    }

    [JsonIgnore]
    [BsonIgnore]
    public System.Type? DataType
    {
        get
        {
            if (StrictDataType == null) return null;
            Type? type = Type.GetType(StrictDataType);
            return type;
        }

    }

    /// <summary>
    /// Null type accept all value types. 
    /// </summary>
    /// <param name="type"></param>
    public void SetStrictDataType(System.Type? type)
    {
        StrictDataType = type?.AssemblyQualifiedName;
    }

    #region Flags
    [JsonIgnore]
    [BsonIgnore]
    public bool IsAllowManualOperator
    {
        get { return AllowManualOperator;  }
        set { AllowManualOperator = value; }
    }

    [JsonIgnore]
    [BsonIgnore]
    public bool AllowManualOperator
    {
        get => Flags.IsEnabled(IotValueFlags.AllowManualOperator);
        set => Flags = value ? Flags.Enable(IotValueFlags.AllowManualOperator) : Flags.Disable(IotValueFlags.AllowManualOperator);
    }

    [JsonIgnore]
    [BsonIgnore]
    public bool IsTimeSeries
    {
        get { return TimeSeries; }
        set { TimeSeries = value; }
    }

    [JsonIgnore]
    [BsonIgnore]
    public bool TimeSeries
    {
        get => Flags.IsEnabled(IotValueFlags.TimeSeries);
        set => Flags = value ? Flags.Enable(IotValueFlags.TimeSeries) : Flags.Disable(IotValueFlags.TimeSeries);
    }

    [JsonIgnore]
    [BsonIgnore]
    public bool IsBlockChain
    {
        get { return BlockChain; }
        set { BlockChain = value; }
    }

    [JsonIgnore]
    [BsonIgnore]
    public bool BlockChain
    {
        get => Flags.IsEnabled(IotValueFlags.BlockChain);
        set => Flags = value ? Flags.Enable(IotValueFlags.BlockChain) : Flags.Disable(IotValueFlags.BlockChain);
    }

    [JsonIgnore]
    [BsonIgnore]
    public bool IsPasswordValue
    {
        get { return PasswordValue; }
        set { PasswordValue = value; }
    }

    [JsonIgnore]
    [BsonIgnore]
    public bool PasswordValue
    {
        get => Flags.IsEnabled(IotValueFlags.PasswordValue);
        set => Flags = value ? Flags.Enable(IotValueFlags.PasswordValue) : Flags.Disable(IotValueFlags.PasswordValue);
    }

    [JsonIgnore]
    [BsonIgnore]
    public bool IsLogChange
    {
        get { return LogChange; }
        set { LogChange = value; }
    }

    [JsonIgnore]
    [BsonIgnore]
    public bool LogChange
    {
        get => Flags.IsEnabled(IotValueFlags.LogChange);
        set => Flags = value ? Flags.Enable(IotValueFlags.LogChange) : Flags.Disable(IotValueFlags.LogChange);
    }

    #endregion

    [JsonIgnore]
    [BsonIgnore]
    public string? Value
    {
        get
        {
            for (int i = 0; i < Values.Length; i++)
            {
                if (Values[i] != null) return Values[i];
            }

            return null;
        }
        
    }

    [JsonIgnore]
    [BsonIgnore]
    public int Priority
    {
        get
        {
            for (int i = 0; i < Values.Length; i++)
            {
                if (Values[i] != null) return i + 1;
            }
            return 0;
        }
        
    }

    [JsonIgnore]
    [BsonIgnore]
    public DateTime Timestamp
    {
        get
        {
            for (int i = 0; i < Timestamps.Length; i++)
            {
                if (Timestamps[i] != null) return Timestamps[i]??DateTime.MinValue;
            }
            return DateTime.MinValue;
        }
    }

    #region Check

    /// <summary>
    /// Check if value is Null
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public bool IsNull
    {
        get
        {
            foreach (var item in Values)
            {
                if (item != null) return false;

            }
            return true;
        }
    }

    [JsonIgnore]
    [BsonIgnore]
    public bool IsStrictDataType
    {
        get
        {

            try
            {
                var type = DataType;
                return type != null;
            }
            catch { }
            return false;
        }
    }
    /// <summary>
    /// Check if value is Guid type
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public bool IsGuid => System.Guid.TryParse(Value, out _);

    /// <summary>
    /// Check if value is Numeric type
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public bool IsNumeric => double.TryParse(Value, out _);

    /// <summary>
    /// Check if value is Double type
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public bool IsDouble => double.TryParse(Value, out _);

    /// <summary>
    /// Check if value is Boolean type
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public bool IsBoolean => bool.TryParse(Value, out _);

    /// <summary>
    /// Check if value is DateTime type
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public bool IsDateTime => DateTime.TryParse(Value, out _);

    /// <summary>
    /// Check if value is Integer type
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public bool IsInteger => int.TryParse(Value, out _);

    /// <summary>
    /// Check if value is Long type
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public bool IsLong => long.TryParse(Value, out _);

    /// <summary>
    /// Check if value is Float type
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public bool IsFloat => float.TryParse(Value, out _);

    /// <summary>
    /// Check if value is Decimal type
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public bool IsDecimal => decimal.TryParse(Value, out _);

    /// <summary>
    /// Check if value is Char type
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public bool IsChar => Value?.Length == 1 &&  char.TryParse(Value, out _);

    /// <summary>
    /// Check if value is Json type
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public bool IsJson
    {
        get
        {
            var strInput = Value;
            if (string.IsNullOrWhiteSpace(strInput))
            {
                return false;
            }

            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || // For object
                (strInput.StartsWith("[") && strInput.EndsWith("]")))   // For array
            {
                try
                {
                    var obj = JsonDocument.Parse(strInput);
                    return true;
                }
                catch (JsonException)
                {
                    return false;
                }
                catch (Exception) // Some other exception
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Is value a string (not guid, not numeric, not char, and not json)
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public bool IsString => !IsGuid && !IsNumeric && !IsChar && !IsJson;
    
    /// <summary>
    /// Check if value is T type
    /// </summary>
    public bool IsObject<T>() where T : class
    {
        if (string.IsNullOrEmpty(Value))
        {
            return false;
        }

        try
        {
            var obj = System.Text.Json.JsonSerializer.Deserialize<T>(Value);
            return obj != null;
        }
        catch (JsonException)
        {
            return false;
        }
    }

    /// <summary>
    /// Checks if the given string is a valid SHA-256 hash.
    /// </summary>
    /// <returns>True if the string is a valid SHA-256 hash; otherwise, false.</returns>
    [JsonIgnore]
    [BsonIgnore]
    public bool IsHash
    {
        get
        {
            if (string.IsNullOrEmpty(Value))
            {
                return false;
            }

            // Regular expression to match a 64-character hexadecimal string
            var regex = new Regex("^[a-fA-F0-9]{64}$");
            return regex.IsMatch(Value);
        }
    }

    /// <summary>
    /// Checks if the given string is a valid SHA-256 hash password.
    /// </summary>
    /// <param name="hash">The string to check.</param>
    /// <returns>True if the string is a valid SHA-256 hash password; otherwise, false.</returns>
    [JsonIgnore]
    [BsonIgnore]
    public bool IsPasswordHash
    {
        get
        {
            return IsHash;
        }
    }
    #endregion

    #region Set
    /// <summary>
    /// Priority 1: Manual Operator Override (Highest priority)
    /// Priority 2: Critical Equipment Control
    /// Priority 3: Life Safety
    /// Priority 4: Fire Safety
    /// Priority 5: Emergency
    /// Priority 6: Safety
    /// Priority 7: Available
    /// Priority 8: Manual Operator
    /// Priority 9: Control Strategy
    /// Priority 10: Available
    /// Priority 11: Available
    /// Priority 12: Available
    /// Priority 13: Available
    /// Priority 14: Available
    /// Priority 15: Default Value Set
    /// Priority 16: Default or Fallback Value(Lowest priority)
    /// </summary>
    /// <param name="priority">int</param>
    /// <param name="value">object</param>
    /// <returns>true/false</returns>
    public bool SetValue(int priority, object? value)
    {
        ValidateType(value);
        int index = priority - 1;

        return SetRawValue(index, ToStringValue(value));

    }

    /// <summary>
    /// Priority 1: Manual Operator Override (Highest priority)
    /// Priority 2: Critical Equipment Control
    /// Priority 3: Life Safety
    /// Priority 4: Fire Safety
    /// Priority 5: Emergency
    /// Priority 6: Safety
    /// Priority 7: Available
    /// Priority 8: Manual Operator
    /// Priority 9: Control Strategy
    /// Priority 10: Available
    /// Priority 11: Available
    /// Priority 12: Available
    /// Priority 13: Available
    /// Priority 14: Available
    /// Priority 15: Default Value Set
    /// Priority 16: Default or Fallback Value(Lowest priority)
    /// </summary>
    /// <param name="priority">int</param>
    /// <param name="value">string</param>
    /// <returns>true/false</returns>
    public bool SetValue(int priority, string? value)
    {
        ValidateType(value);
        int index = priority - 1;
        return SetRawValue(index, ToStringValue(value));
    }

    /// <summary>
    /// Serialize object and set value at priority
    /// Priority 1: Manual Operator Override (Highest priority)
    /// Priority 2: Critical Equipment Control
    /// Priority 3: Life Safety
    /// Priority 4: Fire Safety
    /// Priority 5: Emergency
    /// Priority 6: Safety
    /// Priority 7: Available
    /// Priority 8: Manual Operator
    /// Priority 9: Control Strategy
    /// Priority 10: Available
    /// Priority 11: Available
    /// Priority 12: Available
    /// Priority 13: Available
    /// Priority 14: Available
    /// Priority 15: Default Value Set
    /// Priority 16: Default or Fallback Value(Lowest priority)
    /// </summary>
    /// <param name="priority">int</param>
    /// <param name="value">class T</param>
    /// <returns>true/false</returns>
    public bool SetObject<T>(int priority, T? value) where T : class
    {
        ValidateType(value);
        int index = priority - 1;
        return SetRawValue(index, ToStringValue(value));
    }

    /// <summary>
    /// Serialize object and set value at priority
    /// Priority 1: Manual Operator Override (Highest priority)
    /// Priority 2: Critical Equipment Control
    /// Priority 3: Life Safety
    /// Priority 4: Fire Safety
    /// Priority 5: Emergency
    /// Priority 6: Safety
    /// Priority 7: Available
    /// Priority 8: Manual Operator
    /// Priority 9: Control Strategy
    /// Priority 10: Available
    /// Priority 11: Available
    /// Priority 12: Available
    /// Priority 13: Available
    /// Priority 14: Available
    /// Priority 15: Default Value Set
    /// Priority 16: Default or Fallback Value(Lowest priority)
    /// </summary>
    /// <param name="priority">int</param>
    /// <param name="password">raw password string</param>
    /// <returns>true/false</returns>
    public bool SetPassword(int priority, string? password)
    {
        ValidateType(password);
        int index = priority - 1;
        IsPasswordValue = true;
        return SetRawValue(index, ToPasswordHash(password));

    }

    /// <summary>
    /// Priority 1: Manual Operator Override (Highest priority)
    /// </summary>
    /// <param name="value"></param>
    /// <returns>true/fals</returns>
    public bool SetValue01ManualOperatorOverride(object? value)
    {
        ValidateType(value);
        return SetValue(1, value);
    }

    /// <summary>
    /// Priority 2: Critical Equipment Control
    /// </summary>
    /// <param name="value"></param>
    /// <returns>true/fals</returns>
    public bool SetValue02Critica(object? value) => SetValue(2, value);


    /// <summary>
    /// Priority 3: Life Safety
    /// </summary>
    /// <param name="value"></param>
    /// <returns>true/fals</returns>
    public bool SetValue03LifeSafety(object? value) => SetValue(3, value);


    /// <summary>
    /// Priority 4: Fire Safety
    /// </summary>
    /// <param name="value"></param>
    /// <returns>true/fals</returns>
    public bool SetValue04FireSafety(object? value) => SetValue(4, value);


    /// <summary>
    /// Priority 5: Emergency
    /// </summary>
    /// <param name="value"></param>
    /// <returns>true/fals</returns>
    public bool SetValue05Emergency(object? value) => SetValue(5, value);


    /// <summary>
    /// Priority 6: Safety
    /// </summary>
    /// <param name="value"></param>
    /// <returns>true/fals</returns>
    public bool SetValue06Safety(object? value) => SetValue(6, value);


    /// <summary>
    /// Priority 7: Free
    /// </summary>
    /// <param name="value"></param>
    /// <returns>true/fals</returns>
    public bool SetValue07Free(object? value) => SetValue(7, value);


    /// <summary>
    /// Priority 8: Manual Operator
    /// </summary>
    /// <param name="value"></param>
    /// <returns>true/fals</returns>
    public bool SetValue08ManualOperator(object? value) => SetValue(8, value);


    /// <summary>
    /// Priority 9: Control Strategy
    /// </summary>
    /// <param name="value"></param>
    /// <returns>true/fals</returns>
    public bool SetValue09ControlStrategy(object? value) => SetValue(9, value);


    /// <summary>
    /// Priority 10: Free
    /// </summary>
    /// <param name="value"></param>
    /// <returns>true/fals</returns>
    public bool SetValue10Free(object? value) => SetValue(10, value);


    /// <summary>
    /// Priority 11: Free
    /// </summary>
    /// <param name="value"></param>
    /// <returns>true/fals</returns>
    public bool SetValue11Free(object? value) => SetValue(11, value);


    /// <summary>
    /// Priority 12: Free
    /// </summary>
    /// <param name="value"></param>
    /// <returns>true/fals</returns>
    public bool SetValue12Free(object? value) => SetValue(12, value);


    /// <summary>
    /// Priority 13: Free
    /// </summary>
    /// <param name="value"></param>
    /// <returns>true/fals</returns>
    public bool SetValue13Free(object? value) => SetValue(13, value);


    /// <summary>
    /// Priority 14: Free
    /// </summary>
    /// <param name="value"></param>
    /// <returns>true/fals</returns>
    public bool SetValue14Free(object? value) => SetValue(14, value);


    /// <summary>
    /// Priority 15: Default Value Set
    /// </summary>
    /// <param name="value"></param>
    /// <returns>true/fals</returns>
    public bool SetValue15Default(object? value) => SetValue(15, value);


    /// <summary>
    /// Priority 16: Default or Fallback Value (Lowest priority)
    /// </summary>
    /// <param name="value"></param>
    /// <returns>true/fals</returns>
    public bool SetValue16DefaultFallback(object? value) => SetValue(16, value);

    #endregion

    #region Get

    /// <summary>
    /// Get the highest prioriety. Zero means value is not set.
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public int AsPriority
    {
        get
        {
            return Priority;
        }
    }

    /// <summary>
    /// Get value as data type
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public System.Type? AsType
    {
        get
        {
            if (Value == null) return null;

            if (IsGuid) return typeof(Guid);
            if (IsBoolean) return typeof(bool);
            if (IsDateTime) return typeof(DateTime);
            if (IsInteger) return typeof(int);
            if (IsLong) return typeof(long);
            if (IsDouble) return typeof(double);
            if (IsFloat) return typeof(float);
            if (IsDecimal) return typeof(decimal);
            if (IsChar) return typeof(char);

            try
            {
                var obj = System.Text.Json.JsonSerializer.Deserialize<object>(Value);
                if (obj != null) return obj.GetType();
            }
            catch (JsonException) { }

            return typeof(string); // Default to string if no other type matches
        }
    }

    /// <summary>
    /// Get value as boolean. Return null if Value cannot parse as boolean.
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public bool? AsBoolean
    {
        get
        {
            if (bool.TryParse(Value, out bool result))
            {
                return result;
            }
            return null;
        }
    }

    /// <summary>
    /// Get value as DateTime. Return null if Value cannot parse as DateTime.
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public DateTime? AsDateTime
    {
        get
        {
            if (DateTime.TryParse(Value, out DateTime result))
            {
                return result;
            }
            return null;
        }
    }
    /// <summary>
    /// Get value as integer. Return null if Value cannot parse as integer.
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public int? AsInteger
    {
        get
        {
            if (int.TryParse(Value, out int result))
            {
                return result;
            }
            return null;
        }
    }

    /// <summary>
    /// Get value as double. Return null if Value cannot parse as double.
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public double? AsDouble
    {
        get
        {
            if (double.TryParse(Value, out double result))
            {
                return result;
            }
            return null;
        }
    }

    /// <summary>
    /// Get value as long. Return null if Value cannot parse as long.
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public long? AsLong
    {
        get
        {
            if (long.TryParse(Value, out long result))
            {
                return result;
            }
            return null;
        }
    }

    /// <summary>
    /// Get value as Guid. Return null if Value cannot parse as Guid.
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public Guid? AsGuid
    {
        get
        {
            if (System.Guid.TryParse(Value, out Guid result))
            {
                return result;
            }
            return null;
        }
    }

    /// <summary>
    /// Get value as float. Return null if Value cannot parse as float.
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public float? AsFloat => float.TryParse(Value, out float result) ? result : (float?)null;

    /// <summary>
    /// Get value as decimal. Return null if Value cannot parse as decimal.
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public decimal? AsDecimal => decimal.TryParse(Value, out decimal result) ? result : (decimal?)null;

    /// <summary>
    /// Get value as dhar. Return null if Value cannot parse as char.
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public char? AsChar => char.TryParse(Value, out char result) ? result : (char?)null;
    /// <summary>
    /// Get value as string. Return null if Value cannot parse as string.
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public string? AsString
    {
        get
        {
            return Value;
        }
    }


    /// <summary>
    /// Get value as a deserialized object. Return null if Value cannot deserialize.
    /// </summary>
    public T? AsObject<T>() where T : class
    {
        if (string.IsNullOrEmpty(Value))
        {
            return null;
        }

        try
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(Value);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error deserializing JSON: {ex.Message}");

        }
        return null;

    }

    #endregion

    #region Helper

    /// <summary>
    /// Check value datatype. If DataType is Attribute, all types are accepted.
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="ArgumentException"></exception>
    private void ValidateType(object? value)
    {
        if (value == null) return;
        if (StrictDataType == null) return;
        if (value.GetType() != DataType)
        {
            throw new ArgumentException($"Invalid data type. Expected strict data type of {StrictDataType}, but got {value.GetType()}.");
        }
    }

    /// <summary>
    /// Generates a SHA-256 hash for the given password.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>The hashed password as a hexadecimal string.</returns>
    public static string? ToPasswordHash(string? password)
    {
        //null password is null
        if (password == null) return null;

        //empty password is no password. Different than null
        if (string.IsNullOrEmpty(password))
        {
            return string.Empty;
        }

        using (SHA256 sha256 = SHA256.Create())
        {
            // Convert the password string to a byte array
            byte[] bytes = Encoding.UTF8.GetBytes(password);

            // Compute the hash
            byte[] hashBytes = sha256.ComputeHash(bytes);

            // Convert the hash byte array to a hexadecimal string
            StringBuilder hashStringBuilder = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                hashStringBuilder.Append(b.ToString("x2"));
            }

            return hashStringBuilder.ToString();
        }
    }

    /// <summary>
    /// Convert object to string
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string? ToStringValue(object? value)
    {
        if (value == null) return null;
        return value switch
        {
            int intValue => intValue.ToString(),
            double doubleValue => doubleValue.ToString(),
            float floatValue => floatValue.ToString(),
            decimal decimalValue => decimalValue.ToString(),
            bool boolValue => boolValue.ToString(),
            DateTime dateTimeValue => dateTimeValue.ToString("o"), // ISO 8601 format
            Guid guidValue => guidValue.ToString(),
            char charValue => charValue.ToString(),
            byte[] byteArrayValue => Convert.ToBase64String(byteArrayValue),
            Type typeValue => typeValue.Name, // Handle System.RuntimeType
            _ when value?.GetType().FullName?.Equals("system.string", StringComparison.OrdinalIgnoreCase)==true => value?.ToString()??string.Empty,
            _ when value?.GetType().IsClass == true => System.Text.Json.JsonSerializer.Serialize(value),
            _ => value?.ToString() ?? string.Empty,
        };
    }


    #endregion

    #region Engineering Units
    
    public static class Units
    {
        public static class All
        {
            //this class includes all the unit in all categories. if add to this class, make sure add to the correct category too.
            public static class Name
            {
                // Acceleration
                public const string meters_per_second_per_second = "meters_per_second_per_second";
                public const string standard_gravity = "standard_gravity";
                
                // Angular
                public const string degrees_angular = "degrees_angular";
                public const string radians = "radians";
                public const string radians_per_second = "radians_per_second";
                public const string revolutions_per_minute = "revolutions_per_minute";

                // Area
                public const string square_centimeters = "square_centimeters";
                public const string square_feet = "square_feet";
                public const string square_inches = "square_inches";
                public const string square_meters = "square_meters";

                // Capacitance
                public const string farads = "farads";
                public const string microfarads = "microfarads";
                public const string nanofarads = "nanofarads";
                public const string picofarads = "picofarads";

                // Concentration
                public const string mole_percent = "mole_percent";
                public const string parts_per_billion = "parts_per_billion";
                public const string parts_per_million = "parts_per_million";
                public const string percent = "percent";
                public const string percent_obscuration_per_foot = "percent_obscuration_per_foot";
                public const string percent_obscuration_per_meter = "percent_obscuration_per_meter";
                public const string percent_per_second = "percent_per_second";
                public const string per_mille = "per_mille";

                // Currency
                public const string afghan_afghani = "afghan_afghani";
                public const string albanian_lek = "albanian_lek";
                public const string algerian_dinar = "algerian_dinar";
                public const string angolan_kwanza = "angolan_kwanza";
                public const string argentine_peso = "argentine_peso";
                public const string armenian_dram = "armenian_dram";
                public const string aruban_florin = "aruban_florin";
                public const string australian_dollar = "australian_dollar";
                public const string azerbaijani_manat = "azerbaijani_manat";
                public const string bahamian_dollar = "bahamian_dollar";
                public const string bahraini_dinar = "bahraini_dinar";
                public const string bangladeshi_taka = "bangladeshi_taka";
                public const string barbadian_dollar = "barbadian_dollar";
                public const string belarusian_ruble = "belarusian_ruble";
                public const string belize_dollar = "belize_dollar";
                public const string bermudian_dollar = "bermudian_dollar";
                public const string bhutanese_ngultrum = "bhutanese_ngultrum";
                public const string bolivian_boliviano = "bolivian_boliviano";
                public const string bosnia_and_herzegovina_convertible_mark = "bosnia_and_herzegovina_convertible_mark";
                public const string botswana_pula = "botswana_pula";
                public const string brazilian_real = "brazilian_real";
                public const string brunei_dollar = "brunei_dollar";
                public const string bulgarian_lev = "bulgarian_lev";
                public const string burundian_franc = "burundian_franc";
                public const string cape_verdean_escudo = "cape_verdean_escudo";
                public const string cambodian_riel = "cambodian_riel";
                public const string canadian_dollar = "canadian_dollar";
                public const string cayman_islands_dollar = "cayman_islands_dollar";
                public const string central_african_cfa_franc = "central_african_cfa_franc";
                public const string chilean_peso = "chilean_peso";
                public const string chinese_yuan = "chinese_yuan";
                public const string colombian_peso = "colombian_peso";
                public const string comorian_franc = "comorian_franc";
                public const string congolese_franc = "congolese_franc";
                public const string costa_rican_colon = "costa_rican_colon";
                public const string croatian_kuna = "croatian_kuna";
                public const string cuban_convertible_peso = "cuban_convertible_peso";
                public const string cuban_peso = "cuban_peso";
                public const string czech_koruna = "czech_koruna";
                public const string danish_krone = "danish_krone";
                public const string djiboutian_franc = "djiboutian_franc";
                public const string dominican_peso = "dominican_peso";
                public const string east_caribbean_dollar = "east_caribbean_dollar";
                public const string egyptian_pound = "egyptian_pound";
                public const string eritrean_nakfa = "eritrean_nakfa";
                public const string ethiopian_birr = "ethiopian_birr";
                public const string euro = "euro";
                public const string falkland_islands_pound = "falkland_islands_pound";
                public const string fiji_dollar = "fiji_dollar";
                public const string gambian_dalasi = "gambian_dalasi";
                public const string georgian_lari = "georgian_lari";
                public const string ghanaian_cedi = "ghanaian_cedi";
                public const string gibraltar_pound = "gibraltar_pound";
                public const string guatemalan_quetzal = "guatemalan_quetzal";
                public const string guinean_franc = "guinean_franc";
                public const string guyanese_dollar = "guyanese_dollar";
                public const string haitian_gourde = "haitian_gourde";
                public const string honduran_lempira = "honduran_lempira";
                public const string hong_kong_dollar = "hong_kong_dollar";
                public const string hungarian_forint = "hungarian_forint";
                public const string icelandic_krona = "icelandic_krona";
                public const string indian_rupee = "indian_rupee";
                public const string indonesian_rupiah = "indonesian_rupiah";
                public const string iranian_rial = "iranian_rial";
                public const string iraqi_dinar = "iraqi_dinar";
                public const string israeli_new_shekel = "israeli_new_shekel";
                public const string jamaican_dollar = "jamaican_dollar";
                public const string japanese_yen = "japanese_yen";
                public const string jordanian_dinar = "jordanian_dinar";
                public const string kazakhstani_tenge = "kazakhstani_tenge";
                public const string kenyan_shilling = "kenyan_shilling";
                public const string kuwaiti_dinar = "kuwaiti_dinar";
                public const string kyrgyzstani_som = "kyrgyzstani_som";
                public const string lao_kip = "lao_kip";
                public const string lebanese_pound = "lebanese_pound";
                public const string lesotho_loti = "lesotho_loti";
                public const string liberian_dollar = "liberian_dollar";
                public const string libyan_dinar = "libyan_dinar";
                public const string macanese_pataca = "macanese_pataca";
                public const string malagasy_ariary = "malagasy_ariary";
                public const string malawian_kwacha = "malawian_kwacha";
                public const string malaysian_ringgit = "malaysian_ringgit";
                public const string maldivian_rufiyaa = "maldivian_rufiyaa";
                public const string mauritanian_ouguiya = "mauritanian_ouguiya";
                public const string mauritian_rupee = "mauritian_rupee";
                public const string mexican_peso = "mexican_peso";
                public const string moldovan_leu = "moldovan_leu";
                public const string mongolian_togrog = "mongolian_togrog";
                public const string moroccan_dirham = "moroccan_dirham";
                public const string mozambican_metical = "mozambican_metical";
                public const string myanmar_kyat = "myanmar_kyat";
                public const string namibian_dollar = "namibian_dollar";
                public const string nepalese_rupee = "nepalese_rupee";
                public const string netherlands_antillean_guilder = "netherlands_antillean_guilder";
                public const string new_taiwan_dollar = "new_taiwan_dollar";
                public const string new_zealand_dollar = "new_zealand_dollar";
                public const string nicaraguan_cordoba = "nicaraguan_cordoba";
                public const string nigerian_naira = "nigerian_naira";
                public const string north_korean_won = "north_korean_won";
                public const string norwegian_krone = "norwegian_krone";
                public const string omani_rial = "omani_rial";
                public const string pakistani_rupee = "pakistani_rupee";
                public const string panamanian_balboa = "panamanian_balboa";
                public const string papua_new_guinean_kina = "papua_new_guinean_kina";
                public const string paraguayan_guarani = "paraguayan_guarani";
                public const string peruvian_sol = "peruvian_sol";
                public const string philippine_peso = "philippine_peso";
                public const string polish_zloty = "polish_zloty";
                public const string qatar_riyal = "qatar_riyal";
                public const string romanian_leu = "romanian_leu";
                public const string russian_ruble = "russian_ruble";
                public const string rwandan_franc = "rwandan_franc";
                public const string saint_helena_pound = "saint_helena_pound";
                public const string samoan_tala = "samoan_tala";
                public const string saudi_riyal = "saudi_riyal";
                public const string serbian_dinar = "serbian_dinar";
                public const string seychellois_rupee = "seychellois_rupee";
                public const string sierra_leonean_leone = "sierra_leonean_leone";
                public const string singapore_dollar = "singapore_dollar";
                public const string solomon_islands_dollar = "solomon_islands_dollar";
                public const string somali_shilling = "somali_shilling";
                public const string south_african_rand = "south_african_rand";
                public const string south_korean_won = "south_korean_won";
                public const string south_sudanese_pound = "south_sudanese_pound";
                public const string sri_lankan_rupee = "sri_lankan_rupee";
                public const string sudanese_pound = "sudanese_pound";
                public const string surinamese_dollar = "surinamese_dollar";
                public const string swazi_lilangeni = "swazi_lilangeni";
                public const string swedish_krona = "swedish_krona";
                public const string swiss_franc = "swiss_franc";
                public const string syrian_pound = "syrian_pound";
                public const string taiwanese_dollar = "taiwanese_dollar";
                public const string tajikistani_somoni = "tajikistani_somoni";
                public const string tanzanian_shilling = "tanzanian_shilling";
                public const string thai_baht = "thai_baht";
                public const string tonga_paanga = "tonga_paanga";
                public const string trinidad_and_tobago_dollar = "trinidad_and_tobago_dollar";
                public const string tunisian_dinar = "tunisian_dinar";
                public const string turkish_lira = "turkish_lira";
                public const string turkmenistani_manat = "turkmenistani_manat";
                public const string ugandan_shilling = "ugandan_shilling";
                public const string ukrainian_hryvnia = "ukrainian_hryvnia";
                public const string united_arab_emirates_dirham = "united_arab_emirates_dirham";
                public const string uruguayan_peso = "uruguayan_peso";
                public const string uzbekistani_som = "uzbekistani_som";
                public const string vanuatu_vatu = "vanuatu_vatu";
                public const string venezuelan_bolivar = "venezuelan_bolivar";
                public const string vietnamese_dong = "vietnamese_dong";
                public const string yemeni_rial = "yemeni_rial";
                public const string zambian_kwacha = "zambian_kwacha";
                public const string zimbabwean_dollar = "zimbabwean_dollar";

                // DataRate
                public const string bits_per_second = "bits_per_second";
                public const string gigabits_per_second = "gigabits_per_second";
                public const string kilobits_per_second = "kilobits_per_second";
                public const string megabits_per_second = "megabits_per_second";

                // DataStorage
                public const string bytes = "bytes";
                public const string exabytes = "exabytes";
                public const string gigabytes = "gigabytes";
                public const string kilobytes = "kilobytes";
                public const string megabytes = "megabytes";
                public const string petabytes = "petabytes";
                public const string terabytes = "terabytes";
                public const string yottabytes = "yottabytes";
                public const string zettabytes = "zettabytes";

                // ElectricCharge
                public const string ampere_hours = "ampere_hours";
                public const string coulombs = "coulombs";

                // ElectricPotential
                public const string kilovolts = "kilovolts";
                public const string millivolts = "millivolts";
                public const string volts = "volts";

                // ElectricResistance
                public const string kilohms = "kilohms";
                public const string megohms = "megohms";
                public const string milliohms = "milliohms";
                public const string ohms = "ohms";

                // Electrical
                public const string amperes = "amperes";
                public const string amperes_per_meter = "amperes_per_meter";
                public const string amperes_per_square_meter = "amperes_per_square_meter";
                public const string ampere_square_meters = "ampere_square_meters";
                public const string bars = "bars";
                public const string decibels = "decibels";
                public const string decibels_millivolt = "decibels_millivolt";
                public const string decibels_volt = "decibels_volt";
                public const string degrees_phase = "degrees_phase";
                //public const string farads = "farads";
                public const string henrys = "henrys";
                //public const string kilohms = "kilohms";
                public const string kilovolt_amperes = "kilovolt_amperes";
                public const string kilovolt_amperes_reactive = "kilovolt_amperes_reactive";
                //public const string kilovolts = "kilovolts";
                //public const string megohms = "megohms";
                public const string megavolt_amperes = "megavolt_amperes";
                public const string megavolt_amperes_reactive = "megavolt_amperes_reactive";
                public const string megavolts = "megavolts";
                public const string microsiemens = "microsiemens";
                public const string milliamperes = "milliamperes";
                //public const string milliohms = "milliohms";
                public const string millisiemens = "millisiemens";
                public const string ohm_meter_squared_per_meter = "ohm_meter_squared_per_meter";
                public const string ohm_meters = "ohm_meters";
                //public const string ohms = "ohms";
                public const string power_factor = "power_factor";
                public const string siemens = "siemens";
                public const string siemens_per_meter = "siemens_per_meter";
                public const string teslas = "teslas";
                public const string volt_amperes = "volt_amperes";
                public const string volt_amperes_reactive = "volt_amperes_reactive";
                //public const string volts = "volts";
                public const string volts_per_degree_kelvin = "volts_per_degree_kelvin";
                public const string volts_per_meter = "volts_per_meter";
                public const string webers = "webers";

                // Energy
                public const string ampere_seconds = "ampere_seconds";
                public const string btus = "btus";
                public const string joules = "joules";
                public const string kilo_btus = "kilo_btus";
                public const string kilojoules = "kilojoules";
                public const string kilojoules_per_kilogram = "kilojoules_per_kilogram";
                public const string kilovolt_ampere_hours = "kilovolt_ampere_hours";
                public const string kilovolt_ampere_hours_reactive = "kilovolt_ampere_hours_reactive";
                public const string kilowatt_hours = "kilowatt_hours";
                public const string kilowatt_hours_reactive = "kilowatt_hours_reactive";
                public const string megajoules = "megajoules";
                public const string megavolt_ampere_hours = "megavolt_ampere_hours";
                public const string megavolt_ampere_hours_reactive = "megavolt_ampere_hours_reactive";
                public const string megawatt_hours = "megawatt_hours";
                public const string megawatt_hours_reactive = "megawatt_hours_reactive";
                public const string ton_hours = "ton_hours";
                public const string volt_ampere_hours = "volt_ampere_hours";
                public const string volt_ampere_hours_reactive = "volt_ampere_hours_reactive";
                public const string volt_square_hours = "volt_square_hours";
                public const string watt_hours = "watt_hours";
                public const string watt_hours_reactive = "watt_hours_reactive";

                // EnergyDensity
                public const string joules_per_cubic_meter = "joules_per_cubic_meter";
                public const string kilowatt_hours_per_square_foot = "kilowatt_hours_per_square_foot";
                public const string kilowatt_hours_per_square_meter = "kilowatt_hours_per_square_meter";
                public const string megajoules_per_square_foot = "megajoules_per_square_foot";
                public const string megajoules_per_square_meter = "megajoules_per_square_meter";
                public const string watt_hours_per_cubic_meter = "watt_hours_per_cubic_meter";

                // EnergySpecific
                public const string joule_seconds = "joule_seconds";

                // Enthalpy
                public const string btus_per_pound = "btus_per_pound";
                public const string btus_per_pound_dry_air = "btus_per_pound_dry_air";
                public const string joules_per_degree_kelvin = "joules_per_degree_kelvin";
                public const string joules_per_kilogram_dry_air = "joules_per_kilogram_dry_air";
                public const string joules_per_kilogram_degree_kelvin = "joules_per_kilogram_degree_kelvin";
                public const string kilojoules_per_degree_kelvin = "kilojoules_per_degree_kelvin";
                public const string kilojoules_per_kilogram_dry_air = "kilojoules_per_kilogram_dry_air";
                public const string megajoules_per_degree_kelvin = "megajoules_per_degree_kelvin";
                public const string megajoules_per_kilogram_dry_air = "megajoules_per_kilogram_dry_air";

                // Force
                public const string newton = "newton";

                // Frequency
                public const string cycles_per_hour = "cycles_per_hour";
                public const string cycles_per_minute = "cycles_per_minute";
                public const string hertz = "hertz";
                public const string kilohertz = "kilohertz";
                public const string megahertz = "megahertz";
                public const string per_hour = "per_hour";

                // General
                public const string decibels_a = "decibels_a";
                public const string grams_per_square_meter = "grams_per_square_meter";
                public const string nephelometric_turbidity_unit = "nephelometric_turbidity_unit";
                public const string pH = "pH";

                // Humidity
                public const string grams_of_water_per_kilogram_dry_air = "grams_of_water_per_kilogram_dry_air";
                public const string percent_relative_humidity = "percent_relative_humidity";

                // Illuminance
                public const string foot_candles = "foot_candles";
                public const string lux = "lux";

                // Inductance
                //public const string henrys = "henrys";
                public const string microhenrys = "microhenrys";
                public const string millihenrys = "millihenrys";

                // Length
                public const string centimeters = "centimeters";
                public const string feet = "feet";
                public const string inches = "inches";
                public const string kilometers = "kilometers";
                public const string meters = "meters";
                public const string micrometers = "micrometers";
                public const string millimeters = "millimeters";

                // Light
                public const string candelas = "candelas";
                public const string candelas_per_square_meter = "candelas_per_square_meter";
                //public const string foot_candles = "foot_candles";
                public const string lumens = "lumens";
                public const string luxes = "luxes";
                public const string watts_per_square_foot = "watts_per_square_foot";
                public const string watts_per_square_meter = "watts_per_square_meter";

                // Luminance
                //public const string candelas_per_square_meter = "candelas_per_square_meter";
                public const string nits = "nits";

                // LuminousIntensity
                public const string candela = "candela";

                // MagneticFieldStrength
                //public const string amperes_per_meter = "amperes_per_meter";
                public const string oersteds = "oersteds";

                // MagneticFlux
                public const string maxwells = "maxwells";
                //public const string webers = "webers";

                // Mass
                public const string grams = "grams";
                public const string kilograms = "kilograms";
                public const string milligrams = "milligrams";
                public const string pounds_mass = "pounds_mass";
                public const string tons = "tons";

                // MassDensity
                public const string grams_per_cubic_centimeter = "grams_per_cubic_centimeter";
                public const string grams_per_cubic_meter = "grams_per_cubic_meter";
                public const string kilograms_per_cubic_meter = "kilograms_per_cubic_meter";
                public const string micrograms_per_cubic_meter = "micrograms_per_cubic_meter";
                public const string milligrams_per_cubic_meter = "milligrams_per_cubic_meter";
                public const string nanograms_per_cubic_meter = "nanograms_per_cubic_meter";

                // MassFlow
                public const string grams_per_minute = "grams_per_minute";
                public const string grams_per_second = "grams_per_second";
                public const string kilograms_per_hour = "kilograms_per_hour";
                public const string kilograms_per_minute = "kilograms_per_minute";
                public const string kilograms_per_second = "kilograms_per_second";
                public const string pounds_mass_per_hour = "pounds_mass_per_hour";
                public const string pounds_mass_per_minute = "pounds_mass_per_minute";
                public const string pounds_mass_per_second = "pounds_mass_per_second";
                public const string tons_per_hour = "tons_per_hour";

                // MassFraction
                public const string grams_per_gram = "grams_per_gram";
                public const string grams_per_kilogram = "grams_per_kilogram";
                public const string grams_per_liter = "grams_per_liter";
                public const string grams_per_milliliter = "grams_per_milliliter";
                public const string kilograms_per_kilogram = "kilograms_per_kilogram";
                public const string micrograms_per_liter = "micrograms_per_liter";
                public const string milligrams_per_gram = "milligrams_per_gram";
                public const string milligrams_per_kilogram = "milligrams_per_kilogram";
                public const string milligrams_per_liter = "milligrams_per_liter";

                // PhysicalProperties
                public const string newton_seconds = "newton_seconds";
                public const string newtons_per_meter = "newtons_per_meter";
                public const string pascal_seconds = "pascal_seconds";
                public const string square_meters_per_newton = "square_meters_per_newton";
                public const string watts_per_meter_per_degree_kelvin = "watts_per_meter_per_degree_kelvin";
                public const string watts_per_square_meter_degree_kelvin = "watts_per_square_meter_degree_kelvin";

                // Power
                public const string horsepower = "horsepower";
                public const string joule_per_hours = "joule_per_hours";
                public const string kilo_btus_per_hour = "kilo_btus_per_hour";
                public const string kilowatts = "kilowatts";
                public const string megawatts = "megawatts";
                public const string milliwatts = "milliwatts";
                public const string tons_refrigeration = "tons_refrigeration";
                public const string watts = "watts";
                public const string btus_per_hour = "btus_per_hour";

                // Pressure
                //public const string bars = "bars";
                public const string centimeters_of_mercury = "centimeters_of_mercury";
                public const string centimeters_of_water = "centimeters_of_water";
                public const string hectopascals = "hectopascals";
                public const string inches_of_mercury = "inches_of_mercury";
                public const string inches_of_water = "inches_of_water";
                public const string kilopascals = "kilopascals";
                public const string millibars = "millibars";
                public const string millimeters_of_mercury = "millimeters_of_mercury";
                public const string millimeters_of_water = "millimeters_of_water";
                public const string pascals = "pascals";
                public const string pounds_force_per_square_inch = "pounds_force_per_square_inch";

                // Radiation
                public const string becquerels = "becquerels";
                public const string curies = "curies";
                public const string gray = "gray";
                public const string kilobecquerels = "kilobecquerels";
                public const string megabecquerels = "megabecquerels";
                public const string milligray = "milligray";
                public const string millirems = "millirems";
                public const string millirems_per_hour = "millirems_per_hour";
                public const string millisieverts = "millisieverts";
                public const string microsieverts = "microsieverts";
                public const string microsieverts_per_hour = "microsieverts_per_hour";
                public const string microgray = "microgray";
                public const string rads = "rads";
                public const string rems = "rems";
                public const string sieverts = "sieverts";

                // RadiantIntensity
                public const string microwatts_per_steradian = "microwatts_per_steradian";
                public const string watts_per_steradian = "watts_per_steradian";

                // Temperature
                public const string degree_days_celsius = "degree_days_celsius";
                public const string degree_days_fahrenheit = "degree_days_fahrenheit";
                public const string degrees_celsius = "degrees_celsius";
                public const string degrees_fahrenheit = "degrees_fahrenheit";
                public const string degrees_kelvin = "degrees_kelvin";
                public const string degrees_rankine = "degrees_rankine";
                public const string delta_degrees_fahrenheit = "delta_degrees_fahrenheit";
                public const string delta_degrees_kelvin = "delta_degrees_kelvin";

                // TemperatureRate
                public const string degrees_celsius_per_hour = "degrees_celsius_per_hour";
                public const string degrees_celsius_per_minute = "degrees_celsius_per_minute";
                public const string degrees_fahrenheit_per_hour = "degrees_fahrenheit_per_hour";
                public const string degrees_fahrenheit_per_minute = "degrees_fahrenheit_per_minute";
                public const string minutes_per_degree_kelvin = "minutes_per_degree_kelvin";
                public const string psi_per_degree_fahrenheit = "psi_per_degree_fahrenheit";

                // Time
                public const string days = "days";
                public const string hours = "hours";
                public const string hundredths_seconds = "hundredths_seconds";
                public const string milliseconds = "milliseconds";
                public const string minutes = "minutes";
                public const string months = "months";
                public const string seconds = "seconds";
                public const string weeks = "weeks";
                public const string years = "years";

                // Torque
                public const string newton_meters = "newton_meters";

                // Velocity
                public const string feet_per_minute = "feet_per_minute";
                public const string feet_per_second = "feet_per_second";
                public const string millimeters_per_second = "millimeters_per_second";
                public const string kilometers_per_hour = "kilometers_per_hour";
                public const string meters_per_hour = "meters_per_hour";
                public const string meters_per_minute = "meters_per_minute";
                public const string meters_per_second = "meters_per_second";
                public const string miles_per_hour = "miles_per_hour";
                public const string millimeters_per_minute = "millimeters_per_minute";

                // Volume
                public const string cubic_feet = "cubic_feet";
                public const string cubic_meters = "cubic_meters";
                public const string imperial_gallons = "imperial_gallons";
                public const string liters = "liters";
                public const string milliliters = "milliliters";
                public const string us_gallons = "us_gallons";

                // VolumeSpecific
                public const string cubic_feet_per_pound = "cubic_feet_per_pound";
                public const string cubic_meters_per_kilogram = "cubic_meters_per_kilogram";

                // VolumetricFlow
                public const string cubic_feet_per_day = "cubic_feet_per_day";
                public const string cubic_feet_per_hour = "cubic_feet_per_hour";
                public const string cubic_feet_per_minute = "cubic_feet_per_minute";
                public const string cubic_feet_per_second = "cubic_feet_per_second";
                public const string cubic_meters_per_day = "cubic_meters_per_day";
                public const string cubic_meters_per_hour = "cubic_meters_per_hour";
                public const string cubic_meters_per_minute = "cubic_meters_per_minute";
                public const string cubic_meters_per_second = "cubic_meters_per_second";
                public const string imperial_gallons_per_minute = "imperial_gallons_per_minute";
                public const string liters_per_hour = "liters_per_hour";
                public const string liters_per_minute = "liters_per_minute";
                public const string liters_per_second = "liters_per_second";
                public const string milliliters_per_second = "milliliters_per_second";
                public const string million_standard_cubic_feet_per_day = "million_standard_cubic_feet_per_day";
                public const string million_standard_cubic_feet_per_minute = "million_standard_cubic_feet_per_minute";
                public const string pounds_mass_per_day = "pounds_mass_per_day";
                public const string standard_cubic_feet_per_day = "standard_cubic_feet_per_day";
                public const string thousand_cubic_feet_per_day = "thousand_cubic_feet_per_day";
                public const string thousand_standard_cubic_feet_per_day = "thousand_standard_cubic_feet_per_day";
                public const string us_gallons_per_hour = "us_gallons_per_hour";
                public const string us_gallons_per_minute = "us_gallons_per_minute";
            }
            public static class Symbol
            {
                // Acceleration
                public const string meters_per_second_per_second = "m/s²";

                // Angular
                public const string degrees_angular = "°";
                public const string radians = "rad";
                public const string radians_per_second = "rad/s";
                public const string revolutions_per_minute = "RPM";

                // Area
                public const string square_centimeters = "cm²";
                public const string square_feet = "ft²";
                public const string square_inches = "in²";
                public const string square_meters = "m²";

                // Capacitance
                public const string farads = "F";
                public const string microfarads = "μF";
                public const string nanofarads = "nF";
                public const string picofarads = "pF";

                // Concentration
                public const string mole_percent = "mol%";
                public const string parts_per_billion = "ppb";
                public const string parts_per_million = "ppm";
                public const string percent = "%";
                public const string percent_obscuration_per_foot = "%/ft";
                public const string percent_obscuration_per_meter = "%/m";
                public const string percent_per_second = "%/s";
                public const string per_mille = "‰";

                // Currency
                public const string afghan_afghani = "؋";
                public const string albanian_lek = "L";
                public const string algerian_dinar = "د.ج";
                public const string angolan_kwanza = "Kz";
                public const string argentine_peso = "$";
                public const string armenian_dram = "֏";
                public const string aruban_florin = "ƒ";
                public const string australian_dollar = "$";
                public const string azerbaijani_manat = "₼";
                public const string bahamian_dollar = "$";
                public const string bahraini_dinar = ".د.ب";
                public const string bangladeshi_taka = "৳";
                public const string barbadian_dollar = "$";
                public const string belarusian_ruble = "Br";
                public const string belize_dollar = "$";
                public const string bermudian_dollar = "$";
                public const string bhutanese_ngultrum = "Nu.";
                public const string bolivian_boliviano = "Bs.";
                public const string bosnia_and_herzegovina_convertible_mark = "KM";
                public const string botswana_pula = "P";
                public const string brazilian_real = "R$";
                public const string brunei_dollar = "$";
                public const string bulgarian_lev = "лв";
                public const string burundian_franc = "FBu";
                public const string cape_verdean_escudo = "$";
                public const string cambodian_riel = "៛";
                public const string canadian_dollar = "$";
                public const string cayman_islands_dollar = "$";
                public const string central_african_cfa_franc = "Fr";
                public const string chilean_peso = "$";
                public const string chinese_yuan = "¥";
                public const string colombian_peso = "$";
                public const string comorian_franc = "CF";
                public const string congolese_franc = "FC";
                public const string costa_rican_colon = "₡";
                public const string croatian_kuna = "kn";
                public const string cuban_convertible_peso = "$";
                public const string cuban_peso = "$";
                public const string czech_koruna = "Kč";
                public const string danish_krone = "kr";
                public const string djiboutian_franc = "Fdj";
                public const string dominican_peso = "$";
                public const string east_caribbean_dollar = "$";
                public const string egyptian_pound = "£";
                public const string eritrean_nakfa = "Nfk";
                public const string ethiopian_birr = "Br";
                public const string euro = "€";
                public const string falkland_islands_pound = "£";
                public const string fiji_dollar = "$";
                public const string gambian_dalasi = "D";
                public const string georgian_lari = "₾";
                public const string ghanaian_cedi = "₵";
                public const string gibraltar_pound = "£";
                public const string guatemalan_quetzal = "Q";
                public const string guinean_franc = "FG";
                public const string guyanese_dollar = "$";
                public const string haitian_gourde = "G";
                public const string honduran_lempira = "L";
                public const string hong_kong_dollar = "$";
                public const string hungarian_forint = "Ft";
                public const string icelandic_krona = "kr";
                public const string indian_rupee = "₹";
                public const string indonesian_rupiah = "Rp";
                public const string iranian_rial = "﷼";
                public const string iraqi_dinar = "ع.د";
                public const string israeli_new_shekel = "₪";
                public const string jamaican_dollar = "$";
                public const string japanese_yen = "¥";
                public const string jordanian_dinar = "د.ا";
                public const string kazakhstani_tenge = "₸";
                public const string kenyan_shilling = "KSh";
                public const string kuwaiti_dinar = "د.ك";
                public const string kyrgyzstani_som = "лв";
                public const string lao_kip = "₭";
                public const string lebanese_pound = "ل.ل";
                public const string lesotho_loti = "L";
                public const string liberian_dollar = "$";
                public const string libyan_dinar = "ل.د";
                public const string macanese_pataca = "MOP$";
                public const string malagasy_ariary = "Ar";
                public const string malawian_kwacha = "MK";
                public const string malaysian_ringgit = "RM";
                public const string maldivian_rufiyaa = "ރ.";
                public const string mauritanian_ouguiya = "UM";
                public const string mauritian_rupee = "₨";
                public const string mexican_peso = "$";
                public const string moldovan_leu = "L";
                public const string mongolian_togrog = "₮";
                public const string moroccan_dirham = "د.م.";
                public const string mozambican_metical = "MT";
                public const string myanmar_kyat = "K";
                public const string namibian_dollar = "$";
                public const string nepalese_rupee = "₨";
                public const string netherlands_antillean_guilder = "ƒ";
                public const string new_taiwan_dollar = "$";
                public const string new_zealand_dollar = "$";
                public const string nicaraguan_cordoba = "C$";
                public const string nigerian_naira = "₦";
                public const string north_korean_won = "₩";
                public const string norwegian_krone = "kr";
                public const string omani_rial = "ر.ع.";
                public const string pakistani_rupee = "₨";
                public const string panamanian_balboa = "B/.";
                public const string papua_new_guinean_kina = "K";
                public const string paraguayan_guarani = "₲";
                public const string peruvian_sol = "S/";
                public const string philippine_peso = "₱";
                public const string polish_zloty = "zł";
                public const string qatar_riyal = "ر.ق";
                public const string romanian_leu = "lei";
                public const string russian_ruble = "₽";
                public const string rwandan_franc = "FRw";
                public const string saint_helena_pound = "£";
                public const string samoan_tala = "WS$";
                public const string saudi_riyal = "ر.س";
                public const string serbian_dinar = "дин.";
                public const string seychellois_rupee = "₨";
                public const string sierra_leonean_leone = "Le";
                public const string singapore_dollar = "$";
                public const string solomon_islands_dollar = "$";
                public const string somali_shilling = "Sh";
                public const string south_african_rand = "R";
                public const string south_korean_won = "₩";
                public const string south_sudanese_pound = "£";
                public const string sri_lankan_rupee = "₨";
                public const string sudanese_pound = "ج.س.";
                public const string surinamese_dollar = "$";
                public const string swazi_lilangeni = "L";
                public const string swedish_krona = "kr";
                public const string swiss_franc = "Fr";
                public const string syrian_pound = "£";
                public const string taiwanese_dollar = "NT$";
                public const string tajikistani_somoni = "ЅМ";
                public const string tanzanian_shilling = "Sh";
                public const string thai_baht = "฿";
                public const string tonga_paanga = "T$";
                public const string trinidad_and_tobago_dollar = "$";
                public const string tunisian_dinar = "د.ت";
                public const string turkish_lira = "₺";
                public const string turkmenistani_manat = "m";
                public const string ugandan_shilling = "USh";
                public const string ukrainian_hryvnia = "₴";
                public const string united_arab_emirates_dirham = "د.إ";
                public const string uruguayan_peso = "$U";
                public const string uzbekistani_som = "so'm";
                public const string vanuatu_vatu = "VT";
                public const string venezuelan_bolivar = "Bs.";
                public const string vietnamese_dong = "₫";
                public const string yemeni_rial = "﷼";
                public const string zambian_kwacha = "ZK";
                public const string zimbabwean_dollar = "$";

                // DataRate
                public const string bits_per_second = "bps";
                public const string gigabits_per_second = "Gbps";
                public const string kilobits_per_second = "kbps";
                public const string megabits_per_second = "Mbps";

                // DataStorage
                public const string bytes = "B";
                public const string exabytes = "EB";
                public const string gigabytes = "GB";
                public const string kilobytes = "KB";
                public const string megabytes = "MB";
                public const string petabytes = "PB";
                public const string terabytes = "TB";
                public const string yottabytes = "YB";
                public const string zettabytes = "ZB";

                // ElectricCharge
                public const string ampere_hours = "Ah";
                public const string coulombs = "C";

                // ElectricPotential
                public const string kilovolts = "kV";
                public const string millivolts = "mV";
                public const string volts = "V";

                // ElectricResistance
                public const string kilohms = "kΩ";
                public const string megohms = "MΩ";
                public const string milliohms = "mΩ";
                public const string ohms = "Ω";

                // Electrical
                public const string amperes = "A";
                public const string amperes_per_meter = "A/m";
                public const string amperes_per_square_meter = "A/m²";
                public const string ampere_square_meters = "A·m²";
                public const string bars = "bar";
                public const string decibels = "dB";
                public const string decibels_millivolt = "dBmV";
                public const string decibels_volt = "dBV";
                public const string degrees_phase = "°";
                //public const string farads = "F";
                public const string henrys = "H";
                //public const string kilohms = "kΩ";
                public const string kilovolt_amperes = "kVA";
                public const string kilovolt_amperes_reactive = "kVAR";
                //public const string kilovolts = "kV";
                //public const string megohms = "MΩ";
                public const string megavolt_amperes = "MVA";
                public const string megavolt_amperes_reactive = "MVAR";
                public const string megavolts = "MV";
                public const string microsiemens = "µS";
                public const string milliamperes = "mA";
                //public const string milliohms = "mΩ";
                public const string millisiemens = "mS";
                public const string ohm_meter_squared_per_meter = "Ω·m²/m";
                public const string ohm_meters = "Ω·m";
                //public const string ohms = "Ω";
                public const string power_factor = "PF";
                public const string siemens = "S";
                public const string siemens_per_meter = "S/m";
                public const string teslas = "T";
                public const string volt_amperes = "VA";
                public const string volt_amperes_reactive = "VAR";
                //public const string volts = "V";
                public const string volts_per_degree_kelvin = "V/K";
                public const string volts_per_meter = "V/m";
                public const string webers = "Wb";

                // Energy
                public const string ampere_seconds = "A·s";
                public const string btus = "BTU";
                public const string joules = "J";
                public const string kilo_btus = "kBTU";
                public const string kilojoules = "kJ";
                public const string kilojoules_per_kilogram = "kJ/kg";
                public const string kilovolt_ampere_hours = "kVAh";
                public const string kilovolt_ampere_hours_reactive = "kVARh";
                public const string kilowatt_hours = "kWh";
                public const string kilowatt_hours_reactive = "kVARh";
                public const string megajoules = "MJ";
                public const string megavolt_ampere_hours = "MVAh";
                public const string megavolt_ampere_hours_reactive = "MVARh";
                public const string megawatt_hours = "MWh";
                public const string megawatt_hours_reactive = "MVARh";
                public const string ton_hours = "ton·h";
                public const string volt_ampere_hours = "VAh";
                public const string volt_ampere_hours_reactive = "VARh";
                public const string volt_square_hours = "V²h";
                public const string watt_hours = "Wh";
                public const string watt_hours_reactive = "VARh";

                // EnergyDensity
                public const string joules_per_cubic_meter = "J/m³";
                public const string kilowatt_hours_per_square_foot = "kWh/ft²";
                public const string kilowatt_hours_per_square_meter = "kWh/m²";
                public const string megajoules_per_square_foot = "MJ/ft²";
                public const string megajoules_per_square_meter = "MJ/m²";
                public const string watt_hours_per_cubic_meter = "Wh/m³";

                // EnergySpecific
                public const string joule_seconds = "J·s";

                // Enthalpy
                public const string btus_per_pound = "BTU/lb";
                public const string btus_per_pound_dry_air = "BTU/lb_da";
                public const string joules_per_degree_kelvin = "J/K";
                public const string joules_per_kilogram_dry_air = "J/kg_da";
                public const string joules_per_kilogram_degree_kelvin = "J/(kg·K)";
                public const string kilojoules_per_degree_kelvin = "kJ/K";
                public const string kilojoules_per_kilogram_dry_air = "kJ/kg_da";
                public const string megajoules_per_degree_kelvin = "MJ/K";
                public const string megajoules_per_kilogram_dry_air = "MJ/kg_da";

                // Force
                public const string newton = "N";

                // Frequency
                public const string cycles_per_hour = "cph";
                public const string cycles_per_minute = "cpm";
                public const string hertz = "Hz";
                public const string kilohertz = "kHz";
                public const string megahertz = "MHz";
                public const string per_hour = "/h";

                // General
                public const string decibels_a = "dBA";
                public const string grams_per_square_meter = "g/m²";
                public const string nephelometric_turbidity_unit = "NTU";
                public const string pH = "pH";

                // Humidity
                public const string grams_of_water_per_kilogram_dry_air = "g/kg_da";
                public const string percent_relative_humidity = "%RH";

                // Illuminance
                public const string foot_candles = "fc";
                public const string lux = "lx";

                // Inductance
                //public const string henrys = "H";
                public const string microhenrys = "μH";
                public const string millihenrys = "mH";

                // Length
                public const string centimeters = "cm";
                public const string feet = "ft";
                public const string inches = "in";
                public const string kilometers = "km";
                public const string meters = "m";
                public const string micrometers = "µm";
                public const string millimeters = "mm";

                // Light
                public const string candelas = "cd";
                public const string candelas_per_square_meter = "cd/m²";
                //public const string foot_candles = "fc";
                public const string lumens = "lm";
                public const string luxes = "lx";
                public const string watts_per_square_foot = "W/ft²";
                public const string watts_per_square_meter = "W/m²";

                // Luminance
                //public const string candelas_per_square_meter = "cd/m²";
                public const string nits = "nt";

                // LuminousIntensity
                public const string candela = "cd";

                // MagneticFieldStrength
                //public const string amperes_per_meter = "A/m";
                public const string oersteds = "Oe";

                // MagneticFlux
                public const string maxwells = "Mx";
                //public const string webers = "Wb";

                // Mass
                public const string grams = "g";
                public const string kilograms = "kg";
                public const string milligrams = "mg";
                public const string pounds_mass = "lb";
                public const string tons = "t";

                // MassDensity
                public const string grams_per_cubic_centimeter = "g/cm³";
                public const string grams_per_cubic_meter = "g/m³";
                public const string kilograms_per_cubic_meter = "kg/m³";
                public const string micrograms_per_cubic_meter = "µg/m³";
                public const string milligrams_per_cubic_meter = "mg/m³";
                public const string nanograms_per_cubic_meter = "ng/m³";

                // MassFlow
                public const string grams_per_minute = "g/min";
                public const string grams_per_second = "g/s";
                public const string kilograms_per_hour = "kg/h";
                public const string kilograms_per_minute = "kg/min";
                public const string kilograms_per_second = "kg/s";
                public const string pounds_mass_per_hour = "lb/h";
                public const string pounds_mass_per_minute = "lb/min";
                public const string pounds_mass_per_second = "lb/s";
                public const string tons_per_hour = "t/h";

                // MassFraction
                public const string grams_per_gram = "g/g";
                public const string grams_per_kilogram = "g/kg";
                public const string grams_per_liter = "g/L";
                public const string grams_per_milliliter = "g/mL";
                public const string kilograms_per_kilogram = "kg/kg";
                public const string micrograms_per_liter = "µg/L";
                public const string milligrams_per_gram = "mg/g";
                public const string milligrams_per_kilogram = "mg/kg";
                public const string milligrams_per_liter = "mg/L";

                // PhysicalProperties
                public const string newton_seconds = "N·s";
                public const string newtons_per_meter = "N/m";
                public const string pascal_seconds = "Pa·s";
                public const string square_meters_per_newton = "m²/N";
                public const string watts_per_meter_per_degree_kelvin = "W/(m·K)";
                public const string watts_per_square_meter_degree_kelvin = "W/(m²·K)";

                // Power
                public const string horsepower = "hp";
                public const string joule_per_hours = "J/h";
                public const string kilo_btus_per_hour = "kBTU/h";
                public const string kilowatts = "kW";
                public const string megawatts = "MW";
                public const string milliwatts = "mW";
                public const string tons_refrigeration = "RT";
                public const string watts = "W";
                public const string btus_per_hour = "BTU/h";

                // Pressure
                //public const string bars = "bar";
                public const string centimeters_of_mercury = "cmHg";
                public const string centimeters_of_water = "cmH₂O";
                public const string hectopascals = "hPa";
                public const string inches_of_mercury = "inHg";
                public const string inches_of_water = "inH₂O";
                public const string kilopascals = "kPa";
                public const string millibars = "mbar";
                public const string millimeters_of_mercury = "mmHg";
                public const string millimeters_of_water = "mmH₂O";
                public const string pascals = "Pa";
                public const string pounds_force_per_square_inch = "psi";

                // Radiation
                public const string becquerels = "Bq";
                public const string curies = "Ci";
                public const string gray = "Gy";
                public const string kilobecquerels = "kBq";
                public const string megabecquerels = "MBq";
                public const string milligray = "mGy";
                public const string millirems = "mrem";
                public const string millirems_per_hour = "mrem/h";
                public const string millisieverts = "mSv";
                public const string microsieverts = "µSv";
                public const string microsieverts_per_hour = "µSv/h";
                public const string microgray = "µGy";
                public const string rads = "rad";
                public const string rems = "rem";
                public const string sieverts = "Sv";

                // RadiantIntensity
                public const string microwatts_per_steradian = "µW/sr";
                public const string watts_per_steradian = "W/sr";

                // Temperature
                public const string degree_days_celsius = "°C·d";
                public const string degree_days_fahrenheit = "°F·d";
                public const string degrees_celsius = "°C";
                public const string degrees_fahrenheit = "°F";
                public const string degrees_kelvin = "K";
                public const string degrees_rankine = "°R";
                public const string delta_degrees_fahrenheit = "Δ°F";
                public const string delta_degrees_kelvin = "ΔK";

                // TemperatureRate
                public const string degrees_celsius_per_hour = "°C/h";
                public const string degrees_celsius_per_minute = "°C/min";
                public const string degrees_fahrenheit_per_hour = "°F/h";
                public const string degrees_fahrenheit_per_minute = "°F/min";
                public const string minutes_per_degree_kelvin = "min/K";
                public const string psi_per_degree_fahrenheit = "psi/°F";

                // Time
                public const string days = "d";
                public const string hours = "h";
                public const string hundredths_seconds = "cs";
                public const string milliseconds = "ms";
                public const string minutes = "min";
                public const string months = "mo";
                public const string seconds = "s";
                public const string weeks = "wk";
                public const string years = "y";

                // Torque
                public const string newton_meters = "N·m";

                // Velocity
                public const string feet_per_minute = "ft/min";
                public const string feet_per_second = "ft/s";
                public const string millimeters_per_second = "mm/s";
                public const string kilometers_per_hour = "km/h";
                public const string meters_per_hour = "m/h";
                public const string meters_per_minute = "m/min";
                public const string meters_per_second = "m/s";
                public const string miles_per_hour = "mph";
                public const string millimeters_per_minute = "mm/min";

                // Volume
                public const string cubic_feet = "ft³";
                public const string cubic_meters = "m³";
                public const string imperial_gallons = "gal_imp";
                public const string liters = "L";
                public const string milliliters = "mL";
                public const string us_gallons = "gal_us";

                // VolumeSpecific
                public const string cubic_feet_per_pound = "ft³/lb";
                public const string cubic_meters_per_kilogram = "m³/kg";

                // VolumetricFlow
                public const string cubic_feet_per_day = "ft³/d";
                public const string cubic_feet_per_hour = "ft³/h";
                public const string cubic_feet_per_minute = "ft³/min";
                public const string cubic_feet_per_second = "ft³/s";
                public const string cubic_meters_per_day = "m³/d";
                public const string cubic_meters_per_hour = "m³/h";
                public const string cubic_meters_per_minute = "m³/min";
                public const string cubic_meters_per_second = "m³/s";
                public const string imperial_gallons_per_minute = "gal_imp/min";
                public const string liters_per_hour = "L/h";
                public const string liters_per_minute = "L/min";
                public const string liters_per_second = "L/s";
                public const string milliliters_per_second = "mL/s";
                public const string million_standard_cubic_feet_per_day = "MMSCFD";
                public const string million_standard_cubic_feet_per_minute = "MMSCFM";
                public const string pounds_mass_per_day = "lbm/d";
                public const string standard_cubic_feet_per_day = "SCFD";
                public const string thousand_cubic_feet_per_day = "MCFD";
                public const string thousand_standard_cubic_feet_per_day = "MSCFD";
                public const string us_gallons_per_hour = "gal_us/h";
                public const string us_gallons_per_minute = "gal_us/min";
            }
        }
        public static class Acceleration
        {
            public static class Name
            {
                public const string meters_per_second_per_second = All.Name.meters_per_second_per_second; // 166 Acceleration
                public const string standard_gravity = All.Name.standard_gravity;
            }
        
            public static class Symbol
            {
                public const string meters_per_second_per_second = "m/s²"; // 166 Acceleration
                public const string standard_gravity = "g";
            }
        }

        public static class Angular
        {
            public static class Name
            {
                public const string degrees_angular = "degrees_angular"; // 90 Other
                public const string radians = "radians"; // 103 Other
                public const string radians_per_second = "radians_per_second"; // 184 Other
                public const string revolutions_per_minute = "revolutions_per_minute"; // 104 Other
            }


            public static class Symbol
            {
                public const string degrees_angular = "°"; // 90 Other
                public const string radians = "rad"; // 103 Other
                public const string radians_per_second = "rad/s"; // 184 Other
                public const string revolutions_per_minute = "RPM"; // 104 Other
            }
        }

        public static class Area
        {
            public static class Name
            {
                public const string square_centimeters = "square_centimeters"; // 116 Area
                public const string square_feet = "square_feet"; // 1 Area
                public const string square_inches = "square_inches"; // 115 Area
                public const string square_meters = "square_meters"; // 0 Area
            }


            public static class Symbol
            {
                public const string square_centimeters = "cm²"; // 116 Area
                public const string square_feet = "ft²"; // 1 Area
                public const string square_inches = "in²"; // 115 Area
                public const string square_meters = "m²"; // 0 Area
            }
        }

        public static class Capacitance
        {
            public static class Name
            {
                public const string farads = "farads"; // SI unit of capacitance
                public const string microfarads = "microfarads"; // One millionth of a farad
                public const string nanofarads = "nanofarads"; // One billionth of a farad
                public const string picofarads = "picofarads"; // One trillionth of a farad
            }

            public static class Symbol
            {
                public const string farads = "F"; // SI unit of capacitance
                public const string microfarads = "μF"; // One millionth of a farad
                public const string nanofarads = "nF"; // One billionth of a farad
                public const string picofarads = "pF"; // One trillionth of a farad
            }
        }

        public static class Concentration
        {
            public static class Name
            {
                public const string mole_percent = "mole_percent"; // 252 Other
                public const string parts_per_billion = "parts_per_billion"; // 97 Other
                public const string parts_per_million = "parts_per_million"; // 96 Other
                public const string percent = "percent"; // 98 Other
                public const string percent_obscuration_per_foot = "percent_obscuration_per_foot"; // 143 Other
                public const string percent_obscuration_per_meter = "percent_obscuration_per_meter"; // 144 Other
                public const string percent_per_second = "percent_per_second"; // 99 Other
                public const string per_mille = "per_mille"; // 207 Other
            }


            public static class Symbol
            {
                public const string mole_percent = "mol%"; // 252 Other
                public const string parts_per_billion = "ppb"; // 97 Other
                public const string parts_per_million = "ppm"; // 96 Other
                public const string percent = "%"; // 98 Other
                public const string percent_obscuration_per_foot = "%/ft"; // 143 Other
                public const string percent_obscuration_per_meter = "%/m"; // 144 Other
                public const string percent_per_second = "%/s"; // 99 Other
                public const string per_mille = "‰"; // 207 Other
            }
        }


        public static class Currency
        {
            public static class Name
            {
                public const string afghan_afghani = "afghan_afghani"; // AFN
                public const string albanian_lek = "albanian_lek"; // ALL
                public const string algerian_dinar = "algerian_dinar"; // DZD
                public const string angolan_kwanza = "angolan_kwanza"; // AOA
                public const string argentine_peso = "argentine_peso"; // ARS
                public const string armenian_dram = "armenian_dram"; // AMD
                public const string aruban_florin = "aruban_florin"; // AWG
                public const string australian_dollar = "australian_dollar"; // AUD
                public const string azerbaijani_manat = "azerbaijani_manat"; // AZN
                public const string bahamian_dollar = "bahamian_dollar"; // BSD
                public const string bahraini_dinar = "bahraini_dinar"; // BHD
                public const string bangladeshi_taka = "bangladeshi_taka"; // BDT
                public const string barbadian_dollar = "barbadian_dollar"; // BBD
                public const string belarusian_ruble = "belarusian_ruble"; // BYN
                public const string belize_dollar = "belize_dollar"; // BZD
                public const string bermudian_dollar = "bermudian_dollar"; // BMD
                public const string bhutanese_ngultrum = "bhutanese_ngultrum"; // BTN
                public const string bolivian_boliviano = "bolivian_boliviano"; // BOB
                public const string bosnia_and_herzegovina_convertible_mark = "bosnia_and_herzegovina_convertible_mark"; // BAM
                public const string botswana_pula = "botswana_pula"; // BWP
                public const string brazilian_real = "brazilian_real"; // BRL
                public const string brunei_dollar = "brunei_dollar"; // BND
                public const string bulgarian_lev = "bulgarian_lev"; // BGN
                public const string burundian_franc = "burundian_franc"; // BIF
                public const string cape_verdean_escudo = "cape_verdean_escudo"; // CVE
                public const string cambodian_riel = "cambodian_riel"; // KHR
                public const string canadian_dollar = "canadian_dollar"; // CAD
                public const string cayman_islands_dollar = "cayman_islands_dollar"; // KYD
                public const string central_african_cfa_franc = "central_african_cfa_franc"; // XAF
                public const string chilean_peso = "chilean_peso"; // CLP
                public const string chinese_yuan = "chinese_yuan"; // CNY
                public const string colombian_peso = "colombian_peso"; // COP
                public const string comorian_franc = "comorian_franc"; // KMF
                public const string congolese_franc = "congolese_franc"; // CDF
                public const string costa_rican_colon = "costa_rican_colon"; // CRC
                public const string croatian_kuna = "croatian_kuna"; // HRK
                public const string cuban_convertible_peso = "cuban_convertible_peso"; // CUC
                public const string cuban_peso = "cuban_peso"; // CUP
                public const string czech_koruna = "czech_koruna"; // CZK
                public const string danish_krone = "danish_krone"; // DKK
                public const string djiboutian_franc = "djiboutian_franc"; // DJF
                public const string dominican_peso = "dominican_peso"; // DOP
                public const string east_caribbean_dollar = "east_caribbean_dollar"; // XCD
                public const string egyptian_pound = "egyptian_pound"; // EGP
                public const string eritrean_nakfa = "eritrean_nakfa"; // ERN
                public const string ethiopian_birr = "ethiopian_birr"; // ETB
                public const string euro = "euro"; // EUR
                public const string falkland_islands_pound = "falkland_islands_pound"; // FKP
                public const string fiji_dollar = "fiji_dollar"; // FJD
                public const string gambian_dalasi = "gambian_dalasi"; // GMD
                public const string georgian_lari = "georgian_lari"; // GEL
                public const string ghanaian_cedi = "ghanaian_cedi"; // GHS
                public const string gibraltar_pound = "gibraltar_pound"; // GIP
                public const string guatemalan_quetzal = "guatemalan_quetzal"; // GTQ
                public const string guinean_franc = "guinean_franc"; // GNF
                public const string guyanese_dollar = "guyanese_dollar"; // GYD
                public const string haitian_gourde = "haitian_gourde"; // HTG
                public const string honduran_lempira = "honduran_lempira"; // HNL
                public const string hong_kong_dollar = "hong_kong_dollar"; // HKD
                public const string hungarian_forint = "hungarian_forint"; // HUF
                public const string icelandic_krona = "icelandic_krona"; // ISK
                public const string indian_rupee = "indian_rupee"; // INR
                public const string indonesian_rupiah = "indonesian_rupiah"; // IDR
                public const string iranian_rial = "iranian_rial"; // IRR
                public const string iraqi_dinar = "iraqi_dinar"; // IQD
                public const string israeli_new_shekel = "israeli_new_shekel"; // ILS
                public const string jamaican_dollar = "jamaican_dollar"; // JMD
                public const string japanese_yen = "japanese_yen"; // JPY
                public const string jordanian_dinar = "jordanian_dinar"; // JOD
                public const string kazakhstani_tenge = "kazakhstani_tenge"; // KZT
                public const string kenyan_shilling = "kenyan_shilling"; // KES
                public const string kuwaiti_dinar = "kuwaiti_dinar"; // KWD
                public const string kyrgyzstani_som = "kyrgyzstani_som"; // KGS
                public const string lao_kip = "lao_kip"; // LAK
                public const string lebanese_pound = "lebanese_pound"; // LBP
                public const string lesotho_loti = "lesotho_loti"; // LSL
                public const string liberian_dollar = "liberian_dollar"; // LRD
                public const string libyan_dinar = "libyan_dinar"; // LYD
                public const string macanese_pataca = "macanese_pataca"; // MOP
                public const string malagasy_ariary = "malagasy_ariary"; // MGA
                public const string malawian_kwacha = "malawian_kwacha"; // MWK
                public const string malaysian_ringgit = "malaysian_ringgit"; // MYR
                public const string maldivian_rufiyaa = "maldivian_rufiyaa"; // MVR
                public const string mauritanian_ouguiya = "mauritanian_ouguiya"; // MRU
                public const string mauritian_rupee = "mauritian_rupee"; // MUR
                public const string mexican_peso = "mexican_peso"; // MXN
                public const string moldovan_leu = "moldovan_leu"; // MDL
                public const string mongolian_togrog = "mongolian_togrog"; // MNT
                public const string moroccan_dirham = "moroccan_dirham"; // MAD
                public const string mozambican_metical = "mozambican_metical"; // MZN
                public const string myanmar_kyat = "myanmar_kyat"; // MMK
                public const string namibian_dollar = "namibian_dollar"; // NAD
                public const string nepalese_rupee = "nepalese_rupee"; // NPR
                public const string netherlands_antillean_guilder = "netherlands_antillean_guilder"; // ANG
                public const string new_taiwan_dollar = "new_taiwan_dollar"; // TWD
                public const string new_zealand_dollar = "new_zealand_dollar"; // NZD
                public const string nicaraguan_cordoba = "nicaraguan_cordoba"; // NIO
                public const string nigerian_naira = "nigerian_naira"; // NGN
                public const string north_korean_won = "north_korean_won"; // KPW
                public const string norwegian_krone = "norwegian_krone"; // NOK
                public const string omani_rial = "omani_rial"; // OMR
                public const string pakistani_rupee = "pakistani_rupee"; // PKR
                public const string panamanian_balboa = "panamanian_balboa"; // PAB
                public const string papua_new_guinean_kina = "papua_new_guinean_kina"; // PGK
                public const string paraguayan_guarani = "paraguayan_guarani"; // PYG
                public const string peruvian_sol = "peruvian_sol"; // PEN
                public const string philippine_peso = "philippine_peso"; // PHP
                public const string polish_zloty = "polish_zloty"; // PLN
                public const string qatar_riyal = "qatar_riyal"; // QAR
                public const string romanian_leu = "romanian_leu"; // RON
                public const string russian_ruble = "russian_ruble"; // RUB
                public const string rwandan_franc = "rwandan_franc"; // RWF
                public const string saint_helena_pound = "saint_helena_pound"; // SHP
                public const string samoan_tala = "samoan_tala"; // WST
                public const string saudi_riyal = "saudi_riyal"; // SAR
                public const string serbian_dinar = "serbian_dinar"; // RSD
                public const string seychellois_rupee = "seychellois_rupee"; // SCR
                public const string sierra_leonean_leone = "sierra_leonean_leone"; // SLL
                public const string singapore_dollar = "singapore_dollar"; // SGD
                public const string solomon_islands_dollar = "solomon_islands_dollar"; // SBD
                public const string somali_shilling = "somali_shilling"; // SOS
                public const string south_african_rand = "south_african_rand"; // ZAR
                public const string south_korean_won = "south_korean_won"; // KRW
                public const string south_sudanese_pound = "south_sudanese_pound"; // SSP
                public const string sri_lankan_rupee = "sri_lankan_rupee"; // LKR
                public const string sudanese_pound = "sudanese_pound"; // SDG
                public const string surinamese_dollar = "surinamese_dollar"; // SRD
                public const string swazi_lilangeni = "swazi_lilangeni"; // SZL
                public const string swedish_krona = "swedish_krona"; // SEK
                public const string swiss_franc = "swiss_franc"; // CHF
                public const string syrian_pound = "syrian_pound"; // SYP
                public const string taiwanese_dollar = "taiwanese_dollar"; // TWD
                public const string tajikistani_somoni = "tajikistani_somoni"; // TJS
                public const string tanzanian_shilling = "tanzanian_shilling"; // TZS
                public const string thai_baht = "thai_baht"; // THB
                public const string tonga_paanga = "tonga_paanga"; // TOP
                public const string trinidad_and_tobago_dollar = "trinidad_and_tobago_dollar"; // TTD
                public const string tunisian_dinar = "tunisian_dinar"; // TND
                public const string turkish_lira = "turkish_lira"; // TRY
                public const string turkmenistani_manat = "turkmenistani_manat"; // TMT
                public const string ugandan_shilling = "ugandan_shilling"; // UGX
                public const string ukrainian_hryvnia = "ukrainian_hryvnia"; // UAH
                public const string united_arab_emirates_dirham = "united_arab_emirates_dirham"; // AED
                public const string uruguayan_peso = "uruguayan_peso"; // UYU
                public const string uzbekistani_som = "uzbekistani_som"; // UZS
                public const string vanuatu_vatu = "vanuatu_vatu"; // VUV
                public const string venezuelan_bolivar = "venezuelan_bolivar"; // VES
                public const string vietnamese_dong = "vietnamese_dong"; // VND
                public const string yemeni_rial = "yemeni_rial"; // YER
                public const string zambian_kwacha = "zambian_kwacha"; // ZMW
                public const string zimbabwean_dollar = "zimbabwean_dollar"; // ZWL
            }
            public static class Abbreviation
            {
                public const string afghan_afghani = "AFN"; // Afghan Afghani
                public const string albanian_lek = "ALL"; // Albanian Lek
                public const string algerian_dinar = "DZD"; // Algerian Dinar
                public const string angolan_kwanza = "AOA"; // Angolan Kwanza
                public const string argentine_peso = "ARS"; // Argentine Peso
                public const string armenian_dram = "AMD"; // Armenian Dram
                public const string aruban_florin = "AWG"; // Aruban Florin
                public const string australian_dollar = "AUD"; // Australian Dollar
                public const string azerbaijani_manat = "AZN"; // Azerbaijani Manat
                public const string bahamian_dollar = "BSD"; // Bahamian Dollar
                public const string bahraini_dinar = "BHD"; // Bahraini Dinar
                public const string bangladeshi_taka = "BDT"; // Bangladeshi Taka
                public const string barbadian_dollar = "BBD"; // Barbadian Dollar
                public const string belarusian_ruble = "BYN"; // Belarusian Ruble
                public const string belize_dollar = "BZD"; // Belize Dollar
                public const string bermudian_dollar = "BMD"; // Bermudian Dollar
                public const string bhutanese_ngultrum = "BTN"; // Bhutanese Ngultrum
                public const string bolivian_boliviano = "BOB"; // Bolivian Boliviano
                public const string bosnia_and_herzegovina_convertible_mark = "BAM"; // Bosnia and Herzegovina Convertible Mark
                public const string botswana_pula = "BWP"; // Botswana Pula
                public const string brazilian_real = "BRL"; // Brazilian Real
                public const string brunei_dollar = "BND"; // Brunei Dollar
                public const string bulgarian_lev = "BGN"; // Bulgarian Lev
                public const string burundian_franc = "BIF"; // Burundian Franc
                public const string cape_verdean_escudo = "CVE"; // Cape Verdean Escudo
                public const string cambodian_riel = "KHR"; // Cambodian Riel
                public const string canadian_dollar = "CAD"; // Canadian Dollar
                public const string cayman_islands_dollar = "KYD"; // Cayman Islands Dollar
                public const string central_african_cfa_franc = "XAF"; // Central African CFA Franc
                public const string chilean_peso = "CLP"; // Chilean Peso
                public const string chinese_yuan = "CNY"; // Chinese Yuan
                public const string colombian_peso = "COP"; // Colombian Peso
                public const string comorian_franc = "KMF"; // Comorian Franc
                public const string congolese_franc = "CDF"; // Congolese Franc
                public const string costa_rican_colon = "CRC"; // Costa Rican Colón
                public const string croatian_kuna = "HRK"; // Croatian Kuna
                public const string cuban_convertible_peso = "CUC"; // Cuban Convertible Peso
                public const string cuban_peso = "CUP"; // Cuban Peso
                public const string czech_koruna = "CZK"; // Czech Koruna
                public const string danish_krone = "DKK"; // Danish Krone
                public const string djiboutian_franc = "DJF"; // Djiboutian Franc
                public const string dominican_peso = "DOP"; // Dominican Peso
                public const string east_caribbean_dollar = "XCD"; // East Caribbean Dollar
                public const string egyptian_pound = "EGP"; // Egyptian Pound
                public const string eritrean_nakfa = "ERN"; // Eritrean Nakfa
                public const string ethiopian_birr = "ETB"; // Ethiopian Birr
                public const string euro = "EUR"; // Euro
                public const string falkland_islands_pound = "FKP"; // Falkland Islands Pound
                public const string fiji_dollar = "FJD"; // Fijian Dollar
                public const string gambian_dalasi = "GMD"; // Gambian Dalasi
                public const string georgian_lari = "GEL"; // Georgian Lari
                public const string ghanaian_cedi = "GHS"; // Ghanaian Cedi
                public const string gibraltar_pound = "GIP"; // Gibraltar Pound
                public const string guatemalan_quetzal = "GTQ"; // Guatemalan Quetzal
                public const string guinean_franc = "GNF"; // Guinean Franc
                public const string guyanese_dollar = "GYD"; // Guyanese Dollar
                public const string haitian_gourde = "HTG"; // Haitian Gourde
                public const string honduran_lempira = "HNL"; // Honduran Lempira
                public const string hong_kong_dollar = "HKD"; // Hong Kong Dollar
                public const string hungarian_forint = "HUF"; // Hungarian Forint
                public const string icelandic_krona = "ISK"; // Icelandic Króna
                public const string indian_rupee = "INR"; // Indian Rupee
                public const string indonesian_rupiah = "IDR"; // Indonesian Rupiah
                public const string iranian_rial = "IRR"; // Iranian Rial
                public const string iraqi_dinar = "IQD"; // Iraqi Dinar
                public const string israeli_new_shekel = "ILS"; // Israeli New Shekel
                public const string jamaican_dollar = "JMD"; // Jamaican Dollar
                public const string japanese_yen = "JPY"; // Japanese Yen
                public const string jordanian_dinar = "JOD"; // Jordanian Dinar
                public const string kazakhstani_tenge = "KZT"; // Kazakhstani Tenge
                public const string kenyan_shilling = "KES"; // Kenyan Shilling
                public const string kuwaiti_dinar = "KWD"; // Kuwaiti Dinar
                public const string kyrgyzstani_som = "KGS"; // Kyrgyzstani Som
                public const string lao_kip = "LAK"; // Lao Kip
                public const string lebanese_pound = "LBP"; // Lebanese Pound
                public const string lesotho_loti = "LSL"; // Lesotho Loti
                public const string liberian_dollar = "LRD"; // Liberian Dollar
                public const string libyan_dinar = "LYD"; // Libyan Dinar
                public const string macanese_pataca = "MOP"; // Macanese Pataca
                public const string malagasy_ariary = "MGA"; // Malagasy Ariary
                public const string malawian_kwacha = "MWK"; // Malawian Kwacha
                public const string malaysian_ringgit = "MYR"; // Malaysian Ringgit
                public const string maldivian_rufiyaa = "MVR"; // Maldivian Rufiyaa
                public const string mauritanian_ouguiya = "MRU"; // Mauritanian Ouguiya
                public const string mauritian_rupee = "MUR"; // Mauritian Rupee
                public const string mexican_peso = "MXN"; // Mexican Peso
                public const string moldovan_leu = "MDL"; // Moldovan Leu
                public const string mongolian_togrog = "MNT"; // Mongolian Tögrög
                public const string moroccan_dirham = "MAD"; // Moroccan Dirham
                public const string mozambican_metical = "MZN"; // Mozambican Metical
                public const string myanmar_kyat = "MMK"; // Myanmar Kyat
                public const string namibian_dollar = "NAD"; // Namibian Dollar
                public const string nepalese_rupee = "NPR"; // Nepalese Rupee
                public const string netherlands_antillean_guilder = "ANG"; // Netherlands Antillean Guilder
                public const string new_taiwan_dollar = "TWD"; // New Taiwan Dollar
                public const string new_zealand_dollar = "NZD"; // New Zealand Dollar
                public const string nicaraguan_cordoba = "NIO"; // Nicaraguan Córdoba
                public const string nigerian_naira = "NGN"; // Nigerian Naira
                public const string north_korean_won = "KPW"; // North Korean Won
                public const string norwegian_krone = "NOK"; // Norwegian Krone
                public const string omani_rial = "OMR"; // Omani Rial
                public const string pakistani_rupee = "PKR"; // Pakistani Rupee
                public const string panamanian_balboa = "PAB"; // Panamanian Balboa
                public const string papua_new_guinean_kina = "PGK"; // Papua New Guinean Kina
                public const string paraguayan_guarani = "PYG"; // Paraguayan Guaraní
                public const string peruvian_sol = "PEN"; // Peruvian Sol
                public const string philippine_peso = "PHP"; // Philippine Peso
                public const string polish_zloty = "PLN"; // Polish Złoty
                public const string qatari_riyal = "QAR"; // Qatari Riyal
                public const string romanian_leu = "RON"; // Romanian Leu
                public const string russian_ruble = "RUB"; // Russian Ruble
                public const string rwandan_franc = "RWF"; // Rwandan Franc
                public const string saint_helena_pound = "SHP"; // Saint Helena Pound
                public const string samoan_tala = "WST"; // Samoan Tala
                public const string sao_tome_and_principe_dobra = "STN"; // São Tomé and Príncipe Dobra
                public const string saudi_riyal = "SAR"; // Saudi Riyal
                public const string serbian_dinar = "RSD"; // Serbian Dinar
                public const string seychellois_rupee = "SCR"; // Seychellois Rupee
                public const string sierra_leonean_leone = "SLL"; // Sierra Leonean Leone
                public const string singapore_dollar = "SGD"; // Singapore Dollar
                public const string solomon_islands_dollar = "SBD"; // Solomon Islands Dollar
                public const string somali_shilling = "SOS"; // Somali Shilling
                public const string south_african_rand = "ZAR"; // South African Rand
                public const string south_korean_won = "KRW"; // South Korean Won
                public const string south_sudanese_pound = "SSP"; // South Sudanese Pound
                public const string sri_lankan_rupee = "LKR"; // Sri Lankan Rupee
                public const string sudanese_pound = "SDG"; // Sudanese Pound
                public const string surinamese_dollar = "SRD"; // Surinamese Dollar
                public const string swazi_lilangeni = "SZL"; // Swazi Lilangeni
                public const string swedish_krona = "SEK"; // Swedish Krona
                public const string swiss_franc = "CHF"; // Swiss Franc
                public const string syrian_pound = "SYP"; // Syrian Pound
                public const string taiwanese_dollar = "TWD"; // Taiwanese Dollar
                public const string tajikistani_somoni = "TJS"; // Tajikistani Somoni
                public const string tanzanian_shilling = "TZS"; // Tanzanian Shilling
                public const string thai_baht = "THB"; // Thai Baht
                public const string tongan_pa_anga = "TOP"; // Tongan Paʻanga
                public const string trinidad_and_tobago_dollar = "TTD"; // Trinidad and Tobago Dollar
                public const string tunisian_dinar = "TND"; // Tunisian Dinar
                public const string turkish_lira = "TRY"; // Turkish Lira
                public const string turkmenistani_manat = "TMT"; // Turkmenistani Manat
                public const string ugandan_shilling = "UGX"; // Ugandan Shilling
                public const string ukrainian_hryvnia = "UAH"; // Ukrainian Hryvnia
                public const string united_arab_emirates_dirham = "AED"; // United Arab Emirates Dirham
                public const string uruguayan_peso = "UYU"; // Uruguayan Peso
                public const string uzbekistani_som = "UZS"; // Uzbekistani Som
                public const string vanuatu_vatu = "VUV"; // Vanuatu Vatu
                public const string venezuelan_bolivar = "VES"; // Venezuelan Bolívar
                public const string vietnamese_dong = "VND"; // Vietnamese Đồng
                public const string yemeni_rial = "YER"; // Yemeni Rial
                public const string zambian_kwacha = "ZMW"; // Zambian Kwacha
                public const string zimbabwean_dollar = "ZWL"; // Zimbabwean Dollar
            }

            public static class Symbol
            {
                public const string afghan_afghani = "؋"; // Afghan Afghani (AFN)
                public const string albanian_lek = "L"; // Albanian Lek (ALL)
                public const string algerian_dinar = "د.ج"; // Algerian Dinar (DZD)
                public const string angolan_kwanza = "Kz"; // Angolan Kwanza (AOA)
                public const string argentine_peso = "$"; // Argentine Peso (ARS)
                public const string armenian_dram = "֏"; // Armenian Dram (AMD)
                public const string aruban_florin = "ƒ"; // Aruban Florin (AWG)
                public const string australian_dollar = "$"; // Australian Dollar (AUD)
                public const string azerbaijani_manat = "₼"; // Azerbaijani Manat (AZN)
                public const string bahamian_dollar = "$"; // Bahamian Dollar (BSD)
                public const string bahraini_dinar = ".د.ب"; // Bahraini Dinar (BHD)
                public const string bangladeshi_taka = "৳"; // Bangladeshi Taka (BDT)
                public const string barbadian_dollar = "$"; // Barbadian Dollar (BBD)
                public const string belarusian_ruble = "Br"; // Belarusian Ruble (BYN)
                public const string belize_dollar = "$"; // Belize Dollar (BZD)
                public const string bermudian_dollar = "$"; // Bermudian Dollar (BMD)
                public const string bhutanese_ngultrum = "Nu."; // Bhutanese Ngultrum (BTN)
                public const string bolivian_boliviano = "Bs."; // Bolivian Boliviano (BOB)
                public const string bosnia_and_herzegovina_convertible_mark = "KM"; // Bosnia and Herzegovina Convertible Mark (BAM)
                public const string botswana_pula = "P"; // Botswana Pula (BWP)
                public const string brazilian_real = "R$"; // Brazilian Real (BRL)
                public const string brunei_dollar = "$"; // Brunei Dollar (BND)
                public const string bulgarian_lev = "лв"; // Bulgarian Lev (BGN)
                public const string burundian_franc = "FBu"; // Burundian Franc (BIF)
                public const string cape_verdean_escudo = "$"; // Cape Verdean Escudo (CVE)
                public const string cambodian_riel = "៛"; // Cambodian Riel (KHR)
                public const string canadian_dollar = "$"; // Canadian Dollar (CAD)
                public const string cayman_islands_dollar = "$"; // Cayman Islands Dollar (KYD)
                public const string central_african_cfa_franc = "Fr"; // Central African CFA Franc (XAF)
                public const string chilean_peso = "$"; // Chilean Peso (CLP)
                public const string chinese_yuan = "¥"; // Chinese Yuan (CNY)
                public const string colombian_peso = "$"; // Colombian Peso (COP)
                public const string comorian_franc = "CF"; // Comorian Franc (KMF)
                public const string congolese_franc = "FC"; // Congolese Franc (CDF)
                public const string costa_rican_colon = "₡"; // Costa Rican Colón (CRC)
                public const string croatian_kuna = "kn"; // Croatian Kuna (HRK)
                public const string cuban_convertible_peso = "$"; // Cuban Convertible Peso (CUC)
                public const string cuban_peso = "$"; // Cuban Peso (CUP)
                public const string czech_koruna = "Kč"; // Czech Koruna (CZK)
                public const string danish_krone = "kr"; // Danish Krone (DKK)
                public const string djiboutian_franc = "Fdj"; // Djiboutian Franc (DJF)
                public const string dominican_peso = "$"; // Dominican Peso (DOP)
                public const string east_caribbean_dollar = "$"; // East Caribbean Dollar (XCD)
                public const string egyptian_pound = "£"; // Egyptian Pound (EGP)
                public const string eritrean_nakfa = "Nfk"; // Eritrean Nakfa (ERN)
                public const string ethiopian_birr = "Br"; // Ethiopian Birr (ETB)
                public const string euro = "€"; // Euro (EUR)
                public const string falkland_islands_pound = "£"; // Falkland Islands Pound (FKP)
                public const string fiji_dollar = "$"; // Fijian Dollar (FJD)
                public const string gambian_dalasi = "D"; // Gambian Dalasi (GMD)
                public const string georgian_lari = "₾"; // Georgian Lari (GEL)
                public const string ghanaian_cedi = "₵"; // Ghanaian Cedi (GHS)
                public const string gibraltar_pound = "£"; // Gibraltar Pound (GIP)
                public const string guatemalan_quetzal = "Q"; // Guatemalan Quetzal (GTQ)
                public const string guinean_franc = "FG"; // Guinean Franc (GNF)
                public const string guyanese_dollar = "$"; // Guyanese Dollar (GYD)
                public const string haitian_gourde = "G"; // Haitian Gourde (HTG)
                public const string honduran_lempira = "L"; // Honduran Lempira (HNL)
                public const string hong_kong_dollar = "$"; // Hong Kong Dollar (HKD)
                public const string hungarian_forint = "Ft"; // Hungarian Forint (HUF)
                public const string icelandic_krona = "kr"; // Icelandic Króna (ISK)
                public const string indian_rupee = "₹"; // Indian Rupee (INR)
                public const string indonesian_rupiah = "Rp"; // Indonesian Rupiah (IDR)
                public const string iranian_rial = "﷼"; // Iranian Rial (IRR)
                public const string iraqi_dinar = "ع.د"; // Iraqi Dinar (IQD)
                public const string israeli_new_shekel = "₪"; // Israeli New Shekel (ILS)
                public const string jamaican_dollar = "$"; // Jamaican Dollar (JMD)
                public const string japanese_yen = "¥"; // Japanese Yen (JPY)
                public const string jordanian_dinar = "د.ا"; // Jordanian Dinar (JOD)
                public const string kazakhstani_tenge = "₸"; // Kazakhstani Tenge (KZT)
                public const string kenyan_shilling = "KSh"; // Kenyan Shilling (KES)
                public const string kuwaiti_dinar = "د.ك"; // Kuwaiti Dinar (KWD)
                public const string kyrgyzstani_som = "с"; // Kyrgyzstani Som (KGS)
                public const string lao_kip = "₭"; // Lao Kip (LAK)
                public const string lebanese_pound = "ل.ل"; // Lebanese Pound (LBP)
                public const string lesotho_loti = "L"; // Lesotho Loti (LSL)
                public const string liberian_dollar = "$"; // Liberian Dollar (LRD)
                public const string libyan_dinar = "ل.د"; // Libyan Dinar (LYD)
                public const string macanese_pataca = "MOP$"; // Macanese Pataca (MOP)
                public const string malagasy_ariary = "Ar"; // Malagasy Ariary (MGA)
                public const string malawian_kwacha = "MK"; // Malawian Kwacha (MWK)
                public const string malaysian_ringgit = "RM"; // Malaysian Ringgit (MYR)
                public const string maldivian_rufiyaa = "ރ."; // Maldivian Rufiyaa (MVR)
                public const string mauritanian_ouguiya = "UM"; // Mauritanian Ouguiya (MRU)
                public const string mauritian_rupee = "₨"; // Mauritian Rupee (MUR)
                public const string mexican_peso = "$"; // Mexican Peso (MXN)
                public const string moldovan_leu = "L"; // Moldovan Leu (MDL)
                public const string mongolian_togrog = "₮"; // Mongolian Tögrög (MNT)
                public const string moroccan_dirham = "د.م."; // Moroccan Dirham (MAD)
                public const string mozambican_metical = "MT"; // Mozambican Metical (MZN)
                public const string myanmar_kyat = "K"; // Myanmar Kyat (MMK)
                public const string namibian_dollar = "$"; // Namibian Dollar (NAD)
                public const string nepalese_rupee = "₨"; // Nepalese Rupee (NPR)
                public const string netherlands_antillean_guilder = "ƒ"; // Netherlands Antillean Guilder (ANG)
                public const string new_taiwan_dollar = "$"; // New Taiwan Dollar (TWD)
                public const string new_zealand_dollar = "$"; // New Zealand Dollar (NZD)
                public const string nicaraguan_cordoba = "C$"; // Nicaraguan Córdoba (NIO)
                public const string nigerian_naira = "₦"; // Nigerian Naira (NGN)
                public const string north_korean_won = "₩"; // North Korean Won (KPW)
                public const string norwegian_krone = "kr"; // Norwegian Krone (NOK)
                public const string omani_rial = "ر.ع."; // Omani Rial (OMR)
                public const string pakistani_rupee = "₨"; // Pakistani Rupee (PKR)
                public const string panamanian_balboa = "B/."; // Panamanian Balboa (PAB)
                public const string papua_new_guinean_kina = "K"; // Papua New Guinean Kina (PGK)
                public const string paraguayan_guarani = "₲"; // Paraguayan Guaraní (PYG)
                public const string peruvian_sol = "S/"; // Peruvian Sol (PEN)
                public const string philippine_peso = "₱"; // Philippine Peso (PHP)
                public const string polish_zloty = "zł"; // Polish Złoty (PLN)
                public const string qatari_riyal = "ر.ق"; // Qatari Riyal (QAR)
                public const string romanian_leu = "lei"; // Romanian Leu (RON)
                public const string russian_ruble = "₽"; // Russian Ruble (RUB)
                public const string rwandan_franc = "FRw"; // Rwandan Franc (RWF)
                public const string saint_helena_pound = "£"; // Saint Helena Pound (SHP)
                public const string samoan_tala = "T"; // Samoan Tala (WST)
                public const string sao_tome_and_principe_dobra = "Db"; // São Tomé and Príncipe Dobra (STN)
                public const string saudi_riyal = "ر.س"; // Saudi Riyal (SAR)
                public const string serbian_dinar = "дин."; // Serbian Dinar (RSD)
                public const string seychellois_rupee = "₨"; // Seychellois Rupee (SCR)
                public const string sierra_leonean_leone = "Le"; // Sierra Leonean Leone (SLL)
                public const string singapore_dollar = "$"; // Singapore Dollar (SGD)
                public const string solomon_islands_dollar = "$"; // Solomon Islands Dollar (SBD)
                public const string somali_shilling = "Sh"; // Somali Shilling (SOS)
                public const string south_african_rand = "R"; // South African Rand (ZAR)
                public const string south_korean_won = "₩"; // South Korean Won (KRW)
                public const string south_sudanese_pound = "£"; // South Sudanese Pound (SSP)
                public const string sri_lankan_rupee = "₨"; // Sri Lankan Rupee (LKR)
                public const string sudanese_pound = "ج.س"; // Sudanese Pound (SDG)
                public const string surinamese_dollar = "$"; // Surinamese Dollar (SRD)
                public const string swazi_lilangeni = "E"; // Swazi Lilangeni (SZL)
                public const string swedish_krona = "kr"; // Swedish Krona (SEK)
                public const string swiss_franc = "CHF"; // Swiss Franc (CHF)
                public const string syrian_pound = "£"; // Syrian Pound (SYP)
                public const string taiwanese_dollar = "$"; // Taiwanese Dollar (TWD)
                public const string tajikistani_somoni = "ЅМ"; // Tajikistani Somoni (TJS)
                public const string tanzanian_shilling = "Sh"; // Tanzanian Shilling (TZS)
                public const string thai_baht = "฿"; // Thai Baht (THB)
                public const string tongan_pa_anga = "T$"; // Tongan Paʻanga (TOP)
                public const string trinidad_and_tobago_dollar = "$"; // Trinidad and Tobago Dollar (TTD)
                public const string tunisian_dinar = "د.ت"; // Tunisian Dinar (TND)
                public const string turkish_lira = "₺"; // Turkish Lira (TRY)
                public const string turkmenistani_manat = "T"; // Turkmenistani Manat (TMT)
                public const string ugandan_shilling = "USh"; // Ugandan Shilling (UGX)
                public const string ukrainian_hryvnia = "₴"; // Ukrainian Hryvnia (UAH)
                public const string united_arab_emirates_dirham = "د.إ"; // United Arab Emirates Dirham (AED)
                public const string uruguayan_peso = "$"; // Uruguayan Peso (UYU)
                public const string uzbekistani_som = "so'm"; // Uzbekistani Som (UZS)
                public const string vanuatu_vatu = "VT"; // Vanuatu Vatu (VUV)
                public const string venezuelan_bolivar = "Bs.S"; // Venezuelan Bolívar (VES)
                public const string vietnamese_dong = "₫"; // Vietnamese Đồng (VND)
                public const string yemeni_rial = "﷼"; // Yemeni Rial (YER)
                public const string zambian_kwacha = "ZK"; // Zambian Kwacha (ZMW)
                public const string zimbabwean_dollar = "Z$"; // Zimbabwean Dollar (ZWL)
            }
            
        }

        public static class DataRate
        {
            public static class Name
            {
                public const string bits_per_second = "bits_per_second";
                public const string gigabits_per_second = "gigabits_per_second";
                public const string kilobits_per_second = "kilobits_per_second";
                public const string megabits_per_second = "megabits_per_second";
            }

            public static class Symbol
            {
                public const string bits_per_second = "bps";
                public const string gigabits_per_second = "Gbps";
                public const string kilobits_per_second = "kbps";
                public const string megabits_per_second = "Mbps";
            }
        }

        public static class DataStorage
        {
            public static class Name
            {
                public const string bytes = "bytes"; // 1 Byte
                public const string exabytes = "exabytes"; // 1024 Petabytes
                public const string gigabytes = "gigabytes"; // 1024 Megabytes
                public const string kilobytes = "kilobytes"; // 1024 Bytes
                public const string megabytes = "megabytes"; // 1024 Kilobytes
                public const string petabytes = "petabytes"; // 1024 Terabytes
                public const string terabytes = "terabytes"; // 1024 Gigabytes
                public const string yottabytes = "yottabytes"; // 1024 Zettabytes
                public const string zettabytes = "zettabytes"; // 1024 Exabytes
            }

            public static class Symbol
            {
                public const string bytes = "B"; // 1 Byte
                public const string exabytes = "EB"; // 1024 Petabytes
                public const string gigabytes = "GB"; // 1024 Megabytes
                public const string kilobytes = "KB"; // 1024 Bytes
                public const string megabytes = "MB"; // 1024 Kilobytes
                public const string petabytes = "PB"; // 1024 Terabytes
                public const string terabytes = "TB"; // 1024 Gigabytes
                public const string yottabytes = "YB"; // 1024 Zettabytes
                public const string zettabytes = "ZB"; // 1024 Exabytes
            }
        }

        public static class ElectricCharge
        {
            public static class Name
            {
                public const string ampere_hours = "ampere_hours"; // Unit of electric charge commonly used for batteries
                public const string coulombs = "coulombs"; // SI unit of electric charge
            }

            public static class Symbol
            {
                public const string ampere_hours = "Ah"; // Unit of electric charge commonly used for batteries
                public const string coulombs = "C"; // SI unit of electric charge
            }
        }

        public static class ElectricPotential
        {
            public static class Name
            {
                public const string kilovolts = "kilovolts"; // One thousand volts
                public const string millivolts = "millivolts"; // One thousandth of a volt
                public const string volts = "volts"; // SI unit of electric potential
            }

            public static class Symbol
            {
                public const string kilovolts = "kV"; // One thousand volts
                public const string millivolts = "mV"; // One thousandth of a volt
                public const string volts = "V"; // SI unit of electric potential
            }
        }

        public static class ElectricResistance
        {
            public static class Name
            {
                public const string kilohms = "kilohms"; // One thousand ohms
                public const string megohms = "megohms"; // One million ohms
                public const string milliohms = "milliohms"; // One thousandth of an ohm
                public const string ohms = "ohms"; // SI unit of electric resistance
            }

            public static class Symbol
            {
                public const string kilohms = "kΩ"; // One thousand ohms
                public const string megohms = "MΩ"; // One million ohms
                public const string milliohms = "mΩ"; // One thousandth of an ohm
                public const string ohms = "Ω"; // SI unit of electric resistance
            }
        }

        public static class Electrical
        {
            public static class Name
            {
                public const string amperes = "amperes"; // 3 Electrical
                public const string amperes_per_meter = "amperes_per_meter"; // 167 Electrical
                public const string amperes_per_square_meter = "amperes_per_square_meter"; // 168 Electrical
                public const string ampere_square_meters = "ampere_square_meters"; // 169 Electrical
                public const string bars = "bars"; // 55 Electrical
                public const string decibels = "decibels"; // 199 Electrical
                public const string decibels_millivolt = "decibels_millivolt"; // 200 Electrical
                public const string decibels_volt = "decibels_volt"; // 201 Electrical
                public const string degrees_phase = "degrees_phase"; // 14 Electrical
                public const string farads = "farads"; // 170 Electrical
                public const string henrys = "henrys"; // 171 Electrical
                public const string kilohms = "kilohms"; // 122 Electrical
                public const string kilovolt_amperes = "kilovolt_amperes"; // 9 Electrical
                public const string kilovolt_amperes_reactive = "kilovolt_amperes_reactive"; // 12 Electrical
                public const string kilovolts = "kilovolts"; // 6 Electrical
                public const string megohms = "megohms"; // 123 Electrical
                public const string megavolt_amperes = "megavolt_amperes"; // 10 Electrical
                public const string megavolt_amperes_reactive = "megavolt_amperes_reactive"; // 13 Electrical
                public const string megavolts = "megavolts"; // 7 Electrical
                public const string microsiemens = "microsiemens"; // 190 Electrical
                public const string milliamperes = "milliamperes"; // 2 Electrical
                public const string milliohms = "milliohms"; // 145 Electrical
                public const string millisiemens = "millisiemens"; // 202 Electrical
                public const string ohm_meter_squared_per_meter = "ohm_meter_squared_per_meter"; // 237 Electrical
                public const string ohm_meters = "ohm_meters"; // 172 Electrical
                public const string ohms = "ohms"; // 4 Electrical
                public const string power_factor = "power_factor"; // 15 Electrical
                public const string siemens = "siemens"; // 173 Electrical
                public const string siemens_per_meter = "siemens_per_meter"; // 174 Electrical
                public const string teslas = "teslas"; // 175 Electrical
                public const string volt_amperes = "volt_amperes"; // 8 Electrical
                public const string volt_amperes_reactive = "volt_amperes_reactive"; // 11 Electrical
                public const string volts = "volts"; // 5 Electrical
                public const string volts_per_degree_kelvin = "volts_per_degree_kelvin"; // 176 Electrical
                public const string volts_per_meter = "volts_per_meter"; // 177 Electrical
                public const string webers = "webers"; // 178 Electrical
            }

            public static class Symbol
            {
                public const string amperes = "A"; // 3 Electrical
                public const string amperes_per_meter = "A/m"; // 167 Electrical
                public const string amperes_per_square_meter = "A/m²"; // 168 Electrical
                public const string ampere_square_meters = "A·m²"; // 169 Electrical
                public const string bars = "bar"; // 55 Electrical
                public const string decibels = "dB"; // 199 Electrical
                public const string decibels_millivolt = "dBmV"; // 200 Electrical
                public const string decibels_volt = "dBV"; // 201 Electrical
                public const string degrees_phase = "°"; // 14 Electrical
                public const string farads = "F"; // 170 Electrical
                public const string henrys = "H"; // 171 Electrical
                public const string kilohms = "kΩ"; // 122 Electrical
                public const string kilovolt_amperes = "kVA"; // 9 Electrical
                public const string kilovolt_amperes_reactive = "kVAR"; // 12 Electrical
                public const string kilovolts = "kV"; // 6 Electrical
                public const string megohms = "MΩ"; // 123 Electrical
                public const string megavolt_amperes = "MVA"; // 10 Electrical
                public const string megavolt_amperes_reactive = "MVAR"; // 13 Electrical
                public const string megavolts = "MV"; // 7 Electrical
                public const string microsiemens = "µS"; // 190 Electrical
                public const string milliamperes = "mA"; // 2 Electrical
                public const string milliohms = "mΩ"; // 145 Electrical
                public const string millisiemens = "mS"; // 202 Electrical
                public const string ohm_meter_squared_per_meter = "Ω·m²/m"; // 237 Electrical
                public const string ohm_meters = "Ω·m"; // 172 Electrical
                public const string ohms = "Ω"; // 4 Electrical
                public const string power_factor = "PF"; // 15 Electrical
                public const string siemens = "S"; // 173 Electrical
                public const string siemens_per_meter = "S/m"; // 174 Electrical
                public const string teslas = "T"; // 175 Electrical
                public const string volt_amperes = "VA"; // 8 Electrical
                public const string volt_amperes_reactive = "VAR"; // 11 Electrical
                public const string volts = "V"; // 5 Electrical
                public const string volts_per_degree_kelvin = "V/K"; // 176 Electrical
                public const string volts_per_meter = "V/m"; // 177 Electrical
                public const string webers = "Wb"; // 178 Electrical
            }
        }


        public static class Energy
        {
            public static class Name
            {
                public const string ampere_seconds = "ampere_seconds"; // 238 Energy
                public const string btus = "btus"; // 20 Energy
                public const string joules = "joules"; // 16 Energy
                public const string kilo_btus = "kilo_btus"; // 147 Energy
                public const string kilojoules = "kilojoules"; // 17 Energy
                public const string kilojoules_per_kilogram = "kilojoules_per_kilogram"; // 125 Energy
                public const string kilovolt_ampere_hours = "kilovolt_ampere_hours"; // 240 Energy
                public const string kilovolt_ampere_hours_reactive = "kilovolt_ampere_hours_reactive"; // 243 Energy
                public const string kilowatt_hours = "kilowatt_hours"; // 19 Energy
                public const string kilowatt_hours_reactive = "kilowatt_hours_reactive"; // 204 Energy
                public const string megajoules = "megajoules"; // 126 Energy
                public const string megavolt_ampere_hours = "megavolt_ampere_hours"; // 241 Energy
                public const string megavolt_ampere_hours_reactive = "megavolt_ampere_hours_reactive"; // 244 Energy
                public const string megawatt_hours = "megawatt_hours"; // 146 Energy
                public const string megawatt_hours_reactive = "megawatt_hours_reactive"; // 205 Energy
                public const string ton_hours = "ton_hours"; // 22 Energy
                public const string volt_ampere_hours = "volt_ampere_hours"; // 239 Energy
                public const string volt_ampere_hours_reactive = "volt_ampere_hours_reactive"; // 242 Energy
                public const string volt_square_hours = "volt_square_hours"; // 245 Energy
                public const string watt_hours = "watt_hours"; // 18 Energy
                public const string watt_hours_reactive = "watt_hours_reactive"; // 203 Energy
            }

            public static class Symbol
            {
                public const string ampere_seconds = "As"; // 238 Energy
                public const string btus = "BTU"; // 20 Energy
                public const string joules = "J"; // 16 Energy
                public const string kilo_btus = "kBTU"; // 147 Energy
                public const string kilojoules = "kJ"; // 17 Energy
                public const string kilojoules_per_kilogram = "kJ/kg"; // 125 Energy
                public const string kilovolt_ampere_hours = "kVAh"; // 240 Energy
                public const string kilovolt_ampere_hours_reactive = "kVARh"; // 243 Energy
                public const string kilowatt_hours = "kWh"; // 19 Energy
                public const string kilowatt_hours_reactive = "kWh"; // 204 Energy
                public const string megajoules = "MJ"; // 126 Energy
                public const string megavolt_ampere_hours = "MVAh"; // 241 Energy
                public const string megavolt_ampere_hours_reactive = "MVARh"; // 244 Energy
                public const string megawatt_hours = "MWh"; // 146 Energy
                public const string megawatt_hours_reactive = "MWh"; // 205 Energy
                public const string ton_hours = "ton-h"; // 22 Energy
                public const string volt_ampere_hours = "VAh"; // 239 Energy
                public const string volt_ampere_hours_reactive = "VARh"; // 242 Energy
                public const string volt_square_hours = "V²h"; // 245 Energy
                public const string watt_hours = "Wh"; // 18 Energy
                public const string watt_hours_reactive = "Wh"; // 203 Energy
            }
        }

        public static class EnergyDensity
        {
            public static class Name
            {
                public const string joules_per_cubic_meter = "joules_per_cubic_meter"; // 251 Other
                public const string kilowatt_hours_per_square_foot = "kilowatt_hours_per_square_foot"; // 138 Other
                public const string kilowatt_hours_per_square_meter = "kilowatt_hours_per_square_meter"; // 137 Other
                public const string megajoules_per_square_foot = "megajoules_per_square_foot"; // 140 Other
                public const string megajoules_per_square_meter = "megajoules_per_square_meter"; // 139 Other
                public const string watt_hours_per_cubic_meter = "watt_hours_per_cubic_meter"; // 250 Other
            }

            public static class Symbol
            {
                public const string joules_per_cubic_meter = "J/m³"; // 251 Other
                public const string kilowatt_hours_per_square_foot = "kWh/ft²"; // 138 Other
                public const string kilowatt_hours_per_square_meter = "kWh/m²"; // 137 Other
                public const string megajoules_per_square_foot = "MJ/ft²"; // 140 Other
                public const string megajoules_per_square_meter = "MJ/m²"; // 139 Other
                public const string watt_hours_per_cubic_meter = "Wh/m³"; // 250 Other
            }
        }

        public static class EnergySpecific
        {
            public static class Name
            {
                public const string joule_seconds = "joule_seconds"; // 183 Other
            }

            public static class Symbol
            {
                public const string joule_seconds = "Js"; // 183 Other
            }
        }

        public static class Enthalpy
        {
            public static class Name
            {
                public const string btus_per_pound = "btus_per_pound"; // 117 Enthalpy
                public const string btus_per_pound_dry_air = "btus_per_pound_dry_air"; // 24 Enthalpy
                public const string joules_per_degree_kelvin = "joules_per_degree_kelvin"; // 127 Entropy
                public const string joules_per_kilogram_dry_air = "joules_per_kilogram_dry_air"; // 23 Enthalpy
                public const string joules_per_kilogram_degree_kelvin = "joules_per_kilogram_degree_kelvin"; // 128 Entropy
                public const string kilojoules_per_degree_kelvin = "kilojoules_per_degree_kelvin"; // 151 Entropy
                public const string kilojoules_per_kilogram_dry_air = "kilojoules_per_kilogram_dry_air"; // 149 Enthalpy
                public const string megajoules_per_degree_kelvin = "megajoules_per_degree_kelvin"; // 152 Entropy
                public const string megajoules_per_kilogram_dry_air = "megajoules_per_kilogram_dry_air"; // 150 Enthalpy
            }

            public static class Symbol
            {
                public const string btus_per_pound = "BTU/lb"; // 117 Enthalpy
                public const string btus_per_pound_dry_air = "BTU/lb"; // 24 Enthalpy
                public const string joules_per_degree_kelvin = "J/K"; // 127 Entropy
                public const string joules_per_kilogram_dry_air = "J/kg"; // 23 Enthalpy
                public const string joules_per_kilogram_degree_kelvin = "J/(kg·K)"; // 128 Entropy
                public const string kilojoules_per_degree_kelvin = "kJ/K"; // 151 Entropy
                public const string kilojoules_per_kilogram_dry_air = "kJ/kg"; // 149 Enthalpy
                public const string megajoules_per_degree_kelvin = "MJ/K"; // 152 Entropy
                public const string megajoules_per_kilogram_dry_air = "MJ/kg"; // 150 Enthalpy
            }
        }

        public static class Force
        {
            public static class Name
            {
                public const string newton = "newton"; // 153 Force
            }

            public static class Symbol
            {
                public const string newton = "N"; // 153 Force
            }
        }


        public static class Frequency
        {
            public static class Name
            {
                public const string cycles_per_hour = "cycles_per_hour"; // 25 Frequency
                public const string cycles_per_minute = "cycles_per_minute"; // 26 Frequency
                public const string hertz = "hertz"; // 27 Frequency
                public const string kilohertz = "kilohertz"; // 129 Frequency
                public const string megahertz = "megahertz"; // 130 Frequency
                public const string per_hour = "per_hour"; // 131 Frequency
            }

            public static class Symbol
            {
                public const string cycles_per_hour = "cph"; // 25 Frequency
                public const string cycles_per_minute = "cpm"; // 26 Frequency
                public const string hertz = "Hz"; // 27 Frequency
                public const string kilohertz = "kHz"; // 129 Frequency
                public const string megahertz = "MHz"; // 130 Frequency
                public const string per_hour = "ph"; // 131 Frequency
            }
        }

        public static class General
        {
            public static class Name
            {
                public const string decibels_a = "decibels_a"; // 232 Other
                public const string grams_per_square_meter = "grams_per_square_meter"; // 235 Other
                public const string nephelometric_turbidity_unit = "nephelometric_turbidity_unit"; // 233 Other
                
                public const string pH = "pH"; // 234 Other
            }

            public static class Symbol
            {
                public const string decibels_a = "dBA"; // 232 Other
                public const string grams_per_square_meter = "g/m²"; // 235 Other
                public const string nephelometric_turbidity_unit = "NTU"; // 233 Other
                public const string pH = "pH"; // 234 Other
            }
        }

        public static class Humidity
        {
            public static class Name
            {
                public const string grams_of_water_per_kilogram_dry_air = "grams_of_water_per_kilogram_dry_air"; // 28 Humidity
                public const string percent_relative_humidity = "percent_relative_humidity"; // 29 Humidity
            }

            public static class Symbol
            {
                public const string grams_of_water_per_kilogram_dry_air = "g/kg"; // 28 Humidity
                public const string percent_relative_humidity = "%RH"; // 29 Humidity
            }
        }

        public static class Illuminance
        {
            public static class Name
            {
                public const string foot_candles = "foot_candles"; // Non-SI unit of illuminance
                public const string lux = "lux"; // SI unit of illuminance
            }

            public static class Symbol
            {
                public const string foot_candles = "fc"; // Non-SI unit of illuminance
                public const string lux = "lx"; // SI unit of illuminance
            }
        }

        public static class Inductance
        {
            public static class Name
            {
                public const string henrys = "henrys"; // SI unit of inductance
                public const string microhenrys = "microhenrys"; // One millionth of a henry
                public const string millihenrys = "millihenrys"; // One thousandth of a henry
            }

            public static class Symbol
            {
                public const string henrys = "H"; // SI unit of inductance
                public const string microhenrys = "µH"; // One millionth of a henry
                public const string millihenrys = "mH"; // One thousandth of a henry
            }
        }

        public static class Length
        {
            public static class Name
            {
                public const string centimeters = "centimeters"; // 118 Length
                public const string feet = "feet"; // 33 Length
                public const string inches = "inches"; // 32 Length
                public const string kilometers = "kilometers"; // 193 Length
                public const string meters = "meters"; // 31 Length
                public const string micrometers = "micrometers"; // 194 Length
                public const string millimeters = "millimeters"; // 30 Length
            }

            public static class Symbol
            {
                public const string centimeters = "cm"; // 118 Length
                public const string feet = "ft"; // 33 Length
                public const string inches = "in"; // 32 Length
                public const string kilometers = "km"; // 193 Length
                public const string meters = "m"; // 31 Length
                public const string micrometers = "µm"; // 194 Length
                public const string millimeters = "mm"; // 30 Length
            }
        }

        public static class Light
        {
            public static class Name
            {
                public const string candelas = "candelas"; // 179 Light
                public const string candelas_per_square_meter = "candelas_per_square_meter"; // 180 Light
                public const string foot_candles = "foot_candles"; // 38 Light
                public const string lumens = "lumens"; // 36 Light
                public const string luxes = "luxes"; // 37 Light
                public const string watts_per_square_foot = "watts_per_square_foot"; // 34 Light
                public const string watts_per_square_meter = "watts_per_square_meter"; // 35 Light
            }

            public static class Symbol
            {
                public const string candelas = "cd"; // 179 Light
                public const string candelas_per_square_meter = "cd/m²"; // 180 Light
                public const string foot_candles = "fc"; // 38 Light
                public const string lumens = "lm"; // 36 Light
                public const string luxes = "lx"; // 37 Light
                public const string watts_per_square_foot = "W/ft²"; // 34 Light
                public const string watts_per_square_meter = "W/m²"; // 35 Light
            }
        }


        public static class Luminance
        {
            public static class Name
            {
                public const string candelas_per_square_meter = "candelas_per_square_meter"; // SI unit of luminance
                public const string nits = "nits"; // Common unit of luminance
            }

            public static class Symbol
            {
                public const string candelas_per_square_meter = "cd/m²"; // SI unit of luminance
                public const string nits = "nt"; // Common unit of luminance
            }
        }

        public static class LuminousIntensity
        {
            public static class Name
            {
                public const string candela = "candela"; // SI base unit for luminous intensity
            }

            public static class Symbol
            {
                public const string candela = "cd"; // SI base unit for luminous intensity
            }
        }

        public static class MagneticFieldStrength
        {
            public static class Name
            {
                public const string amperes_per_meter = "amperes_per_meter"; // SI unit of magnetic field strength
                public const string oersteds = "oersteds"; // CGS unit of magnetic field strength
            }

            public static class Symbol
            {
                public const string amperes_per_meter = "A/m"; // SI unit of magnetic field strength
                public const string oersteds = "Oe"; // CGS unit of magnetic field strength
            }
        }

        public static class MagneticFlux
        {
            public static class Name
            {
                public const string maxwells = "maxwells"; // CGS unit of magnetic flux
                public const string webers = "webers"; // SI unit of magnetic flux
            }

            public static class Symbol
            {
                public const string maxwells = "Mx"; // CGS unit of magnetic flux
                public const string webers = "Wb"; // SI unit of magnetic flux
            }
        }

        public static class Mass
        {
            public static class Name
            {
                public const string grams = "grams"; // 195 Mass
                public const string kilograms = "kilograms"; // 39 Mass
                public const string milligrams = "milligrams"; // 196 Mass
                public const string pounds_mass = "pounds_mass"; // 40 Mass
                public const string tons = "tons"; // 41 Mass
            }

            public static class Symbol
            {
                public const string grams = "g"; // 195 Mass
                public const string kilograms = "kg"; // 39 Mass
                public const string milligrams = "mg"; // 196 Mass
                public const string pounds_mass = "lb"; // 40 Mass
                public const string tons = "t"; // 41 Mass
            }
        }

        public static class MassDensity
        {
            public static class Name
            {
                public const string grams_per_cubic_centimeter = "grams_per_cubic_centimeter"; // 221 Other
                public const string grams_per_cubic_meter = "grams_per_cubic_meter"; // 217 Other
                public const string kilograms_per_cubic_meter = "kilograms_per_cubic_meter"; // 186 Other
                public const string micrograms_per_cubic_meter = "micrograms_per_cubic_meter"; // 219 Other
                public const string milligrams_per_cubic_meter = "milligrams_per_cubic_meter"; // 218 Other
                public const string nanograms_per_cubic_meter = "nanograms_per_cubic_meter"; // 220 Other
            }

            public static class Symbol
            {
                public const string grams_per_cubic_centimeter = "g/cm³"; // 221 Other
                public const string grams_per_cubic_meter = "g/m³"; // 217 Other
                public const string kilograms_per_cubic_meter = "kg/m³"; // 186 Other
                public const string micrograms_per_cubic_meter = "µg/m³"; // 219 Other
                public const string milligrams_per_cubic_meter = "mg/m³"; // 218 Other
                public const string nanograms_per_cubic_meter = "ng/m³"; // 220 Other
            }
        }

        public static class MassFlow
        {
            public static class Name
            {
                public const string grams_per_minute = "grams_per_minute"; // 155 Mass Flow
                public const string grams_per_second = "grams_per_second"; // 154 Mass Flow
                public const string kilograms_per_hour = "kilograms_per_hour"; // 44 Mass Flow
                public const string kilograms_per_minute = "kilograms_per_minute"; // 43 Mass Flow
                public const string kilograms_per_second = "kilograms_per_second"; // 42 Mass Flow
                public const string pounds_mass_per_hour = "pounds_mass_per_hour"; // 46 Mass Flow
                public const string pounds_mass_per_minute = "pounds_mass_per_minute"; // 45 Mass Flow
                public const string pounds_mass_per_second = "pounds_mass_per_second"; // 119 Mass Flow
                public const string tons_per_hour = "tons_per_hour"; // 156 Mass Flow
            }

            public static class Symbol
            {
                public const string grams_per_minute = "g/min"; // 155 Mass Flow
                public const string grams_per_second = "g/s"; // 154 Mass Flow
                public const string kilograms_per_hour = "kg/h"; // 44 Mass Flow
                public const string kilograms_per_minute = "kg/min"; // 43 Mass Flow
                public const string kilograms_per_second = "kg/s"; // 42 Mass Flow
                public const string pounds_mass_per_hour = "lb/h"; // 46 Mass Flow
                public const string pounds_mass_per_minute = "lb/min"; // 45 Mass Flow
                public const string pounds_mass_per_second = "lb/s"; // 119 Mass Flow
                public const string tons_per_hour = "t/h"; // 156 Mass Flow
            }
        }

        public static class MassFraction
        {
            public static class Name
            {
                public const string grams_per_gram = "grams_per_gram"; // 208 Other
                public const string grams_per_kilogram = "grams_per_kilogram"; // 210 Other
                public const string grams_per_liter = "grams_per_liter"; // 214 Other
                public const string grams_per_milliliter = "grams_per_milliliter"; // 213 Other
                public const string kilograms_per_kilogram = "kilograms_per_kilogram"; // 209 Other
                public const string micrograms_per_liter = "micrograms_per_liter"; // 216 Other
                public const string milligrams_per_gram = "milligrams_per_gram"; // 211 Other
                public const string milligrams_per_kilogram = "milligrams_per_kilogram"; // 212 Other
                public const string milligrams_per_liter = "milligrams_per_liter"; // 215 Other
            }

            public static class Symbol
            {
                public const string grams_per_gram = "g/g"; // 208 Other
                public const string grams_per_kilogram = "g/kg"; // 210 Other
                public const string grams_per_liter = "g/L"; // 214 Other
                public const string grams_per_milliliter = "g/mL"; // 213 Other
                public const string kilograms_per_kilogram = "kg/kg"; // 209 Other
                public const string micrograms_per_liter = "µg/L"; // 216 Other
                public const string milligrams_per_gram = "mg/g"; // 211 Other
                public const string milligrams_per_kilogram = "mg/kg"; // 212 Other
                public const string milligrams_per_liter = "mg/L"; // 215 Other
            }
        }


        public static class PhysicalProperties
        {
            public static class Name
            {
                public const string newton_seconds = "newton_seconds"; // 187 Other
                public const string newtons_per_meter = "newtons_per_meter"; // 188 Other
                public const string pascal_seconds = "pascal_seconds"; // 253 Other
                public const string square_meters_per_newton = "square_meters_per_newton"; // 185 Other
                public const string watts_per_meter_per_degree_kelvin = "watts_per_meter_per_degree_kelvin"; // 189 Other
                public const string watts_per_square_meter_degree_kelvin = "watts_per_square_meter_degree_kelvin"; // 141 Other
            }

            public static class Symbol
            {
                public const string newton_seconds = "N·s"; // 187 Other
                public const string newtons_per_meter = "N/m"; // 188 Other
                public const string pascal_seconds = "Pa·s"; // 253 Other
                public const string square_meters_per_newton = "m²/N"; // 185 Other
                public const string watts_per_meter_per_degree_kelvin = "W/(m·K)"; // 189 Other
                public const string watts_per_square_meter_degree_kelvin = "W/(m²·K)"; // 141 Other
            }
        }

        public static class Power
        {
            public static class Name
            {
                public const string horsepower = "horsepower"; // 51 Power
                public const string joule_per_hours = "joule_per_hours"; // 247 Power
                public const string kilo_btus_per_hour = "kilo_btus_per_hour"; // 157 Power
                public const string kilowatts = "kilowatts"; // 48 Power
                public const string megawatts = "megawatts"; // 49 Power
                public const string milliwatts = "milliwatts"; // 132 Power
                public const string tons_refrigeration = "tons_refrigeration"; // 52 Power
                public const string watts = "watts"; // 47 Power
                public const string btus_per_hour = "btus_per_hour"; // 50 Power
            }

            public static class Symbol
            {
                public const string horsepower = "hp"; // 51 Power
                public const string joule_per_hours = "J/h"; // 247 Power
                public const string kilo_btus_per_hour = "kBTU/h"; // 157 Power
                public const string kilowatts = "kW"; // 48 Power
                public const string megawatts = "MW"; // 49 Power
                public const string milliwatts = "mW"; // 132 Power
                public const string tons_refrigeration = "TR"; // 52 Power
                public const string watts = "W"; // 47 Power
                public const string btus_per_hour = "BTU/h"; // 50 Power
            }
        }

        public static class Pressure
        {
            public static class Name
            {
                public const string bars = "bars"; // 55 Pressure
                public const string centimeters_of_mercury = "centimeters_of_mercury"; // 60 Pressure
                public const string centimeters_of_water = "centimeters_of_water"; // 57 Pressure
                public const string hectopascals = "hectopascals"; // 133 Pressure
                public const string inches_of_mercury = "inches_of_mercury"; // 61 Pressure
                public const string inches_of_water = "inches_of_water"; // 58 Pressure
                public const string kilopascals = "kilopascals"; // 54 Pressure
                public const string millibars = "millibars"; // 134 Pressure
                public const string millimeters_of_mercury = "millimeters_of_mercury"; // 59 Pressure
                public const string millimeters_of_water = "millimeters_of_water"; // 206 Pressure
                public const string pascals = "pascals"; // 53 Pressure
                public const string pounds_force_per_square_inch = "pounds_force_per_square_inch"; // 56 Pressure
            }

            public static class Symbol
            {
                public const string bars = "bar"; // 55 Pressure
                public const string centimeters_of_mercury = "cmHg"; // 60 Pressure
                public const string centimeters_of_water = "cmH₂O"; // 57 Pressure
                public const string hectopascals = "hPa"; // 133 Pressure
                public const string inches_of_mercury = "inHg"; // 61 Pressure
                public const string inches_of_water = "inH₂O"; // 58 Pressure
                public const string kilopascals = "kPa"; // 54 Pressure
                public const string millibars = "mbar"; // 134 Pressure
                public const string millimeters_of_mercury = "mmHg"; // 59 Pressure
                public const string millimeters_of_water = "mmH₂O"; // 206 Pressure
                public const string pascals = "Pa"; // 53 Pressure
                public const string pounds_force_per_square_inch = "psi"; // 56 Pressure
            }
        }

        public static class Radiation
        {
            public static class Name
            {
                public const string becquerels = "becquerels"; // 222 Other / SI unit of radioactivity
                public const string curies = "curies"; // Traditional unit of radioactivity
                public const string gray = "gray"; // 225 Other / SI unit of absorbed radiation dose
                public const string kilobecquerels = "kilobecquerels"; // 223 Other
                public const string megabecquerels = "megabecquerels"; // 224 Other
                public const string milligray = "milligray"; // 226 Other
                public const string millirems = "millirems"; // 47814 Other
                public const string millirems_per_hour = "millirems_per_hour"; // 47815 Other
                public const string millisieverts = "millisieverts"; // 229 Other
                public const string microsieverts = "microsieverts"; // 230 Other
                public const string microsieverts_per_hour = "microsieverts_per_hour"; // 231 Other
                public const string microgray = "microgray"; // 227 Other
                public const string rads = "rads"; // Traditional unit of absorbed radiation dose
                public const string rems = "rems"; // Traditional unit of radiation dose equivalent
                public const string sieverts = "sieverts"; // 228 Other / SI unit of radiation dose equivalent
            }

            public static class Symbol
            {
                public const string becquerels = "Bq"; // 222 Other / SI unit of radioactivity
                public const string curies = "Ci"; // Traditional unit of radioactivity
                public const string gray = "Gy"; // 225 Other / SI unit of absorbed radiation dose
                public const string kilobecquerels = "kBq"; // 223 Other
                public const string megabecquerels = "MBq"; // 224 Other
                public const string milligray = "mGy"; // 226 Other
                public const string millirems = "mrem"; // 47814 Other
                public const string millirems_per_hour = "mrem/h"; // 47815 Other
                public const string millisieverts = "mSv"; // 229 Other
                public const string microsieverts = "µSv"; // 230 Other
                public const string microsieverts_per_hour = "µSv/h"; // 231 Other
                public const string microgray = "µGy"; // 227 Other
                public const string rads = "rad"; // Traditional unit of absorbed radiation dose
                public const string rems = "rem"; // Traditional unit of radiation dose equivalent
                public const string sieverts = "Sv"; // 228 Other / SI unit of radiation dose equivalent
            }
        }

        public static class RadiantIntensity
        {
            public static class Name
            {
                public const string microwatts_per_steradian = "microwatts_per_steradian";
                public const string watts_per_steradian = "watts_per_steradian";
            }

            public static class Symbol
            {
                public const string microwatts_per_steradian = "µW/sr";
                public const string watts_per_steradian = "W/sr";
            }
        }

        public static class Temperature
        {
            public static class Name
            {
                public const string degree_days_celsius = "degree_days_celsius"; // 65 Temperature
                public const string degree_days_fahrenheit = "degree_days_fahrenheit"; // 66 Temperature
                public const string degrees_celsius = "degrees_celsius"; // 62 Temperature
                public const string degrees_fahrenheit = "degrees_fahrenheit"; // 64 Temperature
                public const string degrees_kelvin = "degrees_kelvin"; // 63 Temperature
                public const string degrees_rankine = "degrees_rankine"; // Non-SI unit for thermodynamic temperature
                public const string delta_degrees_fahrenheit = "delta_degrees_fahrenheit"; // 120 Temperature
                public const string delta_degrees_kelvin = "delta_degrees_kelvin"; // 121 Temperature
            }

            public static class Symbol
            {
                public const string degree_days_celsius = "°C·d"; // 65 Temperature
                public const string degree_days_fahrenheit = "°F·d"; // 66 Temperature
                public const string degrees_celsius = "°C"; // 62 Temperature
                public const string degrees_fahrenheit = "°F"; // 64 Temperature
                public const string degrees_kelvin = "K"; // 63 Temperature
                public const string degrees_rankine = "°R"; // Non-SI unit for thermodynamic temperature
                public const string delta_degrees_fahrenheit = "∆°F"; // 120 Temperature
                public const string delta_degrees_kelvin = "∆K"; // 121 Temperature
            }
        }

        public static class TemperatureRate
        {
            public static class Name
            {
                public const string degrees_celsius_per_hour = "degrees_celsius_per_hour"; // 91 Other
                public const string degrees_celsius_per_minute = "degrees_celsius_per_minute"; // 92 Other
                public const string degrees_fahrenheit_per_hour = "degrees_fahrenheit_per_hour"; // 93 Other
                public const string degrees_fahrenheit_per_minute = "degrees_fahrenheit_per_minute"; // 94 Other
                public const string minutes_per_degree_kelvin = "minutes_per_degree_kelvin"; // 236 Other
                public const string psi_per_degree_fahrenheit = "psi_per_degree_fahrenheit"; // 102 Other
            }

            public static class Symbol
            {
                public const string degrees_celsius_per_hour = "°C/h"; // 91 Other
                public const string degrees_celsius_per_minute = "°C/min"; // 92 Other
                public const string degrees_fahrenheit_per_hour = "°F/h"; // 93 Other
                public const string degrees_fahrenheit_per_minute = "°F/min"; // 94 Other
                public const string minutes_per_degree_kelvin = "min/K"; // 236 Other
                public const string psi_per_degree_fahrenheit = "psi/°F"; // 102 Other
            }
        }

        public static class Time
        {
            public static class Name
            {
                public const string days = "days"; // 70 Time
                public const string hours = "hours"; // 71 Time
                public const string hundredths_seconds = "hundredths_seconds"; // 158 Time
                public const string milliseconds = "milliseconds"; // 159 Time
                public const string minutes = "minutes"; // 72 Time
                public const string months = "months"; // 68 Time
                public const string seconds = "seconds"; // 73 Time
                public const string weeks = "weeks"; // 69 Time
                public const string years = "years"; // 67 Time
            }

            public static class Symbol
            {
                public const string days = "d"; // 70 Time
                public const string hours = "h"; // 71 Time
                public const string hundredths_seconds = "hs"; // 158 Time
                public const string milliseconds = "ms"; // 159 Time
                public const string minutes = "min"; // 72 Time
                public const string months = "mo"; // 68 Time
                public const string seconds = "s"; // 73 Time
                public const string weeks = "wk"; // 69 Time
                public const string years = "yr"; // 67 Time
            }
        }


        public static class Torque
        {
            public static class Name
            {
                public const string newton_meters = "newton_meters"; // 160 Torque
            }

            public static class Symbol
            {
                public const string newton_meters = "N·m"; // 160 Torque
            }
        }

        public static class Velocity
        {
            public static class Name
            {
                public const string feet_per_minute = "feet_per_minute"; // 77 Velocity
                public const string feet_per_second = "feet_per_second"; // 76 Velocity
                public const string millimeters_per_second = "millimeters_per_second"; // 161 Velocity
                public const string kilometers_per_hour = "kilometers_per_hour"; // 75 Velocity
                public const string meters_per_hour = "meters_per_hour"; // 164 Velocity
                public const string meters_per_minute = "meters_per_minute"; // 163 Velocity
                public const string meters_per_second = "meters_per_second"; // 74 Velocity
                public const string miles_per_hour = "miles_per_hour"; // 78 Velocity
                public const string millimeters_per_minute = "millimeters_per_minute"; // 162 Velocity
            }

            public static class Symbol
            {
                public const string feet_per_minute = "ft/min"; // 77 Velocity
                public const string feet_per_second = "ft/s"; // 76 Velocity
                public const string millimeters_per_second = "mm/s"; // 161 Velocity
                public const string kilometers_per_hour = "km/h"; // 75 Velocity
                public const string meters_per_hour = "m/h"; // 164 Velocity
                public const string meters_per_minute = "m/min"; // 163 Velocity
                public const string meters_per_second = "m/s"; // 74 Velocity
                public const string miles_per_hour = "mph"; // 78 Velocity
                public const string millimeters_per_minute = "mm/min"; // 162 Velocity
            }
        }

        public static class Volume
        {
            public static class Name
            {
                public const string cubic_feet = "cubic_feet"; // 79 Volume
                public const string cubic_meters = "cubic_meters"; // 80 Volume
                public const string imperial_gallons = "imperial_gallons"; // 81 Volume
                public const string liters = "liters"; // 82 Volume
                public const string milliliters = "milliliters"; // 197 Volume
                public const string us_gallons = "us_gallons"; // 83 Volume
            }

            public static class Symbol
            {
                public const string cubic_feet = "ft³"; // 79 Volume
                public const string cubic_meters = "m³"; // 80 Volume
                public const string imperial_gallons = "gal (imp)"; // 81 Volume
                public const string liters = "L"; // 82 Volume
                public const string milliliters = "mL"; // 197 Volume
                public const string us_gallons = "gal (US)"; // 83 Volume
            }
        }

        public static class VolumeSpecific
        {
            public static class Name
            {
                public const string cubic_feet_per_pound = "cubic_feet_per_pound";
                public const string cubic_meters_per_kilogram = "cubic_meters_per_kilogram";
            }

            public static class Symbol
            {
                public const string cubic_feet_per_pound = "ft³/lb";
                public const string cubic_meters_per_kilogram = "m³/kg";
            }
        }

        public static class VolumetricFlow
        {
            public static class Name
            {
                public const string cubic_feet_per_day = "cubic_feet_per_day"; // 248 Volumetric Flow
                public const string cubic_feet_per_hour = "cubic_feet_per_hour"; // 191 Volumetric Flow
                public const string cubic_feet_per_minute = "cubic_feet_per_minute"; // 84 Volumetric Flow
                public const string cubic_feet_per_second = "cubic_feet_per_second"; // 142 Volumetric Flow
                public const string cubic_meters_per_day = "cubic_meters_per_day"; // 249 Volumetric Flow
                public const string cubic_meters_per_hour = "cubic_meters_per_hour"; // 135 Volumetric Flow
                public const string cubic_meters_per_minute = "cubic_meters_per_minute"; // 165 Volumetric Flow
                public const string cubic_meters_per_second = "cubic_meters_per_second"; // 85 Volumetric Flow
                public const string imperial_gallons_per_minute = "imperial_gallons_per_minute"; // 86 Volumetric Flow
                public const string liters_per_hour = "liters_per_hour"; // 136 Volumetric Flow
                public const string liters_per_minute = "liters_per_minute"; // 88 Volumetric Flow
                public const string liters_per_second = "liters_per_second"; // 87 Volumetric Flow
                public const string milliliters_per_second = "milliliters_per_second"; // 198 Volumetric Flow
                public const string million_standard_cubic_feet_per_day = "million_standard_cubic_feet_per_day"; // 47809 Volumetric Flow
                public const string million_standard_cubic_feet_per_minute = "million_standard_cubic_feet_per_minute"; // 254 Volumetric Flow
                public const string pounds_mass_per_day = "pounds_mass_per_day"; // 47812 Volumetric Flow
                public const string standard_cubic_feet_per_day = "standard_cubic_feet_per_day"; // 47808 Volumetric Flow
                public const string thousand_cubic_feet_per_day = "thousand_cubic_feet_per_day"; // 47810 Volumetric Flow
                public const string thousand_standard_cubic_feet_per_day = "thousand_standard_cubic_feet_per_day"; // 47811 Volumetric Flow
                public const string us_gallons_per_hour = "us_gallons_per_hour"; // 192 Volumetric Flow
                public const string us_gallons_per_minute = "us_gallons_per_minute"; // 89 Volumetric Flow
            }

            public static class Symbol
            {
                public const string cubic_feet_per_day = "ft³/d"; // 248 Volumetric Flow
                public const string cubic_feet_per_hour = "ft³/h"; // 191 Volumetric Flow
                public const string cubic_feet_per_minute = "cfm"; // 84 Volumetric Flow
                public const string cubic_feet_per_second = "cfs"; // 142 Volumetric Flow
                public const string cubic_meters_per_day = "m³/d"; // 249 Volumetric Flow
                public const string cubic_meters_per_hour = "m³/h"; // 135 Volumetric Flow
                public const string cubic_meters_per_minute = "m³/min"; // 165 Volumetric Flow
                public const string cubic_meters_per_second = "m³/s"; // 85 Volumetric Flow
                public const string imperial_gallons_per_minute = "gal (imp)/min"; // 86 Volumetric Flow
                public const string liters_per_hour = "L/h"; // 136 Volumetric Flow
                public const string liters_per_minute = "L/min"; // 88 Volumetric Flow
                public const string liters_per_second = "L/s"; // 87 Volumetric Flow
                public const string milliliters_per_second = "mL/s"; // 198 Volumetric Flow
                public const string million_standard_cubic_feet_per_day = "MMscfd"; // 47809 Volumetric Flow
                public const string million_standard_cubic_feet_per_minute = "MMscfm"; // 254 Volumetric Flow
                public const string pounds_mass_per_day = "lb/d"; // 47812 Volumetric Flow
                public const string standard_cubic_feet_per_day = "scfd"; // 47808 Volumetric Flow
                public const string thousand_cubic_feet_per_day = "Mscfd"; // 47810 Volumetric Flow
                public const string thousand_standard_cubic_feet_per_day = "Mscfd"; // 47811 Volumetric Flow
                public const string us_gallons_per_hour = "gal (US)/h"; // 192 Volumetric Flow
                public const string us_gallons_per_minute = "gpm"; // 89 Volumetric Flow
            }
        }


        public const string no_units = "no_units"; // 95 Other
    }

    #endregion
}

﻿using Iot.Database.Helper;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Iot.Database;

[Serializable]
public partial class IotValue
{
    public string Guid { get; set; } = System.Guid.NewGuid().ToString();
    public string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public string?[] Values { get; set; } = new string?[16];
    public DateTime?[] Timestamps { get; set; } = new DateTime?[16];
    public IotUnit Unit { get; set; } = Units.no_unit;
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

    public IotValue (string name, string description, object? value, IotUnit unit)
    {
        InitValues();
        Name = name;
        Description = description;
        SetValue(16, value);
        Unit = unit;
        AllowManualOperator = true;
    }
    public IotValue(string name, string description, object? value, IotUnit unit, bool isPasswordValue, bool allowManualOperator, bool timeSeries, bool blockChain, bool logChange)
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

        if (!AllowManualOperator && (index == 7 || index == 0))
        {
            Values[index] = null;
            Timestamps[index] = null;
            return false; //manual operator 
        }

        Values[index] = value;
        Timestamps[index] = value == null?null: DateTime.UtcNow;
        

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

    [JsonIgnore]
    [BsonIgnore]
    public bool IsValueInterpolated
    {
        get { return ValueInterpolated; }
        set { ValueInterpolated = value; }
    }

    [JsonIgnore]
    [BsonIgnore]
    public bool ValueInterpolated
    {
        get => Flags.IsEnabled(IotValueFlags.ValueInterpolated);
        set => Flags = value ? Flags.Enable(IotValueFlags.ValueInterpolated) : Flags.Disable(IotValueFlags.ValueInterpolated);
    }

    [JsonIgnore]
    [BsonIgnore]
    public bool IsPriority9Only
    {
        get { return Priority9Only; }
        set { Priority9Only = value; }
    }

    [JsonIgnore]
    [BsonIgnore]
    public bool Priority9Only
    {
        get => Flags.IsEnabled(IotValueFlags.Priority9Only);
        set => Flags = value ? Flags.Enable(IotValueFlags.Priority9Only) : Flags.Disable(IotValueFlags.Priority9Only);
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
    /// Check if value is manually overriden by operator
    /// </summary>
    [JsonIgnore]
    [BsonIgnore]
    public bool IsManual => !IsPriority9Only && (Priority == 1 || Priority == 8);

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
    public bool IsString => !IsNull && !IsBoolean && !IsDateTime && !IsGuid && !IsNumeric && !IsChar && !IsJson;

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
        Validate(priority, value);
        if (IsPriority9Only && priority != 9) return false;
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
        Validate(priority, value);
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
        Validate(priority, value);
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
        Validate(priority, password);
        int index = priority - 1;
        IsPasswordValue = true;
        return SetRawValue(index, ToPasswordHash(password));

    }

    /// <summary>
    /// Priority 1: Manual Operator Override (Highest priority)
    /// </summary>
    /// <param name="value"></param>
    /// <returns>true/fals</returns>
    public bool SetValue01ManualOperatorOverride(object? value) => SetValue(1, value);

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
    /// Validate value and priority
    /// </summary>
    /// <param name="priority"></param>
    /// <param name="value"></param>
    /// <exception cref="ArgumentException"></exception>
    private void Validate(int priority, object? value)
    {
        if (IsPriority9Only && !(priority == 9 || priority == 16))
        {
            throw new ArgumentException($"Invalid priority. Expected priority 9 or fallback only.");
        }
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

}

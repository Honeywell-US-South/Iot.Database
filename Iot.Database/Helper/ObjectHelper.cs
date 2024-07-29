using System.Text.Json;

namespace Iot.Database.Helper
{
    public static class ObjectHelper
    {
        public static T DeepCopy<T>(T obj)
        {
            
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", nameof(obj));
            }

            
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true, 
                WriteIndented = false 
            };

            // Serialize the object to JSON
            var json = System.Text.Json.JsonSerializer.Serialize(obj, options);

            // Deserialize JSON to create a new instance
            return System.Text.Json.JsonSerializer.Deserialize<T>(json, options) ?? default!;
        }
    }
}

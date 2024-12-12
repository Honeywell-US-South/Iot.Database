using System.Reflection;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Iot.Database.IotValueUnits
{
    public class ValueBase
    {
        public string Guid { get; set; } = System.Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public string JsonValue { get; set; } = string.Empty;
        public string JsonTimestamp { get; set; } = string.Empty;
        public string JsonQueryConfiguration { get; set; } = string.Empty;
        public string? TypeName { get; set; }

        #region Vector
        [JsonIgnore]
        [XmlIgnore]
        public List<float>? Embedding { get; set; }
        #endregion

        #region Functions

        public T? GetProperty<T>(string propertyName, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        {
            var idProperty = this.GetType().GetProperty(propertyName, bindingFlags);
            if (idProperty != null && idProperty.PropertyType == typeof(T))
            {
                return (T?)idProperty.GetValue(this);
            }
            return default(T?);
        }

        public Guid? GetIotDbId()
        {
            return GetProperty<Guid>("Id");
        }
        public bool SetProperty(string propertyName, object value, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        {
            var idProperty = this.GetType().GetProperty(propertyName, bindingFlags);
            if (idProperty != null && idProperty.PropertyType == typeof(object))
            {
                idProperty.SetValue(this, value);
                return true;
            }
            return false;
        }

        public bool SetIotDbId(Guid id)
        {
            return SetProperty("Id", id);
        }

        

        public virtual void CopyFrom<T>(T source) where T : ValueBase
        {
            // Ensure that the runtime types are compatible
            if (source == null || !this.GetType().IsAssignableFrom(source.GetType()))
            {
                throw new InvalidOperationException("Source object is not compatible with the target object.");
            }

            var properties = this.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.CanWrite)
                {
                    var value = property.GetValue(source);
                    property.SetValue(this, value);
                }
            }
        }
        #endregion
    }
}

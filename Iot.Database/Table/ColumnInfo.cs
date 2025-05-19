using System.Reflection;
using Iot.Database.Attributes;

namespace Iot.Database.Table
{
    public class ColumnInfo
    {
        public string Name { get; set; }
        public Attribute? Attribute { get; set; }
        public PropertyInfo PropertyInfo { get; set; }

        // Convenience property for TableForeignKeyAttribute
        public TableForeignKeyAttribute? ForeignKeyAttribute => Attribute as TableForeignKeyAttribute;

        public ColumnInfo() { }

        public ColumnInfo(PropertyInfo propertyInfo, Attribute? attribute = null)
        {
            Name = propertyInfo.Name;
            Attribute = attribute;
            PropertyInfo = propertyInfo;
        }
    }
}
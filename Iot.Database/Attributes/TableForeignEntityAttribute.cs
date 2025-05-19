namespace Iot.Database.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TableForeignEntityAttribute : Attribute
    {
        public List<string> ForeignKeys { get; set; } = new List<string>();
        public TableForeignEntityAttribute(params string[] foreignKey)
        {
            ForeignKeys = foreignKey.ToList();
        }
    }
}

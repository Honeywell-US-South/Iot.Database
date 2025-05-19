namespace Iot.Database.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TableChildNavigationAttribute : Attribute
    {
        public List<string> ForeignKeys { get; set; } = new List<string>();
        public TableChildNavigationAttribute(params string[] foreignKey)
        {
            ForeignKeys = foreignKey.ToList();
        }
    }
}

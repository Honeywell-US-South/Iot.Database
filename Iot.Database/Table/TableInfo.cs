using Iot.Database.Attributes;
using System.Reflection;

namespace Iot.Database.Table;

public class TableInfo
{
    public string Name { get; set; }
    public Type Type { get; set; }
    public ColumnInfo? Id { get; set; }
    public List<ColumnInfo> Uniques { get; set; } = new();
    public List<ColumnInfo> ForeignKeys { get; set; } = new();
    public List<ColumnInfo> Columns { get; set; } = new();
    public List<ColumnInfo> ForeignTables { get; set; } = new();
    public List<ColumnInfo> ChildTables { get; set; } = new();
    public List<TableInfo> Children { get; set; } = new();

    public TableInfo() { }

    public TableInfo(Type type, string tblName = "")
    {
        Name = string.IsNullOrEmpty(tblName) ? type.Name : tblName;
        Type = type;

        // Id
        var id = BaseDatabase.GetIdProperty(type);
        Id = id != null ? new ColumnInfo(id) : null;

        // Uniques and Foreign Keys
        Uniques = Helper.ReflectionHelper.GetTypeColumnsWithAttribute<UniqueValueAttribute>(type).ToList();
        ForeignKeys = Helper.ReflectionHelper.GetTypeColumnsWithAttribute<TableForeignKeyAttribute>(type).ToList();

        // Properties
        PropertyInfo[] properties = type.GetProperties();
        ForeignTables = Helper.ReflectionHelper.GetTypeColumnsWithAttribute<TableForeignEntityAttribute>(type).ToList();
        ChildTables = Helper.ReflectionHelper.GetTypeColumnsWithAttribute<TableChildNavigationAttribute>(type).ToList();
        // Foreign Tables (collections like List<T>)
        //foreach (var property in properties)
        //{
        //    if (property.PropertyType.IsGenericType &&
        //        property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
        //    {
        //        Type refTableType = property.PropertyType.GetGenericArguments()[0];
        //        if (Id != null && BaseDatabase.GetRefTableIdProperty(type, refTableType) != null)
        //        {
        //            if (property.Name.Equals($"{refTableType.Name}Table"))
        //            {
        //                ForeignTables.Add(new ColumnInfo(property));
        //            }
        //        }
        //    }
        //}

        // Columns (non-Id, non-unique, non-foreign key, non-foreign table)
        foreach (var property in properties)
        {
            if (property.Name != "Id" &&
                !Uniques.Any(x => x.Name == property.Name) &&
                !ForeignKeys.Any(x => x.Name == property.Name) &&
                !ForeignTables.Any(x => x.Name == property.Name))
            {
                Columns.Add(new ColumnInfo(property));
            }
        }
    }

    // Helper method to add a child table with validation
    public void AddChildTable(TableInfo childTable)
    {
        if (!Children.Any(ct => ct.Name == childTable.Name))
        {
            // Validate that the child table has a foreign key referencing this table
            var fk = childTable.ForeignKeys.FirstOrDefault(fk => fk.Name == $"{Name}Id");
            if (fk != null)
            {
                Children.Add(childTable);
            }
            else
            {
                throw new InvalidOperationException($"Child table '{childTable.Name}' does not have a foreign key referencing parent table '{Name}'.");
            }
        }
    }
}
namespace Iot.Database.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TableForeignKeyAttribute : Attribute
    {
        public Type Type { get; set; }
        public string? TableName { get; set; } // New property for custom table name
        public TableConstraint Constraint { get; set; } = TableConstraint.NoAction;
        public RelationshipOneTo RelationshipOneTo { get; set; } = RelationshipOneTo.Many;
        public string Description { get; set; } = string.Empty;

    
        public TableForeignKeyAttribute(Type type, string? tableName = null,
            TableConstraint constraint = TableConstraint.Cascading,
            RelationshipOneTo relationshipOneTo = RelationshipOneTo.Many,
            string description = "")
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            TableName = tableName ?? type.Name; // Default to type name if tableName is null
            Constraint = constraint;
            RelationshipOneTo = relationshipOneTo;
            Description = description;
        }
    }
}
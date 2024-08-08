namespace Iot.Database.Attributes
{
    [AttributeUsage(AttributeTargets.Property)] // Apply to properties only.
    public class NonTableFieldAttribute : Attribute
    {
        public string Description { get; set; } = string.Empty;

        public NonTableFieldAttribute() { }
        public NonTableFieldAttribute(string description)
        {
            Description = description;
        }
    }
}

namespace Iot.Database.Attributes
{
    [AttributeUsage(AttributeTargets.Property)] // Apply to properties only.
    public class UniqueValueAttribute : Attribute
    {
        public string Description { get; set; } = string.Empty;

        public UniqueValueAttribute() { }
        public UniqueValueAttribute(string description)
        {
            Description = description;
        }
    }
}

namespace Iot.Database.Attributes;

[AttributeUsage(AttributeTargets.Property)] // Apply to properties only.
public class BlockChainValueAttribute : Attribute
{
    public string Description { get; set; } = string.Empty;

    public BlockChainValueAttribute() { }
    public BlockChainValueAttribute(string description)
    {
        Description = description;
    }
}

namespace ExampleShared;

public class Friend
{
    public Guid Id { get; set; } //All Iot.Database table must have an Id property. It can can be of type int, long, and System.Guid
    public string Name { get; set; }
}

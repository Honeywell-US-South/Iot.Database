namespace Example.Shared.Models
{
    public class Friend
    {
        public Guid Id { get; set; } //A table must have an Id of type (int, long or System.Guid)
        public string Name { get; set; }
    }
}

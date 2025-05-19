using Iot.Database.Attributes;

namespace Example.Shared.Models
{
    public class Address
    {
        public int Id { get; set; }
        [TableForeignKey(typeof(Customer), "Customer", TableConstraint.Cascading)]
        public int CustomerId { get; set; }
        public string AddressLine1 { get; set; }
    }
}

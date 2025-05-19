using Iot.Database.Attributes;

namespace Example.Shared.Models
{
    public class Order
    {
        public int Id { get; set; }
        [TableForeignKey(typeof(Customer), "Customer", TableConstraint.Cascading)]
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
    }
}

using Iot.Database.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Shared.Models
{
    public class Contact
    {
        public Guid Id { get; set; } //A table must have an Id of type (int, long or System.Guid)
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [TableChildNavigation("ContactId")]
        public List<Friend> Friends { get; set; } = new List<Friend>(); // Navigation property
    }
}

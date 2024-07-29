using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

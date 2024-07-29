﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Database.Attributes
{
    [AttributeUsage(AttributeTargets.Property)] // Apply to properties only.
    public class TableForeignKeyAttribute : Attribute
    {
        public Type Type { get; set; }
        public TableConstraint Constraint { get; set; } = TableConstraint.NoAction;
        public RelationshipOneTo RelationshipOneTo { get; set; } = RelationshipOneTo.Many;
        public string Description { get; set; } = string.Empty;

        public TableForeignKeyAttribute(Type type) { 
            Type = type;
        }
        public TableForeignKeyAttribute(Type type, TableConstraint constraint=TableConstraint.Cascading, RelationshipOneTo relationshipOneTo = RelationshipOneTo.Many, string description = "")
        {
            Type = type;
            Constraint = constraint;
            RelationshipOneTo = relationshipOneTo;
            Description = description;
        }
    }
}

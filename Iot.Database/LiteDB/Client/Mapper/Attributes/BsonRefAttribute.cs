﻿using System;
using static Iot.Database.Constants;

namespace Iot.Database
{
    /// <summary>
    /// Indicate that field are not persisted inside this document but it's a reference for another document (DbRef)
    /// </summary>
    public class BsonRefAttribute : Attribute
    {
        public string Collection { get; set; }

        public BsonRefAttribute(string collection)
        {
            this.Collection = collection;
        }

        public BsonRefAttribute()
        {
            this.Collection = null;
        }
    }
}
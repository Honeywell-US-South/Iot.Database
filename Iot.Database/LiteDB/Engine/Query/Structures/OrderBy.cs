﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Iot.Database.Constants;

namespace Iot.Database.Engine
{
    /// <summary>
    /// Represent an OrderBy definition
    /// </summary>
    internal class OrderBy
    {
        public BsonExpression Expression { get; }

        public int Order { get; set; }

        public OrderBy(BsonExpression expression, int order)
        {
            this.Expression = expression;
            this.Order = order;
        }
    }
}

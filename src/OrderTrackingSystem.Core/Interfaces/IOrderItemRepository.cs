﻿using OrderTrackingSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Core.Interfaces
{
    public interface IOrderItemRepository : IRepository<OrderItem>  
    {
    }
}

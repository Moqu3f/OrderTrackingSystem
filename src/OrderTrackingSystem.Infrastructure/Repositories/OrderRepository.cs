using OrderTrackingSystem.Core.Entities;
using OrderTrackingSystem.Core.Interfaces;
using OrderTrackingSystem.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(OrderDbContext context) : base(context)
        {
        }

        // Специфічні методи для Order


    }

}

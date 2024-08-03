using OrderTrackingSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Core.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        // Специфічні методи для Customer, якщо є

            Task<Customer> GetCustomerWithOrdersAsync(int customerId);
    }

}

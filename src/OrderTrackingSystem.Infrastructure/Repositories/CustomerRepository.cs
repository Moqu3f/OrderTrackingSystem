using Microsoft.EntityFrameworkCore;
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
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(OrderDbContext context) : base(context)
        {
        }

        // Специфічні методи для Customer

        public async Task<Customer> GetCustomerWithOrdersAsync(int customerId)
        {
            return await _context.Customers
                .Include(c => c.Orders)  // Завантажуємо замовлення разом з клієнтом
                .ThenInclude(o => o.OrderItems) // Якщо потрібно завантажити і деталі замовлення
                .FirstOrDefaultAsync(c => c.Id == customerId);
        }
    }

}

using OrderTrackingSystem.Core.Interfaces;
using OrderTrackingSystem.Infrastructure.Data;
using OrderTrackingSystem.Infrastructure.Repositories;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Infrastructure.UnitOfWork;
public class UnitOfWork : IUnitOfWork
{
    private readonly OrderDbContext _context;

    public UnitOfWork(OrderDbContext context)
    {
        _context = context;
        Customers = new CustomerRepository(_context);
        Products = new ProductRepository(_context);
        Orders = new OrderRepository(_context);
        OrderItems = new OrderItemRepository(_context);
    }

    public ICustomerRepository Customers { get; private set; }
    public IProductRepository Products { get; private set; }
    public IOrderRepository Orders { get; private set; }
    public IOrderItemRepository OrderItems { get; private set; }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

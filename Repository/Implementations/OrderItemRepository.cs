using MyApiRestTesting.Data;
using MyApiRestTesting.Models.Entities;
using MyApiRestTesting.Repository.Base;
using MyApiRestTesting.Repository.Interfaces;

namespace MyApiRestTesting.Repository.Implementations
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        private readonly AppDbContext _context;
        public OrderItemRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task DeleteOrderItemAsync(int orderId, int lineItemId)
        {
            var orderItem = _context.OrderItems.Find(orderId, lineItemId);
            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
                return _context.SaveChangesAsync();
            }
            return Task.CompletedTask;
        }

        public async Task<OrderItem> GetByOrderIdAndLineItemIdAsync(int orderId, int lineItemId)
        => await _context.OrderItems.FindAsync(orderId, lineItemId);

    }
}
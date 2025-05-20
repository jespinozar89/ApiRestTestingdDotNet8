using MyApiRestTesting.Models.Entities;
using MyApiRestTesting.Repository.Base;

namespace MyApiRestTesting.Repository.Interfaces
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        Task<OrderItem> GetByOrderIdAndLineItemIdAsync(int orderId, int lineItemId);
        Task DeleteOrderItemAsync(int orderId, int lineItemId);
    }
}
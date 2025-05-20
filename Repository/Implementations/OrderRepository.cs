using MyApiRestTesting.Data;
using MyApiRestTesting.Models.Entities;
using MyApiRestTesting.Repository.Base;
using MyApiRestTesting.Repository.Interfaces;

namespace MyApiRestTesting.Repository.Implementations
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }
        
    }
}
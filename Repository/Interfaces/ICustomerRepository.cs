using MyApiRestTesting.Models.DTO;
using MyApiRestTesting.Models.Entities;
using MyApiRestTesting.Repository.Base;

namespace MyApiRestTesting.Repository.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        public Task<List<CustomerDTO>> GetCustomerByIdAsync(int id);
    }
}
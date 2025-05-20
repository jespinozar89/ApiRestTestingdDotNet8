using Microsoft.EntityFrameworkCore;
using MyApiRestTesting.Data;
using MyApiRestTesting.Models.DTO;
using MyApiRestTesting.Models.Entities;
using MyApiRestTesting.Repository.Base;
using MyApiRestTesting.Repository.Interfaces;

namespace MyApiRestTesting.Repository.Implementations
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context) :
        base(context)
        { 
            _context = context;
        }

        public Task<List<CustomerDTO>> GetCustomerByIdAsync(int id)
        {
            var customers = _context.Customers
                .Where(c => c.CustomerId == id)
                .Select(
                    c => new CustomerDTO
                    {
                        FullName = c.FullName,
                        EmailAdress = c.EmailAdress
                    }
                )
                .ToListAsync();

            return customers;
        }
    }
}
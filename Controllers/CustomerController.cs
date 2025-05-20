using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApiRestTesting.Models.DTO;
using MyApiRestTesting.Models.Entities;
using MyApiRestTesting.Repository.Interfaces;

namespace MyApiRestTesting.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                var customers = await _customerRepository.GetAllAsync();
                return Ok(customers);
            }
            catch (DbException)
            {
                // Log the exception
                return StatusCode(500, "Database error");
            }
            catch (Exception)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet("GetCustomerById/{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(customer);
            }
            catch (DbException)
            {
                // Log the exception
                return StatusCode(500, "Database error");
            }
            catch (Exception)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CustomerDTO customer)
        {
            try
            {

                if (customer == null)
                {
                    return BadRequest();
                }

                var newCustomer = new Customer
                {
                    EmailAdress = customer.EmailAdress,
                    FullName = customer.FullName
                };

                await _customerRepository.AddAsync(newCustomer);
                return CreatedAtAction(nameof(GetCustomerById), new { id = customer.CustomerId }, customer);
            }
            catch (DbUpdateException ex) // Error de base de datos (ej: clave duplicada)
            {
                return Conflict($"Error al guardar: {ex.InnerException?.Message}"); // HTTP 409
            }
            catch (ArgumentException ex) // Error de validación en repositorio
            {
                return BadRequest(ex.Message); // HTTP 400
            }
            catch (OperationCanceledException) // Timeout o cancelación
            {
                return StatusCode(503, "Servicio no disponible. Intente más tarde."); // HTTP 503
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message); // HTTP 500
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, CustomerDTO customer)
        {
            try
            {

                if (id != customer.CustomerId)
                {
                    return BadRequest();
                }

                var existingCustomer = await _customerRepository.GetByIdAsync(id);
                if (existingCustomer == null)
                {
                    return NotFound();
                }

                existingCustomer.EmailAdress = customer.EmailAdress ?? string.Empty;
                existingCustomer.FullName = customer.FullName ?? string.Empty;

                await _customerRepository.UpdateAsync(existingCustomer);
                return NoContent();
            }
            catch (DbUpdateException ex) // Error de base de datos (ej: clave duplicada)
            {
                return Conflict($"Error al guardar: {ex.InnerException?.Message}"); // HTTP 409
            }
            catch (ArgumentException ex) // Error de validación en repositorio
            {
                return BadRequest(ex.Message); // HTTP 400
            }
            catch (OperationCanceledException) // Timeout o cancelación
            {
                return StatusCode(503, "Servicio no disponible. Intente más tarde."); // HTTP 503
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message); // HTTP 500
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {

                var customer = await _customerRepository.GetByIdAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }

                await _customerRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, "Error al eliminar en base de datos: "+ex.Message); // HTTP 500
            }
            catch (ArgumentException ex) // Error de validación en repositorio
            {
                return BadRequest(ex.Message); // HTTP 400
            }
            catch (OperationCanceledException) // Timeout o cancelación
            {
                return StatusCode(503, "Servicio no disponible. Intente más tarde."); // HTTP 503
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }

        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCustomers(string email)
        {
            try
            {
                var customers = await _customerRepository.FindAsync(c => c.EmailAdress.ToLower().Contains(email.ToLower()));
                if (customers == null || !customers.Any())
                {
                    return NotFound();
                }
                return Ok(customers);
            }
            catch (DbException)
            {
                // Log the exception
                return StatusCode(500, "Database error");
            }
            catch (Exception)
            {
                // Log the exception
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
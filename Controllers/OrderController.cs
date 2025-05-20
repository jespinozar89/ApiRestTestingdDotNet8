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
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderRepository.GetAllAsync();
                return Ok(orders);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {

                var order = await _orderRepository.GetByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }
                return Ok(order);
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
        public async Task<IActionResult> CreateOrder(OrderDTO order)
        {
            try
            {
                if (order == null)
                {
                    return BadRequest();
                }

                var newOrder = new Order
                {
                    CustomerId = order.CustomerId,
                    OrderStatus = order.OrderStatus,
                    OrderTimestamp = DateTime.UtcNow,
                    StoreId = 1
                };

                await _orderRepository.AddAsync(newOrder);
                return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
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
                return StatusCode(500, "Error interno del servidor:" + ex.Message); // HTTP 500
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderDTO order)
        {
            try
            {

                if (id != order.OrderId)
                {
                    return BadRequest();
                }

                var existingOrder = await _orderRepository.GetByIdAsync(id);
                if (existingOrder == null)
                {
                    return NotFound();
                }

                existingOrder.CustomerId = order.CustomerId;
                existingOrder.OrderStatus = order.OrderStatus;
                existingOrder.OrderTimestamp = DateTime.UtcNow;

                await _orderRepository.UpdateAsync(existingOrder);
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
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {

                var order = await _orderRepository.GetByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }

                await _orderRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (DbUpdateException ex)
            {                
                return StatusCode(500, "Error al eliminar en base de datos"); // HTTP 500
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetOrdersByCustomerId(int customerId)
        {
            try
            {
                var orders = await _orderRepository.FindAsync(o => o.CustomerId == customerId);
                if (orders == null || !orders.Any())
                {
                    return NotFound();
                }
                return Ok(orders);
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

        [HttpGet("store/{orderTimestamp}")]
        public async Task<IActionResult> GetOrdersByOrderTimestamp(DateTime orderTimestamp)
        {
            try
            {

                var orders = await _orderRepository
                    .FindAsync(o => o.OrderTimestamp.Value.Date == orderTimestamp.Date);
                if (orders == null || !orders.Any())
                {
                    return NotFound();
                }
                return Ok(orders);
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
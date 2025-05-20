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
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemRepository _orderItemRepository;
        public OrderItemController(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderItems()
        {
            try
            {
                var orderItems = await _orderItemRepository.GetAllAsync();
                return Ok(orderItems);
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

        [HttpGet("{orderId}/{lineItemId}")]
        public async Task<IActionResult> GetOrderItemById(int orderId, int lineItemId)
        {
            try
            {

                var orderItem = await _orderItemRepository.GetByOrderIdAndLineItemIdAsync(orderId, lineItemId);
                if (orderItem == null)
                {
                    return NotFound();
                }
                return Ok(orderItem);
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
        public async Task<IActionResult> CreateOrderItem(OrderItemDTO orderItem)
        {
            try
            {
                if (orderItem == null)
                {
                    return BadRequest();
                }

                var newOrderItem = new OrderItem
                {
                    OrderId = orderItem.OrderId,
                    LineItemId = orderItem.LineItemId,
                    ProductId = orderItem.ProductId,
                    UnitPrice = orderItem.UnitPrice,
                    Quantity = orderItem.Quantity,
                    ShipmentId = 1

                };

                await _orderItemRepository.AddAsync(newOrderItem);
                return CreatedAtAction(nameof(GetOrderItemById), new { orderId = newOrderItem.OrderId, LineItemId = newOrderItem.LineItemId }, newOrderItem);
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

        [HttpPut("{orderId}/{lineItemId}")]
        public async Task<IActionResult> UpdateOrderItem(int orderId, int lineItemId, OrderItemDTO orderItem)
        {
            try
            {
                if (orderId != orderItem.OrderId || lineItemId != orderItem.LineItemId)
                {
                    return BadRequest();
                }

                var existingOrderItem = await _orderItemRepository.GetByOrderIdAndLineItemIdAsync(orderItem.OrderId, orderItem.LineItemId);
                if (existingOrderItem == null)
                {
                    return NotFound();
                }

                existingOrderItem.ProductId = orderItem.ProductId;
                existingOrderItem.UnitPrice = orderItem.UnitPrice;
                existingOrderItem.Quantity = orderItem.Quantity;

                await _orderItemRepository.UpdateAsync(existingOrderItem);
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

        [HttpDelete("{orderId}/{lineItemId}")]
        public async Task<IActionResult> DeleteOrderItem(int orderId, int lineItemId)
        {
            try
            {
                var orderItem = await _orderItemRepository.GetByOrderIdAndLineItemIdAsync(orderId, lineItemId);
                if (orderItem == null)
                {
                    return NotFound();
                }

                await _orderItemRepository.DeleteOrderItemAsync(orderId, lineItemId);
                return NoContent();

            }

            catch (DbUpdateException ex)
            {                
                return StatusCode(500, "Error al eliminar en base de datos: " + ex.Message); // HTTP 500
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetOrderItemsByOrderId(int orderId)
        {
            try
            {

                var orderItems = await _orderItemRepository.FindAsync(x => x.OrderId == orderId);
                if (orderItems == null || !orderItems.Any())
                {
                    return NotFound();
                }
                return Ok(orderItems);
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

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetOrderItemsByProductId(int productId)
        {
            try
            {

                var orderItems = await _orderItemRepository.FindAsync(x => x.ProductId == productId);
                if (orderItems == null || !orderItems.Any())
                {
                    return NotFound();
                }
                return Ok(orderItems);
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
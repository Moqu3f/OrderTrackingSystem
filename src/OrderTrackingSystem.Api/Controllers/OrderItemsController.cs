using Microsoft.AspNetCore.Mvc;
using OrderTrackingSystem.Core.Entities;
using OrderTrackingSystem.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderItemsController(IOrderItemRepository orderItemRepository, IUnitOfWork unitOfWork)
        {
            _orderItemRepository = orderItemRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems()
        {
            return Ok(await _orderItemRepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItem(int id)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }
            return Ok(orderItem);
        }

        [HttpPost]
        public async Task<ActionResult<OrderItem>> CreateOrderItem(OrderItem orderItem)
        {
            await _orderItemRepository.AddAsync(orderItem);
            await _unitOfWork.CompleteAsync();
            return CreatedAtAction("GetOrderItem", new { id = orderItem.Id }, orderItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(int id, OrderItem orderItem)
        {
            if (id != orderItem.Id)
            {
                return BadRequest();
            }

            _orderItemRepository.Update(orderItem);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            _orderItemRepository.Remove(orderItem);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}

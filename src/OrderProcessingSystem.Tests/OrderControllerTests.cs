using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using OrderTrackingSystem.Api.Controllers;
using OrderTrackingSystem.Core.Entities;
using OrderTrackingSystem.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Api.Tests
{
    public class OrdersControllerTests
    {
        private readonly OrdersController _controller;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public OrdersControllerTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new OrdersController(_mockOrderRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetOrders_ReturnsOkResult_WhenOrdersExist()
        {
            // Arrange
            var orders = new List<Order> { new Order { Id = 1, OrderDate = DateTime.Now, Status = "Pending", CustomerId = 1 } };
            _mockOrderRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(orders);

            // Act
            var result = await _controller.GetOrders();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Order>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<List<Order>>(okResult.Value);
            Assert.Single(returnValue);
        }


        [Fact]
        public async Task GetOrder_ReturnsNotFoundResult_WhenOrderDoesNotExist()
        {
            // Arrange
            _mockOrderRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Order)null);

            // Act
            var result = await _controller.GetOrder(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Order>>(result);
            var notFoundResult = Assert.IsType<NotFoundResult>(actionResult.Result);
        }


        [Fact]
        public async Task CreateOrder_ReturnsCreatedAtActionResult_WithOrder()
        {
            // Arrange
            var newOrder = new Order { Id = 1, OrderDate = DateTime.Now, Status = "Pending", CustomerId = 1 };
            _mockOrderRepository.Setup(repo => repo.AddAsync(newOrder)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(1); // Виправлено на Task<int>

            // Act
            var result = await _controller.CreateOrder(newOrder);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result); // Перевірка правильного типу
            var returnValue = Assert.IsType<Order>(createdAtActionResult.Value);
            Assert.Equal(newOrder.Id, returnValue.Id);
            Assert.Equal(newOrder.OrderDate, returnValue.OrderDate);
            Assert.Equal(newOrder.Status, returnValue.Status);
            Assert.Equal(newOrder.CustomerId, returnValue.CustomerId);
        }



        [Fact]
        public async Task UpdateOrder_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            // Arrange
            var orderToUpdate = new Order { Id = 1, OrderDate = DateTime.Now, Status = "Updated", CustomerId = 1 };

            // Act
            var result = await _controller.UpdateOrder(2, orderToUpdate);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateOrder_ReturnsNoContentResult_WhenOrderIsUpdated()
        {
            // Arrange
            var orderToUpdate = new Order { Id = 1, OrderDate = DateTime.Now, Status = "Updated", CustomerId = 1 };
            _mockOrderRepository.Setup(repo => repo.Update(orderToUpdate));
            _mockUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(1); // Виправлено на Task<int>

            // Act
            var result = await _controller.UpdateOrder(1, orderToUpdate);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }



        [Fact]
        public async Task DeleteOrder_ReturnsNotFoundResult_WhenOrderDoesNotExist()
        {
            // Arrange
            _mockOrderRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Order)null);

            // Act
            var result = await _controller.DeleteOrder(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteOrder_ReturnsNoContentResult_WhenOrderIsDeleted()
        {
            // Arrange
            var existingOrder = new Order { Id = 1, OrderDate = DateTime.Now, Status = "Pending", CustomerId = 1 };
            _mockOrderRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingOrder);
            _mockOrderRepository.Setup(repo => repo.Remove(existingOrder));
            _mockUnitOfWork.Setup(uow => uow.CompleteAsync()).Returns(Task.FromResult(0)); // Виправлено для Task<int>

            // Act
            var result = await _controller.DeleteOrder(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }


    }
}

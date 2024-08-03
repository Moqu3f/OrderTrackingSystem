using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderTrackingSystem.Api.Controllers;
using OrderTrackingSystem.Core.Entities;
using OrderTrackingSystem.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OrderTrackingSystem.Tests
{
    public class CustomerControllerTests
    {
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly CustomersController _controller;

        public CustomerControllerTests()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new CustomersController(_mockCustomerRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetCustomer_ReturnsOkResult_WithCustomer()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "John Doe", Email = "john@example.com" };
            _mockCustomerRepository.Setup(repo => repo.GetCustomerWithOrdersAsync(1)).ReturnsAsync(customer);

            // Act
            var result = await _controller.GetCustomer(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Customer>(okResult.Value);
            Assert.Equal(customer.Id, returnValue.Id);
            Assert.Equal(customer.Name, returnValue.Name);
            Assert.Equal(customer.Email, returnValue.Email);
        }

        [Fact]
        public async Task GetCustomer_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            // Arrange
            _mockCustomerRepository.Setup(repo => repo.GetCustomerWithOrdersAsync(1)).ReturnsAsync((Customer)null);

            // Act
            var result = await _controller.GetCustomer(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateCustomer_ReturnsCreatedAtActionResult_WithCustomer()
        {
            // Arrange
            var newCustomer = new Customer { Id = 1, Name = "Jane Doe", Email = "jane@example.com" };
            _mockCustomerRepository.Setup(repo => repo.AddAsync(newCustomer)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _controller.CreateCustomer(newCustomer);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<Customer>(createdAtActionResult.Value);
            Assert.Equal(newCustomer.Id, returnValue.Id);
            Assert.Equal(newCustomer.Name, returnValue.Name);
            Assert.Equal(newCustomer.Email, returnValue.Email);
        }

        [Fact]
        public async Task UpdateCustomer_ReturnsNoContentResult_WhenCustomerIsUpdated()
        {
            // Arrange
            var customerToUpdate = new Customer { Id = 1, Name = "Updated Name", Email = "updated@example.com" };
            _mockCustomerRepository.Setup(repo => repo.Update(customerToUpdate));
            _mockUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _controller.UpdateCustomer(1, customerToUpdate);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsNoContentResult_WhenCustomerIsDeleted()
        {
            // Arrange
            var existingCustomer = new Customer { Id = 1, Name = "John Doe", Email = "john@example.com" };
            _mockCustomerRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingCustomer);
            _mockCustomerRepository.Setup(repo => repo.Remove(existingCustomer));
            _mockUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _controller.DeleteCustomer(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}

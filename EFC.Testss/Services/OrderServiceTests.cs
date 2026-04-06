using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using EFC.Models;
using EFC.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace EFC.Tests.Services;

[TestClass]
public class OrderServiceMockTests
{
    private Mock<IOrderService> _mockOrderService;

    [TestInitialize]
    public void Setup()
    {
        _mockOrderService = new Mock<IOrderService>();
    }

    #region Тести для Orders (Замовлення)

    [TestMethod]
    public async Task GetAllAsync_ShouldReturnOrdersWithNavigationProperties()
    {
        // Arrange
        var orders = new List<Order>
        {
            new Order {
                Id = 1,
                OrderDate = DateTime.Now,
                Customer = new Customer { LastName = "Savchuk" },
                SuperMarket = new Supermarket { Name = "Silpo" }
            }
        };

        _mockOrderService.Setup(s => s.GetAllAsync()).ReturnsAsync(orders);

        // Act
        var result = await _mockOrderService.Object.GetAllAsync();

        // Assert
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual("Savchuk", result.First().Customer.LastName);
        _mockOrderService.Verify(s => s.GetAllAsync(), Times.Once());
    }

    [TestMethod]
    public async Task GetByIdAsync_ShouldReturnOrderWithDetails()
    {
        // Arrange
        var order = new Order
        {
            Id = 1,
            OrderDetails = new List<OrderDetail>
            {
                new OrderDetail { Id = 10, Product = new Product { Name = "Coffee" } }
            }
        };

        _mockOrderService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(order);

        // Act
        var result = await _mockOrderService.Object.GetByIdAsync(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.OrderDetails.Count);
        Assert.AreEqual("Coffee", result.OrderDetails.First().Product.Name);
    }

    [TestMethod]
    public async Task CreateAsync_ShouldInvokeCreate()
    {
        var newOrder = new Order { OrderDate = DateTime.Now };
        await _mockOrderService.Object.CreateAsync(newOrder);
        _mockOrderService.Verify(s => s.CreateAsync(newOrder), Times.Once());
    }

    [TestMethod]
    public async Task DeleteAsync_ShouldInvokeWithCorrectId()
    {
        await _mockOrderService.Object.DeleteAsync(5);
        _mockOrderService.Verify(s => s.DeleteAsync(5), Times.Once());
    }

    #endregion

    #region Тести для OrderDetails (Деталі замовлення)

    [TestMethod]
    public async Task AddDetailAsync_ShouldInvokeService()
    {
        // Arrange
        var detail = new OrderDetail { ProductId = 1, Quantity = 5 };

        // Act
        await _mockOrderService.Object.AddDetailAsync(detail);

        // Assert
        _mockOrderService.Verify(s => s.AddDetailAsync(detail), Times.Once());
    }

    [TestMethod]
    public async Task DeleteDetailAsync_ShouldInvokeWithId()
    {
        // Act
        await _mockOrderService.Object.DeleteDetailAsync(100);

        // Assert
        _mockOrderService.Verify(s => s.DeleteDetailAsync(100), Times.Once());
    }

    [TestMethod]
    public async Task GetDetailByIdAsync_ShouldReturnDetail()
    {
        // Arrange
        var detail = new OrderDetail { Id = 100, Quantity = 2 };
        _mockOrderService.Setup(s => s.GetDetailByIdAsync(100)).ReturnsAsync(detail);

        // Act
        var result = await _mockOrderService.Object.GetDetailByIdAsync(100);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Quantity);
        _mockOrderService.Verify(s => s.GetDetailByIdAsync(100), Times.Once());
    }

    #endregion
}
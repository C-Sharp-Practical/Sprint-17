using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using EFC.Models;
using EFC.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace EFC.Tests.Services;

[TestClass]
public class CustomerServiceTest
{
    private Mock<ICustomerService> _mockService;

    [TestInitialize]
    public void Setup()
    {
        _mockService = new Mock<ICustomerService>();
    }

    [TestMethod]
    public async Task GetAll_ShouldReturnListFromMock()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new Customer { Id = 1, LastName = "Savchuk", FirstName = "Viacheslav" },
            new Customer { Id = 2, LastName = "Testov", FirstName = "Test" }
        };

        _mockService
            .Setup(s => s.GetAllAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(customers);

        // Act: Викликаємо метод (через контролер або напряму через мок для тесту)
        var result = await _mockService.Object.GetAllAsync();

        // Assert
        Assert.AreEqual(2, result.Count());
        _mockService.Verify(s => s.GetAllAsync(null, "LastName", true), Times.Once());
    }

    [TestMethod]
    public async Task GetById_ShouldReturnSingleCustomer()
    {
        // Arrange
        var expectedCustomer = new Customer { Id = 10, FirstName = "Slava" };
        _mockService.Setup(s => s.GetByIdAsync(10)).ReturnsAsync(expectedCustomer);

        // Act
        var result = await _mockService.Object.GetByIdAsync(10);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Slava", result.FirstName);
    }

    [TestMethod]
    public async Task Create_ShouldInvokeServiceOnce()
    {
        // Arrange
        var newCustomer = new Customer { FirstName = "New" };

        // Act
        await _mockService.Object.CreateAsync(newCustomer);

        // Assert: Перевіряємо, чи був викликаний метод створення
        _mockService.Verify(s => s.CreateAsync(newCustomer), Times.Once());
    }

    [TestMethod]
    public async Task Delete_ShouldInvokeServiceWithCorrectId()
    {
        // Act
        await _mockService.Object.DeleteAsync(5);

        // Assert
        _mockService.Verify(s => s.DeleteAsync(5), Times.Once());
    }

    [TestMethod]
    public async Task Exists_ShouldReturnTrue_WhenSetupToReturnTrue()
    {
        // Arrange
        _mockService.Setup(s => s.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);

        // Act
        var result = await _mockService.Object.ExistsAsync(1);

        // Assert
        Assert.IsTrue(result);
    }
}
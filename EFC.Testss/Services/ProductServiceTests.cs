using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using EFC.Models;
using EFC.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace EFC.Tests.Services;

[TestClass]
public class ProductServiceTests
{
    private Mock<IProductService> _mockProductService;

    [TestInitialize]
    public void Setup()
    {
        _mockProductService = new Mock<IProductService>();
    }

    [TestMethod]
    public async Task GetAllAsync_ShouldReturnProductList()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 1500 },
            new Product { Id = 2, Name = "Mouse", Price = 25 }
        };

        _mockProductService
            .Setup(s => s.GetAllAsync())
            .ReturnsAsync(products);

        // Act
        var result = await _mockProductService.Object.GetAllAsync();

        // Assert
        Assert.AreEqual(2, result.Count());
        Assert.AreEqual("Laptop", result.First().Name);
        _mockProductService.Verify(s => s.GetAllAsync(), Times.Once());
    }

    [TestMethod]
    public async Task GetByIdAsync_ShouldReturnSpecificProduct()
    {
        // Arrange
        var expectedProduct = new Product { Id = 1, Name = "Keyboard" };
        _mockProductService
            .Setup(s => s.GetByIdAsync(1))
            .ReturnsAsync(expectedProduct);

        // Act
        var result = await _mockProductService.Object.GetByIdAsync(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Keyboard", result.Name);
        _mockProductService.Verify(s => s.GetByIdAsync(1), Times.Once());
    }

    [TestMethod]
    public async Task CreateAsync_ShouldInvokeService()
    {
        // Arrange
        var newProduct = new Product { Name = "Monitor", Price = 300 };

        // Act
        await _mockProductService.Object.CreateAsync(newProduct);

        // Assert
        _mockProductService.Verify(s => s.CreateAsync(newProduct), Times.Once());
    }

    [TestMethod]
    public async Task UpdateAsync_ShouldInvokeService()
    {
        // Arrange
        var productToUpdate = new Product { Id = 1, Name = "Updated Name" };

        // Act
        await _mockProductService.Object.UpdateAsync(productToUpdate);

        // Assert
        _mockProductService.Verify(s => s.UpdateAsync(productToUpdate), Times.Once());
    }

    [TestMethod]
    public async Task DeleteAsync_ShouldInvokeServiceWithId()
    {
        // Arrange
        int productId = 99;

        // Act
        await _mockProductService.Object.DeleteAsync(productId);

        // Assert
        _mockProductService.Verify(s => s.DeleteAsync(productId), Times.Once());
    }

    [TestMethod]
    public async Task ExistsAsync_ShouldReturnTrue_WhenProductExists()
    {
        // Arrange
        _mockProductService
            .Setup(s => s.ExistsAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _mockProductService.Object.ExistsAsync(1);

        // Assert
        Assert.IsTrue(result);
        _mockProductService.Verify(s => s.ExistsAsync(1), Times.Once());
    }

    [TestMethod]
    public async Task ExistsAsync_ShouldReturnFalse_WhenProductDoesNotExist()
    {
        // Arrange
        _mockProductService
            .Setup(s => s.ExistsAsync(It.IsAny<int>()))
            .ReturnsAsync(false);

        // Act
        var result = await _mockProductService.Object.ExistsAsync(999);

        // Assert
        Assert.IsFalse(result);
    }
}
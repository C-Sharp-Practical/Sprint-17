using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using EFC.Models;
using EFC.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace EFC.Tests.Services;

[TestClass]
public class SupermarketServiceMockTests
{
    private Mock<ISupermarketService> _mockService;

    [TestInitialize]
    public void Setup()
    {
        _mockService = new Mock<ISupermarketService>();
    }

    [TestMethod]
    public async Task GetAllAsync_ShouldReturnQueryableSupermarkets()
    {
        // Arrange
        var supermarkets = new List<Supermarket>
        {
            new Supermarket { Id = 1, Name = "Silpo", Address = "Kyiv" },
            new Supermarket { Id = 2, Name = "ATB", Address = "Lviv" }
        }.AsQueryable();

        _mockService
            .Setup(s => s.GetAllAsync())
            .ReturnsAsync(supermarkets);

        // Act
        var result = await _mockService.Object.GetAllAsync();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count());
        Assert.AreEqual("Silpo", result.First().Name);
        _mockService.Verify(s => s.GetAllAsync(), Times.Once());
    }

    [TestMethod]
    public async Task GetByIdAsync_ShouldReturnSpecificSupermarket()
    {
        // Arrange
        var supermarket = new Supermarket { Id = 1, Name = "Novus" };
        _mockService
            .Setup(s => s.GetByIdAsync(1))
            .ReturnsAsync(supermarket);

        // Act
        var result = await _mockService.Object.GetByIdAsync(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Novus", result.Name);
        _mockService.Verify(s => s.GetByIdAsync(1), Times.Once());
    }

    [TestMethod]
    public async Task CreateAsync_ShouldInvokeWithCorrectObject()
    {
        // Arrange
        var newSupermarket = new Supermarket { Name = "Ashan" };

        // Act
        await _mockService.Object.CreateAsync(newSupermarket);

        // Assert
        _mockService.Verify(s => s.CreateAsync(newSupermarket), Times.Once());
    }

    [TestMethod]
    public async Task UpdateAsync_ShouldInvokeUpdate()
    {
        // Arrange
        var supermarket = new Supermarket { Id = 1, Name = "Updated Name" };

        // Act
        await _mockService.Object.UpdateAsync(supermarket);

        // Assert
        _mockService.Verify(s => s.UpdateAsync(supermarket), Times.Once());
    }

    [TestMethod]
    public async Task DeleteAsync_ShouldCallWithId()
    {
        // Act
        await _mockService.Object.DeleteAsync(10);

        // Assert
        _mockService.Verify(s => s.DeleteAsync(10), Times.Once());
    }

    [TestMethod]
    public async Task ExistsAsync_ShouldReturnExpectedBoolean()
    {
        // Arrange
        _mockService.Setup(s => s.ExistsAsync(1)).ReturnsAsync(true);
        _mockService.Setup(s => s.ExistsAsync(2)).ReturnsAsync(false);

        // Act & Assert
        Assert.IsTrue(await _mockService.Object.ExistsAsync(1));
        Assert.IsFalse(await _mockService.Object.ExistsAsync(2));

        _mockService.Verify(s => s.ExistsAsync(It.IsAny<int>()), Times.Exactly(2));
    }
}
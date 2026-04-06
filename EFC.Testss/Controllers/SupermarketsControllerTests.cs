using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using EFC.Controllers;
using EFC.Data;
using EFC.Models;
using EFC.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace EFC.Tests.Controllers
{
    [TestClass]
    public class SupermarketsControllerTests
    {
        private ShoppingContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ShoppingContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            return new ShoppingContext(options);
        }

        [TestMethod]
        public async Task Index_Paging_ReturnsCorrectNumberOfItems()
        {
            // Arrange
            using var context = GetContext();
            
            for (int i = 1; i <= 5; i++)
            {
                context.Supermarkets.Add(new Supermarket { Id = i, Name = $"Market {i}", Address = "Street" });
            }
            await context.SaveChangesAsync();
            var service = new SupermarketService(context);
            var controller = new SupermarketsController(service);

            // Act
            var result = await controller.Index(pageNum: 1) as ViewResult;
            var model = result.Model as PaginatedList<Supermarket>;

            // Assert
            Assert.IsNotNull(model);
            Assert.AreEqual(3, model.Count);
            Assert.IsTrue(model.HasNextPage);
            Assert.IsFalse(model.HasPreviousPage);
        }

        [TestMethod]
        public async Task Index_SecondPage_ReturnsRemainingItems()
        {
            // Arrange
            using var context = GetContext();
            for (int i = 1; i <= 5; i++)
            {
                context.Supermarkets.Add(new Supermarket
                {
                    Name = $"Market {i}",
                    Address = "Test Address"
                });
            }
            await context.SaveChangesAsync();

            var service = new SupermarketService(context);
            var controller = new SupermarketsController(service);

            // Act
            var result = await controller.Index(pageNum: 2) as ViewResult;
            var model = result.Model as PaginatedList<Supermarket>;

            // Assert
            Assert.AreEqual(2, model.Count);
            Assert.IsTrue(model.HasPreviousPage);
        }

        [TestMethod]
        public async Task Details_ReturnsNotFound_ForInvalidId()
        {
            // Arrange
            using var context = GetContext();
            var service = new SupermarketService(context);
            var controller = new SupermarketsController(service);

            // Act
            var result = await controller.Details(999);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Create_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            using var context = GetContext();
            var service = new SupermarketService(context);
            var controller = new SupermarketsController(service);
            var market = new Supermarket { Name = "ATB", Address = "Main St" };

            // Act
            var result = await controller.Create(market) as RedirectToActionResult;

            // Assert
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual(1, context.Supermarkets.Count());
        }

        [TestMethod]
        public async Task Edit_Post_ValidModel_UpdatesDatabase()
        {
            // Arrange
            using var context = GetContext();
            var market = new Supermarket { Name = "Old Name", Address = "Old Address" };
            context.Supermarkets.Add(market);
            await context.SaveChangesAsync();

            context.Entry(market).State = EntityState.Detached;

            var service = new SupermarketService(context);
            var controller = new SupermarketsController(service);

            var updated = new Supermarket
            {
                Id = market.Id,
                Name = "New Name",
                Address = "New Address"
            };

            // Act
            var result = await controller.Edit(market.Id, updated);

            // Assert
            var dbMarket = await context.Supermarkets.FindAsync(market.Id);
            Assert.IsNotNull(dbMarket, "Маркет не знайдено в базі після редагування");
            Assert.AreEqual("New Name", dbMarket.Name);
            Assert.AreEqual("New Address", dbMarket.Address);
        }

        [TestMethod]
        public async Task DeleteConfirmed_RemovesSupermarket()
        {
            // Arrange
            using var context = GetContext();

            var market = new Supermarket();
            market.Name = "To Delete";
            market.Address = "Kyiv St. 1";

            context.Supermarkets.Add(market);

            await context.SaveChangesAsync();

            var service = new SupermarketService(context);
            var controller = new SupermarketsController(service);

            // Act
            await controller.DeleteConfirmed(market.Id);

            // Assert
            Assert.AreEqual(0, context.Supermarkets.Count());
        }
    }
}
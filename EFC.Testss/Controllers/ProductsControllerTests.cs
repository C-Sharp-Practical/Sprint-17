using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using EFC.Controllers;
using EFC.Data;
using EFC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFC.Tests.Controllers
{
    [TestClass]
    public class ProductsControllerTests
    {
        private ShoppingContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ShoppingContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            return new ShoppingContext(options);
        }

        [TestMethod]
        public async Task Index_ReturnsViewWithAllProducts()
        {
            // Arrange
            using var context = GetContext();
            context.Products.AddRange(
                new Product { Id = 1, Name = "Laptop", Price = 1200.50 },
                new Product { Id = 2, Name = "Mouse", Price = 25.00 }
            );
            await context.SaveChangesAsync();
            var controller = new ProductsController(context);

            // Act
            var result = await controller.Index() as ViewResult;
            var model = result.Model as List<Product>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, model.Count);
            Assert.AreEqual("Laptop", model[0].Name);
        }

        [TestMethod]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            using var context = GetContext();
            var controller = new ProductsController(context);

            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Details_ReturnsView_WhenProductExists()
        {
            // Arrange
            using var context = GetContext();
            var product = new Product { Id = 10, Name = "Keyboard", Price = 45.99 };
            context.Products.Add(product);
            await context.SaveChangesAsync();
            var controller = new ProductsController(context);

            // Act
            var result = await controller.Details(10) as ViewResult;
            var model = result.Model as Product;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Keyboard", model.Name);
        }

        [TestMethod]
        public async Task Create_Post_ValidProduct_RedirectsToIndex()
        {
            // Arrange
            using var context = GetContext();
            var controller = new ProductsController(context);
            var newProduct = new Product { Name = "Monitor", Price = 300 };

            // Act
            var result = await controller.Create(newProduct) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual(1, context.Products.Count());
            Assert.AreEqual("Monitor", context.Products.First().Name);
        }

        [TestMethod]
        public async Task Create_Post_InvalidProduct_ReturnsViewWithProduct()
        {
            // Arrange
            using var context = GetContext();
            var controller = new ProductsController(context);
            controller.ModelState.AddModelError("Name", "Required"); 
            var invalidProduct = new Product { Price = 100 };

            // Act
            var result = await controller.Create(invalidProduct) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(invalidProduct, result.Model);
        }

        [TestMethod]
        public async Task Edit_Post_ValidUpdate_RedirectsToIndex()
        {
            // Arrange
            using var context = GetContext();
            var product = new Product { Id = 5, Name = "Old Name", Price = 10 };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            context.Entry(product).State = EntityState.Detached;

            var controller = new ProductsController(context);
            var updatedProduct = new Product { Id = 5, Name = "New Name", Price = 15 };

            // Act
            var result = await controller.Edit(5, updatedProduct) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            var dbProduct = await context.Products.FindAsync(5);
            Assert.AreEqual("New Name", dbProduct.Name);
        }

        [TestMethod]
        public async Task DeleteConfirmed_RemovesProduct()
        {
            // Arrange
            using var context = GetContext();
            var product = new Product { Id = 1, Name = "To Delete", Price = 0 };
            context.Products.Add(product);
            await context.SaveChangesAsync();
            var controller = new ProductsController(context);

            // Act
            await controller.DeleteConfirmed(1);

            // Assert
            Assert.AreEqual(0, context.Products.Count());
        }
    }
}
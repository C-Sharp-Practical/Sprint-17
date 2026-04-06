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
    public class CustomersControllerTests
    {
        private ShoppingContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ShoppingContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            return new ShoppingContext(options);
        }

        #region Тести методу Index (Фільтрація та Сортування)

        [TestMethod]
        public async Task Index_FiltersByFirstName_ReturnsCorrectCustomer()
        {
            // Arrange
            using var context = GetContext();
            context.Customers.AddRange(
                new Customer { FirstName = "Mykola", LastName = "Bobro", Address = "Kyiv" },
                new Customer { FirstName = "Ivan", LastName = "Ivanov", Address = "Lviv" }
            );
            await context.SaveChangesAsync();
            var controller = new CustomersController(context);

            var result = await controller.Index(name: "Mykola") as ViewResult;
            var model = result.Model as List<Customer>;

            // Assert
            Assert.AreEqual(1, model.Count);
            Assert.AreEqual("Bobro", model[0].LastName);
        }

        [TestMethod]
        public async Task Index_SortsByAddressDescending_ReturnsCorrectOrder()
        {
            // Arrange
            using var context = GetContext();
            context.Customers.AddRange(
                new Customer { FirstName = "A", LastName = "A", Address = "Kyiv" },
                new Customer { FirstName = "B", LastName = "B", Address = "Lviv" }
            );
            await context.SaveChangesAsync();
            var controller = new CustomersController(context);

            // Act
            var result = await controller.Index(name: null, sortBy: "Address", isAscending: false) as ViewResult;
            var model = result.Model as List<Customer>;

            // Assert
            Assert.AreEqual("Lviv", model[0].Address);
            Assert.AreEqual("Kyiv", model[1].Address);
        }

        #endregion

        #region Тести CRUD операцій

        [TestMethod]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            using var context = GetContext();
            var controller = new CustomersController(context);

            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task Create_Post_ValidModel_RedirectsToIndex()
        {
            // Arrange
            using var context = GetContext();
            var controller = new CustomersController(context);
            var newCustomer = new Customer 
            { 
                FirstName = "Test", 
                LastName = "User" ,
                Address = "Test Street 123"
            };

            // Act
            var result = await controller.Create(newCustomer) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual(1, context.Customers.Count());
        }

        [TestMethod]
        public async Task Create_Post_InvalidModel_ReturnsView()
        {
            // Arrange
            using var context = GetContext();
            var controller = new CustomersController(context);
            controller.ModelState.AddModelError("FirstName", "Required");
            var invalidCustomer = new Customer { LastName = "User" };

            // Act
            var result = await controller.Create(invalidCustomer) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(invalidCustomer, result.Model);
        }

        [TestMethod]
        public async Task DeleteConfirmed_RemovesCustomerFromDatabase()
        {
            // Arrange
            using var context = GetContext();
            var customer = new Customer 
            { 
                Id = 1,
                FirstName = "Delete",
                LastName = "Me" ,
                Address = "Test Street 123"
            };
            context.Customers.Add(customer);
            await context.SaveChangesAsync();
            var controller = new CustomersController(context);

            // Act
            await controller.DeleteConfirmed(1);

            // Assert
            Assert.AreEqual(0, context.Customers.Count());
        }

        #endregion
    }
}
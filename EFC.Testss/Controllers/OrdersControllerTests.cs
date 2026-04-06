using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using EFC.Controllers;
using EFC.Data;
using EFC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFC.Tests.Controllers
{
    [TestClass]
    public class OrdersControllerTests
    {
        private ShoppingContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ShoppingContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ShoppingContext(options);
        }

        [TestMethod]
        public async Task Index_ReturnsViewWithOrdersAndIncludes()
        {
            // Arrange
            using var context = GetContext();
            var customer = new Customer
            {
                FirstName = "Taras",
                LastName = "Shevchenko",
                Address = "Kaniv"
            };
            var market = new Supermarket
            {
                Name = "Silpo",
                Address = "Kyiv"
            };

            context.Customers.Add(customer);
            context.Supermarkets.Add(market);

            await context.SaveChangesAsync();

            context.Orders.Add(new Order
            {
                CustomerId = customer.Id,
                SuperMarketId = market.Id,
                OrderDate = DateTime.Now
            });
            await context.SaveChangesAsync();

            var controller = new OrdersController(context);

            // Act
            var result = await controller.Index(null) as ViewResult;
            var model = result.Model as OrderIndexData;

            // Assert
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Orders.Count());
            Assert.IsNotNull(model.Orders.First().Customer);
            Assert.AreEqual("Shevchenko", model.Orders.First().Customer.LastName);
        }

        [TestMethod]
        public async Task Index_WithSelectedOrderId_LoadsOrderDetails()
        {
            // Arrange
            using var context = GetContext();
            context.Orders.Add(new Order { Id = 5, OrderDate = DateTime.Now });
            context.Products.Add(new Product { Id = 10, Name = "Bread" });
            context.OrderDetails.Add(new OrderDetail { Id = 1, OrderId = 5, ProductId = 10, Quantity = 2 });
            await context.SaveChangesAsync();

            var controller = new OrdersController(context);

            // Act
            var result = await controller.Index(5) as ViewResult;
            var model = result.Model as OrderIndexData;

            // Assert
            Assert.IsNotNull(result);

            Assert.AreEqual(5, result.ViewData["OrderID"]);

            Assert.AreEqual(1, model.OrderDetails.Count());
            Assert.AreEqual("Bread", model.OrderDetails.First().Product.Name);
        }

        [TestMethod]
        public async Task AddDetail_ValidData_AddsDetailAndRedirects()
        {
            // Arrange
            using var context = GetContext();
            var controller = new OrdersController(context);

            // Act
            var result = await controller.AddDetail(OrderId: 1, ProductId: 10, Quantity: 5.5) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Edit", result.ActionName);
            Assert.AreEqual(1, result.RouteValues["id"]);

            var detail = context.OrderDetails.FirstOrDefault();
            Assert.IsNotNull(detail);
            Assert.AreEqual(5.5, detail.Quantity);
        }

        [TestMethod]
        public async Task DeleteDetail_RemovesDetailFromDb()
        {
            // Arrange
            using var context = GetContext();
            var detail = new OrderDetail { Id = 100, OrderId = 1, ProductId = 1, Quantity = 1 };
            context.OrderDetails.Add(detail);
            await context.SaveChangesAsync();
            var controller = new OrdersController(context);

            // Act
            await controller.DeleteDetail(100);

            // Assert
            Assert.AreEqual(0, context.OrderDetails.Count());
        }

        [TestMethod]
        public async Task Create_Post_InvalidModel_ReturnsViewWithSelectLists()
        {
            // Arrange
            using var context = GetContext();
            
            context.Customers.Add(new Customer
            {
                FirstName = "Ivan",
                LastName = "Test",
                Address = "Kyiv"
            });
            await context.SaveChangesAsync();

            var controller = new OrdersController(context);
            controller.ModelState.AddModelError("Error", "Model is invalid");
            var order = new Order { CustomerId = 1 };

            // Act
            var result = await controller.Create(order) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ViewData["CustomerId"]);
            Assert.IsNotNull(result.ViewData["SuperMarketId"]);
            Assert.AreEqual(order, result.Model);
        }
    }
}
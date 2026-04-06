using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using EFC.Models;

namespace EFC.Tests.Models
{
    [TestClass]
    public class OrderTests
    {
        [TestMethod]
        public void Order_Creation_ShouldReturnNotNull() => Assert.IsNotNull(new Order());

        [TestMethod]
        public void Order_SetAndGetId_ShouldWorkCorrectly()
        {
            var order = new Order { Id = 100 };
            Assert.AreEqual(100, order.Id);
        }

        [TestMethod]
        public void Order_SetAndGetDate_ShouldWorkCorrectly()
        {
            var date = new DateTime(2026, 1, 1);
            var order = new Order { OrderDate = date };
            Assert.AreEqual(date, order.OrderDate);
        }

        [TestMethod]
        public void Order_CustomerProperty_ShouldSetAndGetCorrectly()
        {
            var customer = new Customer { FirstName = "Ivan" };
            var order = new Order { Customer = customer };
            Assert.AreEqual("Ivan", order.Customer.FirstName);
        }

        [TestMethod]
        public void Order_OrderDetailsCollection_ShouldBeInitializable()
        {
            var order = new Order { OrderDetails = new List<OrderDetail>() };
            Assert.IsNotNull(order.OrderDetails);
        }

        [TestMethod]
        public void Order_UpdateOrderDate_ShouldChangeValue()
        {
            var order = new Order { OrderDate = new DateTime(2026, 1, 1) };
            var newDate = new DateTime(2026, 12, 31);
            order.OrderDate = newDate;
            Assert.AreEqual(newDate, order.OrderDate);
        }
    }
}
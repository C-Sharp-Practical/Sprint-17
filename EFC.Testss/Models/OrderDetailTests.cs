using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFC.Models;

namespace EFC.Tests.Models
{
    [TestClass] 
    public class OrderDetailTests
    {
        [TestMethod] 
        public void OrderDetail_Creation_ShouldReturnNotNull() => Assert.IsNotNull(new OrderDetail());

        [TestMethod]
        public void OrderDetail_SetAndGetOrderId_ShouldWorkCorrectly()
        {
            var detail = new OrderDetail { OrderId = 10 };
            Assert.AreEqual(10, detail.OrderId);
        }

        [TestMethod]
        public void OrderDetail_SetAndGetProductId_ShouldWorkCorrectly()
        {
            var detail = new OrderDetail { ProductId = 3 };
            Assert.AreEqual(3, detail.ProductId);
        }

        [TestMethod]
        public void OrderDetail_SetAndGetQuantity_ShouldWorkCorrectly()
        {
            var detail = new OrderDetail { Quantity = 5 }; 
            Assert.AreEqual(5, detail.Quantity);
        }

        [TestMethod]
        public void OrderDetail_UpdateQuantity_ShouldChangeValue()
        {
            var detail = new OrderDetail { Quantity = 1 };
            detail.Quantity = 10;
            Assert.AreEqual(10, detail.Quantity);
        }

        [TestMethod]
        public void OrderDetail_UpdateOrderId_ShouldChangeValue()
        {
            var detail = new OrderDetail { OrderId = 1 };
            detail.OrderId = 99;
            Assert.AreEqual(99, detail.OrderId);
        }

        [TestMethod]
        public void OrderDetail_UpdateProductId_ShouldChangeValue()
        {
            var detail = new OrderDetail { ProductId = 2 };
            detail.ProductId = 88;
            Assert.AreEqual(88, detail.ProductId);
        }
    }
}
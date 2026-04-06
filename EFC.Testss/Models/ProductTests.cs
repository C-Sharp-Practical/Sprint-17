using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFC.Models;

namespace EFC.Tests.Models
{
    [TestClass]
    public class ProductTests
    {
        [TestMethod]
        public void Product_Creation_ShouldReturnNotNull() => Assert.IsNotNull(new Product());

        [TestMethod]
        public void Product_SetAndGetId_ShouldWorkCorrectly()
        {
            var product = new Product { Id = 1 };
            Assert.AreEqual(1, product.Id);
        }

        [TestMethod]
        public void Product_SetAndGetName_ShouldWorkCorrectly()
        {
            var product = new Product { Name = "Milk" };
            Assert.AreEqual("Milk", product.Name);
        }

        [TestMethod]
        public void Product_SetAndGetPrice_ShouldWorkCorrectly()
        {
            var product = new Product { Price = 25 };
            Assert.AreEqual(25, product.Price);
        }

        [TestMethod]
        public void Product_UpdatePrice_ShouldChangeValue()
        {
            var product = new Product { Price = 20 };
            product.Price = 30;
            Assert.AreEqual(30, product.Price);
        }
    }
}
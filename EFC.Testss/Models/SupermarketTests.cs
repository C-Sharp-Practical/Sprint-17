using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFC.Models;

namespace EFC.Tests.Models
{
    [TestClass]
    public class SupermarketTests
    {
        [TestMethod]
        public void Supermarket_Creation_ShouldReturnNotNull() => Assert.IsNotNull(new Supermarket());

        [TestMethod]
        public void Supermarket_SetAndGetId_ShouldWorkCorrectly()
        {
            var market = new Supermarket { Id = 1 };
            Assert.AreEqual(1, market.Id);
        }

        [TestMethod]
        public void Supermarket_SetAndGetName_ShouldWorkCorrectly()
        {
            var market = new Supermarket { Name = "ATB" };
            Assert.AreEqual("ATB", market.Name);
        }

        [TestMethod]
        public void Supermarket_SetAndGetAddress_ShouldWorkCorrectly()
        {
            var market = new Supermarket { Address = "Kyiv" };
            Assert.AreEqual("Kyiv", market.Address);
        }

        [TestMethod]
        public void Supermarket_UpdateName_ShouldChangeValue()
        {
            var market = new Supermarket { Name = "Silpo" };
            market.Name = "Novus";
            Assert.AreEqual("Novus", market.Name);
        }
    }
}
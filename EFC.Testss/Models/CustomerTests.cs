using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFC.Models;

namespace EFC.Tests.Models
{
    [TestClass]
    public class CustomerTests
    {
        [TestMethod]
        public void Customer_Creation_ShouldReturnNotNull() => Assert.IsNotNull(new Customer());

        [TestMethod]
        public void Customer_SetAndGetId_ShouldWorkCorrectly()
        {
            var customer = new Customer { Id = 1 };
            Assert.AreEqual(1, customer.Id);
        }

        [TestMethod]
        public void Customer_SetAndGetFirstName_ShouldWorkCorrectly()
        {
            var customer = new Customer { FirstName = "Ivan" };
            Assert.AreEqual("Ivan", customer.FirstName);
        }

        [TestMethod]
        public void Customer_SetAndGetLastName_ShouldWorkCorrectly()
        {
            var customer = new Customer { LastName = "Franko" };
            Assert.AreEqual("Franko", customer.LastName);
        }

        [TestMethod]
        public void Customer_UpdateName_ShouldChangeValue()
        {
            var customer = new Customer { FirstName = "Ivan" };
            customer.FirstName = "Taras";
            Assert.AreEqual("Taras", customer.FirstName);
        }
    }
}
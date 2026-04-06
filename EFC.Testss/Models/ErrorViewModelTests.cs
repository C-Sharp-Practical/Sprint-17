using Microsoft.VisualStudio.TestTools.UnitTesting;
using EFC.Models;

namespace EFC.Tests.Models
{
    [TestClass]
    public class ErrorViewModelTests
    {
        [TestMethod]
        public void ErrorViewModel_Creation_ShouldReturnNotNull() => Assert.IsNotNull(new ErrorViewModel());

        [TestMethod]
        public void ErrorViewModel_RequestId_ShouldSetAndGet()
        {
            var error = new ErrorViewModel { RequestId = "999-ABC" };
            Assert.AreEqual("999-ABC", error.RequestId);
        }

        [TestMethod]
        public void ErrorViewModel_ShowRequestId_ShouldReturnTrueWhenNotEmpty()
        {
            var error = new ErrorViewModel { RequestId = "999-ABC" };
            Assert.IsTrue(error.ShowRequestId);
        }

        [TestMethod]
        public void ErrorViewModel_ShowRequestId_ShouldReturnFalseWhenNull()
        {
            var error = new ErrorViewModel { RequestId = null };
            Assert.IsFalse(error.ShowRequestId);
        }
    }
}
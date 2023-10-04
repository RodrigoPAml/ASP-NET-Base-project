using Application.BCrypt;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.NUnit
{
    [TestClass]
    public class TestHash
    {
        [TestMethod]
        public void IsHasherValid()
        {
            string password = "123123123";

            var hash = PasswordHasher.EncryptPassword(password);
            
            Assert.IsTrue(PasswordHasher.IsValidPassword(password, hash));
        }
    }
}

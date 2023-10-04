using Infra.BCrypt;

namespace Tests.MS.Unit
{
    [TestClass]
    public class PasswordHashTest
    {
        [TestMethod]
        public void TestValid()
        {
            string password = "12312312321";
            string hash = BCryptHasher.EncryptPassword(password);

            Assert.IsTrue(BCryptHasher.IsValidPassword(password, hash));   
        }

        [TestMethod]
        public void TestInvalid()
        {
            string password = "19283789173123";
            string hash = BCryptHasher.EncryptPassword(password);

            Assert.IsFalse(BCryptHasher.IsValidPassword("123123123123123", hash));
        }
    }
}
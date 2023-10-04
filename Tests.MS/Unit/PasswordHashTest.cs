using Infra.BCrypt;

namespace Tests.MS.Unit
{
    [TestClass]
    public class PasswordHashTest
    {
        [TestMethod]
        [DataRow("#4j1239$DKT")] 
        [DataRow("rodrigo12345")]
        [DataRow("00011122222")] 
        public void TestValid(string password)
        {
            string hash = BCryptHasher.EncryptPassword(password);

            Assert.IsTrue(BCryptHasher.IsValidPassword(password, hash));   
        }

        [TestMethod]
        [DataRow("#4j1239$DKT")]
        [DataRow("rodrigo12345")]
        [DataRow("00011122222")]
        public void TestInvalid(string password)
        {
            string hash = BCryptHasher.EncryptPassword(password);

            Assert.IsFalse(BCryptHasher.IsValidPassword("121231233", hash));
        }
    }
}
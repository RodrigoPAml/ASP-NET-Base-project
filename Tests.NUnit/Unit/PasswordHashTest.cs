using Infra.BCrypt;

namespace Tests.NUnit.Unit
{
    [TestFixture]
    public class PasswordHashTest
    {
        [TestCase("#4j1239$DKT")]
        [TestCase("rodrigo12345")]
        [TestCase("00011122222")]
        public void TestValid(string password)
        {
            string hash = BCryptHasher.EncryptPassword(password);

            Assert.True(BCryptHasher.IsValidPassword(password, hash));   
        }

        [TestCase("#4j1239$DKT")]
        [TestCase("rodrigo12345")]
        [TestCase("00011122222")]
        public void TestInvalid(string password)
        {
            string hash = BCryptHasher.EncryptPassword(password);

            Assert.False(BCryptHasher.IsValidPassword("121231233", hash));
        }
    }
}
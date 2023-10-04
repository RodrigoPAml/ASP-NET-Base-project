using Infra.BCrypt;

namespace Tests.xUnit.Unit
{
    public class PasswordHashTest
    {
        [Theory]
        [InlineData("#4j1239$DKT")]
        [InlineData("rodrigo12345")]
        [InlineData("00011122222")]
        public void TestValid(string password)
        {
            string hash = BCryptHasher.EncryptPassword(password);

            Assert.True(BCryptHasher.IsValidPassword(password, hash));   
        }

        [Theory]
        [InlineData("#4j1239$DKT")]
        [InlineData("rodrigo12345")]
        [InlineData("00011122222")]
        public void TestInvalid(string password)
        {
            string hash = BCryptHasher.EncryptPassword(password);

            Assert.False(BCryptHasher.IsValidPassword("121231233", hash));
        }
    }
}
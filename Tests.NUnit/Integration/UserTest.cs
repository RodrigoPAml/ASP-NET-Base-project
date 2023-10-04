using Application.Responses;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Tests.NUnit.Integration
{
    [TestFixture]
    public class UserTest
    {
        private HttpClient _httpClient;

        private string _token = string.Empty;

        [OneTimeSetUp]
        public void Setup()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000") 
            };

            _token = GetToken();
        }

        private string GetToken()
        {
            try
            {
                HttpResponseMessage response = _httpClient.PostAsync("/Authentication/Login?login=Admin&password=AdminAdmin", null)
                    .GetAwaiter()
                    .GetResult();

                response.EnsureSuccessStatusCode();

                string contentAsString = response
                    .Content
                    .ReadAsStringAsync()
                    .GetAwaiter()
                    .GetResult();

                ResponseBody responseBody = JsonConvert.DeserializeObject<ResponseBody>(contentAsString);

                return (string)responseBody.Content;
            }
            catch
            {
                return "";
            }
        }

        [Test]
        public async Task TestLogin()
        {
            HttpResponseMessage response = await _httpClient.PostAsync("/Authentication/Login?login=Admin&password=AdminAdmin", null); 
            response.EnsureSuccessStatusCode();

            string contentAsString = await response.Content.ReadAsStringAsync();
            Console.WriteLine(contentAsString);

            ResponseBody responseBody = JsonConvert.DeserializeObject<ResponseBody>(contentAsString);

            Assert.NotNull(responseBody);
            Assert.True(((string)responseBody.Content).Count() > 0);
            Assert.True(responseBody.Success);

            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + (string)responseBody.Content);

            HttpResponseMessage test = await _httpClient.GetAsync("/Authentication/TestToken");
            test.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task CreateDeleteUser()
        {
            var data = new
            {
                Login = "john123",
                Name = "John",
                Password = "12345678910"
            };

            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _token);

            // Create user
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/User/CreateUser", data);
           
            response.EnsureSuccessStatusCode();

            string contentAsString = await response.Content.ReadAsStringAsync();

            ResponseBody responseBody = JsonConvert.DeserializeObject<ResponseBody>(contentAsString);
            Assert.NotNull(responseBody);
            Assert.True(((long)responseBody.Content) > 0);
            Assert.True(responseBody.Success);

            // Delete user
            HttpResponseMessage responseDelete = await _httpClient.DeleteAsync($"/User/DeleteUser?id={(long)responseBody.Content}");

            response.EnsureSuccessStatusCode();

            string contentAsStringDelete = await responseDelete.Content.ReadAsStringAsync();
            Console.WriteLine(contentAsStringDelete);

            ResponseBody responseBodyDelete = JsonConvert.DeserializeObject<ResponseBody>(contentAsStringDelete);
            Assert.NotNull(responseBodyDelete);
            Assert.True(responseBodyDelete.Success);
        }
    }
}

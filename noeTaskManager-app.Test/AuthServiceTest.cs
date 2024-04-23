using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.Protected;
using noeTaskManager_app.Models.AuthModels;
using noeTaskManager_app.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace noeTaskManager_app.Test
{
    public class AuthServiceTest
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;

        public AuthServiceTest()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost:5241/")
            };
            _mockHttpContextAccessor =  new Mock<IHttpContextAccessor>();
        }

        [Fact]
        public void AuthService_Initialise_ReturnAuthServiceInstance()
        {
            //Act
            var result = new AuthService(_httpClient, _mockHttpContextAccessor.Object);

            //Assert
            result.Should().BeOfType<AuthService>();
        }

        [Fact]
        public async Task AuthService_SignIn_ReturnMockToken()
        {
            //Arrange
            string jsonContent = @"
            {
                ""access_token"": ""abc123xyz"",
                ""user_cred"": {
                    ""firstName"": ""Alice"",
                    ""lastName"": ""Johnson"",
                    ""email"": ""alice.johnson@example.com"",
                    ""userUID"": ""U1001""
                }
            }";

            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(mockResponse);
            _mockHttpContextAccessor.Setup(_ => _.HttpContext.Response.Cookies.Append(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CookieOptions>())).Verifiable();
         

            var authService = new AuthService(_httpClient, _mockHttpContextAccessor.Object);

            //Act
            (bool IsSuccess, SigninResponseObject signinResponseObject) result = await authService.SignIn("alice.johnson@example.com", "test-password");

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.signinResponseObject.UserCreds.FirstName.Should().Be("Alice");
        }

        [Fact]
        public async Task AuthService_SignUp_ReturnsMockSignupObject()
        {
            //Arrange
            var jsonContent = @"{
                    ""firstName"": ""Alice"",
                    ""lastName"": ""Johnson"",
                    ""email"": ""alice.johnson@example.com"",
                    ""userUID"": ""U1001""
                }";

            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(mockResponse);

            var authService = new AuthService(_httpClient, _mockHttpContextAccessor.Object);

            //Act
            (bool IsSuccess, UserCredsResponse? userCredsResponse) result = await authService.SignUp("Alice", "Johnson", "alice.johnson@example.com", "example-password123444");

            //Arrange
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.userCredsResponse.FirstName.Should().NotBeNull();
            result.userCredsResponse.FirstName.Should().Be("Alice");
            

        }
    }
}

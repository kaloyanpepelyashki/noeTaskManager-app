using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using noeTaskManager_app.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}

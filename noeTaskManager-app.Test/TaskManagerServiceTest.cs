using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.Protected;
using noeTaskManager_app.Models;
using noeTaskManager_app.Services;
using noeTaskManager_app.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace noeTaskManager_app.Test
{
    public class TaskManagerServiceTest
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _HttpClient;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<IAuthService> _mockAuthService;

        public TaskManagerServiceTest()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _HttpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost:5241/")
            };
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockAuthService = new Mock<IAuthService>();
        }


        [Fact]
        public void TaskManagerService_Initialise_ReturnTaskManagerServiceInstance()
        {
            //Act
            var result = new TaskManagerService(_HttpClient, _mockHttpContextAccessor.Object, _mockAuthService.Object);

            //Assert
            result.Should().BeOfType<TaskManagerService>();
        }

        [Fact]
        public async Task TaskManager_GetAllTasks_ReturnAListOfMockTasks()
        {
            //Arrange
            var jsonContent = @"[ 
                {
                    ""id"": {
                        ""timestamp"": 1712239925,
                        ""machine"": 2513510,
                        ""pid"": -31671,
                        ""increment"": 13247963,
                        ""creationTime"": ""2024-04-04T14:12:05Z""
                    },
                    ""name"": ""Task 1""
                },
                {
                    ""id"": {
                        ""timestamp"": 1712239925,
                        ""machine"": 2513510,
                        ""pid"": -31671,
                        ""increment"": 13247963,
                        ""creationTime"": ""2024-04-04T14:12:05Z""
                    },
                    ""name"": ""Task 2""
                    }
                ]";

            
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            //Mocking the protected abstract method SendAsync part of the HttpMessageHandler class
            _mockHttpMessageHandler
                .Protected() //This must be here in order to test protected methods
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(mockResponse);

            //Act
            var taskManager = new TaskManagerService(_HttpClient, _mockHttpContextAccessor.Object, _mockAuthService.Object);
            var result = await taskManager.GetAllTasks();

            //Assert
            Assert.True(result.isSuccess);
            Assert.NotNull(result.Item2);
            Assert.Equal(2, result.Item2.Count);
        }

        [Fact] 
        public async Task TaskManagerService_GetAllTasks_NoTasks_ReturnsNoTasks()
        {
            //Arrange
            var jsonContent = @"[]";

            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(mockResponse);

            //Act
            var taskManager = new TaskManagerService(_HttpClient, _mockHttpContextAccessor.Object, _mockAuthService.Object);
            (bool isSuccess, List<TaskItem>? tasksList) result = await taskManager.GetAllTasks();

            //Assert
            result.isSuccess.Should().Be(false);
            Assert.Null(result.tasksList);

        }
    }
}

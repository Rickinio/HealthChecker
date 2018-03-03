using FluentAssertions;
using HealthChecker.Middleware.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Xunit;

namespace HealthChecker.Middleware.Tests
{
    public class Tests
    {
        [Fact]
        public void NewOptions_CreatesOptions()
        {
            var loggerFactory = new Moq.Mock<ILoggerFactory>();
            var healthCheck = new HealthCheckerMiddleware(null, loggerFactory.Object, new HealthCheckerOptions());
            Assert.NotNull(healthCheck);
        }


        [Fact]
        public async void InvokeTest_GivenDefaultOptions_ReturnsApplicationIsAlive()
        {
            //Arrange
            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseHealthcheckEndpoint(new HealthCheckerOptions() { });
                });

            var server = new TestServer(builder);

            //Act 
            var requestMessage = new HttpRequestMessage(new HttpMethod("GET"), "/healthcheck");
            var responseMessage = await server.CreateClient().SendAsync(requestMessage);

            //Assert
            var result = responseMessage.Content.ReadAsStringAsync().Result;
            var expected = "[{\"application\":\"testhost\",\"testMethod\":\"ApplicationIsAlive\",\"hasError\":false,\"exception\":null}]";
            result.Should().Match(expected);
        }

        [Fact]
        public async void InvokeTest_GivenObscurePathOptions_ReturnsApplicationIsAlive()
        {
            //Arrange
            var obscurePath = Guid.NewGuid().ToString();

            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseHealthcheckEndpoint(new HealthCheckerOptions() { ObscurePath = obscurePath });
                });

            var server = new TestServer(builder);

            //Act 
            var requestMessage = new HttpRequestMessage(new HttpMethod("GET"), $"/healthcheck/{obscurePath}");
            var responseMessage = await server.CreateClient().SendAsync(requestMessage);

            //Assert
            var result = responseMessage.Content.ReadAsStringAsync().Result;
            var expected = "[{\"application\":\"testhost\",\"testMethod\":\"ApplicationIsAlive\",\"hasError\":false,\"exception\":null}]";
            result.Should().Match(expected);

        }

        [Fact]
        public async void InvokeTest_CallingHeathCheckUrlWithWrongObscurePath_ReturnsApplicationIsAlive()
        {
            //Arrange
            var obscurePath = Guid.NewGuid().ToString();

            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseHealthcheckEndpoint(new HealthCheckerOptions() { ObscurePath = obscurePath });
                });

            var server = new TestServer(builder);

            //Act 
            var requestMessage = new HttpRequestMessage(new HttpMethod("GET"), $"/healthcheck/{obscurePath}-123");
            var responseMessage = await server.CreateClient().SendAsync(requestMessage);

            //Assert
            responseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async void InvokeTest_GivenOptionsThatAddActions_ReturnsSuccessForExecutionActions()
        {
            var expected = "[{\"application\":\"SomeApp\",\"testMethod\":\"ApplicationIsAlive\",\"hasError\":false,\"exception\":null},{\"application\":\"SomeApp\",\"testMethod\":\"testMethod1\",\"hasError\":false,\"exception\":null},{\"application\":\"SomeApp\",\"testMethod\":\"testMethod2\",\"hasError\":false,\"exception\":null}]";

            //Arrange
            Action testMethod1 = () => { };
            Action testMethod2 = () => { };

            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseHealthcheckEndpoint(new HealthCheckerOptions()
                    {
                        ApplicationName = "SomeApp",
                        TestActions = new List<TestAction>() {
                        new TestAction(){Name="testMethod1",Action=testMethod1},
                        new TestAction(){Name="testMethod2",Action=testMethod2}
                    }
                    });
                });

            var server = new TestServer(builder);

            //Act 
            var requestMessage = new HttpRequestMessage(new HttpMethod("GET"), "/healthcheck");
            var responseMessage = await server.CreateClient().SendAsync(requestMessage);

            //Assert
            var result = responseMessage.Content.ReadAsStringAsync().Result;

            result.Should().Match(expected);
        }

        [Fact]
        public async void InvokeTest_GivenOptionsThatAddActions_ReturnsErrorForExecutionActions()
        {
            //Arrange
            Action testMethod1 = () => { };
            Action testMethod2 = () => { throw new Exception("Custom Exception Message"); };

            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    app.UseHealthcheckEndpoint(new HealthCheckerOptions()
                    {
                        ApplicationName = "SomeApp",
                        TestActions = new List<TestAction>() {
                        new TestAction(){Name="testMethod1",Action=testMethod1},
                        new TestAction(){Name="testMethod2",Action=testMethod2}
                    }
                    });
                });

            var server = new TestServer(builder);

            //Act 
            var requestMessage = new HttpRequestMessage(new HttpMethod("GET"), "/healthcheck");
            var responseMessage = await server.CreateClient().SendAsync(requestMessage);

            //Assert
            var result = responseMessage.Content.ReadAsStringAsync().Result;

            result.Contains(@"Message"":""Custom Exception Message");
        }
    }
}

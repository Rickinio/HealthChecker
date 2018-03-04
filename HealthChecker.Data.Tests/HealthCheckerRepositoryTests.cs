using FluentAssertions;
using HealthChecker.Models;
using Xunit;

namespace HealthChecker.Data.Tests
{
    public class HealthCheckerRepositoryTests
    {
        [Fact]
        public async void InvokeTest_AddApplication_ReturnsSuccess()
        {
            //Arrange
            var application = new Application() { Name = "TestApp", Url = "https://localhost/healthchecker", Interval = 3600 };
            var repository = new HealthCheckerRepository();

            //Act 
            var addedApplication = await repository.AddApplication(application);

            //Assert
            addedApplication.ApplicationId.Should().NotBe(0);
        }
    }
}

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
            var application = new Application() {
                Name = "TestApp",
                Url = "https://localhost:26749/healthchecker/CC61205F-1D1F-40E5-929B-B2E1895FDFD0",
                Interval = 3600,
            };
            var repository = new HealthCheckerRepository();

            //Act 
            var addedApplication = await repository.AddApplication(application);

            //Assert
            addedApplication.ApplicationId.Should().NotBe(0);
        }
    }
}

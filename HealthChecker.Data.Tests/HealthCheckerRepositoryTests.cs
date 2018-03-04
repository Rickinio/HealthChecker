using FluentAssertions;
using HealthChecker.Models;
using Xunit;

namespace HealthChecker.Data.Tests
{
    [TestCaseOrderer("HealthChecker.Data.Tests.PriorityOrderer", "HealthChecker.Data.Tests")]
    public class HealthCheckerRepositoryTests
    {
        [Fact,TestPriority(0)]
        public async void InvokeTest_AddApplication_ReturnsSuccess()
        {
            //Arrange
            var application = new Application() {
                Name = "TestApp",
                Url = "http://testurl.com/",
                Interval = 3600,
            };
            var repository = new HealthCheckerRepository();

            //Act 
            var addedApplication = await repository.AddApplication(application);

            //Assert
            addedApplication.ApplicationId.Should().NotBe(0);
        }

        [Fact, TestPriority(1)]
        public async void InvokeTest_UpdateApplication_ReturnsSuccess()
        {
            //Arrange
            var repository = new HealthCheckerRepository();

            //Act 
            var application = repository.GetApplicationByName("TestApp");
            application.Name = "UpdatedTestApp";
            var updatedApplication = await repository.UpdateApplication(application);
            var dbApplication = await repository.GetApplicationById(application.ApplicationId);

            //Assert
            dbApplication.Name.Should().Be(updatedApplication.Name);
        }

        [Fact, TestPriority(2)]
        public async void InvokeTest_DeleteApplication_ReturnsSuccess()
        {
            //Arrange
            var repository = new HealthCheckerRepository();

            //Act 
            var application = repository.GetApplicationByName("UpdatedTestApp");
            await repository.DeleteApplication(application);
            var dbApplication = repository.GetApplicationByName("UpdatedTestApp");

            //Assert
            dbApplication.Should().Be(null);
        }
    }
}

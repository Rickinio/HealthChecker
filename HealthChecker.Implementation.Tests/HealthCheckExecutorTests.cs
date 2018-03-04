using FluentAssertions;
using Xunit;

namespace HealthChecker.Implementation.Tests
{
    public class HealthCheckExecutorTests
    {
        [Fact(Skip = "Web app is not running locally yet")]
        public async void InvokeTest_RunExecutor_ReturnsSuccess()
        {
            //Arrange
            var executor = new HealthCheckExecutor();

            //Act 
            var results = await executor.RunTests();

            //Assert
            results.Count.Should().Be(3);
        }
    }
}

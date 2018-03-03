using System;

namespace HealthChecker.Middleware.Models
{
    public class HealthCheckerResult
    {
        public string Application { get; set; }
        public string TestMethod { get; set; }
        public bool HasError { get; set; }
        public Exception Exception { get; set; }
    }
}

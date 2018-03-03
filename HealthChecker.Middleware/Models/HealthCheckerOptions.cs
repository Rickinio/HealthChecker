using System;
using System.Collections.Generic;

namespace HealthChecker.Middleware.Models
{
    public class HealthCheckerOptions
    {
        public HealthCheckerOptions()
        {
            Path = "/healthcheck";
            TestActions = new List<TestAction>();
        }

        public string ObscurePath { get; set; }
        public string Path { get; set; }
        public string ApplicationName { get; set; }
        public List<TestAction> TestActions { get; set; }
    }
}

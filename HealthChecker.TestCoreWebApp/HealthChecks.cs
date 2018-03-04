using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthChecker.TestCoreWebApp
{
    public class HealthChecks
    {
        public void TestSomethingSuccess()
        {
            var a = "All Good";
        }

        public void TestSomethingError()
        {
            var a = 15;
            var b = 0;
            var c = a / b;
        }
    }
}

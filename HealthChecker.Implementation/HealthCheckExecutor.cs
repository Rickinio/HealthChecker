using HealthChecker.Data;
using HealthChecker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HealthChecker.Implementation
{
    public class HealthCheckExecutor
    {
        public async Task<List<HealthCheckerResult>> RunTests()
        {
            var results = new List<HealthCheckerResult>();
            var repository = new HealthCheckerRepository();
            var applications = repository.GetAllApplications();

            foreach (var app in applications)
            {
                using(var httpClient = new HttpClient())
                {
                    var data = await httpClient.GetAsync(app.Url).Result.Content.ReadAsStringAsync();
                    results = JsonConvert.DeserializeObject<List<HealthCheckerResult>>(data);                    
                }
            }

            return results;
        }
    }
}

using HealthChecker.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace HealthChecker.Middleware
{
    public class HealthCheckerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly HealthCheckerOptions _options;

        public HealthCheckerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, HealthCheckerOptions options)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<HealthCheckerMiddleware>();
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments($"{_options.Path}/{_options.ObscurePath}".TrimEnd('/')))
            {
                _logger.LogInformation("Healthcheck requested: " + context.Request.Path);

                var assembly = Assembly.GetEntryAssembly();
                var appVersion = assembly
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    .InformationalVersion;

                if (string.IsNullOrEmpty(_options.ApplicationName))
                {
                    _options.ApplicationName = assembly.GetName().Name;
                }

                var response = new List<HealthCheckerResult>()
                {
                    new HealthCheckerResult()
                    {
                        Application = _options.ApplicationName,
                        TestMethod = "ApplicationIsAlive",
                    }
                };

                foreach (var testMethod in _options.TestActions)
                {
                    var healthCheckResult = new HealthCheckerResult() {Application = _options.ApplicationName,TestMethod = testMethod.Name };
                    var stopwatch = new Stopwatch();
                    try
                    {
                        stopwatch.Start();
                        testMethod.Action.Invoke();
                    }
                    catch (Exception ex)
                    {
                        healthCheckResult.HasError = true;
                        healthCheckResult.Exception = ex;
                    }
                    healthCheckResult.ExecutedIn = stopwatch.Elapsed.TotalSeconds;
                    stopwatch.Reset();
                    response.Add(healthCheckResult);
                }

                await context.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
            }
            else
            {
                await _next(context);
            }
        }
    }

    public static class HealthCheckMiddlewareExtension
    {
        public static IApplicationBuilder UseHealthcheckEndpoint(this IApplicationBuilder builder, HealthCheckerOptions options)
        {
            return builder.UseMiddleware<HealthCheckerMiddleware>(options);
        }
    }
}

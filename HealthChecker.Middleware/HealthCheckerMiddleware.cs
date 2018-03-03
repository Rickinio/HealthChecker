using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
            if (context.Request.Path.StartsWithSegments(_options.Path))
            {
                _logger.LogInformation("Healthcheck requested: " + context.Request.Path);

                var assembly = Assembly.GetEntryAssembly();
                var appVersion = assembly
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    .InformationalVersion;

                if (string.IsNullOrEmpty(_options.App))
                {
                    _options.App = assembly.GetName().Name;
                }

                var response = new
                {
                    Message = _options.Message,
                    Version = appVersion,
                    App = _options.App
                };

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

    public class HealthCheckerOptions
    {
        public HealthCheckerOptions()
        {
            Path = "/healthcheck";
            Message = "i am alive!";
        }

        public string Path { get; set; }
        public string Message { get; set; }
        public string App { get; set; }
    }
}

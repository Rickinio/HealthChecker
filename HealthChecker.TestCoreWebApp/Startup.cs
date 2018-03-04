using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthChecker.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HealthChecker.TestCoreWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseHealthcheckEndpoint(new Models.HealthCheckerOptions()
            {
                ApplicationName="TestCoreApp",
                ObscurePath = "CC61205F-1D1F-40E5-929B-B2E1895FDFD0",
                Path = "/healthcheck",                
                TestActions = new List<Models.TestAction>()
                {
                    new Models.TestAction(){Name="SuccessMethod",Action = new HealthChecks().TestSomethingSuccess},
                    new Models.TestAction(){Name="ErrorMethod",Action = new HealthChecks().TestSomethingError}
                }
            });
            app.UseMvc();
        }
    }
}

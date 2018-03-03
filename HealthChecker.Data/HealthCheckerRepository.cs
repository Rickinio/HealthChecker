using HealthChecker.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthChecker.Data
{
    public class HealthCheckerRepository
    {
        public async Task<Application> GetById(int applicationId)
        {
            using (var context = new HealthCheckerContext())
            {
                return await context.Applications.FindAsync(applicationId);
            }
        }

        public async Task<Application> GetByName(string applicationName)
        {
            using (var context = new HealthCheckerContext())
            {
                return context.Applications.FirstOrDefault(x => x.Name == applicationName);
            }
        }

        public async Task Add(Application application)
        {
            using (var context = new HealthCheckerContext())
            {
                context.Applications.Add(application);
                await context.SaveChangesAsync();
            }
        }

        public async Task Update(Application application)
        {
            using (var context = new HealthCheckerContext())
            {
                var dbApplication = context.Applications.Find(application.ApplicationId);
                context.Entry(dbApplication).CurrentValues.SetValues(application);
                await context.SaveChangesAsync();
            }
        }

        public async Task Delete(Application application)
        {
            using (var context = new HealthCheckerContext())
            {
                context.Applications.Remove(application);
                await context.SaveChangesAsync();
            }
        }
    }
}

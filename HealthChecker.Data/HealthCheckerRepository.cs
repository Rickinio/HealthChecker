using HealthChecker.Models;
using System.Linq;
using System.Threading.Tasks;

namespace HealthChecker.Data
{
    public class HealthCheckerRepository
    {
        public async Task<Application> GetApplicationById(int applicationId)
        {
            using (var context = new HealthCheckerContext())
            {
                return await context.Applications.FindAsync(applicationId);
            }
        }

        public Application GetApplicationByName(string applicationName)
        {
            using (var context = new HealthCheckerContext())
            {
                return context.Applications.FirstOrDefault(x => x.Name == applicationName);
            }
        }

        public async Task<Application> AddApplication(Application application)
        {
            using (var context = new HealthCheckerContext())
            {
                context.Applications.Add(application);
                await context.SaveChangesAsync();

                return application;
            }
        }

        public async Task<Application> UpdateApplication(Application application)
        {
            using (var context = new HealthCheckerContext())
            {
                var dbApplication = context.Applications.Find(application.ApplicationId);
                context.Entry(dbApplication).CurrentValues.SetValues(application);
                await context.SaveChangesAsync();

                return application;
            }
        }

        public async Task DeleteApplication(Application application)
        {
            using (var context = new HealthCheckerContext())
            {
                context.Applications.Remove(application);
                await context.SaveChangesAsync();
            }
        }
    }
}

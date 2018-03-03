using System.ComponentModel.DataAnnotations;

namespace HealthChecker.Data.Models
{
    public class Application
    {
        [Key]
        public int ApplicationId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Interval { get; set; }
    }
}

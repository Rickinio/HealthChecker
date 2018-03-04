using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthChecker.Models
{
    public class Application
    {
        [Key]
        public long ApplicationId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Interval { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateTimeCreated { get; set; }
    }
}

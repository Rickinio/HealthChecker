using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthChecker.Models
{
    public class HealthCheckerResult
    {
        internal string _errorMessage;
        internal string _stackTrace;

        [Key]
        public long ResultId { get; set; }
        public string Application { get; set; }
        public string TestMethod { get; set; }
        public double ExecutedIn { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage
        {
            get => Exception?.GetAllFootprints();
            set => _errorMessage = value;
        }
        public string StackTrace
        {
            get => Exception != null ? JsonConvert.SerializeObject(Exception) : null;
            set => _stackTrace = value;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateTimeCreated { get; set; }

        [NotMapped]
        public Exception Exception { get; set; }
        
    }
}

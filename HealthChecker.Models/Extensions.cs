using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace HealthChecker.Models
{
    public static class Extensions
    {
        public static string GetAllFootprints(this Exception ex)
        {
            var st = new StackTrace(ex, true);
            var frames = st.GetFrames();
            var traceString = new StringBuilder();

            var exceptionMessages = ex.Messages();
            exceptionMessages.ForEach((m) =>
            {
                traceString.AppendLine($"Exception: {m}");
            });

            if (frames != null)
            {
                foreach (var frame in frames)
                {
                    if (frame.GetFileLineNumber() < 1)
                        continue;

                    traceString.AppendLine("File: " + frame.GetFileName());
                    traceString.AppendLine("Method:" + frame.GetMethod().Name);
                    traceString.AppendLine("LineNumber: " + frame.GetFileLineNumber());
                    traceString.AppendLine(" <-- -->  ");
                }
            }

            return traceString.ToString();
        }

        public static List<string> Messages(this Exception ex)
        {
            List<string> messages = new List<string>();

            if (ex == null) { return messages; };

            messages.Add(ex.Message);

            IEnumerable<Exception> innerExceptions = Enumerable.Empty<Exception>();

            if (ex is AggregateException && (ex as AggregateException).InnerExceptions.Any())
            {
                innerExceptions = (ex as AggregateException).InnerExceptions;
            }
            else if (ex.InnerException != null)
            {
                innerExceptions = new Exception[] { ex.InnerException };
            }

            foreach (var innerEx in innerExceptions)
            {
                foreach (string msg in innerEx.Messages())
                {
                    messages.Add(msg);
                }
            }

            return messages;
        }
    }
}

using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildFunc = System.Action<System.Func<System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>, System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>>>;

namespace Microservice.Logging
{
    public static class Extensions
    {
        public static BuildFunc UseMonitoringAndLogging(this BuildFunc pipeline, ILogger log, Func<Task<bool>> healthCheck)
        {
            return pipeline
                .UseCorrelationToken()
                .UseRequestLogging(log)
                .UsePerformanceLogging(log)
                .UseMonitoring(healthCheck)
                .UseGlobalErrorLogging(log);
        } 
    }
}

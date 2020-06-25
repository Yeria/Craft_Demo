using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CraftBackEnd.Services.Core
{
    public class LoggerFacade : ILoggerFacade
    {
        private readonly ILogger _logger;

        public LoggerFacade(ILoggerFactory loggerFactory) {
            // initialize logger: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?tabs=aspnetcore2x
            // we can use categories to separate between logs (unit services could have their own logger, ...)
            _logger = loggerFactory.CreateLogger("CB.CCC");
        }

        public void LogTrace(EventId eventId, Exception exception, string message, params object[] args) {
            _logger.LogTrace(eventId, exception, message, args);
            Trace.TraceInformation(message);
        }

        public void LogDebug(EventId eventId, Exception exception, string message, params object[] args) {
            _logger.LogDebug(eventId, exception, message, args);
            Trace.TraceInformation(message);
        }

        public void LogInformation(EventId eventId, Exception exception, string message, params object[] args) {
            _logger.LogInformation(eventId, exception, message, args);
            Trace.TraceInformation(message);
        }

        public void LogWarning(EventId eventId, Exception exception, string message, params object[] args) {
            _logger.LogWarning(eventId, exception, message, args);
            Trace.TraceWarning(message);
        }

        public void LogError(EventId eventId, Exception exception, string message, params object[] args) {
            _logger.LogError(eventId, exception, message, args);
            Trace.TraceError(message);
        }

        public void LogCritical(EventId eventId, Exception exception, string message, params object[] args) {
            _logger.LogCritical(eventId, exception, message, args);
            Trace.TraceError(message);
        }
    }
}

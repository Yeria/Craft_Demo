using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CraftBackEnd.Services.Core
{
    public interface ILoggerFacade
    {
        void LogTrace(EventId eventId, Exception exception, string message, params object[] args);
        void LogDebug(EventId eventId, Exception exception, string message, params object[] args);
        void LogInformation(EventId eventId, Exception exception, string message, params object[] args);
        void LogWarning(EventId eventId, Exception exception, string message, params object[] args);
        void LogError(EventId eventId, Exception exception, string message, params object[] args);
        void LogCritical(EventId eventId, Exception exception, string message, params object[] args);
    }
}

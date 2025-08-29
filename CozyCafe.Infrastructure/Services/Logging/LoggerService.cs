using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Application.Interfaces.Logging;
using Microsoft.Extensions.Logging;

namespace CozyCafe.Infrastructure.Services.Logging
{
    /// <summary>
    /// (UA) Реалізація сервісу логування на основі вбудованого `ILogger`.  
    /// Дозволяє централізовано вести журнали різних рівнів:  
    /// - Інформаційні повідомлення (LogInfo)  
    /// - Попередження (LogWarning)  
    /// - Помилки (LogError) з можливістю передати виняток  
    /// - Логування з прив’язкою до користувача  
    /// Використовується в інфраструктурному шарі для спрощеного доступу до системи логування.  
    ///
    /// (EN) Implementation of a logging service based on the built-in `ILogger`.  
    /// Provides centralized logging across different levels:  
    /// - Informational messages (LogInfo)  
    /// - Warnings (LogWarning)  
    /// - Errors (LogError) with optional exception details  
    /// - Logging tied to a specific user  
    /// Used in the infrastructure layer to simplify access to the logging system.
    /// </summary>
    public class LoggerService : ILoggerService
    {
        private readonly ILogger<LoggerService> _logger;

        public LoggerService(ILogger<LoggerService> logger)
        {
            _logger = logger;
        }

        public void LogInfo(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        public void LogError(string message)
        {
            _logger.LogError(message);
        }

        public void LogError(Exception ex, string message)
        {
            _logger.LogError(ex, message);
        }
        public void LogInfo(string userName, string message)
        {
            _logger.LogInformation("Користувач: {User} | {Message}", userName, message);
        }

    }
}

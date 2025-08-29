using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozyCafe.Application.Interfaces.Logging
{
    /// <summary>
    /// (UA) Інтерфейс для сервісу логування.  
    /// Визначає базові методи для роботи з логами:  
    /// - Інформаційні повідомлення  
    /// - Попередження  
    /// - Помилки (з повідомленням або з винятком)  
    /// - Логування дій користувача  
    /// Забезпечує абстракцію над системою логування для зручності тестування та розширюваності.  
    ///
    /// (EN) Interface for the logging service.  
    /// Defines basic methods for logging:  
    /// - Informational messages  
    /// - Warnings  
    /// - Errors (with a message or with an exception)  
    /// - User-related logging  
    /// Provides an abstraction over the logging system for easier testing and extendability.
    /// </summary>
    public interface ILoggerService
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogError(Exception ex, string message);
        void LogInfo(string userName, string message);
    }
}

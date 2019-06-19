using Logger.Data;
using Logger.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logger
{
   public class ExamLogger : ILogger
    {
        private LoggerDbContext _context = new LoggerDbContext();
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
           
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }
            var message = formatter(state, exception);
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            if (exception != null)
            {
                message += "\n" + exception.ToString();
            }
            try
            {
                _context.EventLog.Add(new EventLog
                {
                    Message = message,
                    EventId = eventId.Id,
                    LogLevel = logLevel.ToString(),
                    CreatedTime = DateTime.UtcNow
                }) ;
                _context.SaveChanges();
            }
            catch {}
        }
    }
}

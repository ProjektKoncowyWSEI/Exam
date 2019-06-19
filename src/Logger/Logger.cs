using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExamLogger
{
    public class Logger : ILogger
    {
    
    public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Directory.CreateDirectory("D://bledy/");
            try
            {
                File.AppendAllText(Path.Combine("D://bledy/", "błędy"),
                    $"{logLevel.ToString()} {Environment.NewLine} Data: {DateTime.Now} Błąd: {state.ToString()} Exeption {exception} ");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

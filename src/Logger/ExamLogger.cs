using Logger.Data;
using Logger.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Logger
{
    public class ExamLogger : ILogger
    {
        public ExamLogger(LoggerDbContext context, IHttpContextAccessor httpContextAccessor) //, IEmailSender emailSender)
        {
            if (context != null)
            {
                var dbFilePath = context.Database.GetDbConnection().ConnectionString.Replace("Filename=", "");
                if (!File.Exists(dbFilePath))
                {
                    var dir = new FileInfo(dbFilePath).Directory.FullName;
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    File.Create(dbFilePath).Close();
                    context.Database.Migrate();
                }
            }
            _context = context;
            this.httpContextAccessor = httpContextAccessor;
            //_emailSender = emailSender;
        }
        private LoggerDbContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;

        //private readonly IEmailSender _emailSender;

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
            string login = httpContextAccessor.HttpContext.User.Identity.Name;
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
                //throw new Exception("test");
                _context.EventLog.Add(new EventLog
                {
                    Message = $"*** {login} ***{Environment.NewLine}{message}",
                    EventId = eventId.Id,
                    LogLevel = logLevel.ToString(),
                    CreatedTime = DateTime.Now
                });
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
                //_emailSender.SendEmailAsync("lnew84@gmail.com", "Exam error", ex.Message).Wait();
                //_emailSender.SendEmailAsync("krzysztof.sauermann@gmail.com", "Exam error", ex.Message).Wait();
            }
        }
    }
    public class ExamLogger<T> : ILogger<T>
    {
        private LoggerDbContext _context;
        //private readonly IEmailSender _emailSender;
        private ExamLogger examLogger;
        public ExamLogger(LoggerDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            //_emailSender = emailSender;
            examLogger = new ExamLogger(context, httpContextAccessor);
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return examLogger.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return examLogger.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            examLogger.Log(logLevel, eventId, state, exception, formatter);
        }
    }
}

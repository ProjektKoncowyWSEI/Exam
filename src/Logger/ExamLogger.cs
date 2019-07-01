using Logger.Data;
using Logger.Model;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Logger
{
    public class ExamLogger : ILogger
    {
        public ExamLogger(LoggerDbContext context, IEmailSender emailSender)
        {
            var dbFilePath = context.Database.GetDbConnection().ConnectionString.Replace("Filename=","");
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
            _context = context;
            _emailSender = emailSender;
        }
        private LoggerDbContext _context;
        private readonly IEmailSender _emailSender;

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
                });
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                //_emailSender.SendEmailAsync("lnew84@gmail.com", "Exam error", ex.Message).Wait();
                _emailSender.SendEmailAsync("krzysztof.sauermann@gmail.com", "Exam error", ex.Message).Wait();
            }
        }
    }
    public class ExamLogger<T> : ILogger<T>
    {
        private LoggerDbContext _context;
        private readonly IEmailSender _emailSender;
        private ExamLogger examLogger;
        public ExamLogger(LoggerDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
            examLogger = new ExamLogger(context, emailSender);
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

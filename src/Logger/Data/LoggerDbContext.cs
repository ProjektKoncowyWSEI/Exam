using Logger.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Logger.Data
{
    public class LoggerDbContext : DbContext
    {
       
        public DbSet<EventLog> EventLog { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=ExamLogger.db");
        }
    }
}

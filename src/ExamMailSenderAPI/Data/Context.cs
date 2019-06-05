using ExamContract.MailingModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMailSenderAPI.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)  :base(options)
        {

        }
        public DbSet<MailModel> Mails { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
    }
}

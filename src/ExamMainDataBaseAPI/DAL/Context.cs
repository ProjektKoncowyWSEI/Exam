using System;
using ExamContract.Auth;
using ExamContract.MainDbModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExamMainDataBaseAPI.DAL
{
    public partial class Context : DbContext
    {
     
        public Context(DbContextOptions<Context> options)
           : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {            
            builder.Entity<ExamApproache>().HasKey(table => new {
                table.ExamId,
                table.Login
            });
            builder.Entity<ExamApproacheResult>().HasKey(table => new {
                table.Login,
                table.ExamId,
                table.QuestionId,
                table.AnswerId
            });
        }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Answer> Answer { get; set; }      
        public DbSet<Question> Questions { get; set; }       
        public DbSet<ExamApproache> ExamApproaches { get; set; }
        public DbSet<ExamApproacheResult> ExamApproacheResults { get; set; }
        public DbSet<Key> Keys { get; set; }
    }
}

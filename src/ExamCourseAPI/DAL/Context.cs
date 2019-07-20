using ExamContract.CourseModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamCourseAPI.DAL
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
           : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(table => new { table.CourseId, table.Login }).IsUnique();
            builder.Entity<ExamCourse>().HasKey(table => new {
                table.CourseId,
                table.ExamId
            });
            builder.Entity<TutorialCourse>().HasKey(table => new {
                table.CourseId,
                table.TutorialId
            });
        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<ExamCourse> ExamCourses { get; set; }
        public DbSet<TutorialCourse> TutorialCourses { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

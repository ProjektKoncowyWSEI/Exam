using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExamMainDataBaseAPI.Models
{
    public partial class ExamQuestionsDbContext : DbContext
    {
     
        public ExamQuestionsDbContext(DbContextOptions<ExamQuestionsDbContext> options)
           : base(options)
        {
        }
       
       
        public DbSet<Answer> Answer { get; set; }
        public DbSet<AnswersType> AnswersType { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswer { get; set; }
        public DbSet<Questions> Questions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.answer)
                    .IsRequired()
                    .HasColumnName("Answer");
            });

            modelBuilder.Entity<AnswersType>(entity =>
            {
                entity.HasKey(e => e.AnswerType)
                    .HasName("PK_AnswerType");

                entity.Property(e => e.AnswerType)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<QuestionAnswer>(entity =>
            {
                entity.HasKey(e => new { e.AnswerId, e.QuestionId });

                entity.Property(e => e.AnswerId).HasColumnName("AnswerID");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.QuestionAnswer)
                    .HasForeignKey(d => d.AnswerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuestionAnswer_Answer");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.QuestionAnswer)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuestionAnswer_Questions");
            });

            modelBuilder.Entity<Questions>(entity =>
            {
                entity.Property(e => e.AnswerType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Question)
                    .IsRequired()
                    .IsUnicode(false);
            });
        }
    }
}

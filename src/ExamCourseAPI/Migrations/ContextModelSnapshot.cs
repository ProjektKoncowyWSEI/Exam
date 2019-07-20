﻿// <auto-generated />
using ExamCourseAPI.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ExamCourseAPI.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ExamContract.CourseModels.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("Description");

                    b.Property<string>("Login")
                        .HasMaxLength(256);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("ExamContract.CourseModels.ExamCourse", b =>
                {
                    b.Property<int>("CourseId");

                    b.Property<int>("ExamId");

                    b.HasKey("CourseId", "ExamId");

                    b.ToTable("ExamCourses");
                });

            modelBuilder.Entity("ExamContract.CourseModels.TutorialCourse", b =>
                {
                    b.Property<int>("CourseId");

                    b.Property<int>("TutorialId");

                    b.HasKey("CourseId", "TutorialId");

                    b.ToTable("TutorialCourses");
                });

            modelBuilder.Entity("ExamContract.CourseModels.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<int>("CourseId");

                    b.Property<string>("Login")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ExamContract.CourseModels.ExamCourse", b =>
                {
                    b.HasOne("ExamContract.CourseModels.Course")
                        .WithMany("ExamCourses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ExamContract.CourseModels.TutorialCourse", b =>
                {
                    b.HasOne("ExamContract.CourseModels.Course")
                        .WithMany("TutorialCourses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ExamContract.CourseModels.User", b =>
                {
                    b.HasOne("ExamContract.CourseModels.Course")
                        .WithMany("Users")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

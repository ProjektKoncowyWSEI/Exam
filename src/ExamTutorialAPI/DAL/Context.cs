using System;
using ExamContract.TutorialModels;
using ExamTutorialsAPI.Helpers;
using ExamTutorialsAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options)
    {
    }
    public DbSet<Tutorial> Tutorials { get; set; }   
    public DbSet<User> Users { get; set; }
    public DbSet<Key> Keys { get; set; }
}

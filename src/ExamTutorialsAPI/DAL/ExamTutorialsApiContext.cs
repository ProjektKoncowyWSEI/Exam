using System;
using ExamTutorialsAPI.Helpers;
using ExamTutorialsAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

public class ExamTutorialsApiContext : DbContext
{
	
    public ExamTutorialsApiContext(DbContextOptions<ExamTutorialsApiContext> options) : base(options)
    {
    }

    public DbSet<Tutorial> Tutorials { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Key> Keys { get; set; }

    

   

}

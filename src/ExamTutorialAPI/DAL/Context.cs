using ExamContract.TutorialModels;
using Microsoft.EntityFrameworkCore;
using ExamContract.Auth;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options)
    {
    }
    public DbSet<Tutorial> Tutorials { get; set; }   
    public DbSet<User> Users { get; set; }
    public DbSet<Key> Keys { get; set; }
}

using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;


namespace MvcMovie.Repositories
{
    public class DBContext : DbContext
    {
            public DBContext(DbContextOptions<DBContext> options) : base(options){ }
            public DbSet<Student> Students { get; set; }       //query use
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Student>().ToTable<Student>("students");
                
            }
    }
}
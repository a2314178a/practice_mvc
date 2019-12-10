using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;


namespace MvcMovie.Repositories
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions options) : base(options){ }
        public DbSet<Student> Students { get; set; }       //query use
        public DbSet<Teacher> Teachers { get; set; } 
         public DbSet<Subject> Subjects { get; set; } 
        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable<Student>("students");
        }*/
    }
}
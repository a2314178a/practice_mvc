using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;


namespace MvcMovie.Repositories
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions options) : base(options){ }
        public DbSet<Student> Students { get; set; }       //query use
        public DbSet<User> Users { get; set; } 
        public DbSet<Subject> Subjects { get; set; } 
        public DbSet<Stu_sub> Stu_subs { get; set; } 
        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable<Student>("students");
        }*/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stu_sub>()
                .HasIndex(b => new{b.StudentID , b.SubjectID}).IsUnique();;
        }
    }
}
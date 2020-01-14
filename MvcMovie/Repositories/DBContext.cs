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
        public DbSet<SubTime> SubTimes { get; set; }
        public DbSet<SubHomework> SubHomeworks { get; set; }
        public DbSet<HomeworkURL> HomeworkURLs { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Student>().ToTable<Student>("students");
            modelBuilder.Entity<Student>().HasIndex(b=>b.AccountID).IsUnique();
            modelBuilder.Entity<Student>()
                .Property(b=>b.Sex).HasColumnType("tinyint(1) unsigned").HasDefaultValue(0);

            modelBuilder.Entity<SubTime>()
                .HasIndex(b => new{b.SubjectID, b.WeekDay, b.StartTime, b.EndTime}).IsUnique();

            modelBuilder.Entity<Stu_sub>()
                .Property(b => b.StudentID).HasColumnType("int(11) unsigned").HasDefaultValue(0);
            modelBuilder.Entity<Stu_sub>()
                .Property(b => b.SubjectID).HasColumnType("int(11) unsigned").HasDefaultValue(0);  //.IsRequired(false)
            modelBuilder.Entity<Stu_sub>()
                .HasIndex(b => new{b.StudentID , b.SubjectID}).IsUnique();

            modelBuilder.Entity<HomeworkURL>().HasIndex(b=>b.NewName).IsUnique();
        }
    }
}
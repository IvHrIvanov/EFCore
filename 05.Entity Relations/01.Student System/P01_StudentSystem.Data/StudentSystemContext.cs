using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;
using System.Diagnostics.CodeAnalysis;
namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {

        }
        public StudentSystemContext([NotNull] DbContextOptions options)
            :base(options)
        {

        }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<Homework> HomeworkSubmissions { get; set; }
        public virtual DbSet<StudentCourse> StudentCourses { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.CONNECTION_STRING);
            }

            
        }
        protected override void OnModelCreating(ModelBuilder modelBUilder)
        {
            modelBUilder.Entity<StudentCourse>().HasKey(x => new { x.CourseId, x.StudentId });
            base.OnModelCreating(modelBUilder);
        }
        
    }
}

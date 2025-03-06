using DziekanatBackend.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DziekanatBackend.Database
{
    public class DziekanatDbContext : DbContext
    {
        public DbSet<Student> Student { get; set; }
        public DbSet<Lecturer> Lecturer { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<CourseStudent> CourseStudents { get; set; }

        public DziekanatDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>(entity => { 
                entity.HasKey(e => e.Index);
                entity.Property(e => e.Index).UseIdentityColumn(1000, 1);
                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
                entity.Property(e => e.Email).IsRequired();
            });

            modelBuilder.Entity<Lecturer>(entity =>{
                entity.HasKey(e => e.EmployeeID);
                entity.Property(e => e.EmployeeID).UseIdentityColumn(1000, 1);
                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
                entity.Property(e => e.Email).IsRequired();
            });

            #region Course
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Lecturer)
                .WithMany(l => l.Courses)
                .HasForeignKey(c => c.LecturerId);

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).UseIdentityColumn(0, 1);
                entity.Property(e => e.Name).IsRequired();
            });
            #endregion

            #region CourseStudent
            modelBuilder.Entity<CourseStudent>().HasKey(cs => new {cs.CourseId, cs.StudentIndex });
            
            modelBuilder.Entity<CourseStudent>().Property(cs => cs.Grade);

            modelBuilder.Entity<CourseStudent>()
                .HasOne(cs => cs.Student)
                .WithMany(s => s.Courses)
                .HasForeignKey(cs => cs.StudentIndex);

            modelBuilder.Entity<CourseStudent>()
                .HasOne(cs => cs.Course)
                .WithMany(c => c.CourseStudents)
                .HasForeignKey(cs => cs.CourseId);
            #endregion
        }
    }
}

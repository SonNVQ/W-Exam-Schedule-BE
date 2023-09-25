using ExamScheduleSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace ExamScheduleSystem.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :base(options) 
        { 
                
        }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Course> Courses { get; set; }      
        public DbSet<ExamSchedule> ExamSchedules { get; set; }
        public DbSet<ExamSlot> ExamSlots { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<Proctoring> Proctorings { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<StudentList> StudentLists { get; set; }
        public DbSet<ClassroomExamSchedule> ClassroomExamSchedules { get; set; } 
        public DbSet<CourseStudentList> CourseStudentLists { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClassroomExamSchedule>()
                .HasKey(pc => new { pc.ExamScheduleId, pc.ClassroomId });
            modelBuilder.Entity<ClassroomExamSchedule>()
                .HasOne(p => p.ExamSchedule)
                .WithMany(pc => pc.ClassroomExamSchedules)
                .HasForeignKey(p => p.ExamScheduleId);
            modelBuilder.Entity<ClassroomExamSchedule>()
                .HasOne(p => p.Classroom)
                .WithMany(pc => pc.ClassroomExamSchedules)
                .HasForeignKey(c => c.ClassroomId);

            modelBuilder.Entity<CourseStudentList>()
                .HasKey(pc => new { pc.CourseId, pc.StudentListId });
            modelBuilder.Entity<CourseStudentList>()
                .HasOne(p => p.Course)
                .WithMany(pc => pc.CourseStudentLists)
                .HasForeignKey(p => p.CourseId);
            modelBuilder.Entity<CourseStudentList>()
                .HasOne(p => p.StudentList)
                .WithMany(pc => pc.CourseStudentLists)
                .HasForeignKey(c => c.StudentListId);
        }

    }
}

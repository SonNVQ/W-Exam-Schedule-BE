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
        public DbSet<CourseStudentList> CourseStudentLists { get; set; }
        public DbSet<ExamSlotExamSchedule> ExamSlotExamSchedules { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<ExamSlotExamSchedule>()
    .HasKey(pc => new { pc.ExamSlotId, pc.ExamScheduleId });
            modelBuilder.Entity<ExamSlotExamSchedule>()
                .HasOne(p => p.ExamSlot)
                .WithMany(pc => pc.ExamSlotExamSchedules)
                .HasForeignKey(p => p.ExamSlotId);
            modelBuilder.Entity<ExamSlotExamSchedule>()
                .HasOne(p => p.ExamSchedule)
                .WithMany(pc => pc.ExamSlotExamSchedules)
                .HasForeignKey(c => c.ExamScheduleId);
        }

    }
}

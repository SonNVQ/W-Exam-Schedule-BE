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
        public DbSet<ExamSlotProctoring> ExamSlotProctorings { get; set; }
        public DbSet<StudentListStudent> StudentListStudents { get; set; }
        public DbSet<SemesterMajor> SemesterMajors { get; set; }
        public DbSet<Student> Students { get; set; }
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

            modelBuilder.Entity<ExamSlotProctoring>()
                .HasKey(pc => new { pc.ExamSlotId, pc.ProctoringId });
            modelBuilder.Entity<ExamSlotProctoring>()
                .HasOne(p => p.ExamSlot)
                .WithMany(pc => pc.ExamSlotProctorings)
                .HasForeignKey(p => p.ExamSlotId);
            modelBuilder.Entity<ExamSlotProctoring>()
                .HasOne(p => p.Proctoring)
                .WithMany(pc => pc.ExamSlotProctorings)
                .HasForeignKey(c => c.ProctoringId);

            modelBuilder.Entity<StudentListStudent>()
                .HasKey(pc => new { pc.StudentListId, pc.Username });
            modelBuilder.Entity<StudentListStudent>()
                .HasOne(p => p.StudentList)
                .WithMany(pc => pc.StudentListStudents)
                .HasForeignKey(p => p.StudentListId);
            modelBuilder.Entity<StudentListStudent>()
                .HasOne(p => p.Student)
                .WithMany(pc => pc.StudentListStudents)
                .HasForeignKey(c => c.Username);

            modelBuilder.Entity<SemesterMajor>()
                .HasKey(pc => new { pc.SemesterId, pc.MajorId });
            modelBuilder.Entity<SemesterMajor>()
                .HasOne(p => p.Semester)
                .WithMany(pc => pc.SemesterMajors)
                .HasForeignKey(p => p.SemesterId);
            modelBuilder.Entity<SemesterMajor>()
                .HasOne(p => p.Major)
                .WithMany(pc => pc.SemesterMajors)
                .HasForeignKey(c => c.MajorId);
        }

    }
}

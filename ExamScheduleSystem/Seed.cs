using ExamScheduleSystem.Data;
using ExamScheduleSystem.Model;
using System.Diagnostics.Metrics;

namespace ExamScheduleSystem
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
             this.dataContext = context;
        }
        public void SeedDataContext()
        {
            if (!dataContext.ClassroomExamSchedules.Any())
            {
                var classroomExamSchedules = new List<ClassroomExamSchedule>()
                {
                    new ClassroomExamSchedule()
                    {
                        Classroom = new Classroom()
                        {
                            Name = "NVH615",
                            Capacity = 30,
                            ClassroomId = "CL-001",
                            ClassroomExamSchedules = new List<ClassroomExamSchedule>()
                            {
                                new ClassroomExamSchedule {ExamSchedule = new ExamSchedule() { ExamScheduleId = "ES-001"}}
                            },
                        },
                        ExamSchedule = new ExamSchedule()
                        {
                            ExamScheduleId = "T-001",
                            CourseId = "CS-001",
                            ExamSlotId = "ES-001",
                            ClassroomId = "CL-001",
                            Course = new Course()
                            {
                                CourseId = "CS-001"
                            }
                        }
                    }
                };
                dataContext.ClassroomExamSchedules.AddRange(classroomExamSchedules);
                dataContext.SaveChanges();
            }
        }
       

    }
}

namespace ExamScheduleSystem.Model
{
    public class Classroom
    {
        public string ClassroomId { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }

        public ICollection<ClassroomExamSchedule> ClassroomExamSchedules { get; set; }
    }
}

namespace ExamScheduleSystem.Model
{
    public class ClassroomExamSchedule
    {
        public string ClassroomExamScheduleId { get; set; }
        public string ClassroomId { get; set; }

        public string ExamScheduleId { get; set; } 

        public Classroom Classroom { get; set; }

        public ExamSchedule ExamSchedule { get; set; }  
    }
}

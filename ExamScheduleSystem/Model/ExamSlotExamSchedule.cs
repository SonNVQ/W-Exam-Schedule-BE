namespace ExamScheduleSystem.Model
{
    public class ExamSlotExamSchedule
    {
        public string ExamSlotId { get; set; }
        public string ExamScheduleId { get; set; }

        public string ExamSlotExamScheduleId { get; set; }
        public ExamSlot ExamSlot { get; set; }

        public ExamSchedule ExamSchedule { get; set; }
    }
}

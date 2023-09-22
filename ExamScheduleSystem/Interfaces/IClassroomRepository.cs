using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Interfaces
{
    public interface IClassroomRepository
    {
        public Task<List<Classroom>> GetAllClassroomsAsync();
        public Task<Classroom> GetClassroomAsync(string id);
        public Task<string> AddClassroomAsync(Classroom model);
        public Task UpdateClassroomAsync(string id, Classroom model);
        public Task DeleteClassroomAsync(string id);
    }
}

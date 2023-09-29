using ExamScheduleSystem.Model;

namespace ExamScheduleSystem.Interfaces
{
    public interface IClassroomRepository
    {
        ICollection<Classroom> GetClassrooms();

        Classroom GetClassroom(string id);


        bool ClassroomExists(string id);

        bool CreateClassroom(Classroom classroom);

        bool UpdateClassroom(Classroom classroom);

        bool DeleteClassroom(Classroom classroom);
        bool Save();

    }
}

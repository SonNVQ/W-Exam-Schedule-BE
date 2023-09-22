using AutoMapper;
using ExamScheduleSystem.Data;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace ExamScheduleSystem.Repositories
{
    public class ClassroomRepository : IClassroomRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ClassroomRepository(DataContext context, IMapper mapper  ) 
        { 
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> AddClassroomAsync(Classroom model)
        {
            var newClassroom = _mapper.Map<Classroom>(model);
            _context.Classrooms!.Add(newClassroom);
            await _context.SaveChangesAsync();

            return newClassroom.ClassroomId;
        }

        public async Task DeleteClassroomAsync(string id)
        {
            var deleteClassroom = _context.Classrooms!.SingleOrDefault(b => b.ClassroomId == id);
            if (deleteClassroom != null)
            {
                _context.Classrooms!.Remove(deleteClassroom);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Classroom>> GetAllClassroomsAsync()
        {
            var classrooms = await _context.Classrooms!.ToListAsync();
            return _mapper.Map<List<Classroom>>(classrooms);
        }

        public async Task<Classroom> GetClassroomAsync(string id)
        {
            var classroom = await _context.Classrooms!.FindAsync(id);
            return _mapper.Map<Classroom>(classroom);
        }

        public async Task UpdateClassroomAsync(string id, Classroom model)
        {
            if (id == model.ClassroomId)
            {
                var updateClassroom = _mapper.Map<Classroom>(model);
                _context.Classrooms!.Update(updateClassroom);
                await _context.SaveChangesAsync();
            }
        }
    }
}

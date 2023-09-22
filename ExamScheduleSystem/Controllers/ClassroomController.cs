using ExamScheduleSystem.Data;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace ExamScheduleSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassroomController : ControllerBase
    {
        private readonly IClassroomRepository _classroomRepository;
        private readonly DataContext context;

        public ClassroomController(IClassroomRepository classroomRepository, DataContext context)
        {
            _classroomRepository = classroomRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type =  typeof(IEnumerable<Classroom>))]
        public IActionResult GetClassrooms()
        {
            var classrooms = _classroomRepository.GetAllClassroomsAsync();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(classrooms);
        }

        // GET: api/Classroom/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Classroom>> GetClassroom(int id)
        {
            var classroom = await context.Classrooms!.FindAsync(id);

            if (classroom == null)
            {
                return NotFound();
            }

            return classroom;
        }

        // PUT: api/Classrooms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClassroom(string id, Classroom classroom)
        {
            if (id != classroom.ClassroomId)
            {
                return BadRequest();
            }

            context.Entry(classroom).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassroomExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Classrooms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Classroom>> PostClassroom(Classroom classroom)
        {
            context.Classrooms!.Add(classroom);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetClassroom", new { id = classroom.ClassroomId }, classroom);
        }

        // DELETE: api/Classrooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassroom(int id)
        {
            var classroom = await context.Classrooms!.FindAsync(id);
            if (classroom == null)
            {
                return NotFound();
            }

            context.Classrooms.Remove(classroom);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClassroomExists(string id)
        {
            return context.Classrooms!.Any(e => e.ClassroomId == id);
        }
    }
}

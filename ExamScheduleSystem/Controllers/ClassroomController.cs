using AutoMapper;
using ExamScheduleSystem.Data;
using ExamScheduleSystem.DTO;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;
using Microsoft.AspNetCore.Mvc;

namespace ExamScheduleSystem.Controllers
{    [Route("api/[controller]")]
    [ApiController]
    public class ClassroomController : Controller
    {
        private readonly IClassroomRepository _classroomRepository;
        private readonly IMapper _mapper;

        public ClassroomController(IClassroomRepository classroomRepository, IMapper mapper)
        {
            _classroomRepository = classroomRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type =  typeof(IEnumerable<Classroom>))]
        public IActionResult GetClassrooms()
        {
            var classrooms = _mapper.Map<List<ClassroomDTO>>(_classroomRepository.GetClassrooms());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(classrooms);
        }

        [HttpGet("{classroomId}")]
        [ProducesResponseType(200, Type=typeof(Classroom))]
        [ProducesResponseType(400)]
        public IActionResult GetClassroom(string classroomId)
        {
            if (!_classroomRepository.ClassroomExists(classroomId))
                return NotFound();
            var classroom = _mapper.Map<ClassroomDTO>(_classroomRepository.GetClassroom(classroomId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(classroom);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateClassroom([FromBody] ClassroomDTO classroomCreate)
        {
            if (classroomCreate == null)
                return BadRequest(ModelState);

            var classroom = _classroomRepository.GetClassrooms()
                .Where(c => c.Name.Trim().ToUpper() == classroomCreate.Name.Trim().ToUpper())
                .FirstOrDefault();

            if (classroom != null)
            {
                ModelState.AddModelError("", "Classroom already existt!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var classroomMap = _mapper.Map<Classroom>(classroomCreate);

            if (!_classroomRepository.CreateClassroom(classroomMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("successfully created!");
        }

        [HttpPut("{classroomId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateClassroom(string classroomId, [FromBody] ClassroomDTO updatedClassroom) 
        {
            if (updatedClassroom == null)
                return BadRequest(ModelState);

            if (classroomId != updatedClassroom.ClassroomId)
                return BadRequest(ModelState);

            if (!_classroomRepository.ClassroomExists(classroomId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var classroomMap = _mapper.Map<Classroom>(updatedClassroom);

            if (!_classroomRepository.UpdateClassroom(classroomMap))
            {
                ModelState.AddModelError("", "Something went wrong updating classroom");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{classroomId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteClassroom(string classroomId)
        {
            if (!_classroomRepository.ClassroomExists(classroomId))
            {
                return NotFound();
            }

            var classroomToDelete = _classroomRepository.GetClassroom(classroomId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_classroomRepository.DeleteClassroom(classroomToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting classroom");
            }

            return NoContent();
        }


    }

}

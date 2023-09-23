using AutoMapper;
using ExamScheduleSystem.DTO;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;
using ExamScheduleSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExamScheduleSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentListController : Controller
    {
        private readonly IStudentListRepository _studentListRepository;
        private readonly IMapper _mapper;

        public StudentListController(IStudentListRepository studentListRepository, IMapper mapper)
        {
            _studentListRepository = studentListRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<StudentList>))]
        public IActionResult GetStudentLists()
        {
            var studentLists = _mapper.Map<List<StudentListDTO>>(_studentListRepository.GetStudentLists());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(studentLists);
        }

        [HttpGet("{studentListId}")]
        [ProducesResponseType(200, Type = typeof(StudentList))]
        [ProducesResponseType(400)]
        public IActionResult GetStudentList(string studentListId)
        {
            if (!_studentListRepository.StudentListExists(studentListId))
                return NotFound();
            var studentList = _mapper.Map<StudentListDTO>(_studentListRepository.GetStudentList(studentListId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(studentList);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateStudentList([FromBody] StudentListDTO studentListCreate)
        {
            if (studentListCreate == null)
                return BadRequest(ModelState);

            var studentList = _studentListRepository.GetStudentLists()
                .Where(c => c.StudentListId.Trim().ToUpper() == studentListCreate.StudentListId.Trim().ToUpper())
                .FirstOrDefault();

            if (studentList != null)
            {
                ModelState.AddModelError("", "StudentList already existt!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var studentListMap = _mapper.Map<StudentList>(studentListCreate);

            if (!_studentListRepository.CreateStudentList(studentListMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("successfully created!");
        }

        [HttpPut("{studentListId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateStudentList(string studentListId, [FromBody] StudentListDTO updatedStudentList)
        {
            if (updatedStudentList == null)
                return BadRequest(ModelState);

            if (studentListId != updatedStudentList.StudentListId)
                return BadRequest(ModelState);

            if (!_studentListRepository.StudentListExists(studentListId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var studentListMap = _mapper.Map<StudentList>(updatedStudentList);

            if (!_studentListRepository.UpdateStudentList(studentListMap))
            {
                ModelState.AddModelError("", "Something went wrong updating studentList");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{studentListId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteStudentList(string studentListId)
        {
            if (!_studentListRepository.StudentListExists(studentListId))
            {
                return NotFound();
            }

            var studentListToDelete = _studentListRepository.GetStudentList(studentListId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_studentListRepository.DeleteStudentList(studentListToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting studentList");
            }

            return NoContent();
        }


    }
}

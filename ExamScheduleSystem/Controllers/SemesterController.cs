using AutoMapper;
using ExamScheduleSystem.DTO;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;
using ExamScheduleSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamScheduleSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  //  [Authorize]
    public class SemesterController : Controller
    {
        private readonly ISemesterRepository _semesterRepository;
        private readonly IMapper _mapper;

        public SemesterController(ISemesterRepository semesterRepository, IMapper mapper)
        {
            _semesterRepository = semesterRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Semester>))]
        public IActionResult GetSemesters()
        {
            var semesters = _mapper.Map<List<SemesterDTO>>(_semesterRepository.GetSemesters());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(semesters);
        }

        [HttpGet("{semesterId}")]
        [ProducesResponseType(200, Type = typeof(Semester))]
        [ProducesResponseType(400)]
        public IActionResult GetSemester(string semesterId)
        {
            if (!_semesterRepository.SemesterExists(semesterId))
                return NotFound();
            var semester = _mapper.Map<SemesterDTO>(_semesterRepository.GetSemester(semesterId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(semester);
        }

        [HttpPost]
   //     [Authorize(Roles = "AD,TA")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateSemester([FromBody] SemesterDTO semesterCreate)
        {
            if (semesterCreate == null)
                return BadRequest(ModelState);

            var semester = _semesterRepository.GetSemesters()
                .Where(c => c.SemesterName.Trim().ToUpper() == semesterCreate.SemesterName.Trim().ToUpper())
                .FirstOrDefault();

            if (semester != null)
            {
                ModelState.AddModelError("", "Semester already existt!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var semesterMap = _mapper.Map<Semester>(semesterCreate);

            if (!_semesterRepository.CreateSemester(semesterMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("successfully created!");
        }

        [HttpPut("{semesterId}")]
 //       [Authorize(Roles = "AD,TA")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateSemester(string semesterId, [FromBody] SemesterDTO updatedSemester)
        {
            if (updatedSemester == null)
                return BadRequest(ModelState);

            if (semesterId != updatedSemester.SemesterId)
                return BadRequest(ModelState);

            if (!_semesterRepository.SemesterExists(semesterId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var semesterMap = _mapper.Map<Semester>(updatedSemester);

            if (!_semesterRepository.UpdateSemester(semesterMap))
            {
                ModelState.AddModelError("", "Something went wrong updating semester");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{semesterId}")]
        //[Authorize(Roles = "AD,TA")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteSemester(string semesterId)
        {
            if (!_semesterRepository.SemesterExists(semesterId))
            {
                return NotFound();
            }

            var semesterToDelete = _semesterRepository.GetSemester(semesterId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_semesterRepository.DeleteSemester(semesterToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting semester");
            }

            return NoContent();
        }
    }
}

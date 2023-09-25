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
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public CourseController(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Course>))]
        public IActionResult GetCourses()
        {
            var courses = _mapper.Map<List<CourseDTO>>(_courseRepository.GetCourses());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(courses);
        }

        [HttpGet("{courseId}")]
        [ProducesResponseType(200, Type = typeof(Course))]
        [ProducesResponseType(400)]
        public IActionResult GetCourse(string courseId)
        {
            if (!_courseRepository.CourseExists(courseId))
                return NotFound();
            var course = _mapper.Map<CourseDTO>(_courseRepository.GetCourse(courseId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(course);
        }

        [HttpPost]
        [Authorize(Roles = "AD,TA")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCourse([FromBody] CourseDTO courseCreate)
        {
            if (courseCreate == null)
                return BadRequest(ModelState);

            var course = _courseRepository.GetCourses()
                .Where(c => c.CourseName.Trim().ToUpper() == courseCreate.CourseName.Trim().ToUpper())
                .FirstOrDefault();

            if (course != null)
            {
                ModelState.AddModelError("", "Course already existt!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var courseMap = _mapper.Map<Course>(courseCreate);

            if (!_courseRepository.CreateCourse(courseMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("successfully created!");
        }

        [HttpPut("{courseId}")]
        [Authorize(Roles = "AD,TA")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCourse(string courseId, [FromBody] CourseDTO updatedCourse)
        {
            if (updatedCourse == null)
                return BadRequest(ModelState);

            if (courseId != updatedCourse.CourseId)
                return BadRequest(ModelState);

            if (!_courseRepository.CourseExists(courseId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var courseMap = _mapper.Map<Course>(updatedCourse);

            if (!_courseRepository.UpdateCourse(courseMap))
            {
                ModelState.AddModelError("", "Something went wrong updating course");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{courseId}")]
        [Authorize(Roles = "AD,TA")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCourse(string courseId)
        {
            if (!_courseRepository.CourseExists(courseId))
            {
                return NotFound();
            }

            var courseToDelete = _courseRepository.GetCourse(courseId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_courseRepository.DeleteCourse(courseToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting course");
            }

            return NoContent();
        }


    }

}
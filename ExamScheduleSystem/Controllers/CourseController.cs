using AutoMapper;
using ExamScheduleSystem.DTO;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;
using ExamScheduleSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace ExamScheduleSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  //  [Authorize]
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentListRepository _studentListRepository;
        private readonly ICourseStudentListRepository _courseStudentListRepository;
        private readonly IMapper _mapper;

        public CourseController(ICourseRepository courseRepository, IMapper mapper, IStudentListRepository studentListRepository, ICourseStudentListRepository courseStudentListRepository)
        {
            _courseRepository = courseRepository;
            _studentListRepository = studentListRepository;
            _courseStudentListRepository = courseStudentListRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PaginationCourseDTO>))]
        public IActionResult GetCourses([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyword = "", [FromQuery] string? sortBy = "", [FromQuery] bool isAscending = true)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Invalid page or pageSize parameters.");
            }

            var allCourses = _courseRepository.GetCourses();
            foreach (var course in allCourses) { }

            IEnumerable<Course> filteredallCourses = allCourses ?? Enumerable.Empty<Course>();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                filteredallCourses = filteredallCourses.Where(course =>
                    (course.CourseId != null && course.CourseId.ToUpper().Contains(keyword.ToUpper())) ||
                    (course.CourseStudentLists != null) ||
                    (course.CourseName != null && course.CourseName.ToUpper().Contains(keyword.ToUpper())))
                    .ToList(); // Add .ToList() to the query to prevent null reference exceptions
            }
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy)
                {
                    case "courseId":
                        filteredallCourses = isAscending
                            ? filteredallCourses.OrderBy(course => course.CourseId)
                            : filteredallCourses.OrderByDescending(course => course.CourseId);
                        break;
                    case "courseName":
                        filteredallCourses = isAscending
                            ? filteredallCourses.OrderBy(course => course.CourseName)
                            : filteredallCourses.OrderByDescending(course => course.CourseName);
                        break;
                    case "semesterId":
                        filteredallCourses = isAscending
                            ? filteredallCourses.OrderBy(course => course.SemesterId)
                            : filteredallCourses.OrderByDescending(course => course.SemesterId);
                        break;
                }
            }


            int totalCount = filteredallCourses.Count();

            var pagedCourses = filteredallCourses
       .Skip((page - 1) * pageSize)
       .Take(pageSize)
       .Select(course => new CourseDTO
       {
           CourseId = course.CourseId,
           CourseName = course.CourseName,
           SemesterId = course.SemesterId,
           Status = course.Status,
           listStudentList = _studentListRepository.GetStudentListsByCourseId(course.CourseId)
               .Select(studentList => new StudentListDTO
               {
                   StudentListId = studentList.StudentListId,
                   CourseId = studentList.CourseId,
                   NumberOfProctoring = studentList.NumberOfProctoring,
                   Status = studentList.Status,
                   listStudent = _studentListRepository.GetStudentsByStudentListId(studentList.StudentListId)
                        .Select(student => new StudentDTO
                        {
                            Username = student.Username,
                            Email = student.Email
                        })
                        .ToList()
               })
               .ToList()
       })
       .ToList();

            var pagination = new Pagination
            {
                currentPage = page,
                pageSize = pageSize,
                totalPage = Convert.ToInt32(Math.Ceiling((double)totalCount / pageSize))
            };


            if (pagedCourses.Any())
            {
                PaginatedCourse<CourseDTO> paginatedResult = new PaginatedCourse<CourseDTO>
                {
                    Data = pagedCourses,
                    Pagination = pagination
                };

                return Ok(paginatedResult);
            }
            else
            {
                return NotFound("No matching courses found.");
            }
        }

        [HttpGet("{course}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetCourse(string courseId)
        {
            // Use your data access layer to retrieve studentLists associated with the given course
            var studentLists = _courseRepository.GetStudentListsByCourseId(courseId);
            var courses = _courseRepository.GetCourse(courseId);
            var students = _studentListRepository.GetStudentsByStudentListId(courseId);
            if (studentLists == null)
            {
                return NotFound("Course not found");
            }

            // Construct the response object
            var response = new
            {
                CourseId = courseId,
                CourseName = courses.CourseId,
                Status = courses.Status,
                SemesterId = courses.SemesterId,
                listStudentList = studentLists.Select(studentList => new
                {
                    studentListId = studentList.StudentListId,
                    listStudent = students.Select(student => new
                    {
                        username = student.Username,
                        email = student.Email
                    }
                ).ToList(),
                }
                ).ToList(),
            };

            return Ok(response);
        }

        [HttpPost]
      //  [Authorize(Roles = "AD,TA")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCourse([FromBody] CourseDTO request)
        {
            if (request == null)
                return BadRequest("Invalid JSON data.");

            var existingCourse = _courseRepository.CourseExists(request.CourseId);
            if (!existingCourse)
            {
                var course = new Course
                {
                    CourseId = request.CourseId,
                    CourseName = request.CourseName,
                    SemesterId = request.SemesterId,
                    Status = request.Status,
                    CourseStudentLists = new List<CourseStudentList>()
                };
                _courseRepository.CreateCourse(course);
            }
            try
            {
                foreach (var studentList in request.listStudentList)
                {
                    var CourseStudentList = new CourseStudentList
                    {
                        CourseId = request.CourseId,
                        StudentListId = studentList.StudentListId
                    };
                    _courseStudentListRepository.AddCourseStudentList(CourseStudentList);
                }
            }

            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
                return StatusCode(500, $"Error: {ex.Message}");
            }
            return Ok("Successfully");
        }

        [HttpPut("{courseId}")]
   //     [Authorize(Roles = "AD,TA")]
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

            var newCourse = new Course
            {
                CourseId = updatedCourse.CourseId,
                CourseName = updatedCourse.CourseName,
                SemesterId = updatedCourse.SemesterId,
                Status = updatedCourse.Status
            };
            var courseMap = _mapper.Map<Course>(newCourse);
            var newListStudentList = new List<CourseStudentList>();
            foreach (var studentList in updatedCourse.listStudentList)
            {
                var CourseStudentList = new CourseStudentList
                {
                    CourseId = updatedCourse.CourseId,
                    StudentListId = studentList.StudentListId
                };
                newListStudentList.Add(CourseStudentList);
            }
            _courseStudentListRepository.UpdateCourseStudentList(updatedCourse.CourseId, newListStudentList);
            if (!_courseRepository.UpdateCourse(courseMap))
            {
                ModelState.AddModelError("", "Something went wrong updating course");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{courseId}")]
    //    [Authorize(Roles = "AD,TA")]
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
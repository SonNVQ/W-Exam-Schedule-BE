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
    //[Authorize]
    public class StudentListController : Controller
    {
        private readonly IStudentListRepository _studentListRepository;
        private readonly IStudentListStudentRepository _studentListStudentRepository;
        private readonly IMapper _mapper;

        public StudentListController(IStudentListRepository studentListRepository, IMapper mapper, IStudentListStudentRepository studentListStudentRepository)
        {
            _studentListRepository = studentListRepository;
            _mapper = mapper;
            _studentListStudentRepository = studentListStudentRepository;
        }

        [HttpGet("{studentListId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetStudentList(string studentListId)
        {
            // Use your data access layer to retrieve students associated with the given student list
            var students = _studentListRepository.GetStudentsByStudentListId(studentListId);
            var studentLists = _studentListRepository.GetStudentList(studentListId);
            if (students == null)
            {
                return NotFound("Student List not found");
            }

            // Construct the response object
            var response = new
            {
                studentListId = studentListId,
                CourseId = studentLists.CourseId,
                Status = studentLists.Status,
                listStudent = students.Select(student => new
                {
                    username = student.Username,
                    email = student.Email
                }
                ).ToList(),
            };

            return Ok(response);
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PaginationStudentListDTO>))]
        public IActionResult GetStudentLists([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyword = "", [FromQuery] string? sortBy = "", [FromQuery] bool isAscending = true)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Invalid page or pageSize parameters.");
            }

            var allStudentLists = _studentListRepository.GetStudentLists();

            foreach (var studentList in allStudentLists) { }
            //StudentListStudent StudentList_Student = _studentListRepository.getStudentListStudent();
            IEnumerable<StudentList> filteredallStudentLists = allStudentLists ?? Enumerable.Empty<StudentList>();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                filteredallStudentLists = filteredallStudentLists.Where(studentList =>
                    (studentList.StudentListId != null && studentList.StudentListId.ToUpper().Contains(keyword.ToUpper())) ||
                    (studentList.StudentListStudents != null) ||
                    (studentList.CourseId != null && studentList.CourseId.ToUpper().Contains(keyword.ToUpper())))
                    .ToList(); // Add .ToList() to the query to prevent null reference exceptions
            }

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy)
                {
                    case "studentListId":
                        filteredallStudentLists = isAscending
                            ? filteredallStudentLists.OrderBy(studentList => studentList.StudentListId)
                            : filteredallStudentLists.OrderByDescending(studentList => studentList.StudentListId);
                        break;
                    case "courseId":
                        filteredallStudentLists = isAscending
                            ? filteredallStudentLists.OrderBy(studentList => studentList.CourseId)
                            : filteredallStudentLists.OrderByDescending(studentList => studentList.CourseId);
                        break;
                }
            }

            int totalCount = filteredallStudentLists.Count();

            var pagedStudentLists = filteredallStudentLists
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(studentList => new StudentListDTO
                {
                    StudentListId = studentList.StudentListId,
                    listStudent = _studentListRepository.GetStudentsByStudentListId(studentList.StudentListId)
                        .Select(student => new StudentDTO
                        {
                            Username = student.Username,
                            Email = student.Email
                        })
                        .ToList(),
                    CourseId = studentList.CourseId,
                    Status = studentList.Status
                })
                .ToList();


            var pagination = new Pagination
            {
                currentPage = page,
                pageSize = pageSize,
                totalPage = Convert.ToInt32(Math.Ceiling((double)totalCount / pageSize))
            };

            if (pagedStudentLists.Any())
            {
                PaginatedStudentList<StudentListDTO> paginatedResult = new PaginatedStudentList<StudentListDTO>
                {
                    Data = pagedStudentLists,
                    Pagination = pagination
                };

                return Ok(paginatedResult);
            }
            else
            {
                return NotFound("No matching student lists found.");
            }
        }

        //[HttpPost]
        ////[Authorize(Roles = "AD,TA")]
        //[ProducesResponseType(204)]
        //[ProducesResponseType(400)]
        //public IActionResult CreateStudentList([FromBody] StudentListDTO studentListCreate)
        //{
        //    if (studentListCreate == null)
        //        return BadRequest(ModelState);

        //    var studentList = _studentListRepository.GetStudentLists()
        //        .Where(c => c.StudentListId.Trim().ToUpper() == studentListCreate.StudentListId.Trim().ToUpper())
        //        .FirstOrDefault();

        //    if (studentList != null)
        //    {
        //        ModelState.AddModelError("", "StudentList already existt!");
        //        return StatusCode(422, ModelState);
        //    }

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var studentListMap = _mapper.Map<StudentList>(studentListCreate);

        //    if (!_studentListRepository.CreateStudentList(studentListMap))
        //    {
        //        ModelState.AddModelError("", "Something went wrong while saving");
        //        return StatusCode(500, ModelState);
        //    }

        //    return Ok("successfully created!");
        //}


        [HttpPost]
        //[Authorize(Roles = "AD,TA")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateStudentList([FromBody] StudentListDTO request)
        {
            if (request == null)
                return BadRequest("Invalid JSON data.");
            var studentList = new StudentList
            {
                StudentListId = request.StudentListId,
                CourseId = request.CourseId,
                Status = request.Status,
                StudentListStudents = new List<StudentListStudent>()
            };
            _studentListRepository.CreateStudentList(studentList);

            try
            {
                foreach (var student in request.listStudent)
                {
                    var StudentListStudent = new StudentListStudent
                    {
                        StudentListId = request.StudentListId,
                        Username = student.Username
                    };
                    _studentListStudentRepository.AddStudentListStudent(StudentListStudent);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        [HttpPut("{studentListId}")]
        //[Authorize(Roles = "AD,TA")]
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

            var newStudentList = new StudentList
            {
                CourseId = updatedStudentList.CourseId,
                StudentListId = updatedStudentList.StudentListId,
                Status = updatedStudentList.Status
            };
            var studentListMap = _mapper.Map<StudentList>(newStudentList);
            var newListStudent = new List<StudentListStudent>();
            foreach (var student in updatedStudentList.listStudent)
            {
                var StudentListStudent = new StudentListStudent
                {
                    StudentListId = updatedStudentList.StudentListId,
                    Username = student.Username
                };
                newListStudent.Add(StudentListStudent);
            }

            _studentListStudentRepository.UpdateStudentListStudent(updatedStudentList.StudentListId, newListStudent);
            if (!_studentListRepository.UpdateStudentList(studentListMap))
            {
                ModelState.AddModelError("", "Something went wrong updating studentList");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }


        [HttpDelete("{studentListId}")]
        //[Authorize(Roles = "AD,TA")]
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

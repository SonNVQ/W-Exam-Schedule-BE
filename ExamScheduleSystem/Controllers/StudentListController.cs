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
        private readonly IMapper _mapper;

        public StudentListController(IStudentListRepository studentListRepository, IMapper mapper)
        {
            _studentListRepository = studentListRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PaginationStudentListDTO>))]
        /*public IActionResult GetStudentLists()
        {
            var studentLists = _mapper.Map<List<StudentListDTO>>(_studentListRepository.GetStudentLists());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(studentLists);
        }*/
        public IActionResult GetStudentLists([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyword = "", [FromQuery] string? sortBy = "", [FromQuery] bool isAscending = true)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Invalid page or pageSize parameters.");
            }

            var allStudentLists = _studentListRepository.GetStudentLists();
            IEnumerable<StudentList> filteredallStudentLists = allStudentLists;
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                filteredallStudentLists = allStudentLists.Where(studentList =>
                    studentList.StudentListId.ToUpper().Contains(keyword.ToUpper()) ||
                    //studentList.ListStudent.Any(student => student.ToUpper().Contains(keyword.ToUpper())) ||
                    studentList.CourseId.ToUpper().Contains(keyword.ToUpper())
                );
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
                .Select(c => _mapper.Map<PaginationStudentListDTO>(c))
                .ToList();

            var pagination = new Pagination
            {
                currentPage = page,
                pageSize = pageSize,
                totalPage = Convert.ToInt32(Math.Ceiling((double)totalCount / pageSize))
            };


            PaginatedStudentList<StudentList> paginatedResult = new PaginatedStudentList<StudentList>
            {
                Data = pagedStudentLists,
                Pagination = pagination
            };

            return Ok(paginatedResult);
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

            var students = new List<Student>();
            foreach (var studentData in request.listStudent)
            {
                var student = new Student
                {
                    Username = studentData.Username,
                    Email = studentData.Email,
                    StudentListStudents = new List<StudentListStudent>()
                };
                students.Add(student);

                var studentListStudent = new StudentListStudent
                {
                    Student = student,
                    StudentList = studentList
                };
                studentList.StudentListStudents.Add(studentListStudent);
            }
            try
            {
                _studentListRepository.CreateStudentList(studentList); 
                return NoContent();
            }
            catch (Exception ex)
            {
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

            var studentListMap = _mapper.Map<StudentList>(updatedStudentList);

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

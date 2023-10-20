using AutoMapper;
using ExamScheduleSystem.DTO;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;
using ExamScheduleSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace ExamScheduleSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  //  [Authorize]
    public class SemesterController : Controller
    {
        private readonly ISemesterRepository _semesterRepository;
        private readonly ISemesterMajorRepository _semesterMajorRepository;
        private readonly IMapper _mapper;

        public SemesterController(ISemesterRepository semesterRepository, IMapper mapper, ISemesterMajorRepository semesterMajorRepository)
        {
            _semesterRepository = semesterRepository;
            _mapper = mapper;
            _semesterMajorRepository = semesterMajorRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PaginationSemesterDTO>))]
        public IActionResult GetSemesters([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyword = "", [FromQuery] string? sortBy = "", [FromQuery] bool isAscending = true)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Invalid page or pageSize parameters.");
            }
            var allSemesters = _semesterRepository.GetSemesters();

            foreach (var semester in allSemesters) { }

            IEnumerable<Semester> filteredallSemesters = allSemesters ?? Enumerable.Empty<Semester>();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                filteredallSemesters = filteredallSemesters.Where(semester =>
                    (semester.SemesterId != null && semester.SemesterId.ToUpper().Contains(keyword.ToUpper())) ||
                    (semester.SemesterMajors != null) ||
                    (semester.SemesterName != null && semester.SemesterName.ToUpper().Contains(keyword.ToUpper())))
                    .ToList(); // Add .ToList() to the query to prevent null reference exceptions
            }
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy)
                {
                    case "semesterId":
                        filteredallSemesters = isAscending
                            ? filteredallSemesters.OrderBy(semester => semester.SemesterId)
                            : filteredallSemesters.OrderByDescending(semester => semester.SemesterId);
                        break;
                    case "semesterName":
                        filteredallSemesters = isAscending
                            ? filteredallSemesters.OrderBy(semester => semester.SemesterName)
                            : filteredallSemesters.OrderByDescending(semester => semester.SemesterName);
                        break;                   
                }
            }
            int totalCount = filteredallSemesters.Count();

            var pagedSemesters = filteredallSemesters
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(semester => new SemesterDTO
                {
                    SemesterId = semester.SemesterId,
                    SemesterName = semester.SemesterName,
                    listMajor = _semesterRepository.GetMajorsBySemesterId(semester.SemesterId)
                        .Select(major => new MajorDTO
                        {
                            MajorId = major.MajorId,
                            MajorName = major.MajorName,
                            Status = major.Status
                        }
                        ).ToList(),
                    Status = semester.Status
                }
                ).ToList();

            var pagination = new Pagination
            {
                currentPage = page,
                pageSize = pageSize,
                totalPage = Convert.ToInt32(Math.Ceiling((double)totalCount / pageSize))
            };


            if (pagedSemesters.Any())
            {
                PaginatedSemester<SemesterDTO> paginatedResult = new PaginatedSemester<SemesterDTO>
                {
                    Data = pagedSemesters,
                    Pagination = pagination
                };

                return Ok(paginatedResult);
            }
            else
            {
                return NotFound("No matching major lists found.");
            }
        }



        [HttpGet("{semesterId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetSemester(string semesterId)
        {
            var majors = _semesterRepository.GetMajorsBySemesterId(semesterId);
            var semesters = _semesterRepository.GetSemester(semesterId);
            if (majors == null)
            {
                return NotFound("Major List not found");
            }
            var response = new
            {
                semesterId = semesterId,
                SemesterName = semesters.SemesterName,
                Status = semesters.Status,
                listMajor = majors.Select(major => new
                {
                    majorId = major.MajorId,
                    majorName = major.MajorName,
                    status = major.Status
                }
                ).ToList(),
            };

            return Ok(response);
        }

        [HttpPost]
   //     [Authorize(Roles = "AD,TA")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateSemester([FromBody] SemesterDTO request)
        {
            if (request == null)
                return BadRequest(ModelState);

            var existingSemester = _semesterRepository.SemesterExists(request.SemesterId);
            if (!existingSemester)
            {
                var semester = new Semester
                {
                    SemesterId = request.SemesterId,
                    SemesterName = request.SemesterName,
                    Status = request.Status,
                    SemesterMajors = new List<SemesterMajor>()
                };
                _semesterRepository.CreateSemester(semester);
            }

            try
            {
                foreach (var major in request.listMajor)
                {
                    var SemesterMajor = new SemesterMajor
                    {
                        SemesterId = request.SemesterId,
                        MajorId = major.MajorId
                    };
                    _semesterMajorRepository.AddSemesterMajor(SemesterMajor);
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

            var newSemester = new Semester
            {
                SemesterId = updatedSemester.SemesterId,
                SemesterName = updatedSemester.SemesterName,
                Status = updatedSemester.Status
            };
            var semesterMap = _mapper.Map<Semester>(newSemester);
            var newListMajor = new List<SemesterMajor>();
            foreach (var major in updatedSemester.listMajor)
            {
                var SemesterMajor = new SemesterMajor
                {
                    SemesterId = updatedSemester.SemesterId,
                    MajorId = major.MajorId
                };
                newListMajor.Add(SemesterMajor);
            }

            _semesterMajorRepository.UpdateSemesterMajor(updatedSemester.SemesterId, newListMajor);
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

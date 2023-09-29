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
        private readonly IMapper _mapper;

        public SemesterController(ISemesterRepository semesterRepository, IMapper mapper)
        {
            _semesterRepository = semesterRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PaginationSemesterDTO>))]
        /*public IActionResult GetSemesters()
        {
            var semesters = _mapper.Map<List<SemesterDTO>>(_semesterRepository.GetSemesters());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(semesters);
        }*/
        public IActionResult GetSemesters([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyword = "", [FromQuery] string? sortBy = "", [FromQuery] bool isAscending = true)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Invalid page or pageSize parameters.");
            }

            var allSemesters = _semesterRepository.GetSemesters();
            IEnumerable<Semester> filteredAllSemesters = allSemesters;
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                filteredAllSemesters = allSemesters.Where(semester =>
            semester.SemesterId.ToUpper().Contains(keyword.ToUpper()) ||
            semester.SemesterName.ToUpper().Contains(keyword.ToUpper()) ||
            semester.MajorId.ToUpper().Contains(keyword.ToUpper())

                );
            }
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy)
                {
                    case "semesterId":
                        filteredAllSemesters = isAscending
                            ? filteredAllSemesters.OrderBy(semester => semester.SemesterId)
                            : filteredAllSemesters.OrderByDescending(semester => semester.SemesterId);
                        break;
                    case "semesterName":
                        filteredAllSemesters = isAscending
                            ? filteredAllSemesters.OrderBy(semester => semester.SemesterName)
                            : filteredAllSemesters.OrderByDescending(semester => semester.SemesterName);
                        break;
                    case "majorId":
                        filteredAllSemesters = isAscending
                            ? filteredAllSemesters.OrderBy(semester => semester.MajorId)
                            : filteredAllSemesters.OrderByDescending(semester => semester.MajorId);
                        break;
                      
                }
            }
            int totalCount = filteredAllSemesters.Count();
            var pagedSemesters = filteredAllSemesters
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => _mapper.Map<PaginationSemesterDTO>(c))
                .ToList();

            var pagination = new Pagination
            {
                currentPage = page,
                pageSize = pageSize,
                totalPage = Convert.ToInt32(Math.Ceiling((double)totalCount / pageSize))
            };


            PaginatedSemester<Semester> paginatedResult = new PaginatedSemester<Semester>
            {
                Data = pagedSemesters,
                Pagination = pagination
            };

            return Ok(paginatedResult);
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

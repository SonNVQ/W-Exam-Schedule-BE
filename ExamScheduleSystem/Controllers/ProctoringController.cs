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
    public class ProctoringController : Controller
    {
        private readonly IProctoringRepository _proctoringRepository;
        private readonly IMapper _mapper;

        public ProctoringController(IProctoringRepository proctoringRepository, IMapper mapper)
        {
            _proctoringRepository = proctoringRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PaginationProctoringDTO>))]
        /*public IActionResult GetProctorings()
        {
            var proctorings = _mapper.Map<List<ProctoringDTO>>(_proctoringRepository.GetProctorings());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(proctorings);
        }*/
        public IActionResult GetProctorings([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyword = "", [FromQuery] string? sortBy = "", [FromQuery] bool isAscending = true)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Invalid page or pageSize parameters.");
            }

            var allProctorings = _proctoringRepository.GetProctorings();
            IEnumerable<Proctoring> filteredallProctorings = allProctorings;
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                filteredallProctorings = allProctorings.Where(proctoring =>
                   proctoring.ProctoringId.ToUpper().Contains(keyword.ToUpper()) ||
                   proctoring.ProctoringName.ToUpper().Contains(keyword.ToUpper()) ||
                   proctoring.ProctoringLocation.ToUpper().Contains(keyword.ToUpper()) ||
                   proctoring.Compensation.ToUpper().Contains(keyword.ToUpper())
               );
            }
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy)
                {
                    case "proctoringId":
                        filteredallProctorings = isAscending
                            ? filteredallProctorings.OrderBy(proctoring => proctoring.ProctoringId)
                            : filteredallProctorings.OrderByDescending(proctoring => proctoring.ProctoringId);
                        break;
                    case "proctoringName":
                        filteredallProctorings = isAscending
                            ? filteredallProctorings.OrderBy(proctoring => proctoring.ProctoringName)
                            : filteredallProctorings.OrderByDescending(proctoring => proctoring.ProctoringName);
                        break;
                    case "proctoringLocation":
                        filteredallProctorings = isAscending
                            ? filteredallProctorings.OrderBy(proctoring => proctoring.ProctoringLocation)
                            : filteredallProctorings.OrderByDescending(proctoring => proctoring.ProctoringLocation);
                        break;
                    case "compensation":
                        filteredallProctorings = isAscending
                            ? filteredallProctorings.OrderBy(proctoring => proctoring.Compensation)
                            : filteredallProctorings.OrderByDescending(proctoring => proctoring.Compensation);
                        break;
                }
            }
            int totalCount = filteredallProctorings.Count();
            var pagedProctorings= filteredallProctorings
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => _mapper.Map<PaginationProctoringDTO>(c))
                .ToList();

            var pagination = new Pagination
            {
                currentPage = page,
                pageSize = pageSize,
                totalPage = Convert.ToInt32(Math.Ceiling((double)totalCount / pageSize))
            };


            PaginatedProctoring<Proctoring> paginatedResult = new PaginatedProctoring<Proctoring>
            {
                Data = pagedProctorings,
                Pagination = pagination
            };

            return Ok(paginatedResult);
        }

        [HttpGet("{proctoringId}")]
        [ProducesResponseType(200, Type = typeof(Proctoring))]
        [ProducesResponseType(400)]
        public IActionResult GetProctoring(string proctoringId)
        {
            if (!_proctoringRepository.ProctoringExists(proctoringId))
                return NotFound();
            var proctoring = _mapper.Map<ProctoringDTO>(_proctoringRepository.GetProctoring(proctoringId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(proctoring);
        }

        [HttpPost]
     //   [Authorize(Roles = "AD,TA,LT")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateProctoring([FromBody] ProctoringDTO proctoringCreate)
        {
            if (proctoringCreate == null)
                return BadRequest(ModelState);

            var proctoring = _proctoringRepository.GetProctorings()
                .Where(c => c.ProctoringName.Trim().ToUpper() == proctoringCreate.ProctoringName.Trim().ToUpper())
                .FirstOrDefault();

            if (proctoring != null)
            {
                ModelState.AddModelError("", "Proctoring already existt!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var proctoringMap = _mapper.Map<Proctoring>(proctoringCreate);

            if (!_proctoringRepository.CreateProctoring(proctoringMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("successfully created!");
        }

        [HttpPut("{proctoringId}")]
   //     [Authorize(Roles = "AD,TA,LT")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProctoring(string proctoringId, [FromBody] ProctoringDTO updatedProctoring)
        {
            if (updatedProctoring == null)
                return BadRequest(ModelState);

            if (proctoringId != updatedProctoring.ProctoringId)
                return BadRequest(ModelState);

            if (!_proctoringRepository.ProctoringExists(proctoringId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var proctoringMap = _mapper.Map<Proctoring>(updatedProctoring);

            if (!_proctoringRepository.UpdateProctoring(proctoringMap))
            {
                ModelState.AddModelError("", "Something went wrong updating proctoring");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{proctoringId}")]
    //    [Authorize(Roles = "AD,TA,LT")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProctoring(string proctoringId)
        {
            if (!_proctoringRepository.ProctoringExists(proctoringId))
            {
                return NotFound();
            }

            var proctoringToDelete = _proctoringRepository.GetProctoring(proctoringId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_proctoringRepository.DeleteProctoring(proctoringToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting proctoring");
            }

            return NoContent();
        }
    }
}

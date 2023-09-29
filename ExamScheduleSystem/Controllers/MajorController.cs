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
    public class MajorController : Controller
    {
        private readonly IMajorRepository _majorRepository;
        private readonly IMapper _mapper;

        public MajorController(IMajorRepository majorRepository, IMapper mapper)
        {
            _majorRepository = majorRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PaginationMajorDTO>))]
        /* public IActionResult GetMajors()
         {
             var majors = _mapper.Map<List<MajorDTO>>(_majorRepository.GetMajors());

             if (!ModelState.IsValid)
                 return BadRequest(ModelState);
             return Ok(majors);
         }*/
       
        public IActionResult GetMajors([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyword = "", [FromQuery] string? sortBy = "", [FromQuery] bool isAscending = true)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Invalid page or pageSize parameters.");
            }

            var allMajors = _majorRepository.GetMajors();
            IEnumerable<Major> filteredallMajors = allMajors;

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                filteredallMajors = allMajors.Where(major =>
                    major.MajorId.ToUpper().Contains(keyword.ToUpper()) ||
                    major.MajorName.ToUpper().Contains(keyword.ToUpper())
                );
            }

            int totalCount = filteredallMajors.Count();

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy)
                {
                    case "majorId":
                        filteredallMajors = isAscending
                            ? filteredallMajors.OrderBy(major => major.MajorId)
                            : filteredallMajors.OrderByDescending(major => major.MajorId);
                        break;
                    case "majorName":
                        filteredallMajors = isAscending
                            ? filteredallMajors.OrderBy(major => major.MajorName)
                            : filteredallMajors.OrderByDescending(major => major.MajorName);
                        break;
                        
                }
            }

            var pagedMajors = filteredallMajors
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => _mapper.Map<PaginationMajorDTO>(c))
                .ToList();

            var pagination = new Pagination
            {
                currentPage = page,
                pageSize = pageSize,
                totalPage = Convert.ToInt32(Math.Ceiling((double)totalCount / pageSize))
            };

            PaginatedMajor<Major> paginatedResult = new PaginatedMajor<Major>
            {
                Data = pagedMajors,
                Pagination = pagination
            };

            return Ok(paginatedResult);
        

           
        }

      


        [HttpGet("{majorId}")]
        [ProducesResponseType(200, Type = typeof(Major))]
        [ProducesResponseType(400)]
        public IActionResult GetMajor(string majorId)
        {
            if (!_majorRepository.MajorExists(majorId))
                return NotFound();
            var major = _mapper.Map<MajorDTO>(_majorRepository.GetMajor(majorId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(major);
        }

        [HttpPost]
    //    [Authorize(Roles = "AD,TA")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateMajor([FromBody] MajorDTO majorCreate)
        {
            if (majorCreate == null)
                return BadRequest(ModelState);

            var major = _majorRepository.GetMajors()
                .Where(c => c.MajorName.Trim().ToUpper() == majorCreate.MajorName.Trim().ToUpper())
                .FirstOrDefault();

            if (major != null)
            {
                ModelState.AddModelError("", "Major already existt!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var majorMap = _mapper.Map<Major>(majorCreate);

            if (!_majorRepository.CreateMajor(majorMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("successfully created!");
        }

        [HttpPut("{majorId}")]
      //  [Authorize(Roles = "AD,TA")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateMajor(string majorId, [FromBody] MajorDTO updatedMajor)
        {
            if (updatedMajor == null)
                return BadRequest(ModelState);

            if (majorId != updatedMajor.MajorId)
                return BadRequest(ModelState);

            if (!_majorRepository.MajorExists(majorId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var majorMap = _mapper.Map<Major>(updatedMajor);

            if (!_majorRepository.UpdateMajor(majorMap))
            {
                ModelState.AddModelError("", "Something went wrong updating major");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{majorId}")]
      //  [Authorize(Roles = "AD,TA")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteMajor(string majorId)
        {
            if (!_majorRepository.MajorExists(majorId))
            {
                return NotFound();
            }

            var majorToDelete = _majorRepository.GetMajor(majorId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_majorRepository.DeleteMajor(majorToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting major");
            }

            return NoContent();
        }
    }
}

using AutoMapper;
using ExamScheduleSystem.DTO;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;
using Microsoft.AspNetCore.Mvc;

namespace ExamScheduleSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [ProducesResponseType(200, Type = typeof(IEnumerable<Major>))]
        public IActionResult GetMajors()
        {
            var majors = _mapper.Map<List<MajorDTO>>(_majorRepository.GetMajors());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(majors);
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

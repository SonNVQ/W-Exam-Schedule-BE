using AutoMapper;
using ExamScheduleSystem.DTO;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;
using Microsoft.AspNetCore.Mvc;

namespace ExamScheduleSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [ProducesResponseType(200, Type = typeof(IEnumerable<Proctoring>))]
        public IActionResult GetProctorings()
        {
            var proctorings = _mapper.Map<List<ProctoringDTO>>(_proctoringRepository.GetProctorings());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(proctorings);
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

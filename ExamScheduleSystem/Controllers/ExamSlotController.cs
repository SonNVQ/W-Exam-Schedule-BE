using AutoMapper;
using ExamScheduleSystem.DTO;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamSlotSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  //  [Authorize]
    public class ExamSlotController : Controller
    {
        private readonly IExamSlotRepository _examSlotRepository;
        private readonly IMapper _mapper;

        public ExamSlotController(IExamSlotRepository examSlotRepository, IMapper mapper)
        {
            _examSlotRepository = examSlotRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ExamSlot>))]
        public IActionResult GetExamSlots()
        {
            var examSlots = _mapper.Map<List<ExamSlotDTO>>(_examSlotRepository.GetExamSlots());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(examSlots);
        }

        [HttpGet("{examSlotId}")]
        [ProducesResponseType(200, Type = typeof(ExamSlot))]
        [ProducesResponseType(400)]
        public IActionResult GetExamSlot(string examSlotId)
        {
            if (!_examSlotRepository.ExamSlotExists(examSlotId))
                return NotFound();
            var examSlot = _mapper.Map<ExamSlotDTO>(_examSlotRepository.GetExamSlot(examSlotId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(examSlot);
        }

        [HttpPost]
      //  [Authorize(Roles = "AD,TA")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateExamSlot([FromBody] ExamSlotDTO examSlotCreate)
        {
            if (examSlotCreate == null)
                return BadRequest(ModelState);

            var examSlot = _examSlotRepository.GetExamSlots()
                .Where(c => c.ExamSlotName.Trim().ToUpper() == examSlotCreate.ExamSlotName.Trim().ToUpper())
                .FirstOrDefault();

            if (examSlot != null)
            {
                ModelState.AddModelError("", "ExamSlot already existt!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var examSlotMap = _mapper.Map<ExamSlot>(examSlotCreate);

            if (!_examSlotRepository.CreateExamSlot(examSlotMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("successfully created!");
        }

        [HttpPut("{examSlotId}")]
      //  [Authorize(Roles = "AD,TA")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateExamSlot(string examSlotId, [FromBody] ExamSlotDTO updatedExamSlot)
        {
            if (updatedExamSlot == null)
                return BadRequest(ModelState);

            if (examSlotId != updatedExamSlot.ExamSlotId)
                return BadRequest(ModelState);

            if (!_examSlotRepository.ExamSlotExists(examSlotId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var examSlotMap = _mapper.Map<ExamSlot>(updatedExamSlot);

            if (!_examSlotRepository.UpdateExamSlot(examSlotMap))
            {
                ModelState.AddModelError("", "Something went wrong updating examSlot");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{examSlotId}")]
      //  [Authorize(Roles = "AD,TA")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteExamSlot(string examSlotId)
        {
            if (!_examSlotRepository.ExamSlotExists(examSlotId))
            {
                return NotFound();
            }

            var examSlotToDelete = _examSlotRepository.GetExamSlot(examSlotId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_examSlotRepository.DeleteExamSlot(examSlotToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting examSlot");
            }

            return NoContent();
        }


    }
}

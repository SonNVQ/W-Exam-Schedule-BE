using AutoMapper;
using ExamScheduleSystem.DTO;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamScheduleSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExamScheduleController : Controller
    {
        private readonly IExamScheduleRepository _examScheduleRepository;
        private readonly IMapper _mapper;

        public ExamScheduleController(IExamScheduleRepository examScheduleRepository, IMapper mapper)
        {
            _examScheduleRepository = examScheduleRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ExamSchedule>))]
        public IActionResult GetExamSchedules()
        {
            var examSchedules = _mapper.Map<List<ExamScheduleDTO>>(_examScheduleRepository.GetExamSchedules());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(examSchedules);
        }

        [HttpGet("{examScheduleId}")]
        [ProducesResponseType(200, Type = typeof(ExamSchedule))]
        [ProducesResponseType(400)]
        public IActionResult GetExamSchedule(string examScheduleId)
        {
            if (!_examScheduleRepository.ExamScheduleExists(examScheduleId))
                return NotFound();
            var examSchedule = _mapper.Map<ExamScheduleDTO>(_examScheduleRepository.GetExamSchedule(examScheduleId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(examSchedule);
        }

        [HttpPost]
        [Authorize (Roles = "AD,TA")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateExamSchedule([FromBody] ExamScheduleDTO examScheduleCreate)
        {
            if (examScheduleCreate == null)
                return BadRequest(ModelState);

            var examSchedule = _examScheduleRepository.GetExamSchedules()
                .Where(c => c.ExamScheduleId.Trim().ToUpper() == examScheduleCreate.ExamScheduleId.Trim().ToUpper())
                .FirstOrDefault();

            if (examSchedule != null)
            {
                ModelState.AddModelError("", "ExamSchedule already existt!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var examScheduleMap = _mapper.Map<ExamSchedule>(examScheduleCreate);

            if (!_examScheduleRepository.CreateExamSchedule(examScheduleMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("successfully created!");
        }

        [HttpPut("{examScheduleId}")]
        [Authorize(Roles = "AD,TA")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateExamSchedule(string examScheduleId, [FromBody] ExamScheduleDTO updatedExamSchedule)
        {
            if (updatedExamSchedule == null)
                return BadRequest(ModelState);

            if (examScheduleId != updatedExamSchedule.ExamScheduleId)
                return BadRequest(ModelState);

            if (!_examScheduleRepository.ExamScheduleExists(examScheduleId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var examScheduleMap = _mapper.Map<ExamSchedule>(updatedExamSchedule);

            if (!_examScheduleRepository.UpdateExamSchedule(examScheduleMap))
            {
                ModelState.AddModelError("", "Something went wrong updating examSchedule");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{examScheduleId}")]
        [Authorize(Roles = "AD,TA")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteExamSchedule(string examScheduleId)
        {
            if (!_examScheduleRepository.ExamScheduleExists(examScheduleId))
            {
                return NotFound();
            }

            var examScheduleToDelete = _examScheduleRepository.GetExamSchedule(examScheduleId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_examScheduleRepository.DeleteExamSchedule(examScheduleToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting examSchedule");
            }

            return NoContent();
        }


    }
}

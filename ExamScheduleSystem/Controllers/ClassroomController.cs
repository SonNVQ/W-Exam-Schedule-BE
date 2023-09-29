using AutoMapper;
using ExamScheduleSystem.Data;
using ExamScheduleSystem.DTO;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamScheduleSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //  [Authorize]
    public class ClassroomController : Controller
    {
        private readonly IClassroomRepository _classroomRepository;
        private readonly IMapper _mapper;

        public ClassroomController(IClassroomRepository classroomRepository, IMapper mapper)
        {
            _classroomRepository = classroomRepository;
            _mapper = mapper;
        }

        /* [HttpGet]
         [ProducesResponseType(200, Type = typeof(IEnumerable<Classroom>))]
         public IActionResult GetClassrooms()
         {
             var classrooms = _mapper.Map<List<ClassroomDTO>>(_classroomRepository.GetClassrooms());

             if (!ModelState.IsValid)
                 return BadRequest(ModelState);
             return Ok(classrooms);
         }*/

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(PaginationClassroomDTO))]
        public IActionResult GetClassrooms([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyword = "")
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Invalid page or pageSize parameters.");
            }

            var allClassrooms = _classroomRepository.GetClassrooms();
            IEnumerable<Classroom> filteredClassrooms = allClassrooms;
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                filteredClassrooms = allClassrooms.Where(classroom =>
                    classroom.ClassroomId.ToUpper().Contains(keyword.ToUpper()) ||
                    classroom.Name.ToUpper().Contains(keyword.ToUpper()) ||
                    classroom.Capacity.ToString().ToUpper().Contains(keyword.ToUpper())
                );
            }

            int totalCount = filteredClassrooms.Count();
            var pagedClassrooms = filteredClassrooms
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => _mapper.Map<PaginationClassroomDTO>(c))
                .ToList();

            var pagination = new Pagination
            {
                currentPage = page,
                pageSize = pageSize,
                totalPage = Convert.ToInt32(Math.Ceiling((double)totalCount / pageSize))
            };


            PaginatedResult<Classroom> paginatedResult = new PaginatedResult<Classroom>
            {
                Data = pagedClassrooms,
                Pagination = pagination
            };

            return Ok(paginatedResult);
        }




        [HttpGet("{classroomId}")]
        [ProducesResponseType(200, Type = typeof(Classroom))]
        [ProducesResponseType(400)]
        public IActionResult GetClassroom(string classroomId)
        {
            if (!_classroomRepository.ClassroomExists(classroomId))
                return NotFound();
            var classroom = _mapper.Map<PaginationClassroomDTO>(_classroomRepository.GetClassroom(classroomId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(classroom);
        }

        [HttpPost]
        //  [Authorize(Roles = "AD,TA")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateClassroom([FromBody] PaginationClassroomDTO classroomCreate)
        {
            if (classroomCreate == null)
                return BadRequest(ModelState);

            var classroom = _classroomRepository.GetClassrooms()
                .Where(c => c.Name.Trim().ToUpper() == classroomCreate.Name.Trim().ToUpper())
                .FirstOrDefault();

            if (classroom != null)
            {
                ModelState.AddModelError("", "Classroom already existt!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var classroomMap = _mapper.Map<Classroom>(classroomCreate);

            if (!_classroomRepository.CreateClassroom(classroomMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("successfully created!");
        }

        [HttpPut("{classroomId}")]
        //    [Authorize(Roles = "AD,TA")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateClassroom(string classroomId, [FromBody] PaginationClassroomDTO updatedClassroom)
        {
            if (updatedClassroom == null)
                return BadRequest(ModelState);

            if (classroomId != updatedClassroom.ClassroomId)
                return BadRequest(ModelState);

            if (!_classroomRepository.ClassroomExists(classroomId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var classroomMap = _mapper.Map<Classroom>(updatedClassroom);

            if (!_classroomRepository.UpdateClassroom(classroomMap))
            {
                ModelState.AddModelError("", "Something went wrong updating classroom");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{classroomId}")]
        //   [Authorize(Roles = "AD,TA")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteClassroom(string classroomId)
        {
            if (!_classroomRepository.ClassroomExists(classroomId))
            {
                return NotFound();
            }

            var classroomToDelete = _classroomRepository.GetClassroom(classroomId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_classroomRepository.DeleteClassroom(classroomToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting classroom");
            }

            return NoContent();
        }


    }

}

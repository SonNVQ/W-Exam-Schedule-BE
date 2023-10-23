using AutoMapper;
using ExamScheduleSystem.DTO;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;
using ExamScheduleSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Microsoft.AspNetCore.Mvc;

namespace ExamScheduleSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //  [Authorize]
    public class ExamScheduleController : Controller
    {
        private readonly IExamScheduleRepository _examScheduleRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentListRepository _studentListRepository;
        private readonly ICourseStudentListRepository _courseStudentListRepository;
        private readonly IClassroomRepository _classroomRepository;
        private readonly IProctoringRepository _proctoringRepository;
        private readonly IExamSlotRepository _examSlotRepository;
        private readonly IClassroomExamScheduleRepository _classroomExamScheduleRepository;
        private readonly IMapper _mapper;

        private bool ValidClassroom(string classroomId, string examSlotId)
        {
            // Retrieve all exam schedules for the given classroom
            var classroomSchedules = _classroomExamScheduleRepository.GetExamScheduleByClassroomId(classroomId);

            // Check if there are no schedules for the classroom
            if (classroomSchedules == null || !classroomSchedules.Any())
            {
                // If no schedules are found, the classroom is valid.
                return true;
            }
            var currentExamSlot = _examSlotRepository.GetExamSlot(examSlotId);
            var currentDateTime = currentExamSlot.Date.Date + currentExamSlot.StartTime;

            // Extract the date and time from each exam slot
            foreach (var schedule in classroomSchedules)
            {
                // Retrieve the associated exam slot
                var examSlot = _examSlotRepository.GetExamSlot(schedule.ExamScheduleId);

                if (examSlot == null)
                {
                    // Handle the case where the exam slot is not found.
                    continue;
                }

                // Combine the Date and StartTime fields into a single DateTime object
                var examSlotDateTime = examSlot.Date.Date + examSlot.StartTime;
                // Compare the exam slot date and time with the input date and time
                if (examSlotDateTime != currentDateTime)
                {
                    // If any exam slot has a different date and time, the classroom is valid.
                    return true;
                }
            }

            // If all exam slots have the same date and time as the input, the classroom is invalid.
            return false;
        }


        public ExamScheduleController(IExamScheduleRepository examScheduleRepository, IMapper mapper, ICourseRepository courseRepository, IStudentListRepository studentList, ICourseStudentListRepository courseStudentListRepository, IClassroomRepository classroomRepository, IProctoringRepository proctoringRepository, IExamSlotRepository examSlotRepository, IClassroomExamScheduleRepository classroomExamScheduleRepository)
        {
            _examScheduleRepository = examScheduleRepository;
            _mapper = mapper;
            _courseRepository = courseRepository;
            _studentListRepository = studentList;
            _courseStudentListRepository = courseStudentListRepository;
            _classroomRepository = classroomRepository;
            _proctoringRepository = proctoringRepository;
            _examSlotRepository = examSlotRepository;
            _classroomExamScheduleRepository = classroomExamScheduleRepository;
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


        [HttpGet("GetExamSchedulesByCourseIDAndExamSlotID")]
        [ProducesResponseType(200, Type = typeof(PaginatedExamSchedule<ExamScheduleDTO>))]
        public IActionResult GetExamSchedulesByCourseIDAndExamSlotID(
    string CourseId,
    string ExamSlotId,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? keyword = "",
    [FromQuery] string? sortBy = "",
    [FromQuery] bool isAscending = true,
    [FromQuery] string? courseIdFilter = "",
    [FromQuery] string? examSlotIdFilter = "")
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Invalid page or pageSize parameters.");
            }

            var allExamSchedules = _examScheduleRepository.GetExamSchedulesByCourseIDAndExamSlotID(CourseId, ExamSlotId);

            var examScheduleList = allExamSchedules.ToList();

            ICollection<ExamSchedule> examCollection = (ICollection<ExamSchedule>)examScheduleList;



            if (!string.IsNullOrWhiteSpace(courseIdFilter))
            {
                allExamSchedules = allExamSchedules.Where(schedule =>
                    schedule.CourseId.Contains(courseIdFilter, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(examSlotIdFilter))
            {
                allExamSchedules = allExamSchedules.Where(schedule =>
                    schedule.ExamSlotId.Contains(examSlotIdFilter, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                allExamSchedules = allExamSchedules.Where(schedule =>
                    schedule.CourseId.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    schedule.ExamSlotId.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            int totalCount = allExamSchedules.Count();

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy)
                {
                    case "CourseId":
                        allExamSchedules = isAscending
                            ? allExamSchedules.OrderBy(schedule => schedule.CourseId).ToList()
                            : allExamSchedules.OrderByDescending(schedule => schedule.CourseId).ToList();
                        break;
                    case "ExamSlotId":
                        allExamSchedules = isAscending
                            ? allExamSchedules.OrderBy(schedule => schedule.ExamSlotId).ToList()
                            : allExamSchedules.OrderByDescending(schedule => schedule.ExamSlotId).ToList();
                        break;
                }
            }


            int startIndex = (page - 1) * pageSize;
            if (startIndex >= totalCount)
            {
                startIndex = 0;
                page = 1;
            }

            var pagedExamSchedules = allExamSchedules.Skip(startIndex).Take(pageSize).ToList();
            var examSchedulesDTO = _mapper.Map<List<PaginationExamScheduleDTO>>(pagedExamSchedules);

            var pagination = new Pagination
            {
                currentPage = page,
                pageSize = pageSize,
                totalPage = Convert.ToInt32(Math.Ceiling((double)totalCount / pageSize))
            };

            PaginatedExamSchedule<ExamScheduleDTO> paginatedResult = new PaginatedExamSchedule<ExamScheduleDTO>
            {
                Data = examSchedulesDTO,
                Pagination = pagination
            };

            return Ok(paginatedResult);
        }


        [HttpPost]
        //     [Authorize (Roles = "AD,TA")]
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
        //    [Authorize(Roles = "AD,TA")]
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
        //  [Authorize(Roles = "AD,TA")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteExamSchedule(string examScheduleId)
        {
            if (!_examScheduleRepository.ExamScheduleExists(examScheduleId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_examScheduleRepository.DeleteExamSchedule(examScheduleId))
            {
                ModelState.AddModelError("", "Something went wrong deleting examSchedule");
            }

            return NoContent();
        }
        [HttpPost("GenerateExamSchedule")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult GenerateExamSchedule(string courseId, string examSlotId)
        {
            if (string.IsNullOrWhiteSpace(courseId) || string.IsNullOrWhiteSpace(examSlotId))
            {
                ModelState.AddModelError("", "Both courseId and examSlotId must be provided.");
                return BadRequest(ModelState);
            }

            // Retrieve the course and associated student list from your repository
            var studentList = _studentListRepository.GetStudentListsByCourseId(courseId);

            
            if (studentList == null)
            {
                ModelState.AddModelError("courseId", "Student list for the course not found.");
                return BadRequest(ModelState);
            }
            var classroomList = _classroomRepository.GetClassrooms().Where(x => x.Status.ToLower() == "active").ToList();
            int currentClassroomIndex = 0;
            // You should have logic to determine ClassroomId and ProctoringId here
            var proctorings = _examSlotRepository.GetProctoringsByExamSlotId(examSlotId);
            int currentProctoringIndex = 0;
            var listProctoring = proctorings.Select(proctoring => new
            {
                proctoringId = proctoring.ProctoringId,
                proctoringName = proctoring.ProctoringName,
                compensation = proctoring.Compensation,
                status = proctoring.Status
            }
                ).ToList();

            // Create an ExamSchedule object
            var currentExamSlot = _examSlotRepository.GetExamSlot(examSlotId);

            //var currentSlotTime = currentExamSlot.Date.ToString().Substring(0,11).Concat(currentExamSlot.StartTime.ToString());
            var currentSlotTime = currentExamSlot.Date.ToString("yyyy-MM-dd") + "T" + currentExamSlot.StartTime.ToString();

            foreach (var item in studentList)
            {
                var examScheduleId = examSlotId + "_" + item.StudentListId;
                var tempCL = false;
                var tempPr = false;
                var existingExamScheduleId = _examScheduleRepository.ExamScheduleExists(examScheduleId);

                if (existingExamScheduleId)
                {
                    _examScheduleRepository.DeleteExamSchedule(examScheduleId);
                }
                if (currentClassroomIndex >= classroomList.Count)
                {
                    currentClassroomIndex--;
                    tempCL = true;
                }
                if (currentProctoringIndex >= listProctoring.Count)
                {
                    currentProctoringIndex--;
                    tempPr = true;
                }

                while (currentClassroomIndex < classroomList.Count)
                {
                    var currentClassroom = classroomList[currentClassroomIndex];
                    var validClassroom = ValidClassroom(currentClassroom.ClassroomId, examSlotId);
                    if (validClassroom)
                    {
                        var currentProctoring = listProctoring[currentProctoringIndex];
                        var examSchedule = new ExamSchedule
                        {
                            ExamScheduleId = examScheduleId,
                            ExamSlotId = examSlotId,
                            ClassroomId = currentClassroom.ClassroomId,
                            CourseId = courseId,
                            StudentListId = item.StudentListId,
                            ProctoringId = currentProctoring.proctoringId,
                            Status = "active"
                        };

                        var classroomExamSchedule = new ClassroomExamSchedule
                        {
                            ClassroomId = currentClassroom.ClassroomId,
                            ExamScheduleId = examScheduleId,
                        };

                        _examScheduleRepository.CreateExamSchedule(examSchedule);
                        _classroomExamScheduleRepository.AddClassroomExamSchedule(classroomExamSchedule);
                        currentClassroomIndex++;
                        currentProctoringIndex++;
                        break;
                    }
                    else
                    {
                        currentClassroomIndex++;
                        tempCL = true;
                    }
                }


            }
            return Ok("Generate Successfully!");
        }
    }


}

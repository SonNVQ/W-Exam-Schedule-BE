using AutoMapper;
using ExamScheduleSystem.DTO;
using ExamScheduleSystem.Interfaces;
using ExamScheduleSystem.Model;
using ExamScheduleSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Globalization;

namespace ExamSlotSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  //  [Authorize]
    public class ExamSlotController : Controller
    {
        private readonly IExamSlotRepository _examSlotRepository;
        private readonly IExamSlotProctoringRepository _examSlotProctoringRepository;
        private readonly IMapper _mapper;

        public ExamSlotController(IExamSlotRepository examSlotRepository, IMapper mapper, IExamSlotProctoringRepository examSlotProctoringRepository)
        {
            _examSlotRepository = examSlotRepository;
            _mapper = mapper;
            _examSlotProctoringRepository = examSlotProctoringRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ExamSlot>))]
        /* public IActionResult GetExamSlots()
         {
             var examSlots = _mapper.Map<List<ExamSlotDTO>>(_examSlotRepository.GetExamSlots());

             if (!ModelState.IsValid)
                 return BadRequest(ModelState);
             return Ok(examSlots);
         }*/
        public IActionResult GetExamSlots([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyword = "", [FromQuery] string? sortBy = "", [FromQuery] bool isAscending = true)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Invalid page or pageSize parameters.");
            }
            
            var allexamSlots = _examSlotRepository.GetExamSlots();

            foreach (var examSlot in allexamSlots) { }

            IEnumerable<ExamSlot> filteredallexamSlots = allexamSlots ?? Enumerable.Empty<ExamSlot>();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                filteredallexamSlots = filteredallexamSlots.Where(examSlot =>
                    (examSlot.ExamSlotId != null && examSlot.ExamSlotId.ToUpper().Contains(keyword.ToUpper())) ||
                    (examSlot.ExamSlotProctorings != null) ||
                    (examSlot.ExamSlotName != null && examSlot.ExamSlotName.ToUpper().Contains(keyword.ToUpper())))
                    .ToList();
            }
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy)
                {
                    case "examSlotId":
                        filteredallexamSlots = isAscending
                            ? filteredallexamSlots.OrderBy(examSlot => examSlot.ExamSlotId)
                            : filteredallexamSlots.OrderByDescending(examSlot => examSlot.ExamSlotId);
                        break;
                    case "examSlotName":
                        filteredallexamSlots = isAscending
                            ? filteredallexamSlots.OrderBy(examSlot => examSlot.ExamSlotName)
                            : filteredallexamSlots.OrderByDescending(examSlot => examSlot.ExamSlotName);
                        break;
                    case "date":
                        filteredallexamSlots = isAscending
                            ? filteredallexamSlots.OrderBy(examSlot => examSlot.Date)
                            : filteredallexamSlots.OrderByDescending(examSlot => examSlot.Date);
                        break;
                }
            }
            int totalCount = filteredallexamSlots.Count();

            var pagedExamSlots = filteredallexamSlots
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(examSlot => new ExamSlotDTO
                {
                    ExamSlotId = examSlot.ExamSlotId,
                    ExamSlotName = examSlot.ExamSlotName,
                    listProctoring = _examSlotRepository.GetProctoringsByExamSlotId(examSlot.ExamSlotId)
                        .Select(proctoring => new ProctoringDTO
                        {
                            ProctoringId = proctoring.ProctoringId,
                            ProctoringName = proctoring.ProctoringName,
                            Compensation = proctoring.Compensation,
                            Status = proctoring.Status
                        })
                        .ToList(),
                    Status = examSlot.Status,
                    Date = examSlot.Date,
                    StartTime = examSlot.StartTime,
                    EndTime = examSlot.EndTime
                })
                .ToList();

            var pagination = new Pagination
            {
                currentPage = page,
                pageSize = pageSize,
                totalPage = Convert.ToInt32(Math.Ceiling((double)totalCount / pageSize))
            };


                if (pagedExamSlots.Any())
                {
                    PaginatedExamSlot<ExamSlotDTO> paginatedResult = new PaginatedExamSlot<ExamSlotDTO>
                    {
                        Data = pagedExamSlots,
                        Pagination = pagination
                    };

                    return Ok(paginatedResult);
                }
                else
                {
                    return NotFound("No matching proctorings lists found.");
                }
        }

        [HttpGet("{examSlotId}")]
        [ProducesResponseType(200, Type = typeof(ExamSlot))]
        [ProducesResponseType(400)]
        public IActionResult GetExamSlot(string examSlotId)
        {
            var proctorings = _examSlotRepository.GetProctoringsByExamSlotId(examSlotId);
            var examSlots = _examSlotRepository.GetExamSlot(examSlotId);
            if (proctorings == null)
            {
                return NotFound("Proctoring List not found");
            }
            // Construct the response object
            var response = new
            {
                examSlotId = examSlotId,
                examSlotName = examSlots.ExamSlotName,
                Status = examSlots.Status,
                Date = examSlots.Date,
                StartTime = examSlots.StartTime,
                EndTime = examSlots.EndTime,
                listProctoring = proctorings .Select(proctoring => new
                {
                    proctoringId = proctoring.ProctoringId,
                    proctoringName = proctoring.ProctoringName,
                    compensation = proctoring.Compensation,
                    status = proctoring.Status
                }
                ).ToList(),
            };

            return Ok(response);
        }

        //  [HttpPost]
        ////  [Authorize(Roles = "AD,TA")]
        //  [ProducesResponseType(204)]
        //  [ProducesResponseType(400)]
        //  public IActionResult CreateExamSlot([FromBody] ExamSlotDTO examSlotCreate)
        //  {
        //      if (examSlotCreate == null)
        //          return BadRequest(ModelState);

        //      var examSlot = _examSlotRepository.GetExamSlots()
        //          .Where(c => c.ExamSlotName.Trim().ToUpper() == examSlotCreate.ExamSlotName.Trim().ToUpper())
        //          .FirstOrDefault();

        //      if (examSlot != null)
        //      {
        //          ModelState.AddModelError("", "ExamSlot already existt!");
        //          return StatusCode(422, ModelState);
        //      }

        //      if (!ModelState.IsValid)
        //          return BadRequest(ModelState);

        //      var examSlotMap = _mapper.Map<ExamSlot>(examSlotCreate);

        //      if (!_examSlotRepository.CreateExamSlot(examSlotMap))
        //      {
        //          ModelState.AddModelError("", "Something went wrong while saving");
        //          return StatusCode(500, ModelState);
        //      }

        //      return Ok("successfully created!");
        //  }


        [HttpPost]
        //[Authorize(Roles = "AD,TA")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateExamSlot([FromBody] ExamSlotDTO request)
        {
            if (request == null)
                return BadRequest("Invalid JSON data.");
            var examSlot = new ExamSlot
            {
                ExamSlotId = request.ExamSlotId,
                ExamSlotName = request.ExamSlotName,
                Status = request.Status,
                Date = request.Date,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                ExamSlotProctorings = new List<ExamSlotProctoring>()
            };
            _examSlotRepository.CreateExamSlot(examSlot);

            try
            {
                foreach (var proctoring in request.listProctoring)
                {
                    var ExamSlotProctoring = new ExamSlotProctoring
                    {
                        ExamSlotId = request.ExamSlotId,
                        ProctoringId = proctoring.ProctoringId
                    };
                    _examSlotProctoringRepository.AddExamSlotProctoring(ExamSlotProctoring);
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

            var newExamSlot = new ExamSlot
            {
                ExamSlotId = updatedExamSlot.ExamSlotId,
                ExamSlotName = updatedExamSlot.ExamSlotName,
                Status = updatedExamSlot.Status,
                Date = DateTime.Now,
                StartTime = updatedExamSlot.StartTime,
                EndTime = updatedExamSlot.EndTime
            };
            var examSlotMap = _mapper.Map<ExamSlot>(updatedExamSlot);
            var newListProctoring = new List<ExamSlotProctoring>();
            foreach (var proctoring in updatedExamSlot.listProctoring)
            {
                var ExamSlotProctoring = new ExamSlotProctoring
                {
                    ExamSlotId = updatedExamSlot.ExamSlotId,
                    ProctoringId = proctoring.ProctoringId
                };
                newListProctoring.Add(ExamSlotProctoring);
            }
            _examSlotProctoringRepository.UpdateExamSlotProctoring(updatedExamSlot.ExamSlotId, newListProctoring);

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

        [HttpPost("import")]
        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file.");
            }
            try
            {
                using (var package = new ExcelPackage(file.OpenReadStream()))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                    {
                        string startTimeText = worksheet.Cells[row, 7].Text.Trim(); // Assuming '7:30:00 AM'
                        string endTimeText = worksheet.Cells[row, 8].Text.Trim(); // Assuming '10:15:00 AM'
                        string proctoringIdText = worksheet.Cells[row, 3].Text.Trim();

                        string timeFormat = "h:mm:ss tt";
                        CultureInfo provider = CultureInfo.InvariantCulture; // Use InvariantCulture
                        DateTime startTime;
                        if (DateTime.TryParseExact(startTimeText, timeFormat, provider, DateTimeStyles.NoCurrentDateDefault, out startTime))
                        {
                            DateTime endTime;

                            if (DateTime.TryParseExact(endTimeText, timeFormat, provider, DateTimeStyles.NoCurrentDateDefault, out endTime))
                            {
                                var examSlot = new ExamSlotDTO
                                {
                                    ExamSlotId = worksheet.Cells[row, 1].Value.ToString(),
                                    ExamSlotName = worksheet.Cells[row, 2].Value.ToString(),
                                    Status = worksheet.Cells[row, 5].Value.ToString(),
                                    Date = DateTime.Parse(worksheet.Cells[row, 6].Text),
                                    StartTime = startTime.TimeOfDay,
                                    EndTime = endTime.TimeOfDay
                                };
                                var examSlotMap = _mapper.Map<ExamSlot>(examSlot);
                                _examSlotRepository.CreateExamSlot(examSlotMap);
                            }
                            else
                            {
                                // Handle invalid end time format
                            }
                        }
                        else
                        {
                            // Handle invalid start time format
                        }
                    }
                    return Ok("Data imported successfully.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}

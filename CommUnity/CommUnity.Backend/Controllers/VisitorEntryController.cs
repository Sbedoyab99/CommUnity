using CommUnity.BackEnd.UnitsOfWork.Implementations;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommUnity.BackEnd.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class VisitorEntryController : GenericController<VisitorEntry>
    {
        private readonly IVisitorEntryUnitOfWork _visitorEntryUnitOfWork;

        public VisitorEntryController(IGenericUnitOfWork<VisitorEntry> unitOfWork, IVisitorEntryUnitOfWork visitorEntryUnitOfWork) : base(unitOfWork)
        {
            _visitorEntryUnitOfWork = visitorEntryUnitOfWork;
        }

        [HttpPost("schedule")]
        public async Task<IActionResult> ScheduleVisitor(VisitorEntryDTO visitorEntryDTO)
        {
            var action = await _visitorEntryUnitOfWork.ScheduleVisitor(User.Identity!.Name!, visitorEntryDTO);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            else
            {
                return BadRequest(action.Message);
            }
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetVisitorEntryByStatus(VisitorStatus status)
        {
            var action = await _visitorEntryUnitOfWork.GetVisitorEntryByStatus(User.Identity!.Name!, status);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            else
            {
                return BadRequest(action.Message);
            }
        }

        [HttpPut("confirm")]
        public async Task<IActionResult> ConfirmVisitorEntry(VisitorEntryDTO visitorEntryDTO)
        {
            var action = await _visitorEntryUnitOfWork.ConfirmVisitorEntry(User.Identity!.Name!, visitorEntryDTO);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            else
            {
                return BadRequest(action.Message);
            }
        }

        [HttpPut("cancel")]
        public async Task<IActionResult> CancelVisitorEntry(VisitorEntryDTO visitorEntryDTO)
        {
            var action = await _visitorEntryUnitOfWork.CancelVisitorEntry(User.Identity!.Name!, visitorEntryDTO);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            else
            {
                return BadRequest(action.Message);
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddVisitor(VisitorEntryDTO visitorEntryDTO)
        {
            var action = await _visitorEntryUnitOfWork.AddVisitor(User.Identity!.Name!, visitorEntryDTO);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            else
            {
                return BadRequest(action.Message);
            }
        }

        [HttpGet("apartment/{apartmentId}")]
        public async Task<IActionResult> GetVisitorEntryByApartment(int apartmentId)
        {
            var action = await _visitorEntryUnitOfWork.GetVisitorEntryByApartment(User.Identity!.Name!, apartmentId);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            else
            {
                return BadRequest(action.Message);
            }
        }

        [HttpGet("RecordsNumber")]
        public async Task<IActionResult> GetVisitorEntryRecordsNumber(int id, VisitorStatus status)
        {
            var action = await _visitorEntryUnitOfWork.GetVisitorEntryRecordsNumber(User.Identity!.Name!, id, status);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            else
            {
                return BadRequest(action.Message);
            }
        }

        [HttpGet("RecordsNumberResidentialUnit")]
        public async Task<IActionResult> GetVisitorEntryRecordsNumberResidentialUnit([FromQuery] PaginationVisitorDTO paginationVisitorDTO)
        {
            var action = await _visitorEntryUnitOfWork.GetVisitorRecordsNumberResidentialUnit(User.Identity!.Name!, paginationVisitorDTO);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            else
            {
                return BadRequest(action.Message);
            }
        }

        [HttpGet("RecordsNumberAparment")]
        public async Task<IActionResult> GetVisitorEntryRecordsNumberApartment([FromQuery] PaginationVisitorDTO paginationVisitorDTO)
        {
            var action = await _visitorEntryUnitOfWork.GetVisitorRecordsNumberApartment(User.Identity!.Name!, paginationVisitorDTO);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            else
            {
                return BadRequest(action.Message);
            }
        }

        [HttpGet("VisitorEntryResidentialUnitStatus")]
        public async Task<IActionResult> GetMailByResidentialUnitStatus([FromQuery] PaginationVisitorDTO paginationVisitorDTO)
        {
            var action = await _visitorEntryUnitOfWork.GetVisitorByResidentialUnitStatus(User.Identity!.Name!, paginationVisitorDTO);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            else
            {
                return BadRequest(action.Message);
            }
        }

        [HttpGet("VisitorEntryApartmentStatus")]
        public async Task<IActionResult> GetMailApartmentStatus([FromQuery] PaginationVisitorDTO paginationVisitorDTO)
        {
            var action = await _visitorEntryUnitOfWork.GetVisitorEntryByAparmentStatus(User.Identity!.Name!, paginationVisitorDTO);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            else
            {
                return BadRequest(action.Message);
            }
        }

    }
}

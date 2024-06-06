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
            if(action.WasSuccess)
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
            if(action.WasSuccess)
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
            if(action.WasSuccess)
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
    }
}

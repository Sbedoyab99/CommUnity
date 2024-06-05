using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Responses;
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
        public async Task<ActionResult<ActionResponse<VisitorEntry>>> ScheduleVisitor(VisitorEntryDTO visitorEntryDTO)
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
    }
}

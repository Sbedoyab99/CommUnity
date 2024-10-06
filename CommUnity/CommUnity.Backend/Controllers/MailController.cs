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
    public class MailController : GenericController<MailArrival>
    {
        private readonly IMailUnitOfWork _mailUnitOfWork;

        public MailController(IGenericUnitOfWork<MailArrival> unitOfWork, IMailUnitOfWork mailUnitOfWork) : base(unitOfWork)
        {
            _mailUnitOfWork = mailUnitOfWork;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterMail(MailArrivalDTO mailArrivalDTO)
        {
            var action = await _mailUnitOfWork.RegisterMail(User.Identity!.Name!, mailArrivalDTO);
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
        public async Task<IActionResult> GetMailByStatus(MailStatus status)
        {
            var action = await _mailUnitOfWork.GetMailByStatus(User.Identity!.Name!, status);
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
        public async Task<IActionResult> ConfirmMail(MailArrivalDTO mailArrivalDTO)
        {
            var action = await _mailUnitOfWork.ConfirmMail(User.Identity!.Name!, mailArrivalDTO);
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
        public async Task<IActionResult> GetMailByApartment(int apartmentId)
        {
            var action = await _mailUnitOfWork.GetMailByApartment(User.Identity!.Name!, apartmentId);
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
        public async Task<IActionResult> GetMailRecordsNumber(int id, MailStatus status)
        {
            var action = await _mailUnitOfWork.GetMailRecordsNumber(User.Identity!.Name!, id, status);
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

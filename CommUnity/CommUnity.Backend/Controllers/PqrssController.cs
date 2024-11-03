using CommUnity.BackEnd.Repositories.Interfaces;
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
    public class PqrssController : GenericController<Pqrs>
    {
        private readonly IPqrsUnitOfWork _pqrsUnitOfWork;

        public PqrssController(IGenericUnitOfWork<Pqrs> unitOfWork, IPqrsUnitOfWork pqrsUnitOfWork) : base(unitOfWork)
        {
            _pqrsUnitOfWork = pqrsUnitOfWork;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePqrs(PqrsDTO pqrsDTO)
        {
            var action = await _pqrsUnitOfWork.CreatePqrs(User.Identity!.Name!, pqrsDTO);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            else
            {
                return BadRequest(action.Message);
            }
        }

        [HttpGet("type/{type}/status/{status}")]
        public async Task<IActionResult> GetPqrsByTypeByStatus(PqrsType type,PqrsState status)
        {
            var action = await _pqrsUnitOfWork.GetPqrsByTypeByStatus(User.Identity!.Name!, type, status);
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
        public async Task<IActionResult> GetVisitorEntryRecordsNumber(int id, PqrsType type, PqrsState status)
        {
            var action = await _pqrsUnitOfWork.GetPqrsRecordsNumber(User.Identity!.Name!, id,type, status);
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

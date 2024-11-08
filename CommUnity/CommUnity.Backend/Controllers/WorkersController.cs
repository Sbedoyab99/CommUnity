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

    public class WorkersController : GenericController<User>
    {
        private readonly IUsersUnitOfWork _usersUnitOfWork;

        public WorkersController(IGenericUnitOfWork<User> unitOfWork, IUsersUnitOfWork usersUnitOfWork) : base(unitOfWork)
        {
            _usersUnitOfWork = usersUnitOfWork;
        }

        [HttpGet("workers")]
        [AllowAnonymous]
        public async Task<IActionResult> GetResidentsAsync([FromQuery] PaginationDTO paginationDTO)
        {
            var response = await _usersUnitOfWork.GetUsersAsync(paginationDTO, UserType.Worker);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _usersUnitOfWork.GetTotalPagesAsync(pagination, UserType.Worker);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpGet("recordsNumber")]
        public async Task<IActionResult> GetRecordsNumber([FromQuery] PaginationDTO pagination)
        {
            var response = await _usersUnitOfWork.GetRecordsNumber(pagination, UserType.Worker);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }
    }
}

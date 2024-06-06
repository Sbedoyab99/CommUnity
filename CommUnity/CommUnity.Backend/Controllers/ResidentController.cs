using CommUnity.BackEnd.UnitsOfWork.Implementations;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommUnity.BackEnd.Controllers
{

    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]

    public class ResidentController : GenericController<User>
    {
        private readonly IResidentUnitOfWork _ResidentUnitOfWork;

        public ResidentController(IGenericUnitOfWork<User> unitOfWork, IResidentUnitOfWork ResidentUnitOfWork) : base(unitOfWork)
        {
            _ResidentUnitOfWork = ResidentUnitOfWork;
        }

        [HttpGet("resident")]
        public override async Task<IActionResult> GetAsync([FromQuery] int apartmentId)
        {
            var response = await _ResidentUnitOfWork.GetResidentAsync(apartmentId);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

    }
}

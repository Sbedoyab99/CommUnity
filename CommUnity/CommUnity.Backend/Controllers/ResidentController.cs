﻿using CommUnity.BackEnd.UnitsOfWork.Implementations;
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

    public class ResidentController : GenericController<User>
    {
        private readonly IResidentUnitOfWork _ResidentUnitOfWork;
        private readonly IUsersUnitOfWork _usersUnitOfWork;

        public ResidentController(IGenericUnitOfWork<User> unitOfWork, IResidentUnitOfWork ResidentUnitOfWork, IUsersUnitOfWork usersUnitOfWork) : base(unitOfWork)
        {
            _ResidentUnitOfWork = ResidentUnitOfWork;
            _usersUnitOfWork = usersUnitOfWork;
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

        [HttpGet("residents")]
        [AllowAnonymous]
        public async Task<IActionResult> GetResidentsAsync([FromQuery] PaginationDTO paginationDTO)
        {
            var response = await _usersUnitOfWork.GetUsersAsync(paginationDTO, UserType.Resident);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _usersUnitOfWork.GetTotalPagesAsync(pagination, UserType.Resident);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpGet("recordsNumber")]
        public async Task<IActionResult> GetRecordsNumber([FromQuery] PaginationDTO pagination)
        {
            var response = await _usersUnitOfWork.GetRecordsNumber(pagination, UserType.Resident);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }
    }
}

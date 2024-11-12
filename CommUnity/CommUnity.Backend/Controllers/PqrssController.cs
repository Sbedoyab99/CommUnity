using CommUnity.BackEnd.Helpers;
using CommUnity.BackEnd.Migrations;
using CommUnity.BackEnd.Repositories.Interfaces;
using CommUnity.BackEnd.UnitsOfWork.Implementations;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using CommUnity.Shared.Enums;
using CommUnity.Shared.Responses;
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
        private readonly IMailHelper _mailHelper;
        private readonly IUsersUnitOfWork _usersUnitOfWork;

        public PqrssController(IGenericUnitOfWork<Pqrs> unitOfWork, IPqrsUnitOfWork pqrsUnitOfWork, IMailHelper mailHelper, IUsersUnitOfWork usersUnitOfWork) : base(unitOfWork)
        {
            _pqrsUnitOfWork = pqrsUnitOfWork;
            _mailHelper = mailHelper;
            _usersUnitOfWork = usersUnitOfWork;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePqrs(PqrsDTO pqrsDTO)
        {
            var action = await _pqrsUnitOfWork.CreatePqrs(User.Identity!.Name!, pqrsDTO);
            if (action.WasSuccess)
            {
                var response = await SendEmailPqrsCreateAsync(action.Result!);
                return Ok(action.Result);
            }
            else
            {
                return BadRequest(action.Message);
            }
        }

        [HttpGet("pqrss")]
        public async Task<IActionResult> GetPqrsByTypeByStatus([FromQuery] PaginationPqrsDTO paginationPqrs)
        {
            var action = await _pqrsUnitOfWork.GetPqrsByTypeByStatus(User.Identity!.Name!, paginationPqrs);
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
        public async Task<IActionResult> GetPqrsRecordsNumber([FromQuery] PaginationPqrsDTO paginationPqrs)
        {
            var action = await _pqrsUnitOfWork.GetPqrsRecordsNumber(User.Identity!.Name!, paginationPqrs);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            else
            {
                return BadRequest(action.Message);
            }
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var response = await _pqrsUnitOfWork.GetAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        [HttpPut("updatePqrs")]
        public async Task<IActionResult> UpdatePqrs(PqrsDTO pqrsDTO)
        {
            var action = await _pqrsUnitOfWork.UpdatePqrs(pqrsDTO);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            else
            {
                return BadRequest(action.Message);
            }
        }

        [HttpPut("updateStatusPqrs")]
        public async Task<IActionResult> UpdateStatusPqrs(PqrsDTO pqrsDTO)
        {
            var action = await _pqrsUnitOfWork.UpdateStatusPqrs(pqrsDTO);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            else
            {
                return BadRequest(action.Message);
            }
        }

        private async Task<ActionResponse<string>> SendEmailPqrsCreateAsync(Pqrs pqrs)
        {
            var user = await _usersUnitOfWork.GetAdminResidentialUnit(pqrs.ResidentialUnitId);

            var emailBody = $@"
            <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f5f5f5;'>
                <div style='max-width: 600px; margin: auto; background: #fff; border-radius: 8px; box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);'>
                    <div style='background-color: #8019fb; color: white; padding: 10px; border-radius: 8px 8px 0 0;'>
                        <h1 style='margin: 0;'>CummUnity - Creación de PQRS</h1>
                    </div>
                    <div style='padding: 20px;'>
                        <h2 style='color: #8019fb;'>Se ha generado la PQRS Nro. {pqrs.Id} desde el apartamento Nro. {pqrs.Apartment!.Number}</h2>
                        <p style='font-size: 16px;'>Con el siguiente contenido:<br><br>
                        <span style='background-color: #e9ecef; padding: 10px; border-radius: 4px; display: inline-block;'>{pqrs.Content}</span></p>
                        <p style='font-size: 16px;'>Ingrese a su bandeja de PQRS para ver el detalle y gestionarla.</p>
                    </div>
                </div>
            </div>";

            return _mailHelper.SendMail($"{user.Result!.FirstName} {user.Result!.LastName}", user.Result!.Email!, "CummUnity - Creación de PQRS", emailBody);
        }

    }
}

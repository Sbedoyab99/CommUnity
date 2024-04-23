using CommUnity.BackEnd.UnitsOfWork.Implementations;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CommUnity.BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApartmentsController : GenericController<Apartment>
    {
        private readonly IApartmentsUnitOfWork _apartmentsUnitOfWork;

        public ApartmentsController(IGenericUnitOfWork<Apartment> unitOfWork, IApartmentsUnitOfWork apartmentsUnitOfWork) : base(unitOfWork)
        {
            _apartmentsUnitOfWork = apartmentsUnitOfWork;
        }

        [HttpGet("full")]
        public override async Task<IActionResult> GetAsync()
        {
            var response = await _apartmentsUnitOfWork.GetAsync();
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _apartmentsUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _apartmentsUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var response = await _apartmentsUnitOfWork.GetAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetFullAsync([FromQuery] int id)
        {
            var response = await _apartmentsUnitOfWork.GetFullAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }
    }
}

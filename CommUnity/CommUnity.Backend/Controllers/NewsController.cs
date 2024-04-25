using CommUnity.BackEnd.UnitsOfWork.Implementations;
using CommUnity.BackEnd.UnitsOfWork.Interfaces;
using CommUnity.Shared.DTOs;
using CommUnity.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CommUnity.BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : GenericController<News>
    {
        private readonly INewsUnitOfWork _NewsUnitOfWork;

        public NewsController(IGenericUnitOfWork<News> unitOfWork, INewsUnitOfWork NewsUnitOfWork) : base(unitOfWork)
        {
            _NewsUnitOfWork = NewsUnitOfWork;
        }

        [HttpGet("full")]
        public override async Task<IActionResult> GetAsync()
        {
            var response = await _NewsUnitOfWork.GetAsync();
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _NewsUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _NewsUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var response = await _NewsUnitOfWork.GetAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetFullAsync([FromQuery] int id)
        {
            var response = await _NewsUnitOfWork.GetFullAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }
    }
}

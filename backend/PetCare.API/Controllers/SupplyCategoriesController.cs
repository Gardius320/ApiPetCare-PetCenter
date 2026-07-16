using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetCare.Application.SupplyCategories.Queries.GetAllCategories;

namespace PetCare.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplyCategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SupplyCategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery());
            return Ok(result);
        }
    }
}
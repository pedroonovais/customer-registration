using Core.CustomerRegistration.Application.Customers;
using Microsoft.AspNetCore.Mvc;
using Shared.CustomerRegistration.Contracts.Customers;

namespace Web.CustomerRegistration.Api.Catalog.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public sealed class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomersController(ICustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CustomerDto>>> GetAll(CancellationToken ct)
            => Ok(await _service.GetAllAsync(ct));

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CustomerDto>> GetById(Guid id, CancellationToken ct)
        {
            var item = await _service.GetByIdAsync(id, ct);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateCustomerRequest req, CancellationToken ct)
        {
            var id = await _service.CreateAsync(req, ct);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }
    }
}

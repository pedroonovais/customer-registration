using Infrastructure.CustomerRegistration.Integration.Clients;
using Microsoft.AspNetCore.Mvc;
using Shared.CustomerRegistration.Contracts.Cep;

namespace Web.Store.Api.Cep.Controllers
{
    [ApiController]
    [Route("api/cep")]
    public sealed class CepController(IViaCepClient client) : ControllerBase
    {
        [HttpGet("{cep}")]
        public async Task<ActionResult<CepResponse>> Get(string cep, CancellationToken ct)
        {
            var res = await client.GetAsync(cep, ct);
            return res is null ? NotFound() : Ok(res);
        }
    }
}
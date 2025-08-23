using Microsoft.AspNetCore.Mvc;
using Shared.CustomerRegistration.Contracts.Cep;
using Shared.CustomerRegistration.Contracts.Customers;

namespace Web.Store.Mvc.Controllers
{
    public sealed class HomeController(IHttpClientFactory httpFactory) : Controller
    {
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var http = httpFactory.CreateClient("CatalogApi");
            var customers = await http.GetFromJsonAsync<List<CustomerDto>>("/api/customers", ct) ?? [];
            return View(customers);
        }

        [HttpPost]
        public async Task<IActionResult> Cep(string cep, CancellationToken ct)
        {
            var http = httpFactory.CreateClient("CepApi");
            var res = await http.GetFromJsonAsync<CepResponse>($"/api/cep/{cep}", ct);
            return View("Cep", res);
        }
    }
}
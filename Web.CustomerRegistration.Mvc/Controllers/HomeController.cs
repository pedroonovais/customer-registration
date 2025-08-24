using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Shared.CustomerRegistration.Contracts.Customers;
using Web.CustomerRegistration.Mvc.Models;

namespace Web.CustomerRegistration.Mvc.Controllers
{
    public sealed class HomeController(IHttpClientFactory httpFactory, ILogger<HomeController> logger) : Controller
    {
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var http = httpFactory.CreateClient("CatalogApi");

            // Mantém o GET robusto para diagnosticar se algo falhar
            var resp = await http.GetAsync("/api/customers", ct);
            var body = await resp.Content.ReadAsStringAsync(ct);

            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = $"CatalogApi GET /api/customers => {(int)resp.StatusCode} {resp.ReasonPhrase}\n{body}";
                return View(new List<CustomerDto>());
            }

            var customers = JsonSerializer.Deserialize<List<CustomerDto>>(
                body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            ) ?? [];

            return View(customers);
        }

        [HttpGet]
        public IActionResult Create() => View(new CreateCustomerViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var http = httpFactory.CreateClient("CatalogApi");
                var req = new CreateCustomerRequest(model.Name, model.Email, model.Occupation);

                var resp = await http.PostAsJsonAsync("/api/customers", req, ct);
                var body = await resp.Content.ReadAsStringAsync(ct);

                if (!resp.IsSuccessStatusCode)
                {
                    // Mostra o erro que veio da API na própria página
                    ModelState.AddModelError(string.Empty,
                        $"Erro da API: {(int)resp.StatusCode} {resp.ReasonPhrase}. " +
                        (string.IsNullOrWhiteSpace(body) ? "" : body));
                    return View(model);
                }

                // tenta ler o Guid; se vier vazio ainda assim segue
                Guid id = Guid.Empty;
                try { id = JsonSerializer.Deserialize<Guid>(body); } catch { /* ok se API não retornar corpo */ }

                TempData["Flash"] = id != Guid.Empty
                    ? $"Cliente criado com sucesso: {id}"
                    : "Cliente criado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Falha no POST de cliente");
                ModelState.AddModelError(string.Empty, $"Falha ao enviar para a API: {ex.Message}");
                return View(model);
            }
        }
    }
}

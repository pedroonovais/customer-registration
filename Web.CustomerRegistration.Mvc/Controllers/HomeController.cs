using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Shared.CustomerRegistration.Contracts.Customers;
using Shared.CustomerRegistration.Contracts.Cep;
using Web.CustomerRegistration.Mvc.Models;

namespace Web.CustomerRegistration.Mvc.Controllers
{
    public sealed class HomeController(IHttpClientFactory httpFactory, ILogger<HomeController> logger) : Controller
    {
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var http = httpFactory.CreateClient("CatalogApi");
            var resp = await http.GetAsync("/api/customers", ct);
            var body = await resp.Content.ReadAsStringAsync(ct);

            if (!resp.IsSuccessStatusCode)
            {
                TempData["Error"] = $"CatalogApi GET /api/customers => {(int)resp.StatusCode} {resp.ReasonPhrase}\n{body}";
                return View(new List<CustomerDto>());
            }

            var customers = JsonSerializer.Deserialize<List<CustomerDto>>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];
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
                    ModelState.AddModelError(string.Empty, $"Erro da API: {(int)resp.StatusCode} {resp.ReasonPhrase}. {body}");
                    return View(model);
                }

                Guid id = Guid.Empty;
                try { id = JsonSerializer.Deserialize<Guid>(body); } catch { /* ok */ }

                TempData["Flash"] = id != Guid.Empty ? $"Cliente criado com sucesso: {id}" : "Cliente criado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Falha no POST de cliente");
                ModelState.AddModelError(string.Empty, $"Falha ao enviar para a API: {ex.Message}");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Cep() => View(model: null);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cep(string cep, CancellationToken ct)
        {
            var digits = new string((cep ?? string.Empty).Where(char.IsDigit).ToArray());
            if (string.IsNullOrWhiteSpace(digits) || digits.Length != 8)
            {
                ModelState.AddModelError(string.Empty, "CEP inválido. Informe 8 dígitos (ex.: 13083-852 → 13083852).");
                return View(model: null);
            }

            var http = httpFactory.CreateClient("CepApi");
            var resp = await http.GetAsync($"/api/cep/{digits}", ct);
            var body = await resp.Content.ReadAsStringAsync(ct);
            ViewBag.RawJson = body;

            if (!resp.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, $"Erro da API de CEP: {(int)resp.StatusCode} {resp.ReasonPhrase}");
                return View(model: null);
            }

            CepResponse? data = null;
            try
            {
                data = JsonSerializer.Deserialize<CepResponse>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                // segue com RawJson no ViewBag
            }

            return View(data);
        }
    }
}

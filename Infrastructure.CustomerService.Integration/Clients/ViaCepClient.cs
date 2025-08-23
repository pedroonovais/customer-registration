using Infrastructure.CustomerRegistration.Integration.Options;
using Microsoft.Extensions.Options;
using Shared.CustomerRegistration.Contracts.Cep;
using System.Net.Http.Json;

namespace Infrastructure.CustomerRegistration.Integration.Clients
{
    public sealed class ViaCepClient(HttpClient http, IOptions<ViaCepOptions> options) : IViaCepClient
    {
        public async Task<CepResponse?> GetAsync(string cep, CancellationToken ct = default)
        {
            cep = new string([.. cep.Where(char.IsDigit)]);
            var url = $"{options.Value.BaseUrl}/ws/{cep}/json";

            var dto = await http.GetFromJsonAsync<CepResponse>(url, ct);
            return dto;
        }
    }
}
using Shared.CustomerRegistration.Contracts.Cep;

namespace Infrastructure.CustomerRegistration.Integration.Clients
{
    public interface IViaCepClient
    {
        Task<CepResponse?> GetAsync(string cep, CancellationToken ct = default);
    }
}
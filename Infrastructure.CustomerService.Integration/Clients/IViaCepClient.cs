using Shared.CustomerRegistration.Contracts.Cep;

namespace Infrastructure.Store.Integration.Clients
{
    public interface IViaCepClient
    {
        Task<CepResponse?> GetAsync(string cep, CancellationToken ct = default);
    }
}
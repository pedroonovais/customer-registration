namespace Shared.CustomerRegistration.Contracts.Cep
{
    public sealed record CepResponse(string Cep, string Logradouro, string Bairro, string Localidade, string Uf);
}
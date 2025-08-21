namespace Shared.CustomerRegistration.Contracts.Customers
{
    public sealed record CustomerDto(Guid Id, string Name, string Email, string Occupation);
}

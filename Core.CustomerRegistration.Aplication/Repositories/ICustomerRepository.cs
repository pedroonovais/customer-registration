using Core.CustomerRegistration.Domain.Entities;

namespace Core.CustomerRegistration.Domain.Repositories
{
    public interface ICustomerRepository
    {
        Task<CustomerEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<CustomerEntity>> GetAllAsync(CancellationToken ct = default);
        Task AddAsync(CustomerEntity entity, CancellationToken ct = default);
        Task UpdateAsync(CustomerEntity entity, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}

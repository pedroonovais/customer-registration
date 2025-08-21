using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.CustomerRegistration.Domain.Entities;
using Core.CustomerRegistration.Domain.Repositories;
using Shared.CustomerRegistration.Contracts.Customers;

namespace Core.CustomerRegistration.Application.Customers
{
    public sealed class CustomerService(ICustomerRepository repo) : ICustomerService
    {
        public async Task<IReadOnlyList<CustomerDto>> GetAllAsync(CancellationToken ct = default)
            => (await repo.GetAllAsync(ct))
                .Select(p => new CustomerDto(p.Id, p.Name, p.Email, p.Occupation))
                .ToList();

        public async Task<CustomerDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var p = await repo.GetByIdAsync(id, ct);
            return p is null ? null : new CustomerDto(p.Id, p.Name, p.Email, p.Occupation);
        }

        public async Task<Guid> CreateAsync(CreateCustomerRequest request, CancellationToken ct = default)
        {
            var entity = new CustomerEntity(request.Name, request.Email, request.Occupation);
            await repo.AddAsync(entity, ct);
            return entity.Id;
        }
    }
}

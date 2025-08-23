using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.CustomerRegistration.Contracts.Customers;

namespace Core.CustomerRegistration.Application.Customers
{
    public interface ICustomerService
    {
        Task<IReadOnlyList<CustomerDto>> GetAllAsync(CancellationToken ct = default);
        Task<CustomerDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Guid> CreateAsync(CreateCustomerRequest request, CancellationToken ct = default);
    }
}

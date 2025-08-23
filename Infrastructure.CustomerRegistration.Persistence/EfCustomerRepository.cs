using Core.CustomerRegistration.Domain.Entities;
using Core.CustomerRegistration.Domain.Repositories;
using Infrastructure.CustomerRegistration.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.CustomerRegistration.Persistence
{
    public sealed class EfCustomerRepository(CustomerRegistrationDbContext db) : ICustomerRepository
    {
        public async Task AddAsync(CustomerEntity entity, CancellationToken ct = default)
        {
            db.Customers.Add(entity);
            await db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await db.Customers.FindAsync([id], ct);
            if (entity is null) return;
            db.Customers.Remove(entity);
            await db.SaveChangesAsync(ct);
        }

        public async Task<IReadOnlyList<CustomerEntity>> GetAllAsync(CancellationToken ct = default)
            => await db.Customers.AsNoTracking().OrderBy(p => p.Name).ToListAsync(ct);

        public async Task<CustomerEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await db.Customers.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);

        public async Task UpdateAsync(CustomerEntity entity, CancellationToken ct = default)
        {
            db.Customers.Update(entity);
            await db.SaveChangesAsync(ct);
        }
    }
}

using AccountManager.Domain.Entities;
using AccountManager.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AccountManager.Repository;

/// <summary>
///     EF Core repository for AccountSubscriptionStatus.
/// </summary>
public class AccountSubscriptionStatusRepository(ApplicationDbContext db) : IAccountSubscriptionStatusRepository
{
    public async Task<List<AccountSubscriptionStatus>> GetAllAsync()
    {
        return await db.AccountSubscriptionStatuses.ToListAsync();
    }

    public async Task<AccountSubscriptionStatus?> GetByIdAsync(int id)
    {
        return await db.AccountSubscriptionStatuses.FindAsync(id);
    }

    public async Task<AccountSubscriptionStatus> CreateAsync(AccountSubscriptionStatus status)
    {
        db.AccountSubscriptionStatuses.Add(status);
        await db.SaveChangesAsync();
        return status;
    }

    public async Task<AccountSubscriptionStatus> UpdateAsync(AccountSubscriptionStatus status)
    {
        db.AccountSubscriptionStatuses.Update(status);
        await db.SaveChangesAsync();
        return status;
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await db.AccountSubscriptionStatuses.FindAsync(id);
        if (existing != null)
        {
            db.AccountSubscriptionStatuses.Remove(existing);
            await db.SaveChangesAsync();
        }
    }
}
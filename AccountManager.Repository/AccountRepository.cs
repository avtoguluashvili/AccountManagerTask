using AccountManager.Domain.Entities;
using AccountManager.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AccountManager.Repository;

/// <summary>
///     EF Core repository for Account.
/// </summary>
public class AccountRepository(ApplicationDbContext db) : IAccountRepository
{
    public async Task<List<Account>> GetAllAsync()
    {
        return await db.Accounts
            .AsNoTracking()
            .Include(a => a.AccountSubscription)
            .ThenInclude(s => s!.Subscription)
            .Include(a => a.AccountSubscription!.AccountSubscriptionStatus)
            .ToListAsync();
    }

    public async Task<Account?> GetByIdAsync(int id)
    {
        return await db.Accounts
            .AsNoTracking()
            .Include(a => a.AccountSubscription)
            .ThenInclude(s => s!.Subscription)
            .Include(a => a.AccountSubscription!.AccountSubscriptionStatus)
            .FirstOrDefaultAsync(a => a.AccountId == id);
    }

    public async Task<Account> CreateAsync(Account entity)
    {
        db.Accounts.Add(entity);
        await db.SaveChangesAsync();
        return entity;
    }

    public async Task<Account> UpdateAsync(Account entity)
    {
        db.Accounts.Attach(entity);

        db.Entry(entity).State = EntityState.Modified;

        if (entity.AccountSubscription is not null)
        {
            db.AccountSubscriptions.Attach(entity.AccountSubscription);
            db.Entry(entity.AccountSubscription).State = EntityState.Modified;
        }

        await db.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await db.Accounts.FindAsync(id);
        if (existing != null)
        {
            db.Accounts.Remove(existing);
            await db.SaveChangesAsync();
        }
    }

    public async Task<List<Account>> SearchAsync(string keyword)
    {
        return await db.Accounts
            .AsNoTracking()
            .Where(a => a.CompanyName.Contains(keyword))
            .Include(a => a.AccountSubscription)
            .ToListAsync();
    }
}
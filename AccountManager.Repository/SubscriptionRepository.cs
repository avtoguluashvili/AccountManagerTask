using AccountManager.Domain.Entities;
using AccountManager.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AccountManager.Repository;

public class SubscriptionRepository(ApplicationDbContext db) : ISubscriptionRepository
{
    public async Task<List<Subscription>> GetAllAsync()
    {
        return await db.Subscriptions
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Subscription?> GetByIdAsync(int id)
    {
        return await db.Subscriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.SubscriptionId == id);
    }

    public async Task<Subscription> CreateAsync(Subscription subscription)
    {
        db.Subscriptions.Add(subscription);
        await db.SaveChangesAsync();
        return subscription;
    }

    public async Task<Subscription> UpdateAsync(Subscription subscription)
    {
        db.Subscriptions.Attach(subscription);
        db.Entry(subscription).State = EntityState.Modified;

        await db.SaveChangesAsync();
        return subscription;
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await db.Subscriptions.FindAsync(id);
        if (existing != null)
        {
            db.Subscriptions.Remove(existing);
            await db.SaveChangesAsync();
        }
    }
}
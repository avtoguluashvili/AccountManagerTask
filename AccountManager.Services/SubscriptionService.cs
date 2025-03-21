using AccountManager.Domain.Entities;
using AccountManager.Interfaces.Repositories;
using AccountManager.Interfaces.Services;

namespace AccountManager.Services;

/// <summary>
///     Implements Subscription logic.
/// </summary>
public class SubscriptionService(ISubscriptionRepository repo) : ISubscriptionService
{
    public async Task<List<Subscription>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<Subscription?> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id);
    }

    public async Task<Subscription> CreateAsync(Subscription subscription)
    {
        return await repo.CreateAsync(subscription);
    }

    public async Task<Subscription> UpdateAsync(Subscription subscription)
    {
        return await repo.UpdateAsync(subscription);
    }

    public async Task DeleteAsync(int id)
    {
        await repo.DeleteAsync(id);
    }
}
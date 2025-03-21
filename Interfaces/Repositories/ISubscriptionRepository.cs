using AccountManager.Domain.Entities;

namespace AccountManager.Interfaces.Repositories;

/// <summary>
///     CRUD operations for Subscription plans.
/// </summary>
public interface ISubscriptionRepository
{
    Task<List<Subscription>> GetAllAsync();
    Task<Subscription?> GetByIdAsync(int id);
    Task<Subscription> CreateAsync(Subscription subscription);
    Task<Subscription> UpdateAsync(Subscription subscription);
    Task DeleteAsync(int id);
}
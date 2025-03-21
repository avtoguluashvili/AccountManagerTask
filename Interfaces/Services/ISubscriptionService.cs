using AccountManager.Domain.Entities;

namespace AccountManager.Interfaces.Services;

/// <summary>
///     Business logic for Subscription plans.
/// </summary>
public interface ISubscriptionService
{
    Task<List<Subscription>> GetAllAsync();
    Task<Subscription?> GetByIdAsync(int id);
    Task<Subscription> CreateAsync(Subscription subscription);
    Task<Subscription> UpdateAsync(Subscription subscription);
    Task DeleteAsync(int id);
}
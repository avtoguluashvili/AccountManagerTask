using AccountManager.Domain.Entities;
using AccountManager.Interfaces.Repositories;
using AccountManager.Interfaces.Services;

namespace AccountManager.Services;

/// <summary>
///     Implements subscription status logic.
/// </summary>
public class AccountSubscriptionStatusService(IAccountSubscriptionStatusRepository repo)
    : IAccountSubscriptionStatusService
{
    public async Task DeleteAsync(int id)
    {
        await repo.DeleteAsync(id);
    }

    public async Task<List<AccountSubscriptionStatus>> GetAllAsync()
    {
        return await repo.GetAllAsync();
    }

    public async Task<AccountSubscriptionStatus?> GetByIdAsync(int id)
    {
        return await repo.GetByIdAsync(id);
    }

    public async Task<AccountSubscriptionStatus> CreateAsync(AccountSubscriptionStatus status)
    {
        return await repo.CreateAsync(status);
    }

    public async Task<AccountSubscriptionStatus> UpdateAsync(AccountSubscriptionStatus status)
    {
        return await repo.UpdateAsync(status);
    }
}
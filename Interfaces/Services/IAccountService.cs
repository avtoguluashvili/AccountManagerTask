using AccountManager.Domain.Entities;

namespace AccountManager.Interfaces.Services;

/// <summary>
///     Business logic for managing Account and subscription changes.
/// </summary>
public interface IAccountService
{
    Task<List<Account>> GetAllAsync();
    Task<Account?> GetByIdAsync(int id);
    Task<Account> CreateAsync(Account newAccount, int subscriptionId);
    Task<Account> UpdateAsync(Account updatedAccount);
    Task DeleteAsync(int accountId);
    Task<List<Account>> SearchAsync(string keyword);
}
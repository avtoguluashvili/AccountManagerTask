using AccountManager.Domain.Entities;

namespace AccountManager.Interfaces.Services;

/// <summary>
///     Business logic for account changes log.
/// </summary>
public interface IAccountChangesLogService
{
    Task<List<AccountChangesLog>> GetAllAsync();
    Task<List<AccountChangesLog>> GetByAccountAsync(int accountId);
    Task<AccountChangesLog> CreateAsync(AccountChangesLog log);
    Task DeleteAsync(int logId);
}
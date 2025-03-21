using AccountManager.Domain.Entities;

namespace AccountManager.Interfaces.Repositories;

/// <summary>
///     CRUD operations for AccountChangesLog.
/// </summary>
public interface IAccountChangesLogRepository
{
    Task<List<AccountChangesLog>> GetAllAsync();
    Task<List<AccountChangesLog>> GetByAccountAsync(int accountId);
    Task<AccountChangesLog> CreateAsync(AccountChangesLog log);
    Task<AccountChangesLog?> GetByIdAsync(int logId);
    Task DeleteAsync(int logId);
}
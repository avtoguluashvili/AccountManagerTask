using AccountManager.Domain.Entities;
using AccountManager.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AccountManager.Repository;

/// <summary>
///     EF Core repository for AccountChangesLog.
/// </summary>
public class AccountChangesLogRepository(ApplicationDbContext db) : IAccountChangesLogRepository
{
    public async Task<List<AccountChangesLog>> GetAllAsync()
    {
        return await db.AccountChangesLogs.OrderByDescending(l => l.Timestamp).ToListAsync();
    }

    public async Task<List<AccountChangesLog>> GetByAccountAsync(int accountId)
    {
        return await db.AccountChangesLogs
            .Where(l => l.AccountId == accountId)
            .OrderByDescending(l => l.Timestamp)
            .ToListAsync();
    }

    public async Task<AccountChangesLog> CreateAsync(AccountChangesLog log)
    {
        db.AccountChangesLogs.Add(log);
        await db.SaveChangesAsync();
        return log;
    }

    public async Task<AccountChangesLog?> GetByIdAsync(int logId)
    {
        return await db.AccountChangesLogs.FindAsync(logId);
    }

    public async Task DeleteAsync(int logId)
    {
        var existing = await db.AccountChangesLogs.FindAsync(logId);
        if (existing != null)
        {
            db.AccountChangesLogs.Remove(existing);
            await db.SaveChangesAsync();
        }
    }
}
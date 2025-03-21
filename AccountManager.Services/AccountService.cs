using AccountManager.Domain.Entities;
using AccountManager.Interfaces.Repositories;
using AccountManager.Interfaces.Services;

namespace AccountManager.Services;

/// <summary>
///     Implements Account business logic.
/// </summary>
public class AccountService(
    IAccountRepository accountRepo,
    ISubscriptionRepository subRepo,
    IAccountChangesLogRepository logRepo)
    : IAccountService
{
    public async Task<List<Account>> GetAllAsync()
    {
        return await accountRepo.GetAllAsync();
    }

    public async Task<Account?> GetByIdAsync(int id)
    {
        return await accountRepo.GetByIdAsync(id);
    }

    public async Task<Account> CreateAsync(Account newAccount, int subscriptionId)
    {
        var existingAccount = await accountRepo.GetByIdAsync(newAccount.AccountId);
        if (existingAccount != null)
            throw new InvalidOperationException("Account already exists.");

        var sub = await subRepo.GetByIdAsync(subscriptionId);
        if (sub == null)
            throw new InvalidOperationException("Invalid subscription.");

        var accSub = new AccountSubscription
        {
            SubscriptionId = sub.SubscriptionId,
            Is2FaAllowed = sub.Is2FaAllowed,
            IsIpFilterAllowed = sub.IsIpFilterAllowed,
            IsSessionTimeoutAllowed = sub.IsSessionTimeoutAllowed,
            SubscriptionStatusId = 1
        };
        newAccount.AccountSubscription = accSub;
        var created = await accountRepo.CreateAsync(newAccount);
        await logRepo.CreateAsync(new AccountChangesLog
        {
            AccountId = created.AccountId,
            ChangedField = "AccountCreated",
            OldValue = "",
            NewValue = created.CompanyName
        });
        return created;
    }


    public async Task<Account> UpdateAsync(Account updatedAccount)
    {
        var existing = await accountRepo.GetByIdAsync(updatedAccount.AccountId);
        if (existing == null)
            throw new InvalidOperationException("Account not found.");

        if (existing.CompanyName != updatedAccount.CompanyName)
            await logRepo.CreateAsync(new AccountChangesLog
            {
                AccountId = updatedAccount.AccountId,
                ChangedField = "CompanyName",
                OldValue = existing.CompanyName,
                NewValue = updatedAccount.CompanyName
            });
        if (existing.IsActive != updatedAccount.IsActive)
            await logRepo.CreateAsync(new AccountChangesLog
            {
                AccountId = updatedAccount.AccountId,
                ChangedField = "IsActive",
                OldValue = existing.IsActive.ToString(),
                NewValue = updatedAccount.IsActive.ToString()
            });
        var saved = await accountRepo.UpdateAsync(updatedAccount);
        return saved;
    }


    public async Task DeleteAsync(int accountId)
    {
        await accountRepo.DeleteAsync(accountId);
        await logRepo.CreateAsync(new AccountChangesLog
        {
            AccountId = accountId,
            ChangedField = "AccountDeleted",
            OldValue = "",
            NewValue = ""
        });
    }

    public async Task<List<Account>> SearchAsync(string keyword)
    {
        return await accountRepo.SearchAsync(keyword);
    }
}
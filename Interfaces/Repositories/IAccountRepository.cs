using AccountManager.Domain.Entities;

namespace AccountManager.Interfaces.Repositories;

public interface IAccountRepository
{
    Task<List<Account>> GetAllAsync();
    Task<Account?> GetByIdAsync(int id);
    Task<Account> CreateAsync(Account entity);
    Task<Account> UpdateAsync(Account entity);
    Task DeleteAsync(int id);
    Task<List<Account>> SearchAsync(string keyword);
}
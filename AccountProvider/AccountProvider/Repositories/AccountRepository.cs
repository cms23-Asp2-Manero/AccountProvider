using AccountProvider.Data.Contexts;
using AccountProvider.Data.Entities;
using AccountProvider.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AccountProvider.Repositories;
public class AccountRepository(Context context) : IAccountRepository
{
    private readonly Context _context = context;

    public async Task<AccountEntity> CreateAsync(AccountEntity entity)
    {
        AccountEntity addedStateEntity = _context.Accounts.Add(entity).Entity;
        await _context.SaveChangesAsync();
        return addedStateEntity;
    }
    public async Task<IEnumerable<AccountEntity>> GetAllAsync()
    {
        IEnumerable<AccountEntity> accounts = await _context.Accounts.ToListAsync();
        return accounts;
    }
    public async Task<AccountEntity> GetOneAsync(string id)
    {
        AccountEntity? account = await _context.Accounts.FirstOrDefaultAsync(x => x.UserId == id);
        return account!;
    }   
    public async Task<AccountEntity> UpdateAsync(AccountEntity entity)
    {
        AccountEntity modifiedStateEntity = _context.Update(entity).Entity;
        await _context.SaveChangesAsync();
        return modifiedStateEntity;
    }
    public async Task<bool> DeleteAsync(string id)
    {
        AccountEntity? account = await _context.Accounts.FirstOrDefaultAsync(x => x.UserId == id);
        if (account != null)
        {
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
    public async Task<bool> ExistsAsync(Expression<Func<AccountEntity, bool>> predicate)
    {
        return await _context.Accounts.AnyAsync(predicate);
    }
}

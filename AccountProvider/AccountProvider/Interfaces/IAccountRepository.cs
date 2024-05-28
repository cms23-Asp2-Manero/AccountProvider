using AccountProvider.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AccountProvider.Interfaces
{
    public interface IAccountRepository
    {
        Task<AccountEntity> CreateAsync(AccountEntity entity);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<AccountEntity>> GetAllAsync();
        Task<AccountEntity> GetOneAsync(string id);
        Task<AccountEntity> GetByEmailAsync(string email);
        Task<AccountEntity> UpdateAsync(AccountEntity entity);
        Task<bool> ExistsAsync(Expression<Func<AccountEntity, bool>> predicate);
    }
}

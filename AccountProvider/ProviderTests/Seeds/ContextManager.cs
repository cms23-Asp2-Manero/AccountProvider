using AccountProvider.Data.Contexts;
using AccountProvider.Data.Entities;
using AccountProvider.Functions;
using Microsoft.EntityFrameworkCore;

namespace ProviderTests.Seeds;

internal class ContextManager(Context context)
{
    private readonly Context _context = context;

    internal IEnumerable<AccountEntity> Seed(int count)
    {
        IEnumerable<AccountEntity> entities = [];
        for (int i = 1; i <= count; i++)
        {
            AccountEntity entity = new()
            {
                UserId = $"{i}",
                FirstName = "Firstname",
                LastName = "Lastname",
                Email = $"Test{i}",
                PhoneNumber = "404"
            };
            var addedEntity = _context.Accounts.Add(entity).Entity;
            entities = entities.Append(addedEntity);
        }
        _context.SaveChanges();
        context.ChangeTracker.Clear();
        return entities;
    }

    internal IEnumerable<AccountEntity> GetAll()
    {
        return _context.Accounts.ToList();
    }

    internal void Cleanup()
    {
        _context.ChangeTracker.Clear();
        _context.Database.EnsureDeletedAsync();
    }
}

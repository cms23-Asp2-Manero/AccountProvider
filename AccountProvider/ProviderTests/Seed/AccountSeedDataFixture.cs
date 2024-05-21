using AccountProvider.Data.Contexts;
using AccountProvider.Data.Entities;

namespace ProviderTests.Seed;

public class AccountSeedDataFixture : IDisposable
{
    public InMemoryAccountContext Context { get; private set; } = new InMemoryAccountContext();

    public void Seed()
    {
        Context.Accounts.Add(new AccountEntity { UserId = "1", FirstName = "Firstname1", LastName = "Lastname1", Email = "Email1" });
        Context.Accounts.Add(new AccountEntity { UserId = "2", FirstName = "Firstname2", LastName = "Lastname2", Email = "Email2" });
        Context.Accounts.Add(new AccountEntity { UserId = "3", FirstName = "Firstname3", LastName = "Lastname3", Email = "Email3" });
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}

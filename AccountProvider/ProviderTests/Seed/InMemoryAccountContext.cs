using AccountProvider.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ProviderTests.Seed;

public class InMemoryAccountContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "BookingDb");
    }

    public DbSet<AccountEntity> Accounts { get; set; }
}

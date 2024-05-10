using AccountProvider.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountProvider.Data.contexts;

public class Context(DbContextOptions<Context> options) : DbContext(options)
{
    public DbSet<AccountEntity> Accounts { get; set; }
}

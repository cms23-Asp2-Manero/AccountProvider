using AccountProvider.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountProvider.Data.Contexts;

public class Context(DbContextOptions<Context> options) : DbContext(options)
{
    public DbSet<AccountEntity> Accounts { get; set; }
}


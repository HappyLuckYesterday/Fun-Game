using Microsoft.EntityFrameworkCore;
using Rhisis.Infrastructure.Persistance.Entities;

namespace Rhisis.Infrastructure.Persistance.Contexts;

public sealed class AccountDbContext : BaseDbContext<AccountDbContext>, IAccountDatabase
{
    public DbSet<AccountEntity> Accounts => Set<AccountEntity>();

    public AccountDbContext()
    {
    }

    public AccountDbContext(DbContextOptions options)
        : base(options)
    {
    }
}

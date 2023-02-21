using Microsoft.EntityFrameworkCore;
using Rhisis.Infrastructure.Persistance.Entities;
using System.Data.Common;
using System.Reflection;

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

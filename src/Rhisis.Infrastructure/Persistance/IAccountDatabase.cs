using Microsoft.EntityFrameworkCore;
using Rhisis.Infrastructure.Persistance.Entities;
using System;

namespace Rhisis.Infrastructure.Persistance;

public interface IAccountDatabase : IDisposable
{
    DbSet<AccountEntity> Accounts { get; }

    void Migrate();

    void SaveChanges();
}

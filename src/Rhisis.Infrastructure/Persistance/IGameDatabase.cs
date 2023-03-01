using Microsoft.EntityFrameworkCore;
using Rhisis.Infrastructure.Persistance.Entities;
using System;

namespace Rhisis.Infrastructure.Persistance;

public interface IGameDatabase : IDisposable
{
    DbSet<PlayerEntity> Players { get; }

    DbSet<ItemEntity> Items { get; }

    void Migrate();

    int SaveChanges();
}

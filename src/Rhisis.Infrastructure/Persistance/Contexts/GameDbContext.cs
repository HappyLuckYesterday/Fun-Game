using Microsoft.EntityFrameworkCore;
using Rhisis.Infrastructure.Persistance.Entities;

namespace Rhisis.Infrastructure.Persistance.Contexts;

public sealed class GameDbContext : BaseDbContext<GameDbContext>, IGameDatabase
{
    public DbSet<PlayerEntity> Players => Set<PlayerEntity>();

    public DbSet<ItemEntity> Items => Set<ItemEntity>();

    public GameDbContext()
    {
    }

    public GameDbContext(DbContextOptions<GameDbContext> options) 
        : base(options)
    {
    }
}
using Microsoft.EntityFrameworkCore;
using Rhisis.Infrastructure.Persistance.Entities;

namespace Rhisis.Infrastructure.Persistance.Contexts;

public sealed class GameDbContext : BaseDbContext<GameDbContext>, IGameDatabase
{
    public DbSet<PlayerEntity> Players => Set<PlayerEntity>();

    public GameDbContext()
    {
    }

    public GameDbContext(DbContextOptions options) 
        : base(options)
    {
    }
}
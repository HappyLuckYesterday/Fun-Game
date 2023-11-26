using Microsoft.EntityFrameworkCore;
using Rhisis.Infrastructure.Persistance.Entities;

namespace Rhisis.Infrastructure.Persistance.Contexts;

public sealed class GameDbContext : BaseDbContext<GameDbContext>, IGameDatabase
{
    public DbSet<PlayerEntity> Players => Set<PlayerEntity>();

    public DbSet<ItemEntity> Items => Set<ItemEntity>();

    public DbSet<PlayerItemEntity> PlayerItems => Set<PlayerItemEntity>();

    public DbSet<PlayerSkillEntity> PlayerSkills => Set<PlayerSkillEntity>();

    public DbSet<PlayerSkillBuffEntity> PlayerSkillBuffs => Set<PlayerSkillBuffEntity>();

    public DbSet<PlayerSkillBuffAttributeEntity> PlayerSkillBuffAttributes => Set<PlayerSkillBuffAttributeEntity>();

    public DbSet<PlayerQuestEntity> PlayerQuests => Set<PlayerQuestEntity>();

    public GameDbContext()
    {
    }

    public GameDbContext(DbContextOptions<GameDbContext> options) 
        : base(options)
    {
    }
}
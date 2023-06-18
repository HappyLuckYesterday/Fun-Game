using Microsoft.EntityFrameworkCore;
using Rhisis.Infrastructure.Persistance.Entities;
using System;

namespace Rhisis.Infrastructure.Persistance;

public interface IGameDatabase : IDisposable
{
    DbSet<PlayerEntity> Players { get; }

    DbSet<ItemEntity> Items { get; }

    DbSet<PlayerItemEntity> PlayerItems { get; }

    DbSet<PlayerSkillEntity> PlayerSkills { get; }

    DbSet<PlayerSkillBuffEntity> PlayerSkillBuffs { get; }

    DbSet<PlayerSkillBuffAttributeEntity> PlayerSkillBuffAttributes { get; }

    DbSet<PlayerQuestEntity> PlayerQuests { get; }

    void Migrate();

    int SaveChanges();
}

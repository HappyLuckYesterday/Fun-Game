﻿using Microsoft.EntityFrameworkCore;
using Rhisis.Infrastructure.Persistance.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rhisis.Infrastructure.Persistance
{
    /// <summary>
    /// Provides a mechanism to access Rhisis database.
    /// </summary>
    public interface IRhisisDatabase : IDisposable
    {
        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        DbSet<DbUser> Users { get; }

        /// <summary>
        /// Gets or sets the characters.
        /// </summary>
        DbSet<DbCharacter> Characters { get; }

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        DbSet<DbAttribute> Attributes { get; }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        DbSet<DbItem> Items { get; }

        /// <summary>
        /// Gets or sets item attributes.
        /// </summary>
        DbSet<DbItemAttributes> ItemAttributes { get; }

        /// <summary>
        /// Gets or sets the item storage.
        /// </summary>
        DbSet<DbItemStorage> ItemStorage { get; }

        /// <summary>
        /// Gets or sets the item storage types.
        /// </summary>
        DbSet<DbItemStorageType> ItemStorageTypes { get; }

        /// <summary>
        /// Gets or sets the mails.
        /// </summary>
        DbSet<DbMail> Mails { get; }

        /// <summary>
        /// Gets or sets the taskbar shortcuts.
        /// </summary>
        DbSet<DbShortcut> TaskbarShortcuts { get; }

        /// <summary>
        /// Gets or sets the quests.
        /// </summary>
        DbSet<DbQuest> Quests { get;}

        /// <summary>
        /// Gets or sets the skills.
        /// </summary>
        DbSet<DbSkill> Skills { get; }

        /// <summary>
        /// Gets the skill buffs.
        /// </summary>
        DbSet<DbSkillBuff> SkillBuffs { get; }

        /// <summary>
        /// Gets the skill buff attributes.
        /// </summary>
        DbSet<DbSkillBuffAttribute> SkillBuffAttributes { get; }

        /// <summary>
        /// Gets the friends.
        /// </summary>
        DbSet<DbFriend> Friends { get; }

        /// <summary>
        /// Creates a Microsoft.EntityFrameworkCore.DbSet`1 that can be used to query and save instances of TEntity.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        /// Saves the database pending changes.
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// Saves the database pending changes asynchronously.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Migrates the database schema.
        /// </summary>
        void Migrate();

        /// <summary>
        /// Check if the database exists.
        /// </summary>
        /// <returns></returns>
        bool Exists();

        /// <summary>
        /// CHecks if the database is alive.
        /// </summary>
        /// <returns>True if the database connection is alive; false otherwise.</returns>
        bool IsAlive();
    }
}

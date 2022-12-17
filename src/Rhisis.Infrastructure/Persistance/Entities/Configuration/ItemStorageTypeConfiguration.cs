﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rhisis.Game.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Infrastructure.Persistance.Entities.Configuration;

public class ItemStorageTypeConfiguration : IEntityTypeConfiguration<DbItemStorageType>
{
    public void Configure(EntityTypeBuilder<DbItemStorageType> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(20).HasColumnType("VARCHAR(20)");

        SeedData(builder);
    }

    private void SeedData(EntityTypeBuilder<DbItemStorageType> builder)
    {
        IEnumerable<DbAttribute> initialValues = Enum.GetValues(typeof(ItemStorageType))
               .Cast<ItemStorageType>()
               .OrderBy(x => (int)x)
               .Select(x => new DbAttribute
               {
                   Id = (int)x,
                   Name = x.ToString()
               });

        builder.HasData(initialValues);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rhisis.Infrastructure.Persistance.Entities;
using System;

namespace Rhisis.Infrastructure.Persistance.Sqlite.Configuration.Account;

public sealed class AccountEntityConfiguration : IEntityTypeConfiguration<AccountEntity>
{
    public void Configure(EntityTypeBuilder<AccountEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired().HasColumnType("TEXT");
        builder.Property(x => x.Username).IsRequired().HasColumnType("TEXT").HasMaxLength(32);
        builder.Property(x => x.Password).IsRequired().HasColumnType("TEXT").HasMaxLength(32);
        builder.Property(x => x.Authority).IsRequired().HasDefaultValue(80);
        builder.Property(x => x.IsValid).IsRequired().HasDefaultValue(false);
        builder.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false);
        builder.Property(x => x.Created).IsRequired().HasDefaultValueSql("DATE()");
        builder.Property(x => x.LastConnectionTime).IsRequired(false);
    }
}

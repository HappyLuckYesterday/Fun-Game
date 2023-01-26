using System;

namespace Rhisis.Infrastructure.Persistance.Entities;

public sealed class PlayerEntity
{
    public Guid Id { get; set; }

    public Guid AccountId { get; set; }

    public string Name { get; set; }

    public int Level { get; set; }
}

using System;

namespace Rhisis.Infrastructure.Persistance.Entities;

public sealed class PlayerEntity
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public string Name { get; set; }

    public int Level { get; set; }
}

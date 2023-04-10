using System;

namespace Rhisis.Infrastructure.Persistance.Entities;

public sealed class AccountEntity
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public int Authority { get; set; }

    public bool IsValid { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime Created { get; set; }

    public DateTime? LastConnectionTime { get; set; }
}

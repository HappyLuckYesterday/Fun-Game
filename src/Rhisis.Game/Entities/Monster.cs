using Rhisis.Game.Resources.Properties;

namespace Rhisis.Game.Entities;

public sealed class Monster : Mover
{
    public Monster(MoverProperties properties) 
        : base(properties)
    {
        Name = properties.Name;
    }
}
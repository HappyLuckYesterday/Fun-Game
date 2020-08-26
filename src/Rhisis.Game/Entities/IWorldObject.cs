using Rhisis.Core.Common;
using System;

namespace Rhisis.Game.Entities
{
    public interface IWorldObject
    {
        int Id { get; }

        WorldEntityType Type { get; }

        int ModelId { get; }

        int MapId { get; }

        int MapLayerId { get; }

        Point3D Position { get; }

        float Angle { get; }

        float Size { get; }

        string Name { get; }

        IServiceProvider Systems { get; }
    }
}

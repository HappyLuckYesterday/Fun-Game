using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.Game.Common;
using Rhisis.Game.Protocol.Packets.World.Server.Snapshots;
using Rhisis.Game.Resources.Properties;

namespace Rhisis.Game.Entities;

public sealed class Monster : Mover
{
    private long _nextMoveTime;

    public int RespawnTime { get; init; }

    public Rectangle Region { get; init; }

    public Monster(MoverProperties properties) 
        : base(properties)
    {
        Name = properties.Name;
    }

    public void Update()
    {
        if (IsDead || !IsSpawned)
        {
            return;
        }

        if (IsFighting)
        {
            // TODO
        }
        else
        {
            if (ObjectState.HasFlag(ObjectState.OBJSTA_STAND) && _nextMoveTime < Time.TimeInSeconds())
            {
                Vector3 randomPosition = Region.GetRandomPosition();

                while (Position.GetDistance2D(randomPosition) > 10f)
                {
                    randomPosition = Region.GetRandomPosition();
                }

                if (Properties.IsFlying)
                {
                    randomPosition.Y = Map.GetHeight(randomPosition.X, randomPosition.Z) + FFRandom.Random(0, 6);
                }

                Move(randomPosition.X, randomPosition.Y, randomPosition.Z);
            }
        }

        UpdateMoves();
    }

    protected override void OnArrived()
    {
        _nextMoveTime = Time.TimeInSeconds() + FFRandom.LongRandom(5, 10);

        if (SpeedFactor > 2)
        {
            SetSpeedFactor(1);
        }
    }

    private void SetSpeedFactor(float speedFactor)
    {
        SpeedFactor = speedFactor;

        using SetSpeedFactorSnapshot snapshot = new(this, speedFactor);
        SendToVisible(snapshot);
    }
}
﻿using Rhisis.Abstractions.Entities;

namespace Rhisis.Protocol.Snapshots
{
    public class SetStatisticsStateSnapshot : FFSnapshot
    {
        public SetStatisticsStateSnapshot(IPlayer player)
            : base(SnapshotType.SETSTATE, player.Id)
        {
            Write(player.Statistics.Strength);
            Write(player.Statistics.Stamina);
            Write(player.Statistics.Dexterity);
            Write(player.Statistics.Intelligence);
            Write(0);
            Write<uint>(player.Statistics.AvailablePoints);
        }
    }
}

using Microsoft.Extensions.Logging;
using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System;

namespace Rhisis.WorldServer.Handlers;

[PacketHandler(PacketType.SNAPSHOT)]
internal sealed class SnapshotHandler : WorldPacketHandler
{
    private readonly ILogger<SnapshotHandler> _logger;

    public SnapshotHandler(ILogger<SnapshotHandler> logger)
    {
        _logger = logger;
    }

    public void Execute(SnapshotPacket packet)
    {
        int snapshotCount = packet.Count;

        while (snapshotCount > 0)
        {
            using FFPacket snapshot = new(packet.Data, ignorePacketHeader: true);
            short snapshotHeaderNumber = snapshot.ReadInt16();
            try
            {
                SnapshotType snapshotHeader = (SnapshotType)snapshotHeaderNumber;

                if (snapshotHeader == SnapshotType.DESTPOS)
                {
                    HandleDestPostSnapshot(new SetDestPositionPacket(snapshot));
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            catch (NotImplementedException)
            {

                if (Enum.IsDefined(typeof(SnapshotType), snapshotHeaderNumber))
                    _logger.LogWarning("Received an unimplemented World snapshot {0} (0x{1}).", Enum.GetName(typeof(SnapshotType), snapshotHeaderNumber), snapshotHeaderNumber.ToString("X4"));
                else
                    _logger.LogWarning("[SECURITY] Received an unknown World snapshot 0x{0}.", snapshotHeaderNumber.ToString("X4"));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occured while handling a world snapshot.");
            }
            finally
            {
                snapshotCount--;
            }
        }
    }

    private void HandleDestPostSnapshot(SetDestPositionPacket snapshot)
    {
        Player.Move(snapshot.X, snapshot.Y, snapshot.Z);
    }
}


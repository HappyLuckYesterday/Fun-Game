using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rhisis.Core.Helpers;
using Rhisis.Game;
using Rhisis.Infrastructure.Persistance;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets;
using Rhisis.Protocol.Packets.Cluster.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhisis.ClusterServer;

public sealed class ClusterUser : FFUserConnection
{
    private readonly IGameDatabase _gameDatabase;
    private readonly IServiceProvider _serviceProvider;
    private int _loginProtectValue = FFRandom.Random(0, 100);

    public int AccountId { get; set; }

    public ClusterUser(ILogger<ClusterUser> logger, IGameDatabase gameDatabase, IServiceProvider serviceProvider) 
        : base(logger)
    {
        _gameDatabase = gameDatabase;
        _serviceProvider = serviceProvider;
    }

    public override Task HandleMessageAsync(byte[] packetBuffer)
    {
        if (Socket is null)
        {
            Logger.LogTrace("Skip to handle login packet. Reason: client is not connected.");
            return Task.CompletedTask;
        }

        try
        {
            // We must skip the first 4 bytes because it represents the DPID which is always 0xFFFFFFFF (uint.MaxValue)
            byte[] packetBufferArray = packetBuffer.Skip(4).ToArray();
            FFPacket packet = new(packetBufferArray);
            PacketDispatcher.Execute(this, packet.Header, packet, _serviceProvider);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "An error occured while handling a cluster packet.");
        }

        return base.HandleMessageAsync(packetBuffer);
    }

    public void SendPlayerList()
    {
        const int authenticationKey = 0;
        IEnumerable<SelectableCharacter> characters = GetCharacterList();

        using var playerListPacket = new PlayerListPacket(authenticationKey, characters);

        Send(playerListPacket);
    }

    public void SendChannelIpAddress(string channelIp)
    {
        using CacheAddressPacket packet = new(channelIp);

        Send(packet);
    }

    public void SendLoginProtect()
    {
        using LoginProctectNumPadPacket packet = new(_loginProtectValue);

        Send(packet);
    }

    public void SendError(ErrorType errorType)
    {
        using ErrorPacket packet = new(errorType);

        Send(packet);
    }

    private IReadOnlyList<SelectableCharacter> GetCharacterList()
    {
        return _gameDatabase.Players
            .Include(x => x.Items.Where(x => x.StorageType == PlayerItemStorageType.Inventory))
                .ThenInclude(x => x.Item)
            .Where(x => x.AccountId == AccountId && !x.IsDeleted)
            .ToList()
            .Select(x => new SelectableCharacter
            {
                Id = x.Id,
                Gender = (GenderType)x.Gender,
                Level = x.Level,
                Slot = x.Slot,
                MapId = x.MapId,
                PositionX = x.PosX,
                PositionY = x.PosY,
                PositionZ = x.PosZ,
                SkinSetId = x.SkinSetId,
                HairId = x.HairId,
                HairColor = (uint)x.HairColor,
                FaceId = x.FaceId,
                JobId = x.JobId,
                Strength = x.Strength,
                Stamina = x.Stamina,
                Intelligence = x.Intelligence,
                Dexterity = x.Dexterity,
                EquipedItems = x.Items.Where(i => i.Slot > 42).OrderBy(i => i.Slot).Select(i => i.Item.Id)
            })
            .ToList();
    }
}

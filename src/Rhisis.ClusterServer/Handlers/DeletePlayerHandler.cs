using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rhisis.Infrastructure.Persistance;
using Rhisis.Infrastructure.Persistance.Entities;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using Rhisis.Protocol.Packets.Cluster.Client;
using System.Linq;

namespace Rhisis.ClusterServer.Handlers;

[PacketHandler(PacketType.DEL_PLAYER)]
internal class DeletePlayerHandler : ClusterHandlerBase, IPacketHandler
{
    private readonly ILogger<DeletePlayerHandler> _logger;
    private readonly IAccountDatabase _accountDatabase;
    private readonly IGameDatabase _gameDatabase;

    public DeletePlayerHandler(ILogger<DeletePlayerHandler> logger, IAccountDatabase accountDatabase, IGameDatabase gameDatabase)
    {
        _logger = logger;
        _accountDatabase = accountDatabase;
        _gameDatabase = gameDatabase;
    }

    public void Execute(DeletePlayerPacket packet)
    {
        if (packet.Password != packet.PasswordConfirmation)
        {
            _logger.LogError($"Unable to verify user account '{packet.Username}'. Reason: passwords doesn't match.");
            User.Disconnect();
            return;
        }

        AccountEntity userAccount = _accountDatabase.Accounts.SingleOrDefault(x => x.Username == packet.Username && x.Password == packet.Password && x.Id == User.AccountId);

        if (userAccount is null)
        {
            _logger.LogWarning($"Unable to create new character for user '{packet.Username}' Reason: bad presented credentials compared to the database.");
            User.Disconnect();

            return;
        }

        PlayerEntity playerToDelete = _gameDatabase.Players.SingleOrDefault(x => x.Id == packet.CharacterId);

        if (playerToDelete is null)
        {
            _logger.LogWarning($"Unable to delete character id '{packet.CharacterId}' for user '{packet.Username}'. Reason: user doesn't have any character with this id.");
            User.Disconnect();
            return;
        }

        if (playerToDelete.IsDeleted)
        {
            _logger.LogWarning($"Unable to delete character id '{packet.CharacterId}' for user '{packet.Username}'. Reason: character is already deleted.");
            User.Disconnect();
            return;
        }

        playerToDelete.IsDeleted = true;

        IQueryable<ItemEntity> playerItems = _gameDatabase.PlayerItems
            .Include(x => x.Item)
            .Where(x => x.PlayerId == playerToDelete.Id)
            .Select(x => x.Item);

        foreach (ItemEntity item in playerItems)
        {
            item.IsDeleted = true;
        }

        _gameDatabase.SaveChanges();
        User.SendPlayerList();

        _logger.LogInformation($"Character '{playerToDelete.Name}' has been deleted successfully for user '{userAccount.Username}' (ID='{userAccount.Id}').");
    }
}

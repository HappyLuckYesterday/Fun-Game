using Microsoft.Extensions.Logging;
using Rhisis.ClusterServer.Abstractions;
using Rhisis.Infrastructure.Persistance;
using Rhisis.Infrastructure.Persistance.Entities;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets.Client.Cluster;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Linq;

namespace Rhisis.ClusterServer.Handlers;

[Handler]
public class DeletePlayerHandler : ClusterHandlerBase
{
    private readonly ILogger<DeletePlayerHandler> _logger;

    public DeletePlayerHandler(ILogger<DeletePlayerHandler> logger, IRhisisDatabase database)
        : base(database)
    {
        _logger = logger;
    }

    [HandlerAction(PacketType.DEL_PLAYER)]
    public void Execute(IClusterUser user, DeletePlayerPacket packet)
    {
        DbUser dbUser = Database.Users.FirstOrDefault(x => x.Username == packet.Username && x.Password == packet.Password);

        if (dbUser is null)
        {
            _logger.LogWarning($"[SECURITY] Unable to create new character for user '{packet.Username}'. " +
                "Reason: bad presented credentials compared to the database.");
            user.Disconnect();
            return;
        }

        if (!string.Equals(packet.Password, packet.PasswordConfirmation, StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning($"Unable to delete character id '{packet.CharacterId}' for user '{packet.Username}'. " +
                "Reason: passwords entered do not match.");
            SendError(user, ErrorType.WRONG_PASSWORD);

            return;
        }

        DbCharacter characterToDelete = Database.Characters.FirstOrDefault(x => x.Id == packet.CharacterId);

        // Check if character exist.
        if (characterToDelete is null)
        {
            _logger.LogWarning($"[SECURITY] Unable to delete character id '{packet.CharacterId}' for user '{packet.Username}'. " +
                "Reason: user doesn't have any character with this id.");
            user.Disconnect();
            return;
        }

        if (characterToDelete.IsDeleted)
        {
            _logger.LogWarning($"[SECURITY] Unable to delete character id '{packet.CharacterId}' for user '{packet.Username}'. " +
                   "Reason: character is already deleted.");
            return;
        }

        characterToDelete.IsDeleted = true;

        Database.Characters.Update(characterToDelete);
        Database.SaveChanges();

        _logger.LogInformation($"Character '{characterToDelete.Name}' has been deleted successfully for user '{packet.Username}'.");

        SendPlayerList(user, packet.AuthenticationKey);
    }
}

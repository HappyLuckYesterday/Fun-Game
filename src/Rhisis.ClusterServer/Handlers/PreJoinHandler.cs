using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Rhisis.ClusterServer.Abstractions;
using Rhisis.Core.Structures.Configuration;
using Rhisis.Infrastructure.Persistance;
using Rhisis.Infrastructure.Persistance.Entities;
using Rhisis.Protocol;
using Rhisis.Protocol.Packets.Client.Cluster;
using Rhisis.Protocol.Packets.Server.Cluster;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Linq;

namespace Rhisis.ClusterServer.Handlers
{
    [Handler]
    public class PreJoinHandler : ClusterHandlerBase
    {
        private readonly ILogger<PreJoinHandler> _logger;
        private readonly IOptions<ClusterConfiguration> _clusterOptions;

        public PreJoinHandler(ILogger<PreJoinHandler> logger, IOptions<ClusterConfiguration> clusterOptions, IRhisisDatabase database)
            : base(database)
        {
            _logger = logger;
            _clusterOptions = clusterOptions;
        }

        [HandlerAction(PacketType.PRE_JOIN)]
        public void OnPreJoin(IClusterUser user, PreJoinPacket packet)
        {
            DbCharacter character = Database.Characters.FirstOrDefault(x => x.Id == packet.CharacterId);

            if (character is null)
            {
                _logger.LogWarning($"[SECURITY] Unable to prejoin character id '{packet.CharacterName}' for user '{packet.Username}'. " +
                      $"Reason: no character with id {packet.CharacterId}.");
                user.Disconnect();
                return;
            }

            if (character.IsDeleted)
            {
                _logger.LogWarning($"[SECURITY] Unable to prejoin with character '{character.Name}' for user '{packet.Username}'. " +
                    "Reason: character is deleted.");
                user.Disconnect();
                return;
            }

            if (character.Name != packet.CharacterName)
            {
                _logger.LogWarning($"[SECURITY] Unable to prejoin character '{character.Name}' for user '{packet.Username}'. " +
                    "Reason: character is not owned by this user.");
                user.Disconnect();
                return;
            }

            if (_clusterOptions.Value.EnableLoginProtect &&
                LoginProtect.GetNumPadToPassword(user.LoginProtectValue, packet.BankCode) != character.BankCode)
            {
                _logger.LogWarning($"Unable to prejoin character '{character.Name}' for user '{packet.Username}'. " +
                    "Reason: bad bank code.");
                user.LoginProtectValue = new Random().Next(0, 1000);
                
                using var loginProtectPacket = new LoginProtectCertPacket(user.LoginProtectValue);
                user.Send(loginProtectPacket);

                return;
            }

            using var prejoinPacket = new PreJoinPacketComplete();
            user.Send(prejoinPacket);

            _logger.LogInformation($"Character '{character.Name}' has prejoin successfully the game for user '{packet.Username}'.");
        }
    }
}

using Microsoft.Extensions.Logging;
using Rhisis.Game;
using Rhisis.Game.Entities;
using Rhisis.Game.Protocol.Packets.World.Client;
using Rhisis.Game.Protocol.Packets.World.Server;
using Rhisis.Game.Protocol.Packets.World.Server.Snapshots;
using Rhisis.Game.Resources;
using Rhisis.Infrastructure.Persistance;
using Rhisis.Infrastructure.Persistance.Entities;
using Rhisis.Protocol;
using Rhisis.Protocol.Handlers;
using System.Linq;

namespace Rhisis.WorldServer.Handlers;

[PacketHandler(PacketType.JOIN)]
internal class JoinGameHandler : WorldPacketHandler
{
    private readonly ILogger<JoinGameHandler> _logger;
    private readonly IAccountDatabase _accountDatabase;
    private readonly IGameDatabase _gameDatabase;

    public JoinGameHandler(ILogger<JoinGameHandler> logger, IAccountDatabase accountDatabase, IGameDatabase gameDatabase)
    {
        _logger = logger;
        _accountDatabase = accountDatabase;
        _gameDatabase = gameDatabase;
    }

    public void Execute(JoinPacket packet)
    {
        AccountEntity userAccount = _accountDatabase.Accounts.SingleOrDefault(x => x.Username == packet.Username && x.Password == packet.Password);

        if (userAccount is null)
        {
            _logger.LogWarning($"Unable to join for user '{packet.Username}' Reason: bad presented credentials compared to the database.");
            User.Disconnect();

            return;
        }

        PlayerEntity player = _gameDatabase.Players.SingleOrDefault(x => x.AccountId == userAccount.Id && x.Id == packet.PlayerId && x.Name == packet.PlayerName);

        if (player is null)
        {
            _logger.LogWarning($"Unable to join for user '{packet.Username}' Reason: Cannot find player with id: '{packet.PlayerId}' and name: '{packet.PlayerName}'.");
            User.Disconnect();

            return;
        }

        if (player.IsDeleted)
        {
            _logger.LogWarning($"Unable to join for user '{packet.Username}' Reason: player '{player.Name}' is deleted.");
            User.Disconnect();

            return;
        }

        int modelId = player.Gender == 0 ? 11 : 12;

        User.Player = new Player(User, GameResources.Current.Movers.Get(modelId))
        {
            Id = player.Id,
            Name = player.Name,
            Slot = player.Slot,
            DeathLevel = 0,
            Authority = (AuthorityType)userAccount.Authority,
            Position = new Vector3(player.PosX, player.PosY, player.PosZ),
            MapId = player.MapId,
            MapLayerId = player.MapLayerId,
            RotationAngle = player.Angle,
            Level = player.Level,
            ModelId = modelId,
            ObjectState = ObjectState.OBJSTA_STAND,
            Job = GameResources.Current.Jobs.Get(player.JobId),
            AvailablePoints = player.StatPoints,
            SkillPoints = (ushort)player.SkillPoints,
            Appearence = new HumanVisualAppearance
            {
                Gender = player.Gender == 0 ? GenderType.Male : GenderType.Female,
                SkinSetId = player.SkinSetId,
                FaceId = player.FaceId,
                HairColor = player.HairColor,
                HairId = player.HairId,
            }
        };
        User.Player.Health.Hp = player.Hp;
        User.Player.Health.Mp = player.Mp;
        User.Player.Health.Fp = player.Fp;

        User.Player.Statistics.Strength = player.Strength;
        User.Player.Statistics.Stamina = player.Stamina;
        User.Player.Statistics.Dexterity = player.Dexterity;
        User.Player.Statistics.Intelligence = player.Intelligence;

        User.Player.Gold.Initialize(player.Gold);
        User.Player.Experience.Initialize(player.Experience);
        // TODO: initialize inventory items
        // TODO: initialize skills
        // TODO: initialize quest diary

        using (JoinCompletePacket joinPacket = new())
        {
            joinPacket.AddSnapshots(
                new EnvironmentAllSnapshot(User.Player, SeasonType.None), // TODO: get the season id using current weather time.
                new WorldReadInfoSnapshot(User.Player),
                new AddObjectSnapshot(User.Player)
                //new TaskbarSnapshot(User.Player)
                //new QueryPlayerDataSnapshot(cachedPlayer),
                //new AddFriendGameJoinSnapshot(User.Player)
            );

            User.Send(joinPacket);
        }

        User.Player.IsSpawned = true;
    }
}
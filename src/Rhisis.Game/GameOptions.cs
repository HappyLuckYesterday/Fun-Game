using Rhisis.Core;
using Rhisis.Core.Configuration;
using Rhisis.Core.Configuration.Cluster;

namespace Rhisis.Game;

public sealed class GameOptions : Singleton<GameOptions>
{
    public bool DeathPenalityEnabled { get; set; }

    public RateOptions Rates { get; set; }

    public MessengerOptions Messenger { get; set; }

    public CustomizationOptions Customization { get; set; }

    public DropOptions Drops { get; set; }

    public DefaultCharacterSection DefaultCharacter { get; set; }
}
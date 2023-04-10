using Rhisis.Core;
using Rhisis.Core.Configuration;

namespace Rhisis.Game;

public sealed class GameOptions : Singleton<GameOptions>
{
    public bool DeathPenalityEnabled { get; set; }

    public RateOptions Rates { get; set; }

    public MessengerOptions Messenger { get; set; }

    public CustomizationOptions Customization { get; set; }
}
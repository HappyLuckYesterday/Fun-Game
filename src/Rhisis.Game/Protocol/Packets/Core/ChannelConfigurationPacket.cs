using Rhisis.Core.Configuration;
using Rhisis.Core.Configuration.Cluster;

namespace Rhisis.Game.Protocol.Packets.Core;

public sealed record ChannelConfigurationPacket(
    RateOptions Rates, 
    MessengerOptions MessengerOptions, 
    CustomizationOptions CustomizationOptions, 
    DropOptions DropOptions,
    DefaultCharacterSection DefaultCharacterOptions,
    string[] Maps);
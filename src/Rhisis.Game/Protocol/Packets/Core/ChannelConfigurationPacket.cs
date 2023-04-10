using Rhisis.Core.Configuration;

namespace Rhisis.Game.Protocol.Packets.Core;

public sealed record ChannelConfigurationPacket(
    RateOptions Rates, 
    MessengerOptions MessengerOptions, 
    CustomizationOptions CustomizationOptions, 
    string[] Maps);
using Microsoft.Extensions.Options;
using Rhisis.Core.Structures.Configuration.World;
using Rhisis.Game.Abstractions;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Rhisis.Network.Snapshots;
using Sylver.HandlerInvoker.Attributes;
using System;
using System.Drawing;

namespace Rhisis.WorldServer.Handlers.Customization
{
    [Handler]
    public class SetHairHandler
    {
        private readonly WorldConfiguration _worldServerConfiguration;

        public SetHairHandler(IOptions<WorldConfiguration> worldServerConfiguration)
        {
            _worldServerConfiguration = worldServerConfiguration.Value;
        }

        /// <summary>
        /// Changes the hair and color of a player.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="packet"></param>
        [HandlerAction(PacketType.SET_HAIR)]
        public void OnSetHair(IPlayer player, SetHairPacket packet)
        {
            var color = Color.FromArgb(packet.R, packet.G, packet.B);
            
            if (packet.UseCoupon)
            {
                IItem coupon = player.Inventory.GetItem(x => x.Id == DefineItem.II_SYS_SYS_SCR_HAIRCHANGE);

                if (coupon == null)
                {
                    using var snapshot = new DefinedTextSnapshot(player, DefineText.TID_GAME_WARNNING_COUPON);
                    player.Send(snapshot);

                    throw new InvalidOperationException($"Failed to find coupon to change face.");
                }

                player.Inventory.DeleteItem(coupon, 1);
            }
            else
            {
                int costs = 0;

                if (player.Appearence.HairId != packet.HairId)
                {
                    costs += (int)_worldServerConfiguration.Customization.ChangeHairCost;
                }

                if (player.Appearence.HairColor != color.ToArgb())
                {
                    costs += (int)_worldServerConfiguration.Customization.ChangeHairColorCost;
                }

                if (player.Gold.Amount < costs)
                {
                    using var snapshot = new DefinedTextSnapshot(player, DefineText.TID_GAME_LACKMONEY);
                    player.Send(snapshot);

                    return;
                }

                player.Appearence.HairId = packet.HairId;
                player.Appearence.HairColor = color.ToArgb();

                player.Gold.Decrease(costs);
            }

            using var changeHairSnapshot = new SetHairSnapshot(player, (byte)player.Appearence.HairId, color.R, color.G, color.B);

            player.Send(changeHairSnapshot);
            player.SendToVisible(changeHairSnapshot);
        }
    }
}

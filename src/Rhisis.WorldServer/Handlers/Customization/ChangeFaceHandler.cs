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

namespace Rhisis.WorldServer.Handlers.Customization
{
    [Handler]
    public class ChangeFaceHandler
    {
        private readonly WorldConfiguration _worldServerConfiguration;

        public ChangeFaceHandler(IOptions<WorldConfiguration> worldServerConfiguration)
        {
            _worldServerConfiguration = worldServerConfiguration.Value;
        }

        /// <summary>
        /// Changes the face of a player.
        /// </summary>
        /// <param name="serverClient"></param>
        /// <param name="packet"></param>
        [HandlerAction(PacketType.CHANGEFACE)]
        public void OnChangeFace(IPlayer player, ChangeFacePacket packet)
        {
            if (packet.UseCoupon)
            {
                IItem coupon = player.Inventory.GetItem(x => x.Id == DefineItem.II_SYS_SYS_SCR_FACEOFFFREE);

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
                if (player.Gold.Amount < _worldServerConfiguration.Customization.ChangeFaceCost)
                {
                    using var snapshot = new DefinedTextSnapshot(player, DefineText.TID_GAME_LACKMONEY);
                    player.Send(snapshot);

                    return;
                }

                player.Gold.Decrease((int)_worldServerConfiguration.Customization.ChangeFaceCost);
            }

            player.Appearence.FaceId = (int)packet.FaceNumber;

            using var changeFaceSnapshot = new ChangeFaceSnapshot(player, packet.FaceNumber);
            player.Send(changeFaceSnapshot);
            player.SendToVisible(changeFaceSnapshot);
        }
    }
}

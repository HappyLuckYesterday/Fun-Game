using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.Game.Common;
using Rhisis.Game.Protocol.Packets.World.Server.Snapshots;
using Rhisis.Game.Resources;
using Rhisis.Game.Resources.Properties;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace Rhisis.Game.Entities;

[DebuggerDisplay("{Name}")]
public sealed class Npc : WorldObject
{
    private static float OralTextRadisu = 50f;

    private long _lastSpeakTime;

    public override WorldObjectType Type => WorldObjectType.Mover;

    public NpcProperties Properties { get; }

    public bool HasShop => Shop is not null;

    public ItemContainer[] Shop { get; }

    public bool HasDialog => Properties.HasDialog;
 
    public Npc(NpcProperties properties)
    {
        Properties = properties;
        Name = Properties.Id;

        if (Properties.HasShop)
        {
            Shop = new ItemContainer[Properties.Shop.Items.Length];
            for (int i = 0; i < Properties.Shop.Items.Length; i++)
            {
                Shop[i] = new ItemContainer(100);
                IEnumerable<ItemContainerSlot> items = Properties.Shop.Items[i].Select((x, index) =>
                {
                    ItemProperties itemProperties = GameResources.Current.Items.Get(x.Id);

                    return new ItemContainerSlot
                    {
                        Index = index,
                        Slot = index,
                        Item = new Item(itemProperties)
                        {
                            Refine = x.Refine,
                            Element = x.Element,
                            ElementRefine = x.ElementRefine,
                            Quantity = itemProperties.PackMax
                        }
                    };
                });

                Shop[i].Initialize(items);
            }
        }
    }

    /// <summary>
    /// Opens the NPC shop to the given player.
    /// </summary>
    /// <param name="target">Target player.</param>
    public void OpenShop(Player target)
    {
        if (!HasShop)
        {
            return;
        }

        using OpenNpcShopWindowSnapshot packet = new(this, Shop);

        target.Send(packet);
    }

    public void Update()
    {
        if (!IsSpawned)
        {
            return;
        }

        Speak();
    }

    /// <summary>
    /// Speaks to given text to the player.
    /// </summary>
    /// <param name="text">Text to speak.</param>
    /// <param name="player">Player.</param>
    public void Speak(string text, Player player)
    {
        using ChatSnapshot packet = new(this, text);

        player.Send(packet);
    }

    private void Speak()
    {
        if (Properties.Dialog is not null && !string.IsNullOrEmpty(Properties.Dialog.OralText))
        {
            if (_lastSpeakTime <= Time.TimeInSeconds())
            {
                IEnumerable<Player> playersAround = VisibleObjects
                    .Where(x => x is Player && Position.IsInCircle(x.Position, OralTextRadisu))
                    .Cast<Player>();

                if (playersAround.Any())
                {
                    foreach (Player player in playersAround)
                    {
                        Speak(Properties.Dialog.OralText, player);
                    }
                }

                _lastSpeakTime = Time.TimeInSeconds() + FFRandom.Random(10, 15);
            }
        }
    }
}
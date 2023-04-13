using Rhisis.Game.Resources.Properties.Dialogs;
using System.Collections.Generic;

namespace Rhisis.Game.Resources.Properties;

public class NpcProperties
{
    /// <summary>
    /// Gets or sets the NPC id.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the NPC name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the NPC model id.
    /// </summary>
    public int ModelId { get; set; }

    /// <summary>
    /// Gets or sets the NPC hair id.
    /// </summary>
    public int HairId { get; set; }

    /// <summary>
    /// Gets or sets the NPC hair color.
    /// </summary>
    public int HairColor { get; set; }

    /// <summary>
    /// Gets or sets the NPC face id.
    /// </summary>
    public int FaceId { get; set; }

    /// <summary>
    /// Gets or sets the NPC viewable models.
    /// </summary>
    public ICollection<int> Items { get; set; }

    /// <summary>
    /// Gets the NPC Shop.
    /// </summary>
    public ShopProperties Shop { get; }

    /// <summary>
    /// Gets a value that indicates if the NPC has a shop.
    /// </summary>
    public bool HasShop => Shop != null;

    /// <summary>
    /// Gets the NPC Dialog.
    /// </summary>
    public DialogProperties Dialog { get; }

    /// <summary>
    /// Gets a value that indicates if the NPC has a dialog.
    /// </summary>
    public bool HasDialog => Dialog != null;

    /// <summary>
    /// Gets or sets a value that indicates if the NPC can buff.
    /// </summary>
    public bool CanBuff { get; set; }

    /// <summary>
    /// Creates a new <see cref="NpcProperties"/> instance.
    /// </summary>
    /// <param name="id">Npc id</param>
    /// <param name="name">Npc Name</param>
    public NpcProperties(string id, string name)
        : this(id, name, null, null)
    {
    }

    /// <summary>
    /// Creates a new <see cref="NpcProperties"/> instance.
    /// </summary>
    /// <param name="id">Npc id</param>
    /// <param name="name">Npc Name</param>
    /// <param name="shop">Npc Shop</param>
    public NpcProperties(string id, string name, ShopProperties shop)
        : this(id, name, shop, null)
    {
    }

    /// <summary>
    /// Creates a new <see cref="NpcProperties"/> instance.
    /// </summary>
    /// <param name="id">Npc id</param>
    /// <param name="name">Npc Name</param>
    /// <param name="shop">Npc Shop</param>
    /// <param name="dialog">Npc dialog</param>
    public NpcProperties(string id, string name, ShopProperties shop, DialogProperties dialog)
    {
        Id = id;
        Name = name;
        Shop = shop;
        Dialog = dialog;
    }
}

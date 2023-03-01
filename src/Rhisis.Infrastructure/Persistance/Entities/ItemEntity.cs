namespace Rhisis.Infrastructure.Persistance.Entities;

public class ItemEntity
{
    /// <summary>
    /// Gets or sets the item's unique id (Serial Number).
    /// </summary>
    public int SerialNumber { get; set; }

    /// <summary>
    /// Gets or sets the item id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the owner id.
    /// </summary>
    public int OwnerId { get; set; }

    /// <summary>
    /// Gets or sets the item refine value.
    /// </summary>
    public byte? Refine { get; set; }

    /// <summary>
    /// Gets or sets the item element.
    /// </summary>
    public byte? Element { get; set; }

    /// <summary>
    /// Gets or sets the item element refine value.
    /// </summary>
    public byte? ElementRefine { get; set; }

    /// <summary>
    /// Gets or sets a boolean value that indicates whether the item is deleted or not.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets the item owner.
    /// </summary>
    public PlayerEntity Owner { get; set; }
}
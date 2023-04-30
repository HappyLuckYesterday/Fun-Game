using Microsoft.Extensions.Logging;
using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using Rhisis.Game.Resources;
using Rhisis.Game.Resources.Properties;
using System;

namespace Rhisis.Game.Chat.Commands;

[ChatCommand("/createitem", AuthorityType.Administrator)]
[ChatCommand("/ci", AuthorityType.Administrator)]
[ChatCommand("/item", AuthorityType.Administrator)]
internal sealed class CreateItemCommand : IChatCommand
{
    private readonly ILogger<CreateItemCommand> _logger;

    public CreateItemCommand(ILogger<CreateItemCommand> logger)
    {
        _logger = logger;
    }

    public void Execute(Player player, object[] parameters)
    {
        _logger.LogTrace($"{player.Name} wants to create an item");

        if (parameters.Length <= 0)
        {
            throw new ArgumentException($"Create item command must have at least one parameter.", nameof(parameters));
        }

        int quantity = parameters.Length >= 2 ? Convert.ToInt32(parameters[1]) : 1;
        byte refine = parameters.Length >= 3 ? Convert.ToByte(parameters[2]) : (byte)0;
        ElementType element = parameters.Length >= 4 ? (ElementType)Enum.Parse(typeof(ElementType), parameters[3].ToString(), true) : default;
        byte elementRefine = parameters.Length >= 5 ? Convert.ToByte(parameters[4]) : (byte)0;

        string itemInput = parameters[0].ToString();
        ItemProperties itemProperties;

        if (!int.TryParse(itemInput, out int itemId))
        {
            itemProperties = GameResources.Current.Items.Get(itemInput);
        }
        else
        {
            itemProperties = GameResources.Current.Items.Get(itemId);
        }

        if (itemProperties is null)
        {
            throw new ArgumentException($"Cannot find item '{itemInput}'.");
        }

        Item item = new(itemProperties)
        {
            CreatorId = player.Id,
            Refine = refine,
            Element = element,
            ElementRefine = elementRefine,
            Quantity = quantity
        };

        player.Inventory.CreateItem(item);
    }
}
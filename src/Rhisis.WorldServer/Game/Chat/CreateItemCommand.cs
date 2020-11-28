using Microsoft.Extensions.Logging;
using Rhisis.Game;
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Features.Chat;
using Rhisis.Game.Abstractions.Resources;
using Rhisis.Game.Common;
using Rhisis.Game.Common.Resources;
using System;
using System.Linq;

namespace Rhisis.WorldServer.Game.Chat
{
    [ChatCommand("/createitem", AuthorityType.Administrator)]
    [ChatCommand("/ci", AuthorityType.Administrator)]
    [ChatCommand("/item", AuthorityType.Administrator)]
    public class CreateItemChatCommand : IChatCommand
    {
        private readonly ILogger<CreateItemChatCommand> _logger;
        private readonly IGameResources _gameResources;

        /// <summary>
        /// Creates a new <see cref="CreateItemChatCommand"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="gameResources">Game resources.</param>
        public CreateItemChatCommand(ILogger<CreateItemChatCommand> logger, IGameResources gameResources)
        {
            _logger = logger;
            _gameResources = gameResources;
        }

        /// <inheritdoc />
        public void Execute(IPlayer player, object[] parameters)
        {
            _logger.LogTrace($"{player.Name} wants to create an item");

            if (parameters.Length <= 0)
            {
                throw new ArgumentException($"Create item command must have at least one parameter.", nameof(parameters));
            }

            var quantity = parameters.Length >= 2 ? Convert.ToInt32(parameters[1]) : 1;
            var refine = parameters.Length >= 3 ? Convert.ToByte(parameters[2]) : (byte)0;
            ElementType element = parameters.Length >= 4 ? (ElementType)Enum.Parse(typeof(ElementType), parameters[3].ToString(), true) : default;
            var elementRefine = parameters.Length >= 5 ? Convert.ToByte(parameters[4]) : (byte)0;

            var itemInput = parameters[0].ToString();
            ItemData itemData;

            if (!int.TryParse(itemInput, out var itemId))
            {
                itemData = _gameResources.Items.Values.FirstOrDefault(x => x.Name.Equals(itemInput, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                _gameResources.Items.TryGetValue(itemId, out itemData);
            }

            if (itemData == null)
            {
                throw new ArgumentException($"Cannot find item '{itemInput}'.");
            }

            var item = new Item(itemData)
            {
                CreatorId = player.CharacterId,
                Refine = refine,
                Element = element,
                ElementRefine = elementRefine,
                Quantity = quantity
            };

            player.Inventory.CreateItem(item, quantity, player.CharacterId);
        }
    }
}

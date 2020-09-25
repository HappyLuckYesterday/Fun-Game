using Microsoft.Extensions.Logging;
using Rhisis.Network;
using Rhisis.Network.Packets.World;
using Rhisis.World.Client;
using Rhisis.World.Systems.Inventory;
using Rhisis.World.Systems.PlayerData;
using Sylver.HandlerInvoker.Attributes;

namespace Rhisis.World.Handlers
{
    /// <summary>
    /// Handles all inventory packets.
    /// </summary>
    [Handler]
    public class InventoryHandler
    {
        private readonly ILogger<InventoryHandler> _logger;
        private readonly IInventorySystem _inventorySystem;
        private readonly IPlayerDataSystem _playerDataSystem;

        /// <summary>
        /// Creates a new <see cref="InventoryHandler"/> instance.
        /// </summary>
        /// <param name="logger">Logger.</param>
        /// <param name="inventorySystem">Inventory System.</param>
        /// <param name="playerDataSystem">Player data system.</param>
        public InventoryHandler(ILogger<InventoryHandler> logger, IInventorySystem inventorySystem, IPlayerDataSystem playerDataSystem)
        {
            _logger = logger;
            _inventorySystem = inventorySystem;
            _playerDataSystem = playerDataSystem;
        }

        /// <summary>
        /// Handles the drop item request.
        /// </summary>
        /// <param name="serverClient"></param>
        /// <param name="packet"></param>
        [HandlerAction(PacketType.DROPITEM)]
        public void OnDropItem(IWorldServerClient serverClient, DropItemPacket packet)
        {
            _inventorySystem.DropItem(serverClient.Player, packet.ItemUniqueId, packet.ItemQuantity);
        }

        /// <summary>
        /// Handles the delete item request.
        /// </summary>
        /// <param name="serverClient"></param>
        /// <param name="packet"></param>
        [HandlerAction(PacketType.REMOVEINVENITEM)]
        public void OnDeleteItem(IWorldServerClient serverClient, RemoveInventoryItemPacket packet)
        {
            _inventorySystem.DeleteItem(serverClient.Player, packet.ItemUniqueId, packet.ItemQuantity);
        }
    }
}

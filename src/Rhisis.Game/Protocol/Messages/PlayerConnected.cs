using Rhisis.Game.Common;

namespace Rhisis.Game.Protocol.Messages
{
    public class PlayerConnected
    {
        public int Id { get; set; }

        public MessengerStatusType Status { get; set; }
    }
}

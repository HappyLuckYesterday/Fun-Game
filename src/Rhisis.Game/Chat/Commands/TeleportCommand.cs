using Rhisis.Game.Common;
using Rhisis.Game.Entities;
using System;

namespace Rhisis.Game.Chat.Commands;

[ChatCommand("/tp", AuthorityType.GameMaster)]
[ChatCommand("/teleport", AuthorityType.GameMaster)]
internal sealed class TeleportCommand : IChatCommand
{
    public void Execute(Player player, object[] parameters)
    {
        int destinationMapId = player.Map.Id;
        var destinationPosition = new Vector3();

        switch (parameters.Length)
        {
            case 2:
                destinationPosition.X = Convert.ToSingle(parameters[0]);
                destinationPosition.Z = Convert.ToSingle(parameters[1]);
                break;
            case 3:
                destinationMapId = Convert.ToInt32(parameters[0]);
                destinationPosition.X = Convert.ToSingle(parameters[1]);
                destinationPosition.Z = Convert.ToSingle(parameters[2]);
                break;
            case 4:
                destinationMapId = Convert.ToInt32(parameters[0]);
                destinationPosition.X = Convert.ToSingle(parameters[1]);
                destinationPosition.Y = Convert.ToSingle(parameters[2]);
                destinationPosition.Z = Convert.ToSingle(parameters[3]);
                break;
            default:
                throw new ArgumentException("Too many or not enough arguments.");
        }

        player.Teleport(destinationMapId, destinationPosition);
    }
}
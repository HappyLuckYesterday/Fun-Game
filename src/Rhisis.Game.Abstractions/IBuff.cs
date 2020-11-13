using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Abstractions.Protocol;
using Rhisis.Game.Common;
using System;
using System.Collections.Generic;

namespace Rhisis.Game.Abstractions
{
    public interface IBuff : IPacketSerializer, IEquatable<IBuff>
    {
        /// <summary>
        /// Gets the buff id.
        /// </summary>
        uint Id { get; }

        /// <summary>
        /// Gets the buff type.
        /// </summary>
        BuffType Type { get; }

        /// <summary>
        /// Gets the buff owner.
        /// </summary>
        IMover Owner { get; }

        /// <summary>
        /// Gets the buff remaining time.
        /// </summary>
        int RemainingTime { get; set; }

        /// <summary>
        /// Gets a boolean value that indicates if the buff has expired.
        /// </summary>
        bool HasExpired { get; }

        /// <summary>
        /// Gets the buff bonus attributes.
        /// </summary>
        IReadOnlyDictionary<DefineAttributes, int> Attributes { get; }

        /// <summary>
        /// Decreases the buff time.
        /// </summary>
        /// <param name="time">Time to decrease in seconds.</param>
        void DecreaseTime(int time = 1);
    }
}

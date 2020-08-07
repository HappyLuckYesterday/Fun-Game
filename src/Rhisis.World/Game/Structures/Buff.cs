using Rhisis.Core.Data;
using Rhisis.Core.Helpers;
using Rhisis.Core.IO;
using Rhisis.Network.Packets;
using Sylver.Network.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Rhisis.World.Game.Structures
{
    public class Buff : IEquatable<Buff>, IPacketSerializer
    {
        /// <summary>
        /// Gets the buff type.
        /// </summary>
        public virtual BuffType Type { get; }

        /// <summary>
        /// Gets the buff unique id.
        /// </summary>
        public uint Id { get; }

        /// <summary>
        /// Gets the buff remaining time.
        /// </summary>
        public int RemainingTime { get; set; }

        /// <summary>
        /// Gets a boolean value that indicates if the buff has expired.
        /// </summary>
        public bool HasExpired => RemainingTime <= 0;

        /// <summary>
        /// Gets the buff bonus attributes.
        /// </summary>
        public IReadOnlyDictionary<DefineAttributes, int> Attributes { get; private set; }

        /// <summary>
        /// Creates a new <see cref="Buff"/> instance.
        /// </summary>
        /// <param name="remainingTime"></param>
        /// <param name="attributes">Bonus attributes.</param>
        public Buff(int remainingTime, IDictionary<DefineAttributes, int> attributes)
        {
            Id = RandomHelper.GenerateUniqueId();
            RemainingTime = remainingTime;
            Attributes = new Dictionary<DefineAttributes, int>(attributes);
        }

        /// <summary>
        /// Decreases the buff time.
        /// </summary>
        /// <param name="time">Time to decrease in seconds.</param>
        public void DecreaseTime(int time = 1)
        {
            RemainingTime -= time * 1000;
        }

        /// <summary>
        /// Compares two <see cref="Buff"/> instances.
        /// </summary>
        /// <param name="other">Other buff instance.</param>
        /// <returns>True if the buffs are the same; false otherwise.</returns>
        public virtual bool Equals([AllowNull] Buff other) => Id == other?.Id;

        public virtual void Serialize(INetPacketStream packet)
        {
            // Nothing to do.
        }
    }
}
﻿using Rhisis.Core.Structures;

namespace Rhisis.Game.Abstractions.Map
{
    /// <summary>
    /// Defines the behavior of a revival region.
    /// </summary>
    public interface IMapRevivalRegion : IMapRegion
    {
        /// <summary>
        /// Gets the revival region's map id.
        /// </summary>
        int MapId { get; }

        /// <summary>
        /// Gets the revival region's map key.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Gets a value that indicates if the region is a revival region for player that has killed other players.
        /// Related to the PK Mode. (Chao mode)
        /// </summary>
        bool IsChaoRegion { get; }

        /// <summary>
        /// Gets a value that indicates if the current region has to target another revival key.
        /// </summary>
        bool TargetRevivalKey { get; }

        /// <summary>
        /// Gets the revival position.
        /// </summary>
        Vector3 RevivalPosition { get; }
    }
}

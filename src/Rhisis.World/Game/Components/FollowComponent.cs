﻿using Rhisis.World.Game.Entities;

namespace Rhisis.World.Game.Components
{
    public class FollowComponent
    {
        /// <summary>
        /// Gets or sets the follow distance.
        /// </summary>
        public float FollowDistance { get; set; }

        /// <summary>
        /// Gets or sets the following target.
        /// </summary>
        public IWorldEntity Target { get; set; }

        /// <summary>
        /// Gets or sets if the object is following another.
        /// </summary>
        public bool IsFollowing => Target != null;

        /// <summary>
        /// Creates a new <see cref="FollowComponent"/> instance.
        /// </summary>
        public FollowComponent()
        {
            Reset();
        }

        /// <summary>
        /// Reset the component.
        /// </summary>
        public void Reset()
        {
            Target = null;
            FollowDistance = 1f;
        }
    }
}

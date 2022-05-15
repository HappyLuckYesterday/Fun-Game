﻿using System;

namespace Rhisis.Abstractions.Features
{
    /// <summary>
    /// Provides a mechanism to delay actions.
    /// </summary>
    public interface IDelayer : IDisposable
    {
        /// <summary>
        /// Delay an action using a time stamp has delay.
        /// </summary>
        /// <param name="delayTime">Delay time before executing action.</param>
        /// <param name="action">Action to execute after delay has passed.</param>
        /// <returns>Delayed action unique Id.</returns>
        Guid DelayAction(TimeSpan delayTime, Action action);

        /// <summary>
        /// Delay an action using seconds as time unit.
        /// </summary>
        /// <param name="delaySeconds">Delay time in seconds before executing action.</param>
        /// <param name="delayedAction">Action to execute after delay has passed.</param>
        /// <returns>Delayed action Id.</returns>
        Guid DelayAction(double delaySeconds, Action delayedAction);

        /// <summary>
        /// Delay an action using milliseconds as time unit.
        /// </summary>
        /// <param name="delayMilliseconds">Delay time in milliseconds before executing the action.</param>
        /// <param name="delayedAction">Action to execute after the delay has passed.</param>
        /// <returns>elayed action Id.</returns>
        Guid DelayActionMilliseconds(double delayMilliseconds, Action delayedAction);

        /// <summary>
        /// Cancels an action.
        /// </summary>
        /// <param name="delayedActionId">Delayed action id.</param>
        void CancelAction(Guid delayedActionId);

        /// <summary>
        /// Cancel all actions.
        /// </summary>
        void CancelAllActions();
    }
}

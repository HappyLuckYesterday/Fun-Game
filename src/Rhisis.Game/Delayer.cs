using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rhisis.Game;

/// <summary>
/// Provides a mechanism to delay an action.
/// </summary>
public sealed class Delayer : IDisposable
{
    private readonly Dictionary<Guid, DelayedAction> _delayedActions = new();

    /// <summary>
    /// Delay an action using a time stamp has delay.
    /// </summary>
    /// <param name="delayTime">Delay time before executing action.</param>
    /// <param name="action">Action to execute after delay has passed.</param>
    /// <returns>Delayed action unique Id.</returns>
    public Guid DelayAction(TimeSpan delayTime, Action action)
    {
        Guid delayedActionId = Guid.NewGuid();
        DelayedAction delayedAction = new(action, delayTime);

        _delayedActions.Add(delayedActionId, delayedAction);

        delayedAction.Start();

        return delayedActionId;
    }

    /// <summary>
    /// Delay an action using seconds as time unit.
    /// </summary>
    /// <param name="delaySeconds">Delay time in seconds before executing action.</param>
    /// <param name="delayedAction">Action to execute after delay has passed.</param>
    /// <returns>Delayed action Id.</returns>
    public Guid DelayAction(double delaySeconds, Action delayedAction)
        => DelayAction(TimeSpan.FromSeconds(delaySeconds), delayedAction);

    /// <summary>
    /// Delay an action using milliseconds as time unit.
    /// </summary>
    /// <param name="delayMilliseconds">Delay time in milliseconds before executing the action.</param>
    /// <param name="delayedAction">Action to execute after the delay has passed.</param>
    /// <returns>elayed action Id.</returns>
    public Guid DelayActionMilliseconds(double delayMilliseconds, Action delayedAction)
        => DelayAction(TimeSpan.FromMilliseconds(delayMilliseconds), delayedAction);

    /// <summary>
    /// Cancels an action.
    /// </summary>
    /// <param name="delayedActionId">Delayed action id.</param>
    public void CancelAction(Guid delayedActionId)
    {
        if (_delayedActions.Remove(delayedActionId, out DelayedAction delayedAction))
        {
            delayedAction.Cancel();
        }
    }

    /// <summary>
    /// Cancel all actions.
    /// </summary>
    public void CancelAllActions()
    {
        foreach (KeyValuePair<Guid, DelayedAction> action in _delayedActions)
        {
            action.Value.Cancel();
        }

        _delayedActions.Clear();
    }

    /// <summary>
    /// Dispose the delayer instance.
    /// </summary>
    public void Dispose() => CancelAllActions();

    private sealed class DelayedAction : IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly Action _actionToExecute;
        private readonly TimeSpan _delayTime;

        /// <summary>
        /// Creates a new delayed action.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        /// <param name="delayTime">Delay time before executing the action.</param>
        /// <exception cref="ArgumentNullException">Thrown when the given action is null.</exception>
        public DelayedAction(Action action, TimeSpan delayTime)
        {
            _actionToExecute = action ?? throw new ArgumentNullException(nameof(action));
            _delayTime = delayTime;
        }

        /// <summary>
        /// Starts the delay before action execution.
        /// </summary>
        public void Start()
        {
            Task.Delay(_delayTime, _cancellationTokenSource.Token).ContinueWith(_ =>
            {
                if (!_cancellationTokenSource.IsCancellationRequested)
                {
                    _actionToExecute.Invoke();
                }
            });
        }

        /// <summary>
        /// Cancels the delayed action.
        /// </summary>
        public void Cancel() => _cancellationTokenSource.Cancel(false);

        /// <summary>
        /// Cancels and releases the action resources.
        /// </summary>
        public void Dispose()
        {
            Cancel();
        }
    }
}
using Rhisis.Game.Abstractions.Entities;
using Rhisis.Game.Common;

namespace Rhisis.Game.Abstractions.Systems
{
    /// <summary>
    /// Provides a mechanism to manage the player job.
    /// </summary>
    public interface IJobSystem
    {
        /// <summary>
        /// Changes a player's job.
        /// </summary>
        /// <param name="player">Current player.</param>
        /// <param name="newJob">New player job.</param>
        void ChangeJob(IPlayer player, DefineJob.Job newJob);

        /// <summary>
        /// Gets the minimum level required for the given job.
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        int GetJobMinLevel(DefineJob.Job job);

        /// <summary>
        /// Gets the maximum level for the given job.
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        int GetJobMaxLevel(DefineJob.Job job);
    }
}

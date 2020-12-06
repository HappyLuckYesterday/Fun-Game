using Microsoft.EntityFrameworkCore;
using Rhisis.ClusterServer.Structures;
using Rhisis.Database;
using Rhisis.Database.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.ClusterServer.Handlers
{
    public class ClusterHandlerBase
    {
        protected IRhisisDatabase Database { get; }

        protected ClusterHandlerBase(IRhisisDatabase database)
        {
            Database = database;
        }

        /// <summary>
        /// Gets the characters of a given user id.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>Collection of <see cref="DbCharacter"/>.</returns>
        protected IEnumerable<ClusterCharacter> GetCharacters(int userId)
        {
            return Database.Characters.AsNoTracking()
                .Include(x => x.Items)
                    .ThenInclude(x => x.Item)
                .Where(x => x.UserId == userId && !x.IsDeleted)
                .Select(x => new ClusterCharacter(x));
        }
    }
}

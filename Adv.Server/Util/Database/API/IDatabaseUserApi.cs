using System.Collections.Generic;
using Adv.Server.Master;

namespace Adv.Server.Util.Database.API
{
    interface IDatabaseUserApi
    {
        public List<User> GetAllUsers(IDatabaseConnection connection, List<Team> allTeams);
        public bool AddUser(User user, List<Team> teams, IDatabaseConnection connection);
    }
}

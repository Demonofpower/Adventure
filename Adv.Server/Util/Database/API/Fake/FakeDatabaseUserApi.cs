using System;
using System.Collections.Generic;
using Adv.Server.Master;

namespace Adv.Server.Util.Database.API.Fake
{
    class FakeDatabaseUserApi : IDatabaseUserApi
    {
        private List<User> users;

        public FakeDatabaseUserApi()
        {
            users = new List<User>();
        }

        public List<User> GetAllUsers(IDatabaseConnection connection, List<Team> allTeams)
        {
            return users;
        }

        public bool AddUser(User user, List<Team> teams, IDatabaseConnection connection)
        {
            users.Add(user);

            return true;
        }
    }
}

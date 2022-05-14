using System;
using System.Collections.Generic;
using System.Text;
using Adv.Server.Master;

namespace Adv.Server.Util.Database.API.Fake
{
    class FakeDatabaseTeamApi : IDatabaseTeamApi
    {
        private List<Team> teams;

        public FakeDatabaseTeamApi()
        {
            teams = new List<Team>();
        }

        public List<Team> GetAllTeams(IDatabaseConnection connection)
        {
            return teams;
        }

        public bool AddTeam(Team team, IDatabaseConnection connection)
        {
            team.Id = Constants.Random.Next(0, 10000);
            teams.Add(team);

            return true;
        }
    }
}

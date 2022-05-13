using System;
using System.Collections.Generic;
using System.Text;
using Adv.Server.Master;

namespace Adv.Server.Util.Database.API
{
    interface IDatabaseTeamApi
    {
        public List<Team> GetAllTeams(IDatabaseConnection connection);
        public bool AddTeam(Team team, IDatabaseConnection connection);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Adv.Server.Master
{
    class Team
    {
        public string TeamName { get; set; }
        public string SecretTeamName { get; set; }

        public Team(string teamName, string secretTeamName)
        {
            TeamName = teamName;
            SecretTeamName = secretTeamName;
        }
    }
}

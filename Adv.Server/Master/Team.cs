namespace Adv.Server.Master
{
    public class Team
    {
        public int Id { get; set; }
        
        public string TeamName { get; set; }
        public string SecretTeamName { get; set; }

        public Team(string teamName, string secretTeamName, int id = 0)
        {
            TeamName = teamName;
            SecretTeamName = secretTeamName;
            Id = id;
        }
    }
}

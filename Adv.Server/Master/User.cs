using System.Collections.Generic;

namespace Adv.Server.Master
{
    class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        
        public int Id { get; set; }
        public Team Team { get; set; }
        public bool IsAdmin { get; set; }

        public List<Character> Characters { get; set; }

        public User(string username, string password, int id, Team team, bool isAdmin, List<Character> characters)
        {
            Username = username;
            Password = password;
            Id = id;
            Team = team;
            IsAdmin = isAdmin;
            Characters = characters;
        }
    }
}

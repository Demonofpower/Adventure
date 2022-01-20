using System.Collections.Generic;

namespace Adv.Server.Master
{
    class User
    {
        public int Id { get; set; }
        
        public string Username { get; set; }
        public string Password { get; set; }
        
        public Team Team { get; set; }
        public bool IsAdmin { get; set; }

        public List<Character> Characters { get; set; }

        public User(string username, string password, Team team, bool isAdmin, List<Character> characters, int id = 0)
        {
            Username = username;
            Password = password;
            Team = team;
            IsAdmin = isAdmin;
            Characters = characters;
            Id = id;
        }
    }
}

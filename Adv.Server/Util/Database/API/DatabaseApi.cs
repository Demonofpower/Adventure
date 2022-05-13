using System.Collections.Generic;
using Adv.Server.Master;

namespace Adv.Server.Util.Database.API
{
    static class DatabaseApi
    {
        private static IDatabaseCharacterApi _databaseCharacterApi;
        private static IDatabaseTeamApi _databaseTeamApi;
        private static IDatabaseUserApi _databaseUserApi;

        public static void Init(IDatabaseCharacterApi databaseCharacterApi, IDatabaseTeamApi databaseTeamApi, IDatabaseUserApi databaseUserApi)
        {
            _databaseCharacterApi = databaseCharacterApi;
            _databaseTeamApi = databaseTeamApi;
            _databaseUserApi = databaseUserApi;
        }

        public static List<Character> GetAllCharacters(IDatabaseConnection connection, List<User> allUsers)
        {
            return _databaseCharacterApi.GetAllCharacters(connection, allUsers);
        }

        public static bool AddCharacter(Character character, IDatabaseConnection connection)
        {
            return _databaseCharacterApi.AddCharacter(character, connection);
        }

        public static List<Team> GetAllTeams(IDatabaseConnection connection)
        {
            return _databaseTeamApi.GetAllTeams(connection);
        }

        public static bool AddTeam(Team team, IDatabaseConnection connection)
        {
            return _databaseTeamApi.AddTeam(team, connection);
        }

        public static List<User> GetAllUsers(IDatabaseConnection connection, List<Team> allTeams)
        {
            return _databaseUserApi.GetAllUsers(connection, allTeams);
        }

        public static bool AddUser(User user, List<Team> teams, IDatabaseConnection connection)
        {
            return _databaseUserApi.AddUser(user, teams, connection);
        }
    }
}

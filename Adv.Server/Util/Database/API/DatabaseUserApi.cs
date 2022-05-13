using System;
using System.Collections.Generic;
using System.Linq;
using Adv.Server.Master;

namespace Adv.Server.Util.Database.API
{
    class DatabaseUserApi
    {
        public static List<User> GetAllUsers(IDatabaseConnection connection, List<Team> allTeams)
        {
            var result = connection.ExecuteQuery(@"SELECT * from adventure.users");

            var list = new List<User>();

            try
            {
                while (result.Read())
                {
                    var id = result.GetInt32("id");
                    var username = result.GetString("username");
                    var password = result.GetString("password");
                    var isAdmin = result.GetBoolean("isAdmin");
                    var userTeam = result.GetInt32("team");

                    var team = allTeams.First(t => t.Id == userTeam);

                    list.Add(new User(username, password, team, isAdmin, new List<Character>(), id));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                result.Close();
            }

            result.Close();
            return list.Any() ? list : null;
        }


        //public static User GetUserByName(string name, IDatabaseConnection connection)
        //{
        //    var result = connection.ExecuteQuery(@"SELECT * from adventure.users WHERE username = @username",
        //        new Tuple<string, object>("@username", name));

        //    try
        //    {
        //        while (result.Read())
        //        {
        //            var id = result.GetInt32("id");
        //            var password = result.GetString("password");
        //            var isAdmin = result.GetBoolean("isAdmin");
        //            result.Close();

        //            var team = DatabaseTeamApi.GetTeamById(result.GetInt32("team"), connection);

        //            return new User(name, password, team, isAdmin, new List<Character>(), id);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }

        //    result.Close();
        //    return null;
        //}

        //public static User GetUserById(int id, IDatabaseConnection connection)
        //{
        //    var result = connection.ExecuteQuery(@"SELECT * from adventure.users WHERE id = @id",
        //        new Tuple<string, object>("@id", id));

        //    try
        //    {
        //        while (result.Read())
        //        {
        //            var username = result.GetString("username");
        //            var password = result.GetString("password");
        //            var isAdmin = result.GetBoolean("isAdmin");
        //            result.Close();

        //            var team = DatabaseTeamApi.GetTeamById(result.GetInt32("team"), connection);

        //            return new User(username, password, team, isAdmin, new List<Character>(), id);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }

        //    result.Close();
        //    return null;
        //}

        public static bool AddUser(User user, List<Team> teams, IDatabaseConnection connection)
        {
            if (user.Characters.Any())
            {
                throw new NotSupportedException("Add the user first before characters!");
            }

            var team = teams.FirstOrDefault(t => t.SecretTeamName == user.Team.SecretTeamName);

            var cmdText =
                @"INSERT INTO adventure.users(username,password,isAdmin,team) VALUES(@username, @password, @isAdmin, @team)";

            var username = new Tuple<string, object>("@username", user.Username);
            var password = new Tuple<string, object>("@password", user.Password);
            var isAdmin = new Tuple<string, object>("@isAdmin", user.IsAdmin);

            if (team == null)
            {
                team = teams.FirstOrDefault(t => t.TeamName == user.Team.TeamName);
                if (team == null)
                {
                    DatabaseTeamApi.AddTeam(new Team(user.Team.TeamName, user.Team.SecretTeamName), connection);
                    team = DatabaseTeamApi.GetAllTeams(connection).First(t => t.TeamName == user.Team.TeamName);
                }
                else
                {
                    return false;
                }
            }
            
            try
            {
                connection.ExecuteNonQuery(cmdText, username, password, isAdmin, new Tuple<string, object>("@team", team.Id));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }
    }
}
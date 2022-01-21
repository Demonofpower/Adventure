using System;
using System.Collections.Generic;
using System.Linq;
using Adv.Server.Master;

namespace Adv.Server.Util.Database.API
{
    class DatabaseTeamApi
    {
        public static List<Team> GetAllTeams(DatabaseConnection connection)
        {
            var result = connection.ExecuteQuery(@"SELECT * from adventure.teams");

            var list = new List<Team>();

            try
            {
                while (result.Read())
                {
                    var id = result.GetInt32("id");
                    var name = result.GetString("name");
                    var secretName = result.GetString("secretName");
                    
                    list.Add(new Team(name, secretName, id));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            result.Close();
            return list.Any() ? list : null;
        }

        //public static Team GetTeamByName(string teamName, DatabaseConnection connection)
        //{
        //    var result = connection.ExecuteQuery(@"SELECT * from adventure.teams WHERE name = @name",
        //        new Tuple<string, object>("@name", teamName));

        //    try
        //    {
        //        while (result.Read())
        //        {
        //            var id = result.GetInt32("id");
        //            var name = result.GetString("name");
        //            var secretName = result.GetString("secretName");

        //            result.Close();
        //            return new Team(name, secretName, id);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }

        //    result.Close();
        //    return null;
        //}

        //public static Team GetTeamBySecretName(string secretTeamName, DatabaseConnection connection)
        //{
        //    var result = connection.ExecuteQuery(@"SELECT * from adventure.teams WHERE secretName = @secretTeamName",
        //        new Tuple<string, object>("@secretTeamName", secretTeamName));

        //    try
        //    {
        //        while (result.Read())
        //        {
        //            var id = result.GetInt32("id");
        //            var name = result.GetString("name");
        //            var secretName = result.GetString("secretName");

        //            result.Close();
        //            return new Team(name, secretName, id);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }

        //    result.Close();
        //    return null;
        //}

        //public static Team GetTeamById(int id, DatabaseConnection connection)
        //{
        //    var result = connection.ExecuteQuery(@"SELECT * from adventure.teams WHERE id = @id",
        //        new Tuple<string, object>("@id", id));

        //    try
        //    {
        //        while (result.Read())
        //        {
        //            var name = result.GetString("name");
        //            var secretName = result.GetString("secretName");

        //            result.Close();
        //            return new Team(name, secretName, id);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }

        //    result.Close();
        //    return null;
        //}

        public static bool AddTeam(Team team, DatabaseConnection connection)
        {
            var cmdText =
                @"INSERT INTO adventure.teams(name,secretName) VALUES(@name, @secretName)";

            var name = new Tuple<string, object>("@name", team.TeamName);
            var secretName = new Tuple<string, object>("@secretName", team.SecretTeamName);
            
            try
            {
                connection.ExecuteNonQuery(cmdText, name, secretName);
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

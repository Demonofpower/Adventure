using System;
using System.Collections.Generic;
using System.Linq;
using Adv.Server.Master;

namespace Adv.Server.Util.Database.API
{
    class DatabaseUserApi
    {
        public static List<User> GetAllUsers(DatabaseConnection connection)
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

                    //TODO TEAM + CHARACTERS
                    
                    list.Add(new User(username, password, null, isAdmin, new List<Character>(), id));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            result.Close();
            return list.Any() ? list : null;
        }


        public static User GetUserByName(string name, DatabaseConnection connection)
        {
            var result = connection.ExecuteQuery(@"SELECT * from adventure.users WHERE username = @username",
                new Tuple<string, object>("@username", name));

            try
            {
                while (result.Read())
                {
                    var id = result.GetInt32("id");
                    var password = result.GetString("password");
                    var isAdmin = result.GetBoolean("isAdmin");

                    //TODO TEAM + CHARACTERS

                    result.Close();
                    return new User(name, password, null, isAdmin, new List<Character>(), id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            result.Close();
            return null;
        }

        public static User GetUserById(int id, DatabaseConnection connection)
        {
            var result = connection.ExecuteQuery(@"SELECT * from adventure.users WHERE id = @id",
                new Tuple<string, object>("@id", id));

            try
            {
                while (result.Read())
                {
                    var username = result.GetString("username");
                    var password = result.GetString("password");
                    var isAdmin = result.GetBoolean("isAdmin");

                    //TODO TEAM + CHARACTERS

                    result.Close();
                    return new User(username, password, null, isAdmin, new List<Character>(), id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            result.Close();
            return null;
        }

        public static void AddUser(User user, DatabaseConnection connection)
        {
            var cmdText =
                @"INSERT INTO adventure.users(username,password,isAdmin,team) VALUES(@username, @password, @isAdmin, @team)";

            var username = new Tuple<string, object>("@username", user.Username);
            var password = new Tuple<string, object>("@password", user.Password);
            var isAdmin = new Tuple<string, object>("@isAdmin", user.IsAdmin);
            var team = new Tuple<string, object>("@team", user.Team);

            if (user.Characters.Any())
            {
                throw new NotSupportedException("Add the user first before characters!");
            }

            connection.ExecuteNonQuery(cmdText, username, password, isAdmin, team);
        }
    }
}
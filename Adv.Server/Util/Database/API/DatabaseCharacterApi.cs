using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adv.Server.Master;
using Adv.Server.Util.Enums;

namespace Adv.Server.Util.Database.API
{
    class DatabaseCharacterApi
    {
        public static List<Character> GetAllCharacters(DatabaseConnection connection, List<User> allUsers)
        {
            var result = connection.ExecuteQuery(@"SELECT * from adventure.characters");

            var list = new List<Character>();

            try
            {
                while (result.Read())
                {
                    var id = result.GetInt32("id");
                    var name = result.GetString("name");
                    var location = result.GetInt32("location");
                    var avatar = result.GetByte("avatar");
                    var colorA = result.GetInt32("colorA");
                    var colorB = result.GetInt32("colorB");
                    var colorC = result.GetInt32("colorC");
                    var colorD = result.GetInt32("colorD");
                    var flags = result.GetInt32("flags");
                    var isAdmin = result.GetBoolean("isAdmin");
                    var dbUser = result.GetInt32("user");

                    var user = allUsers.First(u => u.Id == dbUser);

                    list.Add(new Character(name, Location.LostCave, avatar, colorA, colorB, colorC, colorD, flags,
                        isAdmin, user, id));
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

        public static bool AddCharacter(Character character, DatabaseConnection connection)
        {
            var cmdText =
                @"INSERT INTO adventure.characters(name,location,avatar,colorA,colorB,colorC,colorD,flags,isAdmin,user)
                VALUES(@name,@location,@avatar,@colorA,@colorB,@colorC,@colorD,@flags,@isAdmin,@user)";

            var name = new Tuple<string, object>("@name", character.Name);
            var location = new Tuple<string, object>("@location", character.Location);
            var avatar = new Tuple<string, object>("@avatar", character.Avatar);
            var colorA = new Tuple<string, object>("@colorA", character.ColorA);
            var colorB = new Tuple<string, object>("@colorB", character.ColorB);
            var colorC = new Tuple<string, object>("@colorC", character.ColorC);
            var colorD = new Tuple<string, object>("@colorD", character.ColorD);
            var flags = new Tuple<string, object>("@flags", character.Flags);
            var isAdmin = new Tuple<string, object>("@isAdmin", character.IsAdmin);
            var user = new Tuple<string, object>("@user", character.User.Id);
            

            try
            {
                connection.ExecuteNonQuery(cmdText, name, location, avatar, colorA, colorB, colorC, colorD, flags, isAdmin, user);
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
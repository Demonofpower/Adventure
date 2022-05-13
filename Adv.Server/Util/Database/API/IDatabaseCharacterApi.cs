using System;
using System.Collections.Generic;
using System.Text;
using Adv.Server.Master;

namespace Adv.Server.Util.Database.API
{
    interface IDatabaseCharacterApi
    {
        public List<Character> GetAllCharacters(IDatabaseConnection connection, List<User> allUsers);
        public bool AddCharacter(Character character, IDatabaseConnection connection);
    }
}

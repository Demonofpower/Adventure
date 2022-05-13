using System.Collections.Generic;
using Adv.Server.Master;

namespace Adv.Server.Util.Database.API.Fake
{
    class FakeDatabaseCharacterApi : IDatabaseCharacterApi
    {
        private List<Character> characters;

        public FakeDatabaseCharacterApi()
        {
            characters = new List<Character>();
        }

        public List<Character> GetAllCharacters(IDatabaseConnection connection, List<User> allUsers)
        {
            return characters;
        }

        public bool AddCharacter(Character character, IDatabaseConnection connection)
        {
            characters.Add(character);

            return true;
        }
    }
}

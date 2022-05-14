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
            character.Id = Constants.Random.Next(0, 10000);
            characters.Add(character);

            return true;
        }
    }
}

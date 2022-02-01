using System.Collections.Generic;
using System.Linq;
using Adv.Server.Master;
using Adv.Server.Util.Enums;

namespace Adv.Server.Game
{
    class UseHandler
    {
        private List<Actor> actors;
        
        private int greatBallsOfFireActor;

        public UseHandler(List<Actor> actors)
        {
            this.actors = actors;
            Init();
        }

        private void Init()
        {
            greatBallsOfFireActor = actors.First(a => a.ActorType == ActorType.GreatBallsOfFire).Id;
        }
        
        
        public byte[] Process(int itemId, Character character)
        {
            var returnPacket = new List<byte>();

            if (itemId == greatBallsOfFireActor)
            {
                //TODO CHECK IF ALREADY USED AND ADD TO CHAR

                var addItemPacket = GameConnectionApi.CreateServerAddItemPacket(ItemType.GreatBallsOfFire, 1);
                var pickupAPacket = GameConnectionApi.CreateServerPickedUpPacket(PickupType.Achievement_GreatBallsOfFire);
                var pickupBPacket = GameConnectionApi.CreateServerPickedUpPacket(PickupType.GreatBallsOfFire);
                
                returnPacket.AddRange(addItemPacket);
                returnPacket.AddRange(pickupAPacket);
                returnPacket.AddRange(pickupBPacket);
            }

            return returnPacket.Count > 0 ? returnPacket.ToArray() : null;
        }
    }
}

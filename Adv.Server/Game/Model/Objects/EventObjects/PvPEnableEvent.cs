using System;
using Adv.Server.Master;
using Adv.Server.Util;
using Adv.Server.Util.Enums;

namespace Adv.Server.Game.Model.Objects.EventObjects
{
    class PvPEnableEvent : EventObject
    {
        private readonly Character eventCharacter;
        private readonly bool isPvpNowEnabled;

        public PvPEnableEvent(bool isPvpNowEnabled, Character eventCharacter)
        {
            this.isPvpNowEnabled = isPvpNowEnabled;
            this.eventCharacter = eventCharacter;
        }

        public override void Tick()
        {
            base.Tick();

            var characterStream = ClientHelper.GeTcpClientByCharacter(eventCharacter).GetStream();
            characterStream.Write(GameConnectionApi.CreateServerPvpCountdownUpdatePacket(Convert.ToByte(isPvpNowEnabled), (int) ((50 - TicksAlive)/10)));

            if (TicksAlive >= 50)
            {
                eventCharacter.PvPEnabled = isPvpNowEnabled;

                characterStream.Write(GameConnectionApi.CreateServerPvpEnablePacket(Convert.ToByte(isPvpNowEnabled)));
                ClientHelper.SendToAllExceptCharacter(GameConnectionApi.CreateServerStatePacket(eventCharacter.Id, State.PvP, Convert.ToByte(isPvpNowEnabled)), eventCharacter);

                IsFaded = true;
            }
        }
    }
}

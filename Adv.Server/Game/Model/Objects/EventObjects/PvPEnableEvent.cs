using System;
using Adv.Server.Master;
using Adv.Server.Util;
using Adv.Server.Util.Enums;

namespace Adv.Server.Game.Model.Objects.EventObjects
{
    class PvPEnableEvent : EventObject
    {
        public Character EventCharacter { get; }
        private bool IsPvpNowEnabled { get; }

        private int countdownLeft;

        public PvPEnableEvent(bool isPvpNowEnabled, Character eventCharacter)
        {
            IsPvpNowEnabled = isPvpNowEnabled;
            EventCharacter = eventCharacter;

            countdownLeft = 5;
        }

        public override void Tick()
        {
            base.Tick();

            if (TicksAlive == 1 || TicksAlive % 10 == 0)
            {
                var characterStream = ClientHelper.GeTcpClientByCharacter(EventCharacter).GetStream();

                if (countdownLeft == 0) 
                {
                    EventCharacter.PvPEnabled = IsPvpNowEnabled;

                    characterStream.Write(GameConnectionApi.CreateServerPvpEnablePacket(Convert.ToByte(IsPvpNowEnabled)));
                    ClientHelper.SendToAllExceptCharacter(GameConnectionApi.CreateServerStatePacket(EventCharacter.Id, State.PvP, Convert.ToByte(IsPvpNowEnabled)), EventCharacter);

                    IsFaded = true;
                    return;
                }

                characterStream.Write(GameConnectionApi.CreateServerPvpCountdownUpdatePacket(Convert.ToByte(IsPvpNowEnabled), countdownLeft));

                countdownLeft -= 1;
            }
        }
    }
}

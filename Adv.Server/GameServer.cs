using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using Adv.Server.Game;
using Adv.Server.Packets;
using Adv.Server.Packets.Game;
using Adv.Server.Util;

namespace Adv.Server
{
    class GameServer
    {
        private Dictionary<TcpClient, string> sessions;

        public void Start(int port)
        {
            Populate();

            var listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            while (true)
            {
                Console.WriteLine("Waiting for a client to connect...");
                TcpClient client = listener.AcceptTcpClient();
                ProcessClient(client);
            }
        }

        private void ProcessClient(TcpClient client)
        {
            sessions.Add(client, null);

            var networkStream = client.GetStream();
            try
            {
                networkStream.ReadTimeout = int.MaxValue;
                networkStream.WriteTimeout = int.MaxValue;
                
                while (true)
                {
                    List<byte> packet = ReadMessage(networkStream);

                    if (sessions[client] is null)
                    {
                        var clientHelloPacket = GameConnectionApi.ProcessClientHelloPacket(packet.ToArray());

                        sessions[client] = clientHelloPacket.SessionId;
                        
                        //TODO!!
                        var pos = new Vector3(-54150f, -56283f, 1000);
                        var rot = new Rotation(0, 0, 0);

                        var helloReply = GameConnectionApi.CreateServerHelloPacket(pos, rot);
                        networkStream.Write(helloReply);


                        //TODO SEE https://github.com/w4kfu/pwnadventure3/blob/master/code/game_server/game_server.py#L209
                        //var actorSpawnReply = GameConnectionApi.CreateActorSpawnPacket(pos, rot);
                        //networkStream.Write(actorSpawnReply);

                        //var a = PacketProcessor.StringToByteArray("6d6b0700000000000000000c0047756e53686f704f776e6572005712c700048dc6000017450000ff7f000064000000");
                        //networkStream.Write(a);

                        //var b = PacketProcessor.StringToByteArray(
                        //"6d6b0100000000000000001000477265617442616c6c734f664669726500872ac7000c5ac70000a143000000800000640000006d6b0200000000000000000c004c6f73744361766542757368002351c700d62cc70000b343000000000000640000006d6b030000000000000000090042656172436865737400b0f6c500e27b470070264523fde67f8100640000006d6b0400000000000000000800436f77436865737400fe764800a16fc80040924422fe088fc5fd640000006d6b05000000000000000009004c617661436865737400bc464700d8a3c50060be440000e3380000640000006d6b0600000000000000000b00426c6f636b79436865737400f03ec500bab34600300e45000000c00000640000006d6b0700000000000000000c0047756e53686f704f776e6572005712c700048dc6000017450000ff7f0000640000006d6b0800000000000000000f004a757374696e546f6c657261626c65007c20c700007ec600e00d450000aa6a0000640000006d6b09000000000000000006004661726d65720062a84600102147005005450000e3380000640000006d6b0a00000000000000000d004d69636861656c416e67656c6fc0277e48c0ba72c800e0b0440000c7510000640000006d6b0b00000000000000000a00476f6c64656e4567673100aac3c6004a8d4600008243000000000000640000006d6b0c00000000000000000a00476f6c64656e45676732007249c7001f6fc700e09c45000000000000640000006d6b0d00000000000000000a00476f6c64656e456767330080bf460019884700302645000000000000640000006d6b0e00000000000000000a00476f6c64656e4567673400256c47000288c600b03745000000000000640000006d6b0f00000000000000000a00476f6c64656e456767350040be4400d869460070db45000000000000640000006d6b1000000000000000000a00476f6c64656e4567673600503546002c4dc60080cd43000000000000640000006d6b1100000000000000000a00476f6c64656e4567673780ed8dc7003f51c700a0cd44000000000000640000006d6b1200000000000000000a00476f6c64656e4567673800143d4700aadb4600003044000000000000640000006d6b1300000000000000000a00476f6c64656e4567673900c97e470060b3c500009a45000000000000640000006d6b1400000000000000000e0042616c6c6d65725065616b45676700a02dc5006c2cc600202446000000000000640000006d6b150000000000000000110042616c6c6d65725065616b506f7374657200a8bec500302bc600302646000000000000640000000000");
                        //networkStream.Write(b);

                        continue;
                    }
                    
                    var arrayPacket = new Span<byte>(packet.ToArray());

                    while (arrayPacket[0] != 0)
                    {
                        var reply = GetNewMessageAndCraftAnswer(ref arrayPacket, client);

                        if (reply == null || reply.Length == 0) continue;

                        networkStream.Write(reply);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
                if (e.InnerException != null)
                {
                    Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
                }
            }
            finally
            {
                networkStream.Close();
                client.Close();
            }
        }

        private List<byte> ReadMessage(NetworkStream stream)
        {
            byte[] buffer = new byte[2048];
            int bytes = 0;
            do
            {
                bytes = stream.Read(buffer, 0, buffer.Length);
            } while (bytes == 0);

            return buffer.ToList();
        }

        private byte[] GetNewMessageAndCraftAnswer(ref Span<byte> packet, TcpClient client)
        {
            PacketProcessor.SwitchPacketIdEndian(ref packet);
            var gamePacketType = Enum.Parse<GamePacketType>(PacketProcessor.Read16(ref packet).ToString());

            switch (gamePacketType)
            {
                case GamePacketType.OnNPCConversationStateEvent:
                    break;
                case GamePacketType.OnPlayerPositionEvent:
                    break;
                case GamePacketType.OnRemoveItemEvent:
                    break;
                case GamePacketType.OnActorSpawnEvent:
                    break;
                case GamePacketType.OnReloadEvent:
                    break;
                case GamePacketType.OnRespawnOtherPlayerEvent:
                    break;
                case GamePacketType.OnAddItemEvent:
                    break;
                case GamePacketType.OnRemoteReloadEvent:
                    break;
                case GamePacketType.OnTeleportEvent:
                    break;
                case GamePacketType.OnStartQuestEvent:
                    break;
                case GamePacketType.OnTriggerEvent:
                    break;
                case GamePacketType.OnPickedUpEvent:
                    break;
                case GamePacketType.OnRelativeTeleportEvent:
                    break;
                case GamePacketType.OnPositionAndVelocityEvent:
                    break;
                case GamePacketType.OnRespawnThisPlayerEvent:
                    break;
                case GamePacketType.OnStateEvent:
                    break;
                case GamePacketType.OnPvpEnableEvent:
                    break;
                case GamePacketType.OnDisplayEvent:
                    break;
                case GamePacketType.OnPositionEvent:
                    var clientPosition = GameConnectionApi.ProcessClientPositionPacket(ref packet);

                    return GameConnectionApi.CreateClientPositionPacket(clientPosition.Position, clientPosition.Rotation);
                case GamePacketType.OnActorDestroyEvent:
                    break;
                case GamePacketType.OnPlayerItemEvent:
                    break;
                case GamePacketType.OnCurrentSlotEvent:
                    break;
                case GamePacketType.OnEquipItemEvent:
                    break;
                case GamePacketType.OnCircuitOutputEvent:
                    break;
                case GamePacketType.OnKillEvent:
                    break;
                case GamePacketType.OnSetCurrentQuestEvent:
                    break;
                case GamePacketType.OnHealthUpdateEvent:
                    break;
                case GamePacketType.OnChatEvent:
                    break;
                case GamePacketType.OnFireBulletsEvent:
                    break;
                case GamePacketType.OnNPCShopEvent:
                    break;
                case GamePacketType.OnPvPCountdownUpdateEvent:
                    break;
                case GamePacketType.OnPlayerLeftEvent:
                    break;
                case GamePacketType.OnPlayerJoinedEvent:
                    break;
                case GamePacketType.OnManaUpdateEvent:
                    break;
                case GamePacketType.OnAdvanceQuestToStateEvent:
                    break;
                case GamePacketType.OnLoadedAmmoEvent:
                    break;
                case GamePacketType.OnLastHitByItemEvent:
                    break;
                case GamePacketType.OnCountdownUpdateEvent:
                    break;
                case GamePacketType.OnNPCConversationEndEvent:
                    break;
                case GamePacketType.OnRegionChangeEvent:
                    break;
                case GamePacketType.Use:
                    break;
                case GamePacketType.TransitionToNPCState:
                    break;
                case GamePacketType.BuyItem:
                    break;
                case GamePacketType.Activate:
                    break;
                case GamePacketType.FireRequest:
                    break;
                case GamePacketType.SellItem:
                    break;
                case GamePacketType.Teleport:
                    break;
                case GamePacketType.Sprint:
                    break;
                case GamePacketType.Jump:
                    var clientJumpPacket = GameConnectionApi.ProcessClientJumpPacket(ref packet);
                    Console.WriteLine("JumpPacket - state: " + clientJumpPacket.JumpState);

                    return null;
                case GamePacketType.FastTravel:
                    break;
                case GamePacketType.SubmitDLCKey:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            throw new NotImplementedException();
        }

        private void Populate()
        {
            sessions = new Dictionary<TcpClient, string>();
        }
    }
}
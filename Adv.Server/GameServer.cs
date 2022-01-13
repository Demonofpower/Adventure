using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Numerics;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Adv.Server.Game;
using Adv.Server.Master;
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

                //var buffer = MasterConnectionApi.CreateWelcomePacket(5, "Custom Server", "By Paranoia with <3");
                //Console.WriteLine("Sending hello message.");
                //networkStream.Write(buffer);

                while (true)
                {
                    List<byte> packet = ReadMessage(networkStream);

                    if (sessions[client] is null)
                    {
                        var clientHelloPacket = GameConnectionApi.ProcessClientHelloPacket(packet.ToArray());

                        sessions[client] = clientHelloPacket.SessionId;
                        
                        //TODO!!
                        var helloReply = GameConnectionApi.CreateServerHelloPacket(new Vector3(-54150f, -56283f, 1000), new Rotation(0, 0, 0));
                        
                        networkStream.Write(helloReply);
                        
                        continue;
                    }

                    var reply = GetNewMessageAndCraftAnswer(packet.ToArray(), client);
                    Console.WriteLine("Msg got!");

                    if (reply == null || reply.Length == 0) continue;
                    //if (reply[0] == 0x81) break;

                    //networkStream.Write(reply);
                    Console.WriteLine("Msg answer sent!");
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

        private byte[] GetNewMessageAndCraftAnswer(byte[] packet, TcpClient client)
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
                    var clientPosition = GameConnectionApi.ProcessClientPositionPacket(packet);

                    return null;
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
                    break;
                case GamePacketType.FastTravel:
                    break;
                case GamePacketType.SubmitDLCKey:
                    break;
                default:
                    return null;
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
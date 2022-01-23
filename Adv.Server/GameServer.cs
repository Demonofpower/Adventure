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
                    var clientSlotPacket = GameConnectionApi.ProcessClientSlotPacket(ref packet);
                    Console.WriteLine("SlotPacket - slot: " + clientSlotPacket.Slot);

                    return GameConnectionApi.CreateServerSlotPacket(clientSlotPacket.Slot);
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
                    var clientSprintPacket = GameConnectionApi.ProcessClientSprintPacket(ref packet);
                    Console.WriteLine("SprintPacket - state: " + clientSprintPacket.SprintState);

                    return null;
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
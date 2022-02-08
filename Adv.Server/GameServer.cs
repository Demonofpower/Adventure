using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Threading;
using Adv.Server.Game;
using Adv.Server.Master;
using Adv.Server.Packets;
using Adv.Server.Packets.Game;
using Adv.Server.Util;
using Adv.Server.Util.Enums;

namespace Adv.Server
{
    class GameServer
    {
        public static List<Actor> Actors;

        private UseHandler useHandler;
        
        public static Dictionary<TcpClient, Tuple<string, Character>> sessions;

        public void Start(int port)
        {
            Populate();

            var listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                new Thread(() => ProcessClient(client)).Start();
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

                        sessions[client] = new Tuple<string, Character>(clientHelloPacket.SessionId,
                            MasterServer.Characters.First(c => c.Id == clientHelloPacket.CharacterId));

                        //TODO!!
                        var pos = new Vector3(-54150f, -56283f, 1000);
                        var rot = new Rotation(0, 0, 0);

                        var helloReply = GameConnectionApi.CreateServerHelloPacket(clientHelloPacket.CharacterId, pos, rot);
                        networkStream.Write(helloReply);

                        SendToAllExceptClient(GameConnectionApi.CreateServerPlayerJoinedPacket(sessions[client].Item2), client);
                        foreach (var session in sessions)
                        {
                            if (session.Key != client)
                            {
                                networkStream.Write(GameConnectionApi.CreateServerPlayerJoinedPacket(session.Value.Item2));
                            }
                        }

                        foreach (var actor in Actors)
                        {
                            networkStream.Write(GameConnectionApi.CreateActorSpawnPacket(actor.Id, actor.ActorType, actor.Position, actor.Rotation));
                        }

                        continue;
                    }

                    var arrayPacket = new Span<byte>(packet.ToArray());

                    while (arrayPacket[0] != 0)
                    {
                        var replyCmd = GetNewMessageAndCraftAnswer(ref arrayPacket, client);
                        var reply = replyCmd.Item1;
                        var isBroadcast = replyCmd.Item2;

                        if (reply == null || reply.Length == 0) continue;

                        if (!isBroadcast)
                        {
                            networkStream.Write(reply);
                        }
                        else
                        {
                            foreach (var sessionsClient in sessions.Keys)
                            {
                                var clientStream = sessionsClient.GetStream();
                                clientStream.Write(reply);
                            }
                        }
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
                sessions.Remove(client);
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

        private (byte[], bool) GetNewMessageAndCraftAnswer(ref Span<byte> packet, TcpClient client)
        {
            var currentCharacter = sessions[client]?.Item2;
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
                    var pvpEnablePacket = GameConnectionApi.ProcessClientPvPEnablePacket(ref packet);

                    int pvpEnableTimeLeft = 5;
                    Timer pvpEnableTimer = null;
                    pvpEnableTimer = new Timer(callback =>
                    {
                        client.GetStream().Write(GameConnectionApi.CreateServerPvpCountdownUpdatePacket(pvpEnablePacket.State, pvpEnableTimeLeft));
                        pvpEnableTimeLeft -= 1;
                        if (pvpEnableTimeLeft < 0)
                        {
                            client.GetStream().Write(GameConnectionApi.CreateServerPvpEnablePacket(pvpEnablePacket.State));
                            
                            SendToAllExceptClient(GameConnectionApi.CreateServerStatePacket(currentCharacter.Id, State.PvP, pvpEnablePacket.State), client);

                            currentCharacter.PvPEnabled = pvpEnablePacket.State != 0;
                            
                            pvpEnableTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        }
                    }, null, 0, 1000);

                    return (null, false);
                case GamePacketType.OnPvpCountdownUpdateEvent:
                    break;
                case GamePacketType.OnDisplayEvent:
                    break;
                case GamePacketType.OnPositionEvent:
                    var clientPosition = GameConnectionApi.ProcessClientPositionPacket(ref packet);

                    currentCharacter.Position = clientPosition.Position;
                    currentCharacter.Rotation = clientPosition.Rotation;

                    SendToAllExceptClient(GameConnectionApi.CreateServerPlayerPositionPacket(currentCharacter), client);

                    return (
                        GameConnectionApi.CreateServerPositionPacket(currentCharacter.Id, clientPosition.Position,
                            clientPosition.Rotation), false);
                case GamePacketType.OnActorDestroyEvent:
                    break;
                case GamePacketType.OnPlayerItemEvent:
                    break;
                case GamePacketType.OnCurrentSlotEvent:
                    var clientSlotPacket = GameConnectionApi.ProcessClientSlotPacket(ref packet);
                    Console.WriteLine("SlotPacket - slot: " + clientSlotPacket.Slot);

                    return (GameConnectionApi.CreateServerSlotPacket(clientSlotPacket.Slot), false);
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
                    var clientChatPacket = GameConnectionApi.ProcessClientChatPacket(ref packet);
                    Console.WriteLine($"ChatPacket - msg: {clientChatPacket.Message} + character: {currentCharacter.Name}");

                    if (clientChatPacket.Message.StartsWith('!'))
                    {
                        ChatCommandProcessor.ProcessCommand(clientChatPacket.Message, currentCharacter);
                    }

                    return (GameConnectionApi.CreateServerChatPacket(currentCharacter.Id, clientChatPacket.Message),
                        true);
                case GamePacketType.OnFireBulletsEvent:
                    break;
                case GamePacketType.OnNPCShopEvent:
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
                    var usePacket = GameConnectionApi.ProcessClientUsePacket(ref packet);

                    Console.WriteLine($"UsePacket - itemId: {usePacket.ItemId} + character: {currentCharacter.Name}");

                    var returnPacket = useHandler.Process(usePacket.ItemId, currentCharacter);
                    
                    return (returnPacket, false);
                case GamePacketType.TransitionToNPCState:
                    break;
                case GamePacketType.BuyItem:
                    break;
                case GamePacketType.Activate:
                    var activatePacket = GameConnectionApi.ProcessClientActivatePacket(ref packet);

                    Console.WriteLine($"ActivatePacket - name: {activatePacket.Name} + pos: {activatePacket.Position.X} {activatePacket.Position.Y} {activatePacket.Position.Z}");

                    var fireballSpawnPacket = GameConnectionApi.CreateActorSpawnPacket(3421, ActorType.Fireball,
                        currentCharacter.Position.AddToCoords(x:200), currentCharacter.Rotation);

                    int timeLeft = 1;
                    Timer timer = null;
                    timer = new Timer(callback =>
                    {
                        var fireballUpdatePacket =
                            GameConnectionApi.CreateServerPositionPacket(3421, currentCharacter.Position.AddToCoords(x: 20*timeLeft), currentCharacter.Rotation);
                        
                        client.GetStream().Write(fireballUpdatePacket);
                        
                        timeLeft += 1;
                        if (timeLeft > 30)
                        {
                            timer.Change(Timeout.Infinite, Timeout.Infinite);
                        }
                    }, null, 1000, 100);

                    //TODO
                    return (fireballSpawnPacket, false);
                case GamePacketType.FireRequest:
                    break;
                case GamePacketType.SellItem:
                    break;
                case GamePacketType.Teleport:
                    break;
                case GamePacketType.Sprint:
                    var clientSprintPacket = GameConnectionApi.ProcessClientSprintPacket(ref packet);
                    Console.WriteLine("SprintPacket - state: " + clientSprintPacket.SprintState);

                    return (null, false);
                case GamePacketType.Jump:
                    var clientJumpPacket = GameConnectionApi.ProcessClientJumpPacket(ref packet);
                    Console.WriteLine("JumpPacket - state: " + clientJumpPacket.JumpState);

                    SendToAllExceptClient(
                        GameConnectionApi.CreateServerStatePacket(currentCharacter.Id, State.Jump,
                            clientJumpPacket.JumpState), client);

                    return (null, false);
                case GamePacketType.FastTravel:
                    break;
                case GamePacketType.SubmitDLCKey:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            throw new NotImplementedException();
        }

        private void SendToAllExceptClient(byte[] packet, TcpClient client)
        {
            foreach (var session in sessions)
            {
                if (session.Value != null && session.Key != client)
                {
                    session.Key.GetStream().Write(packet);
                }
            }
        }

        private void Populate()
        {
            sessions = new Dictionary<TcpClient, Tuple<string, Character>>();

            Actors = new List<Actor>();
            Actors.Add(new Actor(100, ActorType.GreatBallsOfFire, new Vector3(-43653.71f, - 55836.54f, 405.65f), new Rotation(-16384, 0, -16451)));

            useHandler = new UseHandler(Actors);
        }
    }
}
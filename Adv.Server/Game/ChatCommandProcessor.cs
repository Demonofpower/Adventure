using System;
using Adv.Server.Master;

namespace Adv.Server.Game
{
    class ChatCommandProcessor
    {
        public static void ProcessCommand(string text, Character sender)
        {
            Console.WriteLine("Chat command found!");
            
            var args = text.Split(' ');

            switch (args[0])
            {
                case "!pos":
                    Console.WriteLine($"{sender.Position.X} {sender.Position.Y} {sender.Position.Z}");
                    break;
                case "!rot":
                    Console.WriteLine($"{sender.Rotation.Yaw} {sender.Rotation.Pitch} {sender.Rotation.Roll}");
                    break;
                default:
                    Console.WriteLine("Unknown command!");
                    break;
            }
        }
    }
}

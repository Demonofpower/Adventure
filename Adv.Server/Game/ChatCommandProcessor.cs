using System;
using Adv.Server.Master;

namespace Adv.Server.Game
{
    class ChatCommandProcessor
    {
        public static void ProcessCommand(string text, Character sender)
        {
            Console.WriteLine("Chat command found!");

            try
            {
                var args = text.Split(' ');

                switch (args[0])
                {
                    case "!pos":
                        Console.WriteLine($"{sender.Position.X} {sender.Position.Y} {sender.Position.Z}");
                        break;
                    case "!rot":
                        Console.WriteLine($"{sender.Rotation.Yaw} {sender.Rotation.Pitch} {sender.Rotation.Roll}");
                        break;
                    case "!health":
                        sender.Health = Convert.ToInt32(args[1]);
                        Console.WriteLine($"{sender.Name} new health: {sender.Health}");
                        break;
                    case "!mana":
                        sender.Mana = Convert.ToInt32(args[1]);
                        Console.WriteLine($"{sender.Name} new mana: {sender.Mana}");
                        break;
                    default:
                        Console.WriteLine("Unknown command!");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}

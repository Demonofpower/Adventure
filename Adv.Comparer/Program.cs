using System;
using Adv.Sniffer;

namespace Adv.Comparer
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = "5d6f19c7";
            
            var a = HexArithmetic.HexToFloat(s);
            var b = HexArithmetic.FloatToHex(a);





            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine("1: ");
                var one = Console.ReadLine();

                Console.WriteLine("2: ");
                var two = Console.ReadLine();

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();

                var bigger = "";
                var smaller = "";

                if (one.Length >= two.Length)
                {
                    bigger = one;
                    smaller = two;
                }
                else
                {
                    bigger = two;
                    smaller = one;
                }
                
                for (int i = 0; i < smaller.Length; i++)
                {
                    if (bigger[i] == smaller[i])
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(smaller[i]);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("x");
                    }
                }

                for (int i = 0; i < bigger.Length - smaller.Length; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("x");
                }
                

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("---------");
                Console.WriteLine();
            }
        }
    }
}
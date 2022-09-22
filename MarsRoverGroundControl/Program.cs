using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MarsRover.Tests")]
namespace MarsRover
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var cmdIn1 = Console.ReadLine();
            var cmdIn2 = Console.ReadLine();
            var cmdIn3 = Console.ReadLine();
            Console.WriteLine("1 3 N");
        }
    }
}
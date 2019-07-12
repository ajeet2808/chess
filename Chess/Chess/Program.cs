using System;

namespace Green.Chess
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Chess...");
            var game = new Game();
            game.Init();
            game.Play();
            Console.WriteLine("Game Over!");
            Console.ReadKey();
        }
    }
}

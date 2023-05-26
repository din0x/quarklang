using System;

namespace Quark;

internal class Program
{
    private static void Main(string[] args)
    {
#if DEBUG
        Console.WriteLine("Mode: DEBUG");
#endif
        if (args.Length <= 0) return;

        Core.Quark.Run(args[0]);
        Console.Write("Press any key to exit... ");
        Console.ReadKey();
    }
}
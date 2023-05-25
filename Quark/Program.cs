using System;

namespace Quark;

internal class Program
{
    private static void Main(string[] args)
    {
#if RELEASE
        Console.WriteLine("Mode: RELEASE");
#endif
        if (args.Length <= 0) return;

        Core.Quark.Run(args[0]);
        Console.Write("Press any key to exit... ");
        Console.ReadKey();
    }
}
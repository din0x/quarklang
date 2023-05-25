using System;

namespace Quark;

internal class Program
{
    private static void Main(string[] args)
    {
#if RELEASE
        Console.WriteLine("Mode: RELEASE");
#endif
        if (args.Length <= 0)
        { 
            Core.Quark.RunTerminal();
            return;
        }

        Core.Quark.Run(args[0]);
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
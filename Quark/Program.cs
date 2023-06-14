using System;

namespace QuarkLang;

internal class Program
{
    private static void Main(string[] args)
    {
#if DEBUG
        Console.WriteLine("Mode: DEBUG");
#endif
        if (args.Length <= 0) return;

        Core.Quark.Run(args[0]);
    }
}
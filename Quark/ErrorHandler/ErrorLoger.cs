using System;

namespace Quark.ErrorHandler;

public class ErrorLoger
{
    public static void Log(Error error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(error.ToString());
        Console.ForegroundColor = ConsoleColor.Gray;
    }
}
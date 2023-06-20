using System;

namespace QuarkLang.Core.ErrorHandler;

public class ErrorLogger
{
    public static void Log(Error error)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(error.ToString());
        Console.ForegroundColor = ConsoleColor.Gray;
    }
}
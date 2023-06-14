using Newtonsoft.Json;
using QuarkLang.Core.Lexer;
using QuarkLang.Core.Parser.AST;
using System;
using System.IO;

namespace QuarkLang.Core;

public static class Quark
{
    public static void Run(string path)
    {
        var start1 = DateTime.Now;

        if (!File.Exists(path))
        {
            Console.WriteLine($"File '{path}' does not exist"); 
            return;
        }
        string code = File.ReadAllText(path);

        var lexer = new Lexer.Lexer(code, path);
        //Console.WriteLine(TokensToString(lexer.Tokens));

        var parser = new Parser.Parser(lexer.Tokens, path);
        Console.WriteLine(ASTToJson(parser.Program));

        if (parser.ErrorEncountered) return;
        var start2 = DateTime.Now;

        Console.ReadLine();

        var interpreter = new Interpreter.Interpreter(parser.Program, path);

        var runtime = Math.Round((DateTime.Now - start2).TotalSeconds, 3).ToString().Replace(',', '.');
        var total = Math.Round((DateTime.Now - start1).TotalSeconds, 3).ToString().Replace(',', '.');
        var result = interpreter.ExitCode;
        Console.WriteLine($"Finshed with code {result} in {runtime}s ({nameof(total)}: {total}s)");
        Console.Write("Press any key to exit... ");
        Console.ReadKey();
    }

    public static string ASTToJson(Statement stmt)
    {
        JsonSerializerSettings settings = new()
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Objects,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
        };
        return JsonConvert.SerializeObject(stmt, settings);
    }

    public static string TokensToString(Token[] tokens)
    {
        var str = "";
        foreach (var token in tokens)
        {
            str += token.ToString() + "\n";
        }
        return str;
    }
}
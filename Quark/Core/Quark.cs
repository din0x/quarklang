using Newtonsoft.Json;
using Quark.Core.Lexer;
using Quark.Core.Parser.AST;
using System;
using System.IO;

namespace Quark.Core;

public static class Quark
{
    public static void Run(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine($"File '{path}' does not exist"); 
            return; 
        }
        string code = File.ReadAllText(path);



        var lexer = new Lexer.Lexer(code, path);
        var parser = new Parser.Parser(lexer.Tokens, path);
        //Console.WriteLine(TokensToString(lexer.Tokens));
        //Console.WriteLine(ASTToJson(parser.Program));

        if (parser.ErrorEncountered) return;
        var start = DateTime.Now;
        var interpreter = new Interpreter.Interpreter(parser.Program, path);
        var runtime = DateTime.Now - start;
        var result = interpreter.ExitCode;
        Console.WriteLine($"Finshed with code {result} in {Math.Round(runtime.TotalSeconds, 2).ToString().Replace(',','.')}s");
    }

    public static void RunTerminal()
    {
        return;

        //bool run = true;
        //var env = new Interpreter.Environment();
        //while (run)
        //{
        //    Console.Write("> ");
        //    string code = Console.ReadLine() ?? "";
        //    if (code == "") continue;
        //    var lexer = new Lexer.Lexer(code, "console");
        //    var parser = new Parser.Parser(lexer.Tokens, "console");
        //    Console.WriteLine(ASTToJson(parser.Program));
        //    Console.WriteLine(TokensToString(lexer.Tokens));
        //    if (!parser.ErrorEncountered)
        //    {
        //        var interpreter = new Interpreter.Interpreter(parser.Program, "console", env);
        //        if (interpreter.ExitCode == 0)
        //            Console.WriteLine("< " + interpreter.LastEvaluated);
        //    }
        //}
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

    private static string TokensToString(Token[] tokens)
    {
        var str = "";
        foreach (var token in tokens)
        {
            str += token.ToString() + "\n";
        }
        return str;
    }
}
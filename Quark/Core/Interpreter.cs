using Newtonsoft.Json;
using QuarkLang.Core.AST;
using QuarkLang.Core.CompileTime;
using QuarkLang.Core.CompileTime.Utils;
using System;

namespace QuarkLang.Core;

public class Interpreter
{
    public static void Run(string sourceCode)
    {
        var tokens = Lexer.Tokenize(sourceCode);
        Console.WriteLine(TokensToString(tokens));

        var program = Parser.Parse(tokens);
        Console.WriteLine(ProgramToJson(program));

        program.Evaluate();
    }

    private static string ProgramToJson(Unit program)
    {
        return JsonConvert.SerializeObject(program, new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
        });
    }

    private static string TokensToString(Token[] tokens)
    {
        var result = "";

        foreach (var token in tokens)
        {
            result += token + "\n";
        }
        return result;
    }
}
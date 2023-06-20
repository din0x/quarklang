using QuarkLang.Core;
using QuarkLang.ErrorHandler;
using System;
using System.IO;
using Newtonsoft.Json;

namespace QuarkLang;

internal class Program
{
	private static void Main(string[] args)
	{
		//if (args.Length <= 0) 
		//	return;

		var file = """C:\Users\super\OneDrive\Desktop\test.q""";
		var tokens = Lexer.Tokenize(File.ReadAllText(file));
        foreach (var token in tokens) 
		{
            Console.WriteLine(token);
        }
        var program = Parser.Parse(tokens);

        Console.WriteLine(JsonConvert.SerializeObject(program, new JsonSerializerSettings() 
		{ 
			Formatting = Formatting.Indented,
		}));

        Interpreter.Run(program);
	}
}
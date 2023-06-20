using System.IO;
using QuarkLang.Core;

namespace QuarkLang;

internal class Program
{
	private static void Main(string[] args)
	{
		//if (args.Length <= 0) 
		//	return;

		var file = """C:\Users\super\OneDrive\Desktop\test.q""";

        Interpreter.Run(File.ReadAllText(file));
	}
}
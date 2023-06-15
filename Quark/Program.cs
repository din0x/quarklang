using System;

namespace QuarkLang;

internal class Program
{
	private static void Main(string[] args)
	{
		if (args.Length <= 0) 
			return;

		if (args[0] == "help" && args[0] == "-h")
			DisplayHelpMessage();

		Core.Quark.Run(args[0]);
	}

	private static void DisplayHelpMessage()
	{
		var help = """
		
		""";

		Console.WriteLine(help);
	}
}
using QuarkLang.AST;

namespace QuarkLang.Core;

public class Interpreter
{
    public static void Run(Unit program)
    {
        program.Evaluate();
    }
}
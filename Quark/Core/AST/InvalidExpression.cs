using QuarkLang.Core.Runtime;

namespace QuarkLang.AST;

public class InvalidExpression : Expression
{
    public override RuntimeValue Evaluate(Environment env)
    {
        System.Console.WriteLine("error");
        throw new System.NotImplementedException();
    }
}
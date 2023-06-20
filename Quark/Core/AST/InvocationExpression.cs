using QuarkLang.Core.Runtime;

namespace QuarkLang.Core.AST;

public class InvocationExpression : Expression
{
    public Expression _expr;
    public Expression[] _args;

    public InvocationExpression(Expression expr, Expression[] args)
    {
        _expr = expr;
        _args = args;
    }

    public override RuntimeValue Evaluate(Environment env)
    {
        throw new System.NotImplementedException();
    }
}
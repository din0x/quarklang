using QuarkLang.Core.Runtime;

namespace QuarkLang.AST;

public class ExpressionStatement : Statement
{
    public Expression _expr;

    public ExpressionStatement(Expression expr)
    {
        _expr = expr;
    }

    public override RuntimeValue? Evaluate(Environment env)
    {
        var result = _expr.Evaluate(env);
        return result is ExceptionValue e ? e : null;
    }
}
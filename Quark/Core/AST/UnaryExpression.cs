using QuarkLang.AST.Utils;
using QuarkLang.Core.Runtime;

namespace QuarkLang.AST;

public class UnaryExpression : Expression
{
    public UnaryOperator _operator;
    public Expression _expr;

    public UnaryExpression(UnaryOperator @operator, Expression expr)
    {
        _operator = @operator;
        _expr = expr;
    }

    public override RuntimeValue Evaluate(Environment env)
    {
        var result = _expr.Evaluate(env);

        if (result is ExceptionValue exception)
            return new ExceptionValue(exception);

        if (_operator == UnaryOperator.Not)
            return result.Not();
        else if (_operator == UnaryOperator.Plus)
            return result.Plus();
        else
            return result.Minus();
    }
}
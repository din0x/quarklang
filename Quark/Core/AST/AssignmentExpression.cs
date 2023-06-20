using QuarkLang.Core.Runtime;

namespace QuarkLang.Core.AST;

public class AssignmentExpression : Expression
{
    public Expression _expr;
    public Expression _value;

    public AssignmentExpression(Expression expr, Expression value)
    {
        _expr = expr;
        _value = value;
    }

    public override RuntimeValue Evaluate(Environment env)
    {
        var result = _expr.Evaluate(env);
        if (result is ExceptionValue exception1)
            return new ExceptionValue(exception1);

        var value = _value.Evaluate(env);
        if (value is ExceptionValue exception2)
            return new ExceptionValue(exception2);

        return result._ptr.AssignVariable(result._name, value);
    }
}
using QuarkLang.AST.Utils;
using QuarkLang.Core.Runtime;

namespace QuarkLang.AST;

public class BinaryExpression : Expression
{
    public BinaryOperator _operator;
    public Expression _left;
    public Expression _right;

    public BinaryExpression(BinaryOperator @operator, Expression left, Expression right)
    {
        _operator = @operator;
        _left = left;
        _right = right;
    }

    public override RuntimeValue Evaluate(Environment env)
    {
        var left = _left.Evaluate(env);
        if (left is ExceptionValue exception1)
            return new ExceptionValue(exception1);

        var right = _right.Evaluate(env);
        if (right is ExceptionValue exception2)
            return new ExceptionValue(exception2);

        if (_operator == BinaryOperator.Add)
            return left.Add(right);
        else if (_operator == BinaryOperator.Subtract)
            return left.Subtract(right);
        else if (_operator == BinaryOperator.Multiply)
            return left.Multiply(right);
        else if (_operator == BinaryOperator.Divide)
            return left.Divide(right);
        else if (_operator == BinaryOperator.Modulo)
            return left.Modulo(right);
        else if (_operator == BinaryOperator.Equal) 
            return left.Equal(right);
        else if (_operator == BinaryOperator.NotEqual) 
            return left.NotEqual(right);
        else if (_operator == BinaryOperator.More)
            return left.Greater(right);
        else if (_operator == BinaryOperator.Less)
            return left.Less(right);
        else if (_operator == BinaryOperator.MoreOrEqual)
            return left.GreaterOrEqual(right);
        else
            return left.LessOrEqual(right);
    }
}
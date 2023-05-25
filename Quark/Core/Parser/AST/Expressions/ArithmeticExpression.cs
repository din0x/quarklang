namespace Quark.Core.Parser.AST;

public class ArithmeticExpression : Expression
{
    public Expression left;
    public Expression right;
    public string op;

    public ArithmeticExpression(Expression left, Expression right, string op)
    {
        this.left = left;
        this.right = right;
        this.op = op;
    }
}

namespace QuarkLang.Core.Parser.AST;

public class RelationalExpression : Expression
{
    public Expression left;
    public Expression right;
    public string op;

    public RelationalExpression(Expression left, Expression right, string op)
    {
        this.left = left;
        this.right = right;
        this.op = op;
    }
}

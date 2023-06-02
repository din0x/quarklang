namespace QuarkLang.Core.Parser.AST;

public class LogicalExpression : Expression
{
    public enum Type
    {
        And,
        Or
    }

    public Expression left;
    public Expression right;
    public Type type;

    public LogicalExpression(Expression left, Expression right, Type type)
    {
        this.left = left;
        this.right = right;
        this.type = type;
    }
}
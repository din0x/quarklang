namespace QuarkLang.Core.Parser.AST;

public class UnaryExpression : Expression
{
    public Expression value;
    public string op;

    public UnaryExpression(Expression value, string op)
    {
        this.value = value;
        this.op = op;
    }
}

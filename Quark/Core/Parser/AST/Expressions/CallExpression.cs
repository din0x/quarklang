namespace QuarkLang.Core.Parser.AST;

public class CallExpression : Expression
{
    public Expression expr;
    public Expression[] args;
    public bool useAsStmt;

    public CallExpression(Expression expr, Expression[] args, bool useAsStmt)
    {
        this.expr = expr;
        this.args = args;
        this.useAsStmt = useAsStmt;
    }
}
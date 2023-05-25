namespace Quark.Core.Parser.AST;

public class ReturnStatement : Statement
{
    public Expression? expr;

    public ReturnStatement(Expression? expr)
    {
        this.expr = expr;
    }
}
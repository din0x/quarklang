namespace Quark.Core.Parser.AST;

public class WhileLoop : Statement
{
    public Expression condition;
    public Statement[] body;

    public WhileLoop(Expression condition, Statement[] body)
    {
        this.condition = condition;
        this.body = body;
    }
}
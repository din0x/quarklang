namespace Quark.Core.Parser.AST;

public class VariableAssignment : Statement
{
    public Expression variable;
    public Expression value;

    public VariableAssignment(Expression variable, Expression expr)
    {
        this.variable = variable;
        value = expr;
    }
}
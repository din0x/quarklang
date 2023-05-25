namespace Quark.Core.Parser.AST;

public class VariableDeclaration : Statement
{
    public bool constant;
    public string name;
    public Expression value;

    public VariableDeclaration(string name, Expression expr, bool constant)
    {
        this.name = name;
        this.constant = constant;
        value = expr;
    }
}
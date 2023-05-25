namespace Quark.Core.Parser.AST;

public class BooleanLiteral : Expression
{
    public bool value;

    public BooleanLiteral(bool value)
    {
        this.value = value;
    }
}

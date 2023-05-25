namespace Quark.Core.Parser.AST;

public class StringLiteral : Expression
{
    public string value;

    public StringLiteral(string value)
    {
        this.value = value;
    }
}
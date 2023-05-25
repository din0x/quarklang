namespace Quark.Core.Parser.AST;

public class Identifier : Expression
{
    public string symbol;

    public Identifier(string symbol)
    {
        this.symbol = symbol;
    }
}
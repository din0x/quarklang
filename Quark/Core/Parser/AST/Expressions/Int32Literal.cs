namespace Quark.Core.Parser.AST;

public class Int32Literal : Expression
{
    public int value;

    public Int32Literal(int value)
    {
        this.value = value;
    }
}
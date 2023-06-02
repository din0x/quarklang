namespace QuarkLang.Core.Parser.AST;

public class Float32Literal : Expression
{
    public float value;

    public Float32Literal(float value)
    {
        this.value = value;
    }
}
using QuarkLang.Core.Runtime;

namespace QuarkLang.AST;

public class Float32Literal : Expression
{
    public float _value;

    public Float32Literal(float value)
    {
        _value = value;
    }

    public override RuntimeValue Evaluate(Environment env)
    {
        return new Float32Value(_value);
    }
}
using QuarkLang.Core;
using QuarkLang.Core.Runtime;

namespace QuarkLang.Core.AST;

public class Int32Literal : Expression
{
    public int _value;

    public Int32Literal(int value)
    {
        _value = value;
    }

    public override RuntimeValue Evaluate(Environment env)
    {
        return new Int32Value(_value);
    }
}
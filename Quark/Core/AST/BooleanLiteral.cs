using QuarkLang.Core.Runtime;

namespace QuarkLang.AST;

public class BooleanLiteral : Expression
{
    public bool _value;
    
    public BooleanLiteral(bool value)
    {
        _value = value;
    }

    public override RuntimeValue Evaluate(Environment env)
    {
        return new BooleanValue(_value);
    }
}
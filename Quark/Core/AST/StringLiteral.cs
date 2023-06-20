using QuarkLang.Core.Runtime;

namespace QuarkLang.Core.AST;

public class StringLiteral : Expression
{
    public string _value;

    public StringLiteral(string value)
    {
        _value = value;
    }

    public override RuntimeValue Evaluate(Environment env)
    {
        return new StringValue(_value);
    }
}
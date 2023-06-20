using QuarkLang.Core.Runtime;

namespace QuarkLang.Core.AST;

public class VariableDeclaration : Statement
{
    public bool _constant;
    public string _name;
    public Expression? _type;
    public Expression _value;

    public VariableDeclaration(string name, Expression? type, Expression value)
    {
        _name = name;
        _type = type;
        _value = value;
    }

    public override RuntimeValue? Evaluate(Environment env)
    {
        return env.DeclareVariable(_name, _value.Evaluate(env), _constant);
    }
}
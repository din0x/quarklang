using QuarkLang.Core.Runtime;

namespace QuarkLang.Core.AST;

public class MemberAccessExpression : Expression
{
    public Expression _expr;
    public string _member;

    public MemberAccessExpression(Expression expr, string member)
    {
        _expr = expr;
        _member = member;
    }

    public override RuntimeValue Evaluate(Environment env)
    {
        throw new System.NotImplementedException();
    }
}
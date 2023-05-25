namespace Quark.Core.Parser.AST;

public class MemberExpression : Expression
{
    public Expression parent;
    public string member;

    public MemberExpression(Expression parent, string member)
    {
        this.parent = parent;
        this.member = member;
    }
}
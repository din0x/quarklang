namespace QuarkLang.Core.Parser.AST;

public class TypeExpression
{
    public string type;
    public TypeExpression? parent;

    public TypeExpression(string type, TypeExpression? parent = null)
    {
        this.type = type;
        this.parent = parent;
    }
}
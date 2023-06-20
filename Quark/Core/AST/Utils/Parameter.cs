namespace QuarkLang.AST.Utils;

public class Parameter
{
    public string _name;
    public Expression _type;

    public Parameter(string name, Expression type)
    {
        _name = name;
        _type = type;
    }
}
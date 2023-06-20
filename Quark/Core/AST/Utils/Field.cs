namespace QuarkLang.AST.Utils;

public class Field
{
    public string _name;
    public Expression _type;
    public Expression? _defaultvalue;

    public Field(string name, Expression type, Expression? defaultValue)
    {
        _name = name;
        _type = type;
        _defaultvalue = defaultValue;
    }
}
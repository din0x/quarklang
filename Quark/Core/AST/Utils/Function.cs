namespace QuarkLang.Core.AST.Utils;

public class Function
{
    public string _name;
    public Parameter[] _params;
    public Statement[] _body;
    public Expression _returnType;

    public Function(string name, Parameter[] @params, Statement[] body, Expression returnType)
    {
        _name = name;
        _params = @params;
        _body = body;
        _returnType = returnType;
    }
}
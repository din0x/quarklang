using QuarkLang.AST;
using QuarkLang.AST.Utils;

namespace QuarkLang.Core.Runtime;

public class FunctionValue : RuntimeValue
{
    public readonly TypeValue _returnType;
    public readonly Parameter[] _params;
    public readonly Statement[] _body;

    public FunctionValue(TypeValue returnType, Parameter[] parameters, Statement[] body)
    {
        _returnType = returnType;
        _params = parameters;
        _body = body;
    }

    public override TypeValue Type()
    {
        return new TypeValue("function");
    }
}
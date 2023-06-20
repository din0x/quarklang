using QuarkLang.AST.Utils;
using QuarkLang.Core.Runtime;

namespace QuarkLang.AST;

public class FunctionDeclaration : Statement
{
    public Function _function;

    public FunctionDeclaration(Function function)
    {
        _function = function;
    }

    public override RuntimeValue? Evaluate(Environment env)
    {
        var name = _function._name;
        var paramethers = new Core.Runtime.Parameter[_function._params.Length];
        var type = _function._returnType.Evaluate(env);

        if (type is not TypeValue returnType)
            return new ExceptionValue($"{type} is not a type");

        var result = env.DeclareVariable(
            name, 
            new FunctionValue(returnType, paramethers, _function._body), 
            true);


        if (result is ExceptionValue exception)
            return exception;

        return null;
    }
}
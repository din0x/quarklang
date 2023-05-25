using Quark.Core.Parser.AST;

namespace Quark.Core.Interpreter.DataTypes;

public class Function : RuntimeValue
{
    public new string name;
    public Statement[] body;
    public FunctionDeclaration.Argument[] args;
    public TypeValue returnType;

    public Function(string name, Statement[] body, FunctionDeclaration.Argument[] args, TypeValue returnType)
    {
        this.name = name;
        this.body = body;
        this.args = args;
        this.returnType = returnType;
    }
}
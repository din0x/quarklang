using Quark.Core.Interpreter.DataTypes;

namespace Quark.Core.Parser.AST;

public class FunctionDeclaration : Statement
{
    public class Argument 
    {
        public string name;
        public TypeValue dataType;

        public Argument(string name, TypeValue dataType)
        {
            this.name = name;
            this.dataType = dataType;
        }
    }

    public string name;
    public TypeValue returnType;
    public Argument[] args;
    public Statement[] body;

    public FunctionDeclaration(string name, Argument[] args, TypeValue returnType, Statement[] body)
    {
        this.name = name;
        this.args = args;
        this.returnType = returnType;
        this.body = body;
    }
}
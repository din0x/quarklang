using System.Diagnostics;

namespace QuarkLang.Core.Parser.AST;

public class ClassDeclaration : Statement
{
    public class Field
    {
        public string name;
        public Expression type;
        public Expression? defualtValue;

        public Field(string name, Expression type, Expression? defualtValue = null)
        {
            this.name = name;
            this.type = type;
            this.defualtValue = defualtValue;
        }
    }

    public class Method
    {
        public string name;
        public Expression returnType;
        public FunctionDeclaration.Argument[] arguments;
        public Statement[] body;

        public Method(string name, Expression returnType, FunctionDeclaration.Argument[] arguments, Statement[] body)
        {
            this.name= name;
            this.returnType = returnType;
            this.arguments = arguments;
            this.body = body;
        }
    }

    public Field[] fields;
    public Method[] methods;

    public ClassDeclaration(Field[] fields, Method[] methods)
    {
        this.fields = fields;
        this.methods = methods;
    }
}
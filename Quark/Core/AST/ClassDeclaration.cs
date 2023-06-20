using QuarkLang.AST.Utils;
using QuarkLang.Core.Runtime;

namespace QuarkLang.AST;

public class ClassDeclaration : Statement
{
    public string _name;
    public Field[] _fields;
    public Function[] _methods;

    public ClassDeclaration(string name, Field[] fields, Function[] methods)
    {
        _name = name;
        _fields = fields;
        _methods = methods;
    }

    public override RuntimeValue? Evaluate(Environment env)
    {
        throw new System.NotImplementedException();
    }
}
using QuarkLang.Core.Runtime;

namespace QuarkLang.AST;

public abstract class Expression
{
    public abstract RuntimeValue Evaluate(Environment env);
}
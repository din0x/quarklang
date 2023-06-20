using QuarkLang.Core.Runtime;

namespace QuarkLang.Core.AST;

public abstract class Expression
{
    public abstract RuntimeValue Evaluate(Environment env);
}
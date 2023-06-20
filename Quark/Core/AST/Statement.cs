using QuarkLang.Core.Runtime;

namespace QuarkLang.AST;

public abstract class Statement
{
    public abstract RuntimeValue? Evaluate(Environment env);
}
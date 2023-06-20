using QuarkLang.Core.Runtime;

namespace QuarkLang.Core.AST;

public abstract class Statement
{
    public abstract RuntimeValue? Evaluate(Environment env);
}
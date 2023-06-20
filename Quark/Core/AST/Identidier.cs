using QuarkLang.Core.Runtime;

namespace QuarkLang.AST;

public class Identidier : Expression
{
    public string _symbol;

    public Identidier(string symbol)
    {
        _symbol = symbol;
    }

    public override RuntimeValue Evaluate(Environment env)
    {
        return env.LookupVariable(_symbol);
    }
}
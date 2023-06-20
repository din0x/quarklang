using QuarkLang.Core.Runtime;

namespace QuarkLang.Core.AST;

public class Unit
{
    public Unit(Statement[] body)
    {
        _body = body;
    }

    public RuntimeValue? Evaluate()
    {
        Environment env = new();

        RuntimeValue? lastEvaluated = null;
        for (int i = 0; i < _body.Length; i++)
        {
            lastEvaluated = _body[i].Evaluate(env);
        }
        return lastEvaluated;
    }

    public readonly Statement[] _body;
}
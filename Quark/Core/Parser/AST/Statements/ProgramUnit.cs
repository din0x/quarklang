using System.Collections.Generic;

namespace Quark.Core.Parser.AST;

public class ProgramUnit : Statement
{
    public List<Statement> body;

    public ProgramUnit()
    {
        body = new();
    }
}
using System.Collections.Generic;

namespace Quark.Core.Parser.AST;

public class ConditionalStatement : Statement
{
    public Expression[] conditions;
    public List<List<Statement>> blocks;

    public Statement[]? falseBlock;

    public ConditionalStatement(Expression[] conditions, List<List<Statement>> blocks, Statement[]? falseBlock)
    {
        this.conditions = conditions;
        this.blocks = blocks;
        this.falseBlock = falseBlock;
    }
}
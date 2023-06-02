namespace QuarkLang.Core.Parser.AST;

public class ImportStatement : Statement
{
    public string module;

    public ImportStatement(string module)
    {
        this.module = module;
    }
}
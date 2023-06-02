using System.IO;
using System.Linq;

namespace QuarkLang.ErrorHandler;

public class ParserError : Error
{
    private readonly int line;
    private readonly int column;
    private readonly string file;

    public ParserError(string error, int line, int column, string file) : base(error)
    {
        base.error = error;
        this.line = line;
        this.column = column;
        this.file = file;
    }

    public override string ToString()
    {
        if (file == "console")
        {
            return $"Unexpected token {error}";
        }

        string[] lines = File.ReadAllText(file).Split('\n');

        string codePart = "";

        try { codePart = lines[line - 1]; } catch { }

        string str = $"""
                     From {file} at line {line}
                        {codePart}
                        {string.Concat(Enumerable.Repeat("-", column - 1))}^
                        {error}
                     """;
        return str;
    }
}